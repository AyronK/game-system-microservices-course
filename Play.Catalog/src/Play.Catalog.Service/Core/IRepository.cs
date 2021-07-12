using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Core
{
    public interface IRepository<TId, TEntity> where TEntity : class, IEntity<TId> 
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Find(TId id);
        Task<TId> Add(TEntity item);
        Task Update(TId id, TEntity item);
        Task Remove(TId id);
    }
}