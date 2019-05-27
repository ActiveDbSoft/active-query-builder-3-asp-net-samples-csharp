using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Infrastructure.Providers;

namespace IoCWebApiDemo.Providers
{
    // Token-based QueryBuilder storage provider
    // Stores TokenStoreItems using values from HTTP request header as a key.
    public class TokenQueryBuilderProvider : IQueryBuilderProvider
    {
        public bool SaveState { get; private set; }

        private readonly MemoryCache _cache;

        public TokenQueryBuilderProvider()
        {
            SaveState = false;
            _cache = new MemoryCache("Tokens");
        }

        public QueryBuilder Get(string id)
        {
            var token = GetToken();

            if (token == null)
                return null;

            return ((TokenStoreItem)_cache[token]).Get(id);
        }

        public void Put(QueryBuilder qb)
        {
            var token = GetToken();
            ((TokenStoreItem)_cache[token]).Put(qb);
        }

        public void Delete(string id)
        {
            var token = GetToken();
            ((TokenStoreItem)_cache[token]).Delete(id);
        }

        private void CreateItem(string token)
        {
            if (_cache.Contains(token))
                ((TokenStoreItem)_cache[token]).Dispose();

            CacheItem ci = new CacheItem(token, new TokenStoreItem());
            CacheItemPolicy cip = new CacheItemPolicy
            {
                RemovedCallback = args => ((TokenStoreItem)args.CacheItem.Value).Dispose(),
                SlidingExpiration = TimeSpan.FromMinutes(20),
                Priority = CacheItemPriority.NotRemovable
            };

            _cache.Set(ci, cip);
        }

        public bool CheckToken(string token)
        {
            if (token == null)
                return false;
            return _cache.Contains(token);
        }

        public string CreateToken()
        {
            var token = Guid.NewGuid().ToString();
            CreateItem(token);
            return token;
        }

        private string GetToken()
        {
            var token = HttpContext.Current.Request.Headers["query-builder-token"];

            if (string.IsNullOrEmpty(token))
                throw new ApplicationException("Token not found");

            if (!CheckToken(token))
                return null;

            return token;
        }
    }

    // Token-based storage item holding an instance of the QueryBuilder object
    public class TokenStoreItem : IDisposable
    {
        private readonly Dictionary<string, QueryBuilder> QueryBuilders = new Dictionary<string, QueryBuilder>();

        public QueryBuilder Get(string id)
        {
            if (!QueryBuilders.ContainsKey(id))
                return null;

            return QueryBuilders[id];
        }

        public void Put(QueryBuilder qb)
        {
            if (QueryBuilders.ContainsKey(qb.Tag) && QueryBuilders[qb.Tag] == qb)
                return;

            QueryBuilders[qb.Tag] = qb;
        }

        public void Delete(string id)
        {
            QueryBuilders[id].Dispose();
            QueryBuilders.Remove(id);
        }

        public void Dispose()
        {
            foreach (var key in QueryBuilders.Keys)
                Delete(key);
        }
    }
}