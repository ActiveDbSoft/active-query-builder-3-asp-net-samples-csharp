using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class AlternateNamesController : Controller
    {
        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("AlternateNames");

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.DB2("AlternateNames");

            // Turn displaying of alternate names on in the text of result SQL query
            queryBuilder.SQLFormattingOptions.UseAltNames = true;

            // Turn displaying of alternate names on in the visual UI
            queryBuilder.SQLGenerationOptions.UseAltNames = true;
            
            queryBuilder.SQLQuery.SQLUpdated += OnSQLUpdated;

            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["Db2XmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Set default query
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private string GetDefaultSql()
        {
            return @"Select ""Employees"".""Employee ID"", ""Employees"".""First Name"", ""Employees"".""Last Name"", ""Employee Photos"".""Photo Image"", ""Employee Resumes"".Resume From ""Employee Photos"" Inner Join
			""Employees"" On ""Employee Photos"".""Employee ID"" = ""Employees"".""Employee ID"" Inner Join
			""Employee Resumes"" On ""Employee Resumes"".""Employee ID"" = ""Employees"".""Employee ID""";
        }

        public void OnSQLUpdated(object sender, EventArgs e)
        {
            var qb = QueryBuilderStore.Get("AlternateNames");

            var opts = new SQLFormattingOptions();

            opts.Assign(qb.SQLFormattingOptions);
            opts.KeywordFormat = KeywordFormat.UpperCase;

            // get SQL query with real object names
            opts.UseAltNames = false;
            var plainSql = FormattedSQLBuilder.GetSQL(qb.SQLQuery.QueryRoot, opts);

            // get SQL query with alternate names
            opts.UseAltNames = true;
            var sqlWithAltNames = FormattedSQLBuilder.GetSQL(qb.SQLQuery.QueryRoot, opts);

            // prepare additional data to be sent to the client
            qb.ExchangeData = new
            {
                SQL = plainSql,
                AlternateSQL = sqlWithAltNames
            };
        }
    }
}