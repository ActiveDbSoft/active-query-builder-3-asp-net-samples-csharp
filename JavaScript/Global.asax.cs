using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ActiveQueryBuilder.Web.Server;
using Microsoft.Ajax.Utilities;

namespace JavaScript
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Instructing to initialize new instances of the QueryBuilder object according to 
            // directives in the special configuration section of 'Web.config' file.
            
            // Uncomment this line to work with the "Create Query Configuration from Web.Config" demo
            // QueryBuilderStore.UseWebConfig();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (!Request.IsSecureConnection)
                return;

            foreach (string name in Response.Cookies)
            {
                var cookie = Response.Cookies[name];

                cookie.SameSite = SameSiteMode.None;
                cookie.Secure = true;
            }
        }
    }
}
