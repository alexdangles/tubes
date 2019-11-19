using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Devices.Properties;

namespace Devices.Controls
{
    public partial class ctlRaptorCam : UserControl
    {
        RaptorCam cam;
        CCDImageAnalyzer ccd;
        CCDMultiImageAnalysis ccdA;

        public ctlRaptorCam()
        {
            InitializeComponent();
        }

        public bool Connect(string ipAddress)
        {
            
            ccd = new CCDImageAnalyzer();
            ccd.Dock = DockStyle.Fill;
            ccd.BackColor = Color.Black;
            ccd.SizeMode = PictureBoxSizeMode.StretchImage;
            tableLayoutPanel1.Controls.Add(ccd, 0, 0);
            ccdA = new CCDMultiImageAnalysis();
            cam = new RaptorCam(ipAddress);
            cam.imageAvailable += Cam_getImage;
            cam.numberOfImages = (int)numericUpDown3.Value;
            Disposed += CtlRaptorCam_Disposed;
            return cam.connected;
        }

        private void CtlRaptorCam_Disposed(object sender, EventArgs e)
        {
            cam?.Disconnect();
        }

        private void Cam_getImage(Bitmap image)
        {
            //ccd.startImageAnalysis(cam.counts, 255, cam.imageIndex);

            ccd.Image = image;
            if (cam.imageIndex == cam.numberOfImages - 1)
            {
                MakeIntensityGraph();
                cam.SaveBitmaps(@"C:\RaptorCam\");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cam.GetStatus();

            checkedListBox1.SetItemChecked(0, cam.sysStatus[1]);
            checkedListBox1.SetItemChecked(1, cam.sysStatus[3]);
            checkedListBox1.SetItemChecked(2, cam.sysStatus[5]);
            checkedListBox1.SetItemChecked(3, cam.sysStatus[6]);
            checkedListBox1.SetItemChecked(4, cam.sysStatus[7]);

            checkedListBox1.SetItemChecked(5, !cam.fpgaStatus[0]);
            checkedListBox1.SetItemChecked(6, cam.fpgaStatus[6]);
            checkedListBox1.SetItemChecked(7, cam.fpgaStatus[7]);

            checkedListBox1.SetItemChecked(8, cam.trigMode[0]);
            checkedListBox1.SetItemChecked(9, cam.trigMode[1]);

            checkedListBox1.SetItemChecked(10, cam.testPattern);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!cam.acquiring) cam.SnapShot();
            else cam.acquiring = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cam.exposure = (double)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            cam.TEC = (double)numericUpDown2.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.Text = cam.acquiring ? $"Snapping\r\nimage { cam.imageIndex + 1 }" : "";
            button1.BackgroundImage = cam.acquiring ? null : Resources.media_pict_camera;
        }

        private void MakeIntensityGraph()
        {
            int w = cam.counts.GetLength(1);
            int h = cam.counts.GetLength(2);
            int n = cam.numberOfImages;
            double[,] intensity = new double[w, h];
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        intensity[i, j] += Convert.ToDouble(cam.counts[k, i, j]) / Convert.ToDouble(n);
                    }
                }
            }
            ctlGraph1.Plot(intensity, 0, 255);
        }

        private void CalculateCenter()
        {
            double[] x = ctlGraph1.intensityGraph.Plots[0].GetXData();
            double[] y = ctlGraph1.intensityGraph.Plots[0].GetYData();
            double[,] z = ctlGraph1.intensityGraph.Plots[0].GetZData();
            double[] xScan = new double[x.Length];
            double[] yScan = new double[y.Length];

            for (int j = 0; j < y.Length; j++)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    xScan[i] = z[i, j];
                }
                yScan[j] = xScan.Sum();
            }
            int yPos = Array.IndexOf(yScan, yScan.Max());

            for (int j = 0; j < x.Length; j++)
            {
                for (int i = 0; i < y.Length; i++)
                {
                    yScan[i] = z[j, i];
                }
                xScan[j] = yScan.Sum();
            }
            int xPos = Array.IndexOf(xScan, xScan.Max());

            ctlGraph1.intensityGraph.Cursors[0].XPosition = xPos;
            ctlGraph1.intensityGraph.Cursors[0].YPosition = yPos;
            ctlGraph1.intensityGraph.Cursors[0].Visible = true;

            double centerX = ccd.spotCenterX;
            double centerY = ccd.spotCenterY;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            cam.numberOfImages = (int)numericUpDown3.Value;
        }
    }
}
