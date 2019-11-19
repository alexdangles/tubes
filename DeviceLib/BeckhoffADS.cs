/// <copyright file="MXBeckhoffADS.cs" company="Moxtek, inc.">
/// Copyright (c) 2018 All Rights Reserved
/// </copyright>
/// <author>Chris R. Carter</author>
/// <date>04/06/2018</date>
/// <summary>Contains classes for use in communicating from PC to Beckhoff PLC via ADS</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using TwinCAT.Ads;

namespace BurnInStns
{
    /// <summary>
    /// Class contains Name, Value, Datatype and ADS read/write handlers
    /// </summary>
    public class MX_PLCDevice
    {
        public string Name { get; set; }  // Device name in the PLC
        public int ReadHndl { get; set; }  // ADS Read handler
        public int WriteHndl { get; set; }  // ADS Write handler
        public dynamic Value { get; set; }  // Stores value read from PLC
        public Type DataType { get; set; }  // DataType of the PLC variable
    }

    /// <summary>
    /// Class for converting from PLC datatypes to C# datatypes
    /// </summary>
    public static class MX_PLCDataTypes
    {
        static public Type BOOL { get; } = typeof(System.Boolean);
        static public Type BYTE { get; } = typeof(System.Byte);
        static public Type WORD { get; } = typeof(System.UInt16);
        static public Type DWORD { get; } = typeof(System.UInt32);
        static public Type SINT { get; } = typeof(System.SByte);
        static public Type INT { get; } = typeof(System.Int16);
        static public Type DINT { get; } = typeof(System.Int32);
        static public Type LINT { get; } = typeof(System.Int64);
        static public Type USINT { get; } = typeof(System.Byte);
        static public Type UINT { get; } = typeof(System.UInt16);
        static public Type UDINT { get; } = typeof(System.UInt32);
        static public Type ULINT { get; } = typeof(System.UInt64);
        static public Type REAL { get; } = typeof(System.Single);
        static public Type LREAL { get; } = typeof(System.Double);
    }

    /// <summary>
    /// Class contains the MX_PLCDevice class for the OnDeviceChanged event
    /// </summary>
    public class DeviceChangedArgs : EventArgs
    {
        private MX_PLCDevice vDevice;
        public DeviceChangedArgs(MX_PLCDevice _Device)
        {
            vDevice = _Device;
        }

        public MX_PLCDevice Device { get { return vDevice; } }
    }

    /// <summary>
    /// Main class that handles ADS communication from PC to ADS
    /// </summary>
    public class MX_ADSClient
    {
        private string amsNetId;  // Ams/Ads address of the Beckhoff PLC
        private int port;  // Ams/Ads port on the Beckhoff PLC
        private int updateTime = 500;  // Time to check for updates from the PLC
        private int readStreamSize = 15;  // Read stream size, I have no idea why 15... but it works

        private AdsStream vStream;  // ADS Stream Reader
        private AdsBinaryReader vBinaryReader;  // ADS Binary reader, used to read data from stream
        private TcAdsClient vAdsClient;  // ADS Client
        private List<MX_PLCDevice> vDevices = new List<MX_PLCDevice>();  // List of devices to read/write from PLC

        public delegate void DeviceChangedDelegate(DeviceChangedArgs e);  // Delegate for OnDeviceChanged Event
        public event DeviceChangedDelegate OnDeviceChanged;  // Event that fires when PLC device has changed

        
        /// <summary>
        /// Ams/Ads address of the Beckhoff PLC
        /// </summary>
        public string AmsNetId { get { return amsNetId; } set { amsNetId = value; } }  // Address to Beckhoff PLC

        /// <summary>
        /// Ams/Ads port on the Beckhoff PLC
        /// </summary>
        public int Port { get { return port; } set { port = value; } }

        /// <summary>
        /// Time to check for updates (ms) from the PLC, default is 500
        /// </summary>
        public int UpdateTime { get { return updateTime; } set { updateTime = value; } }

        /// <summary>
        /// Read stream size, default is 15
        /// </summary>
        public int ReadStreamSize { get { return readStreamSize; } set { readStreamSize = value; } }

        /// <summary>
        /// Default Constructor for clsBeckhoffADS class
        /// </summary>
        public MX_ADSClient()
        {
            AmsNetId = "";
            Port = 851;
        }

        /// <summary>
        /// Constructor for clsBeckhoffADS class that sets the Port, without AmsNetId the class connects using local port
        /// </summary>
        /// <param name="vAmsNetId">Ams/Ads address of the Beckhoff PLC</param>
        public MX_ADSClient(int vPort)
        {
            AmsNetId = "";
            Port = vPort;
        }

        /// <summary>
        /// Constructor for clsBeckhoffADS class that sets the AmsNetId and Port
        /// </summary>
        /// <param name="vAmsNetId">Ams/Ads address of the Beckhoff PLC</param>
        /// <param name="vPort">Ams/Ads port on the Beckhoff PLC</param>
        public MX_ADSClient(string vAmsNetId, int vPort)
        {
            AmsNetId = vAmsNetId;
            Port = vPort;
        }

        /// <summary>
        /// Establishes a connection to the ADS device.
        /// If AmsNetId has not been supplied this connects using localport.
        /// If Port has not been supplied this connects using port 851
        /// </summary>
        public void Connect()
        {
            vStream = new AdsStream(ReadStreamSize);  // Create new Stream Reader
            vBinaryReader = new AdsBinaryReader(vStream);  // Create new binary reader
            vAdsClient = new TcAdsClient();  // Create new Ads Client

            // If amsNetId == "" use the local port, if not connect to the provided Net ID
            if (amsNetId == "") vAdsClient.Connect(port);  
            else vAdsClient.Connect(amsNetId, port);

            // Subscribe to the ADS notification event
            vAdsClient.AdsNotification += new AdsNotificationEventHandler(PLCReadNotification);
        }

