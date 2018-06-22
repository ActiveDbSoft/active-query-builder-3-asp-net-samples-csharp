using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class SaveAndLoadUserQueriesController : Controller
    {
//CUT:STD{{        
        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("SaveAndLoadUserQueries");

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
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

            if (System.IO.File.Exists(file))
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