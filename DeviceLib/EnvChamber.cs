using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using Helper;
using Devices.Properties;

namespace Devices
{
    /// <summary>
    /// ESPEC envionmental chamber control class.
    /// </summary>
    public class EnvChamber : Link
    {
        #region Fields


        public enum Device { Thermotron, ESPEC }
        private Device device;
        private bool _on, partControl, monitor;
        private double _setTemp, _tempHighLim, _tempLowLim;
        private int _setHumi, _humiHighLim, _humiLowLim;

        /// <summary>
        /// How fast to monitor from device. Minimum 1000 ms.
        /// </summary>
        public const int monitorFreqMs = 2000;

        /// <summary>
        /// Baud rate of device.
        /// </summary>
        public const int baudRate = 19200;

        #endregion

        #region Constructors


        /// <summary>
        /// Initialize ESPEC Environmental Chamber.
        /// </summary>
        /// <param name="port">TCP port of connection.</param>
        /// <param name="ipAddress">ipAddress of chamber.</param>
        /// <param name="monitor">Enable continuous monitor of chamber.</param>
        public EnvChamber(string ipAddress = "192.168.1.101", bool monitor = true) : base(Settings.Default.ESPECPort, ipAddress)
        {
            device = Device.ESPEC;
            Thread readQ = new Thread(ReadQ);
            readQ.Start();
            GetDeviceInfo();
            if (monitor)
            {
                Thread monThread = new Thread(Monitor);
                monThread.Start();
            }
        }

        /// <summary>
        /// Initialize Thermotron Environmental Chamber.
        /// </summary>
        /// <param name="port">COM port.</param>
        /// <param name="monitor">Enable continuous monitor of chamber.</param>
        public EnvChamber(int dummy = 1, string port = "COM1", bool monitor = true) : base(new SerialPort(port, baudRate))
        {
            device = Device.Thermotron;
            msToWait = 300;
            Thread readQ = new Thread(ReadQ);
            readQ.Start();
            GetDeviceInfo();
            if (monitor)
            {
                Thread monThread = new Thread(Monitor);
                monThread.Start();
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get the connection state of chamber.
        /// </summary>
        public bool connected
        {
            get
            {
                return portOpen;
            }
        }

        /// <summary>
        /// Get ROM version.
        /// </summary>
        public string model { get; private set; }

        /// <summary>
        /// Turn the chamber operation on or off.
        /// </summary>
        public bool on
        {
            get
            {
                return _on;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    string enable = value ? "CONSTANT" : "STANDBY";
                    Send($"MODE, { enable }");
                }
                else if (device == Device.Thermotron)
                {
                    string enable = value ? "RESM" : "HOLD";
                    Send($"{ enable }");
                }
            }
        }

        /// <summary>
        /// Get monitored air temperature in chamber.
        /// </summary>
        public double monTemp { get; private set; }

        /// <summary>
        /// Get monitored part temperature in chamber.
        /// </summary>
        public double monPartTemp { get; private set; }

        /// <summary>
        /// Get or set the air temperature in chamber.
        /// </summary>
        public double setTemp
        {
            get
            {
                return _setTemp;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    if (partControl)
                    {
                        partControl = false;
                        Send("TEMP PTC, PTCOFF, DEVP5.0, DEVN-5.0");
                    }
                    Send($"TEMP, S{ value.ToString("0.0") }");
                }
                else if (device == Device.Thermotron)
                {
                    Send($"SETP1,{ value.ToString("0") }");
                }
            }
        }

        /// <summary>
        /// Get or set the part temperature in chamber.
        /// </summary>
        public double setPartTemp
        {
            get
            {
                return _setTemp;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    if (!partControl)
                    {
                        partControl = true;
                        Send("TEMP PTC, PTCON, DEVP5.0, DEVN-5.0");
                    }
                    Send($"TEMP, S{ value.ToString("0.0") }");
                }
                else if (device == Device.Thermotron)
                {
                    Send($"SETP3,{ value.ToString("0") }");
                }
            }
        }

