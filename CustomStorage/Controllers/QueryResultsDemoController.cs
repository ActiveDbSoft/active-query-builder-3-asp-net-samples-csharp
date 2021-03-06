﻿using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;
using CustomStorage.Helpers;

namespace CustomStorage.Controllers
{
    public class QueryResultsDemoController : Controller
    {
        private string instanceId = "QueryResults";

        public ActionResult Index()
        {
            return View();
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

        public void LoadQuery(string query)
        {
            var qb = QueryBuilderStore.Get(instanceId);

            if (query == "artist")
                qb.SQL = "Select artists.ArtistId, artists.Name From artists";
            else
                qb.SQL = "Select tracks.TrackId, tracks.Name From tracks";

            QueryBuilderStore.Put(qb);
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