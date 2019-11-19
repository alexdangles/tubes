using Helper;
using System.IO.Ports;

namespace Devices
{
    /// <summary>
    /// Stanford Research Systems current amplifier class.
    /// </summary>
    public class SRS : Link
    {
        #region Fields

        public enum Scaler {
            _1pA_V,
            _2pA_V,
            _5pA_V,
            _10pA_V,
            _20pA_V,
            _50pA_V,
            _100pA_V,
            _200pA_V,
            _500pA_V,
            _1nA_V,
            _2nA_V,
            _5nA_V,
            _10nA_V,
            _20nA_V,
            _50nA_V,
            _100nA_V,
            _200nA_V,
            _500nA_V,
            _1uA_V,
            _2uA_V,
            _5uA_V,
            _10uA_V,
            _20uA_V,
            _50uA_V,
            _100uA_V,
            _200uA_V,
            _500uA_V,
            _1mA_V
        }
        Scaler _scaler;
        #endregion

        #region Constructors


        /// <summary>
        /// Connect to amplifier.
        /// </summary>
        /// <param name="port">COM port which amplifier is connected</param>
        public SRS(string port = "COM1") : base(new SerialPort(port, 9600, Parity.None, 8, StopBits.Two))
        {
            msToWait = 50;
            scaler = Scaler._2pA_V;
        }
        #endregion

        #region Properties


        /// <summary>
        /// Get connection status.
        /// </summary>
        public bool connected
        {
            get
            {
                return portOpen;
            }
        }

        /// <summary>
        /// Get or set amp scaler value.
        /// </summary>
        public Scaler scaler
        {
            get
            {
                return _scaler;
            }
            set
            {
                _scaler = value;
                Send((int)scaler);
            }
        }

        /// <summary>
        /// Get current scaler value.
        /// </summary>
        public double[] currentScaler
        {
            get
            {
                return new double[] {
                    1E-12,
                    2E-12,
                    5E-12,
                    1E-11,
                    2E-11,
                    5E-11,
                    1E-10,
                    2E-10,
                    5E-10,
                    1E-9,
                    2E-9,
                    5E-9,
                    1E-8,
                    2E-8,
                    5E-8,
                    1E-7,
                    2E-7,
                    5E-7,
                    1E-6,
                    2E-6,
                    5E-6,
                    1E-5,
                    2E-5,
                    5E-5,
                    0.0001,
                    0.0002,
                    0.0005,
                    0.001
                };
            }
        }

        #endregion

        #region Methods


        private void Send(int CMD)
        {
            Tx($"SENS { CMD }{ CR }{ LF }");
        }
        /// <summary>
        /// Set autoscale of input.
        /// </summary>
        /// <param name="input">Input signal to autoscale</param>
        public void AutoScale(double input)
        {
            int newScaler = (int)scaler;

            if (newScaler > 26) newScaler = 26;
            else if (newScaler < 0) newScaler = 0;

            if (input > 5.500001 && newScaler < 27)
            {
                newScaler++;
                Send(newScaler);
            }
            else if (input < 0.0100001 && newScaler > 0)
            {
                newScaler--;
                Send(newScaler);
            }

            if (newScaler > 26) newScaler = 26;
            else if (newScaler < 1) newScaler = 1;

            _scaler = (Scaler)newScaler;
        }

        /// <summary>
        /// Disconnect amplifier.
        /// </summary>
        public void Disconnect()
        {
            Dispose();
        }

        #endregion
    }
}
