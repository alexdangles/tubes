using System;
using System.Data.SqlClient;
using System.Data;
using Helper.Properties;

namespace Helper
{
    /// <summary>
    /// Database class for search and modification using SQL commands.
    /// </summary>
    public class DBHelper : IDisposable
    {
        #region Fields


        public enum SQLDataType
        {
            VARCHAR_255,
            VARCHAR_100,
            VARCHAR_50,
            DATETIME,
            BIT,
            INT,
            REAL,
            FLOAT
        }

        public SqlConnection connection = new SqlConnection(Settings.Default.ConnectionString);
        public DataTable table;
        #endregion

        #region Constructors


        /// <summary>
        /// Setup DB connection using connection string in settings.
        /// </summary>
        public DBHelper()
        {
            OpenConnection();
        }
        #endregion

        #region Properties


        /// <summary>
        /// Get database connection status.
        /// </summary>
        public bool connected
        {
            get
            {
                return connection.State == ConnectionState.Open;
            }
        }
        #endregion

        #region Methods


        /// <summary>
        /// Send command for Insert, Delete or alter table.
        /// </summary>
        /// <param name="command">Command to send</param>
        /// <returns></returns>
        private bool SendCommand(string command, bool closeConnection = true)
        {
            using (SqlCommand cmd = new SqlCommand($"{ command };", connection))
            {
                try
                {
                    if (!connected) OpenConnection();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                if (closeConnection) CloseConnection();
                return false;
            }
        }

        /// <summary>
        /// Convert SQL datatype to string command.
        /// </summary>
        /// <param name="type">SQL datatype to convert</param>
        /// <returns></returns>
        private string Convert_SQLTypetoString(SQLDataType type)
        {
            switch (type)
            {
                default:
                    return type.ToString();
                case SQLDataType.VARCHAR_50:
                    return "varchar(50)";
                case SQLDataType.VARCHAR_100:
                    return "varchar(100)";
                case SQLDataType.VARCHAR_255:
                    return "varchar(255)";
            }
        }

        /// <summary>
        /// Query database with SQL statement.
        /// </summary>
        /// <param name="query">SQL query statement.</param>
        /// <returns></returns>
        public static DataTable Query(string query)
        {
            SqlConnection con = new SqlConnection(Settings.Default.ConnectionString);
            using (SqlCommand cmd = new SqlCommand($"{ query };", con))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    try
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        if (dataTable.Rows.Count > 0) return dataTable;
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return null;

                }
            }
        }

        /// <summary>
        /// Open DB connection.
        /// </summary>
        public void OpenConnection()
        {
            if (!connected) connection.Open();
        }

        /// <summary>
        /// Close DB connection.
        /// </summary>
        public void CloseConnection()
        {
            if (connected) connection.Close();
        }

        /// <summary>
        /// Insert data into SQL table.
        /// </summary>
        /// <param name="tableName">Name of table to insert data into.</param>
        /// <param name="data">Data to insert (multiple data types allowed).</param>
        /// <returns>Returns true if success, false if error.</returns>
        public bool Insert(string tableName, params object[] data)
        {
            return SendCommand($"insert into { tableName } values('{ string.Join("','", data) }')", false);
        }

        /// <summary>
        /// Update value of column.
        /// </summary>
        /// <param name="tableName">Name of table to update.</param>
        /// <param name="column">Column name to update.</param>
        /// <param name="value">Value of cell.</param>
        /// <param name="condition">Choose which rows to update based on condition.</param>
        /// <returns></returns>
        public bool Update(string tableName, string column, object value, string condition)
        {
            return SendCommand($"update { tableName } set { column } = '{ value }' where { condition }", false);
        }

        /// <summary>
        /// Create a new table with columns in the database.
        /// </summary>
        /// <param name="tableName">Name of table to create</param>
        /// <param name="columnsAndDataType">Columns and data type of columns (Format: columnName dataType, example: Name varchar(50), Age int)</param>
        /// <returns>Returns true if success, false if error.</returns>
        public bool CreateTable(string tableName, params string[] columnsAndDataType)
        {
            return SendCommand($"create table { tableName }({ string.Join(",", columnsAndDataType) })");
        }

        /// <summary>
        /// Create a new table with auto incrementing RowID column as PK.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool CreateTable(string tableName)
        {
            return SendCommand($"create table { tableName } (RowID int identity(1,1) primary key)");
        }

        /// <summary>
        /// Delete table from database. USE CAREFULLY!!!
        /// </summary>
        /// <param name="tableName">Table to delete</param>
        /// <returns></returns>
        public bool DeleteTable(string tableName)
        {
            frmPassword pwd = new frmPassword();
            if (pwd.askOnce)
            {
                return SendCommand($"drop table { tableName }");
            }
            return false;
        }

        /// <summary>
        /// Create single column in existing database table.
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="columnName">Name of column to make</param>
        /// <param name="dataType">SQL data type of column</param>
        /// <returns></returns>
        public bool CreateColumn(string tableName, string columnName, SQLDataType dataType)
        {
            return SendCommand($"alter table { tableName } add { columnName } { Convert_SQLTypetoString(dataType) }");
        }

        /// <summary>
        /// Delete column from table.
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="columnName">Name of column</param>
        /// <returns></returns>
        public bool DeleteColumn(string tableName, string columnName)
        {
            frmPassword pwd = new frmPassword();
            if (pwd.askOnce)
            {
                return SendCommand($"alter table { tableName } drop column { columnName }");
            }
            return false;
        }

        /// <summary>
        /// Search for a specific string in a table.
        /// </summary>
        /// <param name="tableName">Name of search table</param>
        /// <param name="columnName">Name of search column</param>
        /// <param name="searchID">Search keyword</param>
        /// <returns>Returns new datatable containing results of search</returns>
        public static DataTable SearchTable(string tableName, string columnName, object searchID)
        {
            try
            {
                bool stringType = searchID.GetType() == typeof(string);
                string query = $"select * from { tableName } where { columnName }";
                query += stringType ? $" like '{ searchID }'" : $" = { searchID }";
                return Query(query);
            }
            catch
            {

            }
            return null;

        }

        public void Disconnect()
        {
            Dispose();
        }

        public void Dispose()
        {
            CloseConnection();
        }
        #endregion
    }
}