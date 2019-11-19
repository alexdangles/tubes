/*
SERIAL COMMANDS LIST
--------------------------------------------------------------------
TDK Lambda Zup power supply commands 
:ADRn;  Sets the power supply address. n = 01 to 31 (the number of supplies connected)
:DCL;   Clears the communication buffer and registers: Operation status, Alarm status, Programming error
:RMTn;  Sets remote or local mode
:RMT?;  Returns remote or local mode status
:MDL?;  Returns model of power supply
:REV?;  Returns software version
:VOLn;  Sets output volts. n = 00.00 to 36.00
:VOL!;  Returns SV followed by the present programmed voltage
:VOL?;  Returns AV followed by the actual output voltage
:CURn;  Sets output current. n = 0.000 to 6.000
:CUR!;  Returns SA followed by the present programmed current
:CUR?;  Returns AA followed by the actual output current
:OUTn;  Sets the output to ON/OFF. n =  0(OFF) or 1(ON)
:OUT?;  Returns OT followed by the outpu ON/OFF status
:FLDn;  Sets foldback protection ON/OFF. n = 1(arm foldback), 0(release foldback), 2(cancel foldback)
:FLD?;  Returns FD followed by the foldback protection status
:OVPn;  Sets the overvoltage protection limit in volts. n = 01.8 to 40.0
:OVP?;  Returns OP followed by the present programmed overvoltage protection limit
:UVPn;  Sets the undervoltage protection limit in volts. n = 00.0 to 35.9
:UVP?;  Returns UP followed by the present programmed undervoltage protection limit
:ASTn;  Sets auto-restart mode ON/OFF. n = 0(OFF) or 1(ON)
:AST?;  Returns AS followed by the auto-restart mode status
:STA?;  Reads operational status register content. Returns OS followed by characters representing the register's content
:ALM?;  Reads alarm status register content. Returns AL followed by characters representing the register's content
:STP?;  Reads programming error register content. Returns PS followed by characters representing the register's content
:STT?;  Reads the complete status of the power supply. Returns characters representing the following data:
        AV(actual voltage), SV(set voltage), AA(actual current), SA(set current), OS(operational status), AL(alarm status), PS(programming error)
*/

using System;
using System.Threading;
using System.IO.Ports;
using Helper;
using System.Collections.Generic;

namespace Devices
{
    /// <summary>
    /// TDK Lambda Zup 36-6 power supply class.
    /// </summary>
    public class TDKPS : Link
    {
        #region Fields

        private bool _connected, monitor;
        private double _setV, _setA, _uvp, _ovp;
        private int _address, _foldback;

        /// <summary>
        /// How fast to monitor from device. Minimum 500 ms.
        /// </summary>
        public const int monitorFreqMs = 1000;

        /// <summary>
        /// Baud rate of device.
        /// </summary>
        public const int baudRate = 9600;
        #endregion

        #region Constructors


        /// <summary>
        /// Connect to power supply.
        /// </summary>
        /// <param name="port">COM port which power supply is connnected</param>
        /// <param name="address">Address of power supply if multiple connected in series (1 for single)</param>
        /// <param name="monitor">Enable monitor of power supply volts/amps/status</param>
        public TDKPS(string port = "COM1", int address = 1, bool monitor = true) : base(new SerialPort(port, baudRate))
        {
            msToWait = 50;
            status = new Dictionary<string, bool>();
            status.Add("CC/CV", false);
            status.Add("Foldback", false);
            status.Add("AST", false);
            status.Add("ON", false);
            status.Add("SRF", false);
            status.Add("SRV", false);
            status.Add("SRT", false);
            status.Add("Alarm", false);

            alarm = new Dictionary<string, bool>();
            alarm.Add("OVP", false);
            alarm.Add("OTP", false);
            alarm.Add("AC Fail", false);
            alarm.Add("Foldback", false);
            alarm.Add("Programming", false);

            Thread readQ = new Thread(ReadQ);
            readQ.Start();
            this.address = address;
            GetDeviceInfo();
            if (monitor)
            {
                Thread monThread = new Thread(Monitor);
                monThread.Start();
            }
            Thread.Sleep(msToWait);
        }
        #endregion

        #region Properties


        /// <summary>
        /// Get the connection state of power supply.
        /// </summary>
        public bool connected
        {
            get
            {
                return _connected && portOpen;
            }
        }

        /// <summary>
        /// Get model number of device.
        /// </summary>
        public string model { get; private set; }

        /// <summary>
        /// Get the serial address of power supply.
        /// </summary>
        public int address
        {
            get
            {
                return _address;
            }
            set
            {
                Send($"ADR{ value.ToString("00") }");
                _address = value;
            }
        }
       
        /// <summary>
        /// Get or set the power on state.
        /// </summary>
        public bool on
        {
            get
            {
                return status["ON"];
            }
            set
            {
                ResetRegisters();
                Send($"OUT{ Convert.ToByte(value) }");
            }
        }

        /// <summary>
        /// Get or set the voltage.
        /// </summary>
        public double setV
        {
            get
            {
                return _setV;
            }
            set
            {
                Send($"VOL{ value.ToString("00.00") }");
                Send("VOL!");
            }
        }
      
