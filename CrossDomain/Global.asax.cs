using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActiveQueryBuilder.Web.Server;
using CrossDomain.Providers;

namespace CrossDomain
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            QueryBuilderStore.Provider = new TokenQueryBuilderProvider();
            QueryBuilderStore.UseWebConfig();
        }

        protected void Application_BeginRequest()
        {
            // Instructing to allow cross-domain requests by the server.
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "*");

			// This header is used to find a correspondence between the client and server parts of the QueryBuilder.
            Response.Headers.Add("Access-Control-Allow-Headers", "query-builder-token");

            if (Request.HttpMethod == "OPTIONS")
                Response.Flush();
        }
    }
}
