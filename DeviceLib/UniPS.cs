using System;
using System.Threading;
using Helper;
using Devices.Properties;
using System.Collections.Generic;

namespace Devices
{
    /// <summary>
    /// Universal Tube Controller (UTC) class for use in powering various x-ray tube power supply types.
    /// </summary>
    public class UniPS : Link
    {
        #region Fields

        Tube.ControlType _controlType;
        const int setVCh = 0;
        const int setACh = 1;
        const int monVCh = 12;
        const int monACh = 13;
        double kV, uA, _setV, _setA, _hvPwrAdj, bitsPerkV, bitsPeruA;
        bool _connected, _on, _filReady, kVEnable;
        public bool monitor;

        public int monitorFreqMs = 1000;

        #region I2C Registers

        const int bAddress = 0x67;
        const int bStatus = 0x01;
        const int bFaults = 0x02;
        const int bWatchDog = 0x36;
        const int bkVEnable = 0x38;
        const int bSetkV = 0x1B;
        const int bSetuA = 0x1D;
        const int bMonkV = 0x1F;
        const int bMonuA = 0x21;
        #endregion

        #endregion

        #region Constructors


        /// <summary>
        /// Connect to Raspberry Pi on UTC.
        /// </summary>
        /// <param name="ipAddress">IP address of Raspberry Pi</param>
        /// <param name="monitor">Enable monitor of board</param>
        public UniPS(string ipAddress, bool monitor = true) : base(Settings.Default.UTCPort, ipAddress)
        {
            flag = '@'; // Separates digital commands like I2C_READ with the address read from (ie. I2C_READ@2B) in ReadQ thread.

            status = new Dictionary<string, bool>();
            status.Add("Interlock", false);
            status.Add("HV On", false);
            status.Add("HVPG", false);
            status.Add("Tube Ready", false);
            status.Add("Faulted", false);

            faults = new Dictionary<string, bool>();
            faults.Add("Booted", false);
            faults.Add("Interlock", false);
            faults.Add("Unstable", false);
            faults.Add("HV", false);
            faults.Add("Temperature", false);
            faults.Add("Timeout", false);

            registers = new byte[128];
            Thread watchDog = new Thread(WatchDog);
            watchDog.Start();
            Thread readQ = new Thread(ReadQ);
            readQ.Start();
            Send("IP");
            Send("VERSION");
            Send("PING");
            Send("WDT_RESET");

            hvPwrAdj = 0;
            controlType = Tube.ControlType.Analog;

            if (monitor)
            {
                Thread monThread = new Thread(Monitor);
                monThread.Start();
            }
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
                return portOpen && _connected;
            }
        }
        
        /// <summary>
        /// Device IP address.
        /// </summary>
        public string ip { get; private set; }

        /// <summary>
        /// Device software version.
        /// </summary>
        public string version { get; private set; }

        /// <summary>
        /// Get interlock status.
        /// </summary>
        public bool interlock { get; private set; }

        /// <summary>
        /// Get watchdog state (true = active, false = latched off).
        /// </summary>
        public bool watchDogState { get; private set; }
        /// <summary>
        /// Get time duration since last watchdog command was sent, in ms.
        /// </summary>
        public int watchDogTime { get; private set; }
        /// <summary>
        /// Public registers.
        /// </summary>
        public byte[] registers { get; private set; }

        /// <summary>
        /// i2C status registers.
        /// </summary>
        public Dictionary<string, bool> status { get; private set; }

        /// <summary>
        /// i2C fault registers.
        /// </summary>
        public Dictionary<string, bool> faults { get; private set; }

        /// <summary>
        /// Enable or disable tube filament.
        /// </summary>
        public bool filEnable
        {
            get
            {
                return _filReady;
            }
            set
            {
                if (controlType == Tube.ControlType.Analog)
                {
                    Send("FIL_ENABLE", Convert.ToByte(value));
                }
            }
        }

        /// <summary>
        /// Get temperature reading from sensor.
        /// </summary>
        public double temp { get; private set; }
        
