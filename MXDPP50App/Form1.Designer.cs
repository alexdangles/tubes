namespace MXDPP50App
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
            this.ctlDPP501 = new Devices.Controls.ctlDPP50();
            this.SuspendLayout();
            // 
            // ctlDPP501
            // 
            this.ctlDPP501.acquiring = false;
            this.ctlDPP501.bremstrahlungEV = ((uint)(0u));
            this.ctlDPP501.Location = new System.Drawing.Point(3, 6);
            this.ctlDPP501.Name = "ctlDPP501";
            this.ctlDPP501.roi = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.ctlDPP501.setTime = 0D;
            this.ctlDPP501.Size = new System.Drawing.Size(699, 638);
            this.ctlDPP501.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(704, 651);
            this.Controls.Add(this.ctlDPP501);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Moxtek DPP-50";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Devices.Controls.ctlDPP50 ctlDPP501;
    }
}