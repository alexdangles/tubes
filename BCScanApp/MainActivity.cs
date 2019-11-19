using Android;
using Android.App;
using Android.Widget;
using Android.OS;

using System;
using System.Text;
using System.Net.Sockets;

namespace BCScan
{
    [Activity(Label = "Barcode Scanner", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView scan, prompt, info;
        ImageView img;
        string pcAddress, station;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            LoadMain();
        }

        void LoadMain()
        {
            SetContentView(Resource.Layout.Main);
            img = (ImageView)FindViewById(Resource.Id.imageView1);
            scan = (TextView)FindViewById(Resource.Id.txvScan);
            scan.TextChanged += Scan_TextChanged;
            prompt = (TextView)FindViewById(Resource.Id.txvPrompt);
            info = (TextView)FindViewById(Resource.Id.txvInfo);
            info.Text = "";
            ActionBar.Hide();
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Main);
            ActionBar.Hide();
        }

        private void Scan_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (scan.Text.Contains("\n"))
            {
                string toSend = scan.Text.Trim(new char[] { '\r', '\n', '\t' });
                if (toSend.Contains("@") && toSend.Contains("-"))
                {
                    if (SendCode(toSend).Length > 0)
                    {
                        img.SetImageDrawable(GetDrawable(Resource.Drawable.Tube));
                        prompt.Text = "Scan Tube";
                    }
                    else
                    {
                        img.SetImageDrawable(GetDrawable(Resource.Drawable.QR));
                        prompt.Text = "Scan A Code";
                    }
                }
                else if (!toSend.Contains("@") && toSend.Contains(" "))
                {
                    img.SetImageDrawable(GetDrawable(Resource.Drawable.QR));
                    prompt.Text = "Scan A Code";
                    Toast.MakeText(this, "Wrong barcode format.", ToastLength.Long).Show();
                }
                else
                {
                    SendCode(toSend);
                    img.SetImageDrawable(GetDrawable(Resource.Drawable.QR));
                    prompt.Text = "Scan A Code";
                }
                scan.Text = string.Empty;
            }
        }

        private string SendCode(string code)
        {
            try
            {
                if (code.Contains("@"))
                {
                    string[] ipStation = code.Trim('@').Split(' ');
                    pcAddress = ipStation[0];
                    station = ipStation[1];
                }
                if (pcAddress != null)
                {
                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.SendTimeout = client.ReceiveTimeout = 5000;

                    try
                    {
                        client.Connect(pcAddress, 13000);
                        byte[] sendBuff = Encoding.ASCII.GetBytes(code);
                        client.Send(sendBuff);
                        byte[] recBuff = new byte[1024];
                        client.Receive(recBuff);
                        string msgRec = Encoding.ASCII.GetString(recBuff, 0, recBuff.Length);
                        info.Text = $"{ DateTime.Now.ToString("M/dd HH:mm:ss") }\r\n{ msgRec }\r\n{ info.Text }";
                        client.Close();
                        return msgRec;
                    }
                    catch
                    {
                        Toast.MakeText(this, "No response from target.", ToastLength.Long).Show();
                    }
                    finally
                    {
                        client.Close();
                    }
                }
                return "";
            }
            catch (SocketException e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
                return "";
            }
        }
    }
}