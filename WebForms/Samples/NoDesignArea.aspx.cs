using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public partial class NoDesignArea : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("NoDesignArea");

            if (qb == null)
                qb = CreateQueryBuilder();

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Create("NoDesignArea");

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = true;

            // Assign an instance of the syntax provider which defines SQL syntax and metadata retrieval rules.
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            // Denies metadata loading requests from the metadata provider
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            queryBuilder.BehaviorOptions.DeleteUnusedObjects = true;
            queryBuilder.BehaviorOptions.AddLinkedObjects = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private string GetDefaultSql()
        {
            return @"Select o.OrderID, c.CustomerID, s.ShipperID, o.ShipCity
                        From Orders o Inner Join
                          Customers c On o.Customer_ID = c.ID Inner Join
                          Shippers s On s.ID = o.Shipper_ID
                        Where o.ShipCity = 'A'";
        }
    }
}