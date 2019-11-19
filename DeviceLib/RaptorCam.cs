using Helper;
using PvDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace Devices
{

    public class RaptorCam
    {
        private const int ccdImageSizeX = 256;
        private const int ccdImageSizeY = 256;
        private int _numberOfImages = 5;
        private double _exposure, _TEC;
        public List<Bitmap> imageBitmaps = new List<Bitmap>();
        public Color[] colorTableMax = new Color[65536];
        public int[,] colorTable = new int[512, 512];


        #region Raptor Camera

        #region Properties


        public double[] calibrationADC { get; private set; }

        public double[] calibrationDAC { get; private set; }

        public int numberOfImages
        {
            get
            {
                return _numberOfImages;
            }
            set
            {
                _numberOfImages = value;
                counts = new int[numberOfImages, ccdImageSizeX, ccdImageSizeY];
            }
        }

        public int[,,] counts { get; private set; }

        public int imageIndex { get; private set; }

        public bool acquiring { get; set; }

        public bool[] sysStatus { get; private set; }

        public bool[] fpgaStatus { get; private set; }

        public bool[] trigMode { get; private set; }

        public bool testPattern { get; private set; }

        /// <summary>
        /// Get connection status.
        /// </summary>
        public bool connected
        {
            get
            {
                return device != null && device.IsConnected;
            }
        }

        /// <summary>
        /// Exposure setting, in sec.
        /// </summary>
        public double exposure
        {
            get
            {
                TxRx(0x53, 0xe0, 0x01, 0xed);
                TxRx(0x53, 0xe1, 0x01);
                TxRx(0x53, 0xe0, 0x01, 0xee);
                TxRx(0x53, 0xe1, 0x01);
                TxRx(0x53, 0xe0, 0x01, 0xef);
                TxRx(0x53, 0xe1, 0x01);
                TxRx(0x53, 0xe0, 0x01, 0xf0);
                TxRx(0x53, 0xe1, 0x01);
                byte[] bytes = TxRx(0x53, 0xe0, 0x01, 0xf1);
                long ADCBits = BitFun.ByteToInt(bytes);
                return _exposure; //ADCBits / 40000000.0;
            }
            set
            {
                long DACBits = (int)(40000000 * value);
                byte[] bytes = BitFun.HexToByte(DACBits.ToString("X10")); // DACBits = 5 byte word (40 bit)
                TxRx(0x53, 0xe0, 0x02, 0xed, bytes[0]);
                TxRx(0x53, 0xe0, 0x02, 0xee, bytes[1]);
                TxRx(0x53, 0xe0, 0x02, 0xef, bytes[2]);
                TxRx(0x53, 0xe0, 0x02, 0xf0, bytes[3]);
                TxRx(0x53, 0xe0, 0x02, 0xf1, bytes[4]);
                _exposure = value;
            }
        }

        /// <summary>
        /// Thermo-electric cooler setting, in C.
        /// </summary>
        public double TEC
        {
            get
            {
                TxRx(0x53, 0xe0, 0x01, 0x03);
                TxRx(0x53, 0xe1, 0x01);
                TxRx(0x53, 0xe0, 0x01, 0x04);
                byte[] bytes = TxRx(0x53, 0xe1, 0x01);
                long ADCBits = BitFun.ByteToInt(bytes);
                return _TEC; //calibrationADC[0] * ADCBits + calibrationADC[1];
            }
            set
            {
                long DACBits = (long)((value - calibrationDAC[1]) / calibrationDAC[0]);
                byte[] bytes = BitFun.HexToByte(DACBits.ToString("X4")); // DACBits = 2 byte word (12 bit)
                TxRx(0x53, 0xe0, 0x02, 0x03, bytes[0]);
                TxRx(0x53, 0xe0, 0x02, 0x04, bytes[1]);
                _TEC = value;
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Save multiple snaps to local disk as bitmaps.
        /// </summary>
        /// <param name="folder">Folder on disk.</param>
        public void SaveBitmaps(string folder)
        {
            if (imageBitmaps.Count > 0)
            {
                try
                {
                    for (int i = 0; i < imageBitmaps.Count; i++)
                    {
                        imageBitmaps[i].Save($"{ folder }{ (i + 1).ToString("00") }.bmp", ImageFormat.Bmp);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Send/Recieve to camera.
        /// </summary>
        /// <param name="msg">Bytes to send.</param>
        /// <returns></returns>
        public byte[] TxRx(params byte[] msg)
        {
            byte[] send = new byte[msg.Length + 1];
            for (int i = 0; i < msg.Length; i++)
                send[i] = msg[i];
            send[msg.Length] = 0x50; // Append ETX char at end
            send = BitFun.GetChecksum(send); // Send with checksum
            try
            {
                serial?.Write(send);
                Thread.Sleep(100);
                return serial?.Read();
            }
            catch
            {

            }
            return new byte[0];
        }

        /// <summary>
        /// Init. camera with required settings.
        /// </summary>
        private void Initialize()
        {
            TxRx(0x4f, 0x52); // Boot the FPGA
            TxRx(0x4f, 0x57); // Set system state
            TxRx(0x53, 0xe0, 0x02, 0x00, 0x01); // Set FPGA control register

            TxRx(0x53, 0xae, 0x05, 0x01, 0x00, 0x00, 0x02, 0x00);
            string cal = BitFun.ByteToHex(TxRx(0x53, 0xaf, 0x12)).Substring(20, 16);
            ushort calADC0C = (ushort)BitFun.ByteToInt(BitFun.HexToByte(cal.Substring(0, 4)));
            ushort calADC40C = (ushort)BitFun.ByteToInt(BitFun.HexToByte(cal.Substring(4, 4)));
            ushort calDAC0C = (ushort)BitFun.ByteToInt(BitFun.HexToByte(cal.Substring(8, 4)));
            ushort calDAC40C = (ushort)BitFun.ByteToInt(BitFun.HexToByte(cal.Substring(12, 4)));

            calibrationADC[0] = -40.0 / Convert.ToDouble(calADC0C - calADC40C); // slope
            calibrationADC[1] = 40.0 - (calibrationADC[0] * Convert.ToDouble(calADC40C)); // intercept
            calibrationDAC[0] = -40.0 / Convert.ToDouble(calDAC0C - calDAC40C); // slope
            calibrationDAC[1] = 40.0 - (calibrationDAC[0] * Convert.ToDouble(calDAC40C)); // intercept

            GetStatus();
        }

        /// <summary>
        /// Reset the camera.
        /// </summary>
        public void Reset()
        {
            TxRx(0x55, 0x99, 0x66, 0x11);
            Initialize();
        }

        /// <summary>
        /// Take a snapshot.
        /// </summary>
        public void SnapShot()
        {
            acquiring = true;
            TxRx(0x53, 0xe0, 0x02, 0xd4, 0x01);
        }

        /// <summary>
        /// Get all data from camera memory.
        /// </summary>
        /// <returns></returns>
        public void GetStatus()
        {
            try
            {
                sysStatus = BitFun.HexToBitArray(BitFun.ByteToHex(TxRx(0x49)).Substring(0, 2));

                TxRx(0x53, 0xe0, 0x01, 0x00);
                fpgaStatus = BitFun.HexToBitArray(BitFun.ByteToHex(TxRx(0x53, 0xe1, 0x01)).Substring(0, 2));

                TxRx(0x53, 0xe0, 0x01, 0xd4);
                trigMode = BitFun.HexToBitArray(BitFun.ByteToHex(TxRx(0x53, 0xe1, 0x01)).Substring(0, 2));

                TxRx(0x53, 0xe0, 0x01, 0xf7);
                testPattern = BitFun.ByteToHex(TxRx(0x53, 0xe1, 0x01)) == "4";

            }
            catch { }
        }
        #endregion
        #endregion

        #region Pleora Gig-E Interface

        // Main application objects: device, stream, pipeline, serial
        private PvDevice device;
        private PvStream stream;
        private PvPipeline pipeline;
        private PvDeviceSerialPort serial;

        private PvDisplayThread displayThread;
        private PvAcquisitionStateManager acquisitionManager;

        // Raise event when image is ready
        public delegate void ImageAvailable(Bitmap image);
        public event ImageAvailable imageAvailable;


        /// <summary>
        /// Connect to device.
        /// </summary>
        /// <param name="ipAddress">IP address.</param>
        public RaptorCam(string ipAddress)
        {
            calibrationADC = new double[2];
            calibrationDAC = new double[2];
            counts = new int[numberOfImages, ccdImageSizeX, ccdImageSizeY];
            sysStatus = new bool[8];
            fpgaStatus = new bool[8];
            trigMode = new bool[2];

            try
            {
                // Setup GigE device
                device = PvDevice.CreateAndConnect(ipAddress);
                stream = PvStream.CreateAndOpen(ipAddress);

                PvDeviceGEV deviceGEV = device as PvDeviceGEV;
                PvStreamGEV streamGEV = stream as PvStreamGEV;

                deviceGEV.NegotiatePacketSize();
                deviceGEV.SetStreamDestination(streamGEV.LocalIPAddress, streamGEV.LocalPort);

                // Setup camera serial comms
                serial = new PvDeviceSerialPort();
                serial.Open(device, PvDeviceSerial.Bulk0);

                // Setup pipeline
                pipeline = new PvPipeline(stream);

                SetCameraParameters();
                StartStreaming();
                StartAcquisition();
                Initialize();
            }
            catch
            {
                Disconnect();
                return;
            }
        }

        /// <summary>
        /// Give Pleura Gig-E settings for this camera (important!).
        /// </summary>
        public void SetCameraParameters()
        {
            device.Parameters.SetIntegerValue("Width", 1024);
            device.Parameters.SetIntegerValue("Height", 256);
            device.Parameters.SetEnumValue("PixelFormat", "Mono16");
            device.Parameters.SetEnumValue("TestPattern", "Off");
            device.Parameters.SetEnumValue("BulkBaudRate", "Baud115200");
            device.Parameters.SetBooleanValue("PixelBusDataValidEnabled", true);
        }

        /// <summary>
        /// Callback when buffer is acquired.
        /// </summary>
        /// <param name="aDisplayThread">The display thread.</param>
        /// <param name="aBuffer">The buffer.</param>
        private void OnBufferDisp(PvDisplayThread aDisplayThread, PvBuffer aBuffer)
        {
            if (imageIndex == 0) imageBitmaps.Clear();
            Bitmap snap = new Bitmap(1024, 256);
            aBuffer.Image.CopyToBitmap(snap);
            snap = ImagingFun.Crop(snap, (256 + 256/2), 0, ccdImageSizeX, ccdImageSizeY); // Take middle 256x256 of frame.
            Bitmap inverted = ImagingFun.Invert(snap, false); // Invert so that max is white and min is black.
            imageBitmaps.Add(inverted);
            double[,] intensity = ImagingFun.BmpToIntensityArray(inverted);
            int w = intensity.GetLength(0);
            int h = intensity.GetLength(1);
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    counts[imageIndex, i, j] = (int)intensity[i, j];
                }
            }
            imageAvailable(inverted);
            imageIndex++;
            if (imageIndex < numberOfImages && acquiring)
            {
                SnapShot();
            }
            else
            {
                imageIndex = 0;
                acquiring = false;
            }
        }

        /// <summary>
        /// Setups streaming. After calling this method the application is ready to receive data.
        /// StartAcquisition will instruct the device to actually start sending data.
        /// </summary>
        private void StartStreaming()
        {
            // Create display thread, hook display event
            displayThread = new PvDisplayThread();
            displayThread.OnBufferDisplay += OnBufferDisp;

            // Start threads
            displayThread.Start(pipeline, device.Parameters);
            displayThread.Priority = PvThreadPriority.AboveNormal;

            // Configure acquisition state manager
            acquisitionManager = new PvAcquisitionStateManager(device, stream);

            // Start pipeline
            pipeline.Start();
        }

        /// <summary>
        /// Stops streaming. After calling this method the application is no longer armed or ready
        /// to receive data.
        /// </summary>
        private void StopStreaming()
        {
            if (!displayThread.IsRunning)
            {
                return;
            }

            // Stop display thread
            displayThread.Stop(false);

            // Release acquisition manager
            acquisitionManager.Dispose();
            acquisitionManager = null;

            // Stop pipeline
            if (pipeline.IsStarted)
            {
                pipeline.Stop();
            }

            displayThread.WaitComplete();
        }

        /// <summary>
        /// Starts acquisition.
        /// </summary>
        private void StartAcquisition()
        {
            // Get payload size
            uint payloadSize = payloadSize = device.PayloadSize;

            // Propagate to pipeline to make sure buffers are big enough
            pipeline.BufferSize = payloadSize;

            // Reset pipeline
            pipeline.Reset();

            // Reset stream statistics
            PvGenCommand lResetStats = stream.Parameters.GetCommand("Reset");
            lResetStats.Execute();

            // Reset display thread stats (mostly frames displayed per seconds)
            displayThread.ResetStatistics();

            // Use acquisition manager to send the acquisition start command to the device
            acquisitionManager.Start();
        }

        /// <summary>
        /// Stops the acquisition.
        /// </summary>
        private void StopAcquisition()
        {
            // Use acquisition manager to send the acquisition stop command to the device
            acquisitionManager?.Stop();
        }

        /// <summary>
        /// Disconnect from device.
        /// </summary>
        public void Disconnect()
        {
            StopAcquisition();

            if (stream != null)
            {
                // If streaming, stop streaming
                if (stream.IsOpen)
                {
                    StopStreaming();

                    stream.Close();
                    stream = null;
                }
            }

            if (device != null)
            {
                if (device.IsConnected)
                {
                    device.Disconnect();
                    device = null;
                }
            }
        }
        #endregion
    }
}