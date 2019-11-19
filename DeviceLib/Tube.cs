using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;
using Helper;
using Devices.Properties;

namespace Devices
{
    /// <summary>
    /// Moxtek X-ray tube controller class. Requires at least one analog or digital controller and/or an external power supply.
    /// </summary>
    public class Tube
    {
        #region Fields


        public delegate void UpdateControls(Status status, bool interlock, bool on, double kV, double uA);
        public event UpdateControls updateControls;

        public Log log = new Log();


        public NIcDAQ daq; // National Instruments cDAQ
        public SpellmanPS sps; // Spellman power supply
        public TDKPS tps; // TDK Lambda power supply
        public UniPS utc; // Moxtek universal tube controller
        public SRS amp; // SRS current amplifier

        double[] digMonitors = new double[4];
        double kV, uA;
        bool hvEnable, _on, monitor;
        string niUSB_8452;

        public enum Controller
        {
            None,
            NIcDAQ,
            NIUSB_8452,
            UTC,
            Spellman
        }
        public enum ControlType
        {
            Analog,
            SPI,
            I2C,
            _120kV
        }
        public enum Status
        {
            Idle,
            Testing,
            Paused,
            Done,
            Error
        }
        Status currentStatus;
        Status lastStatus;
        #endregion

        #region Constructors


        /// <summary>
        /// Establish tube control with both Universal Tube Controller (UTC) and Spellman power supply.
        /// </summary>
        /// <param name="utcIP">Universal tube controller IP.</param>
        /// <param name="spellIP">Spellman power supply IP.</param>
        public Tube(string utcIP, string spellIP)
        {
            utc = new UniPS(utcIP);
            controller = Controller.UTC;
            sps = new SpellmanPS(spellIP);
            Thread interlockMon = new Thread(InterlockMonitor);
            interlockMon.Start();
            log.toConsole = true;
        }

        /// <summary>
        /// Establish tube control with Spellman power supply.
        /// </summary>
        /// <param name="ipAddress">IP address of Spellman.</param>
        public Tube(string ipAddress, int dummy = 1)
        {
            sps = new SpellmanPS(ipAddress);
            controller = Controller.Spellman;
            Thread interlockMon = new Thread(InterlockMonitor);
            interlockMon.Start();
            log.toConsole = true;
        }

        /// <summary>
        /// Establish tube control with Universal Tube Controller (UTC).
        /// </summary>
        /// <param name="ipAddress">IP address of Pi on board.</param>
        public Tube(string ipAddress)
        {
            utc = new UniPS(ipAddress);
            controller = Controller.UTC;
            Thread interlockMon = new Thread(InterlockMonitor);
            interlockMon.Start();
            log.toConsole = true;
        }

        /// <summary>
        /// Establish tube control for Tube Source Characterization Boxes or similar setups.
        /// </summary>
        /// <param name="powerSupply">COM port of external power supply.</param>
        /// <param name="amplifier">COM port of photocurrent amplifier.</param>
        /// <param name="spiI2C">Name of digital controller for tube comm via SPI/I2C interface.</param>
        /// <param name="daqTasks">DAQmx task names to load from NIMAX.
        public Tube(string powerSupply, string amplifier, string spiI2C, string daqTasks)
        {
            keVFit = new int[50];
            keVFitByte = new int[50];
            keVSetFitByte = new int[50];
            keVMonFitByte = new int[50];
            page1keVAddresses = new byte[50];
            page1SetAddresses = new byte[50];
            page1MonAddresses = new byte[50];

            if (powerSupply != null && powerSupply.Contains("COM")) tps = new TDKPS(powerSupply, 1);
            if (amplifier != null && amplifier.Contains("COM")) amp = new SRS(amplifier);
            if (spiI2C != null)
            {
                niUSB_8452 = spiI2C;
                i2cRegisters = NI845x.GetRegisters();
            }
            if (daqTasks != null && daqTasks.Length > 0)
            {
                daq = new NIcDAQ(daqTasks, false);
                if (daq.connected) controller = Controller.NIcDAQ;
            }
            else controller = Controller.NIUSB_8452;
            Thread interlockMon = new Thread(InterlockMonitor);
            interlockMon.Start();
            log.toConsole = true;
        }
        #endregion

        #region Properties


        /// <summary>
        /// Get interlock status.
        /// </summary>
        public bool interlock { get; private set; }

        /// <summary>
        /// Get ramping kV, uA status.
        /// </summary>
        public bool ramping { get; private set; }

        /// <summary>
        /// Main tube control device.
        /// </summary>
        public Controller controller { get; private set; }

        /// <summary>
        /// Get or set analog/digital control for the tube.
        /// </summary>
        public ControlType controlType { get; private set; }

        /// <summary>
        /// Get or set status of tube.
        /// </summary>
        public Status status
        {
            get
            {
                return currentStatus;
            }
            set
            {
                if (!interlock) on = false;
                if (status != value)
                {
                    currentStatus = value;
                    if (status != Status.Testing && on)
                    {
                        on = false;
                    }
                    else
                    {
                        Relay();
                        updateControls?.Invoke(status, interlock, _on, kV, uA);
                    }
                    log.Write(DateTime.Now, $"Tube { status.ToString() }");
                }
            }
        }
        
