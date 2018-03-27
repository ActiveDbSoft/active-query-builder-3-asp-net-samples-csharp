using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActiveQueryBuilder.Web.Server.Handlers;

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
            BaseHandler.Register();
        }

        protected void Application_EndRequest()
        {
			//Instructing to allow cross-domain requests by the server.
            Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:9080"); // Host address of the static web server returning the ./FrontEnd/index.html page.
            Response.AddHeader("Access-Control-Allow-Headers", "*");
            Response.AddHeader("Access-Control-Allow-Methods", "*");
            Response.AddHeader("Access-Control-Allow-Credentials", "true");
        }
    }
}
