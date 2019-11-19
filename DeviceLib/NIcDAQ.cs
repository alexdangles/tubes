using System;
using System.Data;
using System.Linq;
using System.Threading;
using NationalInstruments;
using NationalInstruments.DAQmx;
using Helper;
using Devices.Properties;

namespace Devices
{
    /// <summary>
    /// National Instruments DAQmx class. Note: DAQmx drivers must be installed for this class to function.
    /// </summary>
    public class NIcDAQ : IDisposable
    {
        private Task analogIn, analogOut, digitalIn, digitalOut;
        public string[] AIChannels, AOChannels, DIChannels, DOChannels;
        public DataTable analogInData = new DataTable();
        public bool[] digitalInData;
        Device device;
        Log log = new Log();
        public bool monitor;

        #region Properties

        /// <summary>
        /// Get names of all connected DAQ devices.
        /// </summary>
        public static string[] deviceList
        {
            get
            {
                return DaqSystem.Local.Devices;
            }
        }
    
        /// <summary>
        /// Get names of all DAQmx tasks in NIMAX.
        /// </summary>
        public static string[] taskList
        {
            get
            {
                return DaqSystem.Local.Tasks;
            }
        }

        /// <summary>
        /// Get the connection state of DAQ.
        /// </summary>
        public bool connected
        {
            get
            {
                return analogIn != null || analogOut != null || digitalIn != null || digitalOut != null;
            }
        }
        #endregion

        #region Constructors


        /// <summary>
        /// Load tasks from NIMax.
        /// </summary>
        /// <param name="tasks">Tasks to load (separate multiple with ,)</param>
        /// <param name="autoTasks">Automatically load all saved tasks</param>
        public NIcDAQ(string tasks, bool autoTasks = false)
        {
            string[] taskList = new string[0];
            if (autoTasks) taskList = NIcDAQ.taskList;
            else if (tasks.Length > 0) taskList = tasks.Split(',');
            foreach (string t in taskList)
            {
                try
                {
                    Task task = new Task();
                    task = DaqSystem.Local.LoadTask(t);
                    if (task.AIChannels.Count > 0)
                    {
                        AIChannels = new string[task.AIChannels.Count];
                        for (int i = 0; i < task.AIChannels.Count; i++)
                        {
                            AIChannels[i] = task.AIChannels[i].VirtualName;
                        }

                        analogIn = task;
                        Thread aiMonitor = new Thread(AIMonitor);
                        aiMonitor.Start();
                    }
                    else if (task.AOChannels.Count > 0)
                    {
                        AOChannels = new string[task.AOChannels.Count];
                        for (int i = 0; i < task.AOChannels.Count; i++)
                        {
                            AOChannels[i] = task.AOChannels[i].VirtualName;
                        }

                        analogOut = task;
                    }
                    else if (task.DIChannels.Count > 0)
                    {
                        DIChannels = new string[task.DIChannels.Count];
                        for (int i = 0; i < task.DIChannels.Count; i++)
                        {
                            DIChannels[i] = task.DIChannels[i].VirtualName;
                        }

                        digitalIn = task;
                        digitalInData = new bool[task.DIChannels.Count];
                        Thread diMonitor = new Thread(DIMonitor);
                        diMonitor.Start();
                    }
                    else if (task.DOChannels.Count > 0)
                    {
                        DOChannels = new string[task.DOChannels.Count];
                        for (int i = 0; i < task.DOChannels.Count; i++)
                        {
                            DOChannels[i] = task.DOChannels[i].VirtualName;
                        }

                        digitalOut = task;
                    }
                }
                catch (DaqException de)
                {
                    log.Write(de.Message);
                }
            }
        }
        #endregion

        #region Methods


