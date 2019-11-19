using System;
using System.Drawing;
using System.Windows.Forms;
using Helper;
using Devices;
using System.Diagnostics;
using System.Text;
using BurnInStns.Properties;
using System.Threading.Tasks;
using System.Threading;

namespace BurnInStns
{
    public partial class ctlStation : UserControl
    {
        public Tube tub;
        public delegate void DoneTesting(object sender);
        public event DoneTesting doneTesting;
        bool cyclingEnable, blinker;
        public int testID;
        public bool firstRun, manualControl;
        double kVLim, uALim, onTime, offTIme, settlingTime;
        public string testRecipe = "", errReason = "";

        public int serialNum { get; private set; }
        public TimeSpan runTime { get; private set; }
        public Stopwatch runTimer { get; private set; }
        public int[] errorCount { get; private set; }

        const int errorThreshold = 20; // How many errors to fail the test.

        public ctlStation()
        {
            InitializeComponent();
            errorCount = new int[2];
            this.Disposed += OnDispose;
            runTime = new TimeSpan(0, 0, 0);
            runTimer = new Stopwatch();
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            tub?.Disconnect();
        }

        public void Connect(string IP)
        {
            if (IP.Contains(" "))
                tub = new Tube(IP.Split(' ')[0], IP.Split(' ')[1]); // UTC and Spellman control
            else
                tub = new Tube(IP); // UTC control only

            tub.updateControls += UpdateControls;
            Thread.Sleep(500);

            if (tub.connected)
            {
                ledStatus.Caption = Name;
                tub.log = new Log($@"{ Settings.Default.SaveFolder }\{ Name }.txt", 10);
                tub.Log_AddHeaders();
                tmrUpdate.Start();
            }
            else
            {
                Dispose();
            }
        }

        private void UpdateControls(Tube.Status status, bool interlock, bool on, double kV, double uA)
        {
            if (!IsDisposed && !Disposing && Visible)
            {
                Invoke((MethodInvoker)delegate
                {
                    numkV.Value = (decimal)kV;
                    numuA.Value = (decimal)uA;
                    numkV.Enabled = numuA.Enabled = btnPower.Enabled = interlock;
                });
            }
        }

        public int Setup(int serial)
        {
            int setupResult = 0;
            serialNum = serial;
            bool found = tub.Initialize(tub.GetSettingsFromDB(serial));
            if (found) setupResult++;
            var runSettings = found && tub.name.Length > 0 ? DBHelper.SearchTable(Settings.Default.SettingsTable, "Moxtek#", tub.name) : null;
            found = runSettings != null;
            if (found)
            {
                GiveSettings(runSettings.Rows[0].ItemArray);
                picTube.Image = tub.productImage;
                setupResult++;
            }
            else
            {
                runTime = new TimeSpan(0, 0, 0);
                picTube.Image = picTube.ErrorImage;
                tub.status = Tube.Status.Error;
            }
            ledStatus.Value = true;
            return setupResult;
        }

        public void GiveSettings(object[] settings)
        {
            testRecipe = settings[0].ToString();
            tub.setkV = Convert.ToDouble(settings[1]);
            kVLim = Convert.ToDouble(settings[2]) / 100.0;
            tub.kVRampPerSec = Convert.ToDouble(settings[3]);
            tub.setuA = Convert.ToDouble(settings[4]);
            uALim = Convert.ToDouble(settings[5]) / 100.0;
            tub.uARampPerSec = Convert.ToDouble(settings[6]);
            settlingTime = Convert.ToDouble(settings[7]);
            runTime = new TimeSpan(0, 0, (int)(3600 * Convert.ToDouble(settings[8])));
            // photodiode enable
            // photodiode upper lim
            // photodiode lower lim
            cyclingEnable = Convert.ToBoolean(settings[12]);
            onTime = Convert.ToDouble(settings[13]);
            offTIme = Convert.ToDouble(settings[14]);
            firstRun = true;
        }

        public async void Start()
        {
            BackColor = Color.Transparent;
            if (runTime.TotalSeconds > 0 && tub.status != Tube.Status.Done)
            {
                await Task.Run(() =>
                {
                    tub.status = Tube.Status.Testing;
                    if (cyclingEnable) tub.DutyCycle(onTime, offTIme);
                    else tub.on = true;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (sw.ElapsedMilliseconds < (int)(settlingTime * 1000)) Application.DoEvents();
                    sw.Stop();
                    runTimer.Start();
                });
            }
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            tub.on = !tub.on;
        }

        private void numkV_ValueChanged(object sender, EventArgs e)
        {
            tub.setkV = (double)numkV.Value;
        }

        private void numuA_ValueChanged(object sender, EventArgs e)
        {
            tub.setuA = (double)numuA.Value;
        }

        public void Pause()
        {
            runTimer.Stop();
            BackColor = Color.Transparent;
            if (tub.status == Tube.Status.Testing)
            {
                tub.status = Tube.Status.Paused;
            }
        }

