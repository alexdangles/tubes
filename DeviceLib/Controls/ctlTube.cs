using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Helper;
using System.Diagnostics;
using Devices.Properties;

namespace Devices.Controls
{
    public partial class ctlTube : UserControl
    {
        frmPassword pwd = new frmPassword();
        public Tube tub;
        public string testbox, testboxShort, configFolder;
        bool blinker;

        public ctlTube()
        {
            InitializeComponent();
            cmbStatus.Items.AddRange(Enum.GetNames(typeof(Tube.Status)));
            tabControl1.TabPages.Remove(tabI2C);
            dgvPSList.DataSource = DBHelper.Query("select * from TSCBPowerSupplyList");
            this.Disposed += OnDispose;
        }

        /// <summary>
        /// Autoconnect each device from text file where the connection string is listed.
        /// </summary>
        /// <param name="deviceName">Text file name in C:\Hardware\</name></param>
        public void AutoConnect(string deviceName = "")
        {
            string[] settings = Link.ConnectFromFile(deviceName, 
                "Long Name", 
                "Short Name", 
                "Config Folder", 
                "Power Supply", 
                "Amplifier", 
                "NIUSB-8452", 
                "DAQ Tasks",
                "UTC");

            if (settings.Length > 0)
            {
                testbox = settings[0];
                testboxShort = settings[1];
                configFolder = settings[2];
                string ps = settings[3];
                string amp = settings[4];
                string dig = settings[5];
                string daq = settings[6];
                string utc = settings[7];
                if (utc != null) Connect(utc);
                else Connect(ps, amp, dig, daq);
            }
        }

        /// <summary>
        /// Connect to Universal Tube Controller (UTC).
        /// </summary>
        /// <param name="utcIP">IP address of UTC.</param>
        public void Connect(string utcIP)
        {
            tub = new Tube(utcIP);
            tub.updateControls += UpdateControls;

            if (tub.connected)
            {
                tub.Initialize();
                tmrUpdate.Start();
            }
        }

        /// <summary>
        /// Manual connect each device.
        /// </summary>
        /// <param name="powerSupply">COM port.</param>
        /// <param name="amplifier">COM port.</param>
        /// <param name="spiI2c">Controller name.</param>
        /// <param name="daqTasks">DAQ Task names.</param>
        /// <returns></returns>
        public void Connect(string powerSupply, string amplifier, string spiI2c, string daqTasks)
        {
            tub = new Tube(powerSupply, amplifier, spiI2c, daqTasks);
            tub.updateControls += UpdateControls;

            if (tub.connected)
            {
                tub.Initialize();
                tmrUpdate.Start();
            }
        }

        private void UpdateControls(Tube.Status status, bool interlock, bool on, double kV, double uA)
        {
            if (!IsDisposed && !Disposing && Visible)
            {
                Invoke((MethodInvoker)delegate
                {
                    cmbStatus.SelectedItem = status.ToString();
                    btnPower.BackgroundImage = on ? Resources.powerON : Resources.powerOFF;
                    numSetkV.Value = (decimal)kV;
                    numSetuA.Value = (decimal)uA;
                    numSetkV.Enabled = numSetuA.Enabled = btnPower.Enabled = interlock;
                });
            }
        }

        private void UpdateRegisters()
        {
            if (tabI2C != null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 1000) Application.DoEvents();
                dgvRegisters.DataSource = tub.i2cRegisters;
                for (int i = 0; i < dgvRegisters.Rows.Count; i++)
                {
                    dgvRegisters.Rows[i].Cells[1].Style.BackColor = (string)dgvRegisters.Rows[i].Cells[1].FormattedValue == "YES" ? Color.LightGreen : Color.White;
                }
            }
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            tub.on = !tub.on;
            UpdateRegisters();
        }

        private void numSetkV_ValueChanged(object sender, EventArgs e)
        {
            if (sender == numSetkV) tub.setkV = (double)numSetkV.Value;
            else if (sender == numSetuA) tub.setuA = (double)numSetuA.Value;
            UpdateRegisters();
        }

