using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server.Handlers;
using ActiveQueryBuilder.Web.Server.Infrastructure;
using ActiveQueryBuilder.Web.Server;
using Nancy;
using System.Configuration;
using System.IO;
using System.Web.Hosting;

namespace NancyTest
{
    public class IndexModule : NancyModule
    {
        public static string InstanceId = "NancyFX";

        static IndexModule()
        {
            // Prevent the default route registration
            BaseHandler.DisableRegistration();
        }

        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                // Custom context based on the NancyContext to define data interchange rules with the client part of Active Query Builder
                IHttpContext httpContext = new NancyHttpContext(Context);

                // Get an instance of the QueryBuilder object according to the SessionID from the context.
                if (QueryBuilderStore.Get(httpContext, InstanceId) == null)
                    CreateQueryBuilder(httpContext);

                return View["index"];
            };

            // handling queries from the Active Query Builder clients
            Get["/{ActiveQueryBuilder*}"] = Handler;
            Post["/{ActiveQueryBuilder*}"] = Handler;
        }

        private dynamic Handler(dynamic parameters)
        {
            IHttpContext httpContext = new NancyHttpContext(Context);

            // Creating a request handler factory for Active Query Builder
            HandlerFactory factory = new HandlerFactory();

            // handling the request
            var handler = factory.CreateHandler(Context.Request.Path, httpContext);
            handler.ProcessRequest();

            // returning the response
            return Context.Response;
        }


        /// <summary>
        /// Creates and initializes new instance of the QueryBuilder object if it doesn't exist. 
        /// </summary>
        /// <param name="httpContext"></param>
        private void CreateQueryBuilder(IHttpContext httpContext)
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Create(httpContext, InstanceId);
            queryBuilder.SyntaxProvider = new MSSQLSyntaxProvider();

            // Denies metadata loading requests from the metadata provider
            queryBuilder.MetadataLoadingOptions.OfflineMode = true;

            // Load MetaData from XML document. File name is stored in the "Web.config" file in [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["QueryBuilderXmlMetaData"];
            var xml = Path.Combine(HostingEnvironment.MapPath("~"), path);

            queryBuilder.MetadataContainer.ImportFromXML(xml);

            //Set default query
            queryBuilder.SQL = GetDefaultOfflineSql();
        }

        private string GetDefaultOfflineSql()
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