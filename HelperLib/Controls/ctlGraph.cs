using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.UI;
using System.IO;
using System.Drawing.Imaging;
using Helper.Properties;

namespace Helper
{
    public partial class ctlGraph : UserControl
    {
        private Log log = new Log();
        public enum Mode { Scatter, Line, Fill, Scope, Strip, Intensity }
        Mode _mode;

        #region Properties


        public Mode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                plotsToolStripMenuItem.Enabled
                = showGridToolStripMenuItem.Enabled
                = autoScaleXToolStripMenuItem.Enabled
                = autoScaleYToolStripMenuItem.Enabled = mode != Mode.Intensity;
                if (intensityGraph.Visible)
                {
                    tableLayoutPanel1.Show();
                    intensityGraph.Hide();
                }
                switch (_mode)
                {
                    case Mode.Strip:
                        xAxis1.Mode = AxisMode.StripChart;
                        xAxis1.MajorDivisions.LabelFormat = new FormatString(FormatStringMode.DateTime, "HH:mm:ss");
                        autoScaleXToolStripMenuItem.Checked = true;
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.VerticalCrosshairMode = CursorCrosshairMode.None;
                        foreach (ScatterPlot p in xyGraph.Plots)
                        {
                            p.FillMode = PlotFillMode.None;
                            p.LineStyle = LineStyle.Solid;
                        }
                        break;
                    case Mode.Scope:
                        xAxis1.Mode = AxisMode.ScopeChart;
                        xAxis1.MajorDivisions.LabelFormat = new FormatString(FormatStringMode.DateTime, "HH:mm:ss");
                        autoScaleXToolStripMenuItem.Checked = true;
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.VerticalCrosshairMode = CursorCrosshairMode.None;
                        foreach (ScatterPlot p in xyGraph.Plots)
                        {
                            p.FillMode = PlotFillMode.None;
                            p.LineStyle = LineStyle.Solid;
                        }
                        break;
                    case Mode.Scatter:
                        xAxis1.MajorDivisions.LabelFormat = new FormatString(FormatStringMode.Numeric, "G5");
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.VerticalCrosshairMode = CursorCrosshairMode.FullLength;
                        foreach (ScatterPlot p in xyGraph.Plots)
                        {
                            p.FillMode = PlotFillMode.None;
                            p.LineStyle = LineStyle.None;
                        }
                        break;
                    case Mode.Line:
                        xAxis1.MajorDivisions.LabelFormat = new FormatString(FormatStringMode.Numeric, "G5");
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.VerticalCrosshairMode = CursorCrosshairMode.FullLength;
                        foreach (ScatterPlot p in xyGraph.Plots)
                        {
                            p.FillMode = PlotFillMode.None;
                            p.LineStyle = LineStyle.Solid;
                        }
                        break;
                    case Mode.Fill:
                        xAxis1.MajorDivisions.LabelFormat = new FormatString(FormatStringMode.Numeric, "G5");
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.VerticalCrosshairMode = CursorCrosshairMode.FullLength;
                        foreach (ScatterPlot p in xyGraph.Plots)
                        {
                            p.FillMode = PlotFillMode.Fill;
                            p.LineStyle = LineStyle.None;
                            p.PointStyle = PointStyle.None;
                        }
                        break;
                    case Mode.Intensity:
                        intensityGraph.Show();
                        tableLayoutPanel1.Hide();
                        break;
                }
                ClearGraph();
            }
        }

        public string Title
        {
            get
            {
                return xyGraph.Caption;
            }
            set
            {
                xyGraph.Caption = intensityGraph.Caption = Name = value;
            }
        }

        public string xAxis
        {
            get
            {
                return xAxis1.Caption;
            }
            set
            {
                xAxis1.Caption = intensityXAxis1.Caption = value;
                autoScaleXToolStripMenuItem.Text = $"Autoscale { xAxis1.Caption }";
            }
        }

        public string yAxis
        {
            get
            {
                return yAxis1.Caption;
            }
            set
            {
                yAxis1.Caption = intensityYAxis1.Caption = value;
                autoScaleYToolStripMenuItem.Text = $"Autoscale { yAxis1.Caption }";
            }
        }

        public string yAxisSecondary
        {
            get
            {
                return yAxis2.Caption;
            }
            set
            {
                yAxis2.Visible = value != "";
                yAxis2.Caption = value;
                autoScaleYToolStripMenuItem.Text += $"/{ yAxis2.Caption }";
            }
        }

        public override Font Font
        {
            get
            {
                return Settings.Default.GraphFont;
            }
            set
            {
                Font f = new Font(value.Name, value.Size, FontStyle.Regular);

                xyGraph.CaptionFont = intensityGraph.CaptionFont
                = xAxis1.MajorDivisions.LabelFont = xAxis1.CaptionFont
                = yAxis1.MajorDivisions.LabelFont = yAxis1.CaptionFont
                = yAxis2.MajorDivisions.LabelFont = yAxis2.CaptionFont
                = intensityXAxis1.MajorDivisions.LabelFont = intensityXAxis1.CaptionFont
                = intensityYAxis1.MajorDivisions.LabelFont = intensityYAxis1.CaptionFont
                = colorScale1.MajorDivisions.LabelFont
                = f;

                if (f.Size < 12) legend.Font = f;

                Settings.Default.GraphFont = f;
            }
        }

        public bool LegendVisible
        {
            get
            {
                return legend.Visible;
            }
            set
            {
                legend.Visible = value;
                tableLayoutPanel1.ColumnStyles[1].Width = value ? 100 : 0;
            }
        }

        #endregion

        public ctlGraph()
        {
            InitializeComponent();
            cbxFontSize.SelectedItem = Font.Size.ToString();
            LegendVisible = false;
            Disposed += OnDispose;
            Settings.Default.PropertyChanged += Default_PropertyChanged;
            xyGraph.PlotAreaColorChanged += Graph_PlotAreaColorChanged;
        }

        #region Methods


        private void OnDispose(object sender, EventArgs e)
        {
            log.Dispose();
        }

        public void SetXRange(double min, double max)
        {
            if (autoScaleXToolStripMenuItem.Checked) autoScaleXToolStripMenuItem.PerformClick();
            xAxis1.Range = new Range(min, max);
        }

        public void SetYRange(double min, double max)
        {
            if (autoScaleYToolStripMenuItem.Checked) autoScaleYToolStripMenuItem.PerformClick();
            yAxis1.Range = new Range(min, max);
        }

        public void Plot(double[,] image, double minIntensity, double maxIntensity)
        {
            intensityGraph.ColorScales[0].Range = new Range(minIntensity, maxIntensity);
            intensityGraph.Plot(image);
        }

        public void Plot(string name, double x, double y, bool secondary = false)
        {
            foreach (ScatterPlot p in xyGraph.Plots)
            {
                if ((string)p.Tag == name)
                {
                    if (secondary) p.YAxis = yAxis2;
                    if (mode == Mode.Scope || mode == Mode.Strip)
                    {
                        p.PlotXYAppend(DateTime.Now.TimeOfDay.TotalSeconds, y);
                    }
                    else
                    {
                        p.PlotXYAppend(x, y);
                    }
                }
            }
        }

        public void Plot(string name, double[] x, double[] y, bool secondary = false)
        {
            foreach (ScatterPlot p in xyGraph.Plots)
            {
                if ((string)p.Tag == name)
                {
                    if (x == null || x.Length < y.Length)
                    {
                        double[] cnt = new double[y.Length];
                        for (int i = 0; i < y.Length; i++)
                        {
                            cnt[i] = i;
                        }
                        p.PlotXY(cnt, y);
                    }
                    else p.PlotXY(x, y);
                }
            }
        }

        public void SaveGraph()
        {
            saveFileDialog1.Filter = "PNG files (*.png)|*.png";
            saveFileDialog1.FileName = null;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                using (Bitmap bmp = new Bitmap(this.Width, this.Height))
                {
                    try
                    {
                        this.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
                        FileInfo fi = new FileInfo(path);
                        FileStream st = fi.Create();
                        bmp.Save(st, ImageFormat.Png);
                        st.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Couldn't save graph.");
                    }
                }
            }
        }

        public void SaveData()
        {
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog1.FileName = null;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                log.saveLocation = saveFileDialog1.FileName;

                if (mode == Mode.Intensity)
                {
                    log.Clear();
                    log.AddHeaders(intensityXAxis1.Caption, intensityYAxis1.Caption, "Intensity");
                    double[] x = intensityPlot1.GetXData();
                    double[] y = intensityPlot1.GetYData();
                    double[,] z = intensityPlot1.GetZData();
                    for (int i = 0; i < x.Length; i++)
                    {
                        for (int j = 0; j < y.Length; j++)
                        {
                            log.Write(x[i], y[j], z[i, j]);
                        }
                    }
                }
                else
                {
                    int numPlots = xyGraph.Plots.Count;
                    double[] x = scatterPlot1.GetXData();
                    double[][] y = new double[numPlots][];

                    string[] header = new string[numPlots + 1];
                    header[0] = scatterPlot1.XAxis.Caption;
                    for (int i = 0; i < numPlots; i++)
                    {
                        header[i + 1] = xyGraph.Plots[i].Tag.ToString();
                        y[i] = xyGraph.Plots[i].GetYData();
                    }
                    log.Clear();
                    log.AddHeaders(header);

                    for (int i = 0; i < x.Length; i++)
                    {
                        double[] row = new double[numPlots + 1];
                        row[0] = x[i];
                        for (int j = 0; j < numPlots; j++)
                        {
                            row[j + 1] = y[j][i];
                        }
                        log.Write(row);
                    }
                }

                log.SaveToFile(log.saveLocation);
            }
        }

        public void ClearGraph()
        {
            intensityGraph.ClearData();
            foreach (ScatterPlot p in xyGraph.Plots)
            {
                p.ClearData();
            }
            xyGraph.ResetZoomPan();
        }

        public void ClearPlot(string name)
        {
            foreach (ScatterPlot p in xyGraph.Plots)
            {
                if ((string)p.Tag == name)
                {
                    p.ClearData();
                }
            }
        }

        public void AddPlots(params string[] names)
        {
            ToolStripMenuItem[] t = new ToolStripMenuItem[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                t[i] = new ToolStripMenuItem();
                t[i].Text = names[i];
                t[i].Checked = true;
                t[i].CheckOnClick = true;
                t[i].Click += plotsDropDownItem_Clicked;
                xyGraph.Plots[i].Visible = legend.Items[i].Visible = true;
                xyGraph.Plots[i].Tag = legend.Items[i].Text = names[i];
            }
            plotsToolStripMenuItem.DropDownItems.Clear();
            plotsToolStripMenuItem.DropDown.Items.AddRange(t);

            t = new ToolStripMenuItem[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                t[i] = new ToolStripMenuItem();
                t[i].Text = names[i];
                t[i].Click += clearDropDownItem_Click;
            }
            clearToolStripMenuItem.DropDownItems.Clear();
            clearToolStripMenuItem.DropDown.Items.AddRange(t);

            int numPlots = xyGraph.Plots.Count;
            for (int i = numPlots - 1; i >= 0; i--)
            {
                if (!xyGraph.Plots[i].Visible) xyGraph.Plots.RemoveAt(i);
            }
            Enabled = xyGraph.Plots.Count > 0;
            LegendVisible = xyGraph.Plots.Count > 1;
        }
        #endregion

        #region Events


        private void intensityGraph_ColorScaleRangeChanged(object sender, ColorScaleEventArgs e)
        {
            double min = colorScale1.Range.Minimum;
            double max = colorScale1.Range.Maximum;
            if (max > 0) colorScale1.MajorDivisions.Interval = max;
            colorScale1.ColorMap.Clear();
            colorScale1.ColorMap.Add((min + max) / 2, Color.Blue);
        }

        private void ctlGraph_BackColorChanged(object sender, EventArgs e)
        {
            xyGraph.PlotAreaColor = xyGraph.BackColor = legend.BackColor = BackColor;
        }

        private void ctlGraph_ForeColorChanged(object sender, EventArgs e)
        {
            xyGraph.CaptionForeColor = intensityGraph.CaptionForeColor
                = legend.ForeColor
                = xAxis1.MajorDivisions.LabelForeColor = xAxis1.CaptionForeColor = xAxis1.MajorDivisions.TickColor
                = yAxis1.MajorDivisions.LabelForeColor = yAxis1.CaptionForeColor = yAxis1.MajorDivisions.TickColor
                = yAxis2.MajorDivisions.LabelForeColor = yAxis2.CaptionForeColor = yAxis2.MajorDivisions.TickColor
                = colorScale1.MajorDivisions.LabelForeColor
                = intensityXAxis1.MajorDivisions.LabelForeColor = intensityXAxis1.CaptionForeColor = intensityXAxis1.MajorDivisions.TickColor
                = intensityYAxis1.MajorDivisions.LabelForeColor = intensityYAxis1.CaptionForeColor = intensityYAxis1.MajorDivisions.TickColor
                = ForeColor;
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void graph_XAxisRangeChanged(object sender, XAxisEventArgs e)
        {
            if (autoScaleXToolStripMenuItem.Checked && (mode == Mode.Scope || mode == Mode.Strip))
            {
                double shiftX = xAxis1.Range.Maximum - xAxis1.Range.Minimum;
                foreach (XYCursor c in xyGraph.Cursors)
                    if (c.Visible) c.XPosition = c.XPosition + shiftX;
            }
        }

        private void cursor1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double setPosX = xAxis1.Range.Minimum + (xAxis1.Range.Maximum - xAxis1.Range.Minimum) / 2;
            double setPosY = yAxis1.Range.Minimum + (yAxis1.Range.Maximum - yAxis1.Range.Minimum) / 2;

            foreach (XYCursor c in xyGraph.Cursors)
                if (!c.Visible)
                {
                    c.XPosition = setPosX;
                    c.YPosition = setPosY;
                }

            xyCursor1.Visible = intensityCursor1.Visible = cursor1ToolStripMenuItem.Checked;
            xyCursor2.Visible = intensityCursor2.Visible = cursor2ToolStripMenuItem.Checked;
            xyCursor3.Visible = intensityCursor3.Visible = cursor3ToolStripMenuItem.Checked;
            xyCursor4.Visible = intensityCursor4.Visible = cursor4ToolStripMenuItem.Checked;
            cursorsToolStripMenuItem.DropDown.Show();
        }

        private void cursorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripDropDownItem t in cursorsToolStripMenuItem.DropDownItems.OfType<ToolStripDropDownItem>())
            {
                t.PerformClick();
            }
        }

        private void cbxFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font = new Font(xAxis1.MajorDivisions.LabelFont.Name, (float)Convert.ToDouble(cbxFontSize.SelectedItem), FontStyle.Regular);
        }

        private void Graph_PlotAreaColorChanged(object sender, EventArgs e)
        {
            if (mode == Mode.Intensity) intensityPlot1.ColorScale.HighColor = xyGraph.PlotAreaColor;
            cmsGraph.Hide();
        }

        private void plotsDropDownItem_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < plotsToolStripMenuItem.DropDownItems.Count; i++)
            {
                ToolStripMenuItem t = (ToolStripMenuItem)plotsToolStripMenuItem.DropDownItems[i];
                xyGraph.Plots[i].Visible = legend.Items[i].Visible = t.Checked;
            }
            xyGraph.ResetZoomPan();
            plotsToolStripMenuItem.DropDown.Show();
        }

        private void graph_MouseClick(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.None && e.Button == MouseButtons.Right)
            {
                cmsGraph.Show(this, e.Location);
            }
        }

        private void graph_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            xyGraph.ResetZoomPan();
            intensityGraph.ResetZoomPan();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearGraph();
        }

        private void clearDropDownItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = sender as ToolStripMenuItem;
            ClearPlot(t.Text);
        }

        private void annotationsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            xyPointAnnotation1.Visible = annotationsToolStripMenuItem.Checked;
        }

        private void plotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripDropDownItem t in plotsToolStripMenuItem.DropDownItems.OfType<ToolStripDropDownItem>())
            {
                t.PerformClick();
            }
        }

        private void showGridToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            xAxis1.MajorDivisions.GridVisible = yAxis1.MajorDivisions.GridVisible = showGridToolStripMenuItem.Checked;
        }

        private void autoScaleXToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!autoScaleXToolStripMenuItem.Checked) xAxis1.Mode = AxisMode.Fixed;
            else
            {
                switch (mode)
                {
                    case Mode.Scope:
                        xAxis1.Mode = AxisMode.ScopeChart;
                        ClearGraph();
                        double setPosX = xAxis1.Range.Minimum + (xAxis1.Range.Maximum - xAxis1.Range.Minimum) / 2;
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.XPosition = setPosX;
                        break;
                    case Mode.Strip:
                        xAxis1.Mode = AxisMode.StripChart;
                        ClearGraph();
                        setPosX = xAxis1.Range.Minimum + (xAxis1.Range.Maximum - xAxis1.Range.Minimum) / 2;
                        foreach (XYCursor c in xyGraph.Cursors)
                            c.XPosition = setPosX;
                        break;
                    default:
                        xAxis1.Mode = AxisMode.AutoScaleLoose;
                        break;
                }
            }
        }

        private void autoScaleYToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            yAxis1.Mode = yAxis2.Mode = autoScaleYToolStripMenuItem.Checked ? AxisMode.AutoScaleLoose : AxisMode.Fixed;
        }

        private void asImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGraph();
        }

        private void asDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        #endregion
    }
}