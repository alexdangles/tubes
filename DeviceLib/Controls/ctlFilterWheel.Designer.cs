namespace Devices.Controls
{
    partial class ctlFilterWheel
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
            this.knob1 = new NationalInstruments.UI.WindowsForms.Knob();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.knob1)).BeginInit();
            this.SuspendLayout();
            // 
            // knob1
            // 
            this.knob1.CoercionMode = NationalInstruments.UI.NumericCoercionMode.ToInterval;
            this.knob1.DialColor = System.Drawing.Color.Black;
            this.knob1.InteractionMode = NationalInstruments.UI.RadialNumericPointerInteractionModes.SnapPointer;
            this.knob1.KnobStyle = NationalInstruments.UI.KnobStyle.RaisedWithThumb;
            this.knob1.Location = new System.Drawing.Point(0, 0);
            this.knob1.MajorDivisions.Interval = 1D;
            this.knob1.MajorDivisions.LabelForeColor = System.Drawing.Color.Black;
            this.knob1.MajorDivisions.TickColor = System.Drawing.Color.Black;
            this.knob1.MinorDivisions.TickLength = 1F;
            this.knob1.MinorDivisions.TickVisible = false;
            this.knob1.Name = "knob1";
            this.knob1.OutOfRangeMode = NationalInstruments.UI.NumericOutOfRangeMode.CoerceToRange;
            this.knob1.PointerColor = System.Drawing.Color.White;
            this.knob1.Range = new NationalInstruments.UI.Range(1D, 12D);
            this.knob1.ScaleArc = new NationalInstruments.UI.Arc(240F, -330F);
            this.knob1.Size = new System.Drawing.Size(150, 150);
            this.knob1.TabIndex = 1;
            this.knob1.Value = 1D;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // ctlFilterWheel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.knob1);
            this.Enabled = false;
            this.Name = "ctlFilterWheel";
            this.Size = new System.Drawing.Size(153, 153);
            this.Tag = "";
            ((System.ComponentModel.ISupportInitialize)(this.knob1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrUpdate;
        public NationalInstruments.UI.WindowsForms.Knob knob1;
    }
}