        /// <summary>
        /// Establishes a connection to the ADS device using the supplied AmsNetId and Port.
        /// </summary>
        /// If AmsNetId has not been supplied this connects using localport.
        /// <param name="vPort">Ams/Ads port on the Beckhoff PLC</param>
        public void Connect(int vPort)
        {
            Port = vPort;
            Connect();
        }

        /// <summary>
        /// Establishes a connection to the ADS device using the supplied AmsNetId and Port.
        /// </summary>
        /// <param name="vAmsNetId">Ams/Ads address of the Beckhoff PLC</param>
        /// <param name="vPort">Ams/Ads port on the Beckhoff PLC</param>
        public void Connect(string vAmsNetId, int vPort)
        {
            AmsNetId = vAmsNetId;
            Port = vPort;
            Connect();
        }

        /// <summary>
        /// Adds device to device list, creates read and write handlers
        /// </summary>
        /// <param name="vDeviceName">string, Name of device that matches variable name in PLC</param>
        /// <param name="vDataType">Type, Data Type of the variable in the PLC</param>
        public MX_PLCDevice AddDevice(string vDeviceName, Type vDataType)
        {
            MX_PLCDevice vNewDevice = new MX_PLCDevice();  // Create a new temporary device
            vNewDevice.Name = vDeviceName;  // Add name to the temporary device
            vNewDevice.DataType = vDataType;  // Add data type to the temporary device
            vDevices.Add(vNewDevice);  // Add the new device to the device list
            int vIndex = vDevices.Count() - 1;  // Get the index of the newly added device

            // Create an ADS read handler for the new device
            vDevices[vIndex].ReadHndl = vAdsClient.AddDeviceNotification(vDeviceName, vStream, AdsTransMode.OnChange, updateTime, 0, vDevices[vIndex].Value);

            // Create an ADS write handler for the new device
            vDevices[vIndex].WriteHndl = vAdsClient.CreateVariableHandle(vDeviceName);

            // Return the new device in case the calling function wants it
            return vDevices[vIndex];
        }

        /// <summary>
        /// Event that is fired when one of the devices change state in the PLC
        /// </summary>
        private void PLCReadNotification(object sender, AdsNotificationEventArgs e)
        {
            int vIndex = vDevices.FindIndex(ni => ni.ReadHndl == e.NotificationHandle);  // Find Index of Device in List

            // Read Value for each DataType
            switch (vDevices[vIndex].DataType.ToString())
            {
                    case "System.Boolean": vDevices[vIndex].Value = vBinaryReader.ReadBoolean(); break;
                    case "System.Byte": vDevices[vIndex].Value = vBinaryReader.ReadByte(); break;
                    case "System.UInt16": vDevices[vIndex].Value = vBinaryReader.ReadUInt16(); break;
                    case "System.UInt32": vDevices[vIndex].Value = vBinaryReader.ReadUInt32(); break;
                    case "System.SByte": vDevices[vIndex].Value = vBinaryReader.ReadSByte(); break;
                    case "System.Int16": vDevices[vIndex].Value = vBinaryReader.ReadInt16(); break;
                    case "System.Int32": vDevices[vIndex].Value = vBinaryReader.ReadInt32(); break;
                    case "System.Int64": vDevices[vIndex].Value = vBinaryReader.ReadInt64(); break;
                    case "System.UInt64": vDevices[vIndex].Value = vBinaryReader.ReadUInt64(); break;
                    case "System.Single": vDevices[vIndex].Value = vBinaryReader.ReadSingle(); break;
                    case "System.Double": vDevices[vIndex].Value = vBinaryReader.ReadDouble(); break;
            }

            // Raise Event to any subscribers
            if (vIndex >= 0) OnDeviceChanged(new DeviceChangedArgs(vDevices[vIndex]));
        }

        /// <summary>
        /// Returns the last state that was read from the PLC of the specified device
        /// </summary>
        /// <param name="vDeviceName">string, Name of device that matches variable name in PLC</param>
        /// <returns>object, value of the device from the last read of the PLC</returns>
        public dynamic GetState(string vDeviceName)
        {
            // Find Index of the Device
            int vIndex = vDevices.FindIndex(ni => ni.Name == vDeviceName);  

            // Return the device
            return vDevices[vIndex].Value;
        }

        /// <summary>
        /// Sets the state of the device in the PLC
        /// </summary>
        /// <param name="vDeviceName">string, Name of device that matches variable name in PLC</param>
        /// <param name="vValue">object, value to send to the PLC</param>
        public void SetState(string vDeviceName, object vValue)
        {
            // Find Index of the Device
            int vIndex = vDevices.FindIndex(ni => ni.Name == vDeviceName);

            // Write the value using the ADS write handler
            vAdsClient.WriteAny(vDevices[vIndex].WriteHndl, vValue);
        }

        /// <summary>
        /// Gets the the last state of the device in the PLC
        /// </summary>
        /// <param name="vDeviceName">string, Name of device that matches variable name in PLC</param>
        public MX_PLCDevice GetDevice(string vDeviceName)
        {
            // Find Index of the Device
            int vIndex = vDevices.FindIndex(ni => ni.Name == vDeviceName);

            // Return the device
            return vDevices[vIndex];
        }
    }
}
