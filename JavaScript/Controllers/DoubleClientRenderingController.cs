using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;

namespace JavaScript.Controllers
{
    public class DoubleClientRenderingController : Controller
    {
        public ActionResult Index()
        {
            CreateFirstQueryBuilder();
            CreateSecondQueryBuilder();

            return View();
        }

        /// <summary>
        /// Creates and initializes the first instance of the QueryBuilder object if it doesn't exist. 
        /// </summary>
        private void CreateFirstQueryBuilder()
        {
            // Get an instance of the QueryBuilder object
            QueryBuilderStore.GetOrCreate("FirstClient", queryBuilder =>
            {
                queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

                // Denies metadata loading requests from the metadata provider
                queryBuilder.MetadataLoadingOptions.OfflineMode = true;

                // Load MetaData from XML document. File name is stored in the "Web.config" file in [/configuration/appSettings/NorthwindXmlMetaData] key
                var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
                var xml = Path.Combine(Server.MapPath("~"), path);

                queryBuilder.MetadataContainer.ImportFromXML(xml);
            });
        }

        /// <summary>
        /// Creates and initializes the second instance of the QueryBuilder object if it doesn't exist. 
        /// </summary>
        private void CreateSecondQueryBuilder()
        {
            // Get an instance of the QueryBuilder object
            QueryBuilderStore.GetOrCreate("SecondClient", queryBuilder =>
            {
                queryBuilder.SyntaxProvider = new DB2SyntaxProvider();

                // Denies metadata loading requests from the metadata provider
                queryBuilder.MetadataLoadingOptions.OfflineMode = true;

                // Load MetaData from XML document. File name is stored in the "Web.config" file in [/configuration/appSettings/NorthwindXmlMetaData] key
                var path = ConfigurationManager.AppSettings["Db2XmlMetaData"];
                var xml = Path.Combine(Server.MapPath("~"), path);

                queryBuilder.MetadataContainer.ImportFromXML(xml);
            });
        }
    }
}