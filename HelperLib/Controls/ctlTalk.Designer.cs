namespace Helper
{
    partial class ctlTalk
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnConnect = new System.Windows.Forms.Button();
            this.cbxCOM = new System.Windows.Forms.ComboBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lblPortIP = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMsgSend = new System.Windows.Forms.TextBox();
            this.cbxBaud = new System.Windows.Forms.ComboBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblBaudPort = new System.Windows.Forms.Label();
            this.numMsToWait = new System.Windows.Forms.NumericUpDown();
            this.lblMsToWait = new System.Windows.Forms.Label();
            this.txtMsgRec = new System.Windows.Forms.RichTextBox();
            this.lstComType = new System.Windows.Forms.ListBox();
            this.cbxFormat = new System.Windows.Forms.ComboBox();
            this.lblFormat = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numMsToWait)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.DimGray;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(3, 46);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(66, 27);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cbxCOM
            // 
            this.cbxCOM.BackColor = System.Drawing.Color.Gainsboro;
            this.cbxCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCOM.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxCOM.FormattingEnabled = true;
            this.cbxCOM.Location = new System.Drawing.Point(127, 3);
            this.cbxCOM.Name = "cbxCOM";
            this.cbxCOM.Size = new System.Drawing.Size(109, 27);
            this.cbxCOM.TabIndex = 2;
            // 
            // txtIP
            // 
            this.txtIP.BackColor = System.Drawing.Color.White;
            this.txtIP.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP.ForeColor = System.Drawing.Color.Black;
            this.txtIP.Location = new System.Drawing.Point(127, 3);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(109, 27);
            this.txtIP.TabIndex = 0;
            this.txtIP.Text = "192.168.1.4";
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPortIP
            // 
            this.lblPortIP.AutoSize = true;
            this.lblPortIP.BackColor = System.Drawing.Color.Transparent;
            this.lblPortIP.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPortIP.ForeColor = System.Drawing.Color.Black;
            this.lblPortIP.Location = new System.Drawing.Point(78, 9);
            this.lblPortIP.Name = "lblPortIP";
            this.lblPortIP.Size = new System.Drawing.Size(35, 19);
            this.lblPortIP.TabIndex = 3;
            this.lblPortIP.Text = "Port";
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.DimGray;
            this.btnSend.Enabled = false;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSend.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(172, 112);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(64, 55);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMsgSend
            // 
            this.txtMsgSend.BackColor = System.Drawing.Color.White;
            this.txtMsgSend.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsgSend.ForeColor = System.Drawing.Color.Black;
            this.txtMsgSend.Location = new System.Drawing.Point(3, 79);
            this.txtMsgSend.Name = "txtMsgSend";
            this.txtMsgSend.Size = new System.Drawing.Size(233, 27);
            this.txtMsgSend.TabIndex = 0;
            this.txtMsgSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMsgSend_KeyDown);
            this.txtMsgSend.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtMsgSend_MouseDoubleClick);
            // 
            // cbxBaud
            // 
            this.cbxBaud.BackColor = System.Drawing.Color.Gainsboro;
            this.cbxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBaud.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxBaud.FormattingEnabled = true;
            this.cbxBaud.Items.AddRange(new object[] {
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200 ",
            "128000"});
            this.cbxBaud.Location = new System.Drawing.Point(127, 36);
            this.cbxBaud.Name = "cbxBaud";
            this.cbxBaud.Size = new System.Drawing.Size(109, 27);
            this.cbxBaud.TabIndex = 2;
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.White;
            this.txtPort.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.ForeColor = System.Drawing.Color.Black;
            this.txtPort.Location = new System.Drawing.Point(127, 36);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(109, 27);
            this.txtPort.TabIndex = 0;
            this.txtPort.Text = "50001";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblBaudPort
            // 
            this.lblBaudPort.AutoSize = true;
            this.lblBaudPort.BackColor = System.Drawing.Color.Transparent;
            this.lblBaudPort.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaudPort.ForeColor = System.Drawing.Color.Black;
            this.lblBaudPort.Location = new System.Drawing.Point(76, 39);
            this.lblBaudPort.Name = "lblBaudPort";
            this.lblBaudPort.Size = new System.Drawing.Size(42, 19);
            this.lblBaudPort.TabIndex = 3;
            this.lblBaudPort.Text = "Baud";
            // 
            // numMsToWait
            // 
            this.numMsToWait.BackColor = System.Drawing.Color.Gainsboro;
            this.numMsToWait.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numMsToWait.Enabled = false;
            this.numMsToWait.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMsToWait.ForeColor = System.Drawing.Color.Black;
            this.numMsToWait.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMsToWait.Location = new System.Drawing.Point(85, 116);
            this.numMsToWait.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMsToWait.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numMsToWait.Name = "numMsToWait";
            this.numMsToWait.Size = new System.Drawing.Size(80, 26);
            this.numMsToWait.TabIndex = 1;
            this.numMsToWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numMsToWait.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numMsToWait.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMsToWait.ValueChanged += new System.EventHandler(this.numMsToWait_ValueChanged);
            // 
            // lblMsToWait
            // 
            this.lblMsToWait.AutoSize = true;
            this.lblMsToWait.BackColor = System.Drawing.Color.Transparent;
            this.lblMsToWait.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsToWait.ForeColor = System.Drawing.Color.Black;
            this.lblMsToWait.Location = new System.Drawing.Point(3, 118);
            this.lblMsToWait.Name = "lblMsToWait";
            this.lblMsToWait.Size = new System.Drawing.Size(77, 19);
            this.lblMsToWait.TabIndex = 3;
            this.lblMsToWait.Text = "ms to wait";
            // 
            // txtMsgRec
            // 
            this.txtMsgRec.BackColor = System.Drawing.Color.Gainsboro;
            this.txtMsgRec.Location = new System.Drawing.Point(3, 206);
            this.txtMsgRec.Name = "txtMsgRec";
            this.txtMsgRec.ReadOnly = true;
            this.txtMsgRec.Size = new System.Drawing.Size(233, 112);
            this.txtMsgRec.TabIndex = 4;
            this.txtMsgRec.Text = "";
            // 
            // lstComType
            // 
            this.lstComType.BackColor = System.Drawing.Color.Gainsboro;
            this.lstComType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstComType.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstComType.ForeColor = System.Drawing.Color.Black;
            this.lstComType.FormattingEnabled = true;
            this.lstComType.ItemHeight = 19;
            this.lstComType.Items.AddRange(new object[] {
            "Serial",
            "TCP/IP"});
            this.lstComType.Location = new System.Drawing.Point(3, 2);
            this.lstComType.Name = "lstComType";
            this.lstComType.Size = new System.Drawing.Size(52, 38);
            this.lstComType.TabIndex = 9;
            this.lstComType.SelectedIndexChanged += new System.EventHandler(this.lstComType_SelectedIndexChanged);
            // 
            // cbxFormat
            // 
            this.cbxFormat.BackColor = System.Drawing.Color.Gainsboro;
            this.cbxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFormat.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxFormat.FormattingEnabled = true;
            this.cbxFormat.Items.AddRange(new object[] {
            "msg",
            "msg<CR><LF>",
            "msg<LF><CR>",
            "msg<CR>",
            "msg<LF>",
            "<STX>msg<ETX>"});
            this.cbxFormat.Location = new System.Drawing.Point(3, 173);
            this.cbxFormat.Name = "cbxFormat";
            this.cbxFormat.Size = new System.Drawing.Size(233, 27);
            this.cbxFormat.TabIndex = 2;
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.BackColor = System.Drawing.Color.Transparent;
            this.lblFormat.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormat.ForeColor = System.Drawing.Color.Black;
            this.lblFormat.Location = new System.Drawing.Point(3, 151);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(79, 19);
            this.lblFormat.TabIndex = 3;
            this.lblFormat.Text = "Formatting";
            // 
            // ctlTalk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.lstComType);
            this.Controls.Add(this.txtMsgRec);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cbxBaud);
            this.Controls.Add(this.cbxFormat);
            this.Controls.Add(this.cbxCOM);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.numMsToWait);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.lblMsToWait);
            this.Controls.Add(this.lblBaudPort);
            this.Controls.Add(this.lblPortIP);
            this.Controls.Add(this.txtMsgSend);
            this.Name = "ctlTalk";
            this.Size = new System.Drawing.Size(239, 321);
            ((System.ComponentModel.ISupportInitialize)(this.numMsToWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cbxCOM;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lblPortIP;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMsgSend;
        private System.Windows.Forms.ComboBox cbxBaud;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblBaudPort;
        private System.Windows.Forms.NumericUpDown numMsToWait;
        private System.Windows.Forms.Label lblMsToWait;
        private System.Windows.Forms.RichTextBox txtMsgRec;
        private System.Windows.Forms.ListBox lstComType;
        private System.Windows.Forms.ComboBox cbxFormat;
        private System.Windows.Forms.Label lblFormat;
    }
}
