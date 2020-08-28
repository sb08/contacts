using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Contacts.Infrastructure.SeedData
{
    public interface IMongoDbContext : IDisposable
    {
        IMongoCollection<T> GetMongoCollection<T>(string collectionName);
        IEnumerable<string> ListCollections();
        Task<List<T>> ListAllAsync<T>(string collectionName);
        void InsertMany<T>(IEnumerable<T> documents, string collectionName = null);
        Task<T> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, string collectionName = null);
        Task UpsertAsync<T>(Expression<Func<T, bool>> predicate, T entity, string collectionName = null);
    }

    public class MongoDbContext : IMongoDbContext
    {
        private IMongoDatabase _db;

        public MongoDbContext(IOptions<MongoDbConfig> dbConfig)
        {
            _db = new MongoClient(dbConfig.Value.ConnectionString).GetDatabase(dbConfig.Value.Database);

        }

        public virtual IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            return _db?.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> ListAllAsync<T>(string collectionName = null)
        {
            var name = collectionName ?? typeof(T).Name;
            var collection = _db.GetCollection<T>(name);
            return await collection.AsQueryable().ToListAsync();
        }

        public IEnumerable<string> ListCollections()
        {
            var list = _db.ListCollections();
            return list.ToList().Select(doc => doc["name"].ToString());
        }

        public void InsertMany<T>(IEnumerable<T> documents, string collectionName = null)
        {
            var name = collectionName ?? typeof(T).Name;
            var collection = _db.GetCollection<T>(name);
            foreach (var document in documents)
            {
                collection.InsertOne(document);
            }
        }

        public async Task<IAsyncCursor<T>> SingleOrDefaultAsync<T>(FilterDefinition<T> filter, string collectionName = null)
        {
            var name = collectionName ?? typeof(T).Name;
            var collection = _db.GetCollection<T>(name);
            return await collection.FindAsync(filter);
        }

        public async Task<T> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
        {
            var name = collectionName ?? typeof(T).Name;
            var collection = _db.GetCollection<T>(name);
            return await collection.AsQueryable().SingleOrDefaultAsync(predicate);
        }

        [Obsolete]
        public async Task UpsertAsync<T>(Expression<Func<T, bool>> predicate, T entity, string collectionName = null)
        {
            IMongoCollection<T> collection = this._db.GetCollection<T>(collectionName ?? typeof(T).Name, (MongoCollectionSettings)null);
            Expression<Func<T, bool>> filter = predicate;
            T replacement = entity;
            UpdateOptions options = new UpdateOptions();
            options.IsUpsert = true;
            CancellationToken cancellationToken = new CancellationToken();
            _ = await collection.ReplaceOneAsync<T>(filter, replacement, options, cancellationToken);
        }

        #region IDisposable Members

        private Boolean _disposed = false;        // To detect redundant calls
        // IDisposable
        protected void Dispose(Boolean disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(Boolean disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
