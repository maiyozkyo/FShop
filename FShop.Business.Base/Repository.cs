using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Business.Base
{
    public class Repository<TEntity> : MongoRepository<TEntity> where TEntity : class
    {
        public Repository(IMongoDBContext dbContext) : base(dbContext)
        {
        }
    }
}
