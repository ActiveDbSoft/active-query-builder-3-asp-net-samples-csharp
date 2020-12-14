using System;
using System.Configuration;
using System.IO;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public partial class MobileDemo : BasePage
    {
        const string qbId = "Mobile"; // identifies instance of the QueryBuilder object within a session
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get instance of the QueryBuilder object
            var qb = QueryBuilderStore.GetOrCreate(qbId, InitializeQueryBuilder);

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
        }

        /// <summary>
        /// Initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <param name="queryBuilder">Active Query Builder instance.</param>
        private void InitializeQueryBuilder(QueryBuilder queryBuilder)
        {
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            // Denies metadata loading requests from live database connection
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load metadata from XML document. File name stored in the "Web.config" file [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Set default query
            queryBuilder.SQL = GetDefaultSql();
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
    }
}