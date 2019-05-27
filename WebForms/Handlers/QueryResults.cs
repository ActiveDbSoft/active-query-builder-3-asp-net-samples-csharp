using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Handlers
{
    public class QueryResults : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var model = CreateFromPostData(context.Request) ?? CreateFromGetParams(context.Request);
                var result = GetData(model);
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                context.Response.Write(content);
            }
            catch (Exception e)
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new ErrorOutput { Error = e.Message }));
            }
        }

        private class ErrorOutput
        {
            public string Error { get; set; }
        }

        private GridModel CreateFromGetParams(HttpRequest r)
        {
            //filterscount=0&groupscount=0&=0&=10&recordstartindex=0&recordendindex=10
            return new GridModel
            {
                Pagenum = int.Parse(r.Params["pagenum"]),
                Pagesize = int.Parse(r.Params["pagesize"]),
                Sortdatafield = r.Params["sortdatafield"],
                Sortorder = r.Params["sortorder"]
            };
        }

        private GridModel CreateFromPostData(HttpRequest r)
        {
            string input = new StreamReader(r.InputStream).ReadToEnd();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GridModel>(input);
        }

        public static object GetData(GridModel m)
        {
            var qt = QueryTransformerStore.Get("QueryResults");

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

            return GetData(qt);
        }

        private static object GetData(QueryTransformer qt)
        {
            var conn = qt.Query.SQLContext.MetadataProvider.Connection;
            var sql = qt.SQL;
            
            return DataBaseHelper.GetData(conn, sql);
        }

        #endregion
    }

    public class GridModel
    {
        public int Pagenum { get; set; }
        public int Pagesize { get; set; }
        public string Sortdatafield { get; set; }
        public string Sortorder { get; set; }
    }
}
