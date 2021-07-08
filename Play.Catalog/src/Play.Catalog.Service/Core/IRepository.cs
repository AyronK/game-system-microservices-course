using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Core
{
    public interface IRepository<TId, TEntity> where TEntity : class, IEntity<TId> 
    {
        Task<TEntity> Find(TId id);
        Task<bool> Contains(TId id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Add(TEntity item);
        Task<TEntity> Update(TId id, TEntity item);
        Task Remove(TId id);
    }
}