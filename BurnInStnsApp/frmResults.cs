using System;
using System.Windows.Forms;

namespace BurnInStns
{
    public partial class frmResults : Form
    {
        public frmResults()
        {
            InitializeComponent();
        }

        private void frmResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.arcLabBurnInResultsTableAdapter.Fill(this.tubesDataSet.ArcLabBurnInResults);
            e.Cancel = true;
            Hide();
        }

        private void frmResults_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'tubesDataSet.ArcLabBurnInResults' table. You can move, or remove it, as needed.
            this.arcLabBurnInResultsTableAdapter.Fill(this.tubesDataSet.ArcLabBurnInResults);
        }
    }
}
