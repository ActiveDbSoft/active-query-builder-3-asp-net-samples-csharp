using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public partial class ToggleUseAltNames : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("ToggleUseAltNames");

            if (qb == null)
                qb = CreateQueryBuilder();

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.DB2("ToggleUseAltNames");

            // Turn displaying of alternate names on in the text of result SQL query
            queryBuilder.SQLFormattingOptions.UseAltNames = false;

            // Turn displaying of alternate names on in the visual UI
            queryBuilder.SQLGenerationOptions.UseAltNames = false;
            
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

        protected void OnCheckedChanged(object sender, EventArgs e)
        {
            Toggle();
        }

        public void Toggle()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("ToggleUseAltNames");

            qb.SQLFormattingOptions.UseAltNames = !qb.SQLFormattingOptions.UseAltNames;
            qb.SQLGenerationOptions.UseAltNames = !qb.SQLGenerationOptions.UseAltNames;

            // Reload metadata structure to refill it with real or alternate names.
            // Note: reloading the structure does not reload the metadata container. 
            qb.MetadataStructure.Refresh();
        }
    }
}