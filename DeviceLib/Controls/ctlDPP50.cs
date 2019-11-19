using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using Helper;
using System.Threading;

namespace Devices.Controls
{
    public partial class ctlDPP50 : UserControl
    {
        #region Fields


        public ctlGraph spmGraph = new ctlGraph();
        public ctlGraph kevGraph = new ctlGraph();
        public Form frmSpec = new Form();
        public Form frmkeVEdge = new Form();

        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe int CalibrateDetector(int[] spectrum, int spectrum_LENGTH, int lowPeakLowEdgeCH, int lowPeakHighEdgeCH, int highPeakLowEdgeCH, int highPeakHighEdgeCH, int lowPeakEV, int highPeakEV, double* slope_OUTPUT, double* intercept_OUTPUT);

        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double KeVEdge(string method, int[] spectrum, int spectrum_LENGTH, double slope, double intercept, int bremsstrahlungEdge, int roiHalfWidth, double[] xData_OUTPUT, double[] yData_OUTPUT, double[] bestFit_OUTPUT);

        MXDPP50 dpp;
        public bool stopper;
        bool _acquiring, headerStamp;
        double runTimeDead;
        public uint runCounter, rateInput, rateOutput, rateCorrected, rateSlow, rateFast2, rateFast3, _bremstrahlungEV;
        public string data;
        enum KeVEdgeMethod { Standard, SWC, SAM }
        const KeVEdgeMethod _keVEdgeMethod = KeVEdgeMethod.SAM;
        const int numOfChannels = 4096;
        #endregion

        #region Constructors


        public ctlDPP50()
        {
            InitializeComponent();
            Disposed += OnDispose;

            spmGraph.Dock = DockStyle.Fill;
            spmGraph.Title = "Spectrum";
            spmGraph.xAxis = "Channel";
            spmGraph.yAxis = "Counts";
            spmGraph.mode = ctlGraph.Mode.Fill;
            spmGraph.AddPlots("Spectrum");

            kevGraph.Title = "keV Edge";
            kevGraph.xAxis = "keV";
            kevGraph.yAxis = "Amplitude";
            kevGraph.mode = ctlGraph.Mode.Line;
            kevGraph.Dock = DockStyle.Fill;
            kevGraph.AddPlots("keVData", "keVFit");

            channel = new int[numOfChannels];
            counts = new int[numOfChannels];
            roi = new int[7];
            for (int i = 0; i < numOfChannels; i++)
            {
                channel[i] = i;
            }
            calibrationParameters = new Tuple<double, double>(14, -18);
            dpp = new MXDPP50();
            cmbDevices.Items.Clear();
            List<string> vList = dpp.GetDeviceList();
            foreach (string l in vList)
            {
                cmbDevices.Items.Add(l);
            }
        }

        #endregion

        #region Properties


        /// <summary>
        /// Get connection state of DPP.
        /// </summary>
        public bool connected
        {
            get { return dpp.IsConnected(); }
        }

        /// <summary>
        /// Get acquiring state from DPP.
        /// </summary>
        public bool acquiring
        {
            get
            {
                return _acquiring;
            }
            set
            {
                if (_acquiring != value && value == false)
                {
                    Stop();
                    lblRunTimeDead.Text = runCounter == 0 ? "" : (runTimeDead / runCounter).ToString("0.0") + " %";
                    lblRunICR.Text = runCounter == 0 ? "" : (rateInput / runCounter).ToString();
                    lblRunOCR.Text = runCounter == 0 ? "" : (rateOutput / runCounter).ToString();
                    lblRunCorrected.Text = runCounter == 0 ? "" : (rateCorrected / runCounter).ToString();
                    lblRunSlow.Text = runCounter == 0 ? "" : (rateSlow / runCounter).ToString();
                    lblRunFast2.Text = runCounter == 0 ? "" : (rateFast2 / runCounter).ToString();
                    lblRunFast3.Text = runCounter == 0 ? "" : (rateFast3 / runCounter).ToString();
                    lblRunTotalCNTS.Text = runCounter == 0 ? "" : totalCounts.ToString();
                    StringBuilder s = new StringBuilder();

                    if (!headerStamp) // Clear the current file and create column headers
                    {
                        s.Append("Timestamp\t");
                        s.Append("Set Time (s)\t");
                        s.Append("Real Time (s)\t");
                        s.Append("Live Time (s)\t");
                        s.Append("Dead Time (%)\t");
                        s.Append("Rate Input\t");
                        s.Append("Rate Output\t");
                        s.Append("Rate Corrected\t");
                        s.Append("Rate Slow\t");
                        s.Append("Rate Fast2\t");
                        s.Append("Rate Fast3\t");
                        s.Append("Total Counts\t");
                        s.Append("ROI 1 (0-2 keV) counts\t");
                        s.Append("ROI 2 (2-10 keV) counts\t");
                        s.Append("ROI 3 (10-20 keV) counts\t");
                        s.Append("ROI 4 (20-30 keV) counts\t");
                        s.Append("ROI 5 (30-40 keV) counts\t");
                        s.Append("ROI 6 (40-50 keV) counts\t");
                        s.Append("ROI 7 (50-60 keV) counts\t");
                        s.Append("\r\n");
                        headerStamp = true;
                    }
                    s.Clear();
                    s.Append($"{ DateTime.Now.ToString("HH:mm:ss") }\t");
                    s.Append($"{ setTime.ToString("0.0") }\t");
                    s.Append($"{ realTime.ToString("0.0") }\t");
                    s.Append($"{ liveTime.ToString("0.0") }\t");
                    s.Append($"{ deadTime.ToString("0.0") }\t");
                    s.Append($"{ rateInput.ToString() }\t");
                    s.Append($"{ rateOutput.ToString() }\t");
                    s.Append($"{ rateCorrected.ToString() }\t");
                    s.Append($"{ rateSlow.ToString() }\t");
                    s.Append($"{ rateFast2.ToString() }\t");
                    s.Append($"{ rateFast3.ToString() }\t");
                    s.Append($"{ totalCounts.ToString() }\t");
                    s.Append($"{ roi[0].ToString() }\t");
                    s.Append($"{ roi[1].ToString() }\t");
                    s.Append($"{ roi[2].ToString() }\t");
                    s.Append($"{ roi[3].ToString() }\t");
                    s.Append($"{ roi[4].ToString() }\t");
                    s.Append($"{ roi[5].ToString() }\t");
                    s.Append($"{ roi[6].ToString() }\t");
                    s.Append("\r\n");
                }
                _acquiring = value;
                tmrKeVEdge.Enabled = acquiring && bremstrahlungEV > 0;
            }
        }

