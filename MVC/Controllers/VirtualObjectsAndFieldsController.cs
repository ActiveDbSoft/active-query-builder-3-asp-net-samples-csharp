using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class VirtualObjectsAndFieldsController : Controller
    {
        private const string InstanceId = "VirtualObjectsAndFields";

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.GetOrCreate(InstanceId, InitializeQueryBuilder);

            return View(qb);
        }

        /// <summary>
        /// Initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <param name="queryBuilder">Active Query Builder instance.</param>
        private void InitializeQueryBuilder(QueryBuilder queryBuilder)
        {
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = true;
            
            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            MetadataObject o;
            MetadataField f;

            // Virtual fields for real object
            // ===========================================================================
            o = queryBuilder.MetadataContainer.FindItem<MetadataObject>("Orders");

            // first test field - simple expression
            f = o.AddField("OrderId_plus_1");
            f.Expression = "orders.OrderId + 1";

            // second test field - correlated sub-query
            f = o.AddField("CustomerCompanyName");
            f.Expression = "(select c.CompanyName from Customers c where c.CustomerId = orders.CustomerId)";

            // Virtual object (table) with virtual fields
            // ===========================================================================

            o = queryBuilder.MetadataContainer.AddTable("MyOrders");
            o.Expression = "Orders";

            // first test field - simple expression
            f = o.AddField("OrderId_plus_1");
            f.Expression = "MyOrders.OrderId + 1";

            // second test field - correlated sub-query
            f = o.AddField("CustomerCompanyName");
            f.Expression = "(select c.CompanyName from Customers c where c.CustomerId = MyOrders.CustomerId)";

            // Virtual object (sub-query) with virtual fields
            // ===========================================================================

            o = queryBuilder.MetadataContainer.AddTable("MyBetterOrders");
            o.Expression = "(select OrderId, CustomerId, OrderDate from Orders)";

            // first test field - simple expression
            f = o.AddField("OrderId_plus_1");
            f.Expression = "MyBetterOrders.OrderId + 1";

            // second test field - correlated sub-query
            f = o.AddField("CustomerCompanyName");
            f.Expression = "(select c.CompanyName from Customers c where c.CustomerId = MyBetterOrders.CustomerId)";

            queryBuilder.SQLQuery.SQLUpdated += OnSQLUpdated;

            queryBuilder.SQL = "SELECT mbo.OrderId_plus_1, mbo.CustomerCompanyName FROM MyBetterOrders mbo";
        }

        public void OnSQLUpdated(object sender, EventArgs e)
        {
            var qb = QueryBuilderStore.Get(InstanceId);

            var opts = new SQLFormattingOptions();

            opts.Assign(qb.SQLFormattingOptions);
            opts.KeywordFormat = KeywordFormat.UpperCase;

            // get query with virtual objects and fields
            opts.ExpandVirtualObjects = false;
            var sqlWithVirtObjects = FormattedSQLBuilder.GetSQL(qb.SQLQuery.QueryRoot, opts);

            // get SQL query with real object names
            opts.ExpandVirtualObjects = true;
            var plainSql = FormattedSQLBuilder.GetSQL(qb.SQLQuery.QueryRoot, opts);

            // prepare additional data to be sent to the client
            qb.ExchangeData = new
            {
                SQL = plainSql,
                VirtualObjectsSQL = sqlWithVirtObjects
            };
        }
    }
}