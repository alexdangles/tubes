namespace Devices.Controls
{
    partial class ctlEnvChamber
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
            this.thmAirTemp = new NationalInstruments.UI.WindowsForms.Thermometer();
            this.thmPartTemp = new NationalInstruments.UI.WindowsForms.Thermometer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.thmHumi = new NationalInstruments.UI.WindowsForms.Thermometer();
            this.numSetHumi = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkPart = new System.Windows.Forms.CheckBox();
            this.numSetPartTemp = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numSetTemp = new System.Windows.Forms.NumericUpDown();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.thmAirTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thmPartTemp)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thmHumi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetHumi)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetPartTemp)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetTemp)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // thmAirTemp
            // 
            this.thmAirTemp.Caption = "Ambient Temp. (°C)";
            this.thmAirTemp.CaptionBackColor = System.Drawing.Color.Transparent;
            this.thmAirTemp.CaptionFont = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thmAirTemp.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left;
            this.thmAirTemp.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval;
            this.thmAirTemp.Dock = System.Windows.Forms.DockStyle.Top;
            this.thmAirTemp.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thmAirTemp.HoverInterval = System.TimeSpan.Parse("00:00:00.0500000");
            this.thmAirTemp.InteractionMode = NationalInstruments.UI.LinearNumericPointerInteractionModes.EditRange;
            this.thmAirTemp.Location = new System.Drawing.Point(0, 0);
            this.thmAirTemp.Margin = new System.Windows.Forms.Padding(0);
            this.thmAirTemp.Name = "thmAirTemp";
            this.thmAirTemp.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.thmAirTemp.Range = new NationalInstruments.UI.Range(-20D, 100D);
            this.thmAirTemp.Size = new System.Drawing.Size(116, 225);
            this.thmAirTemp.TabIndex = 0;
            this.thmAirTemp.ThermometerStyle = NationalInstruments.UI.ThermometerStyle.Flat;
            this.thmAirTemp.Value = 25D;
            // 
            // thmPartTemp
            // 
            this.thmPartTemp.Caption = "Part Temp. (°C)";
            this.thmPartTemp.CaptionBackColor = System.Drawing.Color.Transparent;
            this.thmPartTemp.CaptionFont = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thmPartTemp.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left;
            this.thmPartTemp.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval;
            this.thmPartTemp.Dock = System.Windows.Forms.DockStyle.Top;
            this.thmPartTemp.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thmPartTemp.HoverInterval = System.TimeSpan.Parse("00:00:00.0500000");
            this.thmPartTemp.InteractionMode = NationalInstruments.UI.LinearNumericPointerInteractionModes.EditRange;
            this.thmPartTemp.Location = new System.Drawing.Point(0, 0);
            this.thmPartTemp.Margin = new System.Windows.Forms.Padding(0);
            this.thmPartTemp.Name = "thmPartTemp";
            this.thmPartTemp.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.thmPartTemp.Range = new NationalInstruments.UI.Range(-20D, 100D);
            this.thmPartTemp.Size = new System.Drawing.Size(117, 225);
            this.thmPartTemp.TabIndex = 0;
            this.thmPartTemp.ThermometerStyle = NationalInstruments.UI.ThermometerStyle.Flat;
            this.thmPartTemp.Value = 25D;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(419, 257);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.thmHumi);
            this.panel3.Controls.Add(this.numSetHumi);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(248, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(117, 251);
            this.panel3.TabIndex = 3;
            // 
            // thmHumi
            // 
            this.thmHumi.Caption = "Humidity (%RH)";
            this.thmHumi.CaptionBackColor = System.Drawing.Color.Transparent;
            this.thmHumi.CaptionFont = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thmHumi.CaptionPosition = NationalInstruments.UI.CaptionPosition.Left;
            this.thmHumi.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval;
            this.thmHumi.Dock = System.Windows.Forms.DockStyle.Top;
            this.thmHumi.FillColor = System.Drawing.Color.Green;
            this.thmHumi.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thmHumi.HoverInterval = System.TimeSpan.Parse("00:00:00.0500000");
            this.thmHumi.InteractionMode = NationalInstruments.UI.LinearNumericPointerInteractionModes.EditRange;
            this.thmHumi.Location = new System.Drawing.Point(0, 0);
            this.thmHumi.Margin = new System.Windows.Forms.Padding(0);
            this.thmHumi.Name = "thmHumi";
            this.thmHumi.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.thmHumi.Size = new System.Drawing.Size(117, 225);
            this.thmHumi.TabIndex = 0;
            this.thmHumi.ThermometerStyle = NationalInstruments.UI.ThermometerStyle.Flat;
            this.thmHumi.Value = 25D;
            // 
            // numSetHumi
            // 
            this.numSetHumi.BackColor = System.Drawing.Color.Gainsboro;
            this.numSetHumi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numSetHumi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.numSetHumi.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSetHumi.ForeColor = System.Drawing.Color.DarkGreen;
            this.numSetHumi.Location = new System.Drawing.Point(0, 225);
            this.numSetHumi.Name = "numSetHumi";
            this.numSetHumi.Size = new System.Drawing.Size(117, 26);
            this.numSetHumi.TabIndex = 2;
            this.numSetHumi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSetHumi.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numSetHumi.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkPart);
            this.panel1.Controls.Add(this.thmPartTemp);
            this.panel1.Controls.Add(this.numSetPartTemp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(125, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(117, 251);
            this.panel1.TabIndex = 2;
            // 
            // chkPart
            // 
            this.chkPart.AutoSize = true;
            this.chkPart.Location = new System.Drawing.Point(0, 208);
            this.chkPart.Name = "chkPart";
            this.chkPart.Size = new System.Drawing.Size(59, 17);
            this.chkPart.TabIndex = 3;
            this.chkPart.Text = "Enable";
            this.chkPart.UseVisualStyleBackColor = true;
            this.chkPart.CheckedChanged += new System.EventHandler(this.chkPart_CheckedChanged);
            // 
            // numSetPartTemp
            // 
            this.numSetPartTemp.BackColor = System.Drawing.Color.Gainsboro;
            this.numSetPartTemp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numSetPartTemp.DecimalPlaces = 1;
            this.numSetPartTemp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.numSetPartTemp.Enabled = false;
            this.numSetPartTemp.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSetPartTemp.ForeColor = System.Drawing.Color.Red;
            this.numSetPartTemp.Location = new System.Drawing.Point(0, 225);
            this.numSetPartTemp.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numSetPartTemp.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            -2147483648});
            this.numSetPartTemp.Name = "numSetPartTemp";
            this.numSetPartTemp.Size = new System.Drawing.Size(117, 26);
            this.numSetPartTemp.TabIndex = 2;
            this.numSetPartTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSetPartTemp.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numSetPartTemp.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numSetPartTemp.ValueChanged += new System.EventHandler(this.numSet_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.thmAirTemp);
            this.panel2.Controls.Add(this.numSetTemp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(116, 251);
            this.panel2.TabIndex = 0;
            // 
            // numSetTemp
            // 
            this.numSetTemp.BackColor = System.Drawing.Color.Gainsboro;
            this.numSetTemp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numSetTemp.DecimalPlaces = 1;
            this.numSetTemp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.numSetTemp.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSetTemp.ForeColor = System.Drawing.Color.Red;
            this.numSetTemp.Location = new System.Drawing.Point(0, 225);
            this.numSetTemp.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numSetTemp.Minimum = new decimal(new int[] {
            80,
            0,
            0,
            -2147483648});
            this.numSetTemp.Name = "numSetTemp";
            this.numSetTemp.Size = new System.Drawing.Size(116, 26);
            this.numSetTemp.TabIndex = 2;
            this.numSetTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSetTemp.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numSetTemp.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numSetTemp.ValueChanged += new System.EventHandler(this.numSet_ValueChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnPower);
            this.panel4.Controls.Add(this.btnSet);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(371, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(45, 251);
            this.panel4.TabIndex = 4;
            // 
            // btnPower
            // 
            this.btnPower.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPower.BackColor = System.Drawing.Color.Gainsboro;
            this.btnPower.BackgroundImage = global::Devices.Properties.Resources.powerOFF;
            this.btnPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPower.FlatAppearance.BorderSize = 0;
            this.btnPower.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPower.Location = new System.Drawing.Point(1, 1);
            this.btnPower.Margin = new System.Windows.Forms.Padding(1);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(43, 42);
            this.btnPower.TabIndex = 4;
            this.btnPower.UseVisualStyleBackColor = false;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSet.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSet.FlatAppearance.BorderSize = 0;
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSet.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet.Location = new System.Drawing.Point(1, 225);
            this.btnSet.Margin = new System.Windows.Forms.Padding(1);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(43, 25);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // ctlEnvChamber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Enabled = false;
            this.Name = "ctlEnvChamber";
            this.Size = new System.Drawing.Size(422, 260);
            ((System.ComponentModel.ISupportInitialize)(this.thmAirTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thmPartTemp)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thmHumi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetHumi)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetPartTemp)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSetTemp)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NationalInstruments.UI.WindowsForms.Thermometer thmAirTemp;
        private NationalInstruments.UI.WindowsForms.Thermometer thmPartTemp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private NationalInstruments.UI.WindowsForms.Thermometer thmHumi;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.NumericUpDown numSetHumi;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.NumericUpDown numSetPartTemp;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.NumericUpDown numSetTemp;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Button btnSet;
        public System.Windows.Forms.Button btnPower;
        public System.Windows.Forms.CheckBox chkPart;
    }
}
