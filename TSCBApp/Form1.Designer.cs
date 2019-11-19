namespace TSCB
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ctlDPP501 = new Devices.Controls.ctlDPP50();
            this.ctlTube1 = new Devices.Controls.ctlTube();
            this.ctlGraph1 = new Helper.ctlGraph();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(916, 700);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ctlTube1);
            this.tabPage1.Controls.Add(this.ctlGraph1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(908, 674);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.ForeColor = System.Drawing.Color.LightYellow;
            this.panel1.Location = new System.Drawing.Point(7, 364);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(898, 304);
            this.panel1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ctlDPP501);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(908, 674);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DPP";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ctlDPP501
            // 
            this.ctlDPP501.acquiring = false;
            this.ctlDPP501.bremstrahlungEV = ((uint)(0u));
            this.ctlDPP501.Location = new System.Drawing.Point(3, 3);
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
            // ctlTube1
            // 
            this.ctlTube1.AutoSize = true;
            this.ctlTube1.BackColor = System.Drawing.SystemColors.Control;
            this.ctlTube1.Location = new System.Drawing.Point(6, 6);
            this.ctlTube1.Name = "ctlTube1";
            this.ctlTube1.Size = new System.Drawing.Size(259, 352);
            this.ctlTube1.TabIndex = 5;
            // 
            // ctlGraph1
            // 
            this.ctlGraph1.BackColor = System.Drawing.Color.Black;
            this.ctlGraph1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctlGraph1.Enabled = false;
            this.ctlGraph1.ForeColor = System.Drawing.Color.LightYellow;
            this.ctlGraph1.LegendVisible = false;
            this.ctlGraph1.Location = new System.Drawing.Point(272, 6);
            this.ctlGraph1.mode = Helper.ctlGraph.Mode.Scope;
            this.ctlGraph1.Name = "ctlGraph1";
            this.ctlGraph1.Size = new System.Drawing.Size(630, 352);
            this.ctlGraph1.TabIndex = 4;
            this.ctlGraph1.Title = "Photodiode";
            this.ctlGraph1.xAxis = "Time";
            this.ctlGraph1.yAxis = "Signal";
            this.ctlGraph1.yAxisSecondary = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(921, 704);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private Helper.ctlGraph ctlGraph1;
        private Devices.Controls.ctlDPP50 ctlDPP501;
        private Devices.Controls.ctlTube ctlTube1;
    }
}