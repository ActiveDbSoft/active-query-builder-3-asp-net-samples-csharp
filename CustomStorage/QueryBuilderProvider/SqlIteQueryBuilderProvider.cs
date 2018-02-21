using System.Data;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Infrastructure.Providers;
using CustomStorage.Helpers;

namespace CustomStorage.QueryBuilderProvider
{
    /// <summary>
    /// QueryBuilder storage provider which saves the state in Sqlite database
    /// </summary>
    public class QueryBuilderSqliteStoreProvider : IQueryBuilderProvider
    {
        /// <summary>
        /// Connection to the Sqlite database
        /// </summary>
        private readonly IDbConnection _connection;

        public QueryBuilderSqliteStoreProvider()
        {
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
            var qb = new SqLiteQueryBuilder(_connection, id);

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

        protected void ExecuteNonQuery(string sql)
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
        
        protected string GetLayout(string id)
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

        protected IDbCommand CreateCommand(string sql)
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

            var qb = new QueryBuilder(id);

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