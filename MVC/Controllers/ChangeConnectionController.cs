using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using MVC_Samples.Helpers;

namespace MVC_Samples.Controllers
{
    public class ChangeConnectionController : Controller
    {
        private readonly string _instanceId = "ChangeConnection";

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(_instanceId);

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
        }

        public ActionResult WithPartiaView()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(_instanceId);

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
        }

        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Create(_instanceId);

            SetNorthwindXml(qb);

            return qb;
        }

        [HttpPost]
        public ActionResult Change(string name)
        {
            ChangeConnection(name);
            return new EmptyResult();
        }

        [HttpPost]
        public PartialViewResult ChangePartial(string name)
        {
            var qb = ChangeConnection(name);
            return PartialView("_queryBuilder", qb);
        }

        public QueryBuilder ChangeConnection(string name)
        {
            var queryBuilder = QueryBuilderStore.Get(_instanceId);

            queryBuilder.MetadataContainer.Clear();

            if (name == "NorthwindXmlMetaData")
                SetNorthwindXml(queryBuilder);
            else if (name == "SQLite")
                SetSqLite(queryBuilder);
            else
                SetDb2Xml(queryBuilder);

            return queryBuilder;
        }

        private void SetNorthwindXml(QueryBuilder qb)
        {
            qb.MetadataLoadingOptions.OfflineMode = true;
            qb.SyntaxProvider = new MSSQLSyntaxProvider();

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            qb.MetadataContainer.ImportFromXML(xml);
            qb.MetadataStructure.Refresh();
        }

        private void SetDb2Xml(QueryBuilder qb)
        {
            qb.MetadataLoadingOptions.OfflineMode = true;
            qb.SyntaxProvider = new DB2SyntaxProvider();

            // Load MetaData from XML document. File name stored in WEB.CONFIG file in [/configuration/appSettings/Db2XmlMetaData] key
            var path = ConfigurationManager.AppSettings["Db2XmlMetaData"];
            var xml = Path.Combine(Server.MapPath("~"), path);

            qb.MetadataContainer.ImportFromXML(xml);
            qb.MetadataStructure.Refresh();
        }

        private void SetSqLite(QueryBuilder qb)
        {
            qb.MetadataLoadingOptions.OfflineMode = false;
            qb.SyntaxProvider = new SQLiteSyntaxProvider();
            qb.MetadataProvider = new SQLiteMetadataProvider
            {
                Connection = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase")
            };

            qb.MetadataStructure.Refresh();
        }
    }
}