        /// <summary>
        /// Get calibration parameters (Item1: slope, Item2: intercept).
        /// </summary>
        public Tuple<double, double> calibrationParameters { get; private set; }

        /// <summary>
        /// Text file to load calibration parameters from.
        /// </summary>
        public string calibrationFile { get; private set; }

        /// <summary>
        /// Get current keV edge measurement.
        /// </summary>
        public double keVEdge { get; private set; }

        /// <summary>
        /// Get or set the Bremstrahlung edge in eV, of spectrum.
        /// </summary>
        public uint bremstrahlungEV
        {
            get
            {
                return _bremstrahlungEV;
            }
            set
            {
                _bremstrahlungEV = value;
            }
        }

        /// <summary>
        /// DPP channel array 0 to _numOfChannels.
        /// </summary>
        public int[] channel { get; private set; }

        /// <summary>
        /// Array of total counts per channel 0 to 4095.
        /// </summary>
        public int[] counts { get; private set; }

        /// <summary>
        /// Total number of counts in entire spectrum.
        /// </summary>
        public int totalCounts
        {
            get
            {
                return counts.Sum();
            }
        }

        /// <summary>
        /// Array of total counts in specified spectrum region of interest.
        /// </summary>
        public int[] roi { get; set; }

        /// <summary>
        /// Get or set acquisition time setting.
        /// </summary>
        public double setTime
        {
            get
            {
                return txtPresetTime.Text != "" ? double.Parse(txtPresetTime.Text) : 0;
            }
            set
            {
                txtPresetTime.Text = value.ToString("0");
                Stop();
                WritePresets();
            }
        }

        /// <summary>
        /// Get elapsed time of current aquisition.
        /// </summary>
        public double realTime { get; private set; }

        /// <summary>
        /// Get elapsed uptime of current acquisition.
        /// </summary>
        public double liveTime { get; private set; }

        /// <summary>
        /// Get elapsed downtime of current acquisition.
        /// </summary>
        public double deadTime { get; private set; }

        #endregion

        #region Methods


        /// <summary>
        /// Try and autoconnect from device file.
        /// </summary>
        /// <param name="deviceFile">Text file name in C:\Hardware\</param>
        public void AutoConnect(string deviceFile = "")
        {
            string sn = Link.ConnectFromFile(deviceFile, "DPP")[0];
            int id = 0;
            int.TryParse(sn, out id);
            Connect(id);
        }

        /// <summary>
        /// Connect the dpp from serial number.
        /// </summary>
        /// <param name="serialNumber">Serial number of dpp.</param>
        public void Connect(int serialNumber)
        {
            if (cmbDevices.Items.Count == 1)
            {
                cmbDevices.SelectedIndex = 0;
                dpp.Connect(cmbDevices.SelectedItem.ToString());
                Initialize();
            }
            else
            {
                string id = $"DPP50-{ serialNumber }";
                foreach (string dev in cmbDevices.Items)
                {
                    if (dev.Contains(id))
                    {
                        cmbDevices.SelectedItem = id;
                        dpp.Connect(id);
                        Initialize();
                        break;
                    }
                }
            }
            if (spmGraph.Parent == null)
            {
                frmSpec.Controls.Add(spmGraph);
                frmSpec.Size = new Size(800, 400);
                frmSpec.FormClosing += GraphUI_FormClosing;
                btnSpectrum.Show();
            }
            if (kevGraph.Parent == null)
            {
                frmkeVEdge.Controls.Add(kevGraph);
                frmkeVEdge.Size = new Size(800, 400);
                frmkeVEdge.FormClosing += GraphUI_FormClosing;
                btnkeVEdge.Show();
            }
        }

        /// <summary>
        /// Update DPP statistics.
        /// </summary>
        private async void ReadStats()
        {
            while (dpp.Busy) Application.DoEvents();

            try
            {
                var dppStats = await dpp.ReadStatisticsAsync();
                acquiring = dppStats.AcquisitionInProgress;
                realTime = dppStats.TimeReal;
                liveTime = dppStats.TimeLive;
                deadTime = dppStats.TimeDead;

                if (acquiring)
                {
                    runTimeDead += dppStats.TimeDead;
                    rateInput += dppStats.RateInput;
                    rateOutput += dppStats.RateOutput;
                    rateCorrected += dppStats.RateCorrected;
                    rateSlow += dppStats.RateSlow;
                    rateFast2 += dppStats.RateFast2;
                    rateFast3 += dppStats.RateFast3;
                    runCounter++;
                }
                UpdateForm_Statistics(dppStats);

                for (int i = 0; i < roi.Length; i++)
                {
                    roi[i] = 0;
                }
                uint[] cnts = await dpp.ReadSpectrumAsync();
                for (int i = 0; i < cnts.Length; i++)
                {
                    counts[i] = (int)cnts[i];
                }
                int[] ranges = new int[] { 0,
                        Convert_eVToChannel(2000),
                        Convert_eVToChannel(10000),
                        Convert_eVToChannel(20000),
                        Convert_eVToChannel(30000),
                        Convert_eVToChannel(40000),
                        Convert_eVToChannel(50000),
                        Convert_eVToChannel(60000) };
                for (int i = 1; i < roi.Length + 1; i++)
                {
                    for (int j = ranges[i - 1]; j < ranges[i]; j++)
                    {
                        if (j < counts.Length) roi[i - 1] += counts[j];
                        else break;
                    }
                }

                if (acquiring && spmGraph.Visible)
                {
                    double[] keV = new double[numOfChannels];
                    double[] ch = new double[numOfChannels];
                    double[] ct = new double[numOfChannels];
                    for (int i = 0; i < numOfChannels; i++)
                    {
                        ch[i] = channel[i];
                        ct[i] = counts[i];
                    }
                    if (calibrationFile != null)
                    {
                        spmGraph.Plot("Spectrum", Convert_ChannelTokeV(channel), ct);
                    }
                    else
                    {
                        spmGraph.Plot("Spectrum", ch, ct);
                    }
                }

            }
            catch
            {
            }
        }

