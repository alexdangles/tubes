using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// Class for logging and saving data.
    /// </summary>
    public class Log : IDisposable
    {
        #region Fields


        bool disposing;
        public static string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string timeStamp = DateTime.Now.ToString("yyyy-M-dd_HH-mm-ss");

        DateTime startTime = DateTime.Now;
        Stopwatch timer = new Stopwatch();

        /// <summary>
        /// Character which specifies data separation in log.
        /// </summary>
        public char delimiter = '\t';

        /// <summary>
        /// Location on PC to save data file.
        /// </summary>
        public string saveLocation;

        /// <summary>
        /// Time interval in seconds between automatic saving.
        /// </summary>
        public double saveFrequency_sec;

        /// <summary>
        /// Each log is recoreded to console if true.
        /// </summary>
        public bool toConsole;

        /// <summary>
        /// Format to display time in log.
        /// </summary>
        public string timeFormat = "MM/dd/yy HH:mm:ss.fff";
        /// <summary>
        /// Queue to save data in native format.
        /// </summary>
        public Queue data = new Queue();

        /// <summary>
        /// Queue to save data in text format.
        /// </summary>
        public Queue log = new Queue();

        #endregion

        #region Constructors


        /// <summary>
        /// Activate logger.
        /// </summary>
        public Log()
        {

        }

        /// <summary>
        /// Activate logger with save intervals.
        /// </summary>
        /// <param name="saveLocation">File location to store log.</param>
        /// <param name="saveFrequency_sec">Frequency of saving data to file in seconds.</param>
        public Log(string saveLocation, double saveFrequency_sec)
        {
            this.saveLocation = saveLocation;
            if (!Directory.Exists(saveLocation))
            {
                try
                {
                    Directory.CreateDirectory($@"{ saveLocation }\..\");
                }
                catch
                {
                    Console.WriteLine("Could not create log file.");
                }
            }
            try
            {
                File.WriteAllText(saveLocation, "");
            }
            catch
            {
                Console.WriteLine("File is busy.");
            }
            this.saveFrequency_sec = saveFrequency_sec;

            if (saveFrequency_sec > 0)
            {
                timer.Start();
                Thread autosave = new Thread(AutoSave);
                autosave.Start();
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Add header text to distinguish columns in log.
        /// </summary>
        /// <param name="columnNames">Names of columns to be added to header.</param>
        public void AddHeaders(params string[] columnNames)
        {
            if (saveLocation != null)
            {
                string header = "";
                foreach (string column in columnNames)
                {
                    header += $"{ column }{ delimiter }";
                }
                header += "\r\n";
                try
                {
                    File.AppendAllText(saveLocation, header);
                }
                catch
                {
                    Console.WriteLine("File is busy.");
                }

            }
        }

        /// <summary>
        /// Show log in text editor.
        /// </summary>
        public void ShowInNotepad()
        {
            if (saveLocation != null) Process.Start(saveLocation, "notepad.exe");
        }

        /// <summary>
        /// Clear log.
        /// </summary>
        public void Clear()
        {
            log.Clear();
            data.Clear();
            if (saveLocation != null)
            {
                try
                {
                    File.WriteAllText(saveLocation, "");
                }
                catch
                {
                    Console.WriteLine("File is busy.");
                }
            }
        }

        /// <summary>
        /// Record to log.
        /// </summary>
        /// <param name="items">Data items to record.</param>
        /// <returns></returns>
        public string Write(params object[] items)
        {
            List<object> dataOut = new List<object>();
            StringBuilder txtOut = new StringBuilder();

            foreach (var item in items)
            {
                Type t = item.GetType();
                if (t == typeof(DateTime))
                {
                    DateTime dt = (DateTime)item;
                    dataOut.Add(dt);
                    txtOut.Append($"{ dt.ToString(timeFormat) }{ delimiter }");
                }
                else
                {
                    if (t.IsArray)
                    {
                        Array arr = item as Array;
                        for (int i = 0; i < arr.Length; i++)
                        {
                            object data = arr.GetValue(i);
                            dataOut.Add(data);
                            if (item.GetType() == typeof(double))
                            {
                                double dData = Convert.ToDouble(data);
                                txtOut.Append($"{ dData.ToString("0.000") }{ delimiter }");
                            }
                            else
                            {
                                txtOut.Append($"{ data.ToString() }{ delimiter }");
                            }
                        }
                    }
                    else
                    {
                        dataOut.Add(item);
                        if (item.GetType() == typeof(double))
                        {
                            double dItem = Convert.ToDouble(item);
                            txtOut.Append($"{ dItem.ToString("0.000") }{ delimiter }");
                        }
                        else
                        {
                            txtOut.Append($"{ item.ToString() }{ delimiter }");
                        }
                    }
                }
            }
            log.Enqueue(txtOut.ToString());
            data.Enqueue(dataOut);

            if (toConsole) Console.WriteLine(txtOut);
            return txtOut.ToString() + "\r\n";
        }

        /// <summary>
        /// Autosave for background thread.
        /// </summary>
        private void AutoSave()
        {
            while (timer.IsRunning && !disposing)
            {
                Thread.Sleep(100);

                if (timer.ElapsedMilliseconds > (int)(saveFrequency_sec * 1000))
                {
                    if (saveLocation != null) SaveToFile(saveLocation);
                    timer.Restart();
                }
            }
        }

        /// <summary>
        /// Save current log to text file. (Only log data since previous save will be appended.)
        /// </summary>
        /// <param name="fileName">Log file location.</param>
        public async void SaveToFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        string allData = "";
                        for (int i = 0; i < log.Count; i++)
                        {
                            allData += $"{ log.Dequeue().ToString() }\r\n";
                        }
                        File.AppendAllText(fileName, allData);
                    }
                    catch
                    {
                        Console.WriteLine("Failed to save to file.");
                    }
                });
            }
        }

        /// <summary>
        /// Save current log to database. (Only log data since previous save will be appended.)
        /// </summary>
        /// <param name="tableName">Name of table to store data. (Note: database connection must be configured in app settings).</param>
        public async void SaveToDB(string tableName)
        {
            await Task.Run(() =>
            {
                try
                {
                    object[] allData = new object[data.Count];
                    for (int i = 0; i < data.Count; i++)
                    {
                        allData = (object[])data.Dequeue();
                    }
                    DBHelper db = new DBHelper();
                    db.Insert(tableName, allData);
                    db.Disconnect();
                }
                catch
                {
                    Console.WriteLine("Failed to save to DB.");
                }
            });
        }

        /// <summary>
        /// Stop saving data at regular intervals.
        /// </summary>
        public void Dispose()
        {
            disposing = true;
            timer.Stop();
            if (saveLocation != null) SaveToFile(saveLocation);
        }
        #endregion
    }
}