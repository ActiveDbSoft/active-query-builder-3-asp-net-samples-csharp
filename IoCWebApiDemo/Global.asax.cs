using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Infrastructure.Factories;
using Autofac;
using Autofac.Integration.WebApi;
using IoCWebApiDemo.Providers;

namespace IoCWebApiDemo
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// Redefine the QueryBuilderStore.Provider object to be an instance of the TokenQueryBuilderProvider class
			QueryBuilderStore.Provider = new TokenQueryBuilderProvider();
			
			var builder = new ContainerBuilder();
			builder.Register(c => QueryBuilderStore.Internal).As<IQueryBuilderStore>().SingleInstance();
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			var container = builder.Build();
			GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}
	}
}
