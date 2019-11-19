namespace Helper
{
    partial class frmTCPListenSend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTCPListenSend));
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.txtRecieve = new System.Windows.Forms.TextBox();
            this.tmrRecieve = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnReply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(12, 19);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 20);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(92, 19);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(100, 20);
            this.txtSend.TabIndex = 1;
            // 
            // txtRecieve
            // 
            this.txtRecieve.Location = new System.Drawing.Point(92, 49);
            this.txtRecieve.Name = "txtRecieve";
            this.txtRecieve.ReadOnly = true;
            this.txtRecieve.Size = new System.Drawing.Size(100, 20);
            this.txtRecieve.TabIndex = 1;
            // 
            // tmrRecieve
            // 
            this.tmrRecieve.Enabled = true;
            this.tmrRecieve.Tick += new System.EventHandler(this.tmrRecieve_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Recieved";
            // 
            // btnReply
            // 
            this.btnReply.Location = new System.Drawing.Point(198, 18);
            this.btnReply.Name = "btnReply";
            this.btnReply.Size = new System.Drawing.Size(85, 21);
            this.btnReply.TabIndex = 0;
            this.btnReply.Text = "Reply";
            this.btnReply.UseVisualStyleBackColor = true;
            this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
            // 
            // frmTCPListenSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 78);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRecieve);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.btnReply);
            this.Controls.Add(this.btnSend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmTCPListenSend";
            this.Text = "TCP I/O";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTCPListenSend_FormClosing);
            this.Shown += new System.EventHandler(this.frmTCPListenSend_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.TextBox txtRecieve;
        private System.Windows.Forms.Timer tmrRecieve;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReply;
    }
}