        /// <summary>
        /// Monitor analog in data.
        /// </summary>
        private void AIMonitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(100);
                analogInData = AnalogRead().Item2;
            }
        }

        /// <summary>
        /// Monitor digital in data.
        /// </summary>
        private void DIMonitor()
        {
            monitor = true;
            while (monitor)
            {
                Thread.Sleep(100);
                digitalInData = DigitalRead();
            }
        }

        /// <summary>
        /// Load new DAQmx device.
        /// </summary>
        /// <param name="deviceName">Name of DAQ device in NIMAX</param>
        public void LoadDevice(string deviceName)
        {
            try
            {
                device = DaqSystem.Local.LoadDevice(deviceName);
            }
            catch (DaqException de)
            {
                log.Write(de.Message);
            }
        }
       
        /// <summary>
        /// Get analog input channels from AI tasks.
        /// </summary>
        /// <returns>Returns both analog waveform and datatable types.</returns>
        public Tuple<AnalogWaveform<double>[], DataTable> AnalogRead()
        {
            if (analogIn != null)
            {
                try
                {
                    AnalogMultiChannelReader ar = new AnalogMultiChannelReader(analogIn.Stream);
                    AnalogWaveform<double>[] wfmOut = ar.ReadWaveform(DAQ.Default.numOfSamples);
                    DataTable dataOut = new DataTable();
                    int numOfChannels = analogIn.AIChannels.Count;
                    DataColumn[] dataColumn = new DataColumn[numOfChannels];
                    for (int currentChannelIndex = 0; currentChannelIndex < numOfChannels; currentChannelIndex++)
                    {
                        dataColumn[currentChannelIndex] = new DataColumn();
                        dataColumn[currentChannelIndex].DataType = typeof(double);
                        dataColumn[currentChannelIndex].ColumnName = analogIn.AIChannels[currentChannelIndex].VirtualName;
                    }
                    dataOut.Columns.AddRange(dataColumn);
                    for (int currentDataIndex = 0; currentDataIndex < DAQ.Default.numOfSamples; currentDataIndex++)
                    {
                        object[] rowArr = new object[numOfChannels];
                        dataOut.Rows.Add(rowArr);
                    }
                    int currentLineIndex = 0;
                    foreach (AnalogWaveform<double> waveform in wfmOut)
                    {
                        for (int sample = 0; sample < waveform.Samples.Count; ++sample)
                        {
                            if (sample == DAQ.Default.numOfSamples) break;
                            dataOut.Rows[sample][currentLineIndex] = waveform.Samples[sample].Value;
                        }
                        currentLineIndex++;
                    }
                    return new Tuple<AnalogWaveform<double>[], DataTable>(wfmOut, dataOut);
                }
                catch (DaqException de)
                {
                    log.Write(de.Message);
                }
            }
            return new Tuple<AnalogWaveform<double>[], DataTable>(null, null);
        }

        /// <summary>
        /// Perform AO task.
        /// </summary>
        /// <param name="volts">Voltage values to write.</param>
        public void AnalogWrite(params double[] volts)
        {
            if (analogOut != null)
            {
                try
                {
                    AnalogMultiChannelWriter aw = new AnalogMultiChannelWriter(analogOut.Stream);
                    double[] input = new double[analogOut.AOChannels.Count];
                    for (int i = 0; i < input.Length; i++)
                    {
                        try
                        {
                            input[i] = volts[i];
                        }
                        catch
                        {
                            input[i] = 0;
                        }
                    }
                    aw.WriteSingleSample(true, input);
                }
                catch (DaqException de)
                {
                    log.Write(de.Message);
                }
            }
        }

        /// <summary>
        /// Perform DI task.
        /// </summary>
        /// <returns></returns>
        public bool[] DigitalRead()
        {
            if (digitalIn != null)
            {
                try
                {
                    DigitalMultiChannelReader dr = new DigitalMultiChannelReader(digitalIn.Stream);
                    return dr.ReadSingleSampleSingleLine();
                }
                catch (DaqException de)
                {
                    log.Write(de.Message);
                }
            }
            return new bool[0];
        }

        /// <summary>
        /// Perform DO task.
        /// </summary>
        /// <param name="bits">0 or 1 to write digital LOW or HIGH output.</param>
        public void DigitalWrite(params bool[] bits)
        {
            if (digitalOut != null)
            {
                try
                {
                    bool[] input = new bool[digitalOut.DOChannels.Count];
                    for (int i = 0; i < input.Length; i++)
                    {
                        input[i] = bits.Length > i && bits[i];
                    }
                    DigitalMultiChannelWriter dw = new DigitalMultiChannelWriter(digitalOut.Stream);
                    dw.WriteSingleSampleSingleLine(true, input);
                }
                catch (DaqException de)
                {
                    log.Write(de.Message);
                }
            }
        }

        /// <summary>
        /// Averages specified column from analog read data table.
        /// </summary>
        /// <param name="columnIndex">Column number to average</param>
        /// <returns>Returns single average value.</returns>
        public double GetData(string column)
        {
            try
            {
                if (analogInData != null)
                    return
                        analogInData.Rows.Count > 0 &&
                        analogInData.Columns[column] != null ?
                        analogInData.AsEnumerable().Average(x => x.Field<double>(analogInData.Columns[column])) : 0;
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// Stop all running tasks.
        /// </summary>
        public void Disconnect()
        {
            monitor = false;
            if (analogIn != null) analogIn.Stop();
            if (analogOut != null) analogOut.Stop();
            if (digitalIn != null) digitalIn.Stop();
            if (digitalOut != null) digitalOut.Stop();
            Dispose();
        }

        /// <summary>
        /// Release all resources used by class.
        /// </summary>
        public void Dispose()
        {

        }
        #endregion
    }
}