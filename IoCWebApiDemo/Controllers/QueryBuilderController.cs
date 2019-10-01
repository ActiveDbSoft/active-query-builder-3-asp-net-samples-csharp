using System;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Hosting;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server.Infrastructure;
using ActiveQueryBuilder.Web.Server.Infrastructure.Factories;
using IoCWebApiDemo.Providers;

namespace IoCWebApiDemo.Controllers
{
	public class QueryBuilderController : ApiController
	{
		private readonly IQueryBuilderStore _store;

		public QueryBuilderController(IQueryBuilderStore store)
		{
			_store = store;
		}

		[System.Web.Http.HttpGet]
		public string CheckToken(string token)
		{
			// get Token QueryBuilder provider from the store
			var provider = (TokenQueryBuilderProvider)_store.Provider;

			// check if the item with specified key exists in the storage. 
			if (provider.CheckToken(token))
				// Return empty string in the case of success
				return string.Empty;
			// Return the new token to the client if the specified token doesn't exist.
			return provider.CreateToken();
		}

		/// <summary>
		/// Creates and initializes new instance of the QueryBuilder object for the given identifier if it doesn't exist. 
		/// </summary>
		/// <param name="name">Instance identifier of object in the current session.</param>
		/// <returns></returns>
		[System.Web.Http.HttpGet]
		public ActionResult CreateQueryBuilder(string name)
		{
			// Get an instance of the QueryBuilder object
			var qb = _store.Get(new AspNetHttpContext(), name);

			if (qb != null)
				return new HttpStatusCodeResult(200);

			try
			{
				// Create an instance of the QueryBuilder object
				qb = _store.Create(new AspNetHttpContext(), name);
				qb.SyntaxProvider = new MSSQLSyntaxProvider();

				// Load MetaData from XML document.
				var path = @"..\Sample databases\Northwind.xml";
				var xml = Path.Combine(HostingEnvironment.MapPath("~"), path);

				qb.MetadataContainer.ImportFromXML(xml);

				return new HttpStatusCodeResult(200);
			}
			catch (QueryBuilderException e)
			{
				return new HttpStatusCodeResult(400, e.Message);
			}
		}
	}
}