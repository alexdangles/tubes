using Helper;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Devices.Controls
{
    public partial class ctlFilterWheel : UserControl
    {
        FltrWhl fw;
        public ctlFilterWheel()
        {
            InitializeComponent();
            this.Disposed += OnDispose;
            knob1.AfterChangeValue += knob1_AfterChangeValue;
        }

        private void Fw_filterChanged(int position)
        {
            knob1.Value = position;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            fw?.Disconnect();
        }

        /// <summary>
        /// Connect to COM port.
        /// </summary>
        /// <param name="port">COM port.</param>
        public void Connect(string port)
        {
            fw = new FltrWhl(port);
            tmrUpdate.Start();
        }

        /// <summary>
        /// Autoconnect device from text file where the connection string is listed.
        /// </summary>
        /// <param name="deviceFile">Text file name in C:\Hardware\</param>
        public void AutoConnect(string deviceFile = "")
        {
            string port = Link.ConnectFromFile(deviceFile, "Filter Wheel")[0];
            if (port == null) port = FltrWhl.AutoConnect();
            Connect(port);
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            Enabled = fw.connected && !fw.switching;
            while (fw.switching) Application.DoEvents();
            knob1.AfterChangeValue -= knob1_AfterChangeValue;
            knob1.Value = fw.position;
            knob1.AfterChangeValue += knob1_AfterChangeValue;
        }

        private void knob1_AfterChangeValue(object sender, EventArgs e)
        {
            fw.position = (int)knob1.Value;
        }

        public void ChangeFilter(int position)
        {
            knob1.Value = position;
        }
    }
}