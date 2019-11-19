namespace Devices.Controls
{
    partial class ctlTube
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlTube));
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dgvRegisters = new System.Windows.Forms.DataGridView();
            this.lblBytesIn = new System.Windows.Forms.Label();
            this.txtBytesIn = new System.Windows.Forms.TextBox();
            this.lblBytesOut = new System.Windows.Forms.Label();
            this.txtBytesOut = new System.Windows.Forms.TextBox();
            this.btnWriteBytes = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.numSetuA = new System.Windows.Forms.NumericUpDown();
            this.btnPower = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.picTube = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvPSList = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.txtWatts = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMonkV = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numSetkV = new System.Windows.Forms.NumericUpDown();
            this.txtMonuA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblHVState = new System.Windows.Forms.Label();
            this.lblWatts = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.lnkLog = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numLog = new System.Windows.Forms.NumericUpDown();
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.tabI2C = new System.Windows.Forms.TabPage();
            this.tmrLog = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisters)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetuA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetkV)).BeginInit();
            this.tabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLog)).BeginInit();
            this.tabI2C.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // dgvRegisters
            // 
            this.dgvRegisters.AllowUserToAddRows = false;
            this.dgvRegisters.AllowUserToDeleteRows = false;
            this.dgvRegisters.AllowUserToResizeColumns = false;
            this.dgvRegisters.AllowUserToResizeRows = false;
            this.dgvRegisters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegisters.ColumnHeadersVisible = false;
            this.dgvRegisters.Location = new System.Drawing.Point(5, 6);
            this.dgvRegisters.MultiSelect = false;
            this.dgvRegisters.Name = "dgvRegisters";
            this.dgvRegisters.ReadOnly = true;
            this.dgvRegisters.RowHeadersVisible = false;
            this.dgvRegisters.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvRegisters.Size = new System.Drawing.Size(166, 303);
            this.dgvRegisters.TabIndex = 1;
            // 
            // lblBytesIn
            // 
            this.lblBytesIn.AutoSize = true;
            this.lblBytesIn.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBytesIn.ForeColor = System.Drawing.Color.Black;
            this.lblBytesIn.Location = new System.Drawing.Point(181, 4);
            this.lblBytesIn.Name = "lblBytesIn";
            this.lblBytesIn.Size = new System.Drawing.Size(18, 14);
            this.lblBytesIn.TabIndex = 5;
            this.lblBytesIn.Text = "in";
            // 
            // txtBytesIn
            // 
            this.txtBytesIn.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBytesIn.Location = new System.Drawing.Point(175, 19);
            this.txtBytesIn.Multiline = true;
            this.txtBytesIn.Name = "txtBytesIn";
            this.txtBytesIn.Size = new System.Drawing.Size(30, 238);
            this.txtBytesIn.TabIndex = 0;
            this.txtBytesIn.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtBytesIn_MouseDoubleClick);
            // 
            // lblBytesOut
            // 
            this.lblBytesOut.AutoSize = true;
            this.lblBytesOut.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBytesOut.ForeColor = System.Drawing.Color.Black;
            this.lblBytesOut.Location = new System.Drawing.Point(212, 4);
            this.lblBytesOut.Name = "lblBytesOut";
            this.lblBytesOut.Size = new System.Drawing.Size(25, 14);
            this.lblBytesOut.TabIndex = 5;
            this.lblBytesOut.Text = "out";
            // 
            // txtBytesOut
            // 
            this.txtBytesOut.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBytesOut.Location = new System.Drawing.Point(209, 19);
            this.txtBytesOut.Multiline = true;
            this.txtBytesOut.Name = "txtBytesOut";
            this.txtBytesOut.ReadOnly = true;
            this.txtBytesOut.Size = new System.Drawing.Size(30, 238);
            this.txtBytesOut.TabIndex = 0;
            // 
            // btnWriteBytes
            // 
            this.btnWriteBytes.BackColor = System.Drawing.Color.DimGray;
            this.btnWriteBytes.FlatAppearance.BorderSize = 0;
            this.btnWriteBytes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWriteBytes.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWriteBytes.ForeColor = System.Drawing.Color.White;
            this.btnWriteBytes.Location = new System.Drawing.Point(176, 263);
            this.btnWriteBytes.Name = "btnWriteBytes";
            this.btnWriteBytes.Size = new System.Drawing.Size(63, 43);
            this.btnWriteBytes.TabIndex = 2;
            this.btnWriteBytes.Text = "Write\r\nBytes";
            this.btnWriteBytes.UseVisualStyleBackColor = false;
            this.btnWriteBytes.Click += new System.EventHandler(this.btnWriteBytes_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabMain);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Controls.Add(this.tabI2C);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(253, 346);
            this.tabControl1.TabIndex = 22;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.numSetuA);
            this.tabMain.Controls.Add(this.btnPower);
            this.tabMain.Controls.Add(this.label2);
            this.tabMain.Controls.Add(this.picTube);
            this.tabMain.Controls.Add(this.label3);
            this.tabMain.Controls.Add(this.dgvPSList);
            this.tabMain.Controls.Add(this.label8);
            this.tabMain.Controls.Add(this.cmbStatus);
            this.tabMain.Controls.Add(this.txtWatts);
            this.tabMain.Controls.Add(this.label4);
            this.tabMain.Controls.Add(this.txtMonkV);
            this.tabMain.Controls.Add(this.label5);
            this.tabMain.Controls.Add(this.label6);
            this.tabMain.Controls.Add(this.label9);
            this.tabMain.Controls.Add(this.numSetkV);
            this.tabMain.Controls.Add(this.txtMonuA);
            this.tabMain.Controls.Add(this.label1);
            this.tabMain.Controls.Add(this.label7);
            this.tabMain.Controls.Add(this.lblHVState);
            this.tabMain.Controls.Add(this.lblWatts);
            this.tabMain.Controls.Add(this.txtSerial);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(245, 320);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Control";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // numSetuA
            // 
            this.numSetuA.BackColor = System.Drawing.Color.White;
            this.numSetuA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numSetuA.DecimalPlaces = 2;
            this.numSetuA.Enabled = false;
            this.numSetuA.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSetuA.ForeColor = System.Drawing.Color.ForestGreen;
            this.numSetuA.Location = new System.Drawing.Point(152, 24);
            this.numSetuA.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numSetuA.Name = "numSetuA";
            this.numSetuA.Size = new System.Drawing.Size(78, 23);
            this.numSetuA.TabIndex = 4;
            this.numSetuA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSetuA.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numSetuA.ValueChanged += new System.EventHandler(this.numSetkV_ValueChanged);
            // 
            // btnPower
            // 
            this.btnPower.BackColor = System.Drawing.Color.Transparent;
            this.btnPower.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPower.BackgroundImage")));
            this.btnPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPower.Enabled = false;
            this.btnPower.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPower.ForeColor = System.Drawing.Color.White;
            this.btnPower.Location = new System.Drawing.Point(10, 23);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(50, 50);
            this.btnPower.TabIndex = 21;
            this.btnPower.UseVisualStyleBackColor = false;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(163, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Set µA";
            // 
            // picTube
            // 
            this.picTube.BackColor = System.Drawing.Color.Transparent;
            this.picTube.InitialImage = ((System.Drawing.Image)(resources.GetObject("picTube.InitialImage")));
            this.picTube.Location = new System.Drawing.Point(5, 147);
            this.picTube.Name = "picTube";
            this.picTube.Size = new System.Drawing.Size(234, 168);
            this.picTube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTube.TabIndex = 19;
            this.picTube.TabStop = false;
            this.picTube.Visible = false;
            this.picTube.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picTube_MouseDoubleClick);
            this.picTube.MouseHover += new System.EventHandler(this.picTube_MouseHover);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(85, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "Set kV";
            // 
            // dgvPSList
            // 
            this.dgvPSList.AllowUserToAddRows = false;
            this.dgvPSList.AllowUserToDeleteRows = false;
            this.dgvPSList.AllowUserToOrderColumns = true;
            this.dgvPSList.AllowUserToResizeColumns = false;
            this.dgvPSList.AllowUserToResizeRows = false;
            this.dgvPSList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPSList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPSList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPSList.ColumnHeadersVisible = false;
            this.dgvPSList.Location = new System.Drawing.Point(84, 178);
            this.dgvPSList.Name = "dgvPSList";
            this.dgvPSList.ReadOnly = true;
            this.dgvPSList.RowHeadersVisible = false;
            this.dgvPSList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvPSList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPSList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPSList.Size = new System.Drawing.Size(155, 134);
            this.dgvPSList.TabIndex = 17;
            this.dgvPSList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPSList_CellMouseDoubleClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(6, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 14);
            this.label8.TabIndex = 5;
            this.label8.Text = "HV State";
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.Enabled = false;
            this.cmbStatus.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.ForeColor = System.Drawing.Color.Black;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(162, 116);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbStatus.Size = new System.Drawing.Size(77, 26);
            this.cmbStatus.TabIndex = 2;
            this.cmbStatus.SelectionChangeCommitted += new System.EventHandler(this.cmbStatus_SelectionChangeCommitted);
            // 
            // txtWatts
            // 
            this.txtWatts.BackColor = System.Drawing.Color.White;
            this.txtWatts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWatts.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWatts.ForeColor = System.Drawing.Color.Firebrick;
            this.txtWatts.Location = new System.Drawing.Point(84, 118);
            this.txtWatts.Name = "txtWatts";
            this.txtWatts.ReadOnly = true;
            this.txtWatts.Size = new System.Drawing.Size(61, 20);
            this.txtWatts.TabIndex = 7;
            this.txtWatts.Text = "0.00";
            this.txtWatts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(6, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Serial Number";
            // 
            // txtMonkV
            // 
            this.txtMonkV.BackColor = System.Drawing.Color.White;
            this.txtMonkV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMonkV.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonkV.ForeColor = System.Drawing.Color.Blue;
            this.txtMonkV.Location = new System.Drawing.Point(84, 73);
            this.txtMonkV.Name = "txtMonkV";
            this.txtMonkV.ReadOnly = true;
            this.txtMonkV.Size = new System.Drawing.Size(72, 20);
            this.txtMonkV.TabIndex = 7;
            this.txtMonkV.Text = "0.00";
            this.txtMonkV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(10, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 104);
            this.label5.TabIndex = 5;
            this.label5.Text = "Enter tube\r\nserial number\r\n\r\nor select\r\npower\r\nsupply\r\nfrom list\r\n------------->";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkGreen;
            this.label6.Location = new System.Drawing.Point(159, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "Mon. µA";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(6, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 14);
            this.label9.TabIndex = 5;
            this.label9.Text = "Power";
            // 
            // numSetkV
            // 
            this.numSetkV.BackColor = System.Drawing.Color.White;
            this.numSetkV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numSetkV.DecimalPlaces = 2;
            this.numSetkV.Enabled = false;
            this.numSetkV.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSetkV.ForeColor = System.Drawing.Color.Blue;
            this.numSetkV.Location = new System.Drawing.Point(69, 24);
            this.numSetkV.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numSetkV.Name = "numSetkV";
            this.numSetkV.Size = new System.Drawing.Size(76, 23);
            this.numSetkV.TabIndex = 3;
            this.numSetkV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSetkV.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numSetkV.ValueChanged += new System.EventHandler(this.numSetkV_ValueChanged);
            // 
            // txtMonuA
            // 
            this.txtMonuA.BackColor = System.Drawing.Color.White;
            this.txtMonuA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMonuA.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonuA.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtMonuA.Location = new System.Drawing.Point(162, 73);
            this.txtMonuA.Name = "txtMonuA";
            this.txtMonuA.ReadOnly = true;
            this.txtMonuA.Size = new System.Drawing.Size(74, 20);
            this.txtMonuA.TabIndex = 7;
            this.txtMonuA.Text = "0.00";
            this.txtMonuA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(85, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mon. kV";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(163, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "Status";
            this.label7.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.label7_MouseDoubleClick);
            // 
            // lblHVState
            // 
            this.lblHVState.BackColor = System.Drawing.Color.IndianRed;
            this.lblHVState.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHVState.ForeColor = System.Drawing.Color.White;
            this.lblHVState.Location = new System.Drawing.Point(9, 91);
            this.lblHVState.Name = "lblHVState";
            this.lblHVState.Padding = new System.Windows.Forms.Padding(2);
            this.lblHVState.Size = new System.Drawing.Size(67, 52);
            this.lblHVState.TabIndex = 5;
            this.lblHVState.Text = "Check Interlock";
            this.lblHVState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWatts
            // 
            this.lblWatts.AutoSize = true;
            this.lblWatts.BackColor = System.Drawing.Color.Transparent;
            this.lblWatts.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWatts.ForeColor = System.Drawing.Color.Maroon;
            this.lblWatts.Location = new System.Drawing.Point(85, 102);
            this.lblWatts.Name = "lblWatts";
            this.lblWatts.Size = new System.Drawing.Size(39, 14);
            this.lblWatts.TabIndex = 5;
            this.lblWatts.Text = "Watts";
            // 
            // txtSerial
            // 
            this.txtSerial.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.Location = new System.Drawing.Point(84, 149);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(155, 24);
            this.txtSerial.TabIndex = 16;
            this.txtSerial.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSerial_MouseClick);
            this.txtSerial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSerial_KeyDown);
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.lnkLog);
            this.tabLog.Controls.Add(this.label11);
            this.tabLog.Controls.Add(this.label10);
            this.tabLog.Controls.Add(this.numLog);
            this.tabLog.Controls.Add(this.chkLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(245, 320);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "Logging";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // lnkLog
            // 
            this.lnkLog.Location = new System.Drawing.Point(6, 38);
            this.lnkLog.Name = "lnkLog";
            this.lnkLog.Size = new System.Drawing.Size(231, 276);
            this.lnkLog.TabIndex = 7;
            this.lnkLog.TabStop = true;
            this.lnkLog.Text = "K:\\Software\\Tubes\\Saved Data\\log.txt";
            this.toolTip1.SetToolTip(this.lnkLog, "Click to change. Ctrl + click to open file.");
            this.lnkLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLog_LinkClicked);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(217, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 14);
            this.label11.TabIndex = 6;
            this.label11.Text = "ms.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(100, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 15);
            this.label10.TabIndex = 6;
            this.label10.Text = "Every";
            // 
            // numLog
            // 
            this.numLog.BackColor = System.Drawing.SystemColors.Control;
            this.numLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numLog.Enabled = false;
            this.numLog.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numLog.ForeColor = System.Drawing.Color.Black;
            this.numLog.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numLog.Location = new System.Drawing.Point(140, 11);
            this.numLog.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numLog.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numLog.Name = "numLog";
            this.numLog.Size = new System.Drawing.Size(78, 20);
            this.numLog.TabIndex = 5;
            this.numLog.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numLog.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numLog.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numLog.ValueChanged += new System.EventHandler(this.numLog_ValueChanged);
            // 
            // chkLog
            // 
            this.chkLog.AutoSize = true;
            this.chkLog.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLog.Location = new System.Drawing.Point(11, 11);
            this.chkLog.Name = "chkLog";
            this.chkLog.Size = new System.Drawing.Size(73, 19);
            this.chkLog.TabIndex = 1;
            this.chkLog.Text = "Log Data";
            this.chkLog.UseVisualStyleBackColor = true;
            this.chkLog.CheckedChanged += new System.EventHandler(this.chkLog_CheckedChanged);
            // 
            // tabI2C
            // 
            this.tabI2C.Controls.Add(this.txtBytesOut);
            this.tabI2C.Controls.Add(this.btnWriteBytes);
            this.tabI2C.Controls.Add(this.dgvRegisters);
            this.tabI2C.Controls.Add(this.lblBytesIn);
            this.tabI2C.Controls.Add(this.lblBytesOut);
            this.tabI2C.Controls.Add(this.txtBytesIn);
            this.tabI2C.Location = new System.Drawing.Point(4, 22);
            this.tabI2C.Name = "tabI2C";
            this.tabI2C.Padding = new System.Windows.Forms.Padding(3);
            this.tabI2C.Size = new System.Drawing.Size(245, 320);
            this.tabI2C.TabIndex = 1;
            this.tabI2C.Text = "I2C";
            this.tabI2C.UseVisualStyleBackColor = true;
            // 
            // tmrLog
            // 
            this.tmrLog.Interval = 1000;
            this.tmrLog.Tick += new System.EventHandler(this.tmrLog_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ctlTube
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "ctlTube";
            this.Size = new System.Drawing.Size(259, 352);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisters)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSetuA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSetkV)).EndInit();
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLog)).EndInit();
            this.tabI2C.ResumeLayout(false);
            this.tabI2C.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dgvRegisters;
        private System.Windows.Forms.Label lblBytesIn;
        private System.Windows.Forms.TextBox txtBytesIn;
        private System.Windows.Forms.Label lblBytesOut;
        private System.Windows.Forms.TextBox txtBytesOut;
        private System.Windows.Forms.Button btnWriteBytes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.NumericUpDown numSetuA;
        private System.Windows.Forms.Button btnPower;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picTube;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvPSList;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWatts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMonkV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.NumericUpDown numSetkV;
        private System.Windows.Forms.TextBox txtMonuA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblHVState;
        private System.Windows.Forms.Label lblWatts;
        private System.Windows.Forms.TabPage tabI2C;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.CheckBox chkLog;
        private System.Windows.Forms.Timer tmrLog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numLog;
        public System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.LinkLabel lnkLog;
    }
}
