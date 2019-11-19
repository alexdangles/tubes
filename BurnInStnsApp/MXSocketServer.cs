using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BurnInStns
{
    /// <summary>
    /// This class contains the Socket Message (UDP or TCP) for use with the MXSocketServer class
    /// </summary>
    public class MXSocketMessage
    {
        static long nextID = 0;  // Static counter for each new message
        Stopwatch vStopWatch = new Stopwatch();  // Times how long the transaction takes

        /// <summary>
        /// Message constructor that creates the new MXSocketMessage object.  
        /// </summary>
        /// <param name="ipaddress">string containing the IP address of the client</param>
        /// <param name="port">Contains the port that the client connects from</param>
        /// <param name="message">string data received from the client</param>
        /// <returns>None</returns>
        public MXSocketMessage(string ipaddress, int port, string message)
        {
            vStopWatch.Start();
            string[] vMsgArray = message.Split(',');

            ID = Interlocked.Increment(ref nextID);
            IPAddress = ipaddress;
            Port = port;
            Raw = message;
            Command = vMsgArray[0].ToUpper();
            if (vMsgArray.Length > 1) Data = vMsgArray.Skip(1).ToArray();
            else Data = new string[0];
        }

        /// <summary>
        /// Message ID of the object, each new instance with auto generate a new sequential ID (Read Only)
        /// </summary>
        public long ID { get; }

        /// <summary>
        /// Elapsed Milliseconds since the object was created (Read Only)
        /// </summary>
        public long ElapsedMilliseconds { get { return vStopWatch.ElapsedMilliseconds; } }

        /// <summary>
        /// Contains the raw string message that was used in the constructor (Read Only)
        /// </summary>
        public string Raw { get; }

        /// <summary>
        /// The command extracted from the Raw string used in the constructor (Read Only)
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// The data extracted from the Raw string used in the constructor (Read Only)
        /// </summary>
        public string[] Data { get; }

        /// <summary>
        /// The IP Address of the client from which the message was received (Read Only)
        /// </summary>
        public string IPAddress { get; }

        /// <summary>
        /// The Port of the client from which the message was received (Read Only)
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The responce that will be sent back to the client (Read/Write)
        /// </summary>
        public string Responce { get; set; } = "";
    }

    /// <summary>
    /// This class contains the event Args for the OnMessageReceive event for use with the MXSocketServer class
    /// </summary>
    public class MessageReceivedArgs : EventArgs
    {
        private MXSocketMessage vMessage;

        public MessageReceivedArgs(MXSocketMessage _Message)
        {
            vMessage = _Message;
        }

        public MXSocketMessage Message { get { return vMessage; } }
    }

    /// <summary>
    /// The MXSocketServer class is a multi-threaded Task based class that creates UDP and TCP
    /// listen servers and handles their send/receive transactions.  It creates a Task on a seperate
    /// thread for UDP transactions.  It also creates a TCP listening Task which in turn creates a new
    /// Task to handle each TCP client connection individually.  This enables the class to handle 
    /// multiple clients at the same time.  When a client message is received it raises the 
    /// OnMessageReceived event in which the host class can set the responce to send back to the client. 
    /// All transactions for this Server are done using strings.
    /// </summary>
    class MXSocketServer
    {
        private List<Task> tskListenTCP = new List<Task>();  // List for the TCP listener Tasks
        private List<Task> tskListenUDP = new List<Task>();  // List for the UDP listener Tasks
        private List<Task> tskHandleTCP = new List<Task>();  // List for the TCP handler Tasks

        public delegate void MessageReceivedDelegate(MessageReceivedArgs e);  // OnMessageReceive Delegate
        public event MessageReceivedDelegate OnMessageReceived = delegate { };  // Event for when messages are received from client

        public int TCPcnt { get; set; } = 0;  // Keeps track of the current number of TCP connections

        /// <summary>
        /// Creates a new Task for the TCP Listen Server on the specified port number
        /// </summary>
        /// <param name="PortNum">Port to listen on for TCP connections</param>
        /// <returns>None</returns>
        public void TCPListen(int PortNum)
        {
            tskListenTCP.Add(Task.Run(() => TCPListener(PortNum)).ContinueWith(CleanUpTasks));  // Start new listener task and clean up lists when finished
        }

        /// <summary>
        /// Creates a new Task for the UDP Listen Server on the specified port number
        /// </summary>
        /// <param name="PortNum">Port to listen on for UDP connections</param>
        /// <returns>None</returns>
        public void UDPListen(int PortNum)
        {
            tskListenUDP.Add(Task.Run(() => UDPListener(PortNum)).ContinueWith(CleanUpTasks)); // Start new listener task and clean up lists when finished
        }

        /// <summary>
        /// Removes finished tasks from the task Lists
        /// </summary>
        /// <param name="Task">Name of the current task running</param>
        /// <returns>None</returns>
        private void CleanUpTasks(Task vTask)
        {
            tskListenUDP.RemoveAll(x => x.IsCompleted);  // Remove UDP Listener tasks that have completed
            tskListenTCP.RemoveAll(x => x.IsCompleted);  // Remove TCP Listener tasks that have completed
            tskHandleTCP.RemoveAll(x => x.IsCompleted);  // Remove TCP Handler tasks that have completed
        }

        /// <summary>
        /// Listens for TCP connections and starts a new task to handle each new connection
        /// </summary>
        /// <param name="PortNum">Port to listen on for TCP connections</param>
        /// <returns>None</returns>
        private void TCPListener(int PortNum)
        {
            TcpListener vTCPserver = new TcpListener(IPAddress.Any, PortNum);  // Setup new TCP listen server
            vTCPserver.Start();  // Start the listen server
            Console.WriteLine("TCP Server Listening on Port " + PortNum.ToString() + "...");  // Write to console
            while (true)  // Loop forever TODO: Add a way to end loop
            {
                TcpClient vTCPclient = vTCPserver.AcceptTcpClient();  // Wait for new TCP client connections
                tskHandleTCP.Add(Task.Run(() => TCPHandler(vTCPclient)).ContinueWith(CleanUpTasks));  // Start new task to handle the client connection
            }
        }

        /// <summary>
        /// Listens for and hangles UDP transactions
        /// </summary>
        /// <param name="PortNum">Port to listen on for TCP connections</param>
        /// <returns>None</returns>
        private void UDPListener(int PortNum)
        {
            UdpClient vUDPclient = new UdpClient(PortNum);  // Setup new UDP listen server
            IPEndPoint vEndPoint = new IPEndPoint(IPAddress.Any, PortNum);  // Setup the endpoint to any address available on computer
            Console.WriteLine("UDP Server Listening on Port " + PortNum.ToString() + "...");  // Write to console
            while (true)  // Loop forever TODO: Add a way to end loop
            {
                try
                {
                    String vMessageIN = Encoding.Default.GetString(vUDPclient.Receive(ref vEndPoint));  // Wait for UDP data and convert it to string
                    string vClientIP = vEndPoint.Address.ToString();  // Store the client IP Address
                    int vClientPort = vEndPoint.Port;  // Store the client Port number
                    MXSocketMessage vMessage = new MXSocketMessage(vClientIP, vClientPort, vMessageIN);  // Create a new Socket Message from received data
                    Console.WriteLine("UDP Client " + vMessage.IPAddress + ":" + vMessage.Port.ToString() + " - RCV Data:  " + vMessage.Raw);  // Write to Console

                    OnMessageReceived(new MessageReceivedArgs(vMessage));  // Raise OnMessageReceived event to host object
                    if (vMessage.Responce == "") vMessage.Responce = "Error - No Listeners Available";  // If no responce was set, set responce to Error

                    string vOut = vMessage.Command + "," + vMessage.Responce;  // Create string to send back to client
                    Console.WriteLine("UDP Client " + vMessage.IPAddress + ":" + vMessage.Port.ToString() + " - SND Data:  " + vOut);  // Write to Console
                    byte[] vBytes = Encoding.ASCII.GetBytes(vOut);  // Convert output string to byte array
                    vUDPclient.Send(vBytes, vBytes.Length, vEndPoint);  // Send byte array to client
                }
                catch (Exception vErr)
                {
                    Console.WriteLine("UDP Error - " + vErr.Message);  // Write Error to console
                }
                Thread.Sleep(1);  // Sleep 1ms so we don't swamp the CPU
            }
        }

        /// <summary>
        /// Handles a single TCP connection.  Listens for new TCP traffic and raises the OnMessageReceived Event
        /// when TCP data is received.  After event is handled it sends back the responce to the TCP client.
        /// </summary>
        /// <param name="TCPclient">TcpClient class that handles the TCP connection</param>
        /// <returns>None</returns>
        private void TCPHandler(TcpClient TCPclient)
        {
            TCPcnt++;  // Increment the TCP client connection counter
            string vClientIP = ((IPEndPoint)TCPclient.Client.RemoteEndPoint).Address.ToString();  // Store the client IP address
            int vClientPort = ((IPEndPoint)TCPclient.Client.RemoteEndPoint).Port;  // Store the client Port number
            Console.WriteLine("TCP Client " + vClientIP + ":" + vClientPort.ToString() + " - Connected");  // Write to console

            TCPclient.ReceiveTimeout = 15000;
            while (TCPclient.Connected)  // Loop until the client is not longer connected or the connection times out
            {
                try
                {
                    NetworkStream vStream = TCPclient.GetStream();  // Create a new stream for reading the writing to client
                    Byte[] vBuffer = new Byte[TCPclient.ReceiveBufferSize];  // Create a buffer for reading and writing to client
                    int vBytesRead = vStream.Read(vBuffer, 0, vBuffer.Length);  // Read data from the client stream
                    if (vBytesRead > 0)
                    {
                        String vMessageIN = Encoding.Default.GetString(vBuffer, 0, vBytesRead);  // Convert data read from client to string
                        MXSocketMessage vMessage = new MXSocketMessage(vClientIP, vClientPort, vMessageIN);  // Create new Socket Message object from received data
                        Console.WriteLine("TCP Client " + vMessage.IPAddress + ":" + vMessage.Port.ToString() + " - RCV Data:  " + vMessage.Raw);  // Write to Console

                        OnMessageReceived(new MessageReceivedArgs(vMessage));  // Raise OnMessageReceived event to host object
                        if (vMessage.Responce == "") vMessage.Responce = "Error - No Listeners Available";  // If no responce was set, set responce to Error

                        string vOut = vMessage.Command + "," + vMessage.Responce;  // Create string to send back to client
                        Console.WriteLine("TCP Client " + vMessage.IPAddress + ":" + vMessage.Port.ToString() + " - SND Data:  " + vOut);  // Write to Console
                        vBuffer = Encoding.ASCII.GetBytes(vOut);  // Convert output string to byte array
                        vStream.Write(vBuffer, 0, vBuffer.Length);  // Write byte array to client stream
                    }
                    else TCPclient.Close();  // Client closed connection
                }
                catch (Exception)
                {
                    Console.WriteLine("TCP Client " + vClientIP + ":" + vClientPort.ToString() + " - Timed Out");  // Write to Console
                    TCPclient.Close();  // Make sure Connection is closed
                }
                Thread.Sleep(1);  // Sleep 1ms so we don't swamp the CPU
            }

            Console.WriteLine("TCP Client " + vClientIP + ":" + vClientPort.ToString() + " - Disconnected");  // Write to Console
            TCPcnt--;  // Decrement the TCP client connection counter
        }
    }
}
// White Space
// Yes I did go overboard on commenting... I was procrastinating moving on to the next task, at least I didn't comment all of the white space
// This was a test to see if anybody actually reads my code  :P
// White Space