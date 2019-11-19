using System.Windows.Forms;

namespace TDKLambdaApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ctlTdkPowerSupply1.AutoConnect(Text);
        }
    }
}
