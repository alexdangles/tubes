using System.Windows.Forms;

namespace BurnInStns
{
    public partial class frmGraph : Form
    {
        public frmGraph()
        {
            InitializeComponent();
        }

        private void frmGraph_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
