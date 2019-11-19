namespace Devices.Controls
{
    partial class ctlSpellmanPS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlSpellmanPS));
            this.numkV = new System.Windows.Forms.NumericUpDown();
            this.numuA = new System.Windows.Forms.NumericUpDown();
            this.numFil = new System.Windows.Forms.NumericUpDown();
            this.numFilPreHeat = new System.Windows.Forms.NumericUpDown();
            this.txtSetkV = new System.Windows.Forms.TextBox();
            this.txtSetuA = new System.Windows.Forms.TextBox();
            this.txtSetFil = new System.Windows.Forms.TextBox();
            this.txtMonkV = new System.Windows.Forms.TextBox();
            this.txtMonuA = new System.Windows.Forms.TextBox();
            this.txtMonFil = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblHardwareVersion = new System.Windows.Forms.Label();
            this.lblSoftwareVersion = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.clbFaults = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.txtWatts = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnGraph = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numkV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numuA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilPreHeat)).BeginInit();
            this.SuspendLayout();
            // 
            // numkV
            // 
            this.numkV.BackColor = System.Drawing.Color.Gainsboro;
            this.numkV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numkV.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numkV.ForeColor = System.Drawing.Color.Blue;
            this.numkV.Location = new System.Drawing.Point(83, 30);
            this.numkV.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.numkV.Name = "numkV";
            this.numkV.Size = new System.Drawing.Size(80, 26);
            this.numkV.TabIndex = 1;
            this.numkV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numkV.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numkV.ValueChanged += new System.EventHandler(this.numkV_ValueChanged);
            // 
            // numuA
            // 
            this.numuA.BackColor = System.Drawing.Color.Gainsboro;
            this.numuA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numuA.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numuA.ForeColor = System.Drawing.Color.Green;
            this.numuA.Location = new System.Drawing.Point(169, 30);
            this.numuA.Maximum = new decimal(new int[] {
            2180,
            0,
            0,
            0});
            this.numuA.Name = "numuA";
            this.numuA.Size = new System.Drawing.Size(80, 26);
            this.numuA.TabIndex = 1;
            this.numuA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numuA.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numuA.ValueChanged += new System.EventHandler(this.numuA_ValueChanged);
            // 
            // numFil
            // 
            this.numFil.BackColor = System.Drawing.Color.Gainsboro;
            this.numFil.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numFil.DecimalPlaces = 2;
            this.numFil.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numFil.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.numFil.Location = new System.Drawing.Point(262, 30);
            this.numFil.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numFil.Name = "numFil";
            this.numFil.Size = new System.Drawing.Size(80, 26);
            this.numFil.TabIndex = 1;
            this.numFil.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFil.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numFil.ValueChanged += new System.EventHandler(this.numFil_ValueChanged);
            // 
            // numFilPreHeat
            // 
            this.numFilPreHeat.BackColor = System.Drawing.Color.Gainsboro;
            this.numFilPreHeat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numFilPreHeat.DecimalPlaces = 2;
            this.numFilPreHeat.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numFilPreHeat.ForeColor = System.Drawing.Color.Indigo;
            this.numFilPreHeat.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numFilPreHeat.Location = new System.Drawing.Point(348, 30);
            this.numFilPreHeat.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numFilPreHeat.Name = "numFilPreHeat";
            this.numFilPreHeat.Size = new System.Drawing.Size(80, 26);
            this.numFilPreHeat.TabIndex = 1;
            this.numFilPreHeat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFilPreHeat.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numFilPreHeat.ValueChanged += new System.EventHandler(this.numFilPreHeat_ValueChanged);
            // 
            // txtSetkV
            // 
            this.txtSetkV.BackColor = System.Drawing.Color.White;
            this.txtSetkV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSetkV.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetkV.ForeColor = System.Drawing.Color.Blue;
            this.txtSetkV.Location = new System.Drawing.Point(83, 62);
            this.txtSetkV.Name = "txtSetkV";
            this.txtSetkV.Size = new System.Drawing.Size(80, 27);
            this.txtSetkV.TabIndex = 0;
            this.txtSetkV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSetuA
            // 
            this.txtSetuA.BackColor = System.Drawing.Color.White;
            this.txtSetuA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSetuA.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetuA.ForeColor = System.Drawing.Color.Green;
            this.txtSetuA.Location = new System.Drawing.Point(169, 62);
            this.txtSetuA.Name = "txtSetuA";
            this.txtSetuA.Size = new System.Drawing.Size(80, 27);
            this.txtSetuA.TabIndex = 0;
            this.txtSetuA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSetFil
            // 
            this.txtSetFil.BackColor = System.Drawing.Color.White;
            this.txtSetFil.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSetFil.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSetFil.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.txtSetFil.Location = new System.Drawing.Point(262, 62);
            this.txtSetFil.Name = "txtSetFil";
            this.txtSetFil.Size = new System.Drawing.Size(80, 27);
            this.txtSetFil.TabIndex = 0;
            this.txtSetFil.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMonkV
            // 
            this.txtMonkV.BackColor = System.Drawing.Color.White;
            this.txtMonkV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMonkV.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonkV.ForeColor = System.Drawing.Color.Blue;
            this.txtMonkV.Location = new System.Drawing.Point(83, 95);
            this.txtMonkV.Name = "txtMonkV";
            this.txtMonkV.Size = new System.Drawing.Size(80, 27);
            this.txtMonkV.TabIndex = 0;
            this.txtMonkV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMonuA
            // 
            this.txtMonuA.BackColor = System.Drawing.Color.White;
            this.txtMonuA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMonuA.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonuA.ForeColor = System.Drawing.Color.Green;
            this.txtMonuA.Location = new System.Drawing.Point(169, 95);
            this.txtMonuA.Name = "txtMonuA";
            this.txtMonuA.Size = new System.Drawing.Size(80, 27);
            this.txtMonuA.TabIndex = 0;
            this.txtMonuA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMonFil
            // 
            this.txtMonFil.BackColor = System.Drawing.Color.White;
            this.txtMonFil.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMonFil.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonFil.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.txtMonFil.Location = new System.Drawing.Point(262, 95);
            this.txtMonFil.Name = "txtMonFil";
            this.txtMonFil.Size = new System.Drawing.Size(80, 27);
            this.txtMonFil.TabIndex = 0;
            this.txtMonFil.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(115, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "kV";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(198, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "uA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.label3.Location = new System.Drawing.Point(259, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Fil. Limit (A)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Indigo;
            this.label4.Location = new System.Drawing.Point(348, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fil. Pre-heat (A)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(46, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Set";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(19, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Monitor";
            // 
            // lblHardwareVersion
            // 
            this.lblHardwareVersion.AutoSize = true;
            this.lblHardwareVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblHardwareVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHardwareVersion.ForeColor = System.Drawing.Color.Black;
            this.lblHardwareVersion.Location = new System.Drawing.Point(18, 153);
            this.lblHardwareVersion.Name = "lblHardwareVersion";
            this.lblHardwareVersion.Size = new System.Drawing.Size(93, 19);
            this.lblHardwareVersion.TabIndex = 3;
            this.lblHardwareVersion.Text = "HW Version: ";
            // 
            // lblSoftwareVersion
            // 
            this.lblSoftwareVersion.AutoSize = true;
            this.lblSoftwareVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblSoftwareVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoftwareVersion.ForeColor = System.Drawing.Color.Black;
            this.lblSoftwareVersion.Location = new System.Drawing.Point(18, 172);
            this.lblSoftwareVersion.Name = "lblSoftwareVersion";
            this.lblSoftwareVersion.Size = new System.Drawing.Size(90, 19);
            this.lblSoftwareVersion.TabIndex = 3;
            this.lblSoftwareVersion.Text = "SW Version: ";
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.BackColor = System.Drawing.Color.Transparent;
            this.lblModel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModel.ForeColor = System.Drawing.Color.Black;
            this.lblModel.Location = new System.Drawing.Point(20, 134);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(58, 19);
            this.lblModel.TabIndex = 3;
            this.lblModel.Text = "Model: ";
            // 
            // clbFaults
            // 
            this.clbFaults.BackColor = System.Drawing.Color.Gainsboro;
            this.clbFaults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbFaults.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbFaults.ForeColor = System.Drawing.Color.Black;
            this.clbFaults.FormattingEnabled = true;
            this.clbFaults.Items.AddRange(new object[] {
            "ARC",
            "Over Temp.",
            "Over Volt.",
            "Under Volt.",
            "Over Cur.",
            "Under Cur."});
            this.clbFaults.Location = new System.Drawing.Point(350, 85);
            this.clbFaults.MultiColumn = true;
            this.clbFaults.Name = "clbFaults";
            this.clbFaults.Size = new System.Drawing.Size(85, 96);
            this.clbFaults.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(366, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 14);
            this.label7.TabIndex = 3;
            this.label7.Text = "Faults";
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Gray;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(350, 183);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(85, 19);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // txtWatts
            // 
            this.txtWatts.BackColor = System.Drawing.Color.White;
            this.txtWatts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWatts.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWatts.ForeColor = System.Drawing.Color.Crimson;
            this.txtWatts.Location = new System.Drawing.Point(262, 151);
            this.txtWatts.Name = "txtWatts";
            this.txtWatts.Size = new System.Drawing.Size(80, 27);
            this.txtWatts.TabIndex = 0;
            this.txtWatts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Crimson;
            this.label8.Location = new System.Drawing.Point(265, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Watts";
            // 
            // btnPower
            // 
            this.btnPower.BackColor = System.Drawing.Color.Transparent;
            this.btnPower.BackgroundImage = global::Devices.Properties.Resources.powerOFF;
            this.btnPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPower.FlatAppearance.BorderSize = 0;
            this.btnPower.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPower.ForeColor = System.Drawing.Color.White;
            this.btnPower.Location = new System.Drawing.Point(5, 17);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(50, 50);
            this.btnPower.TabIndex = 0;
            this.btnPower.UseVisualStyleBackColor = false;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // btnGraph
            // 
            this.btnGraph.BackColor = System.Drawing.Color.White;
            this.btnGraph.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGraph.BackgroundImage")));
            this.btnGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGraph.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGraph.Location = new System.Drawing.Point(217, 128);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(32, 32);
            this.btnGraph.TabIndex = 8;
            this.btnGraph.UseVisualStyleBackColor = false;
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(3, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 14);
            this.label9.TabIndex = 3;
            this.label9.Text = "Power";
            // 
            // ctlSpellmanPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.numFilPreHeat);
            this.Controls.Add(this.numFil);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.numuA);
            this.Controls.Add(this.numkV);
            this.Controls.Add(this.clbFaults);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.lblSoftwareVersion);
            this.Controls.Add(this.lblHardwareVersion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMonFil);
            this.Controls.Add(this.txtSetFil);
            this.Controls.Add(this.txtWatts);
            this.Controls.Add(this.txtMonuA);
            this.Controls.Add(this.txtSetuA);
            this.Controls.Add(this.txtMonkV);
            this.Controls.Add(this.txtSetkV);
            this.Enabled = false;
            this.Name = "ctlSpellmanPS";
            this.Size = new System.Drawing.Size(466, 213);
            ((System.ComponentModel.ISupportInitialize)(this.numkV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numuA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFilPreHeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSetkV;
        private System.Windows.Forms.TextBox txtSetuA;
        private System.Windows.Forms.TextBox txtSetFil;
        private System.Windows.Forms.TextBox txtMonkV;
        private System.Windows.Forms.TextBox txtMonuA;
        private System.Windows.Forms.TextBox txtMonFil;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblHardwareVersion;
        private System.Windows.Forms.Label lblSoftwareVersion;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.CheckedListBox clbFaults;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.TextBox txtWatts;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.NumericUpDown numkV;
        public System.Windows.Forms.NumericUpDown numuA;
        public System.Windows.Forms.NumericUpDown numFil;
        public System.Windows.Forms.NumericUpDown numFilPreHeat;
        public System.Windows.Forms.Button btnPower;
        public System.Windows.Forms.Button btnReset;
        public System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Label label9;
    }
}
