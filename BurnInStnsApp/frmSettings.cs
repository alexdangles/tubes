using System;
using System.Data;
using System.Windows.Forms;

namespace BurnInStns
{
    public partial class frmSettings : Form
    {
        public delegate void NewSettings(object[] settings);
        public event NewSettings newSettings;
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'tubesDataSet.TSCBPowerSupplyList' table. You can move, or remove it, as needed.
            this.tSCBPowerSupplyListTableAdapter.Fill(this.tubesDataSet.TSCBPowerSupplyList);
            // TODO: This line of code loads data into the 'tubesDataSet.ArcLabBurnInSettings' table. You can move, or remove it, as needed.
            this.arcLabBurnInSettingsTableAdapter.Fill(this.tubesDataSet.ArcLabBurnInSettings);
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Disposing)
            {
                var msg = MessageBox.Show("Save these settings?", "Closing burn-in settings", MessageBoxButtons.YesNo);
                if (msg == DialogResult.Yes)
                {
                    tSCBPowerSupplyListTableAdapter.Update(tubesDataSet);
                    arcLabBurnInSettingsTableAdapter.Update(tubesDataSet);
                }
                e.Cancel = true;
                Hide();
            }
        }

        private void btnAddSet_Click(object sender, EventArgs e)
        {
            try
            {
                arcLabBurnInSettingsTableAdapter.Insert(dgvPSList.SelectedCells[0].FormattedValue.ToString(), 
                    Convert.ToDouble(dgvPSList.SelectedCells[4].FormattedValue), 
                    5,
                    20,
                    Convert.ToDouble(dgvPSList.SelectedCells[6].FormattedValue), 
                    5, 
                    40,
                    5,
                    24,
                    false,
                    2,
                    1,
                    false,
                    5,
                    1,
                    "");
                this.arcLabBurnInSettingsTableAdapter.Fill(this.tubesDataSet.ArcLabBurnInSettings);
            }
            catch
            {

            }
        }

        private void btnManualSet_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Load selected row to settings for current station?", "Override default burn-in settings?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                DataRow row = ((DataRowView)dgvBISettings.SelectedRows[0].DataBoundItem).Row;
                newSettings(row.ItemArray);
            }
        }
    }
}