using System.Web;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Handlers
{
    public class GetData : QueryResults<GridModel>
    {
        public override object GetDataForModel(GridModel m)
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
                        qt.OrderBy(c, m.Sortorder.ToLowerInvariant() == "asc");
                }
            }

            return GetData(qt, m.Params);
        }

        public override GridModel CreateFromGetParams(HttpRequest r)
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
    }
}
