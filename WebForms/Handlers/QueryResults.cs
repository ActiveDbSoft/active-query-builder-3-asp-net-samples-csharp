using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Handlers
{
    public abstract class QueryResults<T> : IHttpHandler, IRequiresSessionState where T : class
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
                var result = GetDataForModel(model);
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

        private T CreateFromPostData(HttpRequest r)
        {
            string input = new StreamReader(r.InputStream).ReadToEnd();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
        }

        public abstract object GetDataForModel(T m);
        public abstract T CreateFromGetParams(HttpRequest r);

        public static List<Dictionary<string, object>> GetData(QueryTransformer qt, Param[] _params)
        {
            var conn = qt.Query.SQLContext.MetadataProvider.Connection;
            var sql = qt.SQL;

            if (_params != null)
                foreach (var p in _params)
                    p.DataType = qt.Query.QueryParameters.First(qp => qp.FullName == p.Name).DataType;

            return DataBaseHelper.GetData(conn, sql, _params);
        }

        #endregion
    }

    public class GridModel
    {
        public int Pagenum { get; set; }
        public int Pagesize { get; set; }
        public string Sortdatafield { get; set; }
        public string Sortorder { get; set; }
        public Param[] Params { get; set; }
    }
}
