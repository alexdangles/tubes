namespace EnvChamberApp
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ctlEnvChamber1 = new Devices.Controls.ctlEnvChamber();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Select Chamber..."});
            this.comboBox1.Location = new System.Drawing.Point(3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ctlEnvChamber1
            // 
            this.ctlEnvChamber1.AutoSize = true;
            this.ctlEnvChamber1.Enabled = false;
            this.ctlEnvChamber1.Location = new System.Drawing.Point(3, 23);
            this.ctlEnvChamber1.Name = "ctlEnvChamber1";
            this.ctlEnvChamber1.Size = new System.Drawing.Size(452, 260);
            this.ctlEnvChamber1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 280);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ctlEnvChamber1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Environmental Chamber";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Devices.Controls.ctlEnvChamber ctlEnvChamber1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

