using System.Web.Mvc;
using ActiveQueryBuilder.Web.Server;

namespace JavaScript.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

//CUT:PRO{{
        public void Dispose()
        {
            QueryBuilderStore.Remove();
            QueryTransformerStore.Remove();
        }
//}}CUT:PRO
    }
}