namespace BurnInStns
{
    partial class frmBurnInStns
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBurnInStns));
            this.pan = new System.Windows.Forms.TableLayoutPanel();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGraph = new System.Windows.Forms.ToolStripButton();
            this.btnLog = new System.Windows.Forms.ToolStripButton();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.btnSettings = new System.Windows.Forms.ToolStripButton();
            this.btnResults = new System.Windows.Forms.ToolStripButton();
            this.btnMaintenance = new System.Windows.Forms.ToolStripButton();
            this.chkManual = new System.Windows.Forms.ToolStripButton();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSafe = new System.Windows.Forms.ToolStripLabel();
            this.lblHV = new System.Windows.Forms.ToolStripLabel();
            this.lblXrays = new System.Windows.Forms.ToolStripLabel();
            this.lblTest = new System.Windows.Forms.ToolStripLabel();
            this.lblError = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tmrPlot = new System.Windows.Forms.Timer(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tmrDbWrite = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan
            // 
            this.pan.ColumnCount = 3;
            this.pan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pan.Location = new System.Drawing.Point(2, 45);
            this.pan.Name = "pan";
            this.pan.RowCount = 5;
            this.pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pan.Size = new System.Drawing.Size(903, 774);
            this.pan.TabIndex = 10;
            // 
            // btnStart
            // 
            this.btnStart.AutoSize = false;
            this.btnStart.BackColor = System.Drawing.Color.DarkGreen;
            this.btnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStart.Font = new System.Drawing.Font("Calibri", 9F);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(50, 40);
            this.btnStart.Text = "START";
            this.btnStart.ToolTipText = "Start or pause testing.";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.AutoSize = false;
            this.btnStop.BackColor = System.Drawing.Color.Maroon;
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Calibri", 9F);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(50, 40);
            this.btnStop.Text = "STOP";
            this.btnStop.ToolTipText = "Abort all testing.";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart,
            this.btnStop,
            this.toolStripSeparator3,
            this.btnGraph,
            this.btnLog,
            this.btnConnect,
            this.btnSettings,
            this.btnResults,
            this.btnMaintenance,
            this.chkManual,
            this.btnExit,
            this.toolStripSeparator1,
            this.lblSafe,
            this.lblHV,
            this.lblXrays,
            this.lblTest,
            this.lblError,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1169, 43);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 43);
            // 
            // btnGraph
            // 
            this.btnGraph.AutoSize = false;
            this.btnGraph.BackColor = System.Drawing.Color.Transparent;
            this.btnGraph.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGraph.BackgroundImage")));
            this.btnGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGraph.Enabled = false;
            this.btnGraph.Font = new System.Drawing.Font("Calibri", 9F);
            this.btnGraph.ForeColor = System.Drawing.Color.White;
            this.btnGraph.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGraph.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(35, 35);
            this.btnGraph.ToolTipText = "Show/Hide Tube Data.";
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // btnLog
            // 
            this.btnLog.AutoSize = false;
            this.btnLog.BackColor = System.Drawing.Color.Transparent;
            this.btnLog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLog.BackgroundImage")));
            this.btnLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLog.Font = new System.Drawing.Font("Calibri", 9F);
            this.btnLog.ForeColor = System.Drawing.Color.White;
            this.btnLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLog.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(35, 35);
            this.btnLog.ToolTipText = "Open Log File.";
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.AutoSize = false;
            this.btnConnect.BackColor = System.Drawing.Color.Transparent;
            this.btnConnect.BackgroundImage = global::BurnInStns.Properties.Resources.connect;
            this.btnConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(32, 32);
            this.btnConnect.ToolTipText = "Reconnect all stations.";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.AutoSize = false;
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.BackgroundImage = global::BurnInStns.Properties.Resources.settings;
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSettings.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(35, 35);
            this.btnSettings.ToolTipText = "Show/Hide Test settings.";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnResults
            // 
            this.btnResults.AutoSize = false;
            this.btnResults.BackColor = System.Drawing.Color.Transparent;
            this.btnResults.BackgroundImage = global::BurnInStns.Properties.Resources.results;
            this.btnResults.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnResults.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResults.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnResults.Name = "btnResults";
            this.btnResults.Size = new System.Drawing.Size(32, 32);
            this.btnResults.ToolTipText = "Show/Hide Results Table.";
            this.btnResults.Click += new System.EventHandler(this.btnResults_Click);
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.AutoSize = false;
            this.btnMaintenance.BackColor = System.Drawing.Color.Transparent;
            this.btnMaintenance.BackgroundImage = global::BurnInStns.Properties.Resources.maintenance;
            this.btnMaintenance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMaintenance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaintenance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaintenance.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(32, 32);
            this.btnMaintenance.ToolTipText = "Show/Hide Maintenance tools";
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // chkManual
            // 
            this.chkManual.AutoSize = false;
            this.chkManual.BackColor = System.Drawing.Color.Transparent;
            this.chkManual.BackgroundImage = global::BurnInStns.Properties.Resources.manual;
            this.chkManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.chkManual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.chkManual.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkManual.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.chkManual.Name = "chkManual";
            this.chkManual.Size = new System.Drawing.Size(32, 32);
            this.chkManual.Text = "Manual";
            this.chkManual.ToolTipText = "Take Manual Control of Each Station.";
            this.chkManual.Click += new System.EventHandler(this.chkManual_Click);
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = false;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::BurnInStns.Properties.Resources.Exit;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(32, 32);
            this.btnExit.ToolTipText = "Quit the program.";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 43);
            // 
            // lblSafe
            // 
            this.lblSafe.AutoSize = false;
            this.lblSafe.BackColor = System.Drawing.Color.LightGreen;
            this.lblSafe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblSafe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSafe.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.lblSafe.Name = "lblSafe";
            this.lblSafe.Size = new System.Drawing.Size(34, 32);
            this.lblSafe.Text = "SAFE";
            // 
            // lblHV
            // 
            this.lblHV.AutoSize = false;
            this.lblHV.BackColor = System.Drawing.Color.White;
            this.lblHV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblHV.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHV.Name = "lblHV";
            this.lblHV.Size = new System.Drawing.Size(34, 32);
            this.lblHV.Text = "HV";
            // 
            // lblXrays
            // 
            this.lblXrays.ActiveLinkColor = System.Drawing.Color.Red;
            this.lblXrays.AutoSize = false;
            this.lblXrays.BackColor = System.Drawing.Color.LightBlue;
            this.lblXrays.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblXrays.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblXrays.Name = "lblXrays";
            this.lblXrays.Size = new System.Drawing.Size(38, 32);
            this.lblXrays.Text = "XRAYS";
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = false;
            this.lblTest.BackColor = System.Drawing.Color.Gold;
            this.lblTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblTest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(34, 32);
            this.lblTest.Text = "TEST";
            // 
            // lblError
            // 
            this.lblError.ActiveLinkColor = System.Drawing.Color.Red;
            this.lblError.AutoSize = false;
            this.lblError.BackColor = System.Drawing.Color.IndianRed;
            this.lblError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblError.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(40, 32);
            this.lblError.Text = "ERROR";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 43);
            // 
            // tmrPlot
            // 
            this.tmrPlot.Interval = 500;
            this.tmrPlot.Tick += new System.EventHandler(this.tmrPlot_Tick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Bisque;
            this.richTextBox1.Location = new System.Drawing.Point(909, 45);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ShowSelectionMargin = true;
            this.richTextBox1.Size = new System.Drawing.Size(260, 773);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "Console Window";
            // 
            // tmrDbWrite
            // 
            this.tmrDbWrite.Interval = 1000;
            this.tmrDbWrite.Tick += new System.EventHandler(this.tmrDbWrite_Tick);
            // 
            // frmBurnInStns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1169, 823);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pan);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBurnInStns";
            this.Text = "Burn-in Stations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBurnInStns_FormClosing);
            this.Shown += new System.EventHandler(this.frmBurnInStns_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel pan;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnGraph;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnLog;
        private System.Windows.Forms.Timer tmrPlot;
        private System.Windows.Forms.ToolStripButton btnSettings;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripButton btnMaintenance;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnResults;
        private System.Windows.Forms.ToolStripButton chkManual;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblSafe;
        private System.Windows.Forms.ToolStripLabel lblHV;
        private System.Windows.Forms.ToolStripLabel lblXrays;
        private System.Windows.Forms.ToolStripLabel lblTest;
        private System.Windows.Forms.ToolStripLabel lblError;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.Timer tmrDbWrite;
    }
}