using System;
using System.Windows.Forms;
using Helper;

namespace Devices.Controls
{
    public partial class ctlAmplifier : UserControl
    {
        SRS amp;
        int scaler;

        public ctlAmplifier()
        {
            InitializeComponent();
            this.Disposed += OnDispose;
            knob1.AfterChangeValue += knob1_AfterChangeValue;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            amp?.Disconnect();
        }

        /// <summary>
        /// Autoconnect device from text file where the connection string is listed.
        /// </summary>
        /// <param name="deviceFile">Text file name in C:\Hardware\</param>
        public void AutoConnect(string deviceFile = "")
        {
            string port = Link.ConnectFromFile(deviceFile, "Amplifier")[0];
            if (port == null) port = "COM1";
            Connect(port);
        }

        public void Connect(string port)
        {
            amp = new SRS(port);
            knob1.Value = (int)amp.scaler;
            tmrUpdate.Start();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            Enabled = amp.connected;
        }

        private void ChangeScaler(int newScaler)
        {
            knob1.Enabled = newScaler != 3;
            scaler = newScaler;
            if (knob1.Value != 0) knob1.Value = 0;
            else knob1_AfterChangeValue(this, EventArgs.Empty);
        }

        private void knob1_AfterChangeValue(object sender, EventArgs e)
        {
            amp.scaler = (SRS.Scaler)((scaler * knob1.CustomDivisions.Count) + knob1.Value);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeScaler(0);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ChangeScaler(1);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ChangeScaler(2);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ChangeScaler(3);
        }
    }
}
