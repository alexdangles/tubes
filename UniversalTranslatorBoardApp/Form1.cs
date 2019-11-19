using System;
using System.Windows.Forms;
using Devices;

namespace UniversalTranslatorBoardApp
{
    /// <summary>
    /// Moxtek Universal Translator Board user interface.
    /// </summary>
    public partial class Form1 : Form
    {
        UniPS ps;
        public Form1()
        {
            InitializeComponent();
        }

        private void frmUniversalTB_FormClosing(object sender, FormClosingEventArgs e)
        {
            ps?.Disconnect();
        }

        private void chkLEDs_CheckedChanged(object sender, EventArgs e)
        {
            ps.SetLED(chkGreen.Checked, chkYellow.Checked, chkRed.Checked);
        }

        private void cbxPSSelect_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ps.tubeType = (UniPS.TubeType)cbxPSSelect.SelectedItem;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                checkedListBox1.SetItemChecked(i, ps.status[i]);
                checkedListBox2.SetItemChecked(i, ps.faults[i]);
            }
            string registers = "";
            for (int i = 0; i < ps.userRegisters.Length; i++)
            {
                registers += $"{ i.ToString("X2") }  |  { ps.userRegisters[i].ToString("X2") }\r\n";
            }
            richTextBox1.Text = registers;
            button3.Enabled = ps.connected;
            txtMonkV.Text = ps.monkV.ToString("0.00");
            txtMonuA.Text = ps.monuA.ToString("0.00");
            checkBox3.Checked = ps.filReady;
            checkBox4.Checked = ps.powerRelay;
            button1.BackColor = ps.on ? System.Drawing.Color.Green : System.Drawing.Color.Gray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ps.on = !ps.on;
        }

        private void numSetkV_ValueChanged(object sender, EventArgs e)
        {
            ps.setkV = (double)numSetkV.Value;
        }

        private void numSetuA_ValueChanged(object sender, EventArgs e)
        {
            ps.setuA = (double)numSetuA.Value;
        }

        private void numAdjVolts_ValueChanged(object sender, EventArgs e)
        {
            ps.supplyVolts = (double)numAdjVolts.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ps.setAD = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ps.setDA = checkBox2.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ps.GetRegisters();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ps.powerRelay = checkBox4.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ps.ClearFaults();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ps = new UniPS(textBox1.Text);
            if (ps.connected)
            {
                ps.kVScaler = 14.65;
                ps.uAScaler = 48.8;
                cbxPSSelect.DataSource = Enum.GetValues(typeof(UniPS.TubeType));
                cbxPSSelect.SelectedItem = ps.tubeType;
                tmrUpdate.Start();
                textBox1.Enabled = button4.Enabled = false;
            }
            else Close();
        }
    }
}
