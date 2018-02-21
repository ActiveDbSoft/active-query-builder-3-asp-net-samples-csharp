using System;
using System.Web.UI;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Samples
{
    public partial class SimpleOLEDBDemo : Page
    {
        const string qbId = "Simple"; // identifies instance of the QueryBuilder object within a session
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(qbId);

            if (qb == null)
                qb = CreateQueryBuilder(qbId);

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
        }

        /// <summary>
        /// Creates and initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <param name="AInstanceId">String which uniquely identifies an instance of Active Query Builder in the session.</param>
        /// <returns>Returns instance of the QueryBuilder object.</returns>
        private QueryBuilder CreateQueryBuilder(string AInstanceId)
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Create(AInstanceId);

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

            return queryBuilder;
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