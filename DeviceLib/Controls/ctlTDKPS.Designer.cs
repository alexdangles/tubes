namespace Devices
{
    partial class ctlTdkPowerSupply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlTdkPowerSupply));
            this.btnPower = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMonAmps = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSetAmps = new System.Windows.Forms.TextBox();
            this.txtMonVolts = new System.Windows.Forms.TextBox();
            this.txtSetVolts = new System.Windows.Forms.TextBox();
            this.clbStatus = new System.Windows.Forms.CheckedListBox();
            this.numVolts = new System.Windows.Forms.NumericUpDown();
            this.numAmps = new System.Windows.Forms.NumericUpDown();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnGraph = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWatts = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numVolts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmps)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPower
            // 
            this.btnPower.BackColor = System.Drawing.Color.Transparent;
            this.btnPower.BackgroundImage = global::Devices.Properties.Resources.powerOFF;
            this.btnPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPower.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPower.ForeColor = System.Drawing.Color.White;
            this.btnPower.Location = new System.Drawing.Point(10, 144);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(50, 50);
            this.btnPower.TabIndex = 2;
            this.btnPower.UseVisualStyleBackColor = false;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(14, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Volts";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(80, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Amps";
            // 
            // txtMonAmps
            // 
            this.txtMonAmps.BackColor = System.Drawing.Color.White;
            this.txtMonAmps.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonAmps.ForeColor = System.Drawing.Color.Green;
            this.txtMonAmps.Location = new System.Drawing.Point(93, 83);
            this.txtMonAmps.Name = "txtMonAmps";
            this.txtMonAmps.ReadOnly = true;
            this.txtMonAmps.Size = new System.Drawing.Size(54, 23);
            this.txtMonAmps.TabIndex = 0;
            this.txtMonAmps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "Set";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(3, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "Mon";
            // 
            // txtSetAmps
            // 
            this.txtSetAmps.BackColor = System.Drawing.Color.White;
            this.txtSetAmps.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetAmps.ForeColor = System.Drawing.Color.Green;
            this.txtSetAmps.Location = new System.Drawing.Point(93, 57);
            this.txtSetAmps.Name = "txtSetAmps";
            this.txtSetAmps.ReadOnly = true;
            this.txtSetAmps.Size = new System.Drawing.Size(54, 23);
            this.txtSetAmps.TabIndex = 0;
            this.txtSetAmps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMonVolts
            // 
            this.txtMonVolts.BackColor = System.Drawing.Color.White;
            this.txtMonVolts.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonVolts.ForeColor = System.Drawing.Color.Blue;
            this.txtMonVolts.Location = new System.Drawing.Point(35, 83);
            this.txtMonVolts.Name = "txtMonVolts";
            this.txtMonVolts.ReadOnly = true;
            this.txtMonVolts.Size = new System.Drawing.Size(54, 23);
            this.txtMonVolts.TabIndex = 0;
            this.txtMonVolts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSetVolts
            // 
            this.txtSetVolts.BackColor = System.Drawing.Color.White;
            this.txtSetVolts.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetVolts.ForeColor = System.Drawing.Color.Blue;
            this.txtSetVolts.Location = new System.Drawing.Point(35, 57);
            this.txtSetVolts.Name = "txtSetVolts";
            this.txtSetVolts.ReadOnly = true;
            this.txtSetVolts.Size = new System.Drawing.Size(54, 23);
            this.txtSetVolts.TabIndex = 0;
            this.txtSetVolts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clbStatus
            // 
            this.clbStatus.BackColor = System.Drawing.Color.LightGray;
            this.clbStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbStatus.Font = new System.Drawing.Font("Calibri", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbStatus.ForeColor = System.Drawing.Color.Black;
            this.clbStatus.FormattingEnabled = true;
            this.clbStatus.Items.AddRange(new object[] {
            "cc/cv",
            "fold",
            "ast",
            "out",
            "srf",
            "srv",
            "srt",
            "alarm",
            "ovp",
            "otp",
            "a/c fail",
            "fold",
            "prog"});
            this.clbStatus.Location = new System.Drawing.Point(150, 2);
            this.clbStatus.Name = "clbStatus";
            this.clbStatus.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.clbStatus.Size = new System.Drawing.Size(54, 195);
            this.clbStatus.TabIndex = 5;
            // 
            // numVolts
            // 
            this.numVolts.DecimalPlaces = 2;
            this.numVolts.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numVolts.ForeColor = System.Drawing.Color.Blue;
            this.numVolts.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numVolts.Location = new System.Drawing.Point(14, 24);
            this.numVolts.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numVolts.Name = "numVolts";
            this.numVolts.Size = new System.Drawing.Size(60, 27);
            this.numVolts.TabIndex = 6;
            this.numVolts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numVolts.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numVolts.ValueChanged += new System.EventHandler(this.numVolts_ValueChanged);
            // 
            // numAmps
            // 
            this.numAmps.DecimalPlaces = 2;
            this.numAmps.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAmps.ForeColor = System.Drawing.Color.Green;
            this.numAmps.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numAmps.Location = new System.Drawing.Point(82, 24);
            this.numAmps.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.numAmps.Name = "numAmps";
            this.numAmps.Size = new System.Drawing.Size(60, 27);
            this.numAmps.TabIndex = 7;
            this.numAmps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAmps.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numAmps.ValueChanged += new System.EventHandler(this.numAmps_ValueChanged);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // btnGraph
            // 
            this.btnGraph.BackColor = System.Drawing.Color.White;
            this.btnGraph.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGraph.BackgroundImage")));
            this.btnGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGraph.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraph.Location = new System.Drawing.Point(109, 115);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(32, 32);
            this.btnGraph.TabIndex = 8;
            this.btnGraph.UseVisualStyleBackColor = false;
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkRed;
            this.label5.Location = new System.Drawing.Point(4, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 14);
            this.label5.TabIndex = 1;
            this.label5.Text = "Watts";
            // 
            // txtWatts
            // 
            this.txtWatts.BackColor = System.Drawing.Color.White;
            this.txtWatts.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWatts.ForeColor = System.Drawing.Color.DarkRed;
            this.txtWatts.Location = new System.Drawing.Point(46, 115);
            this.txtWatts.Name = "txtWatts";
            this.txtWatts.ReadOnly = true;
            this.txtWatts.Size = new System.Drawing.Size(57, 23);
            this.txtWatts.TabIndex = 0;
            this.txtWatts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ctlTdkPowerSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.clbStatus);
            this.Controls.Add(this.numAmps);
            this.Controls.Add(this.txtSetAmps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numVolts);
            this.Controls.Add(this.txtMonVolts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSetVolts);
            this.Controls.Add(this.txtWatts);
            this.Controls.Add(this.txtMonAmps);
            this.Enabled = false;
            this.Name = "ctlTdkPowerSupply";
            this.Size = new System.Drawing.Size(207, 200);
            ((System.ComponentModel.ISupportInitialize)(this.numVolts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clbStatus;
        private System.Windows.Forms.TextBox txtMonAmps;
        private System.Windows.Forms.TextBox txtSetVolts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMonVolts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSetAmps;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWatts;
        public System.Windows.Forms.Button btnGraph;
        public System.Windows.Forms.Button btnPower;
        public System.Windows.Forms.NumericUpDown numAmps;
        public System.Windows.Forms.NumericUpDown numVolts;
    }
}
