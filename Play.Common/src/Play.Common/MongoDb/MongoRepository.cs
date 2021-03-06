using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Common.Core;

namespace Play.Common.MongoDb
{
    public class MongoRepository<TEntity> : IRepository<Guid, TEntity> where TEntity : class, IEntity<Guid>
    {
        private readonly IMongoCollection<TEntity> _dbCollection;
        private readonly FilterDefinitionBuilder<TEntity> _filterBuilder = Builders<TEntity>.Filter;
        
        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _dbCollection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task<TEntity> Get(Guid id)
        {
            FilterDefinition<TEntity> filterById = _filterBuilder.Eq(entity => entity.Id, id);
            return await _dbCollection.Find(filterById).SingleOrDefaultAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _dbCollection.InsertOneAsync(item);
        }

        public async Task Update(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            FilterDefinition<TEntity> filterById = _filterBuilder.Eq(existingEntity => existingEntity.Id, item.Id);
            await _dbCollection.ReplaceOneAsync(filterById, item);
        }

        public async Task Remove(Guid id)
        {
            FilterDefinition<TEntity> filterById = _filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            await _dbCollection.DeleteOneAsync(filterById);
        }
    }
}