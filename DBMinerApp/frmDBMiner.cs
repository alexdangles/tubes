using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using NationalInstruments.UI.WindowsForms;
using System.Drawing.Imaging;
using Helper;

namespace DBMiner
{
    /// <summary>
    /// Moxtek Tubes Database and Test Viewer User Interface.
    /// </summary>
	public partial class frmDBMiner : Form
	{
		ScatterGraph selectedGraph;
        DataSet psList, specList;
        frmPassword pwd = new frmPassword();

        enum Operation
		{
			add,
			subtract,
			multiply,
			divide
		}

		enum PreCalData
		{
			KEV,
			SET_KEV,
			MON_KEV,
			KEV_EDGE
		}

        enum SourceCalValidation
		{
			KEV,
			SET_KEV,
			MON_KEV,
			KEV_EDGE_SAM,
			KEV_EDGE_SWC,
			KEV_EDGE_OLD,
			PASS
		}

        enum SourceCalibration
		{
			CALIBRATED_KEV,
			CALIBRATED_KEV_BYTE,
			PAGE1_KEV,
			CALIBRATED_SET_KEV_BYTE,
			PAGE1_SET_KEV,
			CALIBRATED_MONITOR_KEV_BYTE,
			PAGE1_MONITOR_KEV_BYTE
		}

        enum PowerSettingsRawData
		{
			INPUT_POWER_VOLTS,
			INPUT_SET_KV_CONTROL,
			INPUT_SET_UA_CONTROL,
			SET_KV_CONTROL,
			SET_KV_CONTROL_STD,
			SET_UA_CONTROL,
			SET_UA_CONTROL_STD,
			MON_KV,
			MON_KV_STD,
			HV_ENABLE,
			FILAMENT_READY,
			MON_UA,
			MON_UA_STD,
			SET_POWER_VOLTS,
			SET_POWER_VOLTS_STD,
			POWER_AMPS,
			POWER_AMPS_STD,
			TOTAL_WATTS,
			TOTAL_WATTS_STD,
			TUBE_WATTS,
			TUBE_WATTS_STD,
			TUBE_POWER_EFFICIENCY,
			TUBE_POWER_EFFICIENCY_STD,
			PHOTODIODE_1,
			PHOTODIODE_1_STD,
			PHOTODIODE_2,
			PHOTODIODE_2_STD,
			TEMP_1,
			TEMP_2,
			TEMP_3,
			TEMP_4,
			TEMP_5,
			HUMIDITY,
			HARD_STARTUP_TIME_SEC,
			HARD_STARTUP_TIME_SEC_STD,
			SOFT_STARTUP_TIME_SEC,
			SOFT_STARTUP_TIME_SEC_STD,
			CURRENT_STARTUP_TIME_SEC,
			CURRENT_STARTUP_TIME_SEC_STD,
			OTHER_1,
			OTHER_2,
			OTHER_3,
			OTHER_4,
			OTHER_5
		}

        enum SpectrumSummaryRawData
		{
			ELAPSED_TIME_SEC,
			RUN_NUMBER,
			ROI1_2_10KEV,
			ROI2_10_20KEV,
			ROI3_20_30KEV,
			ROI4_30_40KEV,
			ROI5_40_60KEV,
			ROI_TOTAL_2_60KEV,
			EDGE_12KEV,
			INPUT_POWER_AMPS,
			INPUT_POWER_VOLTS,
			UA_MONITOR,
			UA_CONTROL,
			KV_MONITOR,
			KV_CONTROL,
			KV_ENABLE_ANALOG,
			FILAMENT_READY,
			PHOTO_DIODE_FLUX,
			KV_ENABLE_DIGITAL,
			INTERLOCK_FEEDBACK,
			AUX_3,
			AUX_4,
			AUX_5
		}

        DataTable
			tests,
			directDetectionFluxStab,
			imageQuality,
			powerSupplyStability,
			specs,
			powerSettingsRawData,
			preCalData,
			sourceCalibration,
			sourceCalValidation;

		public frmDBMiner()
		{
            InitializeComponent();
            tabControl2.TabPages.Remove(tabRingo);
            tabControl2.TabPages.Remove(tabSummary);
            tabControl2.TabPages.Remove(tabStabRep);
            tabControl2.TabPages.Remove(tabSpectrum);
            tabControl2.TabPages.Remove(tabImageTest);
            tabControl2.TabPages.Remove(tabSettings);
            tabControl2.TabPages.Remove(tabHelp);
            lnkSpot.Text = null;
        }

