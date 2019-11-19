using System;
using System.Windows.Forms;
using Devices.Properties;
using Helper;

namespace Devices.Controls
{
    public partial class ctlSpellmanPS : UserControl
    {
        SpellmanPS ps;
        Form frmGraph = new Form();
        ctlGraph ctlGraph1 = new ctlGraph();

        public ctlSpellmanPS()
        {
            InitializeComponent();
            this.Disposed += OnDispose;
            ctlGraph1.Dock = DockStyle.Fill;
            frmGraph.Controls.Add(ctlGraph1);
            frmGraph.Size = new System.Drawing.Size(800, 400);
            frmGraph.FormClosing += GraphUI_FormClosing;
            ctlGraph1.Title = "Spellman DXM";
            ctlGraph1.xAxis = "Time";
            ctlGraph1.yAxis = "kV";
            ctlGraph1.yAxisSecondary = "uA";
            ctlGraph1.mode = ctlGraph.Mode.Scope;
            ctlGraph1.AddPlots(
                "Set kV",
                "Set uA",
                "Mon kV",
                "Mon uA"
                );
        }

        private void GraphUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            frmGraph.Hide();
        }

        public void Connect(string ip)
        {
            ps = new SpellmanPS(ip);
            lblModel.Text += ps.model;
            lblHardwareVersion.Text += ps.hwVersion;
            lblSoftwareVersion.Text += ps.swVersion;
            tmrUpdate.Start();
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            ps?.Disconnect();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            Enabled = ps.connected;
            txtMonFil.Text = ps.monFilament.ToString("0.000");
            txtMonkV.Text = ps.monkV.ToString("0.00");
            txtMonuA.Text = ps.monuA.ToString("0.00");
            txtSetFil.Text = ps.setFilLimit.ToString("0.000");
            txtSetkV.Text = ps.setkV.ToString("0.00");
            txtSetuA.Text = ps.setuA.ToString("0.00");
            txtWatts.Text = (ps.monkV * ps.monuA).ToString("0.00");
            btnPower.BackgroundImage = ps.on ? Resources.powerON : Resources.powerOFF;

            clbFaults.SetItemChecked(0, ps.fault["Arc"]);
            clbFaults.SetItemChecked(1, ps.fault["Over Temperature"]);
            clbFaults.SetItemChecked(2, ps.fault["Over Voltage"]);
            clbFaults.SetItemChecked(3, ps.fault["Under Voltage"]);
            clbFaults.SetItemChecked(4, ps.fault["Over Current"]);
            clbFaults.SetItemChecked(5, ps.fault["Under Current"]);

            if (ctlGraph1.Visible)
            {
                ctlGraph1.Plot("Set kV", 0, ps.setkV);
                ctlGraph1.Plot("Set uA", 0, ps.setuA, true);
                ctlGraph1.Plot("Mon kV", 0, ps.monkV);
                ctlGraph1.Plot("Mon uA", 0, ps.monuA, true);
            }
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            ps.on = !ps.on;
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            if (!frmGraph.Visible) frmGraph.Show();
            else frmGraph.Hide();
        }

        private void numkV_ValueChanged(object sender, EventArgs e)
        {
            ps.setkV = (double)numkV.Value;
        }

        private void numuA_ValueChanged(object sender, EventArgs e)
        {
            ps.setuA = (double)numuA.Value;
        }

        private void numFil_ValueChanged(object sender, EventArgs e)
        {
            ps.setFilLimit = (double)numFil.Value;
        }

        private void numFilPreHeat_ValueChanged(object sender, EventArgs e)
        {
            ps.setFilPreHeat = (double)numFilPreHeat.Value;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ps.ResetFaults();
        }
    }
}
