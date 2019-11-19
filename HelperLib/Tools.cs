using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Helper
{

    /// <summary>
    /// General pc function tools.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Get names of files in a directory.
        /// </summary>
        /// <param name="path">Directory to get contained files.</param>
        /// <returns></returns>
        public static string[] GetFileNames(string path)
        {
            string[] p = Directory.GetFiles(path);
            List<string> n = new List<string>();
            foreach (string f in p)
                n.Add(f.Split('\\').Last());
            return n.ToArray();
        }

        /// <summary>
        /// Shutdown the PC.
        /// </summary>
        public static void ShutdownPC()
        {
            Process.Start("shutdown", "/s /t 0");
        }

    }

    /// <summary>
    /// Manipulate data in binary or hex format.
    /// </summary>
    public static class BitFun
    {
        /// <summary>
        /// Convert hexidecimal into byte array.
        /// </summary>
        /// <param name="hex">Hexidecimal to convert.</param>
        /// <returns></returns>
        public static byte[] HexToByte(string hex, bool returnChkSum = false)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return returnChkSum ? GetChecksum(bytes) : bytes;
        }

        /// <summary>
        /// Convert byte array to an integer.
        /// </summary>
        /// <param name="b">Byte array to convert.</param>
        /// <param name="switchEndian">Least significant byte first?</param>
        /// <returns></returns>
        public static long ByteToInt(byte[] b, bool switchEndian = false)
        {
            if (switchEndian) Array.Reverse(b);
            return Convert.ToInt64(ByteToHex(b), 16);
        }

        /// <summary>
        /// Convert hexidecimal into a bit array.
        /// </summary>
        /// <param name="hex">Hexidecimal to convert.</param>
        /// <returns></returns>
        public static bool[] HexToBitArray(string hex)
        {
            BitArray ba = new BitArray(4 * hex.Length);
            for (int i = 0; i < hex.Length; i++)
            {
                byte b = byte.Parse(hex[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            int[] bits = ba.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();
            bool[] bitsOut = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                bitsOut[i] = Convert.ToBoolean(bits[i]);
            }
            return bitsOut;
        }

        /// <summary>
        /// Convert array of bytes into hexidecimal.
        /// </summary>
        /// <param name="b">Byte array to convert.</param>
        /// <returns></returns>
        public static string ByteToHex(byte[] b)
        {
            return BitConverter.ToString(b).Replace("-", "");
        }

        /// <summary>
        /// Calculate NMEA checksum byte from input data.
        /// </summary>
        /// <param name="data">Input data to calculate checksum with.</param>
        /// <returns></returns>
        public static byte[] GetChecksum(byte[] data)
        {
            byte chkSumByte = 0x00;
            byte[] output = new byte[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
            {
                chkSumByte ^= data[i];
                output[i] = data[i];
            }
            output[data.Length] = chkSumByte;
            return output;
        }
    }

    /// <summary>
    /// Image processing.
    /// </summary>
    public class ImagingFun
    {
        public enum BinMode { x1, x8, x16, x32, x64, x128 }

        public static Bitmap Crop(Bitmap original, int x, int y, int width, int height)
        {
            Rectangle cropRect = new Rectangle(x, y, width, height);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(original, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }
            return target;
        }

        public static Bitmap Invert(Bitmap image, bool alphaIsInverted = true)
        {
            for (int x = 0; x <= image.Width - 1; x++)
            {
                for (int y = 0; y <= image.Height - 1; y++)
                {
                    Color original = image.GetPixel(x, y);
                    Color inverted;
                    if (alphaIsInverted)
                    {
                        inverted = Color.FromArgb(255 - original.A, 255 - original.R, 255 - original.G, 255 - original.B);
                    }
                    else
                    {
                        inverted = Color.FromArgb(original.A, 255 - original.R, 255 - original.G, 255 - original.B);
                    }
                    image.SetPixel(x, y, inverted);
                }
            }
            return image;
        }

        public static double[,] BmpToIntensityArray(Bitmap image)
        {
            int h = image.Height;
            int w = image.Width;
            double[,] intensityPlot = new double[w, h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    intensityPlot[x, y] = .299 * image.GetPixel(x, y).R + .587 * image.GetPixel(x, y).G + .114 * image.GetPixel(x, y).B;
                }
            }
            return intensityPlot;
        }

        public static double[,] BinPixels(double[,] pixels, BinMode binMode = BinMode.x1)
        {
            int w = pixels.GetLength(0);
            int h = pixels.GetLength(1);
            double[,] binnedPixels = new double[w, h];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x > ((int)binMode - 1) && x < w - (int)binMode && y > ((int)binMode - 1) && y < h - (int)binMode)
                    {
                        binnedPixels[x, y] += pixels[x, y];
                        if (binMode == BinMode.x8) binnedPixels[x, y] += pixels[x - 1, y - 1] + pixels[x + 1, y + 1] + pixels[x + 1, y - 1] + pixels[x - 1, y + 1] + pixels[x, y - 1] + pixels[x, y + 1] + pixels[x - 1, y] + pixels[x + 1, y];
                        if (binMode == BinMode.x16) binnedPixels[x, y] += pixels[x - 2, y - 2] + pixels[x + 2, y + 2] + pixels[x + 2, y - 1] + pixels[x - 2, y + 2] + pixels[x, y - 2] + pixels[x, y + 2] + pixels[x - 2, y] + pixels[x + 2, y];
                        if (binMode == BinMode.x32) binnedPixels[x, y] += pixels[x - 3, y - 3] + pixels[x + 3, y + 3] + pixels[x + 3, y - 1] + pixels[x - 3, y + 3] + pixels[x, y - 3] + pixels[x, y + 3] + pixels[x - 3, y] + pixels[x + 3, y];
                        if (binMode == BinMode.x64) binnedPixels[x, y] += pixels[x - 4, y - 4] + pixels[x + 4, y + 4] + pixels[x + 4, y - 1] + pixels[x - 4, y + 4] + pixels[x, y - 4] + pixels[x, y + 4] + pixels[x - 4, y] + pixels[x + 4, y];
                        if (binMode == BinMode.x128) binnedPixels[x, y] += pixels[x - 5, y - 5] + pixels[x + 5, y + 5] + pixels[x + 5, y - 1] + pixels[x - 5, y + 5] + pixels[x, y - 5] + pixels[x, y + 5] + pixels[x - 5, y] + pixels[x + 5, y];
                    }
                }
            }

            return binnedPixels;
        }

        public static Bitmap ChangeColor(Bitmap vBitmap, Color vColor)
        {
            Bitmap vNewBitmap = new Bitmap(vBitmap);

            for (int vY = 0; vY < vBitmap.Height; vY++)
            {
                for (int vX = 0; vX < vBitmap.Width; vX++)
                {
                    vNewBitmap.SetPixel(vX, vY, MultiplyColor(vColor, vBitmap.GetPixel(vX, vY)));
                }
            }
            return vNewBitmap;
        }

        public static Color MultiplyColor(Color vColor1, Color vColor2)
        {
            int vRED = (vColor1.R * vColor2.R) / 255;
            int vGRN = (vColor1.G * vColor2.G) / 255;
            int vBLU = (vColor1.B * vColor2.B) / 255;

            return Color.FromArgb(vColor2.A, vRED, vGRN, vBLU);
        }

        public static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            int h = img.Height;
            int w = img.Width;

            int stride = 4 * ((w * 24 + 31) / 32);
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Bmp);
                byteArray = stream.ToArray().Skip(stride).ToArray();
            }
            return byteArray;
        }
    }
}
