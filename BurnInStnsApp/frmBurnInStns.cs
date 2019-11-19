using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Helper;
using Devices;
using BurnInStns.Properties;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace BurnInStns
{
    /// <summary>
    /// User interface for testing stations.
    /// </summary>
    public partial class frmBurnInStns : Form
    {
        #region Fields


        frmResults res = new frmResults();
        frmSettings stg = new frmSettings();
        frmGraph gra = new frmGraph();
        frmMaintenance mnt = new frmMaintenance();
        frmPassword pas = new frmPassword();
        ctlStation selected;
        MXSocketServer vServer = new MXSocketServer();
        MX_ADSClient plc = new MX_ADSClient(Settings.Default.PLCRoute, Settings.Default.PLCPort);
        List<MX_PLCDevice> plcDevices = new List<MX_PLCDevice>();
        DBHelper db;
        Link barCodeListener = new Link(Settings.Default.BCScanPort);
        Log log = new Log($@"{ Settings.Default.SaveFolder }\{ Log.timeStamp }.txt", Settings.Default.LogSaveFreq);
        string settingsTable = Settings.Default.SettingsTable;
        string resultsTable = Settings.Default.ResultsTable;
        string equipStatTable = Settings.Default.EquipStatusTable;
        string stationStatTable = Settings.Default.BurnInStatusTable;
        string cabinetName = "BRN01";

        bool manualControl,
            testing,
            safe,
            error,
            xRays,
            eStopPressed,
            doorLocked,
            doorOpen,
            _hvEnable,
            _psEnable,
            powerLoss;

        #endregion

        #region Properties

        /// <summary>
        /// Get chamber temperature in C.
        /// </summary>
        public double chamberTemp
        {
            get
            {
                return plc.GetState(PLC.PLI_TEMP_CHB) / 10.0;
            }
        }

        /// <summary>
        /// Get cabinet temperature in C.
        /// </summary>
        public double cabinetTemp
        {
            get
            {
                return plc.GetState(PLC.PLI_TEMP_CAB) / 10.0;
            }
        }

        /// <summary>
        /// Get application version.
        /// </summary>
        public string appVersion
        {
            get
            {
                try
                {
                    return "Version " + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                catch
                {
                    return "Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
            }
        }

        /// <summary>
        /// Get or set HV enable status of power supplies.
        /// </summary>
        private bool hvEnable
        {
            get
            {
                return _hvEnable;
            }
            set
            {
                ResetSafety();
                plc.SetState(SAF.inPLC_TESTING, value);
                plc.SetState(SAF.inPLC_HV_EN, value);
                plc.SetState(SAF.inPLC_SP_EN, value);
                plc.SetState(PLC.PLO_FAN_CHB, value);
            }
        }

        /// <summary>
        /// Get or set power supply input power.
        /// </summary>
        private bool psEnable
        {
            get
            {
                return _psEnable;
            }
            set
            {
                plc.SetState(PLC.PLO_PS_PWR, value);
                plc.SetState(PLC.PLO_SPELLMAN1, value);
                plc.SetState(PLC.PLO_SPELLMAN2, value);
                plc.SetState(PLC.PLO_SPELLMAN3, value);
                plc.SetState(PLC.PLO_SPELLMAN4, value);
                plc.SetState(PLC.PLO_SPELLMAN5, value);
                plc.SetState(PLC.PLO_SPELLMAN6, value);
            }
        }

        /// <summary>
        /// Returns true if any stations are ready for testing.
        /// </summary>
        private bool anyReady
        {
            get
            {
                foreach (ctlStation s in pan.Controls)
                {
                    if (s.runTime.TotalSeconds > 0 && s.tub.status != Tube.Status.Done) return true;
                }
                return false;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Connect to all universal tube controllers (utc).
        /// </summary>
        private void Connect()
        {
            int nr = pan.RowCount;
            int nc = pan.ColumnCount;
            string nameFormat = $"UTC100-{ cabinetName }";
            string[] allUTCs = new string[nr * nc];
            string[] utcIP = new string[allUTCs.Length]; // IP addresses of each utc
            string[] spIP = new string[utcIP.Length]; // IP addresses of each spellman supply
            string[] utcQuery = Link.GetDeviceIP(Settings.Default.Domain + "255", Settings.Default.UDPServerPort, "UTC100" + "-" + cabinetName);
            string[] spQuery = Settings.Default.SpellmanIP.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < utcIP.Length; i++)
            {
                allUTCs[i] = $"{ nameFormat }-{ (i + 1).ToString("00") }";
                for (int j = 0; j < utcQuery.Length; j++)
                {
                    if (utcQuery[j].Contains(allUTCs[i]))
                    {
                        utcIP[i] = utcQuery[j].Split(',')[1]; // Get IP address of UTC (Name,IP)

                        // Check if there is a spellman supply
                        if (spQuery.Length > i && spQuery[i].Split(' ')[1] != "None") spIP[i] = spQuery[i].Split(' ')[1];
                    }
                }
            }
            for (int c = 0; c < nc; c++)
            {
                for (int r = 0; r < nr; r++)
                {
                    int i = (nc * r) + c;
                    if (i < utcIP.Length)
                    {
                        if (utcIP[i] != null && pan.GetControlFromPosition(c, r) == null)
                        {
                            ctlStation s = new ctlStation();
                            s.Name = allUTCs[i].Length > 7 ? allUTCs[i].Substring(7) : cabinetName;
                            if (spIP[i] == null)
                                s.Connect(utcIP[i]);
                            else
                                s.Connect($"{ utcIP[i] } { spIP[i] }");

                            if (!s.IsDisposed)
                            {
                                s.doneTesting += DoneTesting;
                                s.Dock = DockStyle.Fill;
                                pan.Controls.Add(s, c, r);
                                LogToConsole($"Found { s.Name } @ { utcIP[i] }"); // Print name and ip of device
                            }
                        }
                    }
                }
            }
            btnStart.Enabled = pan.Controls.Count > 0;
            List<string> plots = new List<string>();
            for (int r = 0; r < pan.RowCount; r++)
            {
                for (int c = 0; c < pan.ColumnCount; c++)
                {
                    ctlStation s = (ctlStation)pan.GetControlFromPosition(c, r);
                    if (s != null)
                    {
                        plots.Add($"{ s.Name } kV");
                        plots.Add($"{ s.Name } uA");
                    }
                }
            }
            gra = new frmGraph();
            gra.ctlGraph1.AddPlots(plots.ToArray());
            btnGraph.Enabled = true;
            db = new DBHelper();
            SetupStatusDB();
            tmrDbWrite.Start();
        }

        /// <summary>
        /// Select station from matrix.
        /// </summary>
        /// <param name="row">Row of matrix.</param>
        /// <param name="col">Column of matrix.</param>
        private string SelectStation(int row, int col)
        {
            selected = null;

            if (testing)
                return "Testing in progress, pause before selecting.";

            if (row > 0 && col > 0)
            {
                foreach (Control c in pan.Controls)
                {
                    c.BackColor = Color.Transparent;
                }
                var s = pan.GetControlFromPosition(col - 1, row - 1);
                if (s != null)
                {
                    selected = (ctlStation)s;
                    selected.BackColor = Color.SteelBlue;
                }
            }

            return selected != null ?
                $"Station: { selected.Name } was selected." : "Station not ready.";
        }

        /// <summary>
        /// Load tube settings for selected station.
        /// </summary>
        /// <param name="id">Serial ID of tube.</param>
        /// <returns></returns>
        private string InitStation(int id)
        {
            if (testing)
                return "Testing in progress, pause before scanning tube.";
            if (selected == null)
                return "Select a station first.";
            int found = selected.Setup(id);
            if (found == 2)
            {
                string tubeInfo = $"{ selected.Name } is ready to load.\r\n" +
                            $"\t\tSN: { selected.tub.serialNum }\r\n" +
                            $"\t\tName: { selected.tub.name }\r\n" +
                            $"\t\tPSType: { selected.tub.psType }\r\n" +
                            $"\t\tTUB: { selected.tub.tubNum }";
                return LogToConsole(tubeInfo);
            }
            else if (found == 1) return $"Tube { selected.tub.name } is not ready for burn-in. Add it to settings.";
            return "Tube serial number was not recognized.";
        }

        /// <summary>
        /// Create PLC device list.
        /// </summary>
        private void InitPLC()
        {
            plc.AddDevice(SAF.inPLC_ERRACK, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.inPLC_RESTART, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.inPLC_CHB_UNLCK, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.inPLC_HV_EN, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.inPLC_SP_EN, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.inPLC_TESTING, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.outPLC_RUN, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.outPLC_ERRCOM, MX_PLCDataTypes.BOOL);

            plc.AddDevice(SAF.SFI_CHB_UNLCK, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFI_CHB_OPEN, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFI_HV_PWR, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFI_EMO, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFI_XRAYS_1, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFI_XRAYS_2, MX_PLCDataTypes.BOOL);

            plc.AddDevice(SAF.SFO_CHB_UNLCK, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_HV_PWR, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_SP_INTLCK, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_BUZZER, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_LED_SAFE, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_LED_HV, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_LED_XRAYS, MX_PLCDataTypes.BOOL);
            plc.AddDevice(SAF.SFO_LED_TESTING, MX_PLCDataTypes.BOOL);

            plc.AddDevice(PLC.PLO_PS_PWR, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_LED_ERROR, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_LED_EMO, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_SPELLMAN1, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_SPELLMAN2, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_SPELLMAN3, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_SPELLMAN4, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_SPELLMAN5, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_SPELLMAN6, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLO_FAN_CHB, MX_PLCDataTypes.BOOL);

            plc.AddDevice(PLC.PLI_TEMP_CAB, MX_PLCDataTypes.INT);
            plc.AddDevice(PLC.PLI_TEMP_CHB, MX_PLCDataTypes.INT);

            plc.AddDevice(PLC.PLI_UPS_ALARM, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLI_UPS_BATTERY, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLI_UPS_CHARGE, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLI_PWR_TEMP, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLI_PWR_OUTPUT, MX_PLCDataTypes.BOOL);
            plc.AddDevice(PLC.PLI_PWR_OVP, MX_PLCDataTypes.BOOL);
        }

        /// <summary>
        /// Setup status to DB for external processing.
        /// </summary>
        private async void SetupStatusDB()
        {
            await Task.Run(() =>
            {
                db.Insert(equipStatTable, cabinetName, null, null, null, null, null, null, null, null, DateTime.Now);

                foreach (ctlStation s in pan.Controls)
                {
                    db.Insert(
                        stationStatTable, s.Name, DateTime.Now, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                }
            });
        }

        /// <summary>
        /// Update status to DB for external processing.
        /// </summary>
        private async void UpdateStatusDB()
        {
            await Task.Run(() =>
            {
                db.Update(equipStatTable, "LED_Safe", Convert.ToByte(safe), $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "LED_HV", Convert.ToByte(hvEnable), $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "LED_XRAYS", Convert.ToByte(xRays), $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "LED_Testing", Convert.ToByte(testing), $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "LED_Error", Convert.ToByte(error), $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "EStop", Convert.ToByte(eStopPressed), $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "ChamberTemp", chamberTemp, $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "CabinetTemp", cabinetTemp, $"EquipName like '{ cabinetName }'");
                db.Update(equipStatTable, "TimeStamp", DateTime.Now, $"EquipName like '{ cabinetName }'");

                foreach (ctlStation s in pan.Controls)
                {
                    db.Update(stationStatTable, "TimeStamp", DateTime.Now, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "Connected", Convert.ToByte(s.tub.connected), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "SerialNum", s.serialNum.ToString(), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "TubeName", s.tub.name, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "TubeStatus", s.tub.status.ToString(), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "HVStatus", Convert.ToByte(s.tub.on), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "SetKV", s.tub.setkV, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "SetUA", s.tub.setuA, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "MonKV", s.tub.monkV, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "MonUA", s.tub.monuA, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "Status", s.lblStatus.Text, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "Progress", s.progressBar.Value, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "ErrorReason", s.errReason, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "ProductImage", s.tub.productImageFilePath, $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "LoggingEnabled", Convert.ToByte(s.chkLog.Checked), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "LEDOnColor", s.ledStatus.OnColor.ToKnownColor().ToString(), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "LEDOffColor", s.ledStatus.OffColor.ToKnownColor().ToString(), $"StationName like '{ s.Name }'");
                    db.Update(stationStatTable, "LEDEnabled", Convert.ToByte(s.ledStatus.Value), $"StationName like '{ s.Name }'");
                }
            });
        }

        /// <summary>
        /// Setup results data table.
        /// </summary>
        private async void SetupResults()
        {
            await Task.Run(() =>
            {
                foreach (ctlStation s in pan.Controls)
                {
                    if (Settings.Default.SaveData && s.firstRun)
                    {
                        db.Insert(resultsTable, s.serialNum, s.testRecipe, DateTime.Now, DateTime.Now, s.Name, false, null, null, null);
                        s.testID = Convert.ToInt32(DBHelper.Query($"select MAX(TestID) from { resultsTable }").Rows[0][0]);
                        s.firstRun = false;
                    }
                }
            });
        }

        /// <summary>
        /// Update results data table.
        /// </summary>
        /// <param name="s">Station to update.</param>
        /// <param name="columns">Columns to update.</param>
        /// <param name="data">Data to write.</param>
        private void UpdateResults(ctlStation s)
        {
            if (s.testID != 0)
            {
                int errCount = 0;
                for (int i = 0; i < s.errorCount.Length; i++)
                {
                    errCount += s.errorCount[i];
                }
                db.Update(resultsTable, "Time_End", DateTime.Now, $"TestID = { s.testID }");
                db.Update(resultsTable, "PassFail", s.errReason == "", $"TestID = { s.testID }");
                db.Update(resultsTable, "ErrorMessage", s.errReason, $"TestID = { s.testID }");
                db.Update(resultsTable, "RunTime_hours", s.runTimer.Elapsed.TotalHours, $"TestID = { s.testID }");
                db.Update(resultsTable, "ErrorCount", errCount, $"TestID = { s.testID }");
            }
        }

        /// <summary>
        /// Start testing for all tubes.
        /// </summary>
        private string Start()
        {
            if (!anyReady)
                return LogToConsole("No stations are ready.");

            if (!eStopPressed && !doorOpen && doorLocked)
            {
                btnStart.Enabled = btnStop.Enabled = false;
                selected = null;
                hvEnable = true;

                foreach (ctlStation s in pan.Controls) s.Start();
                btnStart.Text = "PAUSE";
                btnStart.BackColor = Color.DarkBlue;
                btnStart.Enabled = btnStop.Enabled = true;
                SetupResults();
                return LogToConsole("All stations started.");
            }

            if (eStopPressed) return LogToConsole("Disengage emergency stop.");
            if (doorOpen) return LogToConsole("Close the chamber door.");
            return LogToConsole("Lock the chamber door.");
        }

        /// <summary>
        /// Pause testing on all tubes.
        /// </summary>
        private string Pause()
        {
            foreach (ctlStation s in pan.Controls) s.Pause();
            hvEnable = false;
            btnStart.Text = "RESUME";
            btnStart.BackColor = Color.MediumSeaGreen;
            return LogToConsole("All stations paused.");
        }

        /// <summary>
        /// Immediately abort testing on all tubes.
        /// </summary>
        private string Stop()
        {
            selected = null;
            hvEnable = false;
            btnStart.Text = "START";
            btnStart.BackColor = Color.MediumSeaGreen;
            btnStop.Enabled = false;

            foreach (ctlStation s in pan.Controls) s.Stop();

            return LogToConsole("All stations stopped.");
        }

        /// <summary>
        /// Lock or unlock test cabinet.
        /// </summary>
        private string LockUnlock()
        {
            if (doorOpen) return "Close chamber door first.";
            if (testing)
                return "Testing in progress, pause before unlocking.";
            plc.SetState(SAF.inPLC_CHB_UNLCK, doorLocked);
            string state = doorLocked ? "Chamber unlocked." : "Chamber locked.";
            return LogToConsole(state);
        }

        /// <summary>
        /// Reset safety system.
        /// </summary>
        private string ResetSafety()
        {
            plc.SetState(SAF.inPLC_ERRACK, true);
            plc.SetState(SAF.inPLC_RESTART, true);
            return "Safety system was reset.";
        }

        /// <summary>
        /// Regularily check if there are any stations still testing.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAllCompleted()
        {
            bool allDone = pan.Controls.Cast<ctlStation>().All(
                s => s.tub.status != Tube.Status.Testing && s.tub.status != Tube.Status.Paused);

            if (allDone)
            {
                LogToConsole("All stations have finished.");
                btnStart.Text = "START";
                btnStart.BackColor = Color.MediumSeaGreen;
                btnStop.Enabled = false;
                hvEnable = false;
            }
            return allDone;
        }

        /// <summary>
        /// View log in rich text box.
        /// </summary>
        /// <param name="items"></param>
        private string LogToConsole(string item)
        {
            if (richTextBox1.Lines.Length > 10000) richTextBox1.Clear();
            richTextBox1.Text = log.Write(DateTime.Now, item) + "\r\n" + richTextBox1.Text;
            richTextBox1.SelectionStart = 0;
            richTextBox1.ScrollToCaret();
            return item;
        }

        /// <summary>
        /// Get station control from it's number in the matrix.
        /// </summary>
        /// <param name="number">Number of station in matrix (starts at 1)</param>
        /// <returns></returns>
        public ctlStation GetStation(int number)
        {
            if (number > 0)
            {
                int nc = pan.ColumnCount;
                number--;
                int r = (number / nc);
                int c = number - (r * nc);
                return (ctlStation)pan.GetControlFromPosition(c, r);
            }
            return null;
        }

        #region Socket Messaging


        /// <summary>
        /// Gets all of the IPv4 addresses associated with this computer
        /// </summary>
        /// <returns>List of strings containing IP addresses</returns>
        private List<string> GetMyIPs()
        {
            List<string> vList = new List<string>();

            // Get host name
            String vHost = Dns.GetHostName();

            // Find host by name
            IPHostEntry vEntry = Dns.GetHostEntry(vHost);

            // Enumerate IP addresses
            foreach (IPAddress vIP in vEntry.AddressList)
            {
                if (vIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) vList.Add(vIP.ToString());
            }

            return vList;
        }

        /// <summary>
        /// Handles the PING command from the socket
        /// </summary>
        /// <param name="vMessage">Contains the Socket message that was received</param>
        /// <returns>Returns string that is then sent back to the client</returns>
        public string SocketPING(MXSocketMessage vMessage)
        {
            return "PING";
        }

        /// <summary>
        /// Handles the IP command from the socket and sends back the hostname, IP Address, # of current TCP connections.
        /// </summary>
        /// <param name="vMessage">Contains the Socket message that was received</param>
        /// <returns>Returns string that is then sent back to the client</returns>
        public string SocketIP(MXSocketMessage vMessage)
        {
            List<string> vIPs = GetMyIPs();
            string vIP = vIPs[0];
            foreach (string vAddress in vIPs)
            {
                if (vMessage.IPAddress.Contains(vAddress.Remove(vAddress.LastIndexOf("."))))
                {
                    vIP = vAddress;
                    break;
                }
            }
            return Dns.GetHostName() + "," + vIP + "," + vServer.TCPcnt.ToString();
        }

        /// <summary>
        /// Handles the VERSION command from the socket and sends back the application version.
        /// </summary>
        /// <param name="vMessage">Contains the Socket message that was received</param>
        /// <returns>Returns string that is then sent back to the client</returns>
        public string SocketVERSION(MXSocketMessage vMessage)
        {
            return appVersion;
        }

        /// <summary>
        /// Handles the STATIONSTATUS command from the socket and sends back the status of
        /// the requested station.
        /// </summary>
        /// <param name="vMessage">Contains the Socket message that was received</param>
        /// <returns>Returns string that is then sent back to the client</returns>
        public string SocketSTATIONSTATUS(MXSocketMessage vMessage)
        {
            string vRet = "";  // Return string to client

            if (vMessage.Data.Length == 1)  // Make sure station number was sent
            {
                int StationNum = Convert.ToInt16(vMessage.Data[0]);  // Station Number data is requested for
                ctlStation s = GetStation(StationNum);
                if (s == null)
                {
                    vRet = $"Station;{ StationNum },Connected;false";
                }
                else
                {
                    vRet = $"Station;{ StationNum }";
                    vRet += $",StationName;{ s.Name }";
                    vRet += $",Connected;{ s.tub.connected }";
                    vRet += $",SerialNum;{ s.serialNum }";
                    vRet += $",TubeName;{ s.tub.name }";
                    vRet += $",TubeStatus;{ s.tub.status }";
                    string hv = s.tub.on ? "On" : "Off";
                    vRet += $",HVStatus;{ hv }";
                    vRet += $",SetKV;{ s.tub.setkV }";
                    vRet += $",SetUA;{ s.tub.setuA }";
                    vRet += $",MonKV;{ s.tub.monkV }";
                    vRet += $",MonUA;{ s.tub.monuA }";
                    vRet += $",Status;{ s.lblStatus.Text }";
                    vRet += $",Progress;{ s.progressBar.Value }";
                    vRet += $",ErrorReason;{ s.errReason }";
                    vRet += $",ProductImage;{ s.tub.productImageFilePath}";
                    vRet += $",LoggingEnabled;{ s.chkLog.Checked }";
                    vRet += $",LEDOnColor;{ s.ledStatus.OnColor.ToKnownColor().ToString() }";
                    vRet += $",LEDOffColor;{ s.ledStatus.OffColor.ToKnownColor().ToString() }";
                    vRet += $",LEDEnabled;{ s.ledStatus.Value }";
                }

            }
            else vRet = "Error - Wrong Number of Arguments";  // Return error

            return vRet;  // Return string to client
        }

        /// <summary>
        /// Handles the EQUIPMENTSTATUS command from the socket and sends back the status of
        /// the requested equipment.
        /// </summary>
        /// <param name="vMessage">Contains the Socket message that was received</param>
        /// <returns>Returns string that is then sent back to the client</returns>
        public string SocketEQUIPMENTSTATUS(MXSocketMessage vMessage)
        {
            string vRet = "";  // Return string to client

            if (vMessage.Data.Length == 0)
            {
                vRet = $"LED_Safe;{ safe }";
                vRet += $",LED_HV;{ hvEnable }";
                vRet += $",LED_XRays;{ xRays }";
                vRet += $",LED_Testing;{ testing }";
                vRet += $",LED_Error;{ error }";
                vRet += $",EStop;{ eStopPressed }";
            }
            else vRet = "Error - Wrong Number of Arguments";  // Return error

            return vRet;  // Return string to client

        }
        #endregion
        #endregion

        #region Events


        public frmBurnInStns()
        {
            InitializeComponent();
        }

        private void frmBurnInStns_Shown(object sender, EventArgs e)
        {
            if (Settings.Default.CurrentlyTesting)
                MessageBox.Show("Testing was interrupted during last run. Please initialize each loaded station again.");
            Text = "Loading...please wait.";
            log.timeFormat = "M/dd HH:mm:ss";
            plc.OnDeviceChanged += PLCStateChanged; // Handles plc output to program.
            barCodeListener.reply += BarcodeResponder; // Handles messaging to barcode scanner.
            stg.newSettings += LoadSettings; // Handles overriding default settings.
            richTextBox1.Clear();
            plc.UpdateTime = 100;
            plc.Connect();
            InitPLC();
            if (!Settings.Default.PSEnabled)
            {
                Settings.Default.PSEnabled = psEnable = true;
                Settings.Default.Save();
                Thread.Sleep(25000);
            }
            vServer.TCPListen(9000);  // Start TCP Listen Server on port 9000
            vServer.UDPListen(9500);  // Start UDP Listen Server on port 9500
            vServer.OnMessageReceived += vServer_OnMessageReceived;  // Subscribe to the OnMessageReceived Event
            cabinetName = Environment.MachineName;
            Connect();
            if (testing) Stop();
            Text = cabinetName + " - " + appVersion;
        }

        private void frmBurnInStns_FormClosing(object sender, FormClosingEventArgs e)
        {
            var close = MessageBox.Show("Closing this application will end all testing. Are you sure you want to close?", "Close Application", MessageBoxButtons.YesNo);
            if (powerLoss) close = DialogResult.Yes;
            if (close == DialogResult.Yes)
            {
                tmrDbWrite.Stop();
                var shutDownUTC = MessageBox.Show("Power off all stations?", "Power off?,", MessageBoxButtons.YesNo);
                if (powerLoss) shutDownUTC = DialogResult.Yes;
                if (shutDownUTC == DialogResult.Yes)
                {
                    Settings.Default.PSEnabled = psEnable = false;
                    Settings.Default.Save();
                }
                Stop();
                barCodeListener.Dispose();
                log.Dispose();
                db?.Disconnect();
            }
            else e.Cancel = true;
        }

        /// <summary>
        /// Allows user to select any burn-in settings for the tube.
        /// </summary>
        /// <param name="settings">Settings to load.</param>
        private void LoadSettings(object[] settings)
        {
            if (selected != null)
            {
                selected.GiveSettings(settings);
                MessageBox.Show("New settings loaded.");
            }
            else MessageBox.Show("Select a station to load new settings.");
        }

        /// <summary>
        /// Handle actions from barcode scanner, return msg goes to scanner.
        /// </summary>
        /// <param name="bc">Barcode to handle.</param>
        /// <returns>String output to scanner.</returns>
        private string BarcodeResponder(string bc)
        {
            string response = string.Empty;

            if (!bc.Contains(" "))
            {
                int id = 0;
                if (int.TryParse(bc.Split('-')[0], out id))
                {
                    Invoke((MethodInvoker)delegate { response = InitStation(id); });
                }
            }
            else if (bc.Contains("@") && bc.Contains("-"))
            {
                string[] rowcol = bc.Split(' ')[1].Split('-');
                int row = int.Parse(rowcol[0]);
                int col = int.Parse(rowcol[1]);
                Invoke((MethodInvoker)delegate { response = SelectStation(row, col); });
            }
            else if (bc.Contains("START"))
            {
                Invoke((MethodInvoker)delegate {
                    if (btnStart.Text == "START" || btnStart.Text == "RESUME") response = Start();
                    else if (btnStart.Text == "PAUSE") response = Pause();
                });
            }
            else if (bc.Contains("STOP"))
            {
                Invoke((MethodInvoker)delegate { response = Stop(); });
            }
            else if (bc.Contains("LOCK"))
            {
                Invoke((MethodInvoker)delegate { response = LockUnlock(); });
            }
            else if (bc.Contains("SAFE"))
            {
                Invoke((MethodInvoker)delegate { response = ResetSafety(); });
            }
            else if (bc.Contains("STATUS"))
            {
                StringBuilder sb = new StringBuilder();
                for (int r = 0; r < pan.RowCount; r++)
                {
                    for (int c = 0; c < pan.ColumnCount; c++)
                    {
                        ctlStation s = (ctlStation)pan.GetControlFromPosition(c, r);
                        if (s != null) sb.AppendLine($"{ s.Name }: { s.lblStatus.Text }");
                    }
                }
                response = sb.ToString();
            }
            return response;
        }

        /// <summary>
        /// Handle PLC state changes.
        /// </summary>
        /// <param name="e">Event args.</param>
        private void PLCStateChanged(DeviceChangedArgs e)
        {
            Invoke((MethodInvoker)delegate {
                lblSafe.BackColor = safe ? Color.LightGreen : Color.LightGray;
                lblHV.BackColor = hvEnable ? Color.White : Color.LightGray;
                lblXrays.BackColor = xRays ? Color.LightBlue : Color.LightGray;
                lblTest.BackColor = testing ? Color.Gold : Color.LightGray;
                lblError.BackColor = error ? Color.IndianRed : Color.LightGray;
            });

            switch (e.Device.Name)
            {
                case SAF.inPLC_ERRACK:
                    if (e.Device.Value) plc.SetState(SAF.inPLC_ERRACK, false);
                    break;
                case SAF.inPLC_RESTART:
                    if (e.Device.Value) plc.SetState(SAF.inPLC_RESTART, false);
                    break;
                case SAF.SFI_CHB_UNLCK:
                    doorLocked = !e.Device.Value;
                    break;
                case SAF.SFI_CHB_OPEN:
                    doorOpen = e.Device.Value;
                    break;
                case SAF.SFI_EMO:
                    eStopPressed = !e.Device.Value;
                    if (eStopPressed) Pause();
                    break;
                case SAF.SFO_LED_SAFE:
                    safe = e.Device.Value;
                    break;
                case SAF.SFO_LED_XRAYS:
                    xRays = e.Device.Value;
                    break;
                case SAF.SFO_LED_TESTING:
                    testing = e.Device.Value;
                    Invoke((MethodInvoker)delegate
                    {
                        selected = null;
                        Settings.Default.CurrentlyTesting = testing;
                        Settings.Default.Save();
                    });
                    break;
                case PLC.PLO_LED_ERROR:
                    error = e.Device.Value;
                    break;
                case SAF.SFO_HV_PWR:
                    _hvEnable = e.Device.Value;
                    break;
                case PLC.PLO_PS_PWR:
                    _psEnable = e.Device.Value;
                    break;
                case PLC.PLI_UPS_BATTERY:
                    powerLoss = e.Device.Value;
                    if (powerLoss)
                    {
                        plc.SetState(PLC.PLO_LED_ERROR, powerLoss);
                        Tools.ShutdownPC();
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles the OnMessageReceived Event from the MXSocketServer class.  Set e.Messages.Responce string to
        /// send text message back to the client.
        /// </summary>
        /// <param name="e.Message">Contains the Socket message that was received</param>
        /// <returns>Set e.Messages.Responce string to send text message back to client</returns>
        private void vServer_OnMessageReceived(MessageReceivedArgs e)
        {
            string vMethodName = "Socket" + e.Message.Command;  // Construct the name of the method to call from the Message.Command string
            System.Reflection.MethodInfo vMethod = this.GetType().GetMethod(vMethodName);  // Find the method in this class
            object[] vParams = new object[] { e.Message };  // Construct object[] containing parameters to pass to method
            try
            {
                if (vMethod != null) e.Message.Responce = vMethod.Invoke(this, vParams).ToString();  // Call method if it exists
                else e.Message.Responce = "Error - Command Not Found";  // Set responce if the method does not exist
            }
            catch (Exception vErr)
            {
                e.Message.Responce = "Error - " + vErr.Message;  // If there was an error set responce containing the error
            }
        }

        /// <summary>
        /// Handle when station is done testing.
        /// </summary>
        /// <param name="sender">Station that is done.</param>
        private void DoneTesting(object sender)
        {
            ctlStation s = sender as ctlStation;
            UpdateResults(s);
            Thread.Sleep(500);

            if (s.tub.status == Tube.Status.Done) LogToConsole($"{ s.Name } has finished.");
            else if (s.tub.status == Tube.Status.Error) LogToConsole($"{ s.Name } has failed.");
            CheckIfAllCompleted();
        }

        private void tmrPlot_Tick(object sender, EventArgs e)
        {
            foreach (ctlStation s in pan.Controls)
            {
                gra.ctlGraph1.Plot($"{ s.Name } kV", 0, s.tub.monkV);
                gra.ctlGraph1.Plot($"{ s.Name } uA", 0, s.tub.monuA, true);
            }
        }

        private void tmrDbWrite_Tick(object sender, EventArgs e)
        {
            UpdateStatusDB();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "START" || btnStart.Text == "RESUME") Start();
            else if (btnStart.Text == "PAUSE") Pause();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var recon = MessageBox.Show("Try to reconnect missing stations?", "Reconnect", MessageBoxButtons.YesNo);
            if (recon == DialogResult.Yes)
            {
                if (gra != null) gra.Close();
                Connect();
            }
        }

        private void chkManual_Click(object sender, EventArgs e)
        {
            if (!manualControl && pas.askAlways)
            {
                manualControl = true;
                foreach (ctlStation s in pan.Controls) s.manualControl = true;
            }
            else
            {
                manualControl = false;
                foreach (ctlStation s in pan.Controls) s.manualControl = false;
            }
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            if (!res.Visible)
            {
                res.Show();
            }
            else
            {
                res.Hide();
            }
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            if (!gra.Visible)
            {
                tmrPlot.Start();
                gra.Show();
            }
            else
            {
                gra.Hide();
                tmrPlot.Stop();
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            log.ShowInNotepad();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (!stg.Visible && pas.askAlways)
            {
                stg.Show();
            }
            else
            {
                stg.Hide();
            }
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            if (!mnt.Visible && pas.askAlways)
            {
                mnt.Show();
            }
            else
            {
                mnt.Hide();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }
        #endregion
    }
}