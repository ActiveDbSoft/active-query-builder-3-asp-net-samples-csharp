using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Core.QueryTransformer;
using MVC_Samples.Helpers;
using ActiveQueryBuilder.Web.Server;

namespace MVC_Samples.Controllers
{
    public class QueryResultsDemoController : Controller
    {
        private string instanceId = "QueryResults";

        public ActionResult Index()
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get(instanceId);
            var qt = QueryTransformerStore.Get(instanceId);

            if (qb == null)
                qb = CreateQueryBuilder();

            if (qt == null)
                qt = CreateQueryTransformer(qb.SQLQuery);

            ViewBag.QueryTransformer = qt;

            return View(qb);
        }

        public ActionResult GetData(GridModel m)
        {
            var qt = QueryTransformerStore.Get(instanceId);

            qt.Skip((m.Pagenum * m.Pagesize).ToString());
            qt.Take(m.Pagesize == 0 ? "" : m.Pagesize.ToString());

            if (!string.IsNullOrEmpty(m.Sortdatafield))
            {
                qt.Sortings.Clear();

                if (!string.IsNullOrEmpty(m.Sortorder))
                { 
                    var c = qt.Columns.FindColumnByResultName(m.Sortdatafield);

                    if (c != null)
                        qt.OrderBy(c, m.Sortorder.ToLower() == "asc");
                }
            }

            return GetData(qt, m.Params);
        }
        
        private ActionResult GetData(QueryTransformer qt, Param[] _params)
        {
            var conn = qt.Query.SQLContext.MetadataProvider.Connection;
            var sql = qt.SQL;

            if (_params != null)
                foreach (var p in _params)
                    p.DataType = qt.Query.QueryParameters.First(qp => qp.FullName == p.Name).DataType;

            try
            {
                var data = DataBaseHelper.GetData(conn, sql, _params);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Creates and initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <returns>Returns instance of the QueryBuilder object.</returns>
        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.SqLite(instanceId);

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
            return @"Select customers.CustomerId,
                      customers.LastName,
                      customers.FirstName
                    From customers";
        }

        /// <summary>
        /// Creates and initializes a new instance of the QueryTransformer object.
        /// </summary>
        /// <param name="query">SQL Query to transform.</param>
        /// <returns>Returns instance of the QueryTransformer object.</returns>
        private QueryTransformer CreateQueryTransformer(SQLQuery query)
        {
            var qt = QueryTransformerStore.Create(instanceId);

            qt.QueryProvider = query;
            qt.AlwaysExpandColumnsInQuery = true;

            return qt;
        }
    }

    public class GridModel
    {
        public int Pagenum { get; set; }
        public int Pagesize { get; set; }
        public string Sortdatafield { get; set; }
        public string Sortorder { get; set; }
        public Param[] Params { get; set; }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DbType DataType { get; set; }
    }
}