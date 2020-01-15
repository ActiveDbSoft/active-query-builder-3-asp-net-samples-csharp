using System;
using System.Data;
using System.Linq;
using System.Web;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Handlers
{
    public class SelectRecordsCount : QueryResults<Param[]>
    {
        public override object GetDataForModel(Param[] _params)
        {
            var qb = QueryBuilderStore.Get("QueryResults");
            var qt = QueryTransformerStore.Get("QueryResults");
            var qtForSelectRecordsCount = new QueryTransformer { QueryProvider = qt.QueryProvider };

            try
            {
                qtForSelectRecordsCount.Assign(qt);
                qtForSelectRecordsCount.Skip("");
                qtForSelectRecordsCount.Take("");
                qtForSelectRecordsCount.SelectRecordsCount("recCount");

                var data = GetData(qtForSelectRecordsCount, _params);
                return data.First().Values.First();
            }
            finally
            {
                qtForSelectRecordsCount.Dispose();
            }
        }

        public override Param[] CreateFromGetParams(HttpRequest r)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DbType DataType { get; set; }
    }
}
