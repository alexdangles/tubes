using System;
using System.Windows.Forms;
using Devices;
using EnvChamberApp.Properties;

namespace EnvChamberApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(EnvChamber.Device)));
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                comboBox1.Enabled = false;
                EnvChamber.Device device = (EnvChamber.Device)Enum.Parse(typeof(EnvChamber.Device), comboBox1.SelectedItem.ToString());
                if (device == EnvChamber.Device.ESPEC) ctlEnvChamber1.Connect(Settings.Default.IPAddress);
                else if (device == EnvChamber.Device.Thermotron) ctlEnvChamber1.AutoConnect();
            }
        }
    }
}
