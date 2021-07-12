using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Core;
using Play.Catalog.Service.Data.Entities;

namespace Play.Catalog.Service.Data.Repositories.MongoDb
{
    public class ItemsRepository : IRepository<Guid, Item>
    {
        private const string CollectionName = "items";
        private readonly IMongoCollection<Item> _dbCollection;
        private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;
        
        public ItemsRepository(IMongoDatabase database)
        {
            _dbCollection = database.GetCollection<Item>(CollectionName);
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> Find(Guid id)
        {
            FilterDefinition<Item> filterById = _filterBuilder.Eq(entity => entity.Id, id);
            return await _dbCollection.Find(filterById).SingleOrDefaultAsync();
        }

        public async Task<Guid> Add(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Item newItem = item with { Id = new Guid()};
            await _dbCollection.InsertOneAsync(newItem);
            return newItem.Id;
        }

        public async Task Update(Guid id, Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            FilterDefinition<Item> filterById = _filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            await _dbCollection.ReplaceOneAsync(filterById, item);
        }

        public async Task Remove(Guid id)
        {
            FilterDefinition<Item> filterById = _filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            await _dbCollection.DeleteOneAsync(filterById);
        }
    }
}