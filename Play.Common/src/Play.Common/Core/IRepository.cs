using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Play.Common.Core
{
    public interface IRepository<TId, TEntity> where TEntity : class, IEntity<TId> 
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> Get(TId id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        Task<TId> Add(TEntity item);
        Task Update(TEntity item);
        Task Remove(TId id);
    }
}