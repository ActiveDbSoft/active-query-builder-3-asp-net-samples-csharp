using System;
using System.Linq;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.Web.Server;
using WebForms_Samples.Handlers;
using WebForms_Samples.Helpers;

namespace WebForms_Samples.Samples
{
    public partial class QueryResultsDemo : BasePage
    {
        private int _page = 1;
        private int _recordsCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get an instance of the QueryBuilder object
            var qb = QueryBuilderStore.Get("QueryResults");
            var qt = QueryTransformerStore.Get("QueryResults");

            if (qb == null)
                qb = CreateQueryBuilder();

            if (qt == null)
                qt = CreateQueryTransformer(qb.SQLQuery);

            QueryBuilderControl1.QueryBuilder = qb;
            ObjectTreeView1.QueryBuilder = qb;
            Canvas1.QueryBuilder = qb;
            Grid1.QueryBuilder = qb;
            SubQueryNavigationBar1.QueryBuilder = qb;
            SqlEditor1.QueryBuilder = qb;
            StatusBar1.QueryBuilder = qb;
            CriteriaBuilder1.QueryTransformer = qt;
        }

        /// <summary>
        /// Creates and initializes a new instance of the QueryBuilder object.
        /// </summary>
        /// <returns>Returns instance of the QueryBuilder object.</returns>
        private QueryBuilder CreateQueryBuilder()
        {
            // Create an instance of the QueryBuilder object
            var queryBuilder = QueryBuilderStore.Factory.SqLite("QueryResults");

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

        private QueryTransformer CreateQueryTransformer(SQLQuery query)
        {
            var qt = QueryTransformerStore.Create("QueryResults");

            qt.QueryProvider = query;
            qt.AlwaysExpandColumnsInQuery = true;

            return qt;
        }

        protected void Prev_OnClick(object sender, EventArgs e)
        {
            _page = int.Parse(aspPage.Text.Substring(6)) - 1;
            UpdateDataGrid(sender, e);
        }

        protected void Next_OnClick(object sender, EventArgs e)
        {
            _page = int.Parse(aspPage.Text.Substring(6)) + 1;
            UpdateDataGrid(sender, e);
        }

        protected void UpdateDataGrid(object sender, EventArgs e)
        {
            var qb = QueryBuilderStore.Get("QueryResults");
            var qt = QueryTransformerStore.Get("QueryResults");

            if (string.IsNullOrEmpty(qt.SQL))
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                aspPage.Text = "Page: 0";
                recordsCount.Text = "Records count: 0";
                return;
            }

            UpdateRecordCount(qt);

            if (_page < 0)
                _page = 0;

            if (10 * _page > _recordsCount)
                _page -= 1;

            qt.Skip((10 * _page).ToString());
            qt.Take("10");

            using (var conn = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase"))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = qt.SQL;
                    var reader = cmd.ExecuteReader();
                    GridView1.DataSource = reader;
                    GridView1.DataBind();
                }
            }

            aspPage.Text = "Page: " + _page;
        }

        private void UpdateRecordCount(QueryTransformer qt)
        {
            var qtForSelectRecordsCount = new QueryTransformer { QueryProvider = qt.QueryProvider };

            try
            {
                qtForSelectRecordsCount.Assign(qt);
                qtForSelectRecordsCount.Skip("");
                qtForSelectRecordsCount.Take("");
                qtForSelectRecordsCount.SelectRecordsCount("recCount");

                var data = SelectRecordsCount.GetData(qtForSelectRecordsCount, new Param[0]);
                _recordsCount = int.Parse(data.First().Values.First().ToString());
                recordsCount.Text = "Records count: " + _recordsCount;
            }
            finally
            {
                qtForSelectRecordsCount.Dispose();
            }
        }
    }
}
