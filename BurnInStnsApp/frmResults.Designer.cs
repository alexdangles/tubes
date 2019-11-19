namespace BurnInStns
{
    partial class frmResults
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
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.testIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serialNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recipeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeStartDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeEndDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cubbyIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passFailDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.errorMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runTimehoursDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arcLabBurnInResultsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tubesDataSet = new BurnInStns.TubesDataSet();
            this.arcLabBurnInResultsTableAdapter = new BurnInStns.TubesDataSetTableAdapters.ArcLabBurnInResultsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcLabBurnInResultsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tubesDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AutoGenerateColumns = false;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.testIDDataGridViewTextBoxColumn,
            this.serialNumberDataGridViewTextBoxColumn,
            this.recipeDataGridViewTextBoxColumn,
            this.timeStartDataGridViewTextBoxColumn,
            this.timeEndDataGridViewTextBoxColumn,
            this.cubbyIDDataGridViewTextBoxColumn,
            this.passFailDataGridViewCheckBoxColumn,
            this.errorMessageDataGridViewTextBoxColumn,
            this.runTimehoursDataGridViewTextBoxColumn,
            this.errorCountDataGridViewTextBoxColumn});
            this.dgvResults.DataSource = this.arcLabBurnInResultsBindingSource;
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(0, 0);
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.Size = new System.Drawing.Size(845, 455);
            this.dgvResults.TabIndex = 3;
            // 
            // testIDDataGridViewTextBoxColumn
            // 
            this.testIDDataGridViewTextBoxColumn.DataPropertyName = "TestID";
            this.testIDDataGridViewTextBoxColumn.HeaderText = "TestID";
            this.testIDDataGridViewTextBoxColumn.Name = "testIDDataGridViewTextBoxColumn";
            this.testIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.testIDDataGridViewTextBoxColumn.Width = 64;
            // 
            // serialNumberDataGridViewTextBoxColumn
            // 
            this.serialNumberDataGridViewTextBoxColumn.DataPropertyName = "SerialNumber";
            this.serialNumberDataGridViewTextBoxColumn.HeaderText = "SerialNumber";
            this.serialNumberDataGridViewTextBoxColumn.Name = "serialNumberDataGridViewTextBoxColumn";
            this.serialNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.serialNumberDataGridViewTextBoxColumn.Width = 95;
            // 
            // recipeDataGridViewTextBoxColumn
            // 
            this.recipeDataGridViewTextBoxColumn.DataPropertyName = "Recipe";
            this.recipeDataGridViewTextBoxColumn.HeaderText = "Recipe";
            this.recipeDataGridViewTextBoxColumn.Name = "recipeDataGridViewTextBoxColumn";
            this.recipeDataGridViewTextBoxColumn.ReadOnly = true;
            this.recipeDataGridViewTextBoxColumn.Width = 66;
            // 
            // timeStartDataGridViewTextBoxColumn
            // 
            this.timeStartDataGridViewTextBoxColumn.DataPropertyName = "Time_Start";
            this.timeStartDataGridViewTextBoxColumn.HeaderText = "Time_Start";
            this.timeStartDataGridViewTextBoxColumn.Name = "timeStartDataGridViewTextBoxColumn";
            this.timeStartDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeStartDataGridViewTextBoxColumn.Width = 83;
            // 
            // timeEndDataGridViewTextBoxColumn
            // 
            this.timeEndDataGridViewTextBoxColumn.DataPropertyName = "Time_End";
            this.timeEndDataGridViewTextBoxColumn.HeaderText = "Time_End";
            this.timeEndDataGridViewTextBoxColumn.Name = "timeEndDataGridViewTextBoxColumn";
            this.timeEndDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeEndDataGridViewTextBoxColumn.Width = 80;
            // 
            // cubbyIDDataGridViewTextBoxColumn
            // 
            this.cubbyIDDataGridViewTextBoxColumn.DataPropertyName = "CubbyID";
            this.cubbyIDDataGridViewTextBoxColumn.HeaderText = "CubbyID";
            this.cubbyIDDataGridViewTextBoxColumn.Name = "cubbyIDDataGridViewTextBoxColumn";
            this.cubbyIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.cubbyIDDataGridViewTextBoxColumn.Width = 73;
            // 
            // passFailDataGridViewCheckBoxColumn
            // 
            this.passFailDataGridViewCheckBoxColumn.DataPropertyName = "PassFail";
            this.passFailDataGridViewCheckBoxColumn.HeaderText = "PassFail";
            this.passFailDataGridViewCheckBoxColumn.Name = "passFailDataGridViewCheckBoxColumn";
            this.passFailDataGridViewCheckBoxColumn.ReadOnly = true;
            this.passFailDataGridViewCheckBoxColumn.Width = 52;
            // 
            // errorMessageDataGridViewTextBoxColumn
            // 
            this.errorMessageDataGridViewTextBoxColumn.DataPropertyName = "ErrorMessage";
            this.errorMessageDataGridViewTextBoxColumn.HeaderText = "ErrorMessage";
            this.errorMessageDataGridViewTextBoxColumn.Name = "errorMessageDataGridViewTextBoxColumn";
            this.errorMessageDataGridViewTextBoxColumn.ReadOnly = true;
            this.errorMessageDataGridViewTextBoxColumn.Width = 97;
            // 
            // runTimehoursDataGridViewTextBoxColumn
            // 
            this.runTimehoursDataGridViewTextBoxColumn.DataPropertyName = "RunTime_hours";
            this.runTimehoursDataGridViewTextBoxColumn.HeaderText = "RunTime_hours";
            this.runTimehoursDataGridViewTextBoxColumn.Name = "runTimehoursDataGridViewTextBoxColumn";
            this.runTimehoursDataGridViewTextBoxColumn.ReadOnly = true;
            this.runTimehoursDataGridViewTextBoxColumn.Width = 107;
            // 
            // errorCountDataGridViewTextBoxColumn
            // 
            this.errorCountDataGridViewTextBoxColumn.DataPropertyName = "ErrorCount";
            this.errorCountDataGridViewTextBoxColumn.HeaderText = "ErrorCount";
            this.errorCountDataGridViewTextBoxColumn.Name = "errorCountDataGridViewTextBoxColumn";
            this.errorCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.errorCountDataGridViewTextBoxColumn.Width = 82;
            // 
            // arcLabBurnInResultsBindingSource
            // 
            this.arcLabBurnInResultsBindingSource.DataMember = "ArcLabBurnInResults";
            this.arcLabBurnInResultsBindingSource.DataSource = this.tubesDataSet;
            // 
            // tubesDataSet
            // 
            this.tubesDataSet.DataSetName = "TubesDataSet";
            this.tubesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // arcLabBurnInResultsTableAdapter
            // 
            this.arcLabBurnInResultsTableAdapter.ClearBeforeFill = true;
            // 
            // frmResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 455);
            this.Controls.Add(this.dgvResults);
            this.Name = "frmResults";
            this.Text = "Burn-in Results";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmResults_FormClosing);
            this.Load += new System.EventHandler(this.frmResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcLabBurnInResultsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tubesDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvResults;
        private TubesDataSet tubesDataSet;
        private System.Windows.Forms.BindingSource arcLabBurnInResultsBindingSource;
        private TubesDataSetTableAdapters.ArcLabBurnInResultsTableAdapter arcLabBurnInResultsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn testIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recipeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeStartDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeEndDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cubbyIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn passFailDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorMessageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn runTimehoursDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorCountDataGridViewTextBoxColumn;
    }
}