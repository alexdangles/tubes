using System;
using System.Windows.Forms;
using Helper.Properties;
using System.Linq;
using System.Drawing;
using System.Threading;

namespace Helper
{
    /// <summary>
    /// Password input user interface.
    /// </summary>
    public partial class frmPassword : Form
    {
        string[] passwordList;
        bool accessGranted;

        /// <summary>
        /// Ask if password has already been entered. Returns yes or no.
        /// </summary>
        public bool askOnce
        {
            get
            {
                if (!accessGranted)
                {
                    if (Environment.MachineName == Settings.Default.MyPCName) txtPassword.Text = passwordList[0];
                    Location = Cursor.Position;
                    ShowDialog();
                }
                return accessGranted;
            }
        }

        /// <summary>
        /// Ask for password every time called.
        /// </summary>
        public bool askAlways
        {
            get
            {
                if (Environment.MachineName == Settings.Default.MyPCName) txtPassword.Text = passwordList[0];
                Location = Cursor.Position;
                ShowDialog();
                if (accessGranted)
                {
                    accessGranted = false;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Initialize password object with passwords stored in settings.
        /// </summary>
        public frmPassword()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            passwordList = Settings.Default.Passwords.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Hide();
        }

        /// <summary>
        /// Initialize password object with passwords in method argument.
        /// </summary>
        public frmPassword(params string[] passwords)
        {
            InitializeComponent();
            passwordList = passwords;
            Hide();
        }

        public static void Shake(Form form)
        {
            var original = form.Location;
            var rnd = new Random(1337);
            const int shake_amplitude = 10;
            for (int i = 0; i < 10; i++)
            {
                form.Location = new Point(original.X + rnd.Next(-shake_amplitude, shake_amplitude), original.Y + rnd.Next(-shake_amplitude, shake_amplitude));
                Thread.Sleep(20);
            }
            form.Location = original;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (passwordList.Any(x => x.Equals(txtPassword.Text)))
                {
                    accessGranted = true;
                    Hide();
                }
                else
                {
                    Text = "Wrong";
                    Shake(this);
                    Thread.Sleep(300);
                    Text = "Enter Password";
                }
                txtPassword.Text = null;
            }
        }

        private void frmPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}