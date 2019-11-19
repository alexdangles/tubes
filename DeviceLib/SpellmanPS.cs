using System;
using Helper;
using System.Threading;
using Devices.Properties;
using System.Collections.Generic;

namespace Devices
{
    /// <summary>
    /// Class for control of Spellman power supplies.
    /// </summary>
    public class SpellmanPS : Link
    {
        #region Fields

        const double maxkV = 75;
        const double maxmA = 16;
        const double maxFil = 5;
        const double kVScaler = maxkV / 4095; // in kV
        const double uAScaler = maxmA / 4095 * 1000; // in uA
        const double filScaler = maxFil / 4095; // in A

        private bool _on, _connected, monitor;
        private double _setkV, _setuA;
        public const int monitorFreqMs = 1000;
        private double _setFilLimit;
        private double _setFilPreHeat;
        #endregion

        #region Constructors


        /// <summary>
        /// Connect to power supply.
        /// </summary>
        /// <param name="monitor">Enable monitor of power supply volts/amps/status</param>
        public SpellmanPS(string ipAddress = "192.168.1.4", bool monitor = true) : base(Settings.Default.SpellmanPort, ipAddress)
        {
            fault = new Dictionary<string, bool>();
            fault.Add("Arc", false);
            fault.Add("Over Temperature", false);
            fault.Add("Over Voltage", false);
            fault.Add("Under Voltage", false);
            fault.Add("Over Current", false);
            fault.Add("Under Current", false);

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
        /// Read interlock closed status.
        /// </summary>
        public bool interlock { get; private set; }

        /// <summary>
        /// Get software version of device.
        /// </summary>
        public string swVersion { get; private set; }

        /// <summary>
        /// Get hardware version of device.
        /// </summary>
        public string hwVersion { get; private set; }

        /// <summary>
        /// Get model number of device.
        /// </summary>
        public string model { get; private set; }

        /// <summary>
        /// Get or set the power on state.
        /// </summary>
        public bool on
        {
            get
            {
                return _on;
            }
            set
            {
                ResetFaults();
                ResetTotalHours();
                Send(98, Convert.ToByte(value));
            }
        }

        /// <summary>
        /// Get faulted status.
        /// </summary>
        public bool faulted { get; private set; }

        /// <summary>
        /// Get remote status.
        /// </summary>
        public bool remoteMode { get; private set; }

        /// <summary>
        /// Get or set the kV setting.
        /// </summary>
        public double setkV
        {
            get
            {
                return _setkV;
            }
            set
            {
                Send(10, (ushort)(value / kVScaler));
                Send(14);
            }
        }

        /// <summary>
        /// Get or set the beam current setting.
        /// </summary>
        public double setuA
        {
            get
            {
                return _setuA;
            }
            set
            {
                Send(11, (ushort)(value / uAScaler));
                Send(15);
            }
        }

        /// <summary>
        /// Get or set filament limit setting.
        /// </summary>
        public double setFilLimit
        {
            get
            {
                return _setFilLimit;
            }
            set
            {
                Send(12, (ushort)(value / filScaler));
                Send(16);
            }
        }

        /// <summary>
        /// Get or set filament pre-heat setting.
        /// </summary>
        public double setFilPreHeat
        {
            get
            {
                return _setFilPreHeat;
            }
            set
            {
                Send(13, (ushort)(value / filScaler));
                Send(17);
            }
        }

        /// <summary>
        /// Get the monitored kV.
        /// </summary>
        public double monkV { get; private set; }

        /// <summary>
        /// Get the monitored beam current.
        /// </summary>
        public double monuA { get; private set; }

        /// <summary>
        /// Get the monitored filament limit.
        /// </summary>
        public double monFilament { get; private set; }

        /// <summary>
        /// Get total hours HV on.
        /// </summary>
        public double hoursOn { get; private set; }

        /// <summary>
        /// Get the fault status of power supply.
        /// </summary>
        public Dictionary<string, bool> fault { get; private set; }

        #endregion

        #region Methods


        /// <summary>
        /// Monitor power supply status continuously.
        /// </summary>
        private void Monitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(monitorFreqMs);

                Send(19); // Analog monitors
                Send(21); // HV on counter
                Send(22); // Status
                Send(50); // Connected
                Send(68); // Faults
            }
        }

