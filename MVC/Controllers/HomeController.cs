using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

//CUT:PRO{{
        public void DisposeStates()
        {
            QueryBuilderStore.Remove();
            QueryTransformerStore.Remove();
        }
//}}CUT:PRO
    }
}