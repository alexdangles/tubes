using System;
using System.Windows.Forms;
using Devices.Properties;
using Helper;

namespace Devices
{
    public partial class ctlTdkPowerSupply : UserControl
    {
        TDKPS tdk;
        Form frmGraph = new Form();
        ctlGraph ctlGraph1 = new ctlGraph();

        public ctlTdkPowerSupply()
        {
            InitializeComponent();
            this.Disposed += OnDispose;
            ctlGraph1.Dock = DockStyle.Fill;
            frmGraph.Controls.Add(ctlGraph1);
            frmGraph.Size = new System.Drawing.Size(800, 400);
            frmGraph.FormClosing += GraphUI_FormClosing;
            ctlGraph1.Title = "TDK Lambda Zup 36-6";
            ctlGraph1.xAxis = "Time";
            ctlGraph1.yAxis = "Volts";
            ctlGraph1.yAxisSecondary = "Amps";
            ctlGraph1.mode = ctlGraph.Mode.Scope;
            ctlGraph1.AddPlots(
                "Set V",
                "Set A",
                "Mon V",
                "Mon A",
                "Watts"
                );
        }

        private void GraphUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            frmGraph.Hide();
        }

        /// <summary>
        /// Connect to COM port.
        /// </summary>
        /// <param name="port">COM port.</param>
        public void Connect(string port)
        {
            tdk = new TDKPS(port);
            tmrUpdate.Start();
        }

        /// <summary>
        /// Autoconnect device from text file where the connection string is listed.
        /// </summary>
        /// <param name="deviceFile">Text file name in C:\Hardware\</param>
        public void AutoConnect(string deviceFile = "")
        {
            string port = Link.ConnectFromFile(deviceFile, "Power Supply")[0];
            if (port == null) port = TDKPS.AutoConnect();
            Connect(port);
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            tdk?.Disconnect();
        }

        private void numVolts_ValueChanged(object sender, EventArgs e)
        {
            tdk.setV = (double)numVolts.Value;
        }

        private void numAmps_ValueChanged(object sender, EventArgs e)
        {
            tdk.setA = (double)numAmps.Value;
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            tdk.on = !tdk.on;
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            if (!frmGraph.Visible) frmGraph.Show();
            else frmGraph.Hide();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            Enabled = tdk.connected;
            btnPower.BackgroundImage = tdk.on ? Resources.powerON : Resources.powerOFF;
            txtSetVolts.Text = tdk.setV.ToString("0.00");
            txtSetAmps.Text = tdk.setA.ToString("0.00");
            txtMonVolts.Text = tdk.monV.ToString("0.00");
            txtMonAmps.Text = tdk.monA.ToString("0.00");
            txtWatts.Text = (tdk.watts).ToString("0.00");

            clbStatus.SetItemChecked(0, tdk.status["CC/CV"]);
            clbStatus.SetItemChecked(1, tdk.status["Foldback"]);
            clbStatus.SetItemChecked(2, tdk.status["AST"]);
            clbStatus.SetItemChecked(3, tdk.status["ON"]);
            clbStatus.SetItemChecked(4, tdk.status["SRF"]);
            clbStatus.SetItemChecked(5, tdk.status["SRV"]);
            clbStatus.SetItemChecked(6, tdk.status["SRT"]);
            clbStatus.SetItemChecked(7, tdk.status["Alarm"]);

            clbStatus.SetItemChecked(8, tdk.alarm["OVP"]);
            clbStatus.SetItemChecked(9, tdk.alarm["OTP"]);
            clbStatus.SetItemChecked(10, tdk.alarm["AC Fail"]);
            clbStatus.SetItemChecked(11, tdk.alarm["Foldback"]);
            clbStatus.SetItemChecked(12, tdk.alarm["Programming"]);

            if (ctlGraph1.Visible)
            {
                ctlGraph1.Plot("Set V", 0, tdk.setV);
                ctlGraph1.Plot("Set A", 0, tdk.setA, true);
                ctlGraph1.Plot("Mon V", 0, tdk.monV);
                ctlGraph1.Plot("Mon A", 0, tdk.monA, true);
                ctlGraph1.Plot("Watts", 0, tdk.watts);
            }
        }
    }
}
