using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Business.Base
{
    public abstract class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoDatabase Database;
        protected readonly IMongoCollection<TEntity> DBSet;
        public MongoRepository(IMongoDBContext dbContext)
        {
            Database = dbContext.Database;
            DBSet = Database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async Task<TEntity> Add(TEntity obj)
        {
            await DBSet.InsertOneAsync(obj);
            return obj;
        }

        public virtual async Task<TEntity> GetByID(string id)
        {
            var data = DBSet.Find(FilterID(id)).FirstOrDefault();
            return data;
        }

        public virtual async Task<TEntity> Update(string id, TEntity obj)
        {
            await DBSet.ReplaceOneAsync(id, obj);
            return obj;
        }

        public virtual async Task<bool> Delete(string id)
        {
            var res = await DBSet.DeleteOneAsync(FilterID(id));
            return res.IsAcknowledged;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var res = await DBSet.Find(new BsonDocument()).ToListAsync();
            return res;
        }

        public virtual IMongoQueryable<TEntity> Get(Expression<Func<TEntity, bool>> condition)
        {
            var res = DBSet.AsQueryable().Where(condition);
            return res;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public static FilterDefinition<TEntity> FilterID(string id)
        {
            return Builders<TEntity>.Filter.Eq("Id", id);
        }
    }
}
