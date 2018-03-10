using System.Web.Mvc;
using ActiveQueryBuilder.Web.Server;

namespace CustomStorage.Controllers
{
    public class SimpleDemoController : Controller
    {
        // GET
        public ActionResult Index()
        {
            // We've redefined the QueryBuilderStore.Provider object to be of QueryBuilderSqliteStoreProvider class in the Global.asax.cs file.
            // The implementation of Get method in this provider gets _OR_creates_new_ QueryBuilder object.
            // The initialization of the QueryBuilder object is also internally made by the QueryBuilderSqliteStoreProvider.
            var qb = QueryBuilderStore.Get();
            return View(qb);
        }
    }
}