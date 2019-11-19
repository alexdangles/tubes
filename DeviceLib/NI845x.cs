/* Note: Before building ni8452.dll with LabVIEW, right click the dll in build specifications and select clean,
 * or alternatively delete the existing ni8452.dll file before building new one.
 */

using System;
using System.Data;
using System.Runtime.InteropServices;

namespace Devices
{
    /// <summary>
    /// National Instruments SPI/I2C digital controller class.
    /// </summary>
    public class NI845x
    {
        public static string[] registerNames =
        {
            "Faulted",
            "Tube Ready",
            "HVPG",
            "HV On",
            "Interlock",
            "Overshoot",
            "Time Out Fault",
            "Temp Fault",
            "HV Fault",
            "Unstable Fault",
            "Interlock Fault",
            "Booted Fault",
            "HV Enabled",
            "Watch Dog Enabled",
            "Set Fit Adjust Enable",
            "Monitor Fit Adjust Enable",
            "Protocal Revision",
            "Hardware Revision",
            "Firmware Revision",
            "Part Number",
            "Tube Serial Number",
            "Power Serial Number",
            "Anode Thickness (um)",
            "Present Temp",
            "Max Recorded Temp",
            "Min Recorded Temp",
            "Max Temp Limit",
            "Min Temp Limit",
            "Max Watts",
            "Number of DAC Bits",
            "Number of ADC Bits",
            "kV Scaling Factor",
            "Anode Current Scaling Factor",
            "Set kV",
            "Set Anode Current",
            "kV Monitor",
            "Anode Current Monitor",
            "HV Overshoot Limit",
            "HV Ripple Limit",
            "HV Settling Time",
            "Anode Current Overshoot Limit",
            "Anode Current Ripple Limit",
            "Anode Current Settling Time",
            "HV Runtime Second",
            "uA Seconds",
            "kV Seconds",
            "Input Current Monitor",
            "I2C Watch Dog (sec)",
            "I2C Transaction Period (sec)",
            "kV Upper Clamp Limit",
            "uA Upper Clamp Limit",
            "Input Voltage",
            "Boot CNT",
            "HV Enable CNT",
            "HV Fault Enable",
            "Monitor Average CNT",
            "uA Turn On Delay",
            "HV Ramp Rate",
            "uA Ramp Rate",
            "kV Ripple",
            "uA Ripple",
            "kV Offset",
            "uA Offset",
            "Interlock CNT",
            "Unstable CNT",
            "HV Fault",
            "Over Temp CNT",
            "Under Temp CNT",
            "Watch Dog CNT",
            "kV Lower Clamp Limit",
            "uA Lower Clamp Limit",
            "Overshoot CNT",
            "Min Voltage",
            "Min Current",
            "Max Voltage",
            "Max Current",
            "Power Flag",
            "Min PS Voltage Limit",
            "Max PS Voltage Limit",
            "Min PS Current Limit",
            "Max PS Current Limit",
            "Over Range CNT",
            "Power Down"
        };

        public static DataTable GetRegisters()
        {
            DataTable reg = new DataTable();
            reg.Columns.Add("Register");
            reg.Columns.Add("Value");
            foreach (string names in registerNames)
            {
                reg.Rows.Add(names);
            }
            foreach (DataRow row in reg.Rows)
            {
                row["Value"] = 0;
            }
            return reg;
        }

        /// <summary>
        /// Closes the device reference in NIMAX.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CloseReference(string deviceName);
        
        /// <summary>
        /// Read digital HIGH(1) or LOW(0) from a DIO line.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="lineNumber">DIO number to read from</param>
        /// <returns>Returns 0 or 1</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DIOReadLine(string deviceName, byte lineNumber);
        
        /// <summary>
        /// Write digital HIGH(1) or LOW(0) to a DIO line.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="lineNumber">DIO number to write to</param>
        /// <param name="writeValue">0 or 1</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DIOWriteLine(string deviceName, byte lineNumber, byte writeValue);
       
        /// <summary>
        /// Monitor output energy and beam current for digital SPI tube.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="kVScaler">kV Scaler parameter from tube settings</param>
        /// <param name="uAScaler">uA Scaler parameter from tube settings</param>
        /// <param name="monitors_OUTPUT">Output array containing set kV, set uA, monitor kV, monitor uA</param>
        /// <param name="monitors_LENGTH">Length of monitors array</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SPIMon(string deviceName, double[] monitors_OUTPUT, int monitors_LENGTH);
        
        /// <summary>
        /// Set output energy and beam current for digital SPI tube.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="setkV">Set the energy of the tube</param>
        /// <param name="setuA">Set the beam current of the tube</param>
        /// <param name="kVScaler">kV Scaler parameter from tube settings</param>
        /// <param name="uAScaler">uA Scaler parameter from tube settings</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SPISet(string deviceName, double setkV, double setuA);
        
