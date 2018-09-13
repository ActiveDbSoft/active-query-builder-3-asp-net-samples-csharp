using System.Configuration;
using System.IO;
using System.Web;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Infrastructure.Providers;

namespace CookieStorage.Providers
{
    public class CookieQueryBuilderProvider: IQueryBuilderProvider
    {
        public bool SaveState { get; private set; }

        public CookieQueryBuilderProvider()
        {
            SaveState = true;
        }

        public QueryBuilder Get(string id)
        {
            var qb = QueryBuilderStore.Factory.MsSql();
            LoadMetadata(qb);
            LoadState(qb);
            return qb;
        }

        private void LoadMetadata(QueryBuilder qb)
        {
            // Load metadata from XML document. File name stored in the "Web.config" file [/configuration/appSettings/NorthwindXmlMetaData] key
            var path = ConfigurationManager.AppSettings["NorthwindXmlMetaData"];
            var xml = Path.Combine(GetContext().Server.MapPath("~"), path);
            
            qb.MetadataContainer.ImportFromXML(xml);
        }

        private void LoadState(QueryBuilder qb)
        {
            var state = GetState();

            if (!string.IsNullOrEmpty(state))
                qb.LayoutSQL = state;
        }

        public void Put(QueryBuilder qb)
        {
            SetState(qb.LayoutSQL);
        }

        public void Delete(string id)
        {
            
        }

        private string GetState()
        {
            var cookie = GetContext().Request.Cookies.Get("QueryBuilder");

            if (cookie != null)
                return cookie["State"];

            return null;
        }

        private void SetState(string state)
        {
            var resp = GetContext().Response;
            var cookie = resp.Cookies.Get("QueryBuilder");

            if (cookie != null)
                cookie["State"] = state;
        }

        private HttpContext GetContext()
        {
            return HttpContext.Current;
        }
    }
}