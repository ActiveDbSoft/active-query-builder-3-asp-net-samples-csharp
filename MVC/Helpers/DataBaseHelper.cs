using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Data.SQLite;
using MVC_Samples.Controllers;

namespace MVC_Samples.Helpers
{
    /// <summary>
    /// Helper methods to work with data and establish database connections.
    /// </summary>
    public static class DataBaseHelper
    {
        /// <summary>
        /// Executes a query and returns the result data as a list of records. Each record is represented as a list of field name-value pairs.
        /// </summary>
        /// <param name="conn">The DB connection object.</param>
        /// <param name="sql">The SQL query text.</param>
        /// <returns>List of records.</returns>
        public static List<Dictionary<string, object>> GetData(IDbConnection conn, string sql, Param[] parameters)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            if (string.IsNullOrEmpty(sql))
                return new List<Dictionary<string, object>>();

            if (parameters != null)
                AddParameters(cmd, parameters);

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                IDataReader reader = cmd.ExecuteReader();
                return ConvertToList(reader);
            }
            finally
            {
                conn.Close();
            }
        }

        private static void AddParameters(IDbCommand cmd, Param[] parameters)
        {
            foreach (var p in parameters)
            {
                var param = cmd.CreateParameter();
                param.DbType = p.DataType;
                param.ParameterName = p.Name;
                param.Value = p.Value;

                cmd.Parameters.Add(param);
            }
        }

        /// <summary>
        /// Saves data from IDataReader to the list. Each record is represented as a list of field name-value pairs.
        /// </summary>
        /// <param name="reader">The Data Reader object.</param>
        /// <returns>List of records.</returns>
        private static List<Dictionary<string, object>> ConvertToList(IDataReader reader)
        {
            var result = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    row.Add(reader.GetName(i), reader[i]); 

                result.Add(row);
            }

            return result;
        }

        /// <summary>
        /// Executes a query and returns the result data as a list of values lists. Each record is represented as a list of values.
        /// </summary>
        /// <param name="conn">The DB connection object.</param>
        /// <param name="sql">The SQL query text.</param>
        /// <returns>List of values lists.</returns>
        public static List<List<object>> GetDataList(IDbConnection conn, string sql)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            if (string.IsNullOrEmpty(sql))
                return new List<List<object>>();

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                IDataReader reader = cmd.ExecuteReader();
                return Convert(reader);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Saves data from IDataReader to the list. Each record is represented as a list of values.
        /// </summary>
        /// <param name="reader">The Data Reader object.</param>
        /// <returns>List of values lists.</returns>
        private static List<List<object>> Convert(IDataReader reader)
        {
            var result = new List<List<object>>();

            while (reader.Read())
            {
                var row = new List<object>();

                for (int i = 0; i < reader.FieldCount; i++)
                    row.Add(reader[i]);

                result.Add(row);
            }

            return result;
        }

        /// <summary>
        /// Creates DBConnection object for MS Access database.
        /// </summary>
        /// <param name="AConfigurationName">Name of database configuration stored in the Web.Config file.</param>
        /// <returns>Returns an instance of OLEDBConnection.</returns>
        public static IDbConnection CreateMSAccessConnection(string AConfigurationName)
        {
            //var provider = "Microsoft.ACE.OLEDB.12.0";
            var provider = "Microsoft.Jet.OLEDB.4.0";

            // File name stored in the "/configuration/appSettings/<configuration name>" key
            var path = ConfigurationManager.AppSettings[AConfigurationName];
            var file = Path.Combine(HttpContext.Current.Server.MapPath("~"), path);
            var connectionString = string.Format("Provider={0};Data Source={1};Persist Security Info=False;", provider, file);

            return new OleDbConnection(connectionString);
        }

        /// <summary>
        /// Creates DBConnection object for SQLite database.
        /// </summary>
        /// <param name="AConfigurationName">Name of database configuration stored in the Web.Config file.</param>
        /// <returns>Returns an instance of SQLiteConnection.</returns>
        public static IDbConnection CreateSqLiteConnection(string AConfigurationName)
        {
            // File name stored in the "/configuration/appSettings/<configuration name>" key
            var path = ConfigurationManager.AppSettings[AConfigurationName];
            var file = Path.Combine(HttpContext.Current.Server.MapPath("~"), path);

            var connectionString = string.Format("Data Source={0};Version=3;", file);

            return new SQLiteConnection(connectionString);
        }
    }
}