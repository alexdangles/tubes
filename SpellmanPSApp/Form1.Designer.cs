namespace SpellmanPSApp
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
            this.ctlSpellmanPS1 = new Devices.Controls.ctlSpellmanPS();
            this.SuspendLayout();
            // 
            // ctlSpellmanPS1
            // 
            this.ctlSpellmanPS1.AutoSize = true;
            this.ctlSpellmanPS1.Enabled = false;
            this.ctlSpellmanPS1.Location = new System.Drawing.Point(3, 3);
            this.ctlSpellmanPS1.Name = "ctlSpellmanPS1";
            this.ctlSpellmanPS1.Size = new System.Drawing.Size(443, 205);
            this.ctlSpellmanPS1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 211);
            this.Controls.Add(this.ctlSpellmanPS1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Spellman DX Power Supply";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Devices.Controls.ctlSpellmanPS ctlSpellmanPS1;
    }
}

