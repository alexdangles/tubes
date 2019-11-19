namespace ThorLabsFWApp
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
            this.ctlFilterWheel1 = new Devices.Controls.ctlFilterWheel();
            this.SuspendLayout();
            // 
            // ctlFilterWheel1
            // 
            this.ctlFilterWheel1.AutoSize = true;
            this.ctlFilterWheel1.Enabled = false;
            this.ctlFilterWheel1.Location = new System.Drawing.Point(0, 1);
            this.ctlFilterWheel1.Name = "ctlFilterWheel1";
            this.ctlFilterWheel1.Size = new System.Drawing.Size(153, 153);
            this.ctlFilterWheel1.TabIndex = 0;
            this.ctlFilterWheel1.Tag = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(154, 150);
            this.Controls.Add(this.ctlFilterWheel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "FW102c";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Devices.Controls.ctlFilterWheel ctlFilterWheel1;
    }
}

