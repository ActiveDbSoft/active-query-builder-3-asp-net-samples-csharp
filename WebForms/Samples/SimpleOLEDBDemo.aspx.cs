using System;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Samples
{
    public partial class SimpleOLEDBDemo : BasePage
    {
        const string qbId = "Simple"; // identifies instance of the QueryBuilder object within a session
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.GetOrCreate	(qbId, InitializeQueryBuilder);

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
            queryBuilder.SyntaxProvider = new MSAccessSyntaxProvider();

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = false;

            // Assign an instance of the syntax provider which defines SQL syntax and metadata retrieval rules.
            queryBuilder.SyntaxProvider = new MSAccessSyntaxProvider();

            // Bind Active Query Builder to a live database connection.
            queryBuilder.MetadataProvider = new OLEDBMetadataProvider
            {
                // Assign an instance of DBConnection object to the Connection property.
                Connection = DataBaseHelper.CreateMSAccessConnection("NorthwindDataBase")
            };

            // Assign the initial SQL query text the user sees on the _first_ page load
            queryBuilder.SQL = GetDefaultSql();
        }

        private string GetDefaultSql()
        {
            return @"Select o.OrderID,
                      c.CustomerID As a1,
                      c.CompanyName,
                      s.ShipperID
                    From (Orders o
                      Inner Join Customers c On o.CustomerID = c.CustomerID)
                      Inner Join Shippers s On s.ShipperID = o.ShipperID
                    Where o.Ship_City = 'A'";
        }
    }
}