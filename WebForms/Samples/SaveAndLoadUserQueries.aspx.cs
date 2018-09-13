using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public partial class SaveAndLoadUserQueries : BasePage
    {
        //CUT:STD{{
        protected global::ActiveQueryBuilder.Web.WebForms.UserQueries UserQueries1;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("SaveAndLoadUserQueries");

            if (qb == null)
                qb = CreateQueryBuilder();

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
            UserQueries1.QueryBuilder = qb;
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.MsSql("SaveAndLoadUserQueries");
            
            // Denies metadata loading requests from the metadata provider
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            ImportUserQueries(queryBuilder.UserQueries);

            // Subscribe to changing of the user queries container
            queryBuilder.UserQueries.Changed += UserQueriesChanged;

            //Set default query
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private void UserQueriesChanged(object sender, MetadataStructureItem item)
        {
            var container = (UserQueriesContainer)sender;
            container.ExportToXML(Server.MapPath("UserQueriesStructure.xml"));
        }

        private void ImportUserQueries(UserQueriesContainer uqc)
        {
            var file = Server.MapPath("UserQueriesStructure.xml");

            if (File.Exists(file))
                uqc.ImportFromXML(file);
        }

        private string GetDefaultSql()
        {
            return @"Select o.OrderID,
                        c.CustomerID,
                        s.ShipperID,
                        o.ShipCity
                    From Orders o
                        Inner Join Customers c On o.CustomerID = c.CustomerID
                        Inner Join Shippers s On s.ShipperID = o.OrderID
                    Where o.ShipCity = 'A'";
        }
        //}}CUT:STD
    }
}