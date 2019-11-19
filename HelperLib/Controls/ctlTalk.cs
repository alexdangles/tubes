using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace Helper
{
    public partial class ctlTalk : UserControl
    {
        public static string NUL = "\u0000";
        public static string SOH = "\u0001";
        public static string STX = "\u0002";
        public static string ETX = "\u0003";
        public static string EOT = "\u0004";
        public static string ENQ = "\u0005";
        public static string ACK = "\u0006";
        public static string BEL = "\u0007";
        public static string BS = "\u0008";
        public static string HT = "\u0009";
        public static string LF = "\u000A";
        public static string VT = "\u000B";
        public static string FF = "\u000C";
        public static string CR = "\u000D";
        public static string SO = "\u000E";
        public static string SI = "\u000F";
        public static string DLE = "\u0010";
        public static string DC1 = "\u0011";
        public static string DC2 = "\u0012";
        public static string DC3 = "\u0013";
        public static string DC4 = "\u0014";
        public static string NAK = "\u0015";
        public static string SYN = "\u0016";
        public static string ETB = "\u0017";
        public static string CAN = "\u0018";
        public static string EM = "\u0019";
        public static string SUB = "\u001A";
        public static string ESC = "\u001B";
        public static string FS = "\u001C";
        public static string GS = "\u001D";
        public static string RS = "\u001E";
        public static string US = "\u001F";
        public static string SP = "\u0020";
        public static string DEL = "\u007F";

        Link com;
        public ctlTalk()
        {
            InitializeComponent();
            cbxCOM.DataSource = Link.GetPortNames();
            cbxBaud.DataSource = cbxBaud.Items;
            cbxFormat.SelectedIndex = 0;
            lstComType.SelectedIndex = 0;
            this.Disposed += OnDispose;
        }

        private void OnDispose(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            if (com != null) com.Dispose();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (lstComType.SelectedIndex == 0)
            {
                com = new Link(new SerialPort(cbxCOM.SelectedItem.ToString(), int.Parse(cbxBaud.SelectedItem.ToString())));
            }
            else if (lstComType.SelectedIndex == 1)
            {
                if (txtIP.Text != null) com = new Link(int.Parse(txtPort.Text), txtIP.Text);
            }
            tmrUpdate.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            switch (cbxFormat.SelectedIndex)
            {
                case 0:
                    com.Tx($"{ txtMsgSend.Text }");
                    break;
                case 1:
                    com.Tx($"{ txtMsgSend.Text }{ CR }{ LF }");
                    break;
                case 2:
                    com.Tx($"{ txtMsgSend.Text }{ LF }{ CR }");
                    break;
                case 3:
                    com.Tx($"{ txtMsgSend.Text }{ CR }");
                    break;
                case 4:
                    com.Tx($"{ txtMsgSend.Text }{ LF }");
                    break;
                case 5:
                    com.Tx($"{ STX }{ txtMsgSend.Text }{ ETX }");
                    break;
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            lstComType.Enabled = btnConnect.Enabled = txtIP.Enabled = txtPort.Enabled = cbxCOM.Enabled = cbxBaud.Enabled = !com.portOpen;
            numMsToWait.Enabled = btnSend.Enabled = com.portOpen;
            if (com.dataAvailable)
            {
                txtMsgRec.AppendText(com.outQ.Dequeue().ToString() + "\r\n");
                txtMsgRec.SelectionStart = txtMsgRec.TextLength;
                txtMsgRec.ScrollToCaret();
            }
        }

        private void txtMsgSend_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtMsgSend.Clear();
        }

        private void numMsToWait_ValueChanged(object sender, EventArgs e)
        {
            com.msToWait = (int)numMsToWait.Value;
        }

        private void lstComType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCOM.Visible = cbxBaud.Visible = lstComType.SelectedIndex == 0;
            txtIP.Visible = txtPort.Visible = lstComType.SelectedIndex == 1;
            lblPortIP.Text = lstComType.SelectedIndex == 0 ? "Port" : "IP";
            lblBaudPort.Text = lstComType.SelectedIndex == 0 ? "Baud" : "Port";
        }

        private void txtMsgSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
            }
        }
    }
}