        public void Stop()
        {
            if (tub.status == Tube.Status.Testing)
            {
                tub.status = Tube.Status.Idle;
                ledStatus.Value = false;
                picTube.Image = picTube.ErrorImage;
                tub.serialNum = 0;
                tub.Initialize();
                Finished("Aborted by user", true);
            }
        }

        private void Finished(string errorReason, bool isAborted = false)
        {
            errReason = errorReason;
            runTime = new TimeSpan(0, 0, 0);
            if (errReason.Length > 0 && !isAborted) tub.status = Tube.Status.Error;
            doneTesting(this);
            for (int i = 0; i < errorCount.Length; i++)
            {
                errorCount[i] = 0;
            }
            runTimer.Reset();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            blinker = !blinker;
            if (manualControl)
            {
                txtInfo.Visible = false;
                lblkV.Text = tub.monkV.ToString("0.0");
                lbluA.Text = tub.monuA.ToString("0.0");
                btnPower.BackColor = tub.on ? Color.MediumSeaGreen : Color.DarkGray;
                ledWatchdog.Value = tub.utc.watchDogState;
            }
            else txtInfo.Visible = true;
            string msg = tub.status.ToString() + ". ";

            switch (tub.status)
            {
                case Tube.Status.Idle:
                    ledStatus.OnColor = Color.LimeGreen;
                    break;
                case Tube.Status.Testing:
                    ledStatus.OnColor = Color.Orange;
                    ledStatus.Value = blinker;
                    tub.Relay(blinker);
                    msg += runTimer.IsRunning? $"{ (runTime - runTimer.Elapsed).ToString().Split('.')[0] } Remaining." : "";
                    break;
                case Tube.Status.Paused:
                    ledStatus.OnColor = Color.Orange;
                    ledStatus.Value = true;
                    msg += $"{ (runTime - runTimer.Elapsed).ToString().Split('.')[0] } Remaining.";
                    break;
                case Tube.Status.Done:
                    ledStatus.OnColor = Color.LimeGreen;
                    ledStatus.Value = blinker;
                    tub.Relay(blinker);
                    break;
                case Tube.Status.Error:
                    ledStatus.OnColor = Color.Red;
                    ledStatus.Value = true;
                    msg += errReason;
                    break;
            }

            if (runTimer.IsRunning && runTimer.Elapsed.TotalSeconds >= runTime.TotalSeconds) // Passed
            {
                tub.status = Tube.Status.Done;
                Finished("");
            }
            else if (runTimer.IsRunning) // Put failure modes here..
            {
                if (tub.monkV > tub.setkV + tub.setkV * kVLim ||
                    tub.monkV < tub.setkV - tub.setkV * kVLim ||
                    tub.monuA > tub.setuA + tub.setuA * uALim ||
                    tub.monuA < tub.setuA - tub.setuA * uALim)
                {
                    errorCount[0]++;
                    if (errorCount[0] >= errorThreshold)
                    {
                        Finished(@"Monitors +/- thresholds");
                    }
                }
                if (!tub.on)
                {
                    errorCount[1]++;
                    if (errorCount[1] >= errorThreshold)
                    {
                        Finished("HV is unstable");
                    }
                }
                if (!tub.connected)
                {
                    Finished("Station lost connection");
                    Dispose();
                }
            }
            else if (!tub.connected)
            {
                Dispose();
            }

            string hV = tub.on ? "ON" : "OFF";
            StringBuilder s = new StringBuilder();
            if (tub.serialNum > 0) s.AppendLine(tub.serialNum.ToString());
            s.AppendLine(tub.name);
            s.AppendLine($"kV set: { tub.setkV.ToString("0.0") }");
            s.AppendLine($"uA set: { tub.setuA.ToString("0.0") }");
            s.AppendLine($"kV mon: { tub.monkV.ToString("0.0") }");
            s.AppendLine($"uA mon: { tub.monuA.ToString("0.0") }");
            s.AppendLine($"HV Status: { hV }");

            txtInfo.Text = s.ToString();
            lblStatus.Text = msg;

            if (runTime.TotalSeconds > 0) progressBar.Value = (int)((runTimer.Elapsed.TotalSeconds / runTime.TotalSeconds) * 100);
            else if (tub.status == Tube.Status.Done) progressBar.Value = 100;
            else progressBar.Value = 0;
        }

        private void picTube_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(
                $"{ tub.name }\r\n" +
                $"{ tub.tubNum }\r\n" +
                $"{ tub.psType }\r\n" +
                $"{ tub.controlType } controlled\r\n" +
                $"kV Scaler: { tub.kVScaler }\r\n" +
                $"uA Scaler: { tub.uAScaler }\r\n" +
                $"Watts: { tub.maxWatts }\r\n" +
                $"Max kV: { tub.maxkV }\r\n" +
                $"Max uA: { tub.maxuA }\r\n" +
                $"Run Time: { runTime.TotalHours } hours"
                , picTube);
        }

        private void chkLog_CheckedChanged(object sender, EventArgs e)
        {
            tmrLog.Enabled = chkLog.Checked;
        }

        private void tmrLog_Tick(object sender, EventArgs e)
        {
            tub.LogStatus();
        }
    }
}