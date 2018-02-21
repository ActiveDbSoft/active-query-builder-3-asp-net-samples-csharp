using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Web;

namespace CustomStorage.Helpers
{
    /// <summary>
    /// Helper methods to work with data and establish database connections.
    /// </summary>
    public static class DataBaseHelper
    {
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