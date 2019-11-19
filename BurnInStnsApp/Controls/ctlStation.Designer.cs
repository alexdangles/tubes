namespace BurnInStns
{
    partial class ctlStation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.ledStatus = new NationalInstruments.UI.WindowsForms.Led();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.tmrLog = new System.Windows.Forms.Timer(this.components);
            this.numkV = new System.Windows.Forms.NumericUpDown();
            this.numuA = new System.Windows.Forms.NumericUpDown();
            this.btnPower = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblkV = new System.Windows.Forms.Label();
            this.lbluA = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.picTube = new System.Windows.Forms.PictureBox();
            this.ledWatchdog = new NationalInstruments.UI.WindowsForms.Led();
            ((System.ComponentModel.ISupportInitialize)(this.ledStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numkV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numuA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledWatchdog)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(2, 128);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(289, 10);
            this.progressBar.TabIndex = 3;
            // 
            // ledStatus
            // 
            this.ledStatus.Caption = " ";
            this.ledStatus.CaptionBackColor = System.Drawing.Color.Transparent;
            this.ledStatus.CaptionFont = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ledStatus.CaptionForeColor = System.Drawing.Color.White;
            this.ledStatus.Location = new System.Drawing.Point(0, 0);
            this.ledStatus.Name = "ledStatus";
            this.ledStatus.OffColor = System.Drawing.Color.Gray;
            this.ledStatus.OnColor = System.Drawing.Color.DimGray;
            this.ledStatus.Size = new System.Drawing.Size(58, 75);
            this.ledStatus.TabIndex = 4;
            this.ledStatus.TabStop = false;
            // 
            // txtInfo
            // 
            this.txtInfo.BackColor = System.Drawing.Color.Black;
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfo.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInfo.ForeColor = System.Drawing.Color.White;
            this.txtInfo.Location = new System.Drawing.Point(64, -1);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(118, 104);
            this.txtInfo.TabIndex = 6;
            this.txtInfo.TabStop = false;
            this.txtInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(4, 106);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 17);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // chkLog
            // 
            this.chkLog.AutoSize = true;
            this.chkLog.BackColor = System.Drawing.Color.Transparent;
            this.chkLog.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLog.ForeColor = System.Drawing.Color.White;
            this.chkLog.Location = new System.Drawing.Point(7, 81);
            this.chkLog.Name = "chkLog";
            this.chkLog.Size = new System.Drawing.Size(47, 21);
            this.chkLog.TabIndex = 8;
            this.chkLog.Text = "Log";
            this.toolTip1.SetToolTip(this.chkLog, "Check to log tube data.");
            this.chkLog.UseVisualStyleBackColor = false;
            this.chkLog.CheckedChanged += new System.EventHandler(this.chkLog_CheckedChanged);
            // 
            // tmrLog
            // 
            this.tmrLog.Interval = 1000;
            this.tmrLog.Tick += new System.EventHandler(this.tmrLog_Tick);
            // 
            // numkV
            // 
            this.numkV.BackColor = System.Drawing.Color.DimGray;
            this.numkV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numkV.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numkV.ForeColor = System.Drawing.Color.White;
            this.numkV.Location = new System.Drawing.Point(86, 55);
            this.numkV.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numkV.Name = "numkV";
            this.numkV.Size = new System.Drawing.Size(54, 20);
            this.numkV.TabIndex = 1;
            this.numkV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numkV, "Set tube kV");
            this.numkV.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numkV.ValueChanged += new System.EventHandler(this.numkV_ValueChanged);
            // 
            // numuA
            // 
            this.numuA.BackColor = System.Drawing.Color.DimGray;
            this.numuA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numuA.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numuA.ForeColor = System.Drawing.Color.White;
            this.numuA.Location = new System.Drawing.Point(86, 81);
            this.numuA.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numuA.Name = "numuA";
            this.numuA.Size = new System.Drawing.Size(54, 20);
            this.numuA.TabIndex = 1;
            this.numuA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numuA, "Set tube uA");
            this.numuA.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numuA.ValueChanged += new System.EventHandler(this.numuA_ValueChanged);
            // 
            // btnPower
            // 
            this.btnPower.BackColor = System.Drawing.Color.DimGray;
            this.btnPower.FlatAppearance.BorderSize = 0;
            this.btnPower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPower.ForeColor = System.Drawing.Color.White;
            this.btnPower.Location = new System.Drawing.Point(86, 0);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(54, 31);
            this.btnPower.TabIndex = 0;
            this.btnPower.Text = "Power";
            this.toolTip1.SetToolTip(this.btnPower, "Turn tube on or off");
            this.btnPower.UseVisualStyleBackColor = false;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(62, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "kV";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(62, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "uA";
            // 
            // lblkV
            // 
            this.lblkV.AutoSize = true;
            this.lblkV.BackColor = System.Drawing.Color.Transparent;
            this.lblkV.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkV.ForeColor = System.Drawing.Color.White;
            this.lblkV.Location = new System.Drawing.Point(141, 55);
            this.lblkV.Name = "lblkV";
            this.lblkV.Size = new System.Drawing.Size(33, 17);
            this.lblkV.TabIndex = 3;
            this.lblkV.Text = "0.00";
            // 
            // lbluA
            // 
            this.lbluA.AutoSize = true;
            this.lbluA.BackColor = System.Drawing.Color.Transparent;
            this.lbluA.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbluA.ForeColor = System.Drawing.Color.White;
            this.lbluA.Location = new System.Drawing.Point(141, 81);
            this.lbluA.Name = "lbluA";
            this.lbluA.Size = new System.Drawing.Size(33, 17);
            this.lbluA.TabIndex = 3;
            this.lbluA.Text = "0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(88, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Set";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(138, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Mon.";
            // 
            // picTube
            // 
            this.picTube.InitialImage = null;
            this.picTube.Location = new System.Drawing.Point(180, -1);
            this.picTube.Name = "picTube";
            this.picTube.Size = new System.Drawing.Size(112, 104);
            this.picTube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTube.TabIndex = 5;
            this.picTube.TabStop = false;
            this.picTube.MouseHover += new System.EventHandler(this.picTube_MouseHover);
            // 
            // ledWatchdog
            // 
            this.ledWatchdog.Caption = " ";
            this.ledWatchdog.CaptionBackColor = System.Drawing.Color.Transparent;
            this.ledWatchdog.CaptionFont = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ledWatchdog.CaptionForeColor = System.Drawing.Color.White;
            this.ledWatchdog.CaptionVisible = false;
            this.ledWatchdog.Location = new System.Drawing.Point(146, 1);
            this.ledWatchdog.Name = "ledWatchdog";
            this.ledWatchdog.OffColor = System.Drawing.Color.Gray;
            this.ledWatchdog.OnColor = System.Drawing.Color.LightGreen;
            this.ledWatchdog.Size = new System.Drawing.Size(28, 28);
            this.ledWatchdog.TabIndex = 4;
            this.ledWatchdog.TabStop = false;
            this.toolTip1.SetToolTip(this.ledWatchdog, "WatchDog Status");
            // 
            // ctlStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.chkLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.picTube);
            this.Controls.Add(this.ledStatus);
            this.Controls.Add(this.lbluA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblkV);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.numuA);
            this.Controls.Add(this.numkV);
            this.Controls.Add(this.ledWatchdog);
            this.Name = "ctlStation";
            this.Size = new System.Drawing.Size(295, 141);
            ((System.ComponentModel.ISupportInitialize)(this.ledStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numkV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numuA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledWatchdog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.PictureBox picTube;
        public NationalInstruments.UI.WindowsForms.Led ledStatus;
        private System.Windows.Forms.Timer tmrLog;
        public System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.NumericUpDown numkV;
        public System.Windows.Forms.NumericUpDown numuA;
        public System.Windows.Forms.Button btnPower;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblkV;
        public System.Windows.Forms.Label lbluA;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public NationalInstruments.UI.WindowsForms.Led ledWatchdog;
        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.CheckBox chkLog;
    }
}
