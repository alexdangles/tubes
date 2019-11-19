using Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RingoCalibrationApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel1.Controls.Add(DPP1.spmGraph);
            DPP1.numkeV.Enabled = false;

            DPP1.AutoConnect(Text);
            Tube1.AutoConnect(Text);
            FW1.AutoConnect(Text);

            DPP1.LoadSettings(
                $@"{ Tube1.configFolder }PS564 RH3 XPIN_BT\MX-02r00us.dxp",
                $@"{ Tube1.configFolder }start upcalibration\Ringo calibration.txt"
                );
            Text = Tube1.testbox;
            Tube1.numSetkV.ValueChanged += NumSetkV_ValueChanged;
        }

        private void NumSetkV_ValueChanged(object sender, EventArgs e)
        {
            DPP1.numkeV.Value = Tube1.numSetkV.Value;
        }

        /// <summary>
        /// Generate an auto-incrementing test ID in tubes database.
        /// </summary>
        /// <param name="serial">Tube serial number.</param>
        /// <param name="testbox">Testbox name.</param>
        /// <returns></returns>
        public static int GenerateTestID(int serial, string testbox)
        {
            DBHelper.Insert("TSCBTest", serial, DateTime.Now, testbox);
            return Convert.ToInt32(DBHelper.Query("select MAX(TEST_ID_PK) from TSCBTest").Rows[0][0]);
        }

        private void DoTest(bool justVerify = false)
        {
            int testID = GenerateTestID(Tube1.tub.serialNum, Tube1.testboxShort);
            double maxkV = Tube1.tub.maxkV;

            btnStart.Enabled = btnVerify.Enabled = false;
            Tube1.tub.status = Devices.Tube.Status.Testing;
            Tube1.tub.maxkV = 55;

            int n = 0;

            double[] kV;
            double[] kVSet;
            double[] kVMon;
            double[] kVEdge;
            bool[] pass;

            List<double> verifyKV = new List<double>();
            if (Tube1.tub.maxkV == 35) verifyKV.Add(35);
            else
            {
                verifyKV.Add(8);
                verifyKV.Add(13);
                verifyKV.Add(15);
                verifyKV.Add(25);
                verifyKV.Add(35);
                verifyKV.Add(40);
                if (Tube1.tub.maxkV > 40) verifyKV.Add(50);
            }

            if (justVerify) n = 1;

            for (int j = n; j < 2; j++)
            {
                if (n == 0)
                {
                    kV = new double[48];
                    kVSet = new double[48];
                    kVMon = new double[48];
                    kVEdge = new double[48];
                    pass = new bool[48];
                    dataGridView1.Columns["Pass"].Visible = false;

                    Tube1.tub.DeleteCalibration();
                }
                else
                {
                    kV = new double[verifyKV.Count];
                    kVSet = new double[verifyKV.Count];
                    kVMon = new double[verifyKV.Count];
                    kVEdge = new double[verifyKV.Count];
                    pass = new bool[verifyKV.Count];
                    dataGridView1.Columns["Pass"].Visible = true;
                }

                Tube1.tub.setuA = Tube1.tub.maxuA;
                Tube1.tub.on = true;
                dataGridView1.Rows.Clear();

                for (int i = 0; i < kV.Length; i++)
                {
                    if (n == 0)
                        kV[i] = i + 5;
                    else
                        kV[i] = verifyKV[i];

                    if (kV[i] <= 11)
                    {
                        FW1.ChangeFilter(1);
                    }
                    else if (kV[i] >= 12 && kV[i] <= 16)
                    {
                        FW1.ChangeFilter(2);
                    }
                    else if (kV[i] >= 17 && kV[i] <= 24)
                    {
                        FW1.ChangeFilter(3);
                    }
                    else if (kV[i] >= 25)
                    {
                        FW1.ChangeFilter(3);
                    }

                    Tube1.tub.setkV = kV[i];

                    Tools.WaitSync(1000);

                    DPP1.GetSpectrum();

                    if (DPP1.stopper) break;

                    kVSet[i] = Tube1.tub.setkV;
                    kVMon[i] = Tube1.tub.monkV;
                    kVEdge[i] = DPP1.keVEdge;

                    if (n == 0) dataGridView1.Rows.Add(kV[i], kVSet[i], kVMon[i], kVEdge[i]);
                    else
                    {
                        pass[i] = (kVEdge[i] < kV[i] + 0.2) && (kVEdge[i] > kV[i] - 0.2);
                        dataGridView1.Rows.Add(kV[i], kVSet[i], kVMon[i], kVEdge[i], pass[i]);
                    }
                }

                Tube1.tub.on = false;

                if (DPP1.stopper) break;

                if (n == 0)
                {
                    Tube1.tub.Calibrate(kVEdge);
                    n++;

                    Tools.WaitSync(2000);

                    for (int i = 0; i < kV.Length; i++)
                    {
                        DBHelper.Insert("TSCBPreCalData",
                            testID,
                            Tube1.tub.serialNum,
                            kV[i],
                            kVSet[i],
                            kVMon[i],
                            kVEdge[i]
                            );
                    }

                    for (int i = 0; i < Tube1.tub.keVFit.Length; i++)
                    {
                        DBHelper.Insert("TSCBSourceCalibration",
                            testID,
                            Tube1.tub.serialNum,
                            Tube1.tub.keVFit[i],
                            Tube1.tub.keVFitByte[i],
                            Tube1.tub.page1keVAddresses[i],
                            Tube1.tub.keVSetFitByte[i],
                            Tube1.tub.page1SetAddresses[i],
                            Tube1.tub.keVMonFitByte[i],
                            Tube1.tub.page1MonAddresses[i]
                            );
                    }
                }
                else if (n == 1)
                {
                    for (int i = 0; i < kV.Length; i++)
                    {
                        DBHelper.Insert("TSCBSourceCalValidation",
                            testID,
                            Tube1.tub.serialNum,
                            kV[i],
                            kVSet[i],
                            kVMon[i],
                            kVEdge[i],
                            0,
                            0,
                            pass[i]
                            );
                    }

                    Tube1.tub.status = Devices.Tube.Status.Done;

                    string result = "";
                    for (int i = 0; i < kV.Length; i++)
                    {
                        result += $"{ kV[i] }  { kVEdge[i] }  { pass[i] }\r\n";
                    }
                    result = result.Replace("True", "Yes");
                    result = result.Replace("False", "No");
                    MessageBox.Show(
                        "kV   keVEdge   Pass?\r\n" +
                        result, "Test Results");
                }
            }

            Tube1.tub.maxkV = maxkV;
            Tube1.tub.status = Devices.Tube.Status.Idle;

            DPP1.stopper = false;
            btnStart.Enabled = btnVerify.Enabled = true;
            btnStart.BackColor = btnVerify.BackColor = Color.LightBlue;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Tube1.tub.status != Devices.Tube.Status.Error && Tube1.tub.controlType == Devices.Tube.ControlType.I2C)
            {
                btnStart.BackColor = Color.Aqua;
                DoTest();
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (Tube1.tub.status != Devices.Tube.Status.Error && Tube1.tub.controlType == Devices.Tube.ControlType.I2C)
            {
                btnVerify.BackColor = Color.Aqua;
                DoTest(true);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Tube1.tub.status == Devices.Tube.Status.Testing)
            {
                var result = MessageBox.Show("End?", "End Testing?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DPP1.stopper = true;
                }
                e.Cancel = true;
            }
            else
                MessageBox.Show("Open the interlock, then press OK.");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        }
    }
}
