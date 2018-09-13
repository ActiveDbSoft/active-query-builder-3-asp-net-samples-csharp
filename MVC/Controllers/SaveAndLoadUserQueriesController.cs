using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class SaveAndLoadUserQueriesController : Controller
    {
        private const string id = "SaveAndLoadUserQueries";
        private const string filename = "UserQueriesStructure.xml";
//CUT:STD{{        
        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(id);

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.MsSql(id);
            
            // Denies metadata loading requests from the metadata provider
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Comment these 2 lines for using browser localStorage
            ImportUserQueriesFromFile(queryBuilder.UserQueries);
            queryBuilder.UserQueries.Changed += ExportUserQueriesToFile;

            //Set default query
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private void ExportUserQueriesToFile(object sender, MetadataStructureItem item)
        {
            var uq = (UserQueriesContainer)sender;
            uq.ExportToXML(Server.MapPath("~/" + filename));
        }
        
        private void ImportUserQueriesFromFile(UserQueriesContainer uqc)
        {
            var file = Server.MapPath("~/" + filename);

            if (System.IO.File.Exists(file))
                uqc.ImportFromXML(file);
        }

        public void GetUserQueriesXml()
        {
            var qb = QueryBuilderStore.Get(id);
            qb.UserQueries.ExportToXML(Response.OutputStream);
        }

        [ValidateInput(false)]
        public void LoadUserQueries(string xml)
        {
            var qb = QueryBuilderStore.Get(id);
            qb.UserQueries.XML = xml;
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