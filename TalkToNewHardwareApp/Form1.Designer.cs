namespace TalkToNewHardwareApp
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
            this.ctlTalk1 = new Helper.ctlTalk();
            this.SuspendLayout();
            // 
            // ctlTalk1
            // 
            this.ctlTalk1.AutoSize = true;
            this.ctlTalk1.Location = new System.Drawing.Point(12, 12);
            this.ctlTalk1.Name = "ctlTalk1";
            this.ctlTalk1.Size = new System.Drawing.Size(239, 321);
            this.ctlTalk1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 344);
            this.Controls.Add(this.ctlTalk1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Let\'s Talk";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Helper.ctlTalk ctlTalk1;
    }
}

