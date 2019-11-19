using System;
using System.Windows.Forms;
using System.Drawing;
using Devices.Properties;

namespace Devices.Controls
{
    public partial class ctlEnvChamber : UserControl
    {
        EnvChamber env;

        public ctlEnvChamber()
        {
            InitializeComponent();
            this.Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            env?.Disconnect();
        }

        /// <summary>
        /// Connect to ESPEC chamber.
        /// </summary>
        /// <param name="port">57732</param>
        /// <param name="ipAddress">IP address of ESPEC.</param>
        public void Connect(string ipAddress)
        {
            env = new EnvChamber(ipAddress);
            tmrUpdate.Start();
        }

        /// <summary>
        /// Auto connect to Thermotron chamber.
        /// </summary>
        public void AutoConnect()
        {
            env = new EnvChamber(EnvChamber.AutoConnect());
            tmrUpdate.Start();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            Enabled = env.connected;
            btnPower.BackgroundImage = env.on ? Resources.powerON : Resources.powerOFF;
            thmAirTemp.Value = env.monTemp;
            thmPartTemp.Value = env.monPartTemp;
            thmHumi.Value = env.monHumi;

            thmAirTemp.FillColor = thmAirTemp.Value > 0 ? Color.Red : Color.Blue;
            thmPartTemp.FillColor = thmPartTemp.Value > 0 ? Color.Red : Color.Blue;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (chkPart.Checked) env.setPartTemp = (double)numSetPartTemp.Value;
            else
            {
                env.setTemp = (double)numSetTemp.Value;
                env.setHumi = (int)numSetHumi.Value;
            }
        }

        private void numSet_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = sender as NumericUpDown;
            num.ForeColor = num.Value > 0 ? Color.Red : Color.Blue;
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            env.on = !env.on;
        }

        private void chkPart_CheckedChanged(object sender, EventArgs e)
        {
            numSetPartTemp.Enabled = chkPart.Checked;
        }
    }
}
