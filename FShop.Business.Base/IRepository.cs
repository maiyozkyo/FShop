using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Business.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> Add(TEntity obj);
        Task<TEntity> GetByID(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Update(string id, TEntity obj);
        Task<bool> Delete(string id);
    }
}
