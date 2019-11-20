using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FTD2XX_NET;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;

namespace Devices
{
    #region "Helper Classes"
    public class MXDPP50_Settings
    {
        #region "Constants"
        const double cPPG_Vernier = 0.055744;  // See AD8370 datasheet Gain Codes section
        const double cPPG_PreGain = 7.079458;  // See AD8370 datasheet Gain Codes section
        const double cPPG_MaxLowGain = 7.079488;  // See AD8370 datasheet Gain Codes section
        #endregion

        #region "Variables"
        double gEQSlope = 0.293;
        double gEQOffset = 0.038;

        int gClockSpeed_Int = 0;
        double gClockSpeed = 0;
        int gPeakTimeSL_Int = 0;
        int gPeakTimeF2_Int = 0;
        int gPeakTimeF3_Int = 0;
        double gPeakTimeSL = 0;
        double gPeakTimeF2 = 0;
        double gPeakTimeF3 = 0;
        int gHoldTimeSL_Int = 0;
        int gHoldTimeF2_Int = 0;
        int gHoldTimeF3_Int = 0;
        double gHoldTimeSL = 0;
        double gHoldTimeF2 = 0;
        double gHoldTimeF3 = 0;
        int gDGainSL_Int = 0;
        int gDGainF2_Int = 0;
        int gDGainF3_Int = 0;
        double gDGainSL = 0;
        double gDGainF2 = 0;
        double gDGainF3 = 0;
        int gThreshSL = 0;
        int gThreshF2 = 0;
        int gThreshF3 = 0;
        int gPPTC_Int = 0;
        double gPPTC = 0;
        int gPPGain_Int = 0;
        double gPPGain = 0;
        int gEQFactor = 0;
        int gZeroFactor = 0;
        int gRInhibit_Int = 0;
        double gRInhibit = 0;
        int gBLRMode = 0;
        int gBLRWindow = 0;
        int gDTLength = 0;
        int gRInterval_Int = 0;
        double gRInterval = 0;
        int gChnOffset = 0;

        eMXDPP50_SCAMode gSCAMode = 0;
        int gSCALo1 = 900;
        int gSCALo2 = 900;
        int gSCALo3 = 900;
        int gSCALo4 = 900;
        int gSCALo5 = 900;
        int gSCALo6 = 900;
        int gSCALo7 = 900;
        int gSCALo8 = 900;
        int gSCAHi1 = 1100;
        int gSCAHi2 = 1100;
        int gSCAHi3 = 1100;
        int gSCAHi4 = 1100;
        int gSCAHi5 = 1100;
        int gSCAHi6 = 1100;
        int gSCAHi7 = 1100;
        int gSCAHi8 = 1100;
        #endregion

        #region "Constructor"
        public MXDPP50_Settings(double vEQSlope, double vEQOffset)
        {
            EQSlope = vEQSlope;
            EQOffset = vEQOffset;
        }
        #endregion