        /// <summary>
        /// Update the keV Edge.
        /// </summary>
        private void ReadkeVEdge()
        {
            double[] eV = new double[numOfChannels];
            double[] keV = new double[numOfChannels];
            double[] data = new double[numOfChannels];
            double[] dataFit = new double[numOfChannels];
            int roiHalfWidth = (bremstrahlungEV <= 16000) ? (int)(bremstrahlungEV * 0.2) : 4000;

            keVEdge = KeVEdge(_keVEdgeMethod.ToString(), counts, counts.Length, calibrationParameters.Item1, calibrationParameters.Item2, (int)bremstrahlungEV, roiHalfWidth, eV, data, dataFit);
            for (int i = 0; i < eV.Length; i++)
            {
                keV[i] = eV[i] / 1000;
            }

            lblEdge.Text = $"keV Edge:    { keVEdge.ToString("0.000") }";

            if (kevGraph.Visible)
            {
                kevGraph.Plot("keVData", keV, data);
                kevGraph.Plot("keVFit", keV, dataFit);
                kevGraph.xyGraph.Cursors[0].Visible = true;
                kevGraph.xyGraph.Cursors[0].XPosition = keVEdge;
                kevGraph.xyGraph.Cursors[0].YPosition = 0;
                kevGraph.SetXRange(keVEdge - 5, keVEdge + 5);
            }

        }

        /// <summary>
        /// Disconnect the DPP.
        /// </summary>
        public void Disconnect()
        {
            stopper = true;
            Stop();
            Clear();
            cmbDevices.Enabled = true;
            lblCommStatus.Text = "Disconnected";
            btnDisconnect.Hide();
            btnConnect.Show();
            tmrUpdate.Stop();
            dpp?.Disconnect();
        }

        /// <summary>
        /// Load saved detector and calibration settings.
        /// </summary>
        /// <param name="detectorSettingsFile">Saved detector settings (.dxp file format)</param>
        /// <param name="calibrationSettingsFile">Saved calibration settings (.txt file format)</param>
        public void LoadSettings(string detectorSettingsFile, string calibrationSettingsFile)
        {
            if (detectorSettingsFile != null && File.Exists(detectorSettingsFile))
            {
                MXDPP50_Settings dppSettings;
                switch (Path.GetExtension(detectorSettingsFile).ToLower())
                {
                    case ".dxp":
                        MXDPP50_Presets dppPresets;
                        MXDPP50_Hardware dppHardware;
                        dpp.FileReadLegacy(detectorSettingsFile, out dppSettings, out dppPresets, out dppHardware);
                        dppPresets.Timer *= 1000;
                        UpdateForm_Settings(dppSettings);
                        UpdateForm_Presets(dppPresets);
                        break;
                    case ".json":
                        dppSettings = dpp.FileReadSettingsJSON(detectorSettingsFile);
                        UpdateForm_Settings(dppSettings);
                        break;
                }
                WriteSettings();
            }

            if (calibrationSettingsFile != null && File.Exists(calibrationSettingsFile))
            {
                try
                {
                    calibrationFile = calibrationSettingsFile;
                    string[] parameters = File.ReadAllText(calibrationSettingsFile).Split('\t');
                    calibrationParameters = new Tuple<double, double>(double.Parse(parameters[0]), double.Parse(parameters[1]));
                    if (!spmGraph.IsDisposed)
                    {
                        spmGraph.ClearGraph();
                        spmGraph.xAxis = "keV";
                        spmGraph.SetXRange(0, 60);
                    }
                }
                catch
                {
                    MessageBox.Show($"The file { calibrationSettingsFile } is not a compatible calibration file.");
                }
            }
        }

        /// <summary>
        /// Calibrate the detector using Fe55 and Cd109 radioactive sources.
        /// </summary>
        /// <param name="fe55ExposureTime_sec">Time to acquire Fe55 spectrum.</param>
        /// <param name="cd109ExposureTime_sec">Time to acquire Cd109 spectrum.</param>
        public void CalibrateDetector_Fe55_Cd109(int fe55ExposureTime_sec, int cd109ExposureTime_sec, bool autoSwitch = false)
        {
            double _setTime = setTime;
            bremstrahlungEV = 0;
            cd109ExposureTime_sec = (cd109ExposureTime_sec < fe55ExposureTime_sec) ? fe55ExposureTime_sec + 400 : cd109ExposureTime_sec;
            setTime = fe55ExposureTime_sec + cd109ExposureTime_sec;
            Clear();
            if (!autoSwitch) MessageBox.Show("Remove the pinhole and set the Fe55 source");
            Start();
            Thread.Sleep(500);
            Stopwatch t = new Stopwatch();
            t.Start();
            while (t.ElapsedMilliseconds < fe55ExposureTime_sec * 1000) Application.DoEvents();
            t.Stop();
            if (!autoSwitch) MessageBox.Show("Replace Fe55 source with the Cd109 source.");
            while (realTime < (fe55ExposureTime_sec + cd109ExposureTime_sec) && acquiring) Application.DoEvents();

            MessageBox.Show("Select edges for low and high peaks, then press Save Calibration.");
            setTime = _setTime;
        }

        /// <summary>
        /// Save the current calibration settings for the detector to text file.
        /// </summary>
        private void SaveDetCalibration(int lowPeakLowEdge, int lowPeakHighEdge, int highPeakLowEdge, int highPeakHighEdge)
        {
            int error = 0;
            double _slope, _intercept;
            unsafe
            {
                double* pSlope, pIntercept;
                pSlope = &_slope;
                pIntercept = &_intercept;
                int[] counts_i = new int[counts.Length];
                for (int i = 0; i < counts_i.Length; i++)
                {
                    counts_i[i] = (int)counts[i];
                }
                error = CalibrateDetector(counts_i, counts.Length, lowPeakLowEdge, lowPeakHighEdge, highPeakLowEdge, highPeakHighEdge, 5895, 24942, pSlope, pIntercept);
            }
            calibrationParameters = new Tuple<double, double>(_slope, _intercept);
            if (error == 0) File.WriteAllText(calibrationFile, calibrationParameters.Item1.ToString() + "\t" + calibrationParameters.Item2.ToString());
            else error = -1;

            if (error == -1) MessageBox.Show("Insufficient counts in spectrum for accurate calibration. Try again.");
            else if (error > 0) MessageBox.Show("Calibration error code: " + error.ToString());
            else MessageBox.Show("Detector has been calibrated. Set the pinhole in place before you resume testing.");
        }

