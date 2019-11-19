using Helper;
using System;
using System.Windows.Forms;

namespace TSCB
{
    public partial class Form1 : Form
    {
        DBHelper db = new DBHelper();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ctlGraph1.AddPlots("Photodiode");
            panel1.Controls.Add(ctlDPP501.spmGraph);
            ctlDPP501.spmGraph.ForeColor = panel1.ForeColor;
            ctlDPP501.spmGraph.BackColor = panel1.BackColor;
            string testbox = Text;
            Text = "Loading...please wait.";

            ctlTube1.AutoConnect(testbox);
            ctlTube1.numSetkV.ValueChanged += NumSetkV_ValueChanged;
            ctlDPP501.AutoConnect(testbox);
            ctlDPP501.numkeV.Enabled = false;
            timer1.Start();

            Text = ctlTube1.testbox;
        }

        private void NumSetkV_ValueChanged(object sender, EventArgs e)
        {
            ctlDPP501.numkeV.Value = ctlTube1.numSetkV.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ctlGraph1.Plot("Photodiode", 0, ctlTube1.tub.photodiode);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Generate an auto-incrementing test ID in tubes database.
        /// </summary>
        /// <param name="serial">Tube serial number.</param>
        /// <param name="testbox">Testbox name.</param>
        /// <returns></returns>
        public int GenerateTestID(int serial, string testbox)
        {
            db.Insert("TSCBTest", serial, DateTime.Now, testbox);
            return Convert.ToInt32(DBHelper.Query("select MAX(TEST_ID_PK) from TSCBTest").Rows[0][0]);
            db.CloseConnection();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
