using System;
using System.Collections.Generic;
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
            var qb = QueryBuilderStore.GetOrCreate(instanceId, InitializeQueryBuilder);
            var qt = QueryTransformerStore.GetOrCreate(instanceId, q =>
            {
                q.QueryProvider = qb.SQLQuery;
                q.AlwaysExpandColumnsInQuery = true;
            });

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
            try
            {
                var data = Execute(qt, _params);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ErrorOutput { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<Dictionary<string, object>> Execute(QueryTransformer qt, Param[] _params)
        {
            var conn = qt.Query.SQLContext.MetadataProvider.Connection;
            var sql = qt.SQL;

            if (_params != null)
                foreach (var p in _params)
                    p.DataType = qt.Query.QueryParameters.First(qp => qp.FullName == p.Name).DataType;

            return DataBaseHelper.GetData(conn, sql, _params);
        }

        [HttpPost]
        public ActionResult SelectRecordsCount(Param[] _params)
        {
            var qb = QueryBuilderStore.Get(instanceId);
            var qt = QueryTransformerStore.Get(instanceId);
            var qtForSelectRecordsCount = new QueryTransformer { QueryProvider = qb.SQLQuery };

            try
            {
                qtForSelectRecordsCount.Assign(qt);
                qtForSelectRecordsCount.Skip("");
                qtForSelectRecordsCount.Take("");
                qtForSelectRecordsCount.SelectRecordsCount("recCount");

                try
                {
                    var data = Execute(qtForSelectRecordsCount, _params);
                    return Json(data.First().Values.First(), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ErrorOutput {Error = e.Message}, JsonRequestBehavior.AllowGet);
                }
            }
            finally
            {
                qtForSelectRecordsCount.Dispose();
            }
        }

        private class ErrorOutput
        {
            public string Error { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <param name="queryBuilder">Active Query Builder instance.</param>
        private void InitializeQueryBuilder(QueryBuilder queryBuilder)
        {
            queryBuilder.SyntaxProvider = new SQLiteSyntaxProvider();

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
        }
        
        private string GetDefaultSql()
        {
            return @"Select customers.CustomerId,
                      customers.LastName,
                      customers.FirstName
                    From customers";
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