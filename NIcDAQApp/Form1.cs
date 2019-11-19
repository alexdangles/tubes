using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Devices;
using Helper;
using System.IO;
using System.Diagnostics;
using NationalInstruments.UI;

namespace NIcDAQApp
{
    /// <summary>
    /// National Instruments cDAQ User Interface.
    /// </summary>
    public partial class Form1 : Form
    {
        NIcDAQ daq;

        public Form1()
        {
            InitializeComponent();
            listBox1.DataSource = NIcDAQ.taskList;
        }

        private void frmDAQ_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            daq?.Disconnect();
        }

        private void DigitalWrite(object sender, EventArgs e)
        {
            List<bool> relay = new List<bool>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                relay.Add(checkedListBox1.GetItemChecked(i));
            }
            daq.DigitalWrite(relay.ToArray());
        }

        private void AnalogWrite(object sender, EventArgs e)
        {
            List<double> volts = new List<double>();
            foreach (var n in flowLayoutPanel1.Controls.OfType<NumericUpDown>())
                volts.Add((double)n.Value);

            daq.AnalogWrite(volts.ToArray());
        }

        private void LoadTasks(object sender, EventArgs e)
        {
            try
            {
                string tasks = "";
                foreach (string s in listBox1.SelectedItems)
                {
                    if (tasks.Length > 0) tasks += "," + s;
                    else tasks = s;
                }

                daq = new NIcDAQ(tasks, false);
                Thread.Sleep(500);

                if (daq.DOChannels != null)
                {
                    groupBox1.Show();
                    checkedListBox1.Items.Clear();
                    for (int i = 0; i < daq.DOChannels.Length; i++)
                    {
                        checkedListBox1.Items.Add(daq.DOChannels[i]);
                    }
                }

                if (daq.AIChannels != null)
                {
                    groupBox2.Show();
                    List<string> names = new List<string>();
                    for (int i = 0; i < daq.AIChannels.Length; i++)
                    {
                        names.Add(daq.AIChannels[i]);
                    }
                    ctlGraph1.AddPlots(names.ToArray());
                    timer1.Start();
                }

                if (daq.AOChannels != null)
                {
                    groupBox3.Show();
                    for (int i = 0; i < daq.AOChannels.Length; i++)
                    {
                        Label l = MyControls.Clone(label1);
                        l.Text = daq.AOChannels[i];
                        NumericUpDown n = MyControls.Clone(numericUpDown1);
                        l.Visible = n.Visible = true;
                        n.ValueChanged += AnalogWrite;
                        flowLayoutPanel1.Controls.Add(l);
                        flowLayoutPanel1.Controls.Add(n);
                    }
                }

                if (daq.DIChannels != null)
                {
                    groupBox4.Show();
                    ledArray1.ScaleMode = ControlArrayScaleMode.CreateFixedMode(daq.digitalInData.Length);
                    timer2.Start();
                }

                listBox1.Hide();
                button1.Hide();
                button2.Hide();
                button3.Hide();
                tableLayoutPanel1.Show();

                if (groupBox2.Visible)
                {
                    AutoSize = false;
                    AutoSizeMode = AutoSizeMode.GrowOnly;
                }
            }
            catch
            {
                MessageBox.Show("DAQ Assistant Error. Needs reset.");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (string s in daq.AIChannels)
                ctlGraph1.Plot(s, 0, daq.GetData(s));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listBox1.SelectedItems.Count > 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string file = $"{ progFiles }\\National Instruments\\MAX\\NIMax.exe";
            if (File.Exists(file))
            {
                Process.Start(file);
            }
            else MessageBox.Show("Cannot find Measurement and Automation Explorer installed on this PC.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = NIcDAQ.taskList;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < daq.digitalInData.Length; i++)
            {
                ledArray1[i].Value = daq.digitalInData[i];
            }
        }

        private void ctlGraph1_Load(object sender, EventArgs e)
        {

        }
    }
}