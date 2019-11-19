namespace BurnInStns
{
    partial class frmSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddSet = new System.Windows.Forms.Button();
            this.dgvPSList = new System.Windows.Forms.DataGridView();
            this.moxtekDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vMIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kVVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uAVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxKVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minKVDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxUADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minUADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pSVoltageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wattsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tUBNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.digitalOrNotDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repetabilityFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productImageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spectromCollectFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hardwareSetupDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tSCBPowerSupplyListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tubesDataSet = new BurnInStns.TubesDataSet();
            this.dgvBISettings = new System.Windows.Forms.DataGridView();
            this.moxtekDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kVSetpointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kVTolerancepercentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kVSteppersecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uASetpointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uATolerancepercentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uASteppersecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settlingTimesecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runTimehoursDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photodiodeEnableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.photodiodeUpperLimDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photodiodeLowerLimDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cyclingEnableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cyclingOnTimesecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cyclingOffTimesecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arcLabBurnInSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tSCBPowerSupplyListTableAdapter = new BurnInStns.TubesDataSetTableAdapters.TSCBPowerSupplyListTableAdapter();
            this.arcLabBurnInSettingsTableAdapter = new BurnInStns.TubesDataSetTableAdapters.ArcLabBurnInSettingsTableAdapter();
            this.btnManualSet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tSCBPowerSupplyListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tubesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBISettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcLabBurnInSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Power Supply List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Burn-in Settings";
            // 
            // btnAddSet
            // 
            this.btnAddSet.Location = new System.Drawing.Point(209, 22);
            this.btnAddSet.Name = "btnAddSet";
            this.btnAddSet.Size = new System.Drawing.Size(71, 48);
            this.btnAddSet.TabIndex = 2;
            this.btnAddSet.Text = "Add Settings\r\n------------>";
            this.btnAddSet.UseVisualStyleBackColor = true;
            this.btnAddSet.Click += new System.EventHandler(this.btnAddSet_Click);
            // 
            // dgvPSList
            // 
            this.dgvPSList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvPSList.AutoGenerateColumns = false;
            this.dgvPSList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvPSList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPSList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.moxtekDataGridViewTextBoxColumn,
            this.vMIDataGridViewTextBoxColumn,
            this.kVVDataGridViewTextBoxColumn,
            this.uAVDataGridViewTextBoxColumn,
            this.maxKVDataGridViewTextBoxColumn,
            this.minKVDataGridViewTextBoxColumn,
            this.maxUADataGridViewTextBoxColumn,
            this.minUADataGridViewTextBoxColumn,
            this.pSVoltageDataGridViewTextBoxColumn,
            this.wattsDataGridViewTextBoxColumn,
            this.tUBNumberDataGridViewTextBoxColumn,
            this.digitalOrNotDataGridViewTextBoxColumn,
            this.testFileDataGridViewTextBoxColumn,
            this.repetabilityFileDataGridViewTextBoxColumn,
            this.productImageDataGridViewTextBoxColumn,
            this.settingsFileDataGridViewTextBoxColumn,
            this.spectromCollectFileDataGridViewTextBoxColumn,
            this.hardwareSetupDataGridViewTextBoxColumn});
            this.dgvPSList.DataSource = this.tSCBPowerSupplyListBindingSource;
            this.dgvPSList.Location = new System.Drawing.Point(9, 22);
            this.dgvPSList.MultiSelect = false;
            this.dgvPSList.Name = "dgvPSList";
            this.dgvPSList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPSList.Size = new System.Drawing.Size(194, 431);
            this.dgvPSList.TabIndex = 3;
            // 
            // moxtekDataGridViewTextBoxColumn
            // 
            this.moxtekDataGridViewTextBoxColumn.DataPropertyName = "Moxtek#";
            this.moxtekDataGridViewTextBoxColumn.HeaderText = "Moxtek#";
            this.moxtekDataGridViewTextBoxColumn.Name = "moxtekDataGridViewTextBoxColumn";
            this.moxtekDataGridViewTextBoxColumn.Width = 74;
            // 
            // vMIDataGridViewTextBoxColumn
            // 
            this.vMIDataGridViewTextBoxColumn.DataPropertyName = "VMI #";
            this.vMIDataGridViewTextBoxColumn.HeaderText = "VMI #";
            this.vMIDataGridViewTextBoxColumn.Name = "vMIDataGridViewTextBoxColumn";
            this.vMIDataGridViewTextBoxColumn.Width = 51;
            // 
            // kVVDataGridViewTextBoxColumn
            // 
            this.kVVDataGridViewTextBoxColumn.DataPropertyName = "kV V";
            this.kVVDataGridViewTextBoxColumn.HeaderText = "kV V";
            this.kVVDataGridViewTextBoxColumn.Name = "kVVDataGridViewTextBoxColumn";
            this.kVVDataGridViewTextBoxColumn.Width = 45;
            // 
            // uAVDataGridViewTextBoxColumn
            // 
            this.uAVDataGridViewTextBoxColumn.DataPropertyName = "uA V";
            this.uAVDataGridViewTextBoxColumn.HeaderText = "uA V";
            this.uAVDataGridViewTextBoxColumn.Name = "uAVDataGridViewTextBoxColumn";
            this.uAVDataGridViewTextBoxColumn.Width = 45;
            // 
            // maxKVDataGridViewTextBoxColumn
            // 
            this.maxKVDataGridViewTextBoxColumn.DataPropertyName = "Max kV";
            this.maxKVDataGridViewTextBoxColumn.HeaderText = "Max kV";
            this.maxKVDataGridViewTextBoxColumn.Name = "maxKVDataGridViewTextBoxColumn";
            this.maxKVDataGridViewTextBoxColumn.Width = 63;
            // 
            // minKVDataGridViewTextBoxColumn
            // 
            this.minKVDataGridViewTextBoxColumn.DataPropertyName = "Min kV";
            this.minKVDataGridViewTextBoxColumn.HeaderText = "Min kV";
            this.minKVDataGridViewTextBoxColumn.Name = "minKVDataGridViewTextBoxColumn";
            this.minKVDataGridViewTextBoxColumn.Width = 49;
            // 
            // maxUADataGridViewTextBoxColumn
            // 
            this.maxUADataGridViewTextBoxColumn.DataPropertyName = "Max uA";
            this.maxUADataGridViewTextBoxColumn.HeaderText = "Max uA";
            this.maxUADataGridViewTextBoxColumn.Name = "maxUADataGridViewTextBoxColumn";
            this.maxUADataGridViewTextBoxColumn.Width = 63;
            // 
            // minUADataGridViewTextBoxColumn
            // 
            this.minUADataGridViewTextBoxColumn.DataPropertyName = "Min uA";
            this.minUADataGridViewTextBoxColumn.HeaderText = "Min uA";
            this.minUADataGridViewTextBoxColumn.Name = "minUADataGridViewTextBoxColumn";
            this.minUADataGridViewTextBoxColumn.Width = 49;
            // 
            // pSVoltageDataGridViewTextBoxColumn
            // 
            this.pSVoltageDataGridViewTextBoxColumn.DataPropertyName = "PS voltage";
            this.pSVoltageDataGridViewTextBoxColumn.HeaderText = "PS voltage";
            this.pSVoltageDataGridViewTextBoxColumn.Name = "pSVoltageDataGridViewTextBoxColumn";
            this.pSVoltageDataGridViewTextBoxColumn.Width = 78;
            // 
            // wattsDataGridViewTextBoxColumn
            // 
            this.wattsDataGridViewTextBoxColumn.DataPropertyName = "Watts";
            this.wattsDataGridViewTextBoxColumn.HeaderText = "Watts";
            this.wattsDataGridViewTextBoxColumn.Name = "wattsDataGridViewTextBoxColumn";
            this.wattsDataGridViewTextBoxColumn.Width = 60;
            // 
            // tUBNumberDataGridViewTextBoxColumn
            // 
            this.tUBNumberDataGridViewTextBoxColumn.DataPropertyName = "TUBNumber";
            this.tUBNumberDataGridViewTextBoxColumn.HeaderText = "TUBNumber";
            this.tUBNumberDataGridViewTextBoxColumn.Name = "tUBNumberDataGridViewTextBoxColumn";
            this.tUBNumberDataGridViewTextBoxColumn.Width = 91;
            // 
            // digitalOrNotDataGridViewTextBoxColumn
            // 
            this.digitalOrNotDataGridViewTextBoxColumn.DataPropertyName = "Digital or not";
            this.digitalOrNotDataGridViewTextBoxColumn.HeaderText = "Digital or not";
            this.digitalOrNotDataGridViewTextBoxColumn.Name = "digitalOrNotDataGridViewTextBoxColumn";
            this.digitalOrNotDataGridViewTextBoxColumn.Width = 70;
            // 
            // testFileDataGridViewTextBoxColumn
            // 
            this.testFileDataGridViewTextBoxColumn.DataPropertyName = "Test file";
            this.testFileDataGridViewTextBoxColumn.HeaderText = "Test file";
            this.testFileDataGridViewTextBoxColumn.Name = "testFileDataGridViewTextBoxColumn";
            this.testFileDataGridViewTextBoxColumn.Width = 64;
            // 
            // repetabilityFileDataGridViewTextBoxColumn
            // 
            this.repetabilityFileDataGridViewTextBoxColumn.DataPropertyName = "Repetability file";
            this.repetabilityFileDataGridViewTextBoxColumn.HeaderText = "Repetability file";
            this.repetabilityFileDataGridViewTextBoxColumn.Name = "repetabilityFileDataGridViewTextBoxColumn";
            this.repetabilityFileDataGridViewTextBoxColumn.Width = 95;
            // 
            // productImageDataGridViewTextBoxColumn
            // 
            this.productImageDataGridViewTextBoxColumn.DataPropertyName = "Product image";
            this.productImageDataGridViewTextBoxColumn.HeaderText = "Product image";
            this.productImageDataGridViewTextBoxColumn.Name = "productImageDataGridViewTextBoxColumn";
            this.productImageDataGridViewTextBoxColumn.Width = 92;
            // 
            // settingsFileDataGridViewTextBoxColumn
            // 
            this.settingsFileDataGridViewTextBoxColumn.DataPropertyName = "Settings file";
            this.settingsFileDataGridViewTextBoxColumn.HeaderText = "Settings file";
            this.settingsFileDataGridViewTextBoxColumn.Name = "settingsFileDataGridViewTextBoxColumn";
            this.settingsFileDataGridViewTextBoxColumn.Width = 79;
            // 
            // spectromCollectFileDataGridViewTextBoxColumn
            // 
            this.spectromCollectFileDataGridViewTextBoxColumn.DataPropertyName = "Spectrom collect file";
            this.spectromCollectFileDataGridViewTextBoxColumn.HeaderText = "Spectrom collect file";
            this.spectromCollectFileDataGridViewTextBoxColumn.Name = "spectromCollectFileDataGridViewTextBoxColumn";
            this.spectromCollectFileDataGridViewTextBoxColumn.Width = 105;
            // 
            // hardwareSetupDataGridViewTextBoxColumn
            // 
            this.hardwareSetupDataGridViewTextBoxColumn.DataPropertyName = "Hardware setup";
            this.hardwareSetupDataGridViewTextBoxColumn.HeaderText = "Hardware setup";
            this.hardwareSetupDataGridViewTextBoxColumn.Name = "hardwareSetupDataGridViewTextBoxColumn";
            this.hardwareSetupDataGridViewTextBoxColumn.Width = 98;
            // 
            // tSCBPowerSupplyListBindingSource
            // 
            this.tSCBPowerSupplyListBindingSource.DataMember = "TSCBPowerSupplyList";
            this.tSCBPowerSupplyListBindingSource.DataSource = this.tubesDataSet;
            // 
            // tubesDataSet
            // 
            this.tubesDataSet.DataSetName = "TubesDataSet";
            this.tubesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dgvBISettings
            // 
            this.dgvBISettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBISettings.AutoGenerateColumns = false;
            this.dgvBISettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvBISettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBISettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.moxtekDataGridViewTextBoxColumn1,
            this.kVSetpointDataGridViewTextBoxColumn,
            this.kVTolerancepercentDataGridViewTextBoxColumn,
            this.kVSteppersecDataGridViewTextBoxColumn,
            this.uASetpointDataGridViewTextBoxColumn,
            this.uATolerancepercentDataGridViewTextBoxColumn,
            this.uASteppersecDataGridViewTextBoxColumn,
            this.settlingTimesecDataGridViewTextBoxColumn,
            this.runTimehoursDataGridViewTextBoxColumn,
            this.photodiodeEnableDataGridViewCheckBoxColumn,
            this.photodiodeUpperLimDataGridViewTextBoxColumn,
            this.photodiodeLowerLimDataGridViewTextBoxColumn,
            this.cyclingEnableDataGridViewCheckBoxColumn,
            this.cyclingOnTimesecDataGridViewTextBoxColumn,
            this.cyclingOffTimesecDataGridViewTextBoxColumn});
            this.dgvBISettings.DataSource = this.arcLabBurnInSettingsBindingSource;
            this.dgvBISettings.Location = new System.Drawing.Point(286, 22);
            this.dgvBISettings.MultiSelect = false;
            this.dgvBISettings.Name = "dgvBISettings";
            this.dgvBISettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBISettings.Size = new System.Drawing.Size(541, 431);
            this.dgvBISettings.TabIndex = 3;
            // 
            // moxtekDataGridViewTextBoxColumn1
            // 
            this.moxtekDataGridViewTextBoxColumn1.DataPropertyName = "Moxtek#";
            this.moxtekDataGridViewTextBoxColumn1.HeaderText = "Moxtek#";
            this.moxtekDataGridViewTextBoxColumn1.Name = "moxtekDataGridViewTextBoxColumn1";
            this.moxtekDataGridViewTextBoxColumn1.Width = 74;
            // 
            // kVSetpointDataGridViewTextBoxColumn
            // 
            this.kVSetpointDataGridViewTextBoxColumn.DataPropertyName = "kV_Setpoint";
            this.kVSetpointDataGridViewTextBoxColumn.HeaderText = "kV_Setpoint";
            this.kVSetpointDataGridViewTextBoxColumn.Name = "kVSetpointDataGridViewTextBoxColumn";
            this.kVSetpointDataGridViewTextBoxColumn.Width = 90;
            // 
            // kVTolerancepercentDataGridViewTextBoxColumn
            // 
            this.kVTolerancepercentDataGridViewTextBoxColumn.DataPropertyName = "kV_Tolerance_percent";
            this.kVTolerancepercentDataGridViewTextBoxColumn.HeaderText = "kV_Tolerance_percent";
            this.kVTolerancepercentDataGridViewTextBoxColumn.Name = "kVTolerancepercentDataGridViewTextBoxColumn";
            this.kVTolerancepercentDataGridViewTextBoxColumn.Width = 141;
            // 
            // kVSteppersecDataGridViewTextBoxColumn
            // 
            this.kVSteppersecDataGridViewTextBoxColumn.DataPropertyName = "kV_Step_per_sec";
            this.kVSteppersecDataGridViewTextBoxColumn.HeaderText = "kV_Step_per_sec";
            this.kVSteppersecDataGridViewTextBoxColumn.Name = "kVSteppersecDataGridViewTextBoxColumn";
            this.kVSteppersecDataGridViewTextBoxColumn.Width = 117;
            // 
            // uASetpointDataGridViewTextBoxColumn
            // 
            this.uASetpointDataGridViewTextBoxColumn.DataPropertyName = "uA_Setpoint";
            this.uASetpointDataGridViewTextBoxColumn.HeaderText = "uA_Setpoint";
            this.uASetpointDataGridViewTextBoxColumn.Name = "uASetpointDataGridViewTextBoxColumn";
            this.uASetpointDataGridViewTextBoxColumn.Width = 90;
            // 
            // uATolerancepercentDataGridViewTextBoxColumn
            // 
            this.uATolerancepercentDataGridViewTextBoxColumn.DataPropertyName = "uA_Tolerance_percent";
            this.uATolerancepercentDataGridViewTextBoxColumn.HeaderText = "uA_Tolerance_percent";
            this.uATolerancepercentDataGridViewTextBoxColumn.Name = "uATolerancepercentDataGridViewTextBoxColumn";
            this.uATolerancepercentDataGridViewTextBoxColumn.Width = 141;
            // 
            // uASteppersecDataGridViewTextBoxColumn
            // 
            this.uASteppersecDataGridViewTextBoxColumn.DataPropertyName = "uA_Step_per_sec";
            this.uASteppersecDataGridViewTextBoxColumn.HeaderText = "uA_Step_per_sec";
            this.uASteppersecDataGridViewTextBoxColumn.Name = "uASteppersecDataGridViewTextBoxColumn";
            this.uASteppersecDataGridViewTextBoxColumn.Width = 117;
            // 
            // settlingTimesecDataGridViewTextBoxColumn
            // 
            this.settlingTimesecDataGridViewTextBoxColumn.DataPropertyName = "Settling_Time_sec";
            this.settlingTimesecDataGridViewTextBoxColumn.HeaderText = "Settling_Time_sec";
            this.settlingTimesecDataGridViewTextBoxColumn.Name = "settlingTimesecDataGridViewTextBoxColumn";
            this.settlingTimesecDataGridViewTextBoxColumn.Width = 119;
            // 
            // runTimehoursDataGridViewTextBoxColumn
            // 
            this.runTimehoursDataGridViewTextBoxColumn.DataPropertyName = "Run_Time_hours";
            this.runTimehoursDataGridViewTextBoxColumn.HeaderText = "Run_Time_hours";
            this.runTimehoursDataGridViewTextBoxColumn.Name = "runTimehoursDataGridViewTextBoxColumn";
            this.runTimehoursDataGridViewTextBoxColumn.Width = 113;
            // 
            // photodiodeEnableDataGridViewCheckBoxColumn
            // 
            this.photodiodeEnableDataGridViewCheckBoxColumn.DataPropertyName = "Photodiode_Enable";
            this.photodiodeEnableDataGridViewCheckBoxColumn.HeaderText = "Photodiode_Enable";
            this.photodiodeEnableDataGridViewCheckBoxColumn.Name = "photodiodeEnableDataGridViewCheckBoxColumn";
            this.photodiodeEnableDataGridViewCheckBoxColumn.Width = 106;
            // 
            // photodiodeUpperLimDataGridViewTextBoxColumn
            // 
            this.photodiodeUpperLimDataGridViewTextBoxColumn.DataPropertyName = "Photodiode_Upper_Lim";
            this.photodiodeUpperLimDataGridViewTextBoxColumn.HeaderText = "Photodiode_Upper_Lim";
            this.photodiodeUpperLimDataGridViewTextBoxColumn.Name = "photodiodeUpperLimDataGridViewTextBoxColumn";
            this.photodiodeUpperLimDataGridViewTextBoxColumn.Width = 143;
            // 
            // photodiodeLowerLimDataGridViewTextBoxColumn
            // 
            this.photodiodeLowerLimDataGridViewTextBoxColumn.DataPropertyName = "Photodiode_Lower_Lim";
            this.photodiodeLowerLimDataGridViewTextBoxColumn.HeaderText = "Photodiode_Lower_Lim";
            this.photodiodeLowerLimDataGridViewTextBoxColumn.Name = "photodiodeLowerLimDataGridViewTextBoxColumn";
            this.photodiodeLowerLimDataGridViewTextBoxColumn.Width = 143;
            // 
            // cyclingEnableDataGridViewCheckBoxColumn
            // 
            this.cyclingEnableDataGridViewCheckBoxColumn.DataPropertyName = "Cycling_Enable";
            this.cyclingEnableDataGridViewCheckBoxColumn.HeaderText = "Cycling_Enable";
            this.cyclingEnableDataGridViewCheckBoxColumn.Name = "cyclingEnableDataGridViewCheckBoxColumn";
            this.cyclingEnableDataGridViewCheckBoxColumn.Width = 86;
            // 
            // cyclingOnTimesecDataGridViewTextBoxColumn
            // 
            this.cyclingOnTimesecDataGridViewTextBoxColumn.DataPropertyName = "Cycling_On_Time_sec";
            this.cyclingOnTimesecDataGridViewTextBoxColumn.HeaderText = "Cycling_On_Time_sec";
            this.cyclingOnTimesecDataGridViewTextBoxColumn.Name = "cyclingOnTimesecDataGridViewTextBoxColumn";
            this.cyclingOnTimesecDataGridViewTextBoxColumn.Width = 138;
            // 
            // cyclingOffTimesecDataGridViewTextBoxColumn
            // 
            this.cyclingOffTimesecDataGridViewTextBoxColumn.DataPropertyName = "Cycling_Off_Time_sec";
            this.cyclingOffTimesecDataGridViewTextBoxColumn.HeaderText = "Cycling_Off_Time_sec";
            this.cyclingOffTimesecDataGridViewTextBoxColumn.Name = "cyclingOffTimesecDataGridViewTextBoxColumn";
            this.cyclingOffTimesecDataGridViewTextBoxColumn.Width = 138;
            // 
            // arcLabBurnInSettingsBindingSource
            // 
            this.arcLabBurnInSettingsBindingSource.DataMember = "ArcLabBurnInSettings";
            this.arcLabBurnInSettingsBindingSource.DataSource = this.tubesDataSet;
            // 
            // tSCBPowerSupplyListTableAdapter
            // 
            this.tSCBPowerSupplyListTableAdapter.ClearBeforeFill = true;
            // 
            // arcLabBurnInSettingsTableAdapter
            // 
            this.arcLabBurnInSettingsTableAdapter.ClearBeforeFill = true;
            // 
            // btnManualSet
            // 
            this.btnManualSet.Location = new System.Drawing.Point(209, 86);
            this.btnManualSet.Name = "btnManualSet";
            this.btnManualSet.Size = new System.Drawing.Size(71, 48);
            this.btnManualSet.TabIndex = 2;
            this.btnManualSet.Text = "Load To Selected Station\r\n";
            this.btnManualSet.UseVisualStyleBackColor = true;
            this.btnManualSet.Click += new System.EventHandler(this.btnManualSet_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 461);
            this.Controls.Add(this.dgvBISettings);
            this.Controls.Add(this.dgvPSList);
            this.Controls.Add(this.btnManualSet);
            this.Controls.Add(this.btnAddSet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmSettings";
            this.Text = "Burn-in Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tSCBPowerSupplyListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tubesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBISettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcLabBurnInSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TubesDataSet tubesDataSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddSet;
        private System.Windows.Forms.DataGridView dgvPSList;
        private System.Windows.Forms.BindingSource tSCBPowerSupplyListBindingSource;
        private TubesDataSetTableAdapters.TSCBPowerSupplyListTableAdapter tSCBPowerSupplyListTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn moxtekDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vMIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kVVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uAVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxKVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minKVDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxUADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minUADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pSVoltageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wattsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tUBNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn digitalOrNotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn testFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn repetabilityFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productImageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn settingsFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn spectromCollectFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hardwareSetupDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dgvBISettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn kVStepDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kVThresholdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uAStepDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uAThresholdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxHoursDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photodiodeThresholdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cycleTimesecDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource arcLabBurnInSettingsBindingSource;
        private TubesDataSetTableAdapters.ArcLabBurnInSettingsTableAdapter arcLabBurnInSettingsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn moxtekDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn kVSetpointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kVTolerancepercentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kVSteppersecDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uASetpointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uATolerancepercentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uASteppersecDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn settlingTimesecDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn runTimehoursDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn photodiodeEnableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photodiodeUpperLimDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photodiodeLowerLimDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cyclingEnableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cyclingOnTimesecDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cyclingOffTimesecDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnManualSet;
    }
}