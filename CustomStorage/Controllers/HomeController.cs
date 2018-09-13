using System.Web.Mvc;
using ActiveQueryBuilder.Web.Server;

namespace CustomStorage.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}