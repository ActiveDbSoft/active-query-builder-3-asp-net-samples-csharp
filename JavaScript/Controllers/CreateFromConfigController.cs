using System;
using System.Web.Mvc;
using ActiveQueryBuilder.Web.Server;

namespace JavaScript.Controllers
{
    public class CreateFromConfigController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creates and initializes new instance of the QueryBuilder object for the given identifier if it doesn't exist. 
        /// </summary>
        /// <param name="name">Instance identifier of object in the current session.</param>
        /// <returns></returns>
        public ActionResult Create(string name)
        {
            try
            {
                // Create an instance of the QueryBuilder object
                QueryBuilderStore.GetOrCreate(name, qb => qb.SQL = GetDefaultSql());

                // The necessary initialization procedures to setup SQL syntax and the source of metadata will be performed automatically 
                // according to directives in the special configuration section of 'Web.config' file.

                // This behavior is enabled by the QueryBuilderStore.WebConfig() method call in the Application_Start method in Global.asax.cs file.
                // See qb.ConfiguredBy to get information about actual default settings
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }

            return new EmptyResult();
        }
        private string GetDefaultSql()
        {
            return @"Select o.OrderID,
                        c.CustomerID,
                        s.ShipperID,
                        o.ShipCity
                    From Orders o
                        Inner Join Customers c On o.CustomerID = c.CustomerID
                        Inner Join Shippers s On s.ShipperID = o.OrderID
                    Where o.ShipCity = 'A'";
        }
    }
}