        private void txtSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int id = 0;
                if (int.TryParse(txtSerial.Text.Split('-')[0], out id))
                {
                    var search = tub.GetSettingsFromDB(id);
                    if (search != null)
                    {
                        var ask = MessageBox.Show($"Is the power supply a { search[1] }?", $"Load { search[10] }?", MessageBoxButtons.YesNo);
                        if (ask == DialogResult.Yes && tub.Initialize(search))
                        {
                            if (lnkLog.Enabled)
                            {
                                lnkLog.Text = $@"K:\Software\Tubes\Saved Data\{ tub.serialNum }\{ Log.timeStamp }.txt";
                            }
                            picTube.Image = tub.productImage;
                            picTube.Show();
                            if (tub.controlType == Tube.ControlType.I2C && !tabControl1.TabPages.Contains(tabI2C))
                            {
                                tabControl1.TabPages.Add(tabI2C);
                            }
                            else if (tabControl1.TabPages.Contains(tabI2C))
                            {
                                tabControl1.TabPages.Remove(tabI2C);
                            }
                        }
                        else txtSerial.Clear();
                    }
                    else txtSerial.Clear();
                }
                else txtSerial.Clear();
            }
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            tub?.Disconnect();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (tub.interlock)
            {
                lblHVState.BackColor = tub.on ? Color.LimeGreen : Color.SlateGray;
                lblHVState.Text = tub.on ? "Tube Enabled" : "Tube Disabled";
            }
            else
            {
                blinker = !blinker;
                lblHVState.BackColor = blinker ? Color.IndianRed : Color.LightSalmon;
                lblHVState.Text = "Check Interlock";
            }

            txtMonkV.Text = tub.monkV.ToString("0.00");
            txtMonuA.Text = tub.monuA.ToString("0.00");
            txtWatts.Text = tub.watts.ToString("0.0");
            lblWatts.Text = (tub.watts >= tub.maxWatts) ? "Max Watts" : "Watts";
        }

        private void dgvPSList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataRow row = ((DataRowView)dgvPSList.SelectedRows[0].DataBoundItem).Row;

                string name = row[0].ToString() == "" || row[0] == null ? row[1].ToString() : row[0].ToString();
                var result = MessageBox.Show($"Load power supply settings for { name }?", $"Load { row[1].ToString() }?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    tub.GetSettingsFromDB(0);
                    if (tub.Initialize(row.ItemArray))
                    {
                        picTube.Image = tub.productImage;
                        picTube.Show();
                        if (tub.controlType == Tube.ControlType.I2C && !tabControl1.TabPages.Contains(tabI2C))
                        {
                            tabControl1.TabPages.Add(tabI2C);
                        }
                        else if (tabControl1.TabPages.Contains(tabI2C))
                        {
                            tabControl1.TabPages.Remove(tabI2C);
                        }
                    }
                }
            }
            catch { }
        }

        private void txtSerial_MouseClick(object sender, MouseEventArgs e)
        {
            txtSerial.SelectionStart = 0;
            txtSerial.SelectionLength = txtSerial.Text.Length;
        }

        private void picTube_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(
                $"{ tub.serialNum.ToString() }\r\n" +
                $"{ tub.name }\r\n" +
                $"{ tub.description }\r\n" +
                $"{ tub.date }\r\n" +
                $"{ tub.tubNum }\r\n" +
                $"{ tub.psType }\r\n" +
                $"Min kV: { tub.minkV }\r\n" +
                $"Max kV: { tub.maxkV }\r\n" +
                $"Min uA: { tub.minuA }\r\n" +
                $"Max uA: { tub.maxuA }\r\n" +
                $"Max Watts: { tub.maxWatts }\r\n" +
                $"{ tub.controlType } controlled", sender as Control);
        }

        private void txtBytesIn_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtBytesIn.Clear();
            txtBytesOut.Clear();
        }

        private void btnWriteBytes_Click(object sender, EventArgs e)
        {
            txtBytesOut.Clear();
            string[] s = tub.ReadBytes(txtBytesIn.Lines);
            if (s == null) MessageBox.Show("Input format error: make sure there are no spaces and use only 2 digit hexidecimals.");
            else txtBytesOut.Lines = s;
        }

        private void chkLog_CheckedChanged(object sender, EventArgs e)
        {
            tmrLog.Enabled = numLog.Enabled = chkLog.Checked;
            lnkLog.Enabled = !chkLog.Checked;
            if (chkLog.Checked && lnkLog.Text.Length > 0)
            {
                tub.log = new Log(lnkLog.Text, 30);
                tub.Log_AddHeaders();
                tub.log.Write($"Testbox { testbox }");
                tub.log.Write($"SN { tub.serialNum }");
                tub.log.Write(tub.tubNum);
                tub.log.Write(tub.psType);
            }
            else if (!chkLog.Checked) tub.log.Dispose();
        }

        private void tmrLog_Tick(object sender, EventArgs e)
        {
            if (chkLog.Checked) tub.LogStatus();
        }

        private void numLog_ValueChanged(object sender, EventArgs e)
        {
            tmrLog.Interval = (int)numLog.Value;
        }

        private void cmbStatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            tub.status = (Tube.Status)cmbStatus.SelectedIndex;
            cmbStatus.Enabled = false;
        }

        private void label7_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cmbStatus.Enabled = pwd.askOnce;
        }

        private void lnkLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (ModifierKeys == Keys.Control)
                {
                    Process.Start($"{ lnkLog.Text }\\..\\", "explorer.exe");
                }
                else
                {
                    openFileDialog1.Filter = "Text files (*.txt)|*.txt";
                    openFileDialog1.FileName = null;
                    var res = openFileDialog1.ShowDialog();
                    if (res == DialogResult.OK) lnkLog.Text = openFileDialog1.FileName;
                }
            }
            catch { }
        }

        private void picTube_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tub.status != Tube.Status.Testing)
            {
                var res = MessageBox.Show("Do you want to select a new tube?", "Select new PS?", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes) picTube.Hide();
            }
        }

        private void txtStatus_MouseHover(object sender, EventArgs e)
        {
            if (tub.niErrorCode != 0) toolTip1.Show($"NI Error Code: { tub.niErrorCode }", sender as Control);
            else toolTip1.Hide(sender as Control);
        }
    }
}