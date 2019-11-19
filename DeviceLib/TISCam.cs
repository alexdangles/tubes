using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TIS.Imaging;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Devices
{
    public class TISCam
    {
        #region Fields
        [DllImport("LVFunctions.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe void BMPtoArray(string bmpFile, double*[,] imageArray);

        delegate void CameraLostDelegate();
        public ICImagingControl ic;
        #endregion

        #region Constructors
        public TISCam(string deviceName, string settingsFile)
        {
            device = deviceName;
            settings = settingsFile;
            if (device.Contains("DMx")) Initialize();
        }
        #endregion

        #region Properties
        public bool connected
        {
            get
            {
                return ic.DeviceValid;
            }
        }
        public string mainFolder { get; private set; }
        public string log { get; private set; }
        public float frameRate { get; set; }
        public string device { get; set; }
        public string settings { get; set; }
        public byte[] imageArray { get; private set; }
        public byte[] backgroundArray { get; private set; }
        public double[,] intensityPlot { get; private set; }
        #endregion

        #region Events
        private void icImagingControl1_DeviceLost(object sender, ICImagingControl.DeviceLostEventArgs e)
        {
            ic.BeginInvoke(new CameraLostDelegate(ref CameraLost));
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            ic = new ICImagingControl();
            ic.Device = device;
            ic.DeviceFrameRate = frameRate;
            ic.DeviceLost += new EventHandler<ICImagingControl.DeviceLostEventArgs>(icImagingControl1_DeviceLost);
        }
        public void Disconnect()
        {
            if (connected)
            {
                if (ic.LiveVideoRunning) ic.LiveStop();
                ic.Dispose();
            }
        }
        private void CameraLost()
        {
            ic.Device = null;
        }
        public void StartLive()
        {
            if (ic.LiveVideoRunning) ic.LiveStop();
            else ic.LiveStart();
        }
        public void DeviceSettings()
        {
            if (connected) ic.LiveStop();
            ic.ShowDeviceSettingsDialog();
            if (connected) ic.SaveDeviceStateToFile(settings);
        }
        public Task GrabBackgroundImage()
        {
            return Task.Run(() =>
            {
                if (connected)
                {
                    Bitmap background;
                    using (ICImagingControl ic1 = new ICImagingControl())
                    {
                        ic1.Device = device;
                        ic1.DeviceFrameRate = frameRate;
                        ic1.MemorySnapImage(60000);
                        background = ic1.ImageActiveBuffer.Bitmap;
                    }
                    int h = background.Height;
                    int w = background.Width;

                    // Regular method
                    backgroundArray = ImageToByte(background).Skip(1078).ToArray();

                    //// Lockbits method
                    //Rectangle section = new Rectangle(0, 0, background.Width, h);
                    //BitmapData imageData = background.LockBits(section, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    //IntPtr ptr = imageData.Scan0;
                    //int bytes = Math.Abs(imageData.Stride) * h;
                    //backgroundArray = new byte[bytes];
                    //Marshal.Copy(ptr, backgroundArray, 0, bytes);
                }
            });
        }
        public Task GrabImage()
        {
            return Task.Run(() =>
            {
                if (connected)
                {
                    Bitmap image;
                    using (ICImagingControl ic2 = new ICImagingControl())
                    {
                        ic2.Device = device;
                        ic2.DeviceFrameRate = frameRate;
                        ic2.MemorySnapImage(60000);
                        image = ic2.ImageActiveBuffer.Bitmap;
                    }
                    int h = image.Height;
                    int w = image.Width;

                    // Regular method
                    imageArray = ImageToByte(image).Skip(1078).ToArray();

                    // Lockbits method                
                    //Rectangle section = new Rectangle(0, 0, image.Width, h);
                    //BitmapData imageData = image.LockBits(section, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    //IntPtr ptr = imageData.Scan0;
                    //int bytes = Math.Abs(imageData.Stride) * h;
                    //imageArray = new byte[bytes];
                    //Marshal.Copy(ptr, imageArray, 0, bytes);

                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        imageArray[i] = (byte)Math.Abs(imageArray[i] - backgroundArray[i]);
                    }
                    int j = 0;
                    intensityPlot = new double[w, h];
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++, j++)
                        {
                            if (x > 4 && x < w - 5 && y > 4 && y < h - 5)
                            {
                                intensityPlot[x, y] += (imageArray[j - 5] + imageArray[j - 4] + imageArray[j - 3] + imageArray[j - 2] + imageArray[j - 1] + imageArray[j] + imageArray[j + 1] + imageArray[j + 2] + imageArray[j + 3] + imageArray[j + 4] + imageArray[j + 5] +
                                    imageArray[j - (5 * w)] + imageArray[j - (4 * w)] + imageArray[j - (3 * w)] + imageArray[j - (2 * w)] + imageArray[j - w] + imageArray[j + w] + imageArray[j + (2 * w)] + imageArray[j + (3 * w)] + imageArray[j + (4 * w)] + imageArray[j + (5 * w)]) / 21;
                            }
                            else intensityPlot[x, y] += imageArray[j];
                        }
                    }

                    //Marshal.Copy(imageArray, 0, ptr, bytes);
                    //image.UnlockBits(imageData);
                }
            });
        }
        public static Bitmap ByteToImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                return new Bitmap(stream);
            }
        }
        public static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Bmp);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }
        #endregion
    }
}
