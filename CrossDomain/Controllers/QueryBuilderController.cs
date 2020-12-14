using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using CrossDomain.Providers;

namespace CrossDomain.Controllers
{
    public class QueryBuilderController : Controller
    {
        public string CheckToken(string token)
        {
            // get Token QueryBuilder provider from the store
            var provider = (TokenQueryBuilderProvider)QueryBuilderStore.Provider;

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

        public ActionResult CreateQueryBuilder(string name)
        {
            try
            {
                // Create an instance of the QueryBuilder object
                QueryBuilderStore.GetOrCreate(name);
                return new HttpStatusCodeResult(200);
            }
            catch (QueryBuilderException e)
            {
                return new HttpStatusCodeResult(400, e.Message);
            }
        }
    }
}