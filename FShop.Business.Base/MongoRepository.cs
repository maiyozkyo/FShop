using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var data = DBSet.Find(id).FirstOrDefault();
            return data;
        }

        public virtual async Task<TEntity> Update(string id, TEntity obj)
        {
            await DBSet.ReplaceOneAsync(id, obj);
            return obj;
        }

        public virtual async Task<bool> Delete(string id)
        {
            var res = await DBSet.DeleteOneAsync(id);
            return res.IsAcknowledged;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var res = await DBSet.Find(new BsonDocument()).ToListAsync();
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
