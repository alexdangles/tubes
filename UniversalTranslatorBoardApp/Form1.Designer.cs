namespace UniversalTranslatorBoardApp
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.chkGreen = new System.Windows.Forms.CheckBox();
            this.chkYellow = new System.Windows.Forms.CheckBox();
            this.chkRed = new System.Windows.Forms.CheckBox();
            this.cbxPSSelect = new System.Windows.Forms.ComboBox();
            this.numSetkV = new System.Windows.Forms.NumericUpDown();
            this.numSetuA = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.txtMonkV = new System.Windows.Forms.TextBox();
            this.txtMonuA = new System.Windows.Forms.TextBox();
            this.numAdjVolts = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numSetkV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetuA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAdjVolts)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 500;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // chkGreen
            // 
            this.chkGreen.AutoSize = true;
            this.chkGreen.Location = new System.Drawing.Point(145, 183);
            this.chkGreen.Name = "chkGreen";
            this.chkGreen.Size = new System.Drawing.Size(55, 17);
            this.chkGreen.TabIndex = 0;
            this.chkGreen.Text = "Green";
            this.chkGreen.UseVisualStyleBackColor = true;
            this.chkGreen.CheckedChanged += new System.EventHandler(this.chkLEDs_CheckedChanged);
            // 
            // chkYellow
            // 
            this.chkYellow.AutoSize = true;
            this.chkYellow.Location = new System.Drawing.Point(145, 206);
            this.chkYellow.Name = "chkYellow";
            this.chkYellow.Size = new System.Drawing.Size(57, 17);
            this.chkYellow.TabIndex = 0;
            this.chkYellow.Text = "Yellow";
            this.chkYellow.UseVisualStyleBackColor = true;
            this.chkYellow.CheckedChanged += new System.EventHandler(this.chkLEDs_CheckedChanged);
            // 
            // chkRed
            // 
            this.chkRed.AutoSize = true;
            this.chkRed.Location = new System.Drawing.Point(145, 229);
            this.chkRed.Name = "chkRed";
            this.chkRed.Size = new System.Drawing.Size(46, 17);
            this.chkRed.TabIndex = 0;
            this.chkRed.Text = "Red";
            this.chkRed.UseVisualStyleBackColor = true;
            this.chkRed.CheckedChanged += new System.EventHandler(this.chkLEDs_CheckedChanged);
            // 
            // cbxPSSelect
            // 
            this.cbxPSSelect.FormattingEnabled = true;
            this.cbxPSSelect.Location = new System.Drawing.Point(90, 40);
            this.cbxPSSelect.Name = "cbxPSSelect";
            this.cbxPSSelect.Size = new System.Drawing.Size(121, 21);
            this.cbxPSSelect.TabIndex = 1;
            this.cbxPSSelect.SelectionChangeCommitted += new System.EventHandler(this.cbxPSSelect_SelectionChangeCommitted);
            // 
            // numSetkV
            // 
            this.numSetkV.Location = new System.Drawing.Point(20, 123);
            this.numSetkV.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numSetkV.Name = "numSetkV";
            this.numSetkV.Size = new System.Drawing.Size(69, 20);
            this.numSetkV.TabIndex = 2;
            this.numSetkV.ValueChanged += new System.EventHandler(this.numSetkV_ValueChanged);
            // 
            // numSetuA
            // 
            this.numSetuA.Location = new System.Drawing.Point(95, 123);
            this.numSetuA.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numSetuA.Name = "numSetuA";
            this.numSetuA.Size = new System.Drawing.Size(69, 20);
            this.numSetuA.TabIndex = 2;
            this.numSetuA.ValueChanged += new System.EventHandler(this.numSetuA_ValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(185, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 49);
            this.button1.TabIndex = 3;
            this.button1.Text = "Power";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtMonkV
            // 
            this.txtMonkV.Location = new System.Drawing.Point(20, 149);
            this.txtMonkV.Name = "txtMonkV";
            this.txtMonkV.Size = new System.Drawing.Size(69, 20);
            this.txtMonkV.TabIndex = 4;
            // 
            // txtMonuA
            // 
            this.txtMonuA.Location = new System.Drawing.Point(95, 149);
            this.txtMonuA.Name = "txtMonuA";
            this.txtMonuA.Size = new System.Drawing.Size(69, 20);
            this.txtMonuA.TabIndex = 4;
            // 
            // numAdjVolts
            // 
            this.numAdjVolts.DecimalPlaces = 2;
            this.numAdjVolts.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numAdjVolts.Location = new System.Drawing.Point(95, 72);
            this.numAdjVolts.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numAdjVolts.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numAdjVolts.Name = "numAdjVolts";
            this.numAdjVolts.Size = new System.Drawing.Size(69, 20);
            this.numAdjVolts.TabIndex = 2;
            this.numAdjVolts.Value = new decimal(new int[] {
            95,
            0,
            0,
            65536});
            this.numAdjVolts.ValueChanged += new System.EventHandler(this.numAdjVolts_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 206);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Sel AD";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 229);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(59, 17);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "Sel DA";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Enabled = false;
            this.checkBox3.Location = new System.Drawing.Point(12, 183);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(74, 17);
            this.checkBox3.TabIndex = 5;
            this.checkBox3.Text = "Fil. ready?";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(228, 40);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 42);
            this.button3.TabIndex = 3;
            this.button3.Text = "Read All Registers";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(319, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(69, 637);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Power Supply";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Supply Voltage";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "V";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(97, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(106, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "LEDs";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(185, 97);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(86, 17);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "Power Relay";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "interlock",
            "hv on",
            "hv power ok",
            "tube ready",
            "n/a",
            "n/a",
            "n/a",
            "faulted"});
            this.checkedListBox1.Location = new System.Drawing.Point(29, 357);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(120, 124);
            this.checkedListBox1.TabIndex = 10;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Items.AddRange(new object[] {
            "booted",
            "interlock",
            "unstable",
            "hv fault",
            "temp fault",
            "timeout",
            "n/a",
            "n/a"});
            this.checkedListBox2.Location = new System.Drawing.Point(172, 357);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(120, 124);
            this.checkedListBox2.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(172, 487);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Clear Faults";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP Address";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(90, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "192.168.1.102";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(228, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Connect";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 655);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtMonuA);
            this.Controls.Add(this.txtMonkV);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numSetuA);
            this.Controls.Add(this.numAdjVolts);
            this.Controls.Add(this.numSetkV);
            this.Controls.Add(this.cbxPSSelect);
            this.Controls.Add(this.chkRed);
            this.Controls.Add(this.chkYellow);
            this.Controls.Add(this.chkGreen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "frmUniversalTB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUniversalTB_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSetkV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetuA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAdjVolts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.CheckBox chkGreen;
        private System.Windows.Forms.CheckBox chkYellow;
        private System.Windows.Forms.CheckBox chkRed;
        private System.Windows.Forms.ComboBox cbxPSSelect;
        private System.Windows.Forms.NumericUpDown numSetkV;
        private System.Windows.Forms.NumericUpDown numSetuA;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMonkV;
        private System.Windows.Forms.TextBox txtMonuA;
        private System.Windows.Forms.NumericUpDown numAdjVolts;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
    }
}