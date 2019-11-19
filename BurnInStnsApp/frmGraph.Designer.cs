namespace BurnInStns
{
    partial class frmGraph
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
            this.ctlGraph1 = new Helper.ctlGraph();
            this.SuspendLayout();
            // 
            // ctlGraph1
            // 
            this.ctlGraph1.BackColor = System.Drawing.Color.Black;
            this.ctlGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlGraph1.Enabled = false;
            this.ctlGraph1.LegendVisible = true;
            this.ctlGraph1.Location = new System.Drawing.Point(0, 0);
            this.ctlGraph1.mode = Helper.ctlGraph.Mode.Scope;
            this.ctlGraph1.Name = "ctlGraph1";
            this.ctlGraph1.Size = new System.Drawing.Size(784, 361);
            this.ctlGraph1.TabIndex = 0;
            this.ctlGraph1.Title = "";
            this.ctlGraph1.xAxis = "Time";
            this.ctlGraph1.yAxis = "kV";
            this.ctlGraph1.yAxisSecondary = "uA";
            // 
            // frmGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.ctlGraph1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmGraph";
            this.Text = "Burn-in Status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGraph_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        public Helper.ctlGraph ctlGraph1;
    }
}