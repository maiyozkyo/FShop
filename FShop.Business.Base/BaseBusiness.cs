namespace FShop.Business.Base
{
    public class BaseBusiness<TEntity> where TEntity : class
    {
        public Repository<TEntity> Repository { get; set; }

        public BaseBusiness(IMongoDBContext dbContext) {
            Repository = new Repository<TEntity>(dbContext);
        }
       
    }
}
