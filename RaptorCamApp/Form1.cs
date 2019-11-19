using RaptorCamApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devices;

namespace RaptorCamApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ctlRaptorCam1.Connect(Settings.Default.IPAddress);
        }
    }
}