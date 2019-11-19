using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BurnInStns
{
    
    public partial class frmMaintenance : Form
    {
        MX_ADSClient vPLC = new MX_ADSClient("5.45.76.0.1.1", 851);
        List<MX_PLCDevice> vDevices = new List<MX_PLCDevice>();

        public frmMaintenance()
        {
            InitializeComponent();
            vPLC.OnDeviceChanged += new MX_ADSClient.DeviceChangedDelegate(PLC_OnDeviceChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            vPLC.UpdateTime = 100;
            vPLC.Connect();
            CreateDeviceList();
            tmrInitializeSwitches.Interval = 250;
            tmrInitializeSwitches.Start();
        }

        private void InitializeSwitches()
        {
            swPSPower.Switched = vPLC.GetState(PLC.PLO_PS_PWR);
            swHVPower.Switched = vPLC.GetState(SAF.inPLC_HV_EN);
            swSpellmanHV.Switched = vPLC.GetState(SAF.inPLC_SP_EN);
            swTesting.Switched = vPLC.GetState(SAF.inPLC_TESTING);
            swChamberFan.Switched = vPLC.GetState(PLC.PLO_FAN_CHB);
            swError.Switched = vPLC.GetState(PLC.PLO_LED_ERROR);

            swSpellman1.Switched = vPLC.GetState(PLC.PLO_SPELLMAN1);
            swSpellman2.Switched = vPLC.GetState(PLC.PLO_SPELLMAN2);
            swSpellman3.Switched = vPLC.GetState(PLC.PLO_SPELLMAN3);
            swSpellman4.Switched = vPLC.GetState(PLC.PLO_SPELLMAN4);
            swSpellman5.Switched = vPLC.GetState(PLC.PLO_SPELLMAN5);
            swSpellman6.Switched = vPLC.GetState(PLC.PLO_SPELLMAN6);
        }

        private void PLC_OnDeviceChanged(DeviceChangedArgs e)
        {
            switch (e.Device.Name)
            {
                case SAF.inPLC_ERRACK:
                    ledPLC_ERRACK.Energized = e.Device.Value;
                    if (e.Device.Value) vPLC.SetState(SAF.inPLC_ERRACK, false);
                    break;
                case SAF.inPLC_RESTART:
                    ledPLC_RESTART.Energized = e.Device.Value;
                    if (e.Device.Value) vPLC.SetState(SAF.inPLC_RESTART, false);
                    break;
                case SAF.inPLC_CHB_UNLCK:
                    ledPLC_CHB_UNLCK.Energized = e.Device.Value;
                    break;
                case SAF.inPLC_HV_EN:
                    ledPLC_HV_EN.Energized = e.Device.Value;
                    swHVPower.Energized = e.Device.Value;
                    break;
                case SAF.inPLC_SP_EN:
                    ledPLC_SP_EN.Energized = e.Device.Value;
                    swSpellmanHV.Energized = e.Device.Value;
                    break;
                case SAF.inPLC_TESTING:
                    ledPLC_TESTING.Energized = e.Device.Value;
                    swTesting.Energized = e.Device.Value;
                    break;
                case SAF.outPLC_RUN:
                    ledPLC_RUN.Energized = e.Device.Value;
                    break;
                case SAF.outPLC_ERRCOM:
                    ledPLC_ERRCOM.Energized = e.Device.Value;
                    break;
                case SAF.SFI_CHB_UNLCK:
                    ledSFI_CHB_UNLCK.Energized = e.Device.Value;
                    ledCHBLock.Energized = e.Device.Value;
                    if (e.Device.Value) this.Invoke((MethodInvoker)delegate { btnCHBLock.Text = "Lock Chamber"; });
                    else this.Invoke((MethodInvoker)delegate { btnCHBLock.Text = "Unlock Chamber"; });
                    break;
                case SAF.SFI_CHB_OPEN:
                    ledSFI_CHB_OPEN.Energized = e.Device.Value;
                    ledCHBDoor.Energized = e.Device.Value;
                    break;
                case SAF.SFI_HV_PWR:
                    ledSFI_HV_PWR.Energized = e.Device.Value;
                    break;
                case SAF.SFI_EMO:
                    ledSFI_EMO.Energized = !e.Device.Value;
                    break;
                case SAF.SFI_XRAYS_1:
                    ledSFI_XRAYS_1.Energized = e.Device.Value;
                    break;
                case SAF.SFI_XRAYS_2:
                    ledSFI_XRAYS_2.Energized = e.Device.Value;
                    break;
                case SAF.SFO_CHB_UNLCK:
                    ledSFO_CHB_UNLCK.Energized = e.Device.Value;
                    ledCHBLock.Energized = e.Device.Value;
                    break;
                case SAF.SFO_HV_PWR:
                    ledSFO_HV_PWR.Energized = e.Device.Value;
                    break;
                case SAF.SFO_SP_INTLCK:
                    ledSFO_SP_INTLCK.Energized = e.Device.Value;
                    break;
                case SAF.SFO_BUZZER:
                    ledSFO_BUZZER.Energized = e.Device.Value;
                    break;
                case SAF.SFO_LED_SAFE:
                    ledSFO_LED_SAFE.Energized = e.Device.Value;
                    ledSafe.Energized = e.Device.Value;
                    this.Invoke((MethodInvoker)delegate { btnCHBLock.Enabled = e.Device.Value; });
                    break;
                case SAF.SFO_LED_HV:
                    ledSFO_LED_HV.Energized = e.Device.Value;
                    ledHV.Energized = e.Device.Value;
                    break;
                case SAF.SFO_LED_XRAYS:
                    ledSFO_LED_XRAYS.Energized = e.Device.Value;
                    ledXRays.Energized = e.Device.Value;
                    break;
                case SAF.SFO_LED_TESTING:
                    ledSFO_LED_TESTING.Energized = e.Device.Value;
                    ledTesting.Energized = e.Device.Value;
                    break;
                case PLC.PLO_PS_PWR:
                    ledPLO_PS_PWR.Energized = e.Device.Value;
                    swPSPower.Energized = e.Device.Value;
                    break;
                case PLC.PLO_LED_ERROR:
                    ledPLO_LED_ERROR.Energized = e.Device.Value;
                    ledError.Energized = e.Device.Value;
                    swError.Energized = e.Device.Value;
                    break;
                case PLC.PLO_LED_EMO:
                    ledPLO_LED_EMO.Energized = e.Device.Value;
                    break;
                case PLC.PLO_SPELLMAN1:
                    ledPLO_SPELLMAN1.Energized = e.Device.Value;
                    swSpellman1.Energized = e.Device.Value;
                    break;
                case PLC.PLO_SPELLMAN2:
                    ledPLO_SPELLMAN2.Energized = e.Device.Value;
                    swSpellman2.Energized = e.Device.Value;
                    break;
                case PLC.PLO_SPELLMAN3:
                    ledPLO_SPELLMAN3.Energized = e.Device.Value;
                    swSpellman3.Energized = e.Device.Value;
                    break;
                case PLC.PLO_SPELLMAN4:
                    ledPLO_SPELLMAN4.Energized = e.Device.Value;
                    swSpellman4.Energized = e.Device.Value;
                    break;
                case PLC.PLO_SPELLMAN5:
                    ledPLO_SPELLMAN5.Energized = e.Device.Value;
                    swSpellman5.Energized = e.Device.Value;
                    break;
                case PLC.PLO_SPELLMAN6:
                    ledPLO_SPELLMAN6.Energized = e.Device.Value;
                    swSpellman6.Energized = e.Device.Value;
                    break;
                case PLC.PLO_FAN_CHB:
                    ledPLO_FAN_CHB.Energized = e.Device.Value;
                    swChamberFan.Energized = e.Device.Value;
                    break;
                case PLC.PLI_TEMP_CAB:
                    string vTempCAB = e.Device.Value.ToString();
                    vTempCAB = vTempCAB.Insert(vTempCAB.Length - 1, ".") + "°C";
                    this.Invoke((MethodInvoker)delegate { lblTempCAB.Text = vTempCAB; });
                    break;
                case PLC.PLI_TEMP_CHB:
                    string vTempCHB = e.Device.Value.ToString();
                    vTempCHB = vTempCHB.Insert(vTempCHB.Length - 1, ".") + "°C";
                    this.Invoke((MethodInvoker)delegate { lblTempCHB.Text = vTempCHB; });
                    break;
                case PLC.PLI_UPS_ALARM:
                    ledPLI_UPS_ALARM.Energized = !e.Device.Value;
                    break;
                case PLC.PLI_UPS_BATTERY:
                    ledPLI_UPS_BATTERY.Energized = e.Device.Value;
                    break;
                case PLC.PLI_UPS_CHARGE:
                    ledPLI_UPS_CHARGE.Energized = e.Device.Value;
                    break;
                case PLC.PLI_PWR_TEMP:
                    ledPLI_PWR_TEMP.Energized = e.Device.Value;
                    break;
                case PLC.PLI_PWR_OUTPUT:
                    ledPLI_PWR_OUTPUT.Energized = e.Device.Value;
                    break;
                case PLC.PLI_PWR_OVP:
                    ledPLI_PWR_OVP.Energized = e.Device.Value;
                    break;
            }
        }

        

        private void CreateDeviceList()
        {                
            vPLC.AddDevice(SAF.inPLC_ERRACK, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.inPLC_RESTART, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.inPLC_CHB_UNLCK, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.inPLC_HV_EN, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.inPLC_SP_EN, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.inPLC_TESTING, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.outPLC_RUN, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.outPLC_ERRCOM, MX_PLCDataTypes.BOOL);

            vPLC.AddDevice(SAF.SFI_CHB_UNLCK, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFI_CHB_OPEN, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFI_HV_PWR, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFI_EMO, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFI_XRAYS_1, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFI_XRAYS_2, MX_PLCDataTypes.BOOL);
         
            vPLC.AddDevice(SAF.SFO_CHB_UNLCK, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_HV_PWR, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_SP_INTLCK, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_BUZZER, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_LED_SAFE, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_LED_HV, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_LED_XRAYS, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(SAF.SFO_LED_TESTING, MX_PLCDataTypes.BOOL);
           
            vPLC.AddDevice(PLC.PLO_PS_PWR, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_LED_ERROR, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_LED_EMO, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_SPELLMAN1, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_SPELLMAN2, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_SPELLMAN3, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_SPELLMAN4, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_SPELLMAN5, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_SPELLMAN6, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLO_FAN_CHB, MX_PLCDataTypes.BOOL);
                       
            vPLC.AddDevice(PLC.PLI_TEMP_CAB, MX_PLCDataTypes.INT);
            vPLC.AddDevice(PLC.PLI_TEMP_CHB, MX_PLCDataTypes.INT);

            vPLC.AddDevice(PLC.PLI_UPS_ALARM, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLI_UPS_BATTERY, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLI_UPS_CHARGE, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLI_PWR_TEMP, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLI_PWR_OUTPUT, MX_PLCDataTypes.BOOL);
            vPLC.AddDevice(PLC.PLI_PWR_OVP, MX_PLCDataTypes.BOOL);
        }

        private void btnErrAck_Click(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_ERRACK, !vPLC.GetState(SAF.inPLC_ERRACK));
        }

        private void btnSafetyReset_Click(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_RESTART, !vPLC.GetState(SAF.inPLC_RESTART));
        }

        private void btnCHBLock_Click(object sender, EventArgs e)
        {
            bool vState = vPLC.GetState(SAF.inPLC_CHB_UNLCK);
            vPLC.SetState(SAF.inPLC_CHB_UNLCK, !vState);
        }

        private void swPSPower_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_PS_PWR, true);
        }

        private void swPSPower_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_PS_PWR, false);
        }

        private void swHVPower_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_HV_EN, true);
        }

        private void swHVPower_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_HV_EN, false);
        }

        private void swSpellmanHV_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_SP_EN, true);
        }

        private void swSpellmanHV_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_SP_EN, false);
        }

        private void swChamberFan_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_FAN_CHB, true);
        }

        private void swChamberFan_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_FAN_CHB, false);
        }

        private void swSpellman1_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN1, true);
        }

        private void swSpellman1_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN1, false);
        }

        private void swSpellman2_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN2, true);
        }

        private void swSpellman2_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN2, false);
        }

        private void swSpellman3_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN3, true);
        }

        private void swSpellman3_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN3, false);
        }

        private void swSpellman4_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN4, true);
        }

        private void swSpellman4_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN4, false);
        }

        private void swSpellman5_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN5, true);
        }

        private void swSpellman5_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN5, false);
        }

        private void swSpellman6_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN6, true);
        }

        private void swSpellman6_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_SPELLMAN6, false);
        }

        private void swTesting_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_TESTING, true);
        }

        private void swTesting_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(SAF.inPLC_TESTING, false);
        }

        private void swError_ClickON(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_LED_ERROR, true);
        }

        private void swError_ClickOFF(object sender, EventArgs e)
        {
            vPLC.SetState(PLC.PLO_LED_ERROR, false);
        }

        private void tmrInitializeSwitches_Tick(object sender, EventArgs e)
        {
            tmrInitializeSwitches.Stop();
            InitializeSwitches();
        }

        private void frmMaintenance_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void chkManualControl_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
