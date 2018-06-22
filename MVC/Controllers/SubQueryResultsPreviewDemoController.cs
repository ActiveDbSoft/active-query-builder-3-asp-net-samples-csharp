using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;
using MVC_Samples.Helpers;

namespace MVC_Samples.Controllers
{
    public class SubQueryResultsPreviewDemoController : Controller
    {
        private static string Name = "SubQueryResultsPreview";

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(Name);

            if (qb == null)
                qb = CreateQueryBuilder();

            return View(qb);
        }

        public ActionResult GetData()
        {
            var qb = QueryBuilderStore.Get(Name);
            var conn = qb.MetadataProvider.Connection;

            var sqlQuery = new SQLQuery(qb.SQLContext)
            {
                SQL = qb.ActiveUnionSubQuery.SQL
            };

            QueryTransformer qt = new QueryTransformer
            {
                QueryProvider = sqlQuery
            };

            qt.Take("7");

            var columns = qt.Columns.Select(c => c.ResultName).ToList();

            try
            {
                var data = DataBaseHelper.GetDataList(conn, qt.SQL);
                var result = new { columns, data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = new { columns, data = new List<List<object>> { new List<object> { e.Message } } };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Creates and initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <returns>Returns instance of the QueryBuilder object.</returns>
        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.SqLite(Name);

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            queryBuilder.BehaviorOptions.AllowSleepMode = false;
            
            // Bind Active Query Builder to a live database connection.
            queryBuilder.MetadataProvider = new SQLiteMetadataProvider
            {
                // Assign an instance of DBConnection object to the Connection property.
                Connection = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase")
            };

            // Assign the initial SQL query text the user sees on the _first_ page load
            queryBuilder.SQL = GetDefaultSql();

            return queryBuilder;
        }

        private string GetDefaultSql()
        {
            return @"Select Count(Query1.EmployeeId) As Count_L,
                      Count(Query2.EmployeeId) As Count_C
                    From (Select employees.EmployeeId,
                            employees.LastName,
                            employees.FirstName,
                            employees.City
                          From employees
                          Where employees.City = 'Lethbridge') Query1,
                      (Select employees.EmployeeId,
                            employees.LastName,
                            employees.FirstName,
                            employees.City
                          From employees
                          Where employees.City = 'Calgary') Query2";
        }
    }
}