        private void frmData_Shown(object sender, EventArgs e)
        {
            Text = "Loading database...please wait";
            DateTime now = DateTime.Now;
            tests = DBHelper.Query(
                $"select * from TSCBTest where TestDate > Convert(datetime, '{ now.Year - 1 }-{ now.Month }-{ now.Day }') order by 1 desc"
                );
            viewTests.DataSource = tests;
            viewTables.DataSource = DBHelper.Query($"select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME like '%{ txtTableSearch.Text }%' order by TABLE_NAME");
            List<string> list = new List<string>();
            foreach (DataRow r in tests.Rows)
            {
                list.Add(r[3].ToString());
            }
            string[] testboxes = list.Distinct().ToArray();
            cbxFilterBox.DataSource = testboxes;
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtmTo.Value = DateTime.Now;
            psList = new DataSet();
            sqlTSCBPowerSupplyList.Fill(psList);
            viewTSCBPowerSupplyList.DataSource = psList.Tables[0];
            foreach (DataGridViewColumn c in viewTSCBPowerSupplyList.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            specList = new DataSet();
            sqlTSCBSpecs.Fill(specList);
            viewTSCBSpecs.DataSource = specList.Tables[0];
            foreach (DataGridViewColumn c in viewTSCBSpecs.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            Text = "Tubes Database Miner";
        }

        private void frmData_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (sqlTubesConnection.State == ConnectionState.Open) sqlTubesConnection.Close();
        }

        private void viewTables_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				string table = viewTables.Rows[e.RowIndex].Cells["TABLE_NAME"].FormattedValue.ToString();
				string query = $"select top 1000 * from \r\n{ table }\r\n order by 1 desc";
				txtSQLQuery.Text = query;
				viewData.DataSource = DBHelper.Query(query);
			}
		}

