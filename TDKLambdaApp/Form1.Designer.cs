namespace TDKLambdaApp
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
            this.ctlTdkPowerSupply1 = new Devices.ctlTdkPowerSupply();
            this.SuspendLayout();
            // 
            // ctlTdkPowerSupply1
            // 
            this.ctlTdkPowerSupply1.AutoSize = true;
            this.ctlTdkPowerSupply1.BackColor = System.Drawing.Color.White;
            this.ctlTdkPowerSupply1.Enabled = false;
            this.ctlTdkPowerSupply1.Location = new System.Drawing.Point(3, 0);
            this.ctlTdkPowerSupply1.Name = "ctlTdkPowerSupply1";
            this.ctlTdkPowerSupply1.Size = new System.Drawing.Size(207, 200);
            this.ctlTdkPowerSupply1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 200);
            this.Controls.Add(this.ctlTdkPowerSupply1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Devices.ctlTdkPowerSupply ctlTdkPowerSupply1;
    }
}

