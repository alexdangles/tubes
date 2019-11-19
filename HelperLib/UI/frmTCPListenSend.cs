using System;
using System.Windows.Forms;

namespace Helper
{
    public partial class frmTCPListenSend : Form
    {
        Link tcp;
        public frmTCPListenSend()
        {
            InitializeComponent();
        }

        private string Tcp_response(string msg)
        {
            switch (msg)
            {
                case "@10.7.1.44 LOCK":
                    return "This test station was designed by Alex D'Angelo in the X-ray design engineering group";
            }
            return string.Empty;
        }

        private void frmTCPListenSend_Shown(object sender, EventArgs e)
        {
            tcp = new Link(13000);
            tcp.reply += Tcp_response;
        }

        private void frmTCPListenSend_FormClosing(object sender, FormClosingEventArgs e)
        {
            tcp.Dispose();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            tcp.Tx(txtSend.Text);
        }

        private void tmrRecieve_Tick(object sender, EventArgs e)
        {
            if (tcp.dataAvailable) txtRecieve.Text = tcp.outQ.Dequeue().ToString();
        }

        private void btnReply_Click(object sender, EventArgs e)
        {
        }
    }
}
