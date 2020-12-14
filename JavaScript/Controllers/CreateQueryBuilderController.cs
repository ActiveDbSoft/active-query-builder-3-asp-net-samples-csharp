using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace JavaScript.Controllers
{
    public class CreateQueryBuilderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creates and initializes new instance of the QueryBuilder object for the given identifier if it doesn't exist. 
        /// </summary>
        /// <param name="name">Instance identifier of object in the current session.</param>
        /// <returns></returns>
        public ActionResult CreateQueryBuilder(string name)
        {
            // Get an instance of the QueryBuilder object
            QueryBuilderStore.GetOrCreate(name, queryBuilder =>
            {
                queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

                // Denies metadata loading requests from live database connection
                queryBuilder.MetadataLoadingOptions.OfflineMode = true;

                // Load MetaData from XML document. File name is stored in the "Web.config" file in [/configuration/appSettings/NorthwindXmlMetaData] key
                var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
                var xml = Path.Combine(Server.MapPath("~"), path);

                queryBuilder.MetadataContainer.ImportFromXML(xml);

                //Set default query
                queryBuilder.SQL = GetDefaultSql();
            });

            return new EmptyResult();
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