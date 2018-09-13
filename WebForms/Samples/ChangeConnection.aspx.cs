using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Samples
{
    public partial class ChangeConnection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("ChangeConnection");

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
            var qb = QueryBuilderStore.Create("ChangeConnection");

            SetNorthwindXml(qb);

            return qb;
        }

        private void SetNorthwindXml(QueryBuilder qb)
        {
            qb.MetadataLoadingOptions.OfflineMode = true;
            qb.SyntaxProvider = new MSSQLSyntaxProvider();

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            qb.MetadataContainer.ImportFromXML(xml);
            qb.MetadataStructure.Refresh();
        }

        private void SetDb2Xml(QueryBuilder qb)
        {
            qb.MetadataLoadingOptions.OfflineMode = true;
            qb.SyntaxProvider = new DB2SyntaxProvider();

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["Db2XmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            qb.MetadataContainer.ImportFromXML(xml);
            qb.MetadataStructure.Refresh();
        }

        private void SetSqLite(QueryBuilder qb)
        {
            qb.MetadataLoadingOptions.OfflineMode = false;
            qb.SyntaxProvider = new SQLiteSyntaxProvider();
            qb.MetadataProvider = new SQLiteMetadataProvider
            {
                Connection = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase")
            };

            qb.MetadataStructure.Refresh();
        }

        protected void OnClick(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            Change(btn.Text);
        }

        public void Change(string name)
        {
            var queryBuilder = QueryBuilderStore.Get("ChangeConnection");

            queryBuilder.MetadataContainer.Clear();

            if (name == "NorthwindXmlMetaData")
                SetNorthwindXml(queryBuilder);
            else if (name == "SQLite")
                SetSqLite(queryBuilder);
            else
                SetDb2Xml(queryBuilder);
        }
    }
}