        /// <summary>
        /// Get model, HW and SW info.
        /// </summary>
        public void GetDeviceInfo()
        {
            Send(23); // Software version
            Send(24); // Hardware version
            Send(26); // Model number
            Send(50); // Network settings
        }

        public void ResetTotalHours()
        {
            Send(30);
        }

        public void ResetFaults()
        {
            Send(31);
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
                        string[] msgTotal = outQ.Dequeue().ToString().Split(',');
                        int cmd = Convert.ToInt32(msgTotal[0]);

                        switch (cmd)
                        {
                            case 14:
                                _setkV = Convert.ToDouble(msgTotal[1]) * kVScaler;
                                break;

                            case 15:
                                _setuA = Convert.ToDouble(msgTotal[1]) * uAScaler;
                                break;

                            case 16:
                                _setFilLimit = Convert.ToDouble(msgTotal[1]) * filScaler;
                                break;

                            case 17:
                                _setFilPreHeat = Convert.ToDouble(msgTotal[1]) * filScaler;
                                break;

                            case 19:
                                monkV = Convert.ToDouble(msgTotal[1]) * kVScaler;
                                monuA = Convert.ToDouble(msgTotal[2]) * uAScaler;
                                monFilament = Convert.ToDouble(msgTotal[3]) * filScaler;
                                break;

                            case 21:
                                hoursOn = Convert.ToDouble(msgTotal[1]);
                                break;

                            case 22:
                                _on = Convert.ToBoolean(Convert.ToByte(msgTotal[1]));
                                interlock = Convert.ToBoolean(Convert.ToByte(msgTotal[2]));
                                faulted = Convert.ToBoolean(Convert.ToByte(msgTotal[3]));
                                remoteMode = Convert.ToBoolean(Convert.ToByte(msgTotal[4]));
                                break;

                            case 23:
                                swVersion = msgTotal[1];
                                break;

                            case 24:
                                hwVersion = msgTotal[1];
                                break;

                            case 26:
                                model = msgTotal[1];
                                break;

                            case 50:
                                _connected = msgTotal[1].Contains("Spellman");
                                break;

                            case 68:
                                for (int i = 0; i < fault.Count; i++)
                                {
                                    fault["Arc"] = Convert.ToBoolean(Convert.ToByte(msgTotal[1]));
                                    fault["Over Temperature"] = Convert.ToBoolean(Convert.ToByte(msgTotal[2]));
                                    fault["Over Voltage"] = Convert.ToBoolean(Convert.ToByte(msgTotal[3]));
                                    fault["Under Voltage"] = Convert.ToBoolean(Convert.ToByte(msgTotal[4]));
                                    fault["Over Current"] = Convert.ToBoolean(Convert.ToByte(msgTotal[5]));
                                    fault["Under Current"] = Convert.ToBoolean(Convert.ToByte(msgTotal[6]));
                                }
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
        /// Send command with argument.
        /// </summary>
        /// <param name="cmd">Command to send.</param>
        /// <param name="arg">Argument of command.</param>
        private void Send(ushort cmd, ushort arg)
        {
            Tx($"{ STX }{ cmd },{ arg },{ ETX }");
        }

        /// <summary>
        /// Send command with no argument.
        /// </summary>
        /// <param name="cmd"></param>
        private void Send(ushort cmd)
        {
            Tx($"{ STX }{ cmd },{ ETX }");
        }

        /// <summary>
        /// Disconnect the power supply.
        /// </summary>
        public void Disconnect()
        {
            monitor = on = false;
            setkV = setuA = 0;
            Dispose();
        }
        #endregion
    }
}