        /// <summary>
        /// Get xray detected status.
        /// </summary>
        public bool xRaysOn { get; private set; }

        /// <summary>
        /// Select between analog, I2C or SPI communication.
        /// </summary>
        public Tube.ControlType controlType
        {
            get
            {
                return _controlType;
            }
            set
            {
                Send("TBPWR_EN, 1");
                _controlType = value;
                switch (_controlType)
                {
                    case Tube.ControlType.Analog:
                        Send("EXT_COMM, 0");
                        break;
                    case Tube.ControlType.I2C:
                        bitsPerkV = 74.4;
                        bitsPeruA = 19.5;
                        Send("EXT_COMM, 1");
                        break;
                    case Tube.ControlType.SPI:
                        bitsPerkV = 0; // Fix this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        bitsPeruA = 0; // Need to put real values here.
                        Send("EXT_COMM, 2");
                        break;
                    case Tube.ControlType._120kV:
                        bitsPerkV = 402.5;
                        bitsPeruA = 518.4;
                        Send("EXT_COMM, 1");
                        break;
                }
            }
        }

        /// <summary>
        /// Set adjust volts.
        /// </summary>
        public double hvPwrAdj
        {
            get
            {
                return _hvPwrAdj;
            }
            set
            {
                if (value <= 0) Send("HVPWR_EN", 0);
                else if (value >= 24) Send("HVPWR_EN", 2);
                else
                {
                    Send("HVPWR_EN", 1);
                    SendVal("HVPWR_ADJ", value);
                }
            }
        }

        /// <summary>
        /// Get or set tube voltage.
        /// </summary>
        public double setV
        {
            get
            {
                return _setV;
            }
            set
            {
                kV = value;

                switch (controlType)
                {
                    case Tube.ControlType._120kV:
                        I2CSet();
                        break;

                    case Tube.ControlType.I2C:
                        I2CSet();
                        break;

                    case Tube.ControlType.SPI:
                        byte[] b = BitConverter.GetBytes((ushort)(kV * bitsPerkV));
                        SPIWrite(2, 48, b[0], b[1]);
                        break;

                    default:
                        SendDAC(setVCh, value);
                        break;
                }
            }
        } 

        /// <summary>
        /// Get or set tube current.
        /// </summary>
        public double setA
        {
            get
            {
                return _setA;
            }
            set
            {
                uA = value;

                switch (controlType)
                {
                    case Tube.ControlType._120kV:
                        I2CSet();
                        break;

                    case Tube.ControlType.I2C:
                        I2CSet();
                        break;

                    case Tube.ControlType.SPI:
                        byte[] b = BitConverter.GetBytes((ushort)(uA * bitsPeruA));
                        SPIWrite(2, 49, b[0], b[1]);
                        break;

                    default:
                        SendDAC(setACh, value);
                        break;
                }
            }
        }

        /// <summary>
        /// Get monitor volts.
        /// </summary>
        public double monV { get; private set; }

        /// <summary>
        /// Get monitor current.
        /// </summary>
        public double monA { get; private set; }

