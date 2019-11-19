using System.Windows.Forms;
using SpellmanPSApp.Properties;

namespace SpellmanPSApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ctlSpellmanPS1.Connect(Settings.Default.IPAddress);
        }
    }
}