        /// <summary>
        /// Get connection status of tube.
        /// </summary>
        public bool connected
        {
            get
            {
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        return daq.connected;
                    case Controller.NIUSB_8452:
                        return niErrorCode == 0;
                    case Controller.UTC:
                        return utc.connected;
                    case Controller.Spellman:
                        return sps.connected;
                }
                return false;
            }
        }

        /// <summary>
        /// Read photodiode signal from DAQ.
        /// </summary>
        public double photodiode { get; private set; }

        /// <summary>
        /// Get current error code of digital controller (0 = no error);
        /// </summary>
        public int niErrorCode { get; private set; }
       
        /// <summary>
        /// Get analog enabled state.
        /// </summary>
        public bool analogEnable
        {
            get
            {
                return daq != null ? daq.GetData(DAQ.Default.kVEnableAnalog) > 3 : interlock;
            }
        }
      
        /// <summary>
        /// Get digital enabled state.
        /// </summary>
        public bool digitalEnable
        {
            get
            {
                return daq != null ? daq.GetData(DAQ.Default.kVEnableDigital) > 3.0 : interlock;
            }
        }
      
        /// <summary>
        /// Get or set enabled state of tube.
        /// </summary>
        public bool on
        {
            get
            {
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        if (controlType == ControlType.Analog) return _on && analogEnable;
                        else return _on && digitalEnable && hvEnable;
                    case Controller.NIUSB_8452:
                        return _on && digitalEnable;
                    case Controller.UTC:
                        return utc.on;
                    case Controller.Spellman:
                        return sps.on;
                }
                return false;
            }
            set
            {
                _on = interlock && value; // Condition to turn on the tube
                double _kV = kV, _uA = uA;

                if (_on && rampEnable)
                    setkV = setuA = 0;

                Relay();
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        if (controlType == ControlType.I2C) I2CEnable();
                        break;
                    case Controller.NIUSB_8452:
                        if (controlType == ControlType.I2C) I2CEnable();
                        break;
                    case Controller.UTC:
                        utc.on = _on;
                        break;
                    case Controller.Spellman:
                        sps.on = _on;
                        break;
                }
                updateControls?.Invoke(status, interlock, _on, kV, uA);
                if (_on && rampEnable) Ramp(_kV, _uA);
                if (_on && amp != null && daq != null)
                {
                    Thread photodiodeAutoScale = new Thread(PhotodiodeAutoScale);
                    photodiodeAutoScale.Start();
                }
            }
        }

        /// <summary>
        /// Get or set energy of tube.
        /// </summary>
        public double setkV
        {
            get
            {
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        if (controlType == ControlType.Analog) return daq.GetData(DAQ.Default.kVSetMonitor) * kVScaler;
                        else if (controlType == ControlType.I2C) return digMonitors[0];
                        else if (controlType == ControlType.SPI) return kV;
                        break;
                    case Controller.NIUSB_8452:
                        if (controlType == ControlType.I2C) return digMonitors[0];
                        else if (controlType == ControlType.SPI) return kV;
                        break;
                    case Controller.UTC:
                        if (controlType == ControlType.Analog) return utc.setV * kVScaler;
                        else return utc.setV;
                    case Controller.Spellman:
                        return sps.setkV;
                }
                return 0;                
            }
            set
            {
                kV = value >= maxkV ? maxkV : value;
                Set();
            }
        }
    
        /// <summary>
        /// Get or set beam current of tube.
        /// </summary>
        public double setuA
        {
            get
            {
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        if (controlType == ControlType.Analog) return daq.GetData(DAQ.Default.beamCurrentSetMonitor) * uAScaler;
                        else if (controlType == ControlType.I2C) return digMonitors[1];
                        else if (controlType == ControlType.SPI) return uA;
                        break;
                    case Controller.NIUSB_8452:
                        if (controlType == ControlType.I2C) return digMonitors[1];
                        else if (controlType == ControlType.SPI) return uA;
                        break;
                    case Controller.UTC:
                        if (controlType == ControlType.Analog) return utc.setA * uAScaler;
                        else return utc.setA;
                    case Controller.Spellman:
                        return sps.setuA;
                }
                return 0;
            }
            set
            {
                uA = value >= maxuA ? maxuA : value;
                Set();
            }
        }

        /// <summary>
        /// Enable ramping of kV and uA.
        /// </summary>
        public bool rampEnable { get; set; }

        /// <summary>
        /// Ramp up kV per sec rate.
        /// </summary>
        public double kVRampPerSec { get; set; }

        /// <summary>
        /// Ramp up uA per sec rate.
        /// </summary>
        public double uARampPerSec { get; set; }

        /// <summary>
        /// Get monitored energy of tube.
        /// </summary>
        public double monkV
        {
            get
            {
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        if (controlType == ControlType.Analog) return daq.GetData(DAQ.Default.kVMonitor) * kVScaler;
                        else if (controlType == ControlType.I2C) return digMonitors[2];
                        else if (controlType == ControlType.SPI) return digMonitors[2] * kVScaler;
                        break;
                    case Controller.NIUSB_8452:
                        if (controlType == ControlType.I2C) return digMonitors[2];
                        else if (controlType == ControlType.SPI) return digMonitors[2] * kVScaler;
                        break;
                    case Controller.UTC:
                        if (controlType == ControlType.Analog) return utc.monV * kVScaler;
                        else return utc.monV;
                    case Controller.Spellman:
                        return sps.monkV;
                }
                return 0;
            }
        }
     
        /// <summary>
        /// Get monitored beam current of tube.
        /// </summary>
        public double monuA
        {
            get
            {
                switch (controller)
                {
                    case Controller.NIcDAQ:
                        if (controlType == ControlType.Analog) return daq.GetData(DAQ.Default.beamCurrentMonitor) * uAScaler;
                        else if (controlType == ControlType.I2C) return digMonitors[3];
                        else if (controlType == ControlType.SPI) return digMonitors[3] * uAScaler;
                        break;
                    case Controller.NIUSB_8452:
                        if (controlType == ControlType.I2C) return digMonitors[3];
                        else if (controlType == ControlType.SPI) return digMonitors[3] * uAScaler;
                        break;
                    case Controller.UTC:
                        if (controlType == ControlType.Analog) return utc.monA * uAScaler;
                        else return utc.monA;
                    case Controller.Spellman:
                        return sps.monuA;
                }
                return 0;
            }
        }
     
        /// <summary>
        /// Get monitored watts of tube.
        /// </summary>
        public double watts
        {
            get
            {
                return ((monkV * 1000) * (monuA / 1000000));
            }
        }

        /// <summary>
        /// Get registers data for digital tube.
        /// </summary>
        public DataTable i2cRegisters { get; private set; }
     
        /// <summary>
        /// Stability/Repeatability settings for tube.
        /// </summary>
        public DataTable stabRepSettings { get; set; }

        /// <summary>
        /// Linearity settings for tube.
        /// </summary>
        public DataTable linearitySettings { get; set; }

        /// <summary>
        /// Get name of tube.
        /// </summary>
        public string name { get; private set; }
     
        /// <summary>
        /// Get or set tube serial number.
        /// </summary>
        public int serialNum { get; set; }
      
        /// <summary>
        /// Get the TUB number.
        /// </summary>
        public string tubNum { get; private set; }
      
        /// <summary>
        /// Get tube target type.
        /// </summary>
        public string targetType { get; private set; }
      
        /// <summary>
        /// Get tube power supply type.
        /// </summary>
        public string psType { get; private set; }
    
        /// <summary>
        /// Get tube description.
        /// </summary>
        public string description { get; private set; }
    
        /// <summary>
        /// Get ASM number.
        /// </summary>
        public string ASM { get; private set; }
   
        /// <summary>
        /// Get tube ASM number.
        /// </summary>
        public string tubeASM { get; private set; }
    
        /// <summary>
        /// Get tube assembly date.
        /// </summary>
        public string date { get; private set; }
   
        /// <summary>
        /// Get test file of tube.
        /// </summary>
        public string testFile { get; private set; }
       
        /// <summary>
        /// Get product image file location.
        /// </summary>
        public string productImageFilePath { get; private set; }

        /// <summary>
        /// Get product image bitmap.
        /// </summary>
        public Bitmap productImage { get; private set; }
            
        /// <summary>
        /// Get kV scaler setting of tube.
        /// </summary>
        public double kVScaler { get; private set; }
     
        /// <summary>
        /// Get uA scaler setting of tube.
        /// </summary>
        public double uAScaler { get; private set; }
     
        /// <summary>
        /// Get max kV setting of tube.
        /// </summary>
        public double maxkV { get; set; }
      
        /// <summary>
        /// Get min kV setting of tube.
        /// </summary>
        public double minkV { get; private set; }
      
        /// <summary>
        /// Get max uA setting of tube.
        /// </summary>
        public double maxuA { get; private set; }
      
        /// <summary>
        /// Get min uA setting of tube.
        /// </summary>
        public double minuA { get; private set; }
      
        /// <summary>
        /// Get max watts setting of tube.
        /// </summary>
        public double maxWatts { get; private set; }

        #region Ringo Properties


        /// <summary>
        /// Get kV scaling factor setting of digital tube.
        /// </summary>
        public byte kVBitFactor { get; private set; }
    
        /// <summary>
        /// Get uA scaling factor setting of digital tube.
        /// </summary>
        public byte uABitFactor { get; private set; }
       
        /// <summary>
        /// Get number of ADC bits setting of digital tube.
        /// </summary>
        public byte numADCBits { get; private set; }
       
        /// <summary>
        /// Get number of DAC bits setting of digital tube.
        /// </summary>
        public byte numDACBits { get; private set; }

        /// <summary>
        /// Get keV fit data of digital tube.
        /// </summary>
        public int[] keVFit { get; private set; }
       
        /// <summary>
        /// Get keV fit byte data of digital tube.
        /// </summary>
        public int[] keVFitByte { get; private set; }
       
        /// <summary>
        /// Get keV set fit byte data of digital tube.
        /// </summary>
        public int[] keVSetFitByte { get; private set; }
      
        /// <summary>
        /// Get keV mon fit byte data of digital tube.
        /// </summary>
        public int[] keVMonFitByte { get; private set; }
       
        /// <summary>
        /// Read page1 keV data of digital tube.
        /// </summary>
        public byte[] page1keVAddresses { get; private set; }
       
        /// <summary>
        /// Read page1 set keV data of digital tube.
        /// </summary>
        public byte[] page1SetAddresses { get; private set; }
      
        /// <summary>
        /// Read page1 monitor keV data of digital tube.
        /// </summary>
        public byte[] page1MonAddresses { get; private set; }
        #endregion

        #endregion

        #region Methods


        /// <summary>
        /// Add column headers to log file.
        /// </summary>
        public void Log_AddHeaders()
        {
            List<string> header = new List<string>();
            header.Add("Time");
            header.Add("HV state");
            header.Add("Set kV");
            header.Add("Set uA");
            header.Add("Mon kV");
            header.Add("Mon uA");
            header.Add("Watts");
            header.Add("Photodiode");
            if (controller == Controller.NIcDAQ)
            {
                header.Add("Input Voltage");
                header.Add("Input Current");
                header.Add("kV Enable Analog");
                header.Add("kV Enable Digital");
                header.Add("kV Set Monitor");
                header.Add("uA Set Monitor");
                header.Add("kV Monitor");
                header.Add("uA Monitor");
                header.Add("Interlock Feedback");
                header.Add("Tube Filament Ready");
            }
            else if (controller == Controller.Spellman)
            {
                header.Add("Faulted");
                header.Add("Set Filament Limit");
                header.Add("Mon Filament");
                header.Add("Filament Pre Heat");
                header.Add("Hours On");
            }
            else if (controller == Controller.UTC)
            {
                header.Add("WatchDog Status");
                header.Add("WatchDog Timer");
                header.Add("X rays");
            }
            log.AddHeaders(header.ToArray());
        }

        /// <summary>
        /// Write status of tube to log.
        /// </summary>
        public void LogStatus()
        {
            List<object> data = new List<object>();
            data.Add(DateTime.Now);
            data.Add(Convert.ToByte(on));
            data.Add(setkV);
            data.Add(setuA);
            data.Add(monkV);
            data.Add(monuA);
            data.Add(watts);
            data.Add(photodiode);
            if (controller == Controller.NIcDAQ)
            {
                data.Add(daq.GetData(DAQ.Default.inputVoltage));
                data.Add(daq.GetData(DAQ.Default.inputCurrent));
                data.Add(daq.GetData(DAQ.Default.kVEnableAnalog));
                data.Add(daq.GetData(DAQ.Default.kVEnableDigital));
                data.Add(daq.GetData(DAQ.Default.kVSetMonitor));
                data.Add(daq.GetData(DAQ.Default.beamCurrentSetMonitor));
                data.Add(daq.GetData(DAQ.Default.kVMonitor));
                data.Add(daq.GetData(DAQ.Default.beamCurrentMonitor));
                data.Add(daq.GetData(DAQ.Default.interlock));
                data.Add(daq.GetData(DAQ.Default.tubeFilamentReady));
            }
            else if (controller == Controller.Spellman)
            {
                data.Add(sps.faulted);
                data.Add(sps.setFilLimit);
                data.Add(sps.monFilament);
                data.Add(sps.setFilPreHeat);
                data.Add(sps.hoursOn);
            }
            else if (controller == Controller.UTC)
            {
                data.Add(Convert.ToByte(utc.watchDogState));
                data.Add(utc.watchDogTime);
                data.Add(Convert.ToByte(utc.xRaysOn));
            }
            log.Write(data.ToArray());
        }

        /// <summary>
        /// Perform SQL database query for a tube serial number in the tubes database.
        /// </summary>
        /// <param name="serial">Unique serial number of tube.</param>
        /// <returns>Returns single row of datatable containing all required tube operation settings.</returns>
        public object[] GetSettingsFromDB(int serial)
        {
            // Initialize tube info and settings
            var search = serial > 0 ? DBHelper.SearchTable("viewProductionSourceCharacterization", "SN", serial) : null;
            bool found = search != null && search.Rows.Count > 0;
            if (found)
            {
                object[] items = search.Rows[0].ItemArray;
                serialNum = (int)items[0];
                targetType = items[1].ToString();
                tubNum = items[2].ToString();
                psType = items[3].ToString();
                description = items[4].ToString();
                ASM = items[5].ToString();
                tubeASM = items[6].ToString();
                date = items[7].ToString();

                search = tubNum.Length > 0 ? DBHelper.SearchTable("TSCBPowerSupplyList", "TUBNumber", tubNum) : null;
                found = search != null && search.Columns.Count == 18;

                if (found)
                    return search.Rows[0].ItemArray;
                else
                {
                    serialNum = 0;
                    targetType = tubNum = psType = description = ASM = tubeASM = date = "";
                }
            }
            return null;
        }

        /// <summary>
        /// Initializes tube with required settings.
        /// </summary>
        /// <param name="settings">Required settings for tube operation.</param>
        /// <returns>Returns true if initialized success or false if not.</returns>
        public bool Initialize(params object[] settings)
        {
            status = Status.Idle;
            setkV = setuA = 0;
            name = "";
            psType = "";
            kVScaler = 0;
            uAScaler = 0;
            maxkV = 0;
            minkV = 0;
            maxuA = 0;
            minuA = 0;
            maxWatts = 0;
            tubNum = "";
            productImage = Resources.no_tube_image;

            // Initialize tube settings array
            if (settings != null && settings.Length > 17)
            {
                name = settings[0].ToString();
                psType = settings[1].ToString();
                kVScaler = Convert.ToDouble(settings[2]);
                uAScaler = Convert.ToDouble(settings[3]);
                maxkV = Convert.ToDouble(settings[4]);
                minkV = Convert.ToDouble(settings[5]);
                maxuA = Convert.ToDouble(settings[6]);
                minuA = Convert.ToDouble(settings[7]);
                double psVoltage = Convert.ToDouble(settings[8]);
                maxWatts = Convert.ToDouble(settings[9]);
                tubNum = settings[10].ToString();
                if (Convert.ToInt32(settings[11]) == 1) controlType = ControlType.SPI;
                else if (Convert.ToInt32(settings[11]) == 2) controlType = ControlType.I2C;
                else if (Convert.ToInt32(settings[11]) == 3) controlType = ControlType._120kV;
                else if (Convert.ToInt32(settings[11]) == 4) controller = Controller.Spellman;
                else controlType = ControlType.Analog;

                try
                {
                    testFile = settings[12].ToString();
                    string repeatabilityFile = settings[13].ToString() + @"\Repetability.txt";
                    productImageFilePath = settings[14].ToString() + @"\Product image.png";
                    productImage = new Bitmap(productImageFilePath);
                    string linearitySettingsFile = settings[15].ToString() + @"\Linearsettings table.txt";
                    string spectrumCollectFile = settings[16].ToString();
                    string hardwareSetupFile = settings[17].ToString();
                    stabRepSettings = GetStabRepSettings(repeatabilityFile);
                    linearitySettings = GetLinearitySettings(linearitySettingsFile);
                }
                catch { }

                if (controller == Controller.NIUSB_8452 || controller == Controller.NIcDAQ)
                {
                    if (tps != null)
                    {
                        tps.setV = psVoltage;
                        tps.setA = 3;
                        tps.on = true;
                    }

                    if (niUSB_8452 != null)
                    {
                        Thread spiI2cMonitor = new Thread(SPII2CMonitor);
                        spiI2cMonitor.Start();
                    }
                }
                else if (controller == Controller.UTC)
                {
                    utc.hvPwrAdj = psVoltage;

                    if (controlType == ControlType.Analog) utc.controlType = ControlType.Analog;
                    else if (controlType == ControlType.I2C) utc.controlType = ControlType.I2C;
                    else if (controlType == ControlType.SPI) utc.controlType = ControlType.SPI;
                    else utc.controlType = ControlType._120kV;
                }
                else if (controller == Controller.Spellman)
                {
                    sps.setFilPreHeat = 0.25;
                    sps.setFilLimit = 1000;
                }
                return true;
            }
            // Return error if not partial settings loaded
            if (settings != null && settings.Length > 0) status = Status.Error;
            return false;
        }

        /// <summary>
        /// Monitor thread for interlock.
        /// </summary>
        private void InterlockMonitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(10);

                int i = 0, counter = 0;
                while (i < 3)
                {
                    switch (controller)
                    {
                        case Controller.NIcDAQ:
                            if (daq.DIChannels != null && daq.DIChannels.Length > 0) counter += Convert.ToByte(daq.digitalInData[0]);
                            else counter += Convert.ToByte(daq.GetData(DAQ.Default.interlock) > 3);
                            break;
                        case Controller.NIUSB_8452:
                            bool interlockDIO = Convert.ToBoolean(NI845x.DIOReadLine(niUSB_8452, 0));
                            counter += Convert.ToByte(interlockDIO);
                            break;
                        case Controller.UTC:
                            counter += Convert.ToByte(utc.interlock);
                            break;
                        case Controller.Spellman:
                            counter += Convert.ToByte(sps.interlock);
                            break;
                    }
                    i++;
                }

                if (interlock != counter > 0)
                {
                    interlock = counter > 0;
                    if (interlock)
                    {
                        log.Write(DateTime.Now, "Interlock closed");
                        status = lastStatus;
                    }
                    else
                    {
                        log.Write(DateTime.Now, "Interlock open");
                        lastStatus = status;
                        status = Status.Paused;
                    }
                }
            }
        }

        /// <summary>
        /// Monitor thread for NIUSB-8452 controller.
        /// </summary>
        private void SPII2CMonitor()
        {
            while (controlType == ControlType.SPI || controlType == ControlType.I2C)
            {
                Thread.Sleep(2000);

                if (interlock && controlType == ControlType.SPI)
                {
                    niErrorCode = NI845x.SPIMon(niUSB_8452, digMonitors, digMonitors.Length);
                }
                else if (interlock && controlType == ControlType.I2C)
                {
                    niErrorCode = NI845x.I2CMon(niUSB_8452, kVBitFactor, uABitFactor, numDACBits, numADCBits, digMonitors, digMonitors.Length);
                }
                else niErrorCode = 0;
            }
        }

        /// <summary>
        /// Set the kV and uA. This method provides no user control. Uses internal properties setkV and setuA for inputs
        /// </summary>
        private void Set()
        {
            if ((kV * 1000 * (uA / 1000000)) > maxWatts) uA = (maxWatts / (kV * 1000)) * 1000000;
            switch (controller)
            {
                case Controller.NIcDAQ:
                    if (controlType == ControlType.Analog) daq.AnalogWrite(kV / kVScaler, uA / uAScaler);
                    else I2CSPISet();
                    break;
                case Controller.NIUSB_8452:
                    I2CSPISet();
                    break;
                case Controller.UTC:
                    if (controlType == ControlType.Analog)
                    {
                        utc.setV = kV / kVScaler;
                        utc.setA = uA / uAScaler;
                    }
                    else
                    {
                        utc.setV = kV;
                        utc.setA = uA;
                    }
                    break;
                case Controller.Spellman:
                    sps.setkV = kV;
                    sps.setuA = uA;
                    break;
            }
            updateControls?.Invoke(status, interlock, _on, kV, uA);
        }

        /// <summary>
        /// Continuously turn tube on and off.
        /// </summary>
        /// <param name="onTime_sec">Time tube will be on in sec.</param>
        /// <param name="offTime_sec">Time tube will be off in sec.</param>
        public async void DutyCycle(double onTime_sec, double offTime_sec)
        {
            await Task.Run(() =>
            {
                while (status == Status.Testing)
                {
                    on = true;
                    while (ramping) { Thread.Sleep(100); }
                    Thread.Sleep((int)(onTime_sec * 1000));
                    on = false;
                    Thread.Sleep((int)(offTime_sec * 1000));
                }
            });
        }

        /// <summary>
        /// Ramp up kV, uA to target values.
        /// </summary>
        /// <param name="kVTarget"></param>
        /// <param name="uATarget"></param>
        private async void Ramp(double kVTarget, double uATarget)
        {
            ramping = true;

            double rate = 0.2; // this value should be from 0.1 to 1

            await Task.Run(() =>
            {
                if (kVRampPerSec > 0)
                {
                    while (kV < kVTarget && ramping)
                    {
                        setkV = kV + (kVRampPerSec * rate);
                        Thread.Sleep((int)(rate * 1000));
                    }
                }
                if (uARampPerSec > 0)
                {
                    while (uA < uATarget && ramping)
                    {
                        setuA = uA + (uARampPerSec * rate);
                        Thread.Sleep((int)(rate * 1000));
                    }
                }
            });

            ramping = false;
        }

        /// <summary>
        /// Enable or disable HV for digital tube.
        /// </summary>
        private async void I2CEnable()
        {
            if (interlock)
            {
                await Task.Run(() =>
                {
                    niErrorCode = NI845x.I2CEnable(niUSB_8452, _on);
                });
                Thread.Sleep(500);
                ReadI2CRegisters();
            }
        }

        /// <summary>
        /// Set kV uA for digital tube.
        /// </summary>
        private async void I2CSPISet()
        {
            if (interlock)
            {
                await Task.Run(() =>
                {
                    if (controlType == ControlType.I2C) niErrorCode = NI845x.I2CSet(niUSB_8452, kVBitFactor, uABitFactor, numDACBits, kV, uA);
                    else if (controlType == ControlType.SPI) niErrorCode = NI845x.SPISet(niUSB_8452, kV / kVScaler, uA / uAScaler);
                });
                Thread.Sleep(500);
                ReadI2CRegisters();
            }
        }

        /// <summary>
        /// Disconnect tube and all hardware associated.
        /// </summary>
        public void Disconnect()
        {
            ramping = monitor = on = false;
            setkV = setuA = 0;
            controlType = ControlType.Analog;
            status = Status.Idle;

            daq?.Disconnect();
            tps?.Disconnect();
            utc?.Disconnect();
            sps?.Disconnect();
            amp?.Disconnect();

            log.Dispose();
        }

        /// <summary>
        /// Get stability/repeatability datatable from file.
        /// </summary>
        /// <param name="file">Text file containing stab/rep settings</param>
        /// <returns></returns>
        public static DataTable GetStabRepSettings(string file)
        {
            DataTable repSettings = new DataTable("Stability Repetability Settings Table");
            string[] columns =
            {
                "Test",
                "Test Hours",
                "Iter. sec.",
                "kV",
                "µA",
                "S or R",
                "Time Off",
                "Pinhole",
                "Cal. File",
                "Det. File",
                "Cam. File"
            };
            foreach (string s in columns)
            {
                repSettings.Columns.Add(s);
            }
            if (file != null)
            {
                List<string> input = File.ReadAllLines(file).ToList();
                List<string> output = new List<string>();
                foreach (string line in input)
                {
                    List<string> list = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string item in list)
                    {
                        output.Add(item);
                    }
                }
                int numberofSettings = output.Count / 20;
                for (int i = 0; i < numberofSettings; i++)
                {
                    repSettings.Rows.Add();
                    int j = i * 20;
                    repSettings.Rows[i][0] = output[j];
                    repSettings.Rows[i][1] = output[j + 10];
                    repSettings.Rows[i][2] = output[j + 3];
                    repSettings.Rows[i][3] = output[j + 7];
                    repSettings.Rows[i][4] = output[j + 8];
                    repSettings.Rows[i][5] = output[j + 1];
                    repSettings.Rows[i][6] = output[j + 5];
                    repSettings.Rows[i][7] = output[j + 12];
                    repSettings.Rows[i][8] = output[j + 15];
                    repSettings.Rows[i][9] = output[j + 17];
                    repSettings.Rows[i][10] = output[j + 19];
                }
            }
            return repSettings;
        }

        /// <summary>
        /// Get linearity settings datatable from file.
        /// </summary>
        /// <param name="file">Text file containing linearity settings</param>
        /// <returns></returns>
        public static DataTable GetLinearitySettings(string file)
        {
            DataTable linearSettings = new DataTable("Linearity Settings Table");
            linearSettings.Columns.Add("PS V");
            linearSettings.Columns.Add("kV");
            linearSettings.Columns.Add("µA");

            if (file != null)
            {
                List<string> input = File.ReadAllLines(file).ToList();
                List<double> output = new List<double>();
                input.RemoveAt(0);
                input.RemoveAt(0);
                input.RemoveAt(0);
                foreach (string line in input)
                {
                    List<string> list = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    list.RemoveAt(0);
                    list.RemoveAt(0);
                    list.RemoveAt(0);
                    foreach (string item in list)
                    {
                        output.Add(double.Parse(item));
                    }
                }
                int numberofSettings = output.Count / 3;
                for (int i = 0; i < numberofSettings; i++)
                {
                    linearSettings.Rows.Add();
                    linearSettings.Rows[i][0] = output[(i * 3)];
                    linearSettings.Rows[i][1] = output[(i * 3) + 1];
                    linearSettings.Rows[i][2] = output[(i * 3) + 2];
                }
            }
            return linearSettings;
        }

        /// <summary>
        /// Automatic relay switching controlled by current tube state.
        /// </summary>
        public void Relay(bool blinker = true)
        {
            daq?.DigitalWrite(
            utc != null || _on || controlType == ControlType.I2C, // Power relay
            _on && controlType == ControlType.Analog, // Analog enable
            utc != null || (_on && (controlType == ControlType.I2C || controlType == ControlType.SPI)), // Digital enable
            _on, // X-rays on (red)
            status == Status.Testing && interlock, // Testing in progress (blue)
            status == Status.Done && interlock && !blinker, // Testing done (green)
            status == Status.Testing && interlock, // X-ray box fans
            false // Spare
            );

            utc?.SetLED(
                status == Status.Error && blinker,
                (status == Status.Testing || status == Status.Paused) && blinker,
                status == Status.Done && blinker
                );
        }

        /// <summary>
        /// Manual relay switching for each channel.
        /// </summary>
        /// <param name="channels">true or false for switching each channel</param>
        public void Relay(params bool[] channels)
        {
            if (daq != null)
                daq.DigitalWrite(channels);

            else if (niUSB_8452 != null)
                for (int i = 0; i < channels.Length; i++)
                    niErrorCode = NI845x.DIOWriteLine(niUSB_8452, Convert.ToByte(i), Convert.ToByte(channels[i]));

            if (channels.Length > 2) utc?.SetLED(channels[0], channels[1], channels[2]);
        }

        /// <summary>
        /// Convert a single kV value to a byte value.
        /// </summary>
        /// <param name="kV">kV value</param>
        /// <returns>Returns a byte</returns>
        public byte Convert_kVToByte(double kV)
        {
            return (byte)(kV / (kVBitFactor / Math.Pow(2, numADCBits)));
        }
       
        /// <summary>
        /// Convert an array of kV values to an array of byte values.
        /// </summary>
        /// <param name="kV">kV array</param>
        /// <returns>Returns an array of bytes</returns>
        public byte[] Convert_kVtoByte(double[] kV)
        {
            byte[] b = new byte[kV.Length];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)(kV[i] / (kVBitFactor / Math.Pow(2, numADCBits)));
            }
            return b;
        }
        
        /// <summary>
        /// Convert a single uA value to a byte value.
        /// </summary>
        /// <param name="uA">uA value</param>
        /// <returns>Returns a byte</returns>
        public byte Convert_uAtoByte(double uA)
        {
            return (byte)(uA / (uABitFactor / Math.Pow(2, numDACBits)));
        }
        
        /// <summary>
        /// Convert an array of uA values to an array of byte values.
        /// </summary>
        /// <param name="uA">uA array</param>
        /// <returns>Returns an array of bytes</returns>
        public byte[] Convert_uAtoByte(double[] uA)
        {
            byte[] b = new byte[uA.Length];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)(uA[i] / (uABitFactor / Math.Pow(2, numDACBits)));
            }
            return b;
        }
       
        /// <summary>
        /// Read digital addresses from tube. 
        /// </summary>
        /// <param name="bytesIn">An array of hexadecimal strings containing the desired read addresses</param>
        /// <returns>Returns an array of hexadecimal strings</returns>
        public string[] ReadBytes(string[] bytesIn)
        {
            if (niUSB_8452 != null && controlType == ControlType.I2C)
            {
                try
                {
                    List<byte> bytes = new List<byte>();
                    string[] bytesOut = new string[bytesIn.Length];
                    byte[] input = new byte[bytesIn.Length];
                    byte[] output = new byte[bytesIn.Length];
                    foreach (string line in bytesIn)
                    {
                        if (line != "") bytes.Add(byte.Parse(line.Trim('\r','\n'), System.Globalization.NumberStyles.HexNumber));
                        input = bytes.ToArray();
                    }
                    niErrorCode = NI845x.I2CWriteRead(niUSB_8452, Settings.Default.AdminByte, input, input.Length, output);
                    for (int i = 0; i < bytesOut.Length; i++)
                    {
                        bytesOut[i] = output[i].ToString("X2");
                    }
                    return bytesOut;
                }
                catch { }
            }
            return null;
        }
                              
        /// <summary>
        /// Autoscale task for current amplifier and photodiode.
        /// </summary>
        /// <returns>Returns a task</returns>
        private void PhotodiodeAutoScale()
        {
            Thread.Sleep(2000);
            do
            {
                amp?.AutoScale(daq.GetData(DAQ.Default.photoDiode));

                Thread.Sleep(300);

                double scaler = amp.currentScaler[(int)amp.scaler] * 1000000000;
                photodiode = Math.Abs(daq.GetData(DAQ.Default.photoDiode)) * scaler;
            }
            while (_on);
        }

        /// <summary>
        /// Read all register addresses from a digital tube.
        /// </summary>
        public async void ReadI2CRegisters()
        {
            double[] registers = new double[83];
            await Task.Run(() =>
            {
                niErrorCode = NI845x.I2CReadAll(niUSB_8452, Settings.Default.AdminByte, registers, registers.Length);
                hvEnable = Convert.ToBoolean(registers[3]);
                numDACBits = (byte)registers[29];
                numADCBits = (byte)registers[30];
                kVBitFactor = (byte)registers[31];
                uABitFactor = (byte)registers[32];
                try
                {
                    foreach (DataRow row in i2cRegisters.Rows)
                    {
                        if (i2cRegisters.Rows.IndexOf(row) < 16 && registers[i2cRegisters.Rows.IndexOf(row)] == 1) row["Value"] = "YES";
                        else if (i2cRegisters.Rows.IndexOf(row) < 16 && registers[i2cRegisters.Rows.IndexOf(row)] == 0) row["Value"] = "NO";
                        else row["Value"] = registers[i2cRegisters.Rows.IndexOf(row)];
                    }
                }
                catch { }
            });
        }

        /// <summary>
        /// Calibrate the output of a digital tube using an array of measured kV edge values.
        /// </summary>
        /// <param name="kV">An array of measured kV values</param>
        public async void Calibrate(double[] kV)
        {
            await Task.Run(() =>
            {
                niErrorCode = NI845x.I2CCalibrate(
                    niUSB_8452, 
                    Settings.Default.AdminByte, 
                    kV, kV.Length, 
                    keVFit, 
                    keVFitByte, 
                    keVSetFitByte, 
                    keVMonFitByte, 
                    page1keVAddresses, 
                    page1SetAddresses, 
                    page1MonAddresses, 
                    page1keVAddresses.Length
                    );
            });
        }

        /// <summary>
        /// Delete the stored calibration setting of a digital tube.
        /// </summary>
        public async void DeleteCalibration()
        {
            await Task.Run(() =>
            {
                niErrorCode = NI845x.I2CDeleteCalibration(niUSB_8452);
            });
        }

        /// <summary>
        /// Save the EEPROM of a digital tube.
        /// </summary>
        public async void SaveEEPROM()
        {
            await Task.Run(() =>
            {
                niErrorCode = NI845x.I2CSaveEEPROM(niUSB_8452, Settings.Default.AdminByte);
            });
        }
        #endregion
    }
}