        /// <summary>
        /// Read current DPP settings.
        /// </summary>
        private async void ReadSettings()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();

                var dppSettings = await dpp.ReadSettingsAsync();
                UpdateForm_Settings(dppSettings);

                var dppPresets = await dpp.ReadPresetsAsync();
                UpdateForm_Presets(dppPresets);

            }
            catch
            {
            }
        }

        /// <summary>
        /// Write loaded settings to DPP.
        /// </summary>
        private async void WriteSettings()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();

                var dppSettings = GetForm_Settings();
                dppSettings = await dpp.WriteSettingsAsync(dppSettings);
                UpdateForm_Settings(dppSettings);

                var dppPresets = GetForm_Presets();
                dppPresets = await dpp.WritePresetsAsync(dppPresets);
                UpdateForm_Presets(dppPresets);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Read DPP hardware settings.
        /// </summary>
        /// <param name="vUpdateSet"></param>
        private async void ReadHardware(bool vUpdateSet = false)
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();

                var vSW = new Stopwatch();

                vSW.Start();
                var vRet = await dpp.ReadHardwareAsync();
                vSW.Stop();

                Console.Write("ReadHardwareAsync - " + vSW.ElapsedMilliseconds.ToString() + Environment.NewLine);

                UpdateForm_HardwareMon(vRet);
                if (vUpdateSet) UpdateForm_HardwareSet(vRet);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Write loaded hardware settings to DPP.
        /// </summary>
        private async void WriteHardware()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();

                var dppHardware = GetForm_Hardware();
                var vRet = await dpp.WriteHardwareAsync(dppHardware);
                UpdateForm_HardwareSet(vRet);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get current DPP hardware settings, presets and statistics.
        /// </summary>
        private async void Initialize()
        {
            cmbDevices.Enabled = false;
            lblVer.Text = dpp.FWVersion;
            lblDescription.Text = dpp.Description;
            lblCommStatus.Text = "Connected";
            btnConnect.Hide();
            btnDisconnect.Show();

            try
            {
                var dppSettings = await dpp.ReadSettingsAsync();
                UpdateForm_Settings(dppSettings);

                var dppPresets = await dpp.ReadPresetsAsync();
                UpdateForm_Presets(dppPresets);

                var dppHardware = await dpp.ReadHardwareAsync();
                UpdateForm_HardwareSet(dppHardware);
                UpdateForm_HardwareMon(dppHardware);

                var dppStats = await dpp.ReadStatisticsAsync();
                UpdateForm_Statistics(dppStats);
            }
            catch
            {

            }

            tmrUpdate.Start();
            groupBox6.Enabled = connected;

        }

        /// <summary>
        /// Write current settings, hardware and presets to DPP memory.
        /// </summary>
        private async void WriteDefaultsToMemory()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();

                var dppSettings = GetForm_Settings();
                var dppHardware = GetForm_Hardware();
                var dppPresets = GetForm_Presets();

                int vRet = await dpp.WriteDefaultsToMemoryAsync(dppSettings, dppHardware, dppPresets);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Write current presets to DPP memory.
        /// </summary>
        private void WritePresets()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();

                var dppPresets = GetForm_Presets();
                var vRet = dpp.WritePresets(dppPresets);
                UpdateForm_Presets(vRet);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Delay code until dpp is done acquiring.
        /// </summary>
        public void GetSpectrum()
        {
            Clear();

            while (!acquiring && !stopper)
            {
                Start();
                Application.DoEvents();
            }
            while (acquiring && !stopper) Application.DoEvents();
        }

        /// <summary>
        /// Start the acquisition.
        /// </summary>
        public void Start()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();
                if (acquiring) Stop();
                else
                {
                    if (realTime < setTime)
                    {
                        dpp.AcquisitionContinue();
                    }
                    else
                    {
                        Clear();
                        dpp.AcquisitionStart();
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Stop the current acquisition.
        /// </summary>
        public void Stop()
        {
            if (acquiring)
            {
                try
                {
                    while (dpp.Busy) Application.DoEvents();
                    dpp.AcquisitionStop();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Clear the acquired spectrum and statistics from memory.
        /// </summary>
        public void Clear()
        {
            try
            {
                while (dpp.Busy) Application.DoEvents();
                dpp.AcquisitionClear();
                realTime = 0;
                liveTime = 0;
                runCounter = 0;
                runTimeDead = 0;
                rateInput = 0;
                rateOutput = 0;
                rateCorrected = 0;
                rateSlow = 0;
                rateFast2 = 0;
                rateFast3 = 0;
                keVEdge = 0;
                for (int i = 0; i < counts.Length; i++)
                {
                    counts[i] = 0;
                }
                for (int i = 0; i < roi.Length; i++)
                {
                    roi[i] = 0;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Convert an array of channel numbers to calibrated keV values.
        /// </summary>
        /// <param name="channels">Channel array to convert</param>
        /// <returns></returns>
        public double[] Convert_ChannelTokeV(int[] channels)
        {
            double[] keVArray = new double[channels.Length];
            for (int i = 0; i < keVArray.Length; i++)
            {
                keVArray[i] = ((channels[i] * calibrationParameters.Item1) + calibrationParameters.Item2) / 1000;
            }
            return keVArray;
        }

        /// <summary>
        /// Convert a single eV value to channel number.
        /// </summary>
        /// <param name="eV">eV value to convert</param>
        /// <returns></returns>
        public int Convert_eVToChannel(double eV)
        {
            return (int)((eV - calibrationParameters.Item2) / calibrationParameters.Item1);
        }

        /// <summary>
        /// Get setting values from form.
        /// </summary>
        /// <returns></returns>
        private MXDPP50_Settings GetForm_Settings()
        {
            MXDPP50_Settings dppSettings = new MXDPP50_Settings(Convert.ToDouble(txtEqSlope.Text), Convert.ToDouble(txtEqOffset.Text));

            dppSettings.ClockSpeed = Convert.ToDouble(txtDPP_ClockSpeed.Text);
            dppSettings.PeakTimeSL = Convert.ToDouble(txtDPP_PeakTimeSL.Text);
            dppSettings.PeakTimeF2 = Convert.ToDouble(txtDPP_PeakTimeF2.Text);
            dppSettings.PeakTimeF3 = Convert.ToDouble(txtDPP_PeakTimeF3.Text);
            dppSettings.HoldTimeSL = Convert.ToDouble(txtDPP_HoldTimeSL.Text);
            dppSettings.HoldTimeF2 = Convert.ToDouble(txtDPP_HoldTimeF2.Text);
            dppSettings.HoldTimeF3 = Convert.ToDouble(txtDPP_HoldTimeF3.Text);
            dppSettings.DGainSL = Convert.ToDouble(txtDPP_DGainSL.Text);
            dppSettings.DGainF2 = Convert.ToDouble(txtDPP_DGainF2.Text);
            dppSettings.DGainF3 = Convert.ToDouble(txtDPP_DGainF3.Text);
            dppSettings.ThreshSL = Convert.ToInt32(txtDPP_ThresholdSL.Text);
            dppSettings.ThreshF2 = Convert.ToInt32(txtDPP_ThresholdF2.Text);
            dppSettings.ThreshF3 = Convert.ToInt32(txtDPP_ThresholdF3.Text);

            dppSettings.PPTC = Convert.ToDouble(txtDPP_PPTC.Text);
            dppSettings.PPGain = Convert.ToDouble(txtDPP_PPGain.Text);
            dppSettings.EQFactor = Convert.ToInt32(txtDPP_EQFactor.Text);
            dppSettings.ZeroFactor = Convert.ToInt32(txtDPP_ZeroFactor.Text);
            dppSettings.RInhibit = Convert.ToDouble(txtDPP_RInhibit.Text);
            dppSettings.BLRMode = Convert.ToInt32(txtDPP_BLRMode.Text);
            dppSettings.BLRWindow = Convert.ToInt32(txtDPP_BLRWindow.Text);
            dppSettings.DTLength = Convert.ToInt32(txtDPP_DTLength.Text);
            dppSettings.ChnOffset = Convert.ToInt32(txtDPP_CHNOffset.Text);
            dppSettings.RInterval = Convert.ToDouble(txtDPP_RInterval.Text);

            dppSettings.SCAMode = (eMXDPP50_SCAMode)Convert.ToInt32(rdbSCA_Rate.Checked);
            dppSettings.SCALo1 = Convert.ToInt32(txtSCALo1.Text);
            dppSettings.SCALo2 = Convert.ToInt32(txtSCALo2.Text);
            dppSettings.SCALo3 = Convert.ToInt32(txtSCALo3.Text);
            dppSettings.SCALo4 = Convert.ToInt32(txtSCALo4.Text);
            dppSettings.SCALo5 = Convert.ToInt32(txtSCALo5.Text);
            dppSettings.SCALo6 = Convert.ToInt32(txtSCALo6.Text);
            dppSettings.SCALo7 = Convert.ToInt32(txtSCALo7.Text);
            dppSettings.SCALo8 = Convert.ToInt32(txtSCALo8.Text);
            dppSettings.SCAHi1 = Convert.ToInt32(txtSCAHi1.Text);
            dppSettings.SCAHi2 = Convert.ToInt32(txtSCAHi2.Text);
            dppSettings.SCAHi3 = Convert.ToInt32(txtSCAHi3.Text);
            dppSettings.SCAHi4 = Convert.ToInt32(txtSCAHi4.Text);
            dppSettings.SCAHi5 = Convert.ToInt32(txtSCAHi5.Text);
            dppSettings.SCAHi6 = Convert.ToInt32(txtSCAHi6.Text);
            dppSettings.SCAHi7 = Convert.ToInt32(txtSCAHi7.Text);
            dppSettings.SCAHi8 = Convert.ToInt32(txtSCAHi8.Text);

            return dppSettings;
        }

        /// <summary>
        /// Get hardware values from form.
        /// </summary>
        /// <returns></returns>
        private MXDPP50_Hardware GetForm_Hardware()
        {
            MXDPP50_Hardware dppHardware = new MXDPP50_Hardware(Convert.ToDouble(txtTCSupply.Text), Convert.ToDouble(txtTCPullup.Text));

            if (rdbHVPos.Checked) dppHardware.HVPol = eMXDPP50_Polarity.POS;
            else dppHardware.HVPol = eMXDPP50_Polarity.NEG;

            dppHardware.HVSet = Convert.ToDouble(txtHVSet.Text);

            if (rdbTCBox.Checked) dppHardware.TCMode = eMXDPP50_TCMode.BOX;
            else dppHardware.TCMode = eMXDPP50_TCMode.DET;

            dppHardware.TCSet = Convert.ToDouble(txtTCSet.Text);

            if (rdbSigPos.Checked) dppHardware.SigPol = eMXDPP50_Polarity.POS;
            else dppHardware.SigPol = eMXDPP50_Polarity.NEG;

            if (rdbAux1Lo.Checked) dppHardware.AuxOut1 = 0;
            else dppHardware.AuxOut1 = 1;

            if (rdbAux2Lo.Checked) dppHardware.AuxOut2 = 0;
            else dppHardware.AuxOut2 = 1;

            switch (cmbAnalogOut.SelectedItem.ToString())
            {
                case "Slow Filter": dppHardware.AnalogOut = eMXDPP50_DACMode.FilterSlow; break;
                case "Fast2 Filter": dppHardware.AnalogOut = eMXDPP50_DACMode.FilterFast2; break;
                case "Fast3 Filter": dppHardware.AnalogOut = eMXDPP50_DACMode.FilterFast3; break;
            }

            return dppHardware;
        }

        /// <summary>
        /// Get preset values from form.
        /// </summary>
        /// <returns></returns>
        private MXDPP50_Presets GetForm_Presets()
        {
            MXDPP50_Presets dppPresets = new MXDPP50_Presets();

            if (rdbPresetTimeReal.Checked) dppPresets.TimerMode = eMXDPP50_TimerMode.RealTime;
            else dppPresets.TimerMode = eMXDPP50_TimerMode.LiveTime;
            dppPresets.Timer = Convert.ToUInt32(txtPresetTime.Text) * 1000;
            dppPresets.TotalCounts = Convert.ToUInt32(txtPresetTotal.Text);
            dppPresets.PeakCounts = Convert.ToUInt32(txtPresetPeak.Text);
            dppPresets.ROICounts = Convert.ToUInt32(txtPresetROI.Text);
            dppPresets.ROILoCHN = Convert.ToInt32(txtROILo.Text);
            dppPresets.ROIHiCHN = Convert.ToInt32(txtROIHi.Text);

            if (!chkPresetTime.Checked) dppPresets.Timer = UInt32.MaxValue;
            if (!chkPresetTotal.Checked) dppPresets.TotalCounts = UInt32.MaxValue;
            if (!chkPresetPeak.Checked) dppPresets.PeakCounts = UInt32.MaxValue;
            if (!chkPresetROI.Checked)
            {
                dppPresets.ROICounts = UInt32.MaxValue;
                dppPresets.ROILoCHN = 0;
                dppPresets.ROIHiCHN = 4095;
            }

            return dppPresets;
        }

        /// <summary>
        /// Populate form values from DPP settings
        /// </summary>
        /// <param name="settings">DPP settings</param>
        private void UpdateForm_Settings(MXDPP50_Settings settings)
        {
            foreach (TextBox t in groupBox1.Controls.OfType<GroupBox>().SelectMany(x => x.Controls.OfType<TextBox>()))
                t.BackColor = Color.White;

            txtEqSlope.Text = settings.EQSlope.ToString("0.000");
            txtEqOffset.Text = settings.EQOffset.ToString("0.000");

            txtDPP_PeakTimeSL.Text = settings.PeakTimeSL.ToString("0.000");
            txtDPP_PeakTimeF2.Text = settings.PeakTimeF2.ToString("0.000");
            txtDPP_PeakTimeF3.Text = settings.PeakTimeF3.ToString("0.000");
            txtDPP_HoldTimeSL.Text = settings.HoldTimeSL.ToString("0.000");
            txtDPP_HoldTimeF2.Text = settings.HoldTimeF2.ToString("0.000");
            txtDPP_HoldTimeF3.Text = settings.HoldTimeF3.ToString("0.000");
            txtDPP_DGainSL.Text = settings.DGainSL.ToString("0.000");
            txtDPP_DGainF2.Text = settings.DGainF2.ToString("0.000");
            txtDPP_DGainF3.Text = settings.DGainF3.ToString("0.000");
            txtDPP_ThresholdSL.Text = settings.ThreshSL.ToString();
            txtDPP_ThresholdF2.Text = settings.ThreshF2.ToString();
            txtDPP_ThresholdF3.Text = settings.ThreshF3.ToString();

            txtDPP_ClockSpeed.Text = settings.ClockSpeed.ToString();
            txtDPP_PPTC.Text = settings.PPTC.ToString();
            txtDPP_PPGain.Text = settings.PPGain.ToString("0.0000");
            txtDPP_EQFactor.Text = settings.EQFactor.ToString();
            txtDPP_ZeroFactor.Text = settings.ZeroFactor.ToString();
            txtDPP_RInhibit.Text = settings.RInhibit.ToString("0.000");
            txtDPP_BLRMode.Text = settings.BLRMode.ToString();
            txtDPP_BLRWindow.Text = settings.BLRWindow.ToString();
            txtDPP_DTLength.Text = settings.DTLength.ToString();
            txtDPP_CHNOffset.Text = settings.ChnOffset.ToString();
            txtDPP_RInterval.Text = settings.RInterval.ToString("0.000");

            if (settings.SCAMode == eMXDPP50_SCAMode.Pulse) rdbSCA_Pulse.Checked = true;
            else rdbSCA_Rate.Checked = true;
            txtSCALo1.Text = settings.SCALo1.ToString();
            txtSCALo2.Text = settings.SCALo2.ToString();
            txtSCALo3.Text = settings.SCALo3.ToString();
            txtSCALo4.Text = settings.SCALo4.ToString();
            txtSCALo5.Text = settings.SCALo5.ToString();
            txtSCALo6.Text = settings.SCALo6.ToString();
            txtSCALo7.Text = settings.SCALo7.ToString();
            txtSCALo8.Text = settings.SCALo8.ToString();
            txtSCAHi1.Text = settings.SCAHi1.ToString();
            txtSCAHi2.Text = settings.SCAHi2.ToString();
            txtSCAHi3.Text = settings.SCAHi3.ToString();
            txtSCAHi4.Text = settings.SCAHi4.ToString();
            txtSCAHi5.Text = settings.SCAHi5.ToString();
            txtSCAHi6.Text = settings.SCAHi6.ToString();
            txtSCAHi7.Text = settings.SCAHi7.ToString();
            txtSCAHi8.Text = settings.SCAHi8.ToString();
        }

        /// <summary>
        /// Populate form values from DPP statistics
        /// </summary>
        /// <param name="stats">DPP stats</param>
        private void UpdateForm_Statistics(MXDPP50_Statistics stats)
        {
            lblTimeReal.Text = realTime.ToString("0.0");
            lblTimeLive.Text = liveTime.ToString("0.0");
            lblTimeDead.Text = deadTime.ToString("0.0") + " %";
            lblRateInput.Text = stats.RateInput.ToString();
            lblRateOutput.Text = stats.RateOutput.ToString();
            lblRateCorrected.Text = stats.RateCorrected.ToString();
            lblRateSlow.Text = stats.RateSlow.ToString();
            lblRateFast2.Text = stats.RateFast2.ToString();
            lblRateFast3.Text = stats.RateFast3.ToString();

            if (acquiring)
            {
                btnStart.BackColor = Color.LightCoral;
                btnStart.Text = "Stop";
                lblStatus.Text = "Running...";
            }
            else
            {
                btnStart.BackColor = Color.MediumSeaGreen;
                btnStart.Text = "Start";
                lblStatus.Text = "Idle...";
            }
        }

        /// <summary>
        /// Populate form values from DPP hardware monitors.
        /// </summary>
        /// <param name="hardware">DPP hardware</param>
        private void UpdateForm_HardwareMon(MXDPP50_Hardware hardware)
        {
            switch (hardware.HVPol)
            {
                case eMXDPP50_Polarity.POS: lblHVPol.Text = "POS"; break;
                case eMXDPP50_Polarity.NEG: lblHVPol.Text = "NEG"; break;
            }
            lblHVSet.Text = hardware.HVSet.ToString("0.0");
            lblHVMon.Text = hardware.HVMon.ToString("0.0") + " V";
            if (hardware.HVPol == eMXDPP50_Polarity.NEG)
            {
                lblHVSet.Text = "-" + lblHVSet.Text;
                lblHVMon.Text = "-" + lblHVMon.Text;
            }

            switch (hardware.TCMode)
            {
                case eMXDPP50_TCMode.BOX: lblTCMode.Text = "BOX"; break;
                case eMXDPP50_TCMode.DET: lblTCMode.Text = "DET"; break;
            }
            lblTCSet.Text = hardware.TCSet.ToString("0.0") + "°C";
            lblTCMon.Text = hardware.TCMon.ToString("0.0") + "°C";
            lblTCTecMon.Text = hardware.TCTecMon.ToString("0.00") + "V";

            switch (hardware.TCReady)
            {
                case true: lblTCReady.Text = "Ready"; break;
                case false: lblTCReady.Text = "Not Ready"; break;
            }
            lblDPPTemp.Text = hardware.DPPTemp.ToString("0.0") + "°C";

            switch (hardware.SigPol)
            {
                case eMXDPP50_Polarity.POS: lblSignalPolarity.Text = "POS"; break;
                case eMXDPP50_Polarity.NEG: lblSignalPolarity.Text = "NEG"; break;
            }

            lblAuxIn1.Text = hardware.AuxIn1.ToString();
            lblAuxIn2.Text = hardware.AuxIn2.ToString();
            lblAuxOut1.Text = hardware.AuxOut1.ToString();
            lblAuxOut2.Text = hardware.AuxOut2.ToString();

            switch (hardware.AnalogOut)
            {
                case eMXDPP50_DACMode.FilterSlow: lblAnalogOut.Text = "Slow Filter"; break;
                case eMXDPP50_DACMode.FilterFast2: lblAnalogOut.Text = "Fast2 Filter"; break;
                case eMXDPP50_DACMode.FilterFast3: lblAnalogOut.Text = "Fast3 Filter"; break;
                default: lblAnalogOut.Text = "Unknown"; break;
            }
        }

        /// <summary>
        /// Populate form values from DPP hardware settings.
        /// </summary>
        /// <param name="hardware">DPP hardware</param>
        private void UpdateForm_HardwareSet(MXDPP50_Hardware hardware)
        {
            foreach (TextBox t in gbDetMonitor.Controls.OfType<TextBox>())
                t.BackColor = Color.White;

            switch (hardware.HVPol)
            {
                case eMXDPP50_Polarity.POS: rdbHVPos.Checked = true; break;
                case eMXDPP50_Polarity.NEG: rdbHVNeg.Checked = true; break;
            }
            txtHVSet.Text = hardware.HVSet.ToString("0.0");

            switch (hardware.TCMode)
            {
                case eMXDPP50_TCMode.BOX: rdbTCBox.Checked = true; break;
                case eMXDPP50_TCMode.DET: rdbTCDet.Checked = true; break;
            }
            txtTCSet.Text = hardware.TCSet.ToString("0.0");
            txtTCPullup.Text = hardware.TCPullup.ToString();
            txtTCSupply.Text = hardware.TCSupply.ToString("0.0");

            switch (hardware.SigPol)
            {
                case eMXDPP50_Polarity.POS: rdbSigPos.Checked = true; break;
                case eMXDPP50_Polarity.NEG: rdbSigNeg.Checked = true; break;
            }

            switch (hardware.AuxOut1)
            {
                case 0: rdbAux1Lo.Checked = true; break;
                case 1: rdbAux1Hi.Checked = true; break;
            }

            switch (hardware.AuxOut2)
            {
                case 0: rdbAux2Lo.Checked = true; break;
                case 1: rdbAux2Hi.Checked = true; break;
            }

            switch (hardware.AnalogOut)
            {
                case eMXDPP50_DACMode.FilterSlow: cmbAnalogOut.SelectedItem = "Slow Filter"; break;
                case eMXDPP50_DACMode.FilterFast2: cmbAnalogOut.SelectedItem = "Fast2 Filter"; break;
                case eMXDPP50_DACMode.FilterFast3: cmbAnalogOut.SelectedItem = "Fast3 Filter"; break;
                default: cmbAnalogOut.SelectedItem = "Slow Filter"; break;
            }

        }

        /// <summary>
        /// Populate form values from DPP presets.
        /// </summary>
        /// <param name="hardware">DPP presets</param>
        private void UpdateForm_Presets(MXDPP50_Presets presets)
        {
            foreach (TextBox t in groupBox3.Controls.OfType<TextBox>())
                t.BackColor = Color.White;

            if (presets.TimerMode == eMXDPP50_TimerMode.RealTime) rdbPresetTimeReal.Checked = true;
            else rdbPresetTimeLive.Checked = true;
            txtPresetTime.Text = (presets.Timer / 1000).ToString();
            txtPresetTotal.Text = presets.TotalCounts.ToString();
            txtPresetPeak.Text = presets.PeakCounts.ToString();
            txtPresetROI.Text = presets.ROICounts.ToString();
            txtROILo.Text = presets.ROILoCHN.ToString();
            txtROIHi.Text = presets.ROIHiCHN.ToString();

            if (presets.Timer == UInt32.MaxValue) chkPresetTime.Checked = false;
            else chkPresetTime.Checked = true;
            if (presets.TotalCounts == UInt32.MaxValue) chkPresetTotal.Checked = false;
            else chkPresetTotal.Checked = true;
            if (presets.PeakCounts == UInt32.MaxValue) chkPresetPeak.Checked = false;
            else chkPresetPeak.Checked = true;
            if (presets.ROICounts == UInt32.MaxValue) chkPresetROI.Checked = false;
            else chkPresetROI.Checked = true;
        }

        /// <summary>
        /// Convert an array of type T to type double.
        /// </summary>
        /// <typeparam name="T">Type of input array</typeparam>
        /// <param name="data">Input array</param>
        /// <returns></returns>
        public double[] Convert_ToDoubleArray<T>(T[] data)
        {
            double[] d = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                d[i] = Convert.ToDouble(data[i]);
            }
            return d;
        }

        #endregion

        #region Events


        private void GraphUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form f = sender as Form;
            f.Hide();
        }

        private void OnDispose(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void btnReadSettings_Click(object sender, EventArgs e)
        {
            ReadSettings();
        }

        private void btnWriteSettings_Click(object sender, EventArgs e)
        {
            WriteSettings();
            WritePresets();
        }

        private void btnReadHardware_Click(object sender, EventArgs e)
        {
            ReadHardware();
        }

        private void btnWriteHardware_Click(object sender, EventArgs e)
        {
            WriteHardware();
        }

        private void btnSaveDefaults_Click(object sender, EventArgs e)
        {
            WriteDefaultsToMemory();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            AutoConnect(cmbDevices.SelectedItem.ToString());
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void btnPresetOpen_Click(object sender, EventArgs e)
        {
            diaOpenFile.Filter = "JSON files (*.json)|*.json";
            diaOpenFile.FileName = null;
            var vRet = diaOpenFile.ShowDialog();

            if (vRet == DialogResult.OK)
            {
                var dppPresets = dpp.FileReadPresetsJSON(diaOpenFile.FileName);
                UpdateForm_Presets(dppPresets);
            }
        }

        private void btnPresetSave_Click(object sender, EventArgs e)
        {
            diaSaveFile.Filter = "JSON files (*.json)|*.json";
            diaSaveFile.FileName = null;
            var vRet = diaSaveFile.ShowDialog();

            if (vRet == DialogResult.OK)
            {
                var dppPresets = GetForm_Presets();
                dpp.FileWritePresetsJSON(diaSaveFile.FileName, dppPresets);
                UpdateForm_Presets(dppPresets);
            }
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            diaSaveFile.Filter = "DXP files (*.dxp)|*.dxp|JSON files (*.json)|*.json";
            diaSaveFile.FileName = null;
            var vRet = diaSaveFile.ShowDialog();

            if (vRet == DialogResult.OK)
            {
                var dppSettings = GetForm_Settings();
                switch (System.IO.Path.GetExtension(diaSaveFile.FileName).ToLower())
                {
                    case ".dxp":
                        var dppPresets = GetForm_Presets();
                        var dppHardware = GetForm_Hardware();
                        dppPresets.Timer /= 1000;
                        dpp.FileWriteLegacy(diaSaveFile.FileName, dppSettings, dppPresets, dppHardware);
                        dppPresets.Timer *= 1000;
                        UpdateForm_Settings(dppSettings);
                        UpdateForm_Presets(dppPresets);
                        break;
                    case ".json":
                        dpp.FileWriteSettingsJSON(diaOpenFile.FileName, dppSettings);
                        UpdateForm_Settings(dppSettings);
                        break;
                }
            }
        }

        public void btnSettingsOpen_Click(object sender, EventArgs e)
        {
            diaOpenFile.Filter = "DXP files (*.dxp)|*.dxp|JSON files (*.json)|*.json";
            diaOpenFile.FileName = null;
            if (diaOpenFile.ShowDialog() == DialogResult.OK) LoadSettings(diaOpenFile.FileName, null);
        }

        private void btnFileHardwareOpen_Click(object sender, EventArgs e)
        {
            diaOpenFile.Filter = "JSON files (*.json)|*.json";
            diaOpenFile.FileName = null;
            var vRet = diaOpenFile.ShowDialog();

            if (vRet == DialogResult.OK)
            {
                var dppHardware = dpp.FileReadHardwareJSON(diaOpenFile.FileName);
                UpdateForm_HardwareSet(dppHardware);
            }
        }

        private void btnFileHardwareSave_Click(object sender, EventArgs e)
        {
            diaSaveFile.Filter = "JSON files (*.json)|*.json";
            diaSaveFile.FileName = null;
            var vRet = diaSaveFile.ShowDialog();

            if (vRet == DialogResult.OK)
            {
                var dppHardware = GetForm_Hardware();
                dpp.FileWriteHardwareJSON(diaSaveFile.FileName, dppHardware);
                UpdateForm_HardwareSet(dppHardware);
            }
        }

        private void chkPresetTime_CheckedChanged(object sender, EventArgs e)
        {
            txtPresetTime.Enabled = chkPresetTime.Checked;
        }

        private void chkPresetTotal_CheckedChanged(object sender, EventArgs e)
        {
            txtPresetTotal.Enabled = chkPresetTotal.Checked;
        }

        private void chkPresetPeak_CheckedChanged(object sender, EventArgs e)
        {
            txtPresetPeak.Enabled = chkPresetPeak.Checked;
        }

        private void chkPresetROI_CheckedChanged(object sender, EventArgs e)
        {
            txtPresetROI.Enabled = chkPresetROI.Checked;
            txtROILo.Enabled = chkPresetROI.Checked;
            txtROIHi.Enabled = chkPresetROI.Checked;
        }

        private void cmbAnalogOut_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WriteHardware();
        }

        private void btnSpectrum_Click(object sender, EventArgs e)
        {
            if (!frmSpec.Visible) frmSpec.Show();
            else frmSpec.Hide();
        }

        private void btnkeVEdge_Click(object sender, EventArgs e)
        {
            if (!frmkeVEdge.Visible) frmkeVEdge.Show();
            else frmkeVEdge.Hide();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            spmGraph.ClearGraph();
            kevGraph.ClearGraph();
        }

        private void numkeV_ValueChanged(object sender, EventArgs e)
        {
            bremstrahlungEV = (uint)(numkeV.Value * 1000);
        }

        private void lnkCalFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            diaOpenFile.Filter = "Text files (*.txt)|*.txt";
            diaOpenFile.FileName = null;
            var vRet = diaOpenFile.ShowDialog();

            if (vRet == DialogResult.OK)
            {
                string fileName = diaOpenFile.FileName;
                lnkCalFile.Text = fileName;
                LoadSettings(null, fileName);
            }
        }

        private void txtPresetTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    setTime = double.Parse(txtPresetTime.Text);
                }
                catch { }
            }
        }

        private void txtDPPWritePresets_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!txt.ReadOnly)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    WritePresets();
                }
            }
        }

        private void txtDPPWriteSettings_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!txt.ReadOnly)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    WriteSettings();
                }
            }
        }

        private void txtDPPWriteHardware_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!txt.ReadOnly)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    WriteHardware();
                }
            }
        }

        private void txtDPP_TextChanged(object sender, EventArgs e)
        {
            TextBox t = sender as TextBox;
            t.BackColor = Color.LightYellow;
        }

        private void cmbDevices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbDevices.SelectedItem.ToString() != null) btnConnect.Enabled = true;
            else btnConnect.Enabled = false;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            ReadStats();
        }

        private void tmrKeVEdge_Tick(object sender, EventArgs e)
        {
            ReadkeVEdge();
        }

        #endregion
    }
}
