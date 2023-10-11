using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Business.Base
{
    public interface IMongoDBContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoContext : IMongoDBContext
    {
        public IMongoDatabase Database { get; }
        public MongoContext(IMongoDBSettings mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.ConnectionString);
            Database = client.GetDatabase(mongoDBSettings.DatabaseName);
        }
    }
}