        /// <summary>
        /// Read all registers from digital I2C tube.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="adminByte">The key byte for granting full access to all registers</param>
        /// <param name="registers_OUTPUT">Output array of registers</param>
        /// <param name="registers_LENGTH">Length of registers array</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CReadAll(string deviceName, byte adminByte, double[] registers_OUTPUT, int registers_LENGTH);
        
        /// <summary>
        /// Enable X-rays for digital I2C tube.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="hvEnable">Enable switch</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CEnable(string deviceName, bool hvEnable);
        
        /// <summary>
        /// Monitor output energy and beam current from digital I2C tube
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="kVScalingFactor">kV Scaling Factor parameter from registers (I2CReadAll must be done to get this)</param>
        /// <param name="uAScalingFactor">uA Scaling Factor parameter from registers (I2CReadAll must be done to get this)</param>
        /// <param name="numDACBits">Number of DAC bits (I2CReadAll must be done to get this)</param>
        /// <param name="numADCBits">Number of ADC bits (I2CReadAll must be done to get this)</param>
        /// <param name="monitors_OUTPUT">Output array containing set kV, set uA, monitor kV, monitor uA</param>
        /// <param name="monitors_LENGTH">Length of monitor array</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CMon(string deviceName, byte kVScalingFactor, byte uAScalingFactor, byte numDACBits, byte numADCBits, double[] monitors_OUTPUT, int monitors_LENGTH);
        
        /// <summary>
        /// Set energy and beam current of digital I2C tube.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="kVScalingFactor">kV Scaling Factor parameter from registers (I2CReadAll must be done to get this)</param>
        /// <param name="uAScalingFactor">uA Scaling Factor parameter from registers (I2CReadAll must be done to get this)</param>
        /// <param name="numDACBits">Number of DAC bits (I2CReadAll must be done to get this)</param>
        /// <param name="setkV">Set the energy of the tube</param>
        /// <param name="setuA">Set the beam current of the tube</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CSet(string deviceName, byte kVScalingFactor, byte uAScalingFactor, byte numDACBits, double setkV, double setuA);
        
        /// <summary>
        /// Calibrate digital I2C tube with array of keV edge measurements from output spectra
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="adminByte">The key byte for granting full access to all registers</param>
        /// <param name="method">Calibrate or save/delete from I2C tube. Takes parameters: "Calibrate", "Kill Cal" or "Save EEPROM"</param>
        /// <param name="keVEdge">Input keV edge array to calibrate with</param>
        /// <param name="keVEdge_LENGTH">Length of keV edge array</param>
        /// <param name="keVFit_OUTPUT">Output array of keV fit data</param>
        /// <param name="keVFitByte_OUTPUT">Output array of keV fit bytes</param>
        /// <param name="keVSetFitByte_OUTPUT">Output array of keV set fit bytes</param>
        /// <param name="keVMonFitByte_OUTPUT">Output array of keV mon fit bytes</param>
        /// <param name="page1keVAddresses_OUTPUT">Output array of page 1 keV addresses</param>
        /// <param name="page1SetAddresses_OUTPUT">Output array of page 1 set keV addresses</param>
        /// <param name="page1MonAddresses_OUTPUT">Output array of page 1 mon keV addresses</param>
        /// <param name="page1Addresses_LENGTH">Length of each output array (all the same length)</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CCalibrate(string deviceName, byte adminByte, double[] keVEdge, int keVEdge_LENGTH, int[] keVFit_OUTPUT, int[] keVFitByte_OUTPUT, int[] keVSetFitByte_OUTPUT, int[] keVMonFitByte_OUTPUT, byte[] page1keVAddresses_OUTPUT, byte[] page1SetAddresses_OUTPUT, byte[] page1MonAddresses_OUTPUT, int page1Addresses_LENGTH);

        /// <summary>
        /// Delete stored calibration settings.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <returns></returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CDeleteCalibration(string deviceName);

        /// <summary>
        /// Save EEPROM on device
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="adminByte">The key byte for granting full access to all registers</param>
        /// <returns></returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CSaveEEPROM(string deviceName, byte adminByte);

        /// <summary>
        /// Write an array of bytes to digital I2C tube and read back output byte array.
        /// </summary>
        /// <param name="deviceName">Name of the SPI/I2C device in NIMAX</param>
        /// <param name="adminByte">The key byte for granting full access to all registers</param>
        /// <param name="bytesIn">Input byte array to write</param>
        /// <param name="bytesIn_LENGTH">Length of byte array</param>
        /// <param name="bytesOut_OUTPUT">Output byte array to read</param>
        /// <returns>Returns NI error code</returns>
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2CWriteRead(string deviceName, byte adminByte, byte[] bytesIn, int bytesIn_LENGTH, byte[] bytesOut_OUTPUT);
    }
}
