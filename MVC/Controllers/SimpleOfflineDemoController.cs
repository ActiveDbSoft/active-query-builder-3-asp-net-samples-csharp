using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class SimpleOfflineDemoController : Controller
    {
        const string qbId = "Offline"; // identifies instance of the QueryBuilder object within a session

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(qbId);

            if (qb == null)
                qb = CreateQueryBuilder(qbId);

            return View(qb);
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
            
            // Create an instance of the proper syntax provider for your database server.
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            // Denies metadata loading requests from live database connection
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load metadata from XML document. File name stored in the "Web.config" file [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Set default query
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
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