        /// <summary>
        /// Power switch for connected tube.
        /// </summary>
        public bool on
        {
            get
            {
                return _on && kVEnable;
            }
            set
            {
                _on = value && interlock;
                Send("KV_ENABLE", Convert.ToByte(_on));

                switch (controlType)
                {
                    case Tube.ControlType._120kV:
                        ClearFaults();
                        I2CSet();
                        break;

                    case Tube.ControlType.I2C:
                        ClearFaults();
                        I2CSet();
                        I2CWrite(bkVEnable, Convert.ToByte(_on));
                        break;

                    default:
                        filEnable = _on;
                        break;
                }
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Read from device output queue.
        /// </summary>
        private void ReadQ()
        {
            while (go)
            {
                Thread.Sleep(1);

                while (dataAvailable)
                {
                    try
                    {
                        string[] msg = outQ.Dequeue().ToString().Split(',');
                        string[] val = new string[msg.Length - 1];
                        for (int i = 0; i < val.Length; i++)
                            val[i] = msg[i + 1];
                        byte[] data = new byte[2];

                        switch (msg[0])
                        {
                            case "IP":
                                ip = val[1];
                                break;
                            case "PING": // Connected status
                                _connected = val[0] == "PING";
                                break;
                            case "WDT":
                                watchDogState = Convert.ToBoolean(Convert.ToByte(val[0]));
                                watchDogTime = Convert.ToInt16(val[1]);
                                break;
                            case "VERSION": // Version from SVN repo of code running on device
                                version = val[0];
                                break;
                            case "TIME":

                                break;
                            case "TEMPB": // Temperature in bytes

                                break;
                            case "TEMPK": // Temperature in Kelvin

                                break;
                            case "TEMPC": // Temperature in Celsius
                                temp = Convert.ToDouble(val[0]);
                                break;
                            case "TEMPF": // Temperature in Farenheit

                                break;
                            case "LED_TEST": // LED testing on

                                break;
                            case "LED_DONE": // LED done on

                                break;
                            case "LED_ERROR": // LED error on

                                break;
                            case "KV_ENABLE": // KV enable on
                                kVEnable = Convert.ToBoolean(Convert.ToByte(val[0]));
                                break;
                            case "FIL_READY": // Fil ready
                                _filReady = Convert.ToBoolean(Convert.ToByte(val[0]));
                                break;
                            case "INTERLOCK": // Interlock closed
                                interlock = Convert.ToBoolean(Convert.ToByte(val[0]));
                                break;
                            case "XRAYS": // Xrays detected
                                xRaysOn = Convert.ToBoolean(Convert.ToByte(val[0]));
                                break;
                            case "HVPWR_ADJ": // HV adjustable volts
                                _hvPwrAdj = Convert.ToDouble(val[0]);
                                break;
                            case "HVPWR_EN": // HV adj./24v enable

                                break;
                            case "TBPWR_EN": // Tube aux voltages (+/-5v +3.3v) enable

                                break;
                            case "ADC_BYTES":

                                break;
                            case "ADC_VOLTS":
                                int channel = Convert.ToByte(val[0]);
                                switch (channel)
                                {
                                    case monVCh:
                                        monV = Convert.ToDouble(val[1]);
                                        break;
                                    case monACh:
                                        monA = Convert.ToDouble(val[1]);
                                        break;
                                }
                                break;
                            case "ADC_CONVERT":

                                break;
                            case "ADC_SLOPE":

                                break;
                            case "ADC_OFFSET":

                                break;
                            case "ADC_DEFAULTS":

                                break;
                            case "DAC_BYTES":

                                break;
                            case "DAC_VOLTS":
                                channel = Convert.ToByte(val[0]);
                                switch (channel)
                                {
                                    case 0:
                                        _setV = Convert.ToDouble(val[1]);
                                        break;
                                    case 1:
                                        _setA = Convert.ToDouble(val[1]);
                                        break;
                                }
                                break;
                            case "DAC_CONVERT":

                                break;
                            case "DAC_SLOPE":

                                break;
                            case "DAC_OFFSET":

                                break;
                            case "DAC_DEFAULT":

                                break;
                            case "EXT_COMM":

                                break;

                            case "00@I2C_READ":
                                for (int i = 0; i < 128; i++)
                                {
                                    registers[i] = Convert.ToByte(val[1 + i]);
                                }
                                break;
                            case "01@I2C_READ":
                                bool[] s = BitFun.HexToBitArray(Convert.ToByte(val[1]).ToString("X2"));
                                status["Interlock"] = s[0];
                                status["HV On"] = s[1];
                                status["HVPG"] = s[2];
                                status["Tube Ready"] = s[3];
                                status["Faulted"] = s[7];
                                break;
                            case "02@I2C_READ":
                                bool[] f = BitFun.HexToBitArray(Convert.ToByte(val[1]).ToString("X2"));
                                faults["Booted"] = f[0];
                                faults["Interlock"] = f[1];
                                faults["Unstable"] = f[2];
                                faults["HV"] = f[3];
                                faults["Temperature"] = f[4];
                                faults["Timeout"] = f[5];
                                break;
                            case "1B@I2C_READ":
                                data[0] = Convert.ToByte(val[1]);
                                data[1] = Convert.ToByte(val[2]);
                                _setV = BitFun.ByteToInt(data, true) / bitsPerkV;
                                data[0] = Convert.ToByte(val[3]);
                                data[1] = Convert.ToByte(val[4]);
                                _setA = BitFun.ByteToInt(data, true) / bitsPeruA;
                                data[0] = Convert.ToByte(val[5]);
                                data[1] = Convert.ToByte(val[6]);
                                monV = BitFun.ByteToInt(data, true) / bitsPerkV;
                                data[0] = Convert.ToByte(val[7]);
                                data[1] = Convert.ToByte(val[8]);
                                monA = BitFun.ByteToInt(data, true) / bitsPeruA;
                                break;
                            case "38@I2C_READ":
                                kVEnable = Convert.ToByte(val[1]) == 1;
                                break;
                            case "128@SPI_READ":
                                data[0] = Convert.ToByte(val[1]);
                                data[1] = Convert.ToByte(val[2]);
                                monV = BitFun.ByteToInt(data, true) / bitsPerkV;
                                break;
                            case "192@SPI_READ":
                                data[0] = Convert.ToByte(val[1]);
                                data[1] = Convert.ToByte(val[2]);
                                monA = BitFun.ByteToInt(data, true) / bitsPeruA;
                                break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Monitor UTC status continuously.
        /// </summary>
        private void Monitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(monitorFreqMs);
                //Send("PING"); // Connected?
                Send("TEMPC"); // Board temperature
                Send("INTERLOCK"); // Interlock status
                Send("XRAYS"); // Xrays status

                if (controlType == Tube.ControlType.Analog)
                {
                    Send("KV_ENABLE");
                    SendADC(monVCh); // kV volts
                    SendADC(monACh); // uA current
                }
                else if (controlType == Tube.ControlType.I2C)
                {
                    I2CRead(bStatus, 1); // Status
                    I2CRead(bFaults, 1); // Faults
                    I2CRead(bkVEnable, 1); // HV enable
                    I2CRead(bSetkV, 8); // Set/Mon kV/uA
                }
                else if (controlType == Tube.ControlType.SPI)
                {
                    SPIRead(1, 128, 2); // Mon kV
                    SPIRead(1, 192, 2); // Mon uA
                }
                else if (controlType == Tube.ControlType._120kV)
                {
                    Send("KV_ENABLE");
                    I2CRead(bStatus, 1); // Status
                    I2CRead(bFaults, 1); // Faults
                    I2CRead(bSetkV, 8); // Set/Mon kV/uA
                }
            }
        }

        /// <summary>
        /// Switch for board LEDs.
        /// </summary>
        /// <param name="error">Red LED</param>
        /// <param name="test">Yellow LED</param>
        /// <param name="done">Green LED</param>
        /// <returns></returns>
        public void SetLED(bool error, bool test, bool done)
        {
            Send("LED_ERROR", Convert.ToByte(error));
            Send("LED_TEST", Convert.ToByte(test));
            Send("LED_DONE", Convert.ToByte(done));
        }

        /// <summary>
        /// Read user registers from digital tube.
        /// </summary>
        /// <returns></returns>
        public void GetRegisters()
        {
            I2CRead(0, 128);
        }

        /// <summary>
        /// Clear a fault registers.
        /// </summary>
        public void ClearFaults()
        {
            I2CWrite(bFaults, 0);
        }

        /// <summary>
        /// Give digital tube kV and uA setting.
        /// </summary>
        private void I2CSet()
        {
            byte[] kVBytes = BitConverter.GetBytes((ushort)(kV * bitsPerkV));
            byte[] uABytes = BitConverter.GetBytes((ushort)(uA * bitsPeruA));
            byte[] bytesToSend = new byte[4];
            bytesToSend[0] = kVBytes[0];
            bytesToSend[1] = kVBytes[1];
            bytesToSend[2] = uABytes[0];
            bytesToSend[3] = uABytes[1];
            I2CWrite(bSetkV, bytesToSend);
            GetRegisters();
        }

        /// <summary>
        /// Send value with command string.
        /// </summary>
        /// <param name="command">Command to send.</param>
        /// <param name="val">Double value to send with command.</param>
        public void SendVal(string command, double val)
        {
            Tx($"{ command },{ val.ToString("0.0") }");
        }

        /// <summary>
        /// Send string command to device.
        /// </summary>
        /// <param name="command">Command to send.</param>
        /// <param name="data">Integer data to send with command.</param>
        public void Send(string command, int data = -1)
        {
            if (data > -1) Tx($"{ command },{ data }");
            else Tx(command);
        }

        /// <summary>
        /// Send data to onboard digital to analog converter.
        /// </summary>
        /// <param name="command">Command integer</param>
        /// <param name="register">Registers to send to</param>
        /// <returns></returns>
        private void SendDAC(int channel, double val)
        {
            Tx($"DAC_VOLTS,{ channel },{ val.ToString("0.0") }");
        }

        /// <summary>
        /// Send data to onboard analog to digtial converter.
        /// </summary>
        /// <param name="channel">Channel to read.</param>
        private void SendADC(int channel)
        {
            Tx($"ADC_VOLTS,{ channel }");
        }

        /// <summary>
        /// Write data to SPI bus.
        /// </summary>
        /// <param name="deviceID">1=AD; 2=DA; 3=HVCAL; 4=HVCLAMP; 5=12W;</param>
        /// <param name="data">Data to write (comma delimited).</param>
        private void SPIWrite(int deviceID, params byte[] data)
        {
            Tx($"SPI_WRITE,{ deviceID },{ string.Join(",", data) }");
        }

        /// <summary>
        /// Read bytes from SPI bus.
        /// </summary>
        /// <param name="deviceID">1=AD; 2=DA; 3=HVCAL; 4=HVCLAMP; 5=12W;</param>
        /// <param name="startRegister">Start register to read from.</param>
        /// <param name="bytesToRead">Number of bytes to read.</param>
        private void SPIRead(int deviceID, int startRegister, int bytesToRead = 1)
        {
            Tx($"SPI_WRITE,{ deviceID },{ startRegister }");
            Tx($"SPI_READ,{ deviceID },{ bytesToRead }", startRegister.ToString());
        }

        /// <summary>
        /// Write data to I2C bus.
        /// </summary>
        /// <param name="startRegister">First register to write to.</param>
        /// <param name="data">Data to write (comma delimited).</param>
        private void I2CWrite(int startRegister, params byte[] data)
        {
            Tx($"I2C_WRITE,{ bAddress },{ startRegister },{ string.Join(",", data) }");
        }

        /// <summary>
        /// Read bytes from I2C bus.
        /// </summary>
        /// <param name="startRegister">First register to read from.</param>
        /// <param name="bytesToRead">Number of bytes to read.</param>
        private void I2CRead(int startRegister, int bytesToRead = 1)
        {
            Tx($"I2C_WRITE,{ bAddress },{ startRegister }");
            Tx($"I2C_READ,{ bAddress },{ bytesToRead }", startRegister.ToString("X2"));
        }

        /// <summary>
        /// Send command to keep device connected.
        /// </summary>
        public void WatchDog()
        {
            while (go)
            {
                Thread.Sleep(50);
                Send("WDT");
            }
        }

        /// <summary>
        /// Disconnect the device.
        /// </summary>
        public void Disconnect()
        {
            monitor = false;
            if (connected)
            {
                SetLED(false, false, false);
                on = false;
                setV = setA = 0;
                hvPwrAdj = 0;
                controlType = Tube.ControlType.Analog;
            }
            Dispose();
        }
        #endregion
    }
}