using System;
using System.Threading;
using System.IO.Ports;
using Helper;

namespace Devices
{
    /// <summary>
    /// Thor Labs filter wheel FW102c class.
    /// </summary>
    public class FltrWhl : Link
    {
        #region Fields


        bool _connected, monitor;
        int newPosition, currentPosition = 1;

        /// <summary>
        /// How fast to monitor from device. Minimum 500 ms.
        /// </summary>
        public const int monitorFreqMs = 1000;

        /// <summary>
        /// Baud rate of device.
        /// </summary>
        public const int baudRate = 115200;

        #endregion

        #region Constructors


        public FltrWhl(string port = "COM1", bool monitor = true) : base(new SerialPort(port, baudRate))
        {
            msToWait = 200;
            Thread readQ = new Thread(ReadQ);
            readQ.Start();
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
        /// Get connection status of filter wheel.
        /// </summary>
        public bool connected
        {
            get
            {
                return _connected && portOpen;
            }
        }

        /// <summary>
        /// Get status of moving filter.
        /// </summary>
        public bool switching { get; private set; }
        
        /// <summary>
        /// Model number and/or version of device.
        /// </summary>
        public string model { get; private set; }

        /// <summary>
        /// Set position of filter.
        /// </summary>
        public int position
        {
            get
            {
                return currentPosition;
            }
            set
            {
                newPosition = (value > 0 && value <= 12)? value : currentPosition;
                Thread sf = new Thread(SwitchFilter);
                sf.Start();
            }
        }
        #endregion

        #region Methods


        public static string AutoConnect()
        {
            return FindCOMPort(new SerialPort("COM1", baudRate), "THORLABS", $"*idn?{ CR }");
        }

        /// <summary>
        /// Get model number of device.
        /// </summary>
        public void GetDeviceInfo()
        {
            Send("*idn?");
        }

        /// <summary>
        /// Move the wheel to a new position.
        /// </summary>
        public void SwitchFilter()
        {
            switching = true;
            Send($"pos={ newPosition }");
            Thread.Sleep(Math.Abs(newPosition - currentPosition) * monitorFreqMs); // Wait for wheel to reach set position
            Send("pos?");
            switching = false;
        }

        private void Send(string msg)
        {
            Tx($"{ msg }{ CR }");
        }

        /// <summary>
        /// Monitor filter position continuously.
        /// </summary>
        private void Monitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(monitorFreqMs);

                if (!switching) Send("pos?");
            }
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
                        string msgRec = RemoveJunk(outQ.Dequeue().ToString(), '>');

                        if (msgRec.Contains("THORLABS"))
                        {
                            _connected = true;
                            model = msgRec.Split('?')[1];
                        }
                        else if (msgRec.Contains("pos?")) currentPosition = int.Parse(msgRec.Split('?')[1]);
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Disconnect the filter wheel.
        /// </summary>
        public void Disconnect()
        {
            monitor = false;
            Dispose();
        }
        #endregion
    }
}