        /// <summary>
        /// Get or set the current limit.
        /// </summary>
        public double setA
        {
            get
            {
                return _setA;
            }
            set
            {
                Send($"CUR{ value.ToString("0.000") }");
                Send("CUR!");
            }
        }
        
        /// <summary>
        /// Get or set under-voltage protection value.
        /// </summary>
        public double uvp
        {
            get
            {
                return _uvp;
            }
            set
            {
                Send($"UVP{ value.ToString("00.0") }");
            }
        }
       
        /// <summary>
        /// Get or set over-voltage protection value.
        /// </summary>
        public double ovp
        {
            get
            {
                return _ovp;
            }
            set
            {
                Send($"OVP{ value.ToString("00.0") }");
            }
        }

        /// <summary>
        /// Get or set foldback protection value.
        /// </summary>
        public int foldback
        {
            get
            {
                return _foldback;
            }
            set
            {
                Send($"FLD{ value.ToString("0") }");
            }
        }
       
        /// <summary>
        /// Get the monitored voltage.
        /// </summary>
        public double monV { get; private set; }
       
        /// <summary>
        /// Get the monitored current.
        /// </summary>
        public double monA { get; private set; }
       
        /// <summary>
        /// Get monitored watts.
        /// </summary>
        public double watts
        {
            get
            {
                return monV * monA;
            }
        }
        /// <summary>
        /// Get the operation status of power supply.
        /// </summary>
        public Dictionary<string, bool> status { get; private set; }
       
        /// <summary>
        /// Get the alarm status of power supply.
        /// </summary>
        public Dictionary<string, bool> alarm { get; private set; }

        #endregion

        #region Methods

        
        public static string AutoConnect()
        {
            return FindCOMPort(new SerialPort("COM1", baudRate), "Lambda", $":ADR01;", ":MDL?;");
        }
        /// <summary>
        /// Reset registers.
        /// </summary>
        private void ResetRegisters()
        {
            Send("DCL");
        }

        /// <summary>
        /// Monitor power supply status.
        /// </summary>
        private void Monitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(monitorFreqMs);

                Send("MDL?"); // is connected
                Send("VOL?"); // mon. volts
                Send("CUR?"); // mon. amps
                Send("STA?"); // status
                Send("ALM?"); // alarm
                Send("UVP?"); // undervolt
                Send("OVP?"); // overvolt
                Send("FLD?"); // foldback
            }
        }

        /// <summary>
        /// Get model, HW and SW info.
        /// </summary>
        public void GetDeviceInfo()
        {
            Send("MDL?");
        }

        /// <summary>
        /// Read from device output queue.
        /// </summary>
        private void ReadQ()
        {
            while (go)
            {
                Thread.Sleep(msToWait);

                while (dataAvailable)
                {
                    try
                    {
                        string msgRec = outQ.Dequeue().ToString();

                        if (msgRec.Contains("Lambda"))
                        {
                            _connected = true;
                            model = msgRec;
                        }
                        else if (msgRec.Contains("AV")) monV = double.Parse(msgRec.TrimStart('A', 'V', '0'));
                        else if (msgRec.Contains("AA")) monA = double.Parse(msgRec.TrimStart('A', '0'));
                        else if (msgRec.Contains("SV")) _setV = double.Parse(msgRec.TrimStart('S', 'V', '0'));
                        else if (msgRec.Contains("SA")) _setA = double.Parse(msgRec.TrimStart('S', 'A', '0'));
                        else if (msgRec.Contains("UP")) _uvp = double.Parse(msgRec.TrimStart('U', 'P', '0'));
                        else if (msgRec.Contains("OP")) _ovp = double.Parse(msgRec.TrimStart('O', 'P', '0'));
                        else if (msgRec.Contains("FD")) _foldback = int.Parse(msgRec.TrimStart('F', 'D'));
                        else if (msgRec.Contains("OS"))
                        {
                            string sta = msgRec.TrimStart('O', 'S');
                            status["CC/CV"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(0, 1)));
                            status["Foldback"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(1, 1)));
                            status["AST"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(2, 1)));
                            status["ON"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(3, 1)));
                            status["SRF"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(4, 1)));
                            status["SRV"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(5, 1)));
                            status["SRT"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(6, 1)));
                            status["Alarm"] = Convert.ToBoolean(Convert.ToByte(sta.Substring(7, 1)));
                        }
                        else if (msgRec.Contains("AL"))
                        {
                            string alm = msgRec.TrimStart('A', 'L');
                            alarm["OVP"] = Convert.ToBoolean(Convert.ToByte(alm.Substring(0, 1)));
                            alarm["OTP"] = Convert.ToBoolean(Convert.ToByte(alm.Substring(1, 1)));
                            alarm["AC Fail"] = Convert.ToBoolean(Convert.ToByte(alm.Substring(2, 1)));
                            alarm["Foldback"] = Convert.ToBoolean(Convert.ToByte(alm.Substring(3, 1)));
                            alarm["Programming"] = Convert.ToBoolean(Convert.ToByte(alm.Substring(4, 1)));
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Send message to device.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        private void Send(string msg)
        {
            Tx($":{ msg };");
        }

        /// <summary>
        /// Disconnect the power supply.
        /// </summary>
        public void Disconnect()
        {
            monitor = on = false;
            setV = setA = 0;
            Dispose();
        }

        #endregion
    }
}