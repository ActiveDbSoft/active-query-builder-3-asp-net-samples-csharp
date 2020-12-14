using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace JavaScript.Controllers
{
    public class AngularClientRenderingController : Controller
    {
        private static string InstanceId = "Angular";

        public ActionResult Index()
        {
            CreateQueryBuilder();

			//Please follow the steps described in the Scripts/Angular/README.md file to run this demo project
            try
            {
                var file = System.IO.File.OpenRead(Server.MapPath("/Scripts/Angular/dist/index.html"));
                return File(file, "text/html");
            }
            catch (IOException)
            {
                return new HttpStatusCodeResult(400, "/Scripts/Angular/dist/index.html not found. Details: Scripts/Angular/README.md");
            }
        }
        
        /// <summary>
        /// Creates and initializes new instance of the QueryBuilder object if it doesn't exist. 
        /// </summary>
        private void CreateQueryBuilder()
        {
            // Get an instance of the QueryBuilder object
            QueryBuilderStore.GetOrCreate(InstanceId, queryBuilder =>
            {
                queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

                // Denies metadata loading requests from the metadata provider
                queryBuilder.MetadataLoadingOptions.OfflineMode = true;

                // Load MetaData from XML document. File name is stored in the "Web.config" file in [/configuration/appSettings/NorthwindXmlMetaData] key
                var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
                var xml = Path.Combine(Server.MapPath("~"), path);

                queryBuilder.MetadataContainer.ImportFromXML(xml);

                //Set default query
                queryBuilder.SQL = GetDefaultSql();
            });
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