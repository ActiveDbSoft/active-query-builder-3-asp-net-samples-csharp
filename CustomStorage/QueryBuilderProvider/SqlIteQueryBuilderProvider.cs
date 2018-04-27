using System.Data;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Core.QueryTransformer;
using ActiveQueryBuilder.View;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Infrastructure.Providers;
using CustomStorage.Helpers;

namespace CustomStorage.QueryBuilderProvider
{
    /// <summary>
    /// QueryBuilder storage provider which saves the state in Sqlite database
    /// </summary>
    public class QueryBuilderSqLiteStoreProvider : IQueryBuilderProvider
    {
        public bool SaveState { get; private set; }

        /// <summary>
        /// Connection to the Sqlite database
        /// </summary>
        private readonly IDbConnection _connection;

        public QueryBuilderSqLiteStoreProvider()
        {
            SaveState = true;

            _connection = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase");

            var sql = "create table if not exists QueryBuilders(id text primary key, layout TEXT)";
            ExecuteNonQuery(sql);
        }

        /// <summary>
        /// Creates an instance of the QueryBuilder object and loads its state identified by the given id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public QueryBuilder Get(string id)
        {
            QueryBuilder qb = QueryBuilderStore.Factory.SqLite(id);

            // Turn this property on to suppress parsing error messages when user types non-SELECT statements in the text editor.
            qb.BehaviorOptions.AllowSleepMode = false;
            
            // Bind Active Query Builder to a live database connection.
            qb.MetadataProvider = new SQLiteMetadataProvider 
            {
                // Assign an instance of DBConnection object to the Connection property.
                Connection = _connection
            };

            var layout = GetLayout(id);

            if (layout != null)
                qb.LayoutSQL = layout;

            return qb;
        }

        /// <summary>
        /// Saves the state of QueryBuilder object identified by its Tag property.
        /// </summary>
        /// <param name="qb">The QueryBuilder object.</param>
        public void Put(QueryBuilder qb)
        {           
            if (GetLayout(qb.Tag) == null)
                Insert(qb);
            else
                Update(qb);
        }

        /// <summary>
        /// Clears the state of QueryBuilder object identified by the given id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(string id)
        {
            var sql = string.Format("delete from QueryBuilders where id = {0}", id);
            ExecuteNonQuery(sql);
        }

        private void Insert(QueryBuilder qb)
        {
            var sql = string.Format("insert into QueryBuilders values ('{0}', '{1}')", qb.Tag, qb.LayoutSQL);
            ExecuteNonQuery(sql);
        }
        private void Update(QueryBuilder qb)
        {
            var sql = string.Format("update QueryBuilders set layout = '{1}' where id = '{0}'", qb.Tag, qb.LayoutSQL);
            ExecuteNonQuery(sql);
        }

        private void ExecuteNonQuery(string sql)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var cmd = CreateCommand(sql))
                    cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }

        private string GetLayout(string id)
        {
            var sql = string.Format("select layout from QueryBuilders where id = '{0}'", id);

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var cmd = CreateCommand(sql))
                    using (var reader = cmd.ExecuteReader())
                        if (reader.Read())
                            return reader["layout"].ToString();

                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        private IDbCommand CreateCommand(string sql)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }
    }

    public class QueryTransformerSqliteStoreProvider : IQueryTransformerProvider
    {
        public bool SaveState { get; private set; }

        private readonly IDbConnection _connection;

        public QueryTransformerSqliteStoreProvider()
        {
            SaveState = true;
            _connection = DataBaseHelper.CreateSqLiteConnection("SqLiteDataBase");

            var sql = "create table if not exists QueryTransformers(id text primary key, state TEXT)";
            ExecuteNonQuery(sql);
        }
        
        public QueryTransformer Get(string id)
        {
            var qt = new QueryTransformer { Tag = id };

            qt.QueryProvider = QueryBuilderStore.Get(id).SQLQuery;

            var state = GetState(id);

            if (state != null)
                qt.XML = state;

            return qt;
        }
        
        public void Put(QueryTransformer qt)
        {
            if (GetState(qt.Tag.ToString()) == null)
                Insert(qt);
            else
                Update(qt);
        }

        public void Delete(string id)
        {
            var sql = string.Format("delete from QueryTransformers where id = {0}", id);
            ExecuteNonQuery(sql);
        }

        private void Insert(QueryTransformer qt)
        {
            var sql = string.Format("insert into QueryTransformers values ('{0}', '{1}')", qt.Tag, qt.XML);
            ExecuteNonQuery(sql);
        }
        private void Update(QueryTransformer qt)
        {
            var sql = string.Format("update QueryTransformers set state = '{1}' where id = '{0}'", qt.Tag, qt.XML);
            ExecuteNonQuery(sql);
        }

        private void ExecuteNonQuery(string sql)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var cmd = CreateCommand(sql))
                    cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }

        private string GetState(string id)
        {
            var sql = string.Format("select state from QueryTransformers where id = '{0}'", id);

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var cmd = CreateCommand(sql))
                using (var reader = cmd.ExecuteReader())
                    if (reader.Read())
                        return reader["state"].ToString();

                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        private IDbCommand CreateCommand(string sql)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }
    }

    /// <summary>
    /// Sample of the QueryBuilder storage provider for Redis (https://redis.io/)
    /// </summary>
    /* public class QueryBuilderRedisStoreProvider : IQueryBuilderProvider
    {
        private readonly IDatabase _db;

        public RedisQueryBuilderProvider()
        {
            var redis = ConnectionMultiplexer.Connect();
            _db = redis.GetDatabase();
        }

        public QueryBuilder Get(string id)
        {
            var layout = _db.StringGet(id);

            var qb = new SqLiteQueryBuilder(id);

            if (layout.HasValue)
                qb.LayoutSQL = layout;

            return qb;
        }

        public void Put(QueryBuilder qb)
        {
            _db.StringSetAsync(qb.Tag, qb.LayoutSQL);
        }

        public void Delete(string id)
        {
            _db.StringSetAsync(id, "");
        }
    }*/
}