using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnInStns
{
    static class SAF
    {
        public const string inPLC_ERRACK = "GVL_Safety.inPLC_ERRACK";
        public const string inPLC_RESTART = "GVL_Safety.inPLC_RESTART";
        public const string inPLC_CHB_UNLCK = "GVL_Safety.inPLC_CHB_UNLCK";
        public const string inPLC_HV_EN = "GVL_Safety.inPLC_HV_EN";
        public const string inPLC_SP_EN = "GVL_Safety.inPLC_SP_EN";
        public const string inPLC_TESTING = "GVL_Safety.inPLC_TESTING";
        public const string outPLC_RUN = "GVL_Safety.outPLC_RUN";
        public const string outPLC_ERRCOM = "GVL_Safety.outPLC_ERRCOM";

        public const string SFI_CHB_UNLCK = "GVL_Safety.SFI_CHB_UNLCK";
        public const string SFI_CHB_OPEN = "GVL_Safety.SFI_CHB_OPEN";
        public const string SFI_HV_PWR = "GVL_Safety.SFI_HV_PWR";
        public const string SFI_EMO = "GVL_Safety.SFI_EMO";
        public const string SFI_XRAYS_1 = "GVL_Safety.SFI_XRAYS_1";
        public const string SFI_XRAYS_2 = "GVL_Safety.SFI_XRAYS_2";

        public const string SFO_CHB_UNLCK = "GVL_Safety.SFO_CHB_UNLCK";
        public const string SFO_HV_PWR = "GVL_Safety.SFO_HV_PWR";
        public const string SFO_SP_INTLCK = "GVL_Safety.SFO_SP_INTLCK";
        public const string SFO_BUZZER = "GVL_Safety.SFO_BUZZER";
        public const string SFO_LED_SAFE = "GVL_Safety.SFO_LED_SAFE";
        public const string SFO_LED_HV = "GVL_Safety.SFO_LED_HV";
        public const string SFO_LED_XRAYS = "GVL_Safety.SFO_LED_XRAYS";
        public const string SFO_LED_TESTING = "GVL_Safety.SFO_LED_TESTING";
    }

    static class PLC
    {
        public const string PLO_PS_PWR = "GVL_IO.PLO_PS_PWR";
        public const string PLO_LED_ERROR = "GVL_IO.PLO_LED_ERROR";
        public const string PLO_LED_EMO = "GVL_IO.PLO_LED_EMO";
        public const string PLO_SPELLMAN1 = "GVL_IO.PLO_SPELLMAN1";
        public const string PLO_SPELLMAN2 = "GVL_IO.PLO_SPELLMAN2";
        public const string PLO_SPELLMAN3 = "GVL_IO.PLO_SPELLMAN3";
        public const string PLO_SPELLMAN4 = "GVL_IO.PLO_SPELLMAN4";
        public const string PLO_SPELLMAN5 = "GVL_IO.PLO_SPELLMAN5";
        public const string PLO_SPELLMAN6 = "GVL_IO.PLO_SPELLMAN6";
        public const string PLO_FAN_CHB = "GVL_IO.PLO_FAN_CHB";
        public const string PLI_TEMP_CAB = "GVL_IO.PLI_TEMP_CAB";
        public const string PLI_TEMP_CHB = "GVL_IO.PLI_TEMP_CHB";

        public const string PLI_UPS_ALARM = "GVL_IO.PLI_UPS_ALARM";
        public const string PLI_UPS_BATTERY = "GVL_IO.PLI_UPS_BATTERY";
        public const string PLI_UPS_CHARGE = "GVL_IO.PLI_UPS_CHARGE";
        public const string PLI_PWR_TEMP = "GVL_IO.PLI_PWR_TEMP";
        public const string PLI_PWR_OUTPUT = "GVL_IO.PLI_PWR_OUTPUT";
        public const string PLI_PWR_OVP = "GVL_IO.PLI_PWR_OVP";
    }
}
