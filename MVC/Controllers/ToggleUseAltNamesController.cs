using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class ToggleUseAltNamesController : Controller
    {
        private const string InstanceId = "ToggleUseAltNames";

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.GetOrCreate(InstanceId, InitializeQueryBuilder);
            return View(qb);
        }

        public ActionResult Toggle()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(InstanceId);

            qb.SQLFormattingOptions.UseAltNames = !qb.SQLFormattingOptions.UseAltNames;

            // Reload metadata structure to refill it with real or alternate names.
            // Note: reloading the structure does not reload the metadata container. 
            qb.MetadataStructure.Refresh();

            return new EmptyResult();
        }

        /// <summary>
        /// Initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <param name="queryBuilder">Active Query Builder instance.</param>
        private void InitializeQueryBuilder(QueryBuilder queryBuilder)
        {
            queryBuilder.SyntaxProvider = new DB2SyntaxProvider();

            queryBuilder.SQLFormattingOptions.UseAltNames = false;
            
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["Db2XmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Set default query
            queryBuilder.SQL = GetDefaultSql();
        }

        private string GetDefaultSql()
        {
            return @"Select ""Employees"".""Employee ID"", ""Employees"".""First Name"", ""Employees"".""Last Name"", "+
                @"""Employee Photos"".""Photo Image"", ""Employee Resumes"".Resume From ""Employee Photos"" Inner Join "+
                @"""Employees"" On ""Employee Photos"".""Employee ID"" = ""Employees"".""Employee ID"" Inner Join " +
                @"""Employee Resumes"" On ""Employee Resumes"".""Employee ID"" = ""Employees"".""Employee ID""";
        }
    }
}