        #region "Properties"
        public double EQSlope
        {
            get
            {
                return gEQSlope;
            }

            set
            {
                gEQSlope = value;
                CalcEQ();
            }
        }
        public double EQOffset
        {
            get
            {
                return gEQOffset;
            }

            set
            {
                gEQOffset = value;
                CalcEQ();
            }
        }
        public int ClockSpeed_Int
        {
            get
            {
                return gClockSpeed_Int;
            }

            set
            {
                if (value <= 4) gClockSpeed_Int = 4;
                else if (value <= 8) gClockSpeed_Int = 8;
                else if (value <= 16) gClockSpeed_Int = 16;
                else if (value <= 32) gClockSpeed_Int = 32;
                else if (value >= 64) gClockSpeed_Int = 64;

                gClockSpeed = Convert.ToDouble(100 / gClockSpeed_Int);
            }
        }
        public double ClockSpeed
        {
            get
            {
                return gClockSpeed;
            }

            set
            {
                ClockSpeed_Int = Convert.ToInt32(100 / value);
            }
        }
        public int PeakTimeSL_Int
        {
            get
            {
                return gPeakTimeSL_Int;
            }

            set
            {
                gPeakTimeSL_Int = value;

                if (gPeakTimeSL_Int < 1) gPeakTimeSL_Int = 1;
                else if (gPeakTimeSL_Int > 511) gPeakTimeSL_Int = 511;

                gPeakTimeSL = gPeakTimeSL_Int / gClockSpeed;
            }
        }
        public int PeakTimeF2_Int
        {
            get
            {
                return gPeakTimeF2_Int;
            }

            set
            {
                gPeakTimeF2_Int = value;

                if (gPeakTimeF2_Int < 1) gPeakTimeF2_Int = 1;
                else if (gPeakTimeF2_Int > 127) gPeakTimeF2_Int = 127;

                gPeakTimeF2 = gPeakTimeF2_Int / gClockSpeed;
            }
        }
        public int PeakTimeF3_Int
        {
            get
            {
                return gPeakTimeF3_Int;
            }

            set
            {
                gPeakTimeF3_Int = value;

                if (gPeakTimeF3_Int < 1) gPeakTimeF3_Int = 1;
                else if (gPeakTimeF3_Int > 127) gPeakTimeF3_Int = 127;

                gPeakTimeF3 = gPeakTimeF3_Int / gClockSpeed;
            }
        }
        public double PeakTimeSL
        {
            get
            {
                return gPeakTimeSL;
            }

            set
            {
                PeakTimeSL_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public double PeakTimeF2
        {
            get
            {
                return gPeakTimeF2;
            }

            set
            {
                PeakTimeF2_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public double PeakTimeF3
        {
            get
            {
                return gPeakTimeF3;
            }

            set
            {
                PeakTimeF3_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public int HoldTimeSL_Int
        {
            get
            {
                return gHoldTimeSL_Int;
            }

            set
            {
                gHoldTimeSL_Int = value;

                if (gHoldTimeSL_Int < 1) gHoldTimeSL_Int = 1;
                else if (gHoldTimeSL_Int > 31) gHoldTimeSL_Int = 31;

                gHoldTimeSL = gHoldTimeSL_Int / gClockSpeed;
            }
        }
        public int HoldTimeF2_Int
        {
            get
            {
                return gHoldTimeF2_Int;
            }

            set
            {
                gHoldTimeF2_Int = value;

                if (gHoldTimeF2_Int < 0) gHoldTimeF2_Int = 0;
                else if (gHoldTimeF2_Int > 31) gHoldTimeF2_Int = 31;

                gHoldTimeF2 = gHoldTimeF2_Int / gClockSpeed;
            }
        }
        public int HoldTimeF3_Int
        {
            get
            {
                return gHoldTimeF3_Int;
            }

            set
            {
                gHoldTimeF3_Int = value;

                if (gHoldTimeF3_Int < 0) gHoldTimeF3_Int = 0;
                else if (gHoldTimeF3_Int > 31) gHoldTimeF3_Int = 31;

                gHoldTimeF3 = gHoldTimeF3_Int / gClockSpeed;
            }
        }
        public double HoldTimeSL
        {
            get
            {
                return gHoldTimeSL;
            }

            set
            {
                HoldTimeSL_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public double HoldTimeF2
        {
            get
            {
                return gHoldTimeF2;
            }

            set
            {
                HoldTimeF2_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public double HoldTimeF3
        {
            get
            {
                return gHoldTimeF3;
            }

            set
            {
                HoldTimeF3_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public int DGainSL_Int
        {
            get
            {
                return gDGainSL_Int;
            }

            set
            {
                gDGainSL_Int = value;

                if (gDGainSL_Int < 1) gDGainSL_Int = 1;
                else if (gDGainSL_Int > 16777215) gDGainSL_Int = 16777215;

                gDGainSL = gDGainSL_Int / Math.Pow(2, 22) * gPeakTimeSL_Int;
            }
        }
        public int DGainF2_Int
        {
            get
            {
                return gDGainF2_Int;
            }

            set
            {
                gDGainF2_Int = value;

                if (gDGainF2_Int < 1) gDGainF2_Int = 1;
                else if (gDGainF2_Int > 16777215) gDGainF2_Int = 16777215;

                gDGainF2 = gDGainF2_Int / Math.Pow(2, 22) * gPeakTimeF2_Int;
            }
        }
        public int DGainF3_Int
        {
            get
            {
                return gDGainF3_Int;
            }

            set
            {
                gDGainF3_Int = value;

                if (gDGainF3_Int < 1) gDGainF3_Int = 1;
                else if (gDGainF3_Int > 16777215) gDGainF3_Int = 16777215;

                gDGainF3 = gDGainF3_Int / Math.Pow(2, 22) * gPeakTimeF3_Int;
            }
        }
        public double DGainSL
        {
            get
            {
                return gDGainSL;
            }

            set
            {
                DGainSL_Int = Convert.ToInt32(value * Math.Pow(2, 22) / gPeakTimeSL_Int);
            }
        }
        public double DGainF2
        {
            get
            {
                return gDGainF2;
            }

            set
            {
                DGainF2_Int = Convert.ToInt32(value * Math.Pow(2, 22) / gPeakTimeF2_Int);
            }
        }
        public double DGainF3
        {
            get
            {
                return gDGainF3;
            }

            set
            {
                DGainF3_Int = Convert.ToInt32(value * Math.Pow(2, 22) / gPeakTimeF3_Int);
            }
        }
        public int ThreshSL
        {
            get
            {
                return gThreshSL;
            }

            set
            {
                gThreshSL = value;

                if (gThreshSL < 1) gThreshSL = 1;
                else if (gThreshSL > 4095) gThreshSL = 4095;
            }
        }
        public int ThreshF2
        {
            get
            {
                return gThreshF2;
            }

            set
            {
                gThreshF2 = value;

                if (gThreshF2 < 1) gThreshF2 = 1;
                else if (gThreshF2 > 4095) gThreshF2 = 4095;
            }
        }
        public int ThreshF3
        {
            get
            {
                return gThreshF3;
            }

            set
            {
                gThreshF3 = value;

                if (gThreshF3 < 1) gThreshF3 = 1;
                else if (gThreshF3 > 4095) gThreshF3 = 4095;
            }
        }
        public int PPTC_Int
        {
            get
            {
                return gPPTC_Int;
            }

            set
            {
                gPPTC_Int = value;

                if (gPPTC_Int < 0) gPPTC_Int = 0;
                else if (gPPTC_Int > 255) gPPTC_Int = 255;

                CalcEQ();
            }
        }
        public double PPTC
        {
            get
            {
                return gPPTC;
            }

            set
            {
                if (gEQSlope == 0) { gEQSlope = 0.293; gEQOffset = 0.038; }
                PPTC_Int = Convert.ToInt32((value - gEQOffset) / gEQSlope);
            }
        }
        public int PPGain_Int
        {
            get
            {
                return gPPGain_Int;
            }

            set
            {
                gPPGain_Int = value;

                if (gPPGain_Int < 0) gPPGain_Int = 0;
                else if (gPPGain_Int > 255) gPPGain_Int = 255;

                int vMSB = 0;
                if (Convert.ToInt32(gPPGain_Int) > 127) vMSB = 1;
                int vGainCode = Convert.ToInt32(gPPGain_Int) - vMSB * 128;
                gPPGain = vGainCode * cPPG_Vernier * (1 + (cPPG_PreGain - 1) * vMSB);
            }
        }
        public double PPGain
        {
            get
            {
                return gPPGain;
            }

            set
            {
                int vMSB = 0;
                double vGain = value;

                if (vGain > cPPG_MaxLowGain) vMSB = 1;

                int vGainCode = Convert.ToInt32(vGain / (cPPG_Vernier * (1 + (cPPG_PreGain - 1) * vMSB)));
                if (vGainCode < 1) vGainCode = 1;
                if (vGainCode > 127) vGainCode = 127;

                PPGain_Int = Convert.ToInt32((vMSB << 7) + vGainCode);
            }
        }
        public int EQFactor
        {
            get
            {
                return gEQFactor;
            }

            set
            {
                gEQFactor = value;

                if (gEQFactor < 0) gEQFactor = 0;
                else if (gEQFactor > 65535) gEQFactor = 65535;
            }
        }
        public int ZeroFactor
        {
            get
            {
                return gZeroFactor;
            }

            set
            {
                gZeroFactor = value;

                if (gZeroFactor < -1) gZeroFactor = -1;
                else if (gZeroFactor > 16383) gZeroFactor = 16383;
            }
        }
        public int RInhibit_Int
        {
            get
            {
                return gRInhibit_Int;
            }

            set
            {
                gRInhibit_Int = value;

                if (gRInhibit_Int < 1) gRInhibit_Int = 1;
                else if (gRInhibit_Int > 4095) gRInhibit_Int = 4095;

                gRInhibit = gRInhibit_Int / gClockSpeed;
            }
        }
        public double RInhibit
        {
            get
            {
                return gRInhibit;
            }

            set
            {
                RInhibit_Int = Convert.ToInt32(value * gClockSpeed);
            }
        }
        public int BLRMode
        {
            get
            {
                return gBLRMode;
            }

            set
            {
                gBLRMode = value;
                if (gBLRMode < 0) gBLRMode = 0;
                else if (gBLRMode > 1) gBLRMode = 1;
            }
        }
        public int BLRWindow
        {
            get
            {
                return gBLRWindow;
            }

            set
            {
                gBLRWindow = value;
                if (gBLRWindow < 1) gBLRWindow = 1;
                else if (gBLRWindow > 4095) gBLRWindow = 4095;
            }
        }
        public int DTLength
        {
            get
            {
                return gDTLength;
            }

            set
            {
                gDTLength = value;
                if (gDTLength < 0) gDTLength = 0;
                else if (gDTLength > 4) gDTLength = 4;
            }
        }
        public int RInterval_Int
        {
            get
            {
                return gRInterval_Int;
            }

            set
            {
                gRInterval_Int = value;
                if (gRInterval_Int < 1) gRInterval_Int = 1;
                else if (gRInterval_Int > 65535) gRInterval_Int = 65535;
                gRInterval = Convert.ToDouble(gRInterval_Int) / 1000;
            }
        }
        public double RInterval
        {
            get
            {
                return gRInterval;
            }

            set
            {
                RInterval_Int = Convert.ToInt32(value * 1000);
            }
        }
        public int ChnOffset
        {
            get
            {
                return gChnOffset;
            }

            set
            {
                gChnOffset = value;
                if (gChnOffset < 0) gChnOffset = 0;
                else if (gChnOffset > 4095) gChnOffset = 4095;
            }
        }

        public eMXDPP50_SCAMode SCAMode
        {
            get
            {
                return gSCAMode;
            }

            set
            {
                gSCAMode = value;
            }
        }
        public int SCALo1
        {
            get
            {
                return gSCALo1;
            }

            set
            {
                gSCALo1 = value;
                if (gSCALo1 < 0) gSCALo1 = 0;
                else if (gSCALo1 > 4095) gSCALo1 = 4095;
            }
        }
        public int SCALo2
        {
            get
            {
                return gSCALo2;
            }

            set
            {
                gSCALo2 = value;
                if (gSCALo2 < 0) gSCALo2 = 0;
                else if (gSCALo2 > 4095) gSCALo2 = 4095;
            }
        }
        public int SCALo3
        {
            get
            {
                return gSCALo3;
            }

            set
            {
                gSCALo3 = value;
                if (gSCALo3 < 0) gSCALo3 = 0;
                else if (gSCALo3 > 4095) gSCALo3 = 4095;
            }
        }
        public int SCALo4
        {
            get
            {
                return gSCALo4;
            }

            set
            {
                gSCALo4 = value;
                if (gSCALo4 < 0) gSCALo4 = 0;
                else if (gSCALo4 > 4095) gSCALo4 = 4095;
            }
        }
        public int SCALo5
        {
            get
            {
                return gSCALo5;
            }

            set
            {
                gSCALo5 = value;
                if (gSCALo5 < 0) gSCALo5 = 0;
                else if (gSCALo5 > 4095) gSCALo5 = 4095;
            }
        }
        public int SCALo6
        {
            get
            {
                return gSCALo6;
            }

            set
            {
                gSCALo6 = value;
                if (gSCALo1 < 0) gSCALo1 = 0;
                else if (gSCALo1 > 4095) gSCALo1 = 4095;
            }
        }
        public int SCALo7
        {
            get
            {
                return gSCALo7;
            }

            set
            {
                gSCALo7 = value;
                if (gSCALo7 < 0) gSCALo7 = 0;
                else if (gSCALo7 > 4095) gSCALo7 = 4095;
            }
        }
        public int SCALo8
        {
            get
            {
                return gSCALo8;
            }

            set
            {
                gSCALo8 = value;
                if (gSCALo8 < 0) gSCALo8 = 0;
                else if (gSCALo8 > 4095) gSCALo8 = 4095;
            }
        }
        public int SCAHi1
        {
            get
            {
                return gSCAHi1;
            }

            set
            {
                gSCAHi1 = value;
                if (gSCAHi1 < 0) gSCAHi1 = 0;
                else if (gSCAHi1 > 4095) gSCAHi1 = 4095;
            }
        }
        public int SCAHi2
        {
            get
            {
                return gSCAHi2;
            }

            set
            {
                gSCAHi2 = value;
                if (gSCAHi2 < 0) gSCAHi2 = 0;
                else if (gSCAHi2 > 4095) gSCAHi2 = 4095;
            }
        }
        public int SCAHi3
        {
            get
            {
                return gSCAHi3;
            }

            set
            {
                gSCAHi3 = value;
                if (gSCAHi3 < 0) gSCAHi3 = 0;
                else if (gSCAHi3 > 4095) gSCAHi3 = 4095;
            }
        }
        public int SCAHi4
        {
            get
            {
                return gSCAHi4;
            }

            set
            {
                gSCAHi4 = value;
                if (gSCAHi4 < 0) gSCAHi4 = 0;
                else if (gSCAHi4 > 4095) gSCAHi4 = 4095;
            }
        }
        public int SCAHi5
        {
            get
            {
                return gSCAHi5;
            }

            set
            {
                gSCAHi5 = value;
                if (gSCAHi5 < 0) gSCAHi5 = 0;
                else if (gSCAHi5 > 4095) gSCAHi5 = 4095;
            }
        }
        public int SCAHi6
        {
            get
            {
                return gSCAHi6;
            }

            set
            {
                gSCAHi6 = value;
                if (gSCAHi6 < 0) gSCAHi6 = 0;
                else if (gSCAHi6 > 4095) gSCAHi6 = 4095;
            }
        }
        public int SCAHi7
        {
            get
            {
                return gSCAHi7;
            }

            set
            {
                gSCAHi7 = value;
                if (gSCAHi7 < 0) gSCAHi7 = 0;
                else if (gSCAHi7 > 4095) gSCAHi7 = 4095;
            }
        }
        public int SCAHi8
        {
            get
            {
                return gSCAHi8;
            }

            set
            {
                gSCAHi8 = value;
                if (gSCAHi8 < 0) gSCAHi8 = 0;
                else if (gSCAHi8 > 4095) gSCAHi8 = 4095;
            }
        }
        #endregion

        #region "Private Functions"
        private void CalcEQ()
        {
            try
            {
                gPPTC = gPPTC_Int * gEQSlope + gEQOffset;
                gEQFactor = Convert.ToInt32(1 / gClockSpeed / gPPTC * Math.Pow(2, 17));
            }
            catch 
            {
                gPPTC = 0;
                gEQFactor = 0;
            }
        }
        #endregion
    }
    public class MXDPP50_Hardware
    {
        #region "Constants"
        const int cHVPolBit = 0;
        const int cSigPolBit = 1;
        const int cTCModeBit = 2;
        const int cAuxOut2Bit = 6;
        const int cAuxOut1Bit = 7;
        const int cTCReadyBit = 0;
        const int cAuxIn2Bit = 6;
        const int cAuxIn1Bit = 7;

        const double cM = 0.0002367;
        const double cN = 0.0000007811;
        const double cA = 0.0018590668;
        #endregion

        #region "Variables"
        eMXDPP50_TCMode gTCMode;
        double gTCSet;
        double gTCMon;
        bool gTCReady;
        double gTCTecMon;
        double gDPPTemp;

        eMXDPP50_Polarity gHVPol;
        double gHVSet;
        double gHVMon;

        eMXDPP50_Polarity gSigPol;
        int gAuxIn1;
        int gAuxIn2;
        int gAuxOut1;
        int gAuxOut2;
        eMXDPP50_DACMode gAnalogOut;

        int gTCSet_Int;
        int gTCMon_Int;
        int gTCTecMon_Int;
        int gDPPTemp_Int;
        int gHVSet_Int;
        int gHVMon_Int;
        int gDigInputs_Int;
        int gDigOutputs_Int;

        double gTCPullup = 3320;
        double gTCSupply = 5.0;
        #endregion

        #region "Constructor"
        public MXDPP50_Hardware(double vTCSupply, double vTCPullup)
        {
            TCSupply = vTCSupply;
            TCPullup = vTCPullup;
        }
        #endregion

        #region "Properties"
        public double TCPullup
        {
            get
            {
                return gTCPullup;
            }

            set
            {
                gTCPullup = value;
            }
        }
        public double TCSupply
        {
            get
            {
                return gTCSupply;
            }

            set
            {
                gTCSupply = value;
            }
        }

        public eMXDPP50_TCMode TCMode
        {
            get
            {
                return gTCMode;
            }

            set
            {
                gTCMode = value;
                switch (gTCMode)
                {
                    case eMXDPP50_TCMode.DET: gDigOutputs_Int &= ~(1 << cTCModeBit); break;  // Clear Bit
                    case eMXDPP50_TCMode.BOX: gDigOutputs_Int |= (1 << cTCModeBit); break;  // Set Bit
                }
            }
        }
        public double TCSet
        {
            get
            {
                return gTCSet;
            }

            set
            {
                TCSet_Int = Convert.ToInt32(ThermResToVolt(ThermTempCToRes(value), gTCPullup, gTCSupply) * 2048 / 2.5);
            }
        }
        public double TCMon
        {
            get
            {
                return gTCMon;
            }

            set
            {
                gTCMon = value;

                TCMon_Int = Convert.ToInt32(ThermResToVolt(ThermTempCToRes(value), gTCPullup, gTCSupply) * 4095 / 2.5 / 2);
            }
        }
        public bool TCReady
        {
            get
            {
                return gTCReady;
            }

            set
            {
                gTCReady = value;
                switch (gTCReady)
                {
                    case false: gDigInputs_Int &= ~(1 << cTCReadyBit); break;  // Clear Bit
                    case true: gDigInputs_Int |= (1 << cTCReadyBit); break;  // Set Bit
                }
            }
        }
        public double TCTecMon
        {
            get
            {
                return gTCTecMon;
            }

            set
            {
                gTCTecMon = value;

                TCTecMon_Int = Convert.ToInt32(value * 4095 / 2.5 / 2);
            }
        }
        public double DPPTemp
        {
            get
            {
                return gDPPTemp;
            }

            set
            {
                gDPPTemp = value;

                DPPTemp_Int = Convert.ToInt32((4095 * (value * 0.0156 + 0.48)) / 2.5);


            }
        }

        public eMXDPP50_Polarity HVPol
        {
            get
            {
                return gHVPol;
            }

            set
            {
                gHVPol = value;
                switch (gHVPol)
                {
                    case eMXDPP50_Polarity.POS: gDigOutputs_Int &= ~(1 << cHVPolBit); break;  // Clear Bit
                    case eMXDPP50_Polarity.NEG: gDigOutputs_Int |= (1 << cHVPolBit); break;  // Set Bit
                }
            }
        }
        public double HVSet
        {
            get
            {
                return gHVSet;
            }

            set
            {
                HVSet_Int = Convert.ToInt32((value * 2048) / (2.5 * 50));
            }
        }
        public double HVMon
        {
            get
            {
                return gHVMon;
            }

            set
            {
                HVMon_Int = Convert.ToInt32((value * 4095) / (2.5 * 101.5));
            }
        }

        public eMXDPP50_Polarity SigPol
        {
            get
            {
                return gSigPol;
            }

            set
            {
                gSigPol = value;
                switch (gSigPol)
                {
                    case eMXDPP50_Polarity.NEG: gDigOutputs_Int &= ~(1 << cSigPolBit); break;  // Clear Bit
                    case eMXDPP50_Polarity.POS: gDigOutputs_Int |= (1 << cSigPolBit); break;  // Set Bit
                }
            }
        }
        public int AuxIn1
        {
            get
            {
                return gAuxIn1;
            }

            set
            {
                gAuxIn1 = value;
                if (gAuxIn1 < 0) gAuxIn1 = 0;
                else if (gAuxIn1 > 1) gAuxIn1 = 1;

                switch (gAuxIn1)
                {
                    case 0: gDigInputs_Int &= ~(1 << cAuxIn1Bit); break;  // Clear Bit
                    case 1: gDigInputs_Int |= (1 << cAuxIn1Bit); break;  // Set Bit
                }
            }
        }
        public int AuxIn2
        {
            get
            {
                return gAuxIn2;
            }

            set
            {
                gAuxIn2 = value;
                if (gAuxIn2 < 0) gAuxIn2 = 0;
                else if (gAuxIn2 > 1) gAuxIn2 = 1;

                switch (gAuxIn2)
                {
                    case 0: gDigInputs_Int &= ~(1 << cAuxIn2Bit); break;  // Clear Bit
                    case 1: gDigInputs_Int |= (1 << cAuxIn2Bit); break;  // Set Bit
                }
            }
        }
        public int AuxOut1
        {
            get
            {
                return gAuxOut1;
            }

            set
            {
                gAuxOut1 = value;
                if (gAuxOut1 < 0) gAuxOut1 = 0;
                else if (gAuxOut1 > 1) gAuxOut1 = 1;

                switch (gAuxOut1)
                {
                    case 0: gDigOutputs_Int &= ~(1 << cAuxOut1Bit); break;  // Clear Bit
                    case 1: gDigOutputs_Int |= (1 << cAuxOut1Bit); break;  // Set Bit
                }
            }
        }
        public int AuxOut2
        {
            get
            {
                return gAuxOut2;
            }

            set
            {
                gAuxOut2 = value;
                if (gAuxOut2 < 0) gAuxOut2 = 0;
                else if (gAuxOut2 > 1) gAuxOut2 = 1;

                switch (gAuxOut2)
                {
                    case 0: gDigOutputs_Int &= ~(1 << cAuxOut2Bit); break;  // Clear Bit
                    case 1: gDigOutputs_Int |= (1 << cAuxOut2Bit); break;  // Set Bit
                }
            }
        }
        public eMXDPP50_DACMode AnalogOut
        {
            get
            {
                return gAnalogOut;
            }

            set
            {
                gAnalogOut = value;
            }
        }

        public int TCSet_Int
        {
            get
            {
                return gTCSet_Int;
            }

            set
            {
                gTCSet_Int = value;
                if (gTCSet_Int < 0) gTCSet_Int = 0;
                else if (gTCSet_Int > 4095) gTCSet_Int = 4095;

                gTCSet = ThermResToTempC(ThermVoltToRes(2.5 * Convert.ToDouble(gTCSet_Int) / 2048, TCPullup, TCSupply));
            }
        }
        public int TCMon_Int
        {
            get
            {
                return gTCMon_Int;
            }

            set
            {
                gTCMon_Int = value;
                if (gTCMon_Int < 0) gTCMon_Int = 0;
                else if (gTCMon_Int > 4095) gTCMon_Int = 4095;

                double vVolts = 2.5 * Convert.ToInt32(gTCMon_Int) / 4095 * 2;
                gTCMon = ThermResToTempC(ThermVoltToRes(vVolts, TCPullup, TCSupply));
            }
        }
        public int TCTecMon_Int
        {
            get
            {
                return gTCTecMon_Int;
            }

            set
            {
                gTCTecMon_Int = value;
                if (gTCTecMon_Int < 0) gTCTecMon_Int = 0;
                else if (gTCTecMon_Int > 4095) gTCTecMon_Int = 4095;

                gTCTecMon = 2.5 * Convert.ToInt32(gTCTecMon_Int) / 4095 * 2;
            }
        }
        public int DPPTemp_Int
        {
            get
            {
                return gDPPTemp_Int;
            }

            set
            {
                gDPPTemp_Int = value;
                if (gDPPTemp_Int < 0) gDPPTemp_Int = 0;
                else if (gDPPTemp_Int > 4095) gDPPTemp_Int = 4095;

                double vVolts = 2.5 * Convert.ToInt32(gDPPTemp_Int) / 4095;
                gDPPTemp = (vVolts - 0.48) / 0.0156;
            }
        }
        public int HVSet_Int
        {
            get
            {
                return gHVSet_Int;
            }

            set
            {
                gHVSet_Int = value;
                if (gHVSet_Int < 0) gHVSet_Int = 0;
                else if (gHVSet_Int > 4095) gHVSet_Int = 4095;

                gHVSet = 2.5 * Convert.ToDouble(gHVSet_Int) / 2048 * 50;
            }
        }
        public int HVMon_Int
        {
            get
            {
                return gHVMon_Int;
            }

            set
            {
                gHVMon_Int = value;
                if (gHVMon_Int < 0) gHVMon_Int = 0;
                else if (gHVMon_Int > 4095) gHVMon_Int = 4095;

                gHVMon = 2.5 * Convert.ToInt32(gHVMon_Int) / 4095 * 101.5;
            }
        }
        public int DigInputs_Int
        {
            get
            {
                return gDigInputs_Int;
            }

            set
            {
                gDigInputs_Int = value;
                if (gDigInputs_Int < 0) gDigInputs_Int = 0;
                else if (gDigInputs_Int > 255) gDigInputs_Int = 255;

                gTCReady = Convert.ToBoolean(((gDigInputs_Int >> cTCReadyBit) & 1));
                gAuxIn1 = ((gDigInputs_Int >> cAuxIn1Bit) & 1);
                gAuxIn2 = ((gDigInputs_Int >> cAuxIn2Bit) & 1);
            }
        }
        public int DigOutputs_Int
        {
            get
            {
                return gDigOutputs_Int;
            }

            set
            {
                gDigOutputs_Int = value;
                if (gDigOutputs_Int < 0) gDigOutputs_Int = 0;
                else if (gDigOutputs_Int > 255) gDigOutputs_Int = 255;

                gHVPol = (eMXDPP50_Polarity)((~gDigOutputs_Int >> cHVPolBit) & 1);
                gSigPol = (eMXDPP50_Polarity)((gDigOutputs_Int >> cSigPolBit) & 1);
                gTCMode = (eMXDPP50_TCMode)((gDigOutputs_Int >> cTCModeBit) & 1);
                gAuxOut1 = ((gDigOutputs_Int >> cAuxOut1Bit) & 1);
                gAuxOut2 = ((gDigOutputs_Int >> cAuxOut2Bit) & 1);
            }
        }
        #endregion

        #region "Utilities"
        private double ThermVoltToRes(double vVolt, double vPullup, double vSupply)
        {
            return vVolt * vPullup / (vSupply - vVolt);
        }
        private double ThermResToTempC(double vRes)
        {
            return 1 / (cM * Math.Log(vRes) + cN * Math.Pow(Math.Log(vRes), 3) + cA) - 273.15;
        }
        private double ThermResToTempF(double vRes)
        {
            return ThermResToTempC(vRes) * 1.8 + 32;
        }
        private double ThermResToVolt(double vRes, double vPullup, double vSupply)
        {
            return vSupply * vRes / (vRes + vPullup);
        }
        private double ThermTempCToRes(double vTemp)
        {
            double vA;
            double vB;
            double vC;
            double vD;

            vA = (cA - 1 / (vTemp + 273.15)) / cN;
            vB = Math.Sqrt(Math.Pow(cM / (3 * cN), 3) + Math.Pow(vA, 2) / 4);
            vC = Math.Pow(vB - vA / 2, 1 / 3f);
            vD = Math.Pow(vB + vA / 2, 1 / 3f);
            return Math.Exp(vC - vD);
        }
        private double ThermTempFtoRes(double vTemp)
        {
            return ThermTempCToRes((vTemp - 32) * 1.8);
        }
        #endregion
    }
    public class MXDPP50_Statistics
    {
        #region "Variables"
        bool gAcquisitionInProgress = false;
        double gTimeReal = 0;
        double gTimeLive = 0;
        double gTimeDead = 0;
        uint gRateInput = 0;
        uint gRateOutput = 0;
        uint gRateCorrected = 0;
        uint gRateSlow = 0;
        uint gRateFast2 = 0;
        uint gRateFast3 = 0;
        #endregion

        #region "Properties"
        public bool AcquisitionInProgress
        {
            get
            {
                return gAcquisitionInProgress;
            }

            set
            {
                gAcquisitionInProgress = value;
            }
        }
        public double TimeReal
        {
            get
            {
                return gTimeReal;
            }

            set
            {
                gTimeReal = value;
            }
        }
        public double TimeLive
        {
            get
            {
                return gTimeLive;
            }

            set
            {
                gTimeLive = value;
            }
        }
        public double TimeDead
        {
            get
            {
                return gTimeDead;
            }

            set
            {
                gTimeDead = value;
            }
        }
        public uint RateInput
        {
            get
            {
                return gRateInput;
            }

            set
            {
                gRateInput = value;
            }
        }
        public uint RateOutput
        {
            get
            {
                return gRateOutput;
            }

            set
            {
                gRateOutput = value;
            }
        }
        public uint RateCorrected
        {
            get
            {
                return gRateCorrected;
            }

            set
            {
                gRateCorrected = value;
            }
        }
        public uint RateSlow
        {
            get
            {
                return gRateSlow;
            }

            set
            {
                gRateSlow = value;
            }
        }
        public uint RateFast2
        {
            get
            {
                return gRateFast2;
            }

            set
            {
                gRateFast2 = value;
            }
        }
        public uint RateFast3
        {
            get
            {
                return gRateFast3;
            }

            set
            {
                gRateFast3 = value;
            }
        }
        #endregion
    }
    public class MXDPP50_Presets
    {
        #region "Variables"
        eMXDPP50_TimerMode gTimerMode;
        uint gTimer;
        uint gTotalCounts;
        uint gPeakCounts;
        uint gROICounts;
        int gROIHiCHN;
        int gROILoCHN;
        #endregion

        #region "Properties"
        public eMXDPP50_TimerMode TimerMode
        {
            get
            {
                return gTimerMode;
            }

            set
            {
                gTimerMode = value;
            }
        }
        public uint Timer
        {
            get
            {
                return gTimer;
            }

            set
            {
                gTimer = value;
            }
        }
        public uint TotalCounts
        {
            get
            {
                return gTotalCounts;
            }

            set
            {
                gTotalCounts = value;
            }
        }
        public uint PeakCounts
        {
            get
            {
                return gPeakCounts;
            }

            set
            {
                gPeakCounts = value;
            }
        }
        public uint ROICounts
        {
            get
            {
                return gROICounts;
            }

            set
            {
                gROICounts = value;
            }
        }
        public int ROIHiCHN
        {
            get
            {
                return gROIHiCHN;
            }

            set
            {
                gROIHiCHN = value;
                if (gROIHiCHN < 0) gROIHiCHN = 0;
                else if (gROIHiCHN > 4095) gROIHiCHN = 4095;
            }
        }
        public int ROILoCHN
        {
            get
            {
                return gROILoCHN;
            }

            set
            {
                gROILoCHN = value;
                if (gROILoCHN < 0) gROILoCHN = 0;
                else if (gROILoCHN > 4095) gROILoCHN = 4095;
            }
        }
        #endregion
    }
    public class MXDPP50_CommEventArgs : EventArgs
    {
        #region "Variables"
        DateTime gTimeStamp = DateTime.Now;
        string gEventType = "";
        string gEventData = "";
        #endregion

        #region "Constructor"
        public MXDPP50_CommEventArgs(string vType, string vData)
        {
            gTimeStamp = DateTime.Now;
            this.gEventType = vType;
            this.gEventData = vData;
        }
        #endregion

        #region "Properties"
        public DateTime TimeStamp
        {
            get { return gTimeStamp; }
            set { this.gTimeStamp = value; }
        }
        public string EventType
        {
            get { return gEventType; }
            set { this.gEventType = value; }
        }
        public string EventData
        {
            get { return gEventData; }
            set { this.gEventData = value; }
        }
        #endregion
    }
    public class MXDPP50_ProgressArgs : EventArgs
    {
        #region "Variables"
        string gTask = "Default";
        int gProgressTotal = 100;
        int gProgressValue = 100;
        #endregion

        #region "Constructors"
        public MXDPP50_ProgressArgs(string vTaskName, int vNewProgressTotal, int vNewProgressValue)
        {
            this.Task = vTaskName;
            this.ProgressTotal = vNewProgressTotal;
            this.ProgressValue = vNewProgressValue;
        }
        #endregion

        #region "Properties"
        public string Task
        {
            get
            {
                return gTask;
            }

            set
            {
                gTask = value;
            }
        }
        public int ProgressTotal
        {
            get
            {
                return gProgressTotal;
            }

            set
            {
                gProgressTotal = value;
            }
        }
        public int ProgressValue
        {
            get
            {
                return gProgressValue;
            }

            set
            {
                gProgressValue = value;
            }
        }
        #endregion
    }
    #endregion

    #region "MXDPP50 Enumerated Types"
    public enum eMXDPP50_Polarity
    {
        NEG = 0,
        POS = 1
    }
    public enum eMXDPP50_TCMode
    {
        DET = 0,
        BOX = 1
    }
    public enum eMXDPP50_SCAMode
    {
        Pulse = 0,
        Rate = 1
    }
    public enum eMXDPP50_DACMode
    {
        FilterSlow = 8,
        FilterFast2 = 10,
        FilterFast3 = 11
    }
    public enum eMXDPP50_TimerMode
    {
        LiveTime = 0,
        RealTime = 1
    }
    #endregion

    public class MXDPP50 : IDisposable
    {
        #region "Constants"
        const double cPPG_Vernier = 0.055744;  // See AD8370 datasheet Gain Codes section
        const double cPPG_PreGain = 7.079458;  // See AD8370 datasheet Gain Codes section
        const double cPPG_MaxLowGain = 7.079488;  // See AD8370 datasheet Gain Codes section
        #endregion

        #region "Variables"
        FTDI gFTDI = new FTDI();
        bool gDisposed = false;

        // Instantiate a SafeHandle instance.
        SafeHandle gHandle = new SafeFileHandle(IntPtr.Zero, true);

        string gDescription = "";
        string gFWVersion = "";
        bool gBusy = false;
        bool gConnected = false;

        MXDPP50_Settings gSettings = new MXDPP50_Settings(0.293, 0.038);
        MXDPP50_Hardware gHardware = new MXDPP50_Hardware(5, 3320);
        MXDPP50_Statistics gStatistics = new MXDPP50_Statistics();
        MXDPP50_Presets gPresets = new MXDPP50_Presets();
        UInt32[] gSpectrum = new UInt32[4096];

        
        #endregion

        #region "Events"
        public delegate void CommEventHandler(object sender, MXDPP50_CommEventArgs e);
        public event CommEventHandler CommEvent;
        protected virtual void OnCommEvent(string vEventType, string vEventData)
        {
            MXDPP50_CommEventArgs vArgs = new MXDPP50_CommEventArgs(vEventType, vEventData);
            if (CommEvent != null) CommEvent(this, vArgs);
        }

        public delegate void ProgressHandler(object sender, MXDPP50_ProgressArgs e);
        public event ProgressHandler ProgressEvent;
        protected virtual void OnProgress(string vTaskName, int vProgressTotal, int vProgressNow)
        {
            MXDPP50_ProgressArgs vArgs = new MXDPP50_ProgressArgs(vTaskName, vProgressTotal, vProgressNow);
            if (ProgressEvent != null) ProgressEvent(this, vArgs);
        }
        #endregion

        #region "Constructors"
        public MXDPP50()
        {

        }
        #endregion

        #region "Properties"
        public string FWVersion
        {
            get
            {
                return gFWVersion;
            }

            set
            {
                gFWVersion = value;
            }
        }
        public string Description
        {
            get
            {
                return gDescription;
            }

            set
            {
                gDescription = value;
            }
        }
        public bool Busy
        {
            get
            {
                return gBusy;
            }
        }
        #endregion

        #region "Private Communication Utilities"
        private uint CommWriteStr(string vSendStr)
        {
            FTDI.FT_STATUS vFTDI_Status = new FTDI.FT_STATUS();
            byte[] vBytes = System.Text.Encoding.UTF8.GetBytes(vSendStr);
            uint vBytesWritten = 0;

            try
            {
                // Raise a Comm Event with the Send String
                OnCommEvent("CommWriteStr", vSendStr.Replace("\r", ""));

                // Purge the receive and transmit buffers
                vFTDI_Status = gFTDI.Purge(3);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("CommWriteStr - FTDI_Purge");

                // Write the command to the USB port
                vFTDI_Status = gFTDI.Write(vBytes, vBytes.Length, ref vBytesWritten);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("CommWriteStr - FTDI_Write_String");
            }
            catch (Exception vERR)
            {
                Console.Write("Error CommWriteStr - " + vERR.Message);

                // Raise a Comm Event with the Error Message
                OnCommEvent("CommError", vERR.Message);
                throw;  // Rethrow the original error
            }

            // Return the bytes written
            return vBytesWritten;
        }
        private string CommReadStr()
        {
            FTDI.FT_STATUS vFTDI_Status = new FTDI.FT_STATUS();
            string vRetStr = "";
            bool vDone = false;
            uint vBytesWaiting = 0u;
            uint vBytesRead = 0u;
            Stopwatch vTimeOut = new Stopwatch();

            try
            {
                // Start TimeOut timer
                vTimeOut.Start();

                // Loop until end character is received
                while (!vDone)
                {
                    // Check if Time Out has elapsed
                    if (vTimeOut.ElapsedMilliseconds > 2000) throw new Exception("CommReadStr - Time Out");

                    // Get number of bytes waiting to be read
                    vFTDI_Status = gFTDI.GetRxBytesAvailable(ref vBytesWaiting);
                    if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("CommReadStr - FTDI_GetRxBytesAvailable");

                    // Read bytes from buffer
                    byte[] vBytes = new byte[vBytesWaiting];
                    vFTDI_Status = gFTDI.Read(vBytes, vBytesWaiting, ref vBytesRead);
                    if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("CommReadStr - FTDI_Read");

                    // Add bytes to return string
                    vRetStr += System.Text.Encoding.Default.GetString(vBytes);

                    // Check if end character has been received
                    if (vRetStr.Contains("\r")) vDone = true;
                }

                // Remove end character from string
                vRetStr = vRetStr.Replace("\r", "");

                // Raise a Comm Event with the Return String
                OnCommEvent("CommReadStr", vRetStr);
            }
            catch (Exception vERR)
            {
                Console.Write("Error CommReadStr - " + vERR.Message);

                // Raise a Comm Event with the Error Message
                OnCommEvent("CommError", vERR.Message);
                throw;  // Rethrow the original error
            }

            // Return string
            return vRetStr;
        }
        private byte[] CommReadBytes()
        {
            FTDI.FT_STATUS vFTDI_Status = new FTDI.FT_STATUS();
            bool vDone = false;
            uint vBytesWaiting = 0u;
            uint vBytesRead = 0u;
            uint vTotalBytes = 0;
            byte[] vDataBytes = new byte[16384];
            Stopwatch vTimeOut = new Stopwatch();

            try
            {
                // Start TimeOut timer
                vTimeOut.Start();

                // Loop until end character is received
                while (!vDone)
                {
                    // Check if Time Out has elapsed
                    if (vTimeOut.ElapsedMilliseconds > 2000) throw new Exception("CommReadArray - Time Out");

                    // Get number of bytes waiting to be read
                    vFTDI_Status = gFTDI.GetRxBytesAvailable(ref vBytesWaiting);
                    if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("CommReadArray - FTDI_GetRxBytesAvailable");

                    // Read bytes from buffer
                    byte[] vBytes = new byte[vBytesWaiting];
                    vFTDI_Status = gFTDI.Read(vBytes, vBytesWaiting, ref vBytesRead);
                    if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("CommReadArray - FTDI_Read");

                    // Add bytes to data byte array
                    for (int vI = 0; vI < vBytesRead; vI++)
                    {
                        vDataBytes[vTotalBytes + vI] = vBytes[vI];
                    }
                    vTotalBytes += vBytesRead;

                    // Check if end character has been received
                    if (vTotalBytes >= 16384) vDone = true;
                }

                // Raise a Comm Event with the Return String
                OnCommEvent("CommReadArray", "OK");
            }
            catch (Exception vERR)
            {
                Console.Write("Error CommReadArray - " + vERR.Message);

                // Raise a Comm Event with the Error Message
                OnCommEvent("CommError", vERR.Message);
                throw;  // Rethrow the original error
            }

            // Return byte array
            return vDataBytes;
        }
        private bool TestCommunication()
        {
            bool vReturn = true;  // Return value indicating Test was successful
            string vSendStr = "VN\r";  // Setup command to send to device to get the device version

            // Raise Event for Connect Successful
            OnCommEvent("TestCommunication", "Testing USB Device...");

            // Send string to USB device.  Check to if bytes sent > 0.
            if (CommWriteStr(vSendStr) > 0)
            {
                try
                {
                    string vVersion = CommReadStr();  // Read String from Comm Device
                    gFWVersion = vVersion.Insert(2, ".");
                }
                catch
                {
                    vReturn = false;  // Set return to false
                }
            }
            else vReturn = false;  // Set return to false

            // Raise Event for Test Results
            string vResult;
            if (vReturn) vResult = "Test Successful";
            else vResult = "Test Failed";
            OnCommEvent("Connect", vResult);


            // Return if test was successful or not
            return vReturn;
        }
        #endregion

        #region "Private Utilities"
        private double ParseDescriptionEQSlope(string vDes)
        {
            return Convert.ToDouble(vDes.Substring(vDes.IndexOf("(") + 1, vDes.IndexOf(",") - vDes.IndexOf("(") - 1));
        }
        private double ParseDescriptionEQOffset(string vDes)
        {
            return Convert.ToDouble(vDes.Substring(vDes.IndexOf(",") + 1, vDes.IndexOf(")") - vDes.IndexOf(",") - 1));
        }



        #endregion

        #region "Public Sync Commands"

        /// <summary>
        /// Gets a list of MXDPP50 serial numbers connected to the computer.
        /// </summary>
        /// <returns>Returns a list of MXDPP50 serial numbers.</returns>
        public List<string> GetDeviceList()
        {
            List<string> vDeviceList = new List<string>();  // List to hold the list of USB devices
            uint vDeviceCNT = 0;  // Count of how many devices connected to the computer

            // Get list of USB devices from FTDI class
            FTDI.FT_STATUS vStatus = gFTDI.GetNumberOfDevices(ref vDeviceCNT);

            // Check if there are any devices connected
            if (vDeviceCNT > 0)
            {

                FTDI.FT_DEVICE_INFO_NODE[] vFTDI_DeviceList = new FTDI.FT_DEVICE_INFO_NODE[vDeviceCNT];  // Holds info pertaining to each device

                // Get info list from the FTDI class
                gFTDI.GetDeviceList(vFTDI_DeviceList);

                // Go through the info for each device and add any MXDPP-50 devices to return list
                foreach (FTDI.FT_DEVICE_INFO_NODE vDevice in vFTDI_DeviceList)
                {
                    if (vDevice.Description.Contains("MXDPP-50")) vDeviceList.Add(vDevice.SerialNumber);
                }
            }

            return vDeviceList;  // Return the list of strings
        }

        /// <summary>
        /// Opens and tests the USB port for communication with the DPP. 
        /// </summary>
        /// <param name="vSerialNumber">The DPP Serial to connect to.  Example:  DPP50-12345</param>
        /// <returns>No return value.  Raises an error if unsuccessful.</returns>
        public void Connect(string vSerialNumber)
        {
            FTDI.FT_STATUS vFTDI_Status;  // FTDI class status return variable

            try
            {
                Disconnect();

                // Raise Event for Open Port
                OnCommEvent("Connect", "Open Port - " + vSerialNumber);

                // Open the USB port that matches the serial number
                vFTDI_Status = gFTDI.OpenBySerialNumber(vSerialNumber);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_OpenBySerialNumber");

                // Get the USB description from the device
                gDescription = "";
                vFTDI_Status = gFTDI.GetDescription(out gDescription);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_GetDescription");

                // Raise Event for Device Description
                OnCommEvent("Connect", "USB - " + gDescription);

                // Parse the USB description and save the EQSlope and EQOffset
                if (gDescription.Contains("(") && gDescription.Contains(",") && gDescription.Contains(")"))
                {
                    gSettings.EQSlope = ParseDescriptionEQSlope(gDescription);  // Parse EQ Slope
                    gSettings.EQOffset = ParseDescriptionEQOffset(gDescription);  // Parse EQ Offset
                }
                else
                {
                    gSettings.EQSlope = 0.293;  // Set default EQ Slope
                    gSettings.EQOffset = 0.038; // Set default EQ Offset
                }
                //gSettings.EQSlope = EQSlope;
                //gSettings.EQOffset = EQOffset;

                // Reset the device
                vFTDI_Status = gFTDI.ResetDevice();
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_ResetDevice");

                // Purge the receive and transmit buffers
                vFTDI_Status = gFTDI.Purge(3);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_Purge");

                // Set the device Baud Rate
                vFTDI_Status = gFTDI.SetBaudRate(115200);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_SetBaudRate");

                // Set the device data characteristics
                vFTDI_Status = gFTDI.SetDataCharacteristics(8, 0, 0x0000);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_SetDataCharacteristics");

                // Set the device Flow Control
                vFTDI_Status = gFTDI.SetFlowControl(0x0100, 0, 0);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_SetFlowControl");

                // Asserts the Request To Send (RTS) line
                vFTDI_Status = gFTDI.SetRTS(true);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_SetRTS");

                // Asserts the Data Terminal Ready (DTR) line
                vFTDI_Status = gFTDI.SetDTR(true);
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_SetDTR");

                // Test the communication
                if (!TestCommunication()) throw new Exception("Connect Error - Communication Test");

                gConnected = true;

                // Raise Event for Connect Successful
                OnCommEvent("Connect", "Device Connected");

                // If we get this far without any errors then the connection was successful!
            }
            catch (Exception e)
            {
                // Raise a Comm Event with the Error Message
                OnCommEvent("CommError", e.Message);
                throw;  // Rethrow the original error
            }
        }

        /// <summary>
        /// Closes the USB port if it is open.. 
        /// </summary>
        /// <returns>No return value.  Raises an error if unsuccessful.</returns>
        public void Disconnect()
        {
            if (gFTDI.IsOpen)
            {
                FTDI.FT_STATUS vFTDI_Status;  // FTDI class status return variable

                gConnected = false;

                // Raise Event for Close Port
                OnCommEvent("Disconnect", "Close Port");

                // Close the Port
                vFTDI_Status = gFTDI.Close();
                if (vFTDI_Status != FTDI.FT_STATUS.FT_OK) throw new Exception("Connect Error - FTDI_Close");
            }
        }

        public bool IsConnected()
        {
            return gConnected;
        }

        /// <summary>
        /// Reads the settings from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Settings class that contains the settings read from the DPP.</returns>
        public MXDPP50_Settings ReadSettings()
        {
            if (gBusy) throw new Exception("ReadSettings - Busy");

            gBusy = true;
            try
            {
                int vTotalCNT = 40;  OnProgress("ReadSettings", vTotalCNT, 0);
                ReadClockSpeed(); OnProgress("ReadSettings", vTotalCNT, 1);
                ReadPeakTimeSL(); OnProgress("ReadSettings", vTotalCNT, 2);
                ReadPeakTimeF2(); OnProgress("ReadSettings", vTotalCNT, 3);
                ReadPeakTimeF3(); OnProgress("ReadSettings", vTotalCNT, 4);
                ReadHoldTimeSL(); OnProgress("ReadSettings", vTotalCNT, 5);
                ReadHoldTimeF2(); OnProgress("ReadSettings", vTotalCNT, 6);
                ReadHoldTimeF3(); OnProgress("ReadSettings", vTotalCNT, 7);
                ReadDGainSL(); OnProgress("ReadSettings", vTotalCNT, 8);
                ReadDGainF2(); OnProgress("ReadSettings", vTotalCNT, 9);
                ReadDGainF3(); OnProgress("ReadSettings", vTotalCNT, 10);
                ReadThreshSL(); OnProgress("ReadSettings", vTotalCNT, 11);
                ReadThreshF2(); OnProgress("ReadSettings", vTotalCNT, 12);
                ReadThreshF3(); OnProgress("ReadSettings", vTotalCNT, 13);
                ReadPPTC(); OnProgress("ReadSettings", vTotalCNT, 14);
                ReadPPGain(); OnProgress("ReadSettings", vTotalCNT, 15);
                ReadEQFactor(); OnProgress("ReadSettings", vTotalCNT, 16);
                ReadZeroFactor(); OnProgress("ReadSettings", vTotalCNT, 17);
                ReadRInhibit(); OnProgress("ReadSettings", vTotalCNT, 18);
                ReadBLRMode(); OnProgress("ReadSettings", vTotalCNT, 19);
                ReadBLRWindow(); OnProgress("ReadSettings", vTotalCNT, 20);
                ReadDTLength(); OnProgress("ReadSettings", vTotalCNT, 21);
                ReadRInterval(); OnProgress("ReadSettings", vTotalCNT, 22);
                ReadChnOffset(); OnProgress("ReadSettings", vTotalCNT, 23);

                ReadSCAMode(); OnProgress("ReadSettings", vTotalCNT, 24);
                ReadSCALo(1); OnProgress("ReadSettings", vTotalCNT, 25);
                ReadSCALo(2); OnProgress("ReadSettings", vTotalCNT, 26);
                ReadSCALo(3); OnProgress("ReadSettings", vTotalCNT, 27);
                ReadSCALo(4); OnProgress("ReadSettings", vTotalCNT, 28);
                ReadSCALo(5); OnProgress("ReadSettings", vTotalCNT, 29);
                ReadSCALo(6); OnProgress("ReadSettings", vTotalCNT, 30);
                ReadSCALo(7); OnProgress("ReadSettings", vTotalCNT, 31);
                ReadSCALo(8); OnProgress("ReadSettings", vTotalCNT, 32);
                ReadSCAHi(1); OnProgress("ReadSettings", vTotalCNT, 33);
                ReadSCAHi(2); OnProgress("ReadSettings", vTotalCNT, 34);
                ReadSCAHi(3); OnProgress("ReadSettings", vTotalCNT, 35);
                ReadSCAHi(4); OnProgress("ReadSettings", vTotalCNT, 36);
                ReadSCAHi(5); OnProgress("ReadSettings", vTotalCNT, 37);
                ReadSCAHi(6); OnProgress("ReadSettings", vTotalCNT, 38);
                ReadSCAHi(7); OnProgress("ReadSettings", vTotalCNT, 39);
                ReadSCAHi(8); OnProgress("ReadSettings", vTotalCNT, 40);
            }
            catch (Exception vERR)
            {
                Console.Write("Error ReadSettings - " + vERR.Message);
            }
            finally { gBusy = false; }

            return gSettings;
        }

        /// <summary>
        /// Write the settings to the DPP and then reads them back.
        /// </summary>
        /// <param name="vSettings">Optional settings to write to the DPP.  If not provided it will write the Settings in the class property.</param>
        /// <returns>Returns a MXDPP50_Settings class that contains the settings read from the DPP.</returns>
        public MXDPP50_Settings WriteSettings(MXDPP50_Settings vSettings)
        {
            if (gBusy) throw new Exception("WriteSettings - Busy");
            gBusy = true;

            try
            {
                gSettings.EQSlope = vSettings.EQSlope;
                gSettings.EQOffset = vSettings.EQOffset;

                int vTotalCNT = 40; OnProgress("WriteSettings", vTotalCNT, 0);
                WriteClockSpeed(vSettings.ClockSpeed_Int); OnProgress("WriteSettings", vTotalCNT, 1);
                WritePeakTimeSL(vSettings.PeakTimeSL_Int); OnProgress("WriteSettings", vTotalCNT, 2);
                WritePeakTimeF2(vSettings.PeakTimeF2_Int); OnProgress("WriteSettings", vTotalCNT, 3);
                WritePeakTimeF3(vSettings.PeakTimeF3_Int); OnProgress("WriteSettings", vTotalCNT, 4);
                WriteHoldTimeSL(vSettings.HoldTimeSL_Int); OnProgress("WriteSettings", vTotalCNT, 5);
                WriteHoldTimeF2(vSettings.HoldTimeF2_Int); OnProgress("WriteSettings", vTotalCNT, 6);
                WriteHoldTimeF3(vSettings.HoldTimeF3_Int); OnProgress("WriteSettings", vTotalCNT, 7);
                WriteDGainSL(vSettings.DGainSL_Int); OnProgress("WriteSettings", vTotalCNT, 8);
                WriteDGainF2(vSettings.DGainF2_Int); OnProgress("WriteSettings", vTotalCNT, 9);
                WriteDGainF3(vSettings.DGainF3_Int); OnProgress("WriteSettings", vTotalCNT, 10);
                WriteThreshSL(vSettings.ThreshSL); OnProgress("WriteSettings", vTotalCNT, 11);
                WriteThreshF2(vSettings.ThreshF2); OnProgress("WriteSettings", vTotalCNT, 12);
                WriteThreshF3(vSettings.ThreshF3); OnProgress("WriteSettings", vTotalCNT, 13);
                WritePPTC(vSettings.PPTC_Int); OnProgress("WriteSettings", vTotalCNT, 14);
                WritePPGain(vSettings.PPGain_Int); OnProgress("WriteSettings", vTotalCNT, 15);
                WriteEQFactor(vSettings.EQFactor); OnProgress("WriteSettings", vTotalCNT, 16);
                WriteZeroFactor(vSettings.ZeroFactor); OnProgress("WriteSettings", vTotalCNT, 17);
                WriteRInhibit(vSettings.RInhibit_Int); OnProgress("WriteSettings", vTotalCNT, 18);
                WriteBLRMode(vSettings.BLRMode); OnProgress("WriteSettings", vTotalCNT, 19);
                WriteBLRWindow(vSettings.BLRWindow); OnProgress("WriteSettings", vTotalCNT, 20);
                WriteDTLength(vSettings.DTLength); OnProgress("WriteSettings", vTotalCNT, 21);
                WriteRInterval(vSettings.RInterval_Int); OnProgress("WriteSettings", vTotalCNT, 22);
                WriteChnOffset(vSettings.ChnOffset); OnProgress("WriteSettings", vTotalCNT, 23);

                WriteSCAMode((int)vSettings.SCAMode); OnProgress("WriteSettings", vTotalCNT, 24);
                WriteSCALo(1, vSettings.SCALo1); OnProgress("WriteSettings", vTotalCNT, 25);
                WriteSCALo(2, vSettings.SCALo2); OnProgress("WriteSettings", vTotalCNT, 26);
                WriteSCALo(3, vSettings.SCALo3); OnProgress("WriteSettings", vTotalCNT, 27);
                WriteSCALo(4, vSettings.SCALo4); OnProgress("WriteSettings", vTotalCNT, 28);
                WriteSCALo(5, vSettings.SCALo5); OnProgress("WriteSettings", vTotalCNT, 29);
                WriteSCALo(6, vSettings.SCALo6); OnProgress("WriteSettings", vTotalCNT, 30);
                WriteSCALo(7, vSettings.SCALo7); OnProgress("WriteSettings", vTotalCNT, 31);
                WriteSCALo(8, vSettings.SCALo8); OnProgress("WriteSettings", vTotalCNT, 32);
                WriteSCAHi(1, vSettings.SCAHi1); OnProgress("WriteSettings", vTotalCNT, 33);
                WriteSCAHi(2, vSettings.SCAHi2); OnProgress("WriteSettings", vTotalCNT, 34);
                WriteSCAHi(3, vSettings.SCAHi3); OnProgress("WriteSettings", vTotalCNT, 35);
                WriteSCAHi(4, vSettings.SCAHi4); OnProgress("WriteSettings", vTotalCNT, 36);
                WriteSCAHi(5, vSettings.SCAHi5); OnProgress("WriteSettings", vTotalCNT, 37);
                WriteSCAHi(6, vSettings.SCAHi6); OnProgress("WriteSettings", vTotalCNT, 38);
                WriteSCAHi(7, vSettings.SCAHi7); OnProgress("WriteSettings", vTotalCNT, 39);
                WriteSCAHi(8, vSettings.SCAHi8); OnProgress("WriteSettings", vTotalCNT, 40);
            }
            finally { gBusy = false; }

            return gSettings;
        }

        /// <summary>
        /// Reads the hardware from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Hardware class that contains the hardware values read from the DPP.</returns>
        public MXDPP50_Hardware ReadHardware()
        {
            if (gBusy) throw new Exception("ReadHardware - Busy");

            gBusy = true;
            try
            {
                int vTotalCNT = 9; OnProgress("ReadHardware", vTotalCNT, 0);
                ReadDigOutputs(); OnProgress("ReadHardware", vTotalCNT, 1);
                ReadDigInputs(); OnProgress("ReadHardware", vTotalCNT, 2);
                ReadTCSet(); OnProgress("ReadHardware", vTotalCNT, 3);
                ReadTCMon(); OnProgress("ReadHardware", vTotalCNT, 4);
                ReadTCTecMon(); OnProgress("ReadHardware", vTotalCNT, 5);
                ReadDPPTemp(); OnProgress("ReadHardware", vTotalCNT, 6);
                ReadHVSet(); OnProgress("ReadHardware", vTotalCNT, 7);
                ReadHVMon(); OnProgress("ReadHardware", vTotalCNT, 8);
                ReadAnalogOut(); OnProgress("ReadHardware", vTotalCNT, 9);
            }
            catch (Exception vERR)
            {
                Console.Write("Error ReadSettings - " + vERR.Message);
            }
            finally { gBusy = false; }

            return gHardware;
        }

        /// <summary>
        /// Write the hardware settings to the DPP and then reads them back.
        /// </summary>
        /// <param name="vHardware">Optional hardware settings to write to the DPP.  If not provided it will write the hardware settings in the class property.</param>
        /// <returns>Returns a MXDPP50_Hardware class that contains the hardware settings read from the DPP.</returns>
        public MXDPP50_Hardware WriteHardware(MXDPP50_Hardware vHardware)
        {
            if (gBusy) throw new Exception("WriteHardware - Busy");
            gBusy = true;

            try
            {
                //gTCSupply = vHardware.TCSupply;
                //gTCPullup = vHardware.TCPullup;
                gHardware.TCSupply = vHardware.TCSupply;
                gHardware.TCPullup = vHardware.TCPullup;

                int vTotalCNT = 4; OnProgress("WriteHardware", vTotalCNT, 0);
                WriteDigOutputs(vHardware.DigOutputs_Int); OnProgress("WriteHardware", vTotalCNT, 1);
                WriteHVSet(vHardware.HVSet_Int); OnProgress("WriteHardware", vTotalCNT, 2);
                WriteTCSet(vHardware.TCSet_Int); OnProgress("WriteHardware", vTotalCNT, 3);
                WriteAnalogOut((int)vHardware.AnalogOut); OnProgress("WriteHardware", vTotalCNT, 4);
            }
            finally { gBusy = false; }

            return gHardware;
        }

        /// <summary>
        /// Reads the statistics from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Statistics class that contains the statistics read from the DPP.</returns>
        public MXDPP50_Statistics ReadStatistics()
        {
            if (gBusy) throw new Exception("ReadStatistics - Busy");

            gBusy = true;
            try
            {
                int vTotalCNT = 10; OnProgress("ReadStatistics", vTotalCNT, 0);
                ReadAcquisitionInProgress(); OnProgress("ReadStatistics", vTotalCNT, 1);
                ReadTimeReal(); OnProgress("ReadStatistics", vTotalCNT, 2);
                ReadTimeLive(); OnProgress("ReadStatistics", vTotalCNT, 3);
                ReadTimeDead(); OnProgress("ReadStatistics", vTotalCNT, 4);
                ReadRateInput(); OnProgress("ReadStatistics", vTotalCNT, 5);
                ReadRateOutput(); OnProgress("ReadStatistics", vTotalCNT, 6);
                ReadRateCorrected(); OnProgress("ReadStatistics", vTotalCNT, 7);
                ReadRateSlow(); OnProgress("ReadStatistics", vTotalCNT, 8);
                ReadRateFast2(); OnProgress("ReadStatistics", vTotalCNT, 9);
                ReadRateFast3(); OnProgress("ReadStatistics", vTotalCNT, 10);
            }
            catch (Exception vERR)
            {
                Console.Write("Error ReadSettings - " + vERR.Message);
            }
            finally { gBusy = false; }

            return gStatistics;
        }

        /// <summary>
        /// Reads the presets from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Presets class that contains the presets read from the DPP.</returns>
        public MXDPP50_Presets ReadPresets()
        {
            if (gBusy) throw new Exception("ReadPresets - Busy");

            gBusy = true;
            try
            {
                int vTotalCNT = 7; OnProgress("ReadPresets", vTotalCNT, 0);
                ReadPresetTimerMode(); OnProgress("ReadPresets", vTotalCNT, 1);
                ReadPresetTimer(); OnProgress("ReadPresets", vTotalCNT, 2);
                ReadPresetTotalCounts(); OnProgress("ReadPresets", vTotalCNT, 3);
                ReadPresetPeakCounts(); OnProgress("ReadPresets", vTotalCNT, 4);
                ReadPresetROICounts(); OnProgress("ReadPresets", vTotalCNT, 5);
                ReadPresetROILo(); OnProgress("ReadPresets", vTotalCNT, 6);
                ReadPresetROIHi(); OnProgress("ReadPresets", vTotalCNT, 7);
                
            }
            catch (Exception vERR)
            {
                Console.Write("Error ReadPresets - " + vERR.Message);
            }
            finally { gBusy = false; }

            return gPresets;
        }

        /// <summary>
        /// Write the presets to the DPP and then reads them back.
        /// </summary>
        /// <param name="vPresets">Optional presets to write to the DPP.  If not provided it will write the presets in the class property.</param>
        /// <returns>Returns a MXDPP50_Presets class that contains the presets read from the DPP.</returns>
        public MXDPP50_Presets WritePresets(MXDPP50_Presets vPresets = null)
        {
            if (gBusy) throw new Exception("WritePresets - Busy");
            gBusy = true;

            try
            {
                if (vPresets == null) vPresets = gPresets;

                int vTotalCNT = 40; OnProgress("WritePresets", vTotalCNT, 0);
                WritePresetTimerMode((int) vPresets.TimerMode); OnProgress("WritePresets", vTotalCNT, 1);
                WritePresetTimer(vPresets.Timer); OnProgress("WritePresets", vTotalCNT, 2);
                WritePresetTotalCounts(vPresets.TotalCounts); OnProgress("WritePresets", vTotalCNT, 3);
                WritePresetPeakCounts(vPresets.PeakCounts); OnProgress("WritePresets", vTotalCNT, 4);
                WritePresetROICounts(vPresets.ROICounts); OnProgress("WritePresets", vTotalCNT, 5);
                WritePresetROILo(vPresets.ROILoCHN); OnProgress("WritePresets", vTotalCNT, 6);
                WritePresetROIHi(vPresets.ROIHiCHN); OnProgress("WritePresets", vTotalCNT, 7);
            }
            finally { gBusy = false; }

            return gPresets;
        }

        /// <summary>
        /// Writes the DPP Settings, Hardware and Presets to the DPPs memory so they are loaded when the DPP is powered up.
        /// </summary>
        /// <param name="vSettings">Settings to write to memory.</param>
        /// <param name="vPresets">Presets to write to memory.</param>
        /// <param name="vHardware">Hardware to write to memory.  T
        /// <returns>The number of bytes written to the DPP memory</returns>
        public int WriteDefaultsToMemory(MXDPP50_Settings vSettings, MXDPP50_Hardware vHardware, MXDPP50_Presets vPresets)
        {
            if (gBusy) throw new Exception("WriteDefaultsToMemory - Busy");
            gBusy = true;

            int vRet = 0;
            int vWait = 50;

            try
            {
                int vTotalCNT = 53; OnProgress("WriteDefaultsToMemory", vTotalCNT, 0);
                SendMemoryStart(); OnProgress("WriteDefaultsToMemory", vTotalCNT, 1);

                /* Send Settings */
                CommWriteStr("CD" + vSettings.ClockSpeed_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 2); Thread.Sleep(vWait);
                CommWriteStr("PS" + vSettings.PeakTimeSL_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 3); Thread.Sleep(vWait);
                CommWriteStr("PX" + vSettings.PeakTimeF2_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 4); Thread.Sleep(vWait);
                CommWriteStr("PY" + vSettings.PeakTimeF3_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 5); Thread.Sleep(vWait);
                CommWriteStr("HS" + vSettings.HoldTimeSL_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 6); Thread.Sleep(vWait);
                CommWriteStr("HX" + vSettings.HoldTimeF2_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 7); Thread.Sleep(vWait);
                CommWriteStr("HY" + vSettings.HoldTimeF3_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 8); Thread.Sleep(vWait);
                CommWriteStr("GS" + vSettings.DGainSL_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 9); Thread.Sleep(vWait);
                CommWriteStr("GX" + vSettings.DGainF2_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 10); Thread.Sleep(vWait);
                CommWriteStr("GY" + vSettings.DGainF3_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 11); Thread.Sleep(vWait);
                CommWriteStr("TS" + vSettings.ThreshSL.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 12); Thread.Sleep(vWait);
                CommWriteStr("TX" + vSettings.ThreshF2.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 13); Thread.Sleep(vWait);
                CommWriteStr("TY" + vSettings.ThreshF3.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 14); Thread.Sleep(vWait);
                CommWriteStr("PT" + vSettings.PPTC_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 15); Thread.Sleep(vWait);
                CommWriteStr("PG" + vSettings.PPGain_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 16); Thread.Sleep(vWait);
                CommWriteStr("EF" + vSettings.EQFactor.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 17); Thread.Sleep(vWait);
                CommWriteStr("ZF" + vSettings.ZeroFactor.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 18); Thread.Sleep(vWait);
                CommWriteStr("IL" + vSettings.RInhibit_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 19); Thread.Sleep(vWait);
                CommWriteStr("BM" + vSettings.BLRMode.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 20); Thread.Sleep(vWait);
                CommWriteStr("BW" + vSettings.BLRWindow.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 21); Thread.Sleep(vWait);
                CommWriteStr("DL" + vSettings.DTLength.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 22); Thread.Sleep(vWait);
                CommWriteStr("RI" + vSettings.RInterval_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 23); Thread.Sleep(vWait);
                CommWriteStr("CO" + vSettings.ChnOffset.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 24); Thread.Sleep(vWait);

                CommWriteStr("RD" + Convert.ToString((int)vSettings.SCAMode) + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 25); Thread.Sleep(vWait);
                CommWriteStr("_A" + vSettings.SCALo1.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 26); Thread.Sleep(vWait);
                CommWriteStr("_B" + vSettings.SCALo2.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 27); Thread.Sleep(vWait);
                CommWriteStr("_C" + vSettings.SCALo3.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 28); Thread.Sleep(vWait);
                CommWriteStr("_D" + vSettings.SCALo4.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 29); Thread.Sleep(vWait);
                CommWriteStr("_E" + vSettings.SCALo5.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 30); Thread.Sleep(vWait);
                CommWriteStr("_F" + vSettings.SCALo6.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 31); Thread.Sleep(vWait);
                CommWriteStr("_G" + vSettings.SCALo7.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 32); Thread.Sleep(vWait);
                CommWriteStr("_H" + vSettings.SCALo8.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 33); Thread.Sleep(vWait);

                CommWriteStr("^A" + vSettings.SCAHi1.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 34); Thread.Sleep(vWait);
                CommWriteStr("^B" + vSettings.SCAHi2.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 35); Thread.Sleep(vWait);
                CommWriteStr("^C" + vSettings.SCAHi3.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 36); Thread.Sleep(vWait);
                CommWriteStr("^D" + vSettings.SCAHi4.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 37); Thread.Sleep(vWait);
                CommWriteStr("^E" + vSettings.SCAHi5.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 38); Thread.Sleep(vWait);
                CommWriteStr("^F" + vSettings.SCAHi6.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 39); Thread.Sleep(vWait);
                CommWriteStr("^G" + vSettings.SCAHi7.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 40); Thread.Sleep(vWait);
                CommWriteStr("^H" + vSettings.SCAHi8.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 41); Thread.Sleep(vWait);

                /* Send Hardware */
                CommWriteStr("PO" + vHardware.DigOutputs_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 42); Thread.Sleep(vWait);
                CommWriteStr("&A" + vHardware.HVSet_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 43); Thread.Sleep(vWait);
                CommWriteStr("&B" + vHardware.TCSet_Int.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 44); Thread.Sleep(vWait);
                CommWriteStr("DM" + Convert.ToString((int)vHardware.AnalogOut) + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 45); Thread.Sleep(vWait);

                /* Send Presets */
                CommWriteStr("TM" + Convert.ToString((int)vPresets.TimerMode) + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 46); Thread.Sleep(vWait);
                CommWriteStr("PR" + vPresets.Timer.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 47); Thread.Sleep(vWait);
                CommWriteStr("TC" + vPresets.TotalCounts.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 48); Thread.Sleep(vWait);
                CommWriteStr("PC" + vPresets.PeakCounts.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 49); Thread.Sleep(vWait);
                CommWriteStr("RC" + vPresets.ROICounts.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 50); Thread.Sleep(vWait);
                CommWriteStr("WL" + vPresets.ROILoCHN.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 51); Thread.Sleep(vWait);
                CommWriteStr("WH" + vPresets.ROIHiCHN.ToString() + "\r"); OnProgress("WriteDefaultsToMemory", vTotalCNT, 52); Thread.Sleep(vWait);

                vRet = SendMemoryEnd(); OnProgress("WriteDefaultsToMemory", vTotalCNT, 53);

            }
            finally { gBusy = false; }

            return vRet;
        }

        /// <summary>
        /// Clears the current Acquisition and starts a new one.
        /// </summary>
        /// <returns>None</returns>
        public void AcquisitionStart()
        {
            if (gBusy) throw new Exception("AcquisitionStart - Busy");
            gBusy = true;

            try
            {
                SendClear();
                SendStart();
            }
            finally { gBusy = false; }
        }

        /// <summary>
        /// Stops the current Acquisition.
        /// </summary>
        /// <returns>None</returns>
        public void AcquisitionStop()
        {
            if (gBusy) throw new Exception("AcquisitionStop - Busy");
            gBusy = true;

            try
            {
                SendStop();
            }
            finally { gBusy = false; }
        }

        /// <summary>
        /// Continues the current Acquisition without clearing the old one.
        /// </summary>
        /// <returns>None</returns>
        public void AcquisitionContinue()
        {
            if (gBusy) throw new Exception("AcquisitionContinue - Busy");
            gBusy = true;

            try
            {
                SendStart();
            }
            finally { gBusy = false; }
        }

        /// <summary>
        /// Clears the current Acquisition.
        /// </summary>
        /// <returns>None</returns>
        public void AcquisitionClear()
        {
            if (gBusy) throw new Exception("AcquisitionClear - Busy");
            gBusy = true;

            try
            {
                SendClear();
            }
            finally { gBusy = false; }
        }

        /// <summary>
        /// Reads Spectrum from the DPP.
        /// </summary>
        /// <returns>uint array containing the downloaded spectrum.</returns>
        public uint[] ReadSpectrum()
        {
            if (gBusy) throw new Exception("ReadSpectrum - Busy");
            gBusy = true;

            try
            {
                ReadSpectrumData();
            }
            finally { gBusy = false; }

            return gSpectrum;
        }

        /// <summary>
        /// Legacy DPP Config file reader from XSpectrum-DX Config files.  Reads the DPP Settings, Presets and Hardware from file.  The Lagacy file only
        /// supports the Analog Output and Auxillary Outputs of the Hardware class, all other values are defaults in the Hardware class.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <param name="vSettings">Output Settings read from file.</param>
        /// <param name="vPresets">Output Presets read from the file.</param>
        /// <param name="vHardware">Output Hardware read from the file.  The Lagacy file only
        /// supports the Analog Output and Auxillary Outputs of the Hardware class, all other values are defaults.</param>
        /// <returns>None</returns>
        public void FileReadLegacy(string vFilename, out MXDPP50_Settings vSettings, out MXDPP50_Presets vPresets, out MXDPP50_Hardware vHardware)
        {
            if (!File.Exists(vFilename)) throw new Exception("FileReadSettingsLegacy - File Does Not Exist");

            var vText = File.ReadAllLines(vFilename);

            if (!vText.Contains("[MoxtekDppConfiguration]")) throw new Exception("FileReadSettingsLegacy - Wrong File Format");

            vSettings = new MXDPP50_Settings(gSettings.EQSlope, gSettings.EQOffset);
            vPresets = new MXDPP50_Presets();
            vHardware = new MXDPP50_Hardware(gHardware.TCSupply, gHardware.TCPullup);

            foreach (string vLine in vText)
            {
                string vCMD = "";
                string vData = "";

                try
                {
                    vCMD = vLine.Substring(0, vLine.IndexOf("="));
                    vData = vLine.Substring(vLine.IndexOf("=") + 1, vLine.IndexOf(";") - vLine.IndexOf("=") - 1).Trim(' ');
                }
                catch { vCMD = ""; }
                
                switch (vCMD)
                {
                    // Settings
                    case "CD": vSettings.ClockSpeed_Int = Convert.ToInt32(vData); break;
                    case "PS": vSettings.PeakTimeSL = Convert.ToDouble(vData); break;
                    case "PX": vSettings.PeakTimeF2 = Convert.ToDouble(vData); break;
                    case "PY": vSettings.PeakTimeF3 = Convert.ToDouble(vData); break;
                    case "HS": vSettings.HoldTimeSL = Convert.ToDouble(vData); break;
                    case "HX": vSettings.HoldTimeF2 = Convert.ToDouble(vData); break;
                    case "HY": vSettings.HoldTimeF3 = Convert.ToDouble(vData); break;
                    case "GS": vSettings.DGainSL = Convert.ToDouble(vData); break;
                    case "GX": vSettings.DGainF2 = Convert.ToDouble(vData); break;
                    case "GY": vSettings.DGainF3 = Convert.ToDouble(vData); break;
                    case "TS": vSettings.ThreshSL = Convert.ToInt32(vData); break;
                    case "TX": vSettings.ThreshF2 = Convert.ToInt32(vData); break;
                    case "TY": vSettings.ThreshF3 = Convert.ToInt32(vData); break;
                    case "PT": vSettings.PPTC = Convert.ToDouble(vData); break;
                    case "PG": vSettings.PPGain = Convert.ToDouble(vData); break;
                    case "ZF": vSettings.ZeroFactor = Convert.ToInt32(vData); break;
                    case "IL": vSettings.RInhibit = Convert.ToDouble(vData); break;
                    case "BM": vSettings.BLRMode = Convert.ToInt32(vData); break;
                    case "BW": vSettings.BLRWindow = Convert.ToInt32(vData); break;
                    case "DL": vSettings.DTLength = Convert.ToInt32(vData); break;
                    case "RI": vSettings.RInterval = Convert.ToDouble(vData); break;
                    case "CO": vSettings.ChnOffset = Convert.ToInt32(Convert.ToDouble(vData)); break;
                    case "RD": vSettings.SCAMode = (eMXDPP50_SCAMode) Convert.ToInt32(vData); break;
                    case "_A": vSettings.SCALo1 = Convert.ToInt32(vData); break;
                    case "_B": vSettings.SCALo2 = Convert.ToInt32(vData); break;
                    case "_C": vSettings.SCALo3 = Convert.ToInt32(vData); break;
                    case "_D": vSettings.SCALo4 = Convert.ToInt32(vData); break;
                    case "_E": vSettings.SCALo5 = Convert.ToInt32(vData); break;
                    case "_F": vSettings.SCALo6 = Convert.ToInt32(vData); break;
                    case "_G": vSettings.SCALo7 = Convert.ToInt32(vData); break;
                    case "_H": vSettings.SCALo8 = Convert.ToInt32(vData); break;
                    case "^A": vSettings.SCAHi1 = Convert.ToInt32(vData); break;
                    case "^B": vSettings.SCAHi2 = Convert.ToInt32(vData); break;
                    case "^C": vSettings.SCAHi3 = Convert.ToInt32(vData); break;
                    case "^D": vSettings.SCAHi4 = Convert.ToInt32(vData); break;
                    case "^E": vSettings.SCAHi5 = Convert.ToInt32(vData); break;
                    case "^F": vSettings.SCAHi6 = Convert.ToInt32(vData); break;
                    case "^G": vSettings.SCAHi7 = Convert.ToInt32(vData); break;
                    case "^H": vSettings.SCAHi8 = Convert.ToInt32(vData); break;

                    // Presets
                    case "TM": vPresets.TimerMode = (eMXDPP50_TimerMode) Convert.ToInt32(vData); break;
                    case "PR": vPresets.Timer = Convert.ToUInt32(vData); break;
                    case "TC": vPresets.TotalCounts = Convert.ToUInt32(vData); break;
                    case "RC": vPresets.ROICounts = Convert.ToUInt32(vData); break;
                    case "PC": vPresets.PeakCounts = Convert.ToUInt32(vData); break;
                    case "WL": vPresets.ROILoCHN = Convert.ToInt32(vData); break;
                    case "WH": vPresets.ROIHiCHN = Convert.ToInt32(vData); break;

                    // Hardware
                    case "DM":
                        vHardware.AnalogOut = (eMXDPP50_DACMode) Convert.ToInt64(vData);
                        if ((vHardware.AnalogOut != eMXDPP50_DACMode.FilterSlow) ||
                                (vHardware.AnalogOut != eMXDPP50_DACMode.FilterSlow) ||
                                (vHardware.AnalogOut != eMXDPP50_DACMode.FilterSlow))
                            vHardware.AnalogOut = eMXDPP50_DACMode.FilterSlow;
                        break;
                    case "AX1": vHardware.AuxOut1 = Convert.ToInt32(vData); break;
                    case "AX2": vHardware.AuxOut1 = Convert.ToInt32(vData); break;
                }
            }
        }

        /// <summary>
        /// Legacy DPP Config file writer from XSpectrum-DX Config files.  Writes the DPP Settings, Presets and Hardware from file.  The Lagacy file only
        /// supports the Analog Output and Auxillary Outputs of the Hardware class, all other values are ignored in the Hardware class.
        /// </summary>
        /// <param name="vFilename">Filename of the file to write to.</param>
        /// <param name="vSettings">Settings to write to file.</param>
        /// <param name="vPresets">Presets to write to file.</param>
        /// <param name="vHardware">Hardware to write to file.  The Lagacy file only supports the Analog Output and Auxillary Outputs
        /// of the Hardware class, all other values are ignored in the Hardware class.</param>
        /// <returns>None</returns>
        public void FileWriteLegacy(string vFilename, MXDPP50_Settings vSettings, MXDPP50_Presets vPresets, MXDPP50_Hardware vHardware)
        {
            string vText = "[MoxtekDppConfiguration]\r";
            string vLine = "";
            int vPad = 18;

            vLine = "CD=" + vSettings.ClockSpeed_Int.ToString();
            vLine = vLine.PadRight(vPad) + "; Clock Divider (4, 8, 16, 32, 64)\r\n";
            vText += vLine;

            vLine = "TM=" + (int) vPresets.TimerMode;
            vLine = vLine.PadRight(vPad) + "; Timer Mode (0 = Live, 1 = Real)\r\n";
            vText += vLine;

            vLine = "PR=" + vPresets.Timer.ToString();
            vLine = vLine.PadRight(vPad) + "; Preset Time (sec)\r\n";
            vText += vLine;

            vLine = "TC=" + vPresets.TotalCounts.ToString();
            vLine = vLine.PadRight(vPad) + "; Preset Total Count\r\n";
            vText += vLine;

            vLine = "RC=" + vPresets.ROICounts.ToString();
            vLine = vLine.PadRight(vPad) + "; Preset ROI Count\r\n";
            vText += vLine;

            vLine = "PC=" + vPresets.PeakCounts.ToString();
            vLine = vLine.PadRight(vPad) + "; Preset Peak Count\r\n";
            vText += vLine;

            vLine = "PS=" + vSettings.PeakTimeSL.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Slow Ch Peaking Time (usec)\r\n";
            vText += vLine;

            vLine = "PX=" + vSettings.PeakTimeF2.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Fast Ch2 Peaking Time (usec)\r\n";
            vText += vLine;

            vLine = "PY=" + vSettings.PeakTimeF3.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Fast Ch3 Peaking Time (usec)\r\n";
            vText += vLine;

            vLine = "HS=" + vSettings.HoldTimeSL.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Slow Ch Holding Time (usec)\r\n";
            vText += vLine;

            vLine = "HX=" + vSettings.HoldTimeF2.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Fast Ch2 Holding Time (usec)\r\n";
            vText += vLine;

            vLine = "HY=" + vSettings.HoldTimeF3.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Fast Ch3 Holding Time (usec)\r\n";
            vText += vLine;

            vLine = "GS=" + vSettings.DGainSL.ToString("0.0000");
            vLine = vLine.PadRight(vPad) + "; Slow Ch Fine Gain\r\n";
            vText += vLine;

            vLine = "GX=" + vSettings.DGainF2.ToString("0.0000");
            vLine = vLine.PadRight(vPad) + "; Fast Ch2 Fine Gain\r\n";
            vText += vLine;

            vLine = "GY=" + vSettings.DGainF3.ToString("0.0000");
            vLine = vLine.PadRight(vPad) + "; Fast Ch3 Fine Gain\r\n";
            vText += vLine;

            vLine = "TS=" + vSettings.ThreshSL.ToString();
            vLine = vLine.PadRight(vPad) + "; Slow Ch Threshold\r\n";
            vText += vLine;

            vLine = "TX=" + vSettings.ThreshF2.ToString();
            vLine = vLine.PadRight(vPad) + "; Fast Ch2 Threshold\r\n";
            vText += vLine;

            vLine = "TY=" + vSettings.ThreshF3.ToString();
            vLine = vLine.PadRight(vPad) + "; Fast Ch3 Threshold\r\n";
            vText += vLine;

            vLine = "CO=" + vSettings.ChnOffset.ToString("0.0000");
            vLine = vLine.PadRight(vPad) + "; Channel Offset\r\n";
            vText += vLine;

            vLine = "PG=" + vSettings.PPGain.ToString("0.0000");
            vLine = vLine.PadRight(vPad) + "; Preprocessor(Analog) Gain\r\n";
            vText += vLine;

            vLine = "PT=" + vSettings.PPTC.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Preprocessor Time Constant(usec)\r\n";
            vText += vLine;

            vLine = "IL=" + vSettings.RInhibit.ToString("0.00");
            vLine = vLine.PadRight(vPad) + "; Inhibit Length After ADC Overflow\r\n";
            vText += vLine;

            vLine = "BM=" + vSettings.BLRMode.ToString();
            vLine = vLine.PadRight(vPad) + "; Baseline Mode (1 = ON, 0 = OFF)\r\n";
            vText += vLine;

            vLine = "BW=" + vSettings.BLRWindow.ToString();
            vLine = vLine.PadRight(vPad) + "; Baseline Window\r\n";
            vText += vLine;

            vLine = "EF=" + vSettings.EQFactor.ToString();
            vLine = vLine.PadRight(vPad) + "; Equalization Factor\r\n";
            vText += vLine;

            vLine = "ZF=" + vSettings.ZeroFactor.ToString();
            vLine = vLine.PadRight(vPad) + "; Zero Factor (-1 for auto)\r\n";
            vText += vLine;

            vLine = "WL=" + vPresets.ROILoCHN.ToString();
            vLine = vLine.PadRight(vPad) + "; ROI Window Low (channel)\r\n";
            vText += vLine;

            vLine = "WH=" + vPresets.ROIHiCHN.ToString();
            vLine = vLine.PadRight(vPad) + "; ROI Window High (channel)\r\n";
            vText += vLine;

            vLine = "RI=" + vSettings.RInterval.ToString("0.000");
            vLine = vLine.PadRight(vPad) + "; Rate Meter Sampling Interval (sec)\r\n";
            vText += vLine;

            vLine = "DL=" + vSettings.DTLength.ToString();
            vLine = vLine.PadRight(vPad) + "; Deadtime Length (0:1PT 1:1.5PT 2:2PT 3:2.5PT 4:3PT)\r\n";
            vText += vLine;

            vLine = "DM=" + (int) vHardware.AnalogOut;
            vLine = vLine.PadRight(vPad) + "; DAC Analog Output Mode (8:Slow 10:Fast2 11:Fast3)\r\n";
            vText += vLine;

            vLine = "RD=" + (int) vSettings.SCAMode;
            vLine = vLine.PadRight(vPad) + "; SCA Mode (0:Pulse 1:Rate)\r\n";
            vText += vLine;

            vLine = "_A=" + vSettings.SCALo1.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA1 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_B=" + vSettings.SCALo2.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA2 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_C=" + vSettings.SCALo3.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA3 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_D=" + vSettings.SCALo4.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA4 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_E=" + vSettings.SCALo5.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA5 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_F=" + vSettings.SCALo6.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA6 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_G=" + vSettings.SCALo7.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA7 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "_H=" + vSettings.SCALo8.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA8 Window Low (channel)\r\n";
            vText += vLine;

            vLine = "^A=" + vSettings.SCAHi1.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA1 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^B=" + vSettings.SCAHi2.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA2 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^C=" + vSettings.SCAHi3.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA3 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^D=" + vSettings.SCAHi4.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA4 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^E=" + vSettings.SCAHi5.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA5 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^F=" + vSettings.SCAHi6.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA6 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^G=" + vSettings.SCAHi7.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA7 Window High (channel)\r\n";
            vText += vLine;

            vLine = "^H=" + vSettings.SCAHi8.ToString();
            vLine = vLine.PadRight(vPad) + "; SCA8 Window High (channel)\r\n";
            vText += vLine;

            vLine = "AX1=" + vHardware.AuxOut1.ToString();
            vLine = vLine.PadRight(vPad) + "; Auxilary Output 1 (0:Low 1:High)\r\n";
            vText += vLine;

            vLine = "AX2=" + vHardware.AuxOut2.ToString();
            vLine = vLine.PadRight(vPad) + "; Auxilary Output 2 (0:Low 1:High)\r\n";
            vText += vLine;

            File.WriteAllText(vFilename, vText);
        }

        /// <summary>
        /// Reads the DPP Settings from JSON formatted file.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <returns>Settings read from file.</returns>
        public MXDPP50_Settings FileReadSettingsJSON(string vFilename)
        {
            var vLines = File.ReadAllLines(vFilename);
            var vSettings = new MXDPP50_Settings(gSettings.EQSlope, gSettings.EQOffset);

            foreach (string vLine in vLines)
            {
                if (vLine.Contains(":"))
                {
                    string vName = vLine.Substring(0, vLine.IndexOf(":"));
                    vName = vName.TrimStart('\t');
                    string vData = vLine.Substring(vLine.IndexOf(":") + 2, vLine.Length - vLine.IndexOf(":") - 2);
                    vData = vData.TrimEnd(',');

                    switch (vName)
                    {
                        case "ClockSpeed": vSettings.ClockSpeed = Convert.ToDouble(vData); break;
                        case "PeakTimeSL": vSettings.PeakTimeSL = Convert.ToDouble(vData); break;
                        case "PeakTimeF2": vSettings.PeakTimeF2 = Convert.ToDouble(vData); break;
                        case "PeakTimeF3": vSettings.PeakTimeF3 = Convert.ToDouble(vData); break;
                        case "HoldTimeSL": vSettings.HoldTimeSL = Convert.ToDouble(vData); break;
                        case "HoldTimeF2": vSettings.HoldTimeF2 = Convert.ToDouble(vData); break;
                        case "HoldTimeF3": vSettings.HoldTimeF3 = Convert.ToDouble(vData); break;
                        case "DGainSL": vSettings.DGainSL = Convert.ToDouble(vData); break;
                        case "DGainF2": vSettings.DGainF2 = Convert.ToDouble(vData); break;
                        case "DGainF3": vSettings.DGainF3 = Convert.ToDouble(vData); break;
                        case "ThreshSL": vSettings.ThreshSL = Convert.ToInt32(vData); break;
                        case "ThreshF2": vSettings.ThreshF2 = Convert.ToInt32(vData); break;
                        case "ThreshF3": vSettings.ThreshF3 = Convert.ToInt32(vData); break;
                        case "PPTC": vSettings.PPTC = Convert.ToDouble(vData); break;
                        case "PPGain": vSettings.PPGain = Convert.ToDouble(vData); break;
                        case "ZeroFactor": vSettings.ZeroFactor = Convert.ToInt32(vData); break;
                        case "RInhibit": vSettings.RInhibit = Convert.ToDouble(vData); break;
                        case "BLRMode": vSettings.BLRMode = Convert.ToInt32(vData); break;
                        case "BLRWindow": vSettings.BLRWindow = Convert.ToInt32(vData); break;
                        case "DTLength": vSettings.DTLength = Convert.ToInt32(vData); break;
                        case "RInterval": vSettings.RInterval = Convert.ToDouble(vData); break;
                        case "ChnOffset": vSettings.ChnOffset = Convert.ToInt32(vData); break;
                        case "SCAMode": vSettings.SCAMode = (eMXDPP50_SCAMode) Convert.ToInt32(vData); break;
                        case "SCALo1": vSettings.SCALo1 = Convert.ToInt32(vData); break;
                        case "SCALo2": vSettings.SCALo2 = Convert.ToInt32(vData); break;
                        case "SCALo3": vSettings.SCALo3 = Convert.ToInt32(vData); break;
                        case "SCALo4": vSettings.SCALo4 = Convert.ToInt32(vData); break;
                        case "SCALo5": vSettings.SCALo5 = Convert.ToInt32(vData); break;
                        case "SCALo6": vSettings.SCALo6 = Convert.ToInt32(vData); break;
                        case "SCALo7": vSettings.SCALo7 = Convert.ToInt32(vData); break;
                        case "SCALo8": vSettings.SCALo8 = Convert.ToInt32(vData); break;
                        case "SCAHi1": vSettings.SCAHi1 = Convert.ToInt32(vData); break;
                        case "SCAHi2": vSettings.SCAHi2 = Convert.ToInt32(vData); break;
                        case "SCAHi3": vSettings.SCAHi3 = Convert.ToInt32(vData); break;
                        case "SCAHi4": vSettings.SCAHi4 = Convert.ToInt32(vData); break;
                        case "SCAHi5": vSettings.SCAHi5 = Convert.ToInt32(vData); break;
                        case "SCAHi6": vSettings.SCAHi6 = Convert.ToInt32(vData); break;
                        case "SCAHi7": vSettings.SCAHi7 = Convert.ToInt32(vData); break;
                        case "SCAHi8": vSettings.SCAHi8 = Convert.ToInt32(vData); break;
                    }
                }
            }
            
            return vSettings;
        }

        /// <summary>
        /// Writes the DPP Settings to JSON formatted file.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <param name="vSettings">Settings to write to file.</param>
        /// <returns>None</returns>
        public void FileWriteSettingsJSON(string vFilename, MXDPP50_Settings vSettings)
        {
            string vStrJSON = "{\r\n";
            vStrJSON += "\tClockSpeed: " + vSettings.ClockSpeed.ToString() + ",\r\n";
            vStrJSON += "\tPeakTimeSL: " + vSettings.PeakTimeSL.ToString() + ",\r\n";
            vStrJSON += "\tPeakTimeF2: " + vSettings.PeakTimeF2.ToString() + ",\r\n";
            vStrJSON += "\tPeakTimeF3: " + vSettings.PeakTimeF3.ToString() + ",\r\n";
            vStrJSON += "\tHoldTimeSL: " + vSettings.HoldTimeSL.ToString() + ",\r\n";
            vStrJSON += "\tHoldTimeF2: " + vSettings.HoldTimeF2.ToString() + ",\r\n";
            vStrJSON += "\tHoldTimeF3: " + vSettings.HoldTimeF3.ToString() + ",\r\n";
            vStrJSON += "\tDGainSL: " + vSettings.DGainSL.ToString() + ",\r\n";
            vStrJSON += "\tDGainF2: " + vSettings.DGainF2.ToString() + ",\r\n";
            vStrJSON += "\tDGainF3: " + vSettings.DGainF3.ToString() + ",\r\n";
            vStrJSON += "\tThreshSL: " + vSettings.ThreshSL.ToString() + ",\r\n";
            vStrJSON += "\tThreshF2: " + vSettings.ThreshF2.ToString() + ",\r\n";
            vStrJSON += "\tThreshF3: " + vSettings.ThreshF3.ToString() + ",\r\n";
            vStrJSON += "\tPPTC: " + vSettings.PPTC.ToString() + ",\r\n";
            vStrJSON += "\tPPGain: " + vSettings.PPGain.ToString() + ",\r\n";
            vStrJSON += "\tZeroFactor: " + vSettings.ZeroFactor.ToString() + ",\r\n";
            vStrJSON += "\tRInhibit: " + vSettings.RInhibit.ToString() + ",\r\n";
            vStrJSON += "\tBLRMode: " + vSettings.BLRMode.ToString() + ",\r\n";
            vStrJSON += "\tBLRWindow: " + vSettings.BLRWindow.ToString() + ",\r\n";
            vStrJSON += "\tDTLength: " + vSettings.DTLength.ToString() + ",\r\n";
            vStrJSON += "\tRInterval: " + vSettings.RInterval.ToString() + ",\r\n";
            vStrJSON += "\tChnOffset: " + vSettings.ChnOffset.ToString() + ",\r\n";
            vStrJSON += "\tSCAMode: " + Convert.ToString((int)vSettings.SCAMode) + ",\r\n";
            vStrJSON += "\tSCALo1: " + vSettings.SCALo1.ToString() + ",\r\n";
            vStrJSON += "\tSCALo2: " + vSettings.SCALo2.ToString() + ",\r\n";
            vStrJSON += "\tSCALo3: " + vSettings.SCALo3.ToString() + ",\r\n";
            vStrJSON += "\tSCALo4: " + vSettings.SCALo4.ToString() + ",\r\n";
            vStrJSON += "\tSCALo5: " + vSettings.SCALo5.ToString() + ",\r\n";
            vStrJSON += "\tSCALo6: " + vSettings.SCALo6.ToString() + ",\r\n";
            vStrJSON += "\tSCALo7: " + vSettings.SCALo7.ToString() + ",\r\n";
            vStrJSON += "\tSCALo8: " + vSettings.SCALo8.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi1: " + vSettings.SCAHi1.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi2: " + vSettings.SCAHi2.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi3: " + vSettings.SCAHi3.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi4: " + vSettings.SCAHi4.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi5: " + vSettings.SCAHi5.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi6: " + vSettings.SCAHi6.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi7: " + vSettings.SCAHi7.ToString() + ",\r\n";
            vStrJSON += "\tSCAHi8: " + vSettings.SCAHi8.ToString() + "\r\n";
            vStrJSON += "}";

            File.WriteAllText(vFilename, vStrJSON);
        }

        /// <summary>
        /// Reads the DPP Hardware Settings from JSON formatted file.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <returns>hardware Settings read from file.</returns>
        public MXDPP50_Hardware FileReadHardwareJSON(string vFilename)
        {
            var vLines = File.ReadAllLines(vFilename);
            var vHardware = new MXDPP50_Hardware(gHardware.TCSupply, gHardware.TCPullup);

            foreach (string vLine in vLines)
            {
                if (vLine.Contains(":"))
                {
                    string vName = vLine.Substring(0, vLine.IndexOf(":"));
                    vName = vName.TrimStart('\t');
                    string vData = vLine.Substring(vLine.IndexOf(":") + 2, vLine.Length - vLine.IndexOf(":") - 2);
                    vData = vData.TrimEnd(',');

                    switch (vName)
                    {
                        case "TCSupply": vHardware.TCSupply = Convert.ToDouble(vData); break;
                        case "TCPullup": vHardware.TCPullup = Convert.ToDouble(vData); break;
                        case "TCMode": vHardware.TCMode = (eMXDPP50_TCMode) Convert.ToInt32(vData); break;
                        case "TCSet": vHardware.TCSet = Convert.ToDouble(vData); break;
                        case "HVPol": vHardware.HVPol = (eMXDPP50_Polarity) Convert.ToInt32(vData); break;
                        case "HVSet": vHardware.HVSet = Convert.ToDouble(vData); break;
                        case "SigPol": vHardware.SigPol = (eMXDPP50_Polarity) Convert.ToInt32(vData); break;
                        case "AuxOut1": vHardware.AuxOut1 = Convert.ToInt32(vData); break;
                        case "AuxOut2": vHardware.AuxOut2 = Convert.ToInt32(vData); break;
                        case "AnalogOut": vHardware.AnalogOut = (eMXDPP50_DACMode) Convert.ToInt32(vData); break;
                    }
                }
            }

            return vHardware;
        }

        /// <summary>
        /// Writes the DPP Hardware Settings to JSON formatted file.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <param name="vHardware">Hardware settings to write to file.</param>
        /// <returns>None</returns>
        public void FileWriteHardwareJSON(string vFilename, MXDPP50_Hardware vHardware)
        {
            string vStrJSON = "{\r\n";
            vStrJSON += "\tTCSupply: " + vHardware.TCSupply.ToString() + ",\r\n";
            vStrJSON += "\tTCPullup: " + vHardware.TCPullup.ToString() + ",\r\n";
            vStrJSON += "\tTCMode: " + Convert.ToString((int) vHardware.TCMode) + ",\r\n";
            vStrJSON += "\tTCSet: " + vHardware.TCSet.ToString() + ",\r\n";
            vStrJSON += "\tHVPol: " + Convert.ToString((int)vHardware.HVPol) + ",\r\n";
            vStrJSON += "\tHVSet: " + vHardware.HVSet.ToString() + ",\r\n";
            vStrJSON += "\tSigPol: " + Convert.ToString((int)vHardware.SigPol) + ",\r\n";
            vStrJSON += "\tAuxOut1: " + vHardware.AuxOut1.ToString() + ",\r\n";
            vStrJSON += "\tAuxOut2: " + vHardware.AuxOut2.ToString() + ",\r\n";
            vStrJSON += "\tAnalogOut: " + Convert.ToString((int)vHardware.AnalogOut) + "\r\n";
            vStrJSON += "}";

            File.WriteAllText(vFilename, vStrJSON);
        }

        /// <summary>
        /// Reads the DPP Presets from JSON formatted file.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <returns>Presets read from file.</returns>
        public MXDPP50_Presets FileReadPresetsJSON(string vFilename)
        {       
            var vLines = File.ReadAllLines(vFilename);
            var vPresets = new MXDPP50_Presets();

            foreach (string vLine in vLines)
            {
                if (vLine.Contains(":"))
                {
                    string vName = vLine.Substring(0, vLine.IndexOf(":"));
                    vName = vName.TrimStart('\t');
                    string vData = vLine.Substring(vLine.IndexOf(":") + 2, vLine.Length - vLine.IndexOf(":") - 2);
                    vData = vData.TrimEnd(',');

                    switch (vName)
                    {
                        case "TimerMode": vPresets.TimerMode = (eMXDPP50_TimerMode) Convert.ToInt32(vData); break;
                        case "Timer": vPresets.Timer = Convert.ToUInt32(vData); break;
                        case "TotalCounts": vPresets.TotalCounts = Convert.ToUInt32(vData); break;
                        case "PeakCounts": vPresets.PeakCounts = Convert.ToUInt32(vData); break;
                        case "ROICounts": vPresets.ROICounts = Convert.ToUInt32(vData); break;
                        case "ROIHiCHN": vPresets.ROIHiCHN = Convert.ToInt32(vData); break;
                        case "ROILoCHN": vPresets.ROILoCHN = Convert.ToInt32(vData); break;
                    }
                }
            }

            return vPresets;
        }

        /// <summary>
        /// Writes the DPP Presets to JSON formatted file.
        /// </summary>
        /// <param name="vFilename">Filename of the file to read from.</param>
        /// <param name="vHardware">Preset settings to write to file.</param>
        /// <returns>None</returns>
        public void FileWritePresetsJSON(string vFilename, MXDPP50_Presets vPresets)
        {
            string vStrJSON = "{\r\n";
            vStrJSON += "\tTimerMode: " + (int)vPresets.TimerMode + ",\r\n";
            vStrJSON += "\tTimer: " + vPresets.Timer.ToString() + ",\r\n";
            vStrJSON += "\tTotalCounts: " + vPresets.TotalCounts.ToString() + ",\r\n";
            vStrJSON += "\tPeakCounts: " + vPresets.PeakCounts.ToString() + ",\r\n";
            vStrJSON += "\tROICounts: " + vPresets.ROICounts.ToString() + ",\r\n";
            vStrJSON += "\tROIHiCHN: " + vPresets.ROIHiCHN.ToString() + ",\r\n";
            vStrJSON += "\tROILoCHN: " + vPresets.ROILoCHN.ToString() + "\r\n";
            vStrJSON += "}";

            File.WriteAllText(vFilename, vStrJSON);
        }

        #endregion

        #region "Public Async Commands"

        /// <summary>
        /// Asynchronously reads the settings from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Settings class that contains the settings read from the DPP.</returns>
        public Task<MXDPP50_Settings> ReadSettingsAsync()
        {
            return Task.Factory.StartNew(() => ReadSettings());
        }

        /// <summary>
        /// Asynchonously writes the settings to the DPP and then reads them back.
        /// </summary>
        /// <param name="vOptSettings">Optional settings to write to the DPP.  If not provided it will write the Settings in the class property.</param>
        /// <returns>Returns a MXDPP50_Settings class that contains the settings read from the DPP.</returns>
        public Task<MXDPP50_Settings> WriteSettingsAsync(MXDPP50_Settings vOptSettings = null)
        {
            return Task.Factory.StartNew(() => WriteSettings(vOptSettings));
        }

        /// <summary>
        /// Asynchronously reads the hardware from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Hardware class that contains the hardware values read from the DPP.</returns>
        public Task<MXDPP50_Hardware> ReadHardwareAsync()
        {
            return Task.Factory.StartNew(() => ReadHardware());
        }

        /// <summary>
        /// Asynchonously write the hardware settings to the DPP and then reads them back.
        /// </summary>
        /// <param name="vHardware">Optional hardware settings to write to the DPP.  If not provided it will write the hardware settings in the class property.</param>
        /// <returns>Returns a MXDPP50_Hardware class that contains the hardware settings read from the DPP.</returns>
        public Task<MXDPP50_Hardware> WriteHardwareAsync(MXDPP50_Hardware vOptHardware = null)
        {
            return Task.Factory.StartNew(() => WriteHardware(vOptHardware));
        }

        /// <summary>
        /// Asynchronously reads the statistics from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Statistics class that contains the statistics read from the DPP.</returns>
        public Task<MXDPP50_Statistics> ReadStatisticsAsync()
        {
            return Task.Factory.StartNew(() => ReadStatistics());
        }

        /// <summary>
        /// Asynchronously reads the presets from the DPP.
        /// </summary>
        /// <returns>Returns a MXDPP50_Presets class that contains the presets read from the DPP.</returns>
        public Task<MXDPP50_Presets> ReadPresetsAsync()
        {
            return Task.Factory.StartNew(() => ReadPresets());
        }

        /// <summary>
        /// Asynchronously write the presets to the DPP and then reads them back.
        /// </summary>
        /// <param name="vPresets">Optional presets to write to the DPP.  If not provided it will write the presets in the class property.</param>
        /// <returns>Returns a MXDPP50_Presets class that contains the presets read from the DPP.</returns>
        public Task<MXDPP50_Presets> WritePresetsAsync(MXDPP50_Presets vOptPresets = null)
        {
            return Task.Factory.StartNew(() => WritePresets(vOptPresets));
        }

        /// <summary>
        /// Asynchronously reads Spectrum from the DPP.
        /// </summary>
        /// <returns>uint array containing the downloaded spectrum.</returns>
        public Task<uint[]> ReadSpectrumAsync()
        {
            return Task.Factory.StartNew(() => ReadSpectrum());
        }

        /// <summary>
        /// Asynchronously writes the DPP Settings, Hardware and Presets to the DPPs memory so they are loaded when the DPP is powered up.
        /// </summary>
        /// <param name="vSettings">Settings to write to memory.</param>
        /// <param name="vPresets">Presets to write to memory.</param>
        /// <param name="vHardware">Hardware to write to memory.  T
        /// <returns>The number of bytes written to the DPP memory</returns>
        public Task<int> WriteDefaultsToMemoryAsync(MXDPP50_Settings vSettings, MXDPP50_Hardware vHardware, MXDPP50_Presets vPresets)
        {
            return Task.Factory.StartNew(() => WriteDefaultsToMemory(vSettings, vHardware, vPresets));
        }
        #endregion

        #region "Private Read Commands"        
        private void ReadClockSpeed()
        {
            CommWriteStr("CD\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ClockSpeed_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadClockSpeed - Returned -1");
        }
        private void ReadPeakTimeSL()
        {
            CommWriteStr("PS\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PeakTimeSL_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadPeakTimeSL - Returned -1");
        }
        private void ReadPeakTimeF2()
        {
            CommWriteStr("PX\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PeakTimeF2_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadTimeF2 - Returned -1");
        }
        private void ReadPeakTimeF3()
        {
            CommWriteStr("PY\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PeakTimeF3_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadPeakTimeF3 - Returned -1");
        }
        private void ReadHoldTimeSL()
        {
            CommWriteStr("HS\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.HoldTimeSL_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadHoldTimeSL - Returned -1");
        }
        private void ReadHoldTimeF2()
        {
            CommWriteStr("HX\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.HoldTimeF2_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadHoldTimeF2 - Returned -1");
        }
        private void ReadHoldTimeF3()
        {
            CommWriteStr("HY\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.HoldTimeF3_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadHoldTimeF3 - Returned -1");
        }
        private void ReadDGainSL()
        {
            CommWriteStr("GS\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DGainSL_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadDGainSL - Returned -1");
        }
        private void ReadDGainF2()
        {
            CommWriteStr("GX\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DGainF2_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadGainF2 - Returned -1");
        }
        private void ReadDGainF3()
        {
            CommWriteStr("GY\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DGainF3_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadGainF3 - Returned -1");
        }
        private void ReadThreshSL()
        {
            CommWriteStr("TS\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ThreshSL = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadThreshSL - Returned -1");
        }
        private void ReadThreshF2()
        {
            CommWriteStr("TX\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ThreshF2 = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadThreshF2 - Returned -1");
        }
        private void ReadThreshF3()
        {
            CommWriteStr("TY\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ThreshF3 = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadThreshF3 - Returned -1");
        }
        private void ReadPPTC()
        {
            CommWriteStr("PT\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PPTC_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadPPTC - Returned -1");
        }
        private void ReadPPGain()
        {
            CommWriteStr("PG\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PPGain_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadPPGain - Returned -1");
        }
        private void ReadEQFactor()
        {
            CommWriteStr("EF\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.EQFactor = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadEQFactor - Returned -1");
        }
        private void ReadZeroFactor()
        {
            CommWriteStr("ZF\r");
            string vRet = CommReadStr();
            gSettings.ZeroFactor = Convert.ToInt32(vRet);
        }
        private void ReadRInhibit()
        {
            CommWriteStr("IL\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.RInhibit_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadRInhbit - Returned -1");
        }
        private void ReadBLRMode()
        {
            CommWriteStr("BM\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.BLRMode = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadBLRMode - Returned -1");
        }
        private void ReadBLRWindow()
        {
            CommWriteStr("BW\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.BLRWindow = Convert.ToInt32(vRet);
            }
            else throw new Exception("ReadBLRWindow - Returned -1");
        }
        private void ReadDTLength()
        {
            CommWriteStr("DL\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DTLength = Convert.ToInt32(vRet);
            }
            else throw new Exception("DTLength - Returned -1");
        }
        private void ReadRInterval()
        {
            CommWriteStr("RI\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.RInterval_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("RInterval - Returned -1");
        }
        private void ReadChnOffset()
        {
            CommWriteStr("CO\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ChnOffset = Convert.ToInt32(vRet) * (1 / 16);
            }
            else throw new Exception("ChnOffset - Returned -1");
        }

        private void ReadAcquisitionInProgress()
        {
            CommWriteStr("AP\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.AcquisitionInProgress = Convert.ToBoolean(Convert.ToInt32(vRet));
            }
            else throw new Exception("AcquisitionInProgress - Returned -1");
        }
        private void ReadTimeReal()
        {
            CommWriteStr("RT\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.TimeReal = Convert.ToDouble(vRet) / 1000;
            }
            else throw new Exception("TimeReal - Returned -1");
        }
        private void ReadTimeLive()
        {
            CommWriteStr("LT\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.TimeLive = Convert.ToDouble(vRet) / 1000;
            }
            else throw new Exception("TimeLive - Returned -1");
        }
        private void ReadTimeDead()
        {
            CommWriteStr("DR\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.TimeDead = Convert.ToDouble(vRet) / 1000;
            }
            else throw new Exception("TimeDead - Returned -1");
        }
        private void ReadRateInput()
        {
            CommWriteStr("IR\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.RateInput = Convert.ToUInt32(vRet);
            }
            else throw new Exception("RateInput - Returned -1");
        }
        private void ReadRateOutput()
        {
            CommWriteStr("OR\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.RateOutput = Convert.ToUInt32(vRet);
            }
            else throw new Exception("RateOutput - Returned -1");
        }
        private void ReadRateCorrected()
        {
            CommWriteStr("RM\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.RateCorrected = Convert.ToUInt32(vRet);
            }
            else throw new Exception("RateCorrected - Returned -1");
        }
        private void ReadRateSlow()
        {
            CommWriteStr("RS\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.RateSlow = Convert.ToUInt32(vRet);
            }
            else throw new Exception("RateSlow - Returned -1");
        }
        private void ReadRateFast2()
        {
            CommWriteStr("RX\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.RateFast2 = Convert.ToUInt32(vRet);
            }
            else throw new Exception("RateFast2 - Returned -1");
        }
        private void ReadRateFast3()
        {
            CommWriteStr("RY\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gStatistics.RateFast3 = Convert.ToUInt32(vRet);
            }
            else throw new Exception("RateFast3 - Returned -1");
        }
        private void ReadDPPTemp()
        {
            CommWriteStr("#A\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.DPPTemp_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("DPPTemp - Returned -1");
        }
        private void ReadTCMon()
        {
            CommWriteStr("#B\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.TCMon_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("TCMon - Returned -1");
        }
        private void ReadTCTecMon()
        {
            CommWriteStr("#C\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.TCTecMon_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("TCTecMon - Returned -1");
        }
        private void ReadDigInputs()
        {
            CommWriteStr("PI\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.DigInputs_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("DigInputs - Returned -1");
        }
        private void ReadHVMon()
        {
            CommWriteStr("#D\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.HVMon_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("HVMon - Returned -1");
        }
        private void ReadDigOutputs()
        {
            CommWriteStr("PO\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.DigOutputs_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("DigOutputs - Returned -1");
        }
        private void ReadHVSet()
        {
            CommWriteStr("&A\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.HVSet_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("HVSet - Returned -1");
        }
        private void ReadTCSet()
        {
            CommWriteStr("&B\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.TCSet_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("TCSet - Returned -1");
        }
        private void ReadAnalogOut()
        {
            CommWriteStr("DM\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.AnalogOut = (eMXDPP50_DACMode)Convert.ToInt32(vRet);
            }
            else throw new Exception("AnalogOut - Returned -1");
        }

        private void ReadSCAMode()
        {
            CommWriteStr("RD\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.SCAMode = (eMXDPP50_SCAMode) Convert.ToInt32(vRet);
            }
            else throw new Exception("SCAMode - Returned -1");
        }
        private void ReadSCALo(int vChannel)
        {
            string vCMD = "";

            switch (vChannel)
            {
                case 1: vCMD = "_A"; break;
                case 2: vCMD = "_B"; break;
                case 3: vCMD = "_C"; break;
                case 4: vCMD = "_D"; break;
                case 5: vCMD = "_E"; break;
                case 6: vCMD = "_F"; break;
                case 7: vCMD = "_G"; break;
                case 8: vCMD = "_H"; break;
            }

            CommWriteStr(vCMD + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                switch (vChannel)
                {
                    case 1: gSettings.SCALo1 = Convert.ToInt32(vRet); break;
                    case 2: gSettings.SCALo2 = Convert.ToInt32(vRet); break;
                    case 3: gSettings.SCALo3 = Convert.ToInt32(vRet); break;
                    case 4: gSettings.SCALo4 = Convert.ToInt32(vRet); break;
                    case 5: gSettings.SCALo5 = Convert.ToInt32(vRet); break;
                    case 6: gSettings.SCALo6 = Convert.ToInt32(vRet); break;
                    case 7: gSettings.SCALo7 = Convert.ToInt32(vRet); break;
                    case 8: gSettings.SCALo8 = Convert.ToInt32(vRet); ; break;
                }
            }
            else throw new Exception("SCALo - Returned -1");
        }
        private void ReadSCAHi(int vChannel)
        {
            string vCMD = "";

            switch (vChannel)
            {
                case 1: vCMD = "^A"; break;
                case 2: vCMD = "^B"; break;
                case 3: vCMD = "^C"; break;
                case 4: vCMD = "^D"; break;
                case 5: vCMD = "^E"; break;
                case 6: vCMD = "^F"; break;
                case 7: vCMD = "^G"; break;
                case 8: vCMD = "^H"; break;
            }

            CommWriteStr(vCMD + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                switch (vChannel)
                {
                    case 1: gSettings.SCAHi1 = Convert.ToInt32(vRet); break;
                    case 2: gSettings.SCAHi2 = Convert.ToInt32(vRet); break;
                    case 3: gSettings.SCAHi3 = Convert.ToInt32(vRet); break;
                    case 4: gSettings.SCAHi4 = Convert.ToInt32(vRet); break;
                    case 5: gSettings.SCAHi5 = Convert.ToInt32(vRet); break;
                    case 6: gSettings.SCAHi6 = Convert.ToInt32(vRet); break;
                    case 7: gSettings.SCAHi7 = Convert.ToInt32(vRet); break;
                    case 8: gSettings.SCAHi8 = Convert.ToInt32(vRet); ; break;
                }
            }
            else throw new Exception("SCAHi - Returned -1");
        }

        private void ReadPresetTimerMode()
        {
            CommWriteStr("TM\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.TimerMode = (eMXDPP50_TimerMode) Convert.ToInt32(vRet);
            }
            else throw new Exception("TimerMode - Returned -1");
        }
        private void ReadPresetTimer()
        {
            CommWriteStr("PR\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.Timer = Convert.ToUInt32(vRet);
            }
            else throw new Exception("PresetTimer - Returned -1");
        }
        private void ReadPresetTotalCounts()
        {
            CommWriteStr("TC\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.TotalCounts = Convert.ToUInt32(vRet);
            }
            else throw new Exception("TotalCounts - Returned -1");
        }
        private void ReadPresetPeakCounts()
        {
            CommWriteStr("PC\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.PeakCounts = Convert.ToUInt32(vRet);
            }
            else throw new Exception("PeakCounts - Returned -1");
        }
        private void ReadPresetROICounts()
        {
            CommWriteStr("RC\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.ROICounts = Convert.ToUInt32(vRet);
            }
            else throw new Exception("ROICounts - Returned -1");
        }
        private void ReadPresetROIHi()
        {
            CommWriteStr("WH\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.ROIHiCHN = Convert.ToInt32(vRet);
            }
            else throw new Exception("ROIHiCHN - Returned -1");
        }
        private void ReadPresetROILo()
        {
            CommWriteStr("WL\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.ROILoCHN = Convert.ToInt32(vRet);
            }
            else throw new Exception("ROILoCHN - Returned -1");
        }

        private void ReadSpectrumData()
        {
            CommWriteStr("DC\r");
            byte[] vRet = CommReadBytes();
            if (vRet.Length >= 16384)
            {
                // Convert byte array to uint32 array
                Buffer.BlockCopy(vRet, 0, gSpectrum, 0, vRet.Length);
            }
            else throw new Exception("SpectrumData - Incorrect byte length");
        }

        #endregion

        #region "Private Write Commands"
        private void WriteClockSpeed(int vValue)
        {
            CommWriteStr("CD" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ClockSpeed_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteClockSpeed - Returned -1");
        }
        private void WritePeakTimeSL(int vValue)
        {
            CommWriteStr("PS" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PeakTimeSL_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WritePeakTimeSL - Returned -1");
        }
        private void WritePeakTimeF2(int vValue)
        {
            CommWriteStr("PX" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PeakTimeF2_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WritePeakTimeF2 - Returned -1");
        }
        private void WritePeakTimeF3(int vValue)
        {
            CommWriteStr("PY" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PeakTimeF3_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WritePeakTimeF3 - Returned -1");
        }
        private void WriteHoldTimeSL(int vValue)
        {
            CommWriteStr("HS" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.HoldTimeSL_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteHoldTimeSL - Returned -1");
        }
        private void WriteHoldTimeF2(int vValue)
        {
            CommWriteStr("HX" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.HoldTimeF2_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteHoldTimeF2 - Returned -1");
        }
        private void WriteHoldTimeF3(int vValue)
        {
            CommWriteStr("HY" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.HoldTimeF3_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteHoldTimeF3 - Returned -1");
        }
        private void WriteDGainSL(int vValue)
        {
            CommWriteStr("GS" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DGainSL_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteDGainSL - Returned -1");
        }
        private void WriteDGainF2(int vValue)
        {
            CommWriteStr("GX" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DGainF2_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteGainF2 - Returned -1");
        }
        private void WriteDGainF3(int vValue)
        {
            CommWriteStr("GY" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DGainF3_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteGainF3 - Returned -1");
        }
        private void WriteThreshSL(int vValue)
        {
            CommWriteStr("TS" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ThreshSL = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteThreshSL - Returned -1");
        }
        private void WriteThreshF2(int vValue)
        {
            CommWriteStr("TX" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ThreshF2 = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteThreshF2 - Returned -1");
        }
        private void WriteThreshF3(int vValue)
        {
            CommWriteStr("TY" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ThreshF3 = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteThreshF3 - Returned -1");
        }
        private void WritePPTC(int vValue)
        {
            CommWriteStr("PT" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PPTC_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WritePPTC - Returned -1");
        }
        private void WritePPGain(int vValue)
        {
            CommWriteStr("PG" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.PPGain_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WritePPGain - Returned -1");
        }
        private void WriteEQFactor(int vValue)
        {
            CommWriteStr("EF" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.EQFactor = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteEQFactor - Returned -1");
        }
        private void WriteZeroFactor(int vValue)
        {
            CommWriteStr("ZF" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            gSettings.ZeroFactor = Convert.ToInt32(vRet);
        }
        private void WriteRInhibit(int vValue)
        {
            CommWriteStr("IL" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.RInhibit_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteRInhbit - Returned -1");
        }
        private void WriteBLRMode(int vValue)
        {
            CommWriteStr("BM" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.BLRMode = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteBLRMode - Returned -1");
        }
        private void WriteBLRWindow(int vValue)
        {
            CommWriteStr("BW" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.BLRWindow = Convert.ToInt32(vRet);
            }
            else throw new Exception("WriteBLRWindow - Returned -1");
        }
        private void WriteDTLength(int vValue)
        {
            CommWriteStr("DL" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.DTLength = Convert.ToInt32(vRet);
            }
            else throw new Exception("DTLength - Returned -1");
        }
        private void WriteRInterval(int vValue)
        {
            CommWriteStr("RI" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.RInterval_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("RInterval - Returned -1");
        }
        private void WriteChnOffset(int vValue)
        {
            CommWriteStr("CO" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.ChnOffset = Convert.ToInt32(vRet);
            }
            else throw new Exception("ChnOffset - Returned -1");
        }
        private void WriteDigOutputs(int vValue)
        {
            CommWriteStr("PO" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.DigOutputs_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("DigOutputs - Returned -1");
        }
        private void WriteHVSet(int vValue)
        {
            CommWriteStr("&A" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.HVSet_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("HVSet - Returned -1");
        }
        private void WriteTCSet(int vValue)
        {
            CommWriteStr("&B" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.TCSet_Int = Convert.ToInt32(vRet);
            }
            else throw new Exception("TCSet - Returned -1");
        }
        private void WriteAnalogOut(int vValue)
        {
            CommWriteStr("DM" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gHardware.AnalogOut = (eMXDPP50_DACMode)Convert.ToInt32(vRet);
            }
            else throw new Exception("AnalogOut - Returned -1");
        }

        private void WriteSCAMode(int vValue)
        {
            CommWriteStr("RD" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gSettings.SCAMode = (eMXDPP50_SCAMode) Convert.ToInt32(vRet);
            }
            else throw new Exception("SCAMode - Returned -1");
        }
        private void WriteSCALo(int vChannel, int vValue)
        {
            string vCMD = "";

            switch (vChannel)
            {
                case 1: vCMD = "_A"; break;
                case 2: vCMD = "_B"; break;
                case 3: vCMD = "_C"; break;
                case 4: vCMD = "_D"; break;
                case 5: vCMD = "_E"; break;
                case 6: vCMD = "_F"; break;
                case 7: vCMD = "_G"; break;
                case 8: vCMD = "_H"; break;
            }

            CommWriteStr(vCMD + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                switch (vChannel)
                {
                    case 1: gSettings.SCALo1 = Convert.ToInt32(vRet); break;
                    case 2: gSettings.SCALo2 = Convert.ToInt32(vRet); break;
                    case 3: gSettings.SCALo3 = Convert.ToInt32(vRet); break;
                    case 4: gSettings.SCALo4 = Convert.ToInt32(vRet); break;
                    case 5: gSettings.SCALo5 = Convert.ToInt32(vRet); break;
                    case 6: gSettings.SCALo6 = Convert.ToInt32(vRet); break;
                    case 7: gSettings.SCALo7 = Convert.ToInt32(vRet); break;
                    case 8: gSettings.SCALo8 = Convert.ToInt32(vRet); ; break;
                }
            }
            else throw new Exception("SCALo - Returned -1");
        }
        private void WriteSCAHi(int vChannel, int vValue)
        {
            string vCMD = "";

            switch (vChannel)
            {
                case 1: vCMD = "^A"; break;
                case 2: vCMD = "^B"; break;
                case 3: vCMD = "^C"; break;
                case 4: vCMD = "^D"; break;
                case 5: vCMD = "^E"; break;
                case 6: vCMD = "^F"; break;
                case 7: vCMD = "^G"; break;
                case 8: vCMD = "^H"; break;
            }

            CommWriteStr(vCMD + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                switch (vChannel)
                {
                    case 1: gSettings.SCAHi1 = Convert.ToInt32(vRet); break;
                    case 2: gSettings.SCAHi2 = Convert.ToInt32(vRet); break;
                    case 3: gSettings.SCAHi3 = Convert.ToInt32(vRet); break;
                    case 4: gSettings.SCAHi4 = Convert.ToInt32(vRet); break;
                    case 5: gSettings.SCAHi5 = Convert.ToInt32(vRet); break;
                    case 6: gSettings.SCAHi6 = Convert.ToInt32(vRet); break;
                    case 7: gSettings.SCAHi7 = Convert.ToInt32(vRet); break;
                    case 8: gSettings.SCAHi8 = Convert.ToInt32(vRet); ; break;
                }
            }
            else throw new Exception("SCAHi - Returned -1");
        }

        private void WritePresetTimerMode(int vValue)
        {
            CommWriteStr("TM" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.TimerMode = (eMXDPP50_TimerMode)Convert.ToInt32(vRet);
            }
            else throw new Exception("TimerMode - Returned -1");
        }
        private void WritePresetTimer(uint vValue)
        {
            CommWriteStr("PR" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.Timer = Convert.ToUInt32(vRet);
            }
            else throw new Exception("PresetTimer - Returned -1");
        }
        private void WritePresetTotalCounts(uint vValue)
        {
            CommWriteStr("TC" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.TotalCounts = Convert.ToUInt32(vRet);
            }
            else throw new Exception("TotalCounts - Returned -1");
        }
        private void WritePresetPeakCounts(uint vValue)
        {
            CommWriteStr("PC" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.PeakCounts = Convert.ToUInt32(vRet);
            }
            else throw new Exception("PeakCounts - Returned -1");
        }
        private void WritePresetROICounts(uint vValue)
        {
            CommWriteStr("RC" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.ROICounts = Convert.ToUInt32(vRet);
            }
            else throw new Exception("ROICounts - Returned -1");
        }
        private void WritePresetROIHi(int vValue)
        {
            CommWriteStr("WH" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.ROIHiCHN = Convert.ToInt32(vRet);
            }
            else throw new Exception("ROIHiCHN - Returned -1");
        }
        private void WritePresetROILo(int vValue)
        {
            CommWriteStr("WL" + vValue.ToString() + "\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                gPresets.ROILoCHN = Convert.ToInt32(vRet);
            }
            else throw new Exception("ROILoCHN - Returned -1");
        }

        private void SendStart()
        {
            CommWriteStr("GO\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                // Do Nothing
            }
            else throw new Exception("Start - Returned -1");
        }
        private void SendStop()
        {
            CommWriteStr("SP\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                // Do Nothing
            }
            else throw new Exception("Start - Returned -1");
        }
        private void SendClear()
        {
            CommWriteStr("CL\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                // Do Nothing
            }
            else throw new Exception("Start - Returned -1");
        }

        private void SendMemoryStart()
        {
            CommWriteStr("EW\r");
        }
        private int SendMemoryEnd()
        {
            CommWriteStr("`\r");
            string vRet = CommReadStr();
            if (vRet != "-1")
            {
                return Convert.ToInt32(vRet);
            }
            else throw new Exception("MemoryEnd - Returned -1");
        }
        #endregion

        #region "Class Disposal"
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool vDisposing)
        {
            if (gDisposed) return;

            if (vDisposing)
            {
                gHandle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            try { Disconnect(); } catch { }
            gDisposed = true;
        }
        ~MXDPP50()
        {
            Dispose(false);
        }
        #endregion

    }
}