        private void viewData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                object selectedCell = viewData.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;
                if (selectedCell.GetType() == typeof(bool))
                {
                    selectedCell = (bool)selectedCell ? 1 : 0;
                }
                string query = $"select * from \r\n { viewTables.Rows[viewTables.CurrentCell.RowIndex].Cells[0].FormattedValue }\r\n" +
                    $"where { viewData.Columns[e.ColumnIndex].HeaderText }\r\n like '{ selectedCell }'";
				txtSQLQuery.Text = query;
				viewData.DataSource = DBHelper.Query(query);
			}
		}

        private void viewTests_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SelectTest(e.RowIndex);
        }

        private void txtTableSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) viewTables.DataSource = DBHelper.Query($"select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_NAME like '%{ txtTableSearch.Text }%' order by TABLE_NAME");
		}

        private void lnkSpot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("explorer.exe", lnkSpot.Text);
		}

        private void graph1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && ModifierKeys == Keys.None)
			{
				ScatterGraph graph = sender as ScatterGraph;
				selectedGraph = graph;
				contextMenuStrip1.Show(Cursor.Position);
			}
			else contextMenuStrip1.Hide();
		}

        private void graph2_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && ModifierKeys == Keys.None)
			{
				ScatterGraph graph = sender as ScatterGraph;
				selectedGraph = graph;
				contextMenuStrip2.Show(Cursor.Position);
			}
			else contextMenuStrip2.Hide();
		}

        private void graph3_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && ModifierKeys == Keys.None)
			{
				ScatterGraph graph = sender as ScatterGraph;
				selectedGraph = graph;
				contextMenuStrip3.Show(Cursor.Position);
			}
			else contextMenuStrip3.Hide();
		}

        private void graph4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ModifierKeys == Keys.None)
            {
                ScatterGraph graph = sender as ScatterGraph;
                selectedGraph = graph;
                contextMenuStrip3.Show(Cursor.Position);
            }
            else contextMenuStrip3.Hide();
        }

        private void graph_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ScatterGraph graph = sender as ScatterGraph;
			graph.ResetZoomPan();
		}

        private void btnSQLQuery_Click(object sender, EventArgs e)
		{
			viewData.DataSource = DBHelper.Query(txtSQLQuery.Text);
		}

        private void plotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool[] chk = new bool[plotsToolStripMenuItem.DropDownItems.Count];
			foreach (ToolStripMenuItem t in plotsToolStripMenuItem.DropDownItems)
            {
                chk[plotsToolStripMenuItem.DropDownItems.IndexOf(t)] = t.Checked;
            }
            for (int i = 0; i < plotsToolStripMenuItem.DropDownItems.Count; i++)
			{
                if (tabControl2.TabPages.Contains(tabRingo)) graphRingoSpectra.Plots[i].Visible = chk[i];
                else
                {
                    graphInputVoltage.Plots[i].Visible
                        = graphInputCurrent.Plots[i].Visible
                        = graphSpectrum.Plots[i].Visible
                        = graphkeVEdge.Plots[i].Visible
                        = graphPhotodiode.Plots[i].Visible
                        = graphTotalCounts.Plots[i].Visible
                        = graphSetkV.Plots[i].Visible
                        = graphMonkV.Plots[i].Visible
                        = graphSetuA.Plots[i].Visible
                        = graphSetuA.Plots[i].Visible
                        = graphMonuA.Plots[i].Visible
                        = graphTFR.Plots[i].Visible
                        = graphAnalogEnable.Plots[i].Visible
                        = graphDigitalEnable.Plots[i].Visible
                        = graphInterlockFeedback.Plots[i].Visible
                        = chk[i];

                    legend1.Items[i].Source = graphInputVoltage.Plots[i];
                    legend2.Items[i].Source = graphSpectrum.Plots[i];
                    legend3.Items[i].Source = graphkeVEdge.Plots[i];
                    legend6.Items[i].Source = graphSetkV.Plots[i];
                    legend1.Items[i].Text
                        = legend2.Items[i].Text
                        = legend3.Items[i].Text
                        = legend6.Items[i].Text
                        = plotsToolStripMenuItem.DropDownItems[i].Text;
                    legend1.Items[i].Visible
                        = legend2.Items[i].Visible
                        = legend3.Items[i].Visible
                        = legend6.Items[i].Visible
                        = chk[i];
                }
            }
			plotsToolStripMenuItem.ShowDropDown();
		}

        private void meanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			sTDToolStripMenuItem.Checked = false;
			graphuAkV.Plots[0].Visible
				= graphSetkVMonkV.Plots[0].Visible
				= graphSetuAMonuA.Plots[0].Visible
				= graphPSVoltage.Plots[0].Visible
				= graphDeltauADeltakV.Plots[0].Visible
				= graphPhotodiodekV.Plots[0].Visible
				= graphPhotodiodeuA.Plots[0].Visible
				= true;

			graphuAkV.Plots[1].Visible
				= graphSetkVMonkV.Plots[1].Visible
				= graphSetuAMonuA.Plots[1].Visible
				= graphPSVoltage.Plots[1].Visible
				= graphDeltauADeltakV.Plots[1].Visible
				= graphPhotodiodekV.Plots[1].Visible
				= graphPhotodiodeuA.Plots[1].Visible
				= false;
		}

        private void sTDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			meanToolStripMenuItem.Checked = false;
			graphuAkV.Plots[0].Visible
				= graphSetkVMonkV.Plots[0].Visible
				= graphSetuAMonuA.Plots[0].Visible
				= graphPSVoltage.Plots[0].Visible
				= graphDeltauADeltakV.Plots[0].Visible
				= graphPhotodiodekV.Plots[0].Visible
				= graphPhotodiodeuA.Plots[0].Visible
				= false;

			graphuAkV.Plots[1].Visible
				= graphSetkVMonkV.Plots[1].Visible
				= graphSetuAMonuA.Plots[1].Visible
				= graphPSVoltage.Plots[1].Visible
				= graphDeltauADeltakV.Plots[1].Visible
				= graphPhotodiodekV.Plots[1].Visible
				= graphPhotodiodeuA.Plots[1].Visible
				= true;
		}

        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.Filter = "PNG files (*.png)|*.png";
            saveFileDialog1.FileName = null;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Bitmap bmp = new Bitmap(selectedGraph.Width, selectedGraph.Height);
				selectedGraph.DrawToBitmap(bmp, new Rectangle(0, 0, selectedGraph.Width, selectedGraph.Height));
				bmp.Save(saveFileDialog1.FileName, ImageFormat.Png);
				bmp.Dispose();
			}
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtFilterSerial.Enabled = !txtFilterSerial.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            cbxFilterBox.Enabled = !cbxFilterBox.Enabled;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtmTo.Enabled = !dtpFrom.Enabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetTests();
        }

        private void viewTSCBSpecs_Leave(object sender, EventArgs e)
        {
            UpdateDB();
        }

        private void txtTUB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtTUB.Text != "")
            {
                NewPSEntry();
            }
        }

        private void plotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripDropDownItem t in plotsToolStripMenuItem.DropDownItems)
            {
                t.PerformClick();
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl1.SelectedTab == tabRecipe)
            {
                if (pwd.askAlways)
                {
                    txtTUB.Clear();
                }
                else
                {
                    tabControl1.SelectedTab = tabTest;
                }
            }
        }

        private void txtSQLQuery_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtSQLQuery.Text == "Enter SQL query here.") txtSQLQuery.Clear();
        }

        private void deviationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphRingoCalValidation.Plots[0].Visible = graphRingoCalValidation.Plots[1].Visible = graphRingoCalValidation.Plots[2].Visible = graphRingoCalValidation.Plots[3].Visible
                = graphRingoPreCal.Plots[0].Visible = graphRingoPreCal.Plots[1].Visible = graphRingoPreCal.Plots[2].Visible = graphRingoPreCal.Plots[3].Visible
                = legend5.Items[2].Visible = !deviationToolStripMenuItem.Checked;
            graphRingoCalValidation.Plots[4].Visible = graphRingoPreCal.Plots[4].Visible = legend5.Items[3].Visible = deviationToolStripMenuItem.Checked;
        }

        double[] GetData(DataTable dt, int column)
		{
			double[] data = new double[dt.Rows.Count];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = Convert.ToDouble(dt.Rows[i][column]);
			}
			return data;
		}

        double[] GetData(DataTable dt, int column1, int column2, Operation operation)
		{
			double[] data = new double[dt.Rows.Count];
			for (int i = 0; i < data.Length; i++)
			{
				switch (operation)
				{
					case Operation.add:
						data[i] = Convert.ToDouble(dt.Rows[i][(int)column1]) + Convert.ToDouble(dt.Rows[i][(int)column2]);
						break;
					case Operation.subtract:
						data[i] = Convert.ToDouble(dt.Rows[i][(int)column1]) - Convert.ToDouble(dt.Rows[i][(int)column2]);
						break;
					case Operation.multiply:
						data[i] = Convert.ToDouble(dt.Rows[i][(int)column1]) * Convert.ToDouble(dt.Rows[i][(int)column2]);
						break;
					case Operation.divide:
						data[i] = Convert.ToDouble(dt.Rows[i][(int)column1]) / Convert.ToDouble(dt.Rows[i][(int)column2]);
						break;
				}
			}
			return data;
		}

        Tuple<double[], double[]> GetSpectrumData(DataTable dt)
		{
			double[] x = new double[dt.Rows.Count];
			double[] y = new double[dt.Rows.Count];
			for (int i = 0; i < x.Length; i++)
			{
				x[i] = Convert.ToDouble(dt.Rows[i][0]) / 1000;
				y[i] = Convert.ToDouble(dt.Rows[i][1]);
			}
			return new Tuple<double[], double[]>(x, y);
		}

        private void GetTests()
        {
            string query = $"declare @start datetime, @end datetime select @start = '{ dtpFrom.Value.Year }-{ dtpFrom.Value.Month }-{ dtpFrom.Value.Day } 00:00:00'," +
                $"@end = '{ dtmTo.Value.Year }-{ dtmTo.Value.Month }-{ dtmTo.Value.Day } { dtmTo.Value.Hour }:{ dtmTo.Value.Minute }:{ dtmTo.Value.Second }'" +
                $"select * from TSCBTest where Test_ID_PK > 0";
            if (chkFilterSerial.Checked) query += $" and Serial like '{ txtFilterSerial.Text }%'";
            if (chkFilterBox.Checked) query += $" and TestBox like '{ cbxFilterBox.SelectedItem }'";
            if (chkFilterDate.Checked)
            {
                if (dtpFrom.Value > dtmTo.Value) MessageBox.Show("To date must be greater than or the same as from date.");
                query += " and TestDate >= @start and TestDate <= @end";
            }
            query += " order by 1 desc";
            DataTable dt = DBHelper.Query(query);
            viewTests.DataSource = tests = dt == null ? tests : dt;
        }

        private void SelectTest(int rowIndex)
        {
            if (tests != null)
            {
                txtFilterSerial.Text = viewTests.CurrentRow.Cells[1].FormattedValue.ToString();
                cbxFilterBox.SelectedItem = viewTests.CurrentRow.Cells[3].FormattedValue.ToString();

                // Clear existing data
                if (tabControl2.TabPages.Contains(tabHelp)) tabControl2.TabPages.Remove(tabHelp);
                DataTable dt = new DataTable();
                string[] numOfSettings = null;
                directDetectionFluxStab = null;
                imageQuality = null;
                powerSupplyStability = null;
                specs = null;
                powerSettingsRawData = null;
                preCalData = null;
                sourceCalibration = null;
                sourceCalValidation = null;

                ClearGraphs(
                graphSpectrum,

                graphDeltauADeltakV,
                graphuAkV,
                graphPSVoltage,
                graphSetkVMonkV,
                graphSetuAMonuA,

                graphPhotodiodekV,
                graphPhotodiodeuA,

                graphkeVEdge,
                graphTotalCounts,
                graphPhotodiode,

                graphInputCurrent,
                graphInputVoltage,

                graphSetkV,
                graphSetuA,
                graphMonkV,
                graphMonuA,
                graphTFR,
                graphAnalogEnable,
                graphDigitalEnable,
                graphInterlockFeedback,

                graphRingoCal,
                graphRingoCalValidation,
                graphRingoPreCal,
                graphRingoSpectra
                );

                lblRingoPass.Visible = ledRingoPass1.Visible = ledRingoPass2.Visible = ledRingoPass3.Visible = ledRingoPass4.Visible = ledRingoPass5.Visible = ledRingoPass6.Visible = ledRingoPass7.Visible = false;
                txtRingoInfo.Clear();
                plotsToolStripMenuItem.DropDownItems.Clear();

                try
                {
                    int testID = Convert.ToInt32(tests.Rows[rowIndex][0]);
                    int sn = Convert.ToInt32(tests.Rows[rowIndex][1]);
                    dt = DBHelper.SearchTable("viewProductionSourceCharacterization", "SN", sn);
                    string target = dt == null ? "" : dt.Rows[0][1].ToString();
                    string tubNum = dt == null ? "" : dt.Rows[0][2].ToString();
                    string psType = dt == null ? "" : dt.Rows[0][3].ToString();

                    dt = DBHelper.SearchTable("TSCBSourceCalValidation", "Test_ID", testID);
                    if (dt != null)  // check if ringo cal table is present
                    {
                        #region Ringo Calibration
                        if (deviationToolStripMenuItem.Checked) deviationToolStripMenuItem.PerformClick();
                        if (!tabControl2.TabPages.Contains(tabRingo))
                        {
                            tabControl2.TabPages.Add(tabRingo);
                            tabControl2.TabPages.Remove(tabSummary);
                            tabControl2.TabPages.Remove(tabStabRep);
                            tabControl2.TabPages.Remove(tabSpectrum);
                            tabControl2.TabPages.Remove(tabImageTest);
                            tabControl2.TabPages.Remove(tabSettings);
                        }
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        sourceCalValidation = dt;
                        List<string> settingsList = new List<string>();
                        foreach (DataRow r in sourceCalValidation.Rows)
                        {
                            settingsList.Add($"{ r[0].ToString() } keV");
                        }
                        numOfSettings = settingsList.Distinct().ToArray();
                        graphRingoCalValidation.Plots[0].PlotXY(GetData(sourceCalValidation, (int)SourceCalValidation.KEV), GetData(sourceCalValidation, (int)SourceCalValidation.KEV));
                        graphRingoCalValidation.Plots[1].PlotXY(GetData(sourceCalValidation, (int)SourceCalValidation.KEV), GetData(sourceCalValidation, (int)SourceCalValidation.SET_KEV));
                        graphRingoCalValidation.Plots[2].PlotXY(GetData(sourceCalValidation, (int)SourceCalValidation.KEV), GetData(sourceCalValidation, (int)SourceCalValidation.MON_KEV));
                        graphRingoCalValidation.Plots[3].PlotXY(GetData(sourceCalValidation, (int)SourceCalValidation.KEV), GetData(sourceCalValidation, (int)SourceCalValidation.KEV_EDGE_SAM));
                        graphRingoCalValidation.Plots[4].PlotXY(GetData(sourceCalValidation, (int)SourceCalValidation.KEV), GetData(sourceCalValidation, (int)SourceCalValidation.KEV_EDGE_SAM, (int)SourceCalValidation.MON_KEV, Operation.subtract));


                        dt = DBHelper.SearchTable("TSCBSourceCalibration", "Test_ID", testID);
                        if (dt != null) txtRingoInfo.AppendText("Calibration Run\r\n");
                        else txtRingoInfo.AppendText("Validation Run\r\n");
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        sourceCalibration = dt;
                        graphRingoCal.Plots[0].PlotXY(GetData(sourceCalibration, (int)SourceCalibration.CALIBRATED_KEV_BYTE), GetData(sourceCalibration, (int)SourceCalibration.CALIBRATED_KEV_BYTE));
                        graphRingoCal.Plots[1].PlotXY(GetData(sourceCalibration, (int)SourceCalibration.CALIBRATED_KEV_BYTE), GetData(sourceCalibration, (int)SourceCalibration.CALIBRATED_SET_KEV_BYTE));
                        graphRingoCal.Plots[2].PlotXY(GetData(sourceCalibration, (int)SourceCalibration.CALIBRATED_KEV_BYTE), GetData(sourceCalibration, (int)SourceCalibration.CALIBRATED_MONITOR_KEV_BYTE));

                        dt = DBHelper.SearchTable("TSCBPreCalData", "Test_ID", testID);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        preCalData = dt;
                        graphRingoPreCal.Plots[0].PlotXY(GetData(preCalData, (int)PreCalData.KEV), GetData(preCalData, (int)PreCalData.KEV));
                        graphRingoPreCal.Plots[1].PlotXY(GetData(preCalData, (int)PreCalData.KEV), GetData(preCalData, (int)PreCalData.SET_KEV));
                        graphRingoPreCal.Plots[2].PlotXY(GetData(preCalData, (int)PreCalData.KEV), GetData(preCalData, (int)PreCalData.MON_KEV));
                        graphRingoPreCal.Plots[3].PlotXY(GetData(preCalData, (int)PreCalData.KEV), GetData(preCalData, (int)PreCalData.KEV_EDGE));
                        graphRingoPreCal.Plots[4].PlotXY(GetData(preCalData, (int)PreCalData.KEV), GetData(preCalData, (int)PreCalData.KEV_EDGE, (int)PreCalData.MON_KEV, Operation.subtract));

                        graphRingoCalValidation.Plots[1].Visible = graphRingoCalValidation.Plots[2].Visible = graphRingoCalValidation.Plots[3].Visible = graphRingoCal.Plots[1].Visible = graphRingoCal.Plots[2].Visible
                        = graphRingoPreCal.Plots[1].Visible = graphRingoPreCal.Plots[2].Visible = graphRingoPreCal.Plots[3].Visible = true;

                        int[] keV = new int[] { 8, 13, 15, 25, 35, 40, 50 };
                        for (int i = 0; i < keV.Length; i++)
                        {
                            legend4.Items[i].Text = $"{ keV[i].ToString() } keV";
                            dt = DBHelper.Query($"select * from TSCBSpectrumData where TestID_FK_CPK = { testID } and Test = { keV[i].ToString() }");
                            if (dt != null)
                            {
                                dt.Columns.RemoveAt(0);
                                graphRingoSpectra.Plots[i].PlotXY(GetSpectrumData(dt).Item1, GetSpectrumData(dt).Item2);
                            }
                        }
                        if (sourceCalValidation != null)
                        {
                            lblRingoPass.Visible = ledRingoPass1.Visible = sourceCalValidation.Rows.Count > 0;
                            ledRingoPass2.Visible = sourceCalValidation.Rows.Count > 1;
                            ledRingoPass3.Visible = sourceCalValidation.Rows.Count > 2;
                            ledRingoPass4.Visible = sourceCalValidation.Rows.Count > 3;
                            ledRingoPass5.Visible = sourceCalValidation.Rows.Count > 4;
                            ledRingoPass6.Visible = sourceCalValidation.Rows.Count > 5;
                            ledRingoPass7.Visible = sourceCalValidation.Rows.Count > 6;
                            ledRingoPass1.Value = sourceCalValidation.Rows.Count > 0 && Convert.ToBoolean(sourceCalValidation.Rows[0][6]);
                            ledRingoPass2.Value = sourceCalValidation.Rows.Count > 1 && Convert.ToBoolean(sourceCalValidation.Rows[1][6]);
                            ledRingoPass3.Value = sourceCalValidation.Rows.Count > 2 && Convert.ToBoolean(sourceCalValidation.Rows[2][6]);
                            ledRingoPass4.Value = sourceCalValidation.Rows.Count > 3 && Convert.ToBoolean(sourceCalValidation.Rows[3][6]);
                            ledRingoPass5.Value = sourceCalValidation.Rows.Count > 4 && Convert.ToBoolean(sourceCalValidation.Rows[4][6]);
                            ledRingoPass6.Value = sourceCalValidation.Rows.Count > 5 && Convert.ToBoolean(sourceCalValidation.Rows[5][6]);
                            ledRingoPass7.Value = sourceCalValidation.Rows.Count > 6 && Convert.ToBoolean(sourceCalValidation.Rows[6][6]);
                        }
                        txtRingoInfo.AppendText($"{ tubNum }\r\n");
                        txtRingoInfo.AppendText(psType);
                        #endregion
                    }
                    else
                    {
                        #region TSCB
                        if (tabControl2.TabPages.Contains(tabRingo) || tabControl2.SelectedTab == null)
                        {
                            tabControl2.TabPages.Add(tabSummary);
                            tabControl2.TabPages.Add(tabStabRep);
                            tabControl2.TabPages.Add(tabSpectrum);
                            tabControl2.TabPages.Add(tabImageTest);
                            tabControl2.TabPages.Add(tabSettings);
                            tabControl2.TabPages.Remove(tabRingo);
                        }

                        dt = DBHelper.SearchTable("TSCBPowerSupplyStability", "Test_ID_FK", testID);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        powerSupplyStability = dt;

                        dt = DBHelper.SearchTable("TSCBSpecs", "TubePartNumber", tubNum);
                        specs = dt;

                        dt = DBHelper.SearchTable("TSCBDirectDetectionFluxStab", "Test_ID_FK", testID);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        directDetectionFluxStab = dt;
                        List<string> settingsList = new List<string>();
                        foreach (DataRow r in directDetectionFluxStab.Rows)
                        {
                            settingsList.Add(r[3].ToString());
                        }
                        numOfSettings = settingsList.Distinct().ToArray();

                        dt = DBHelper.SearchTable("TSCBImageQuality", "Test_ID_FK", testID);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        imageQuality = dt;

                        dt = DBHelper.SearchTable("TSCBPassFail", "Test_ID_FK", testID);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);

                        lnkSpot.Text = null;
                        led1.Value = Convert.ToBoolean(dt.Rows[0][0]);
                        textBox2.Text = dt.Rows[0][1].ToString();
                        led2.Value = Convert.ToBoolean(dt.Rows[0][2]);
                        textBox3.Text = dt.Rows[0][3].ToString();
                        led3.Value = Convert.ToBoolean(dt.Rows[0][4]);
                        textBox4.Text = dt.Rows[0][5].ToString();
                        led4.Value = Convert.ToBoolean(dt.Rows[0][6]);
                        textBox5.Text = dt.Rows[0][7].ToString();
                        led5.Value = Convert.ToBoolean(dt.Rows[0][8]);
                        textBox6.Text = dt.Rows[0][9].ToString();
                        led6.Value = Convert.ToBoolean(dt.Rows[0][10]);
                        textBox7.Text = dt.Rows[0][11].ToString();
                        led7.Value = Convert.ToBoolean(dt.Rows[0][12]);
                        textBox8.Text = dt.Rows[0][13].ToString();
                        led8.Value = Convert.ToBoolean(dt.Rows[0][14]);
                        textBox9.Text = dt.Rows[0][15].ToString();
                        led9.Value = Convert.ToBoolean(dt.Rows[0][16]);
                        textBox10.Text = dt.Rows[0][27].ToString();
                        led10.Value = Convert.ToBoolean(dt.Rows[0][28]);
                        textBox11.Text = dt.Rows[0][29].ToString();
                        led11.Value = Convert.ToBoolean(dt.Rows[0][30]);
                        textBox12.Text = dt.Rows[0][17].ToString();
                        led12.Value = Convert.ToBoolean(dt.Rows[0][18]);
                        textBox13.Text = dt.Rows[0][31].ToString();
                        led13.Value = Convert.ToBoolean(dt.Rows[0][32]);
                        textBox14.Text = dt.Rows[0][33].ToString();
                        led14.Value = Convert.ToBoolean(dt.Rows[0][34]);
                        textBox15.Text = dt.Rows[0][35].ToString();
                        led15.Value = Convert.ToBoolean(dt.Rows[0][36]);
                        textBox16.Text = dt.Rows[0][19].ToString();
                        led16.Value = Convert.ToBoolean(dt.Rows[0][20]);
                        textBox17.Text = dt.Rows[0][21].ToString();
                        led17.Value = Convert.ToBoolean(dt.Rows[0][22]);
                        textBox18.Text = dt.Rows[0][23].ToString();
                        led18.Value = Convert.ToBoolean(dt.Rows[0][24]);
                        textBox19.Text = dt.Rows[0][25].ToString();
                        led19.Value = Convert.ToBoolean(dt.Rows[0][26]);
                        lnkSpot.Text += dt.Rows[0][37].ToString();
                        try // Correct for leakage current addition in pass/fail table
                        {
                            textBox20.Text = dt.Rows[0][39].ToString();
                            led20.Value = Convert.ToBoolean(dt.Rows[0][40]);
                        }
                        catch
                        {
                            textBox20.Text = null;
                            led20.Value = false;
                        }

                        dt = DBHelper.SearchTable("TSCBPowerSettingsRawData", "Test_ID_FK", testID);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        dt.Columns.RemoveAt(0);
                        powerSettingsRawData = dt;
                        graphSetkVMonkV.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_KV_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_KV_CONTROL, (int)PowerSettingsRawData.MON_KV, Operation.subtract));
                        graphSetkVMonkV.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_KV_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.MON_KV_STD));
                        graphSetuAMonuA.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_UA_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_UA_CONTROL, (int)PowerSettingsRawData.MON_UA, Operation.subtract));
                        graphSetuAMonuA.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_UA_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.MON_UA_STD));
                        graphPSVoltage.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_POWER_VOLTS), GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_POWER_VOLTS, (int)PowerSettingsRawData.SET_POWER_VOLTS, Operation.subtract));
                        graphPSVoltage.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_POWER_VOLTS), GetData(powerSettingsRawData, (int)PowerSettingsRawData.SET_POWER_VOLTS_STD));
                        graphPhotodiodekV.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_KV_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.PHOTODIODE_1));
                        graphPhotodiodekV.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_KV_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.PHOTODIODE_1_STD));
                        graphPhotodiodeuA.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_UA_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.PHOTODIODE_1));
                        graphPhotodiodeuA.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.INPUT_SET_UA_CONTROL), GetData(powerSettingsRawData, (int)PowerSettingsRawData.PHOTODIODE_1_STD));
                        graphuAkV.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.MON_KV), GetData(powerSettingsRawData, (int)PowerSettingsRawData.MON_UA));
                        graphuAkV.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.MON_KV_STD), GetData(powerSettingsRawData, (int)PowerSettingsRawData.MON_UA_STD));
                        graphDeltauADeltakV.Plots[0].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.SET_KV_CONTROL, (int)PowerSettingsRawData.MON_KV, Operation.subtract), GetData(powerSettingsRawData, (int)PowerSettingsRawData.SET_UA_CONTROL, (int)PowerSettingsRawData.MON_UA, Operation.subtract));
                        graphDeltauADeltakV.Plots[1].PlotXY(GetData(powerSettingsRawData, (int)PowerSettingsRawData.SET_KV_CONTROL_STD, (int)PowerSettingsRawData.MON_KV_STD, Operation.subtract), GetData(powerSettingsRawData, (int)PowerSettingsRawData.SET_UA_CONTROL_STD, (int)PowerSettingsRawData.MON_UA_STD, Operation.subtract));

                        for (int i = 0; i < numOfSettings.Length; i++)
                        {
                            dt = DBHelper.SearchTable("TSCBSpectrumData", "TestID_FK_CPK", testID);
                            dt.Columns.RemoveAt(0);
                            dt.Columns.RemoveAt(2);
                            graphSpectrum.Plots[i].PlotXY(GetSpectrumData(dt).Item1, GetSpectrumData(dt).Item2);

                            dt = DBHelper.Query($"select * from TSCBSpectrumSummaryRawData where Test_ID_FK = { testID } and Test_Name = '{ numOfSettings[i] }'");
                            dt.Columns.RemoveAt(0);
                            dt.Columns.RemoveAt(0);
                            dt.Columns.RemoveAt(0);
                            dt.Columns.RemoveAt(0);
                            graphkeVEdge.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.EDGE_12KEV));
                            graphTotalCounts.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.ROI_TOTAL_2_60KEV));
                            graphPhotodiode.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.PHOTO_DIODE_FLUX));
                            graphInputVoltage.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.INPUT_POWER_VOLTS));
                            graphInputCurrent.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.INPUT_POWER_AMPS));
                            graphSetkV.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.KV_CONTROL));
                            graphMonkV.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.KV_MONITOR));
                            graphSetuA.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.UA_CONTROL));
                            graphMonuA.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.UA_MONITOR));
                            graphTFR.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.FILAMENT_READY));
                            graphAnalogEnable.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.KV_ENABLE_ANALOG));
                            graphDigitalEnable.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.KV_ENABLE_DIGITAL));
                            graphInterlockFeedback.Plots[i].PlotXY(GetData(dt, (int)SpectrumSummaryRawData.ELAPSED_TIME_SEC), GetData(dt, (int)SpectrumSummaryRawData.INTERLOCK_FEEDBACK));

                            graphInputVoltage.Plots[i].Visible
                                = graphInputCurrent.Plots[i].Visible
                                = graphSpectrum.Plots[i].Visible
                                = graphkeVEdge.Plots[i].Visible
                                = graphPhotodiode.Plots[i].Visible
                                = graphTotalCounts.Plots[i].Visible
                                = graphSetkV.Plots[i].Visible
                                = graphMonkV.Plots[i].Visible
                                = graphSetuA.Plots[i].Visible
                                = graphSetuA.Plots[i].Visible
                                = graphMonuA.Plots[i].Visible
                                = graphTFR.Plots[i].Visible
                                = graphAnalogEnable.Plots[i].Visible
                                = graphDigitalEnable.Plots[i].Visible
                                = graphInterlockFeedback.Plots[i].Visible
                                = false;
                        }
                        #endregion
                    }
                    if (numOfSettings != null)
                    {
                        ToolStripItem[] tsi = new ToolStripItem[numOfSettings.Length];
                        for (int i = 0; i < tsi.Length; i++)
                        {
                            tsi[i] = new ToolStripMenuItem(numOfSettings[i]);
                        }
                        plotsToolStripMenuItem.DropDownItems.AddRange(tsi);
                        foreach (ToolStripMenuItem t in tsi)
                        {
                            t.CheckOnClick = true;
                            t.CheckedChanged += new EventHandler(plotToolStripMenuItem_Click);
                        }
                        plotsToolStripMenuItem.PerformClick();
                        plotsToolStripMenuItem.HideDropDown();
                    }
                    meanToolStripMenuItem.PerformClick();
                }
                catch
                {
                    led1.Value = false;
                    led2.Value = false;
                    led3.Value = false;
                    led4.Value = false;
                    led5.Value = false;
                    led6.Value = false;
                    led7.Value = false;
                    led8.Value = false;
                    led9.Value = false;
                    led10.Value = false;
                    led11.Value = false;
                    led12.Value = false;
                    led13.Value = false;
                    led14.Value = false;
                    led15.Value = false;
                    led16.Value = false;
                    led17.Value = false;
                    led18.Value = false;
                    led19.Value = false;
                    led20.Value = false;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox4.Text = null;
                    textBox5.Text = null;
                    textBox6.Text = null;
                    textBox7.Text = null;
                    textBox8.Text = null;
                    textBox9.Text = null;
                    textBox10.Text = null;
                    textBox11.Text = null;
                    textBox12.Text = null;
                    textBox13.Text = null;
                    textBox14.Text = null;
                    textBox15.Text = null;
                    textBox16.Text = null;
                    textBox17.Text = null;
                    textBox18.Text = null;
                    textBox19.Text = null;
                    textBox20.Text = null;
                    MessageBox.Show("The selected test is either incomplete or is missing data.");
                }
                viewFluxStabRep.DataSource = directDetectionFluxStab;
                viewImageQuality.DataSource = imageQuality;
                viewPSStab.DataSource = powerSupplyStability;
                viewSpecs.DataSource = specs;
                viewRingoCalValidation.DataSource = sourceCalValidation;
                viewPreCal.DataSource = preCalData;
                viewCal.DataSource = sourceCalibration;
                tabControl2.TabPages.Add(tabHelp);
            }
        }

        private void NewPSEntry()
        {
            DataTable dt = DBHelper.SearchTable("TSCBPowerSupplyList", "TUBNumber", txtTUB.Text);
            if (dt != null) MessageBox.Show($"{ txtTUB.Text } already exists. Enter a unique TUB number.");
            else
            {
                DataGridViewRow rowCopy = viewTSCBPowerSupplyList.SelectedRows[0];
                DialogResult result = MessageBox.Show($"Select { rowCopy.Cells[1].FormattedValue.ToString() } as power supply for { txtTUB.Text }?", "Confirm New Recipe", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    bool noSpecs = true;
                    foreach (DataGridViewRow r in viewTSCBSpecs.Rows)
                    {
                        if (r.Cells[0].FormattedValue.ToString().Equals(rowCopy.Cells[10].FormattedValue.ToString()))
                        {
                            viewTSCBSpecs.Rows[r.Index].Selected = true;
                            noSpecs = false;
                            break;
                        }
                    }

                    DataRow dr = psList.Tables[0].NewRow();
                    for (int i = 0; i < viewTSCBPowerSupplyList.Columns.Count; i++)
                    {
                        dr[i] = rowCopy.Cells[i].FormattedValue;
                        dr[10] = txtTUB.Text;
                    }
                    psList.Tables[0].Rows.Add(dr);
                    int newRow = viewTSCBPowerSupplyList.Rows.Count - 2;
                    viewTSCBPowerSupplyList.Rows[newRow].Selected = true;
                    viewTSCBPowerSupplyList.FirstDisplayedScrollingRowIndex = newRow;

                    dr = specList.Tables[0].NewRow();
                    for (int i = 0; i < viewTSCBSpecs.Columns.Count; i++)
                    {
                        dr[i] = viewTSCBSpecs.SelectedRows[0].Cells[i].FormattedValue;
                        dr[0] = txtTUB.Text;
                    }
                    specList.Tables[0].Rows.Add(dr);
                    newRow = viewTSCBSpecs.Rows.Count - 2;
                    viewTSCBSpecs.Rows[newRow].Selected = true;
                    viewTSCBSpecs.FirstDisplayedScrollingRowIndex = newRow;
                    viewTSCBSpecs.EndEdit();

                    string msg;
                    if (noSpecs) msg = "No specs for selected power supply in existing table. You will need to modify specs manually.";
                    else msg = $"{ txtTUB.Text } is now ready for production.";
                    MessageBox.Show(msg);
                    UpdateDB();
                }
            }
        }

        void ClearGraphs(params ScatterGraph[] graphs)
        {
            foreach (ScatterGraph s in graphs)
            {
                s.ClearData();
            }
        }

        void UpdateDB()
        {
            try
            {
                sqlTSCBPowerSupplyList.Update(psList);
                sqlTSCBSpecs.Update(specList);
            }
            catch
            {
                MessageBox.Show("Database save error. Check if data is duplicate entry.");
            }
        }
    }
}