namespace RaptorCamApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ctlRaptorCam1 = new Devices.Controls.ctlRaptorCam();
            this.SuspendLayout();
            // 
            // ctlRaptorCam1
            // 
            this.ctlRaptorCam1.Location = new System.Drawing.Point(12, 12);
            this.ctlRaptorCam1.Name = "ctlRaptorCam1";
            this.ctlRaptorCam1.Size = new System.Drawing.Size(1128, 407);
            this.ctlRaptorCam1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1156, 425);
            this.Controls.Add(this.ctlRaptorCam1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Camera";
            this.ResumeLayout(false);

        }

        #endregion

        private Devices.Controls.ctlRaptorCam ctlRaptorCam1;
    }
}

