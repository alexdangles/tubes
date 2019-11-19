using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Helper
{
    /// <summary>
    /// Class for serial and network communication between hardware.
    /// </summary>
    public class Link : IDisposable
    {
        #region Fields


        /// <summary>
        /// Responder delegate which handles msg recieved events.
        /// </summary>
        /// <param name="msgRecieved">Message recieved.</param>
        /// <returns></returns>
        public delegate string Responder(string msgRecieved);
        /// <summary>
        /// Custom reply event for each msg recieved.
        /// </summary>
        public event Responder reply;

        TcpListener server = null;
        SerialPort ser;
        Socket soc;
        Thread sendReceive;
        Queue inQ = new Queue();
        public Queue outQ = new Queue();

        /// <summary>
        /// Character to split a custom tag and message at recieving end of device. (message recieved = tag + flag + msg)
        /// </summary>
        public char flag = '~';

        #region Unicode Definitions


        /// <summary>
        /// Null.
        /// </summary>
        public static char NUL = '\u0000';
        public static char SOH = '\u0001';
        /// <summary>
        /// Start of transmission.
        /// </summary>
        public static char STX = '\u0002';
        /// <summary>
        /// End of transmission.
        /// </summary>
        public static char ETX = '\u0003';
        public static char EOT = '\u0004';
        public static char ENQ = '\u0005';
        public static char ACK = '\u0006';
        public static char BEL = '\u0007';
        public static char BS = '\u0008';
        public static char HT = '\u0009';
        /// <summary>
        /// Line feed.
        /// </summary>
        public static char LF = '\u000A';
        public static char VT = '\u000B';
        public static char FF = '\u000C';
        /// <summary>
        /// Carriage return.
        /// </summary>
        public static char CR = '\u000D';
        public static char SO = '\u000E';
        public static char SI = '\u000F';
        public static char DLE = '\u0010';
        public static char DC1 = '\u0011';
        public static char DC2 = '\u0012';
        public static char DC3 = '\u0013';
        public static char DC4 = '\u0014';
        public static char NAK = '\u0015';
        public static char SYN = '\u0016';
        public static char ETB = '\u0017';
        public static char CAN = '\u0018';
        public static char EM = '\u0019';
        public static char SUB = '\u001A';
        public static char ESC = '\u001B';
        public static char FS = '\u001C';
        public static char GS = '\u001D';
        public static char RS = '\u001E';
        public static char US = '\u001F';
        public static char DEL = '\u007F';

        public static char[] allChars =
        {
            NUL,SOH,STX,ETX,EOT,ENQ,ACK,BEL,BS,HT,LF,VT,FF,CR,SO,SI,DLE,DC1,DC2,DC3,DC4,NAK,SYN,ETB,CAN,EM,SUB,ESC,FS,GS,RS,US,DEL
        };

        #endregion

        /// <summary>
        /// Start or stop send/receive thread of device.
        /// </summary>
        public bool go = true;

        /// <summary>
        /// Time to wait between serial port send and recieve commands.
        /// </summary>
        public int msToWait = 50;

        /// <summary>
        /// Leave port open on disconnect.
        /// </summary>
        public bool portStayOpen;

        #endregion

        #region Properties


        /// <summary>
        /// Connection port.
        /// </summary>
        public int port { get; private set; }

        /// <summary>
        /// Connection IP Address.
        /// </summary>
        public string ipAddress { get; private set; }

        /// <summary>
        /// Get connection state of device.
        /// </summary>
        public bool portOpen
        {
            get
            {
                if (ser != null)
                {
                    return ser.IsOpen;
                }
                else if (soc != null)
                {
                    return soc.Connected;
                }
                return false;
            }
        }

        /// <summary>
        /// Indicates if data from device is available to be processed.
        /// </summary>
        public bool dataAvailable
        {
            get
            {
                return outQ.Count > 0;
            }
        }

        /// <summary>
        /// Host or IP to relay last message.
        /// </summary>
        public string from { get; private set; }

        #endregion

        #region Constructors


        /// <summary>
        /// Initialize serial communication.
        /// </summary>
        /// <param name="serialPort">Serial port configuration of device.</param>
        public Link(SerialPort serialPort)
        {
            ser = serialPort;
            try
            {
                if (portOpen) ser.Close();
                ser.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (portOpen)
            {
                sendReceive = new Thread(SendReceiveQ);
                sendReceive.Start();
            }
            else Dispose();

        }

        /// <summary>
        /// Initialize socket communication to specific host or IP address.
        /// </summary>
        /// <param name="port">Port to open.</param>
        /// <param name="ipAddress">Host name or IP address.</param>
        public Link(int port, string ipAddress)
        {
            try
            {
                this.ipAddress = ipAddress;
                this.port = port;
                soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (soc.Connected) soc.Disconnect(true);
                soc.SendTimeout = soc.ReceiveTimeout = 1000;
                soc.ReceiveBufferSize = 8192;
                soc.Connect(ipAddress, port);

                if (portOpen)
                {
                    sendReceive = new Thread(SendReceiveQ);
                    sendReceive.Start();
                }
                else Dispose();
            }
            catch (FormatException)
            {
                Console.WriteLine("Not a valid IP address");
            }
            catch (SocketException se)
            {
                Console.WriteLine("Socket Exception : {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.Message);
            }
        }

        /// <summary>
        /// Listen for incoming requests on specific port from all hosts in network domain.
        /// </summary>
        /// <param name="port">Port to listen.</param>
        public Link(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            Thread tcpListener = new Thread(Listen);
            tcpListener.Start();
        }

        #endregion

        #region Methods


        /// <summary>
        /// Auto-connect to devices from hardware settings file.
        /// </summary>
        /// <param name="fileName">Optional: File in settings folder to use. (use null or "" to take first file in folder). </param>
        /// <param name="deviceID">Device name in settings file. (Example: Power Supply).</param>
        /// <returns></returns>
        public static string[] ConnectFromFile(string fileName, params string[] deviceID)
        {
            string sysDir = Path.GetPathRoot(Environment.SystemDirectory);
            string[] files = Directory.GetFiles($"{ sysDir }Hardware");
            string fName = $"{ sysDir }Hardware\\{ fileName }.txt";
            List<string> names = new List<string>();

            if (files.Length > 0 && File.Exists(fName))
                files[0] = fName;

            if (files.Length > 0 && File.Exists(files[0]))
            {
                string[] settings = File.ReadAllLines(files[0]);

                if (settings != null && settings.Length > 0)
                {
                    for (int i = 0; i < deviceID.Length; i++)
                    {
                        bool found = false;
                        for (int j = 0; j < settings.Length; j++)
                        {
                            if (settings[j] == deviceID[i])
                            {
                                names.Add(settings[j].Split('=')[1].Trim());
                                found = true;
                                break;
                            }
                        }
                        if (!found) names.Add(null);
                    }
                }
            }
            if (names.Count == 0) names.Add(null);
            return names.ToArray();
        }

        /// <summary>
        /// Find COM port of connected device from an identifier key which the device returns.
        /// </summary>
        /// <param name="serialPort">Serial port setup for device.</param>
        /// <param name="contains">String to search for in device return data.</param>
        /// <param name="queryIDs">Commands to send which return unique id of device.</param>
        /// <returns></returns>
        public static string FindCOMPort(SerialPort serialPort, string contains, params string[] queryIDs)
        {
            string[] ports = GetPortNames();
            bool found = false;
            foreach (string p in ports)
            {
                SerialPort s = new SerialPort(p, serialPort.BaudRate, serialPort.Parity, serialPort.DataBits, serialPort.StopBits);
                Console.WriteLine($"Trying { p }...");

                try
                {
                    s.Open();
                }
                catch
                {
                    Console.WriteLine("busy");
                }

                if (s.IsOpen)
                {
                    foreach (string q in queryIDs)
                    {
                        byte[] send = Encoding.ASCII.GetBytes(q);
                        byte[] rec = new byte[1024];
                        string queryRes = "";
                        try
                        {
                            s.Write(send, 0, send.Length);
                            Thread.Sleep(200);
                            rec = new byte[s.BytesToRead];
                            s.Read(rec, 0, rec.Length);
                            queryRes = Encoding.ASCII.GetString(rec, 0, rec.Length);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        if (queryRes.Contains(contains))
                        {
                            found = true;
                            Console.WriteLine("That's the one!");
                            break;
                        }
                    }
                    s.Close();
                }
                if (found)
                {
                    return p;
                }
            }
            return serialPort.PortName;
        }

        /// <summary>
        /// Get IP address of device on network.
        /// </summary>
        /// <param name="address">IP address of Udp broadcast.</param>
        /// <param name="port">Port of Udp broadcast.</param>
        /// <param name="contains">Device identifier name.</param>
        /// <param name="cancel">Cancel the operation.</param>
        /// <returns></returns>
        public static string[] GetDeviceIP(string address, int port, string contains, CancellationToken cancel = default(CancellationToken))
        {
            List<string> list = new List<string>();
            UdpClient udpSocket = new UdpClient(0);

            try
            {
                IPAddress ip = IPAddress.Parse(address);
                IPEndPoint ep = new IPEndPoint(ip, port);

                Stopwatch vSW = new Stopwatch();
                vSW.Start();

                byte[] send = Encoding.UTF8.GetBytes("IP");
                udpSocket.Send(send, send.Length, ep);

                byte[] rec = new byte[1024];
                IPEndPoint vSource = new IPEndPoint(IPAddress.Any, 0);

                Stopwatch swTimeout = new Stopwatch();
                swTimeout.Start();
                while (swTimeout.ElapsedMilliseconds < 30)
                {
                    if (udpSocket.Available > 0)
                    {
                        rec = udpSocket.Receive(ref vSource);
                        string data = Encoding.Default.GetString(rec).Replace("IP,", "");
                        if (contains != "")
                        {
                            if (data.Contains(contains)) list.Add(data);
                        }
                        else
                        {
                            list.Add(data);
                        }
                    }
                    cancel.ThrowIfCancellationRequested();
                    Thread.Sleep(1);
                }
                udpSocket.Close();
                vSW.Stop();
                Console.WriteLine(vSW.ElapsedMilliseconds);
            }
            catch
            {
                udpSocket.Close();
            }

            return list.ToArray();
        }

        /// <summary>
        /// Thread for sending and receiving to the queue.
        /// </summary>
        private void SendReceiveQ()
        {
            while (go)
            {
                Thread.Sleep(1);

                while (inQ.Count > 0)
                {
                    try
                    {
                        string received = TxRx(inQ.Dequeue().ToString());
                        if (received.Length > 0) outQ.Enqueue(received);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Send/Receive direct command.
        /// </summary>
        /// <param name="message">String to write.</param>
        /// <returns></returns>
        public string TxRx(string message)
        {
            if (portOpen)
            {
                string tag = "";
                string msg = "";
                if (message.Contains(flag.ToString()))
                {
                    tag = message.Split(flag)[0];
                    msg = message.Split(flag)[1];
                }
                else msg = message;

                byte[] send = Encoding.UTF8.GetBytes(msg);
                byte[] rec = new byte[8192];
                if (ser != null)
                {
                    try
                    {
                        ser.Write(send, 0, send.Length);
                        Thread.Sleep(msToWait);
                        rec = new byte[ser.BytesToRead];
                        ser.Read(rec, 0, rec.Length);
                    }
                    catch (TimeoutException te)
                    {
                        Console.WriteLine("TimeoutException : {0}", te.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.Message);
                    }
                }
                else if (soc != null)
                {
                    try
                    {
                        soc.Send(send, SocketFlags.None);
                        soc.Receive(rec, SocketFlags.None);
                    }
                    catch (ArgumentNullException ane)
                    {
                        Console.WriteLine("ArgumentNullException : {0}", ane.Message);
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("SocketException : {0}", se.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.Message);
                    }
                }

                string msgRec = Encoding.ASCII.GetString(rec, 0, rec.Length);

                if (msgRec.Length > 0)
                {
                    msgRec = RemoveJunk(msgRec, NUL, CR, LF, STX, ETX);
                    if (tag == "") return msgRec;
                    else return $"{ tag }{ flag }{ msgRec }";
                }
            }
            return ""; 
        }

        /// <summary>
        /// Write to the device's input queue.
        /// </summary>
        /// <param name="msg">String to write.</param>
        /// <param name="tag">Add custom tag at recieving end of thread.</param>
        /// <returns></returns>
        public void Tx(string msg, string tag = "")
        {
            if (portOpen && sendReceive.IsAlive)
            {
                try
                {
                    if (tag.Length > 0) inQ.Enqueue(tag + flag + msg);
                    else inQ.Enqueue(msg);
                }
                catch { }
            }
        }

        /// <summary>
        /// Get names of ports on computer.
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Removes useless characters from input string.
        /// </summary>
        /// <param name="expression">Input string to clean up.</param>
        /// <param name="junk">Useless characters to remove from input.</param>
        /// <returns></returns>
        public static string RemoveJunk(string expression, params char[] junk)
        {
            foreach (char j in junk)
            {
                expression = Regex.Replace(expression, j.ToString(), string.Empty);
            }
            return expression;
        }

        /// <summary>
        /// Create a server which listens for data packets.
        /// </summary>
        private void Listen()
        {
            try
            {
                // Start listening for client requests.
                server.Start();
                while (go)
                {
                    Socket client = server.AcceptSocket();

                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    int bytesToRead;

                    // Loop to receive all the data sent by the client.
                    while ((bytesToRead = client.Receive(buffer)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        string msgRec = Encoding.ASCII.GetString(buffer, 0, bytesToRead);
                        IPEndPoint ep = (IPEndPoint)client.RemoteEndPoint;
                        from = ep.Address.ToString();
                        Console.WriteLine("Received: {0} from {1}", msgRec, from);
                        string replyMsg = reply == null? msgRec : reply(msgRec);
                        byte[] rBytes = Encoding.ASCII.GetBytes($"{ replyMsg }\r\n");
                        client.Send(rBytes);
                        Console.WriteLine("Sent reply: {0}", replyMsg);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        /// <summary>
        /// Disconnect from device or server.
        /// </summary>
        public void Dispose()
        {
            Thread.Sleep(1000);
            go = false;

            try
            {
                server?.Stop();

                if (portOpen && !portStayOpen)
                {
                    ser?.Close();
                    soc?.Shutdown(SocketShutdown.Both);
                    soc?.Close();
                }
            }
            catch
            {

            }
        }

        #endregion
    }
}