        /// <summary>
        /// Get or set the upper limit temperature in chamber.
        /// </summary>
        public double tempHighLim
        {
            get
            {
                return _tempHighLim;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    Send($"TEMP, H{ value.ToString("0.0") }");
                }
            }
        }

        /// <summary>
        /// Get or set the lower limit temperature in chamber.
        /// </summary>
        public double tempLowLim
        {
            get
            {
                return _tempLowLim;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    Send($"TEMP, L{ value.ToString("0.0") }");
                }
            }
        }

        /// <summary>
        /// Get monitored humidity in chamber.
        /// </summary>
        public int monHumi { get; private set; }

        /// <summary>
        /// Get or set the humidity in chamber.
        /// </summary>
        public int setHumi
        {
            get
            {
                return _setHumi;
            }
            set
            {
                if (device == Device.ESPEC && !partControl)
                {
                    Send($"HUMI, S{ value.ToString("0") }");
                }
                else if (device == Device.Thermotron)
                {
                    Send($"SETP2,{ value.ToString("0") }");
                }
            }
        }

        /// <summary>
        /// Get or set the upper limit humidity in chamber.
        /// </summary>
        public int humiHighLim
        {
            get
            {
                return _humiHighLim;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    Send($"HUMI, H{ value.ToString("0") }");
                }
            }
        }

        /// <summary>
        /// Get or set the lower limit humidity in chamber.
        /// </summary>
        public int humiLowLim
        {
            get
            {
                return _humiLowLim;
            }
            set
            {
                if (device == Device.ESPEC)
                {
                    Send($"HUMI, L{ value.ToString("0") }");
                }
            }
        }

        /// <summary>
        /// Get the alarms status.
        /// </summary>
        public int alarm { get; private set; }
        #endregion

        #region Methods


        /// <summary>
        /// Autoconnect to Thermotron.
        /// </summary>
        /// <returns></returns>
        public static string AutoConnect()
        {
            return FindCOMPort(new SerialPort("COM1", baudRate), "CHAMBER", "IDEN?");
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
                        string[] msgTotal = outQ.Dequeue().ToString().Split(flag);
                        string tag = msgTotal[0];
                        string msg = msgTotal[1];

                        if (device == Device.ESPEC)
                        {
                            switch (tag)
                            {
                                case "TEMP?":
                                    string[] mon = msg.Split(',');
                                    monTemp = double.Parse(mon[0]);
                                    _setTemp = double.Parse(mon[1]);
                                    _tempHighLim = double.Parse(mon[2]);
                                    _tempLowLim = double.Parse(mon[3]);

                                    break;
                                case "HUMI?":
                                    mon = msg.Split(',');
                                    monHumi = int.Parse(mon[0]);
                                    _setHumi = int.Parse(mon[1]);
                                    _humiHighLim = int.Parse(mon[2]);
                                    _humiLowLim = int.Parse(mon[3]);

                                    break;
                                case "ROM?":
                                    model = msg;

                                    break;
                                case "MODE?":
                                    _on = msg.Contains("CONSTANT") || msg.Contains("RUN");

                                    break;
                            }
                        }
                        else if (device == Device.Thermotron)
                        {
                            switch (tag)
                            {
                                case "PVAR1?":
                                    monTemp = int.Parse(msg);

                                    break;
                                case "PVAR2?":
                                    monHumi = int.Parse(msg);

                                    break;
                                case "PVAR3?":
                                    monPartTemp = int.Parse(msg);

                                    break;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        public void GetDeviceInfo()
        {
            if (device == Device.ESPEC)
            {
                Send("ROM?");
            }
            else if (device == Device.Thermotron)
            {
                Send("IDEN?");
            }
        }

        /// <summary>
        /// Send message to device.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        private void Send(string msg)
        {
            Tx($"{ msg }\r\n", msg);
        }

        /// <summary>
        /// Monitor environmental chamber continuously.
        /// </summary>
        private void Monitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(monitorFreqMs);

                if (device == Device.ESPEC)
                {
                    Send("MODE?");
                    Send("TEMP?");
                    Send("HUMI?");
                }
                else if (device == Device.Thermotron)
                {
                    Send("PVAR1?");
                    Send("PVAR2?");
                    Send("PVAR3?");
                }
            }
        }

        /// <summary>
        /// Cycle temperature.
        /// </summary>
        /// <param name="TEMP">Desired temp. to be reached.</param>
        /// <param name="PART_CONTROL">Select between part or chamber temp.</param>
        /// <param name="HOLD_SEC">Hold time in sec. for each desired temp.</param>
        /// <param name="CYCLES">Number of times to repeat this cycle.</param>
        public async void Cycle(double[] TEMP, bool PART_CONTROL, double HOLD_SEC, int CYCLES)
        {
            await Task.Run(() =>
            {
                on = true;
                for (int j = 0; j < CYCLES && connected; j++)
                {
                    for (int i = 0; i < TEMP.Length; i++)
                    {
                        if (PART_CONTROL)
                        {
                            setPartTemp = TEMP[i];
                            Thread.Sleep(2000);
                            while (connected && on && monPartTemp < TEMP[i]) Thread.Sleep(monitorFreqMs);
                        }
                        else
                        {
                            setTemp = TEMP[i];
                            Thread.Sleep(2000);
                            while (connected && on && monTemp < TEMP[i]) Thread.Sleep(monitorFreqMs);
                        }
                        if (!connected || !on) break;
                        Thread.Sleep((int)(HOLD_SEC * 1000));
                    }
                }
                on = false;
            });
        }

        /// <summary>
        /// Disconnect the chamber.
        /// </summary>
        public void Disconnect()
        {
            monitor = false;
            if (device == Device.ESPEC)
            {
                Send("MODE, OFF");
            }
            else if (device == Device.Thermotron)
            {
                Send("STOP");
            }
            Dispose();
        }
        #endregion
    }
}