using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Catalog.Service.Core;
using Play.Catalog.Service.Data.Entities;

namespace Play.Catalog.Service.Data.Repositories.InMemory
{
    public class StaticItemsRepository : IRepository<Guid, Item>
    {
        private static readonly List<Item> Items = new()
        {
            new Item{Id = Guid.NewGuid(), Name = "Small Health Potion", Description = "Restores a small amount of HP", Price = 5, CreatedDate = DateTimeOffset.UtcNow, UpdatedDate = DateTimeOffset.UtcNow },
            new Item{Id = Guid.NewGuid(), Name = "Medium Health Potion", Description = "Restores a medium amount of HP", Price = 10, CreatedDate = DateTimeOffset.UtcNow, UpdatedDate = DateTimeOffset.UtcNow },
            new Item{Id = Guid.NewGuid(), Name = "Large Health Potion", Description = "Restores a large amount of HP", Price = 15, CreatedDate = DateTimeOffset.UtcNow, UpdatedDate = DateTimeOffset.UtcNow },
            new Item{Id = Guid.NewGuid(), Name = "Antidote", Description = "Cures poison", Price = 8, CreatedDate = DateTimeOffset.UtcNow, UpdatedDate = DateTimeOffset.UtcNow },
            new Item{Id = Guid.NewGuid(), Name = "Bronze sword", Description = "Deals a small amount of melee damage", Price = 20, CreatedDate = DateTimeOffset.UtcNow, UpdatedDate = DateTimeOffset.UtcNow },
        };
        
        public Task<Item> Find(Guid id)
        {
            return Task.FromResult(Items.SingleOrDefault(i => i.Id == id));
        }

        public Task<IEnumerable<Item>> GetAll()
        {
            return Task.FromResult(Items.AsEnumerable());
        }

        public Task<Guid> Add(Item item)
        {
            Item newItem = item with { Id = Guid.NewGuid(), CreatedDate = DateTimeOffset.UtcNow, UpdatedDate = DateTimeOffset.UtcNow };
            Items.Add(newItem);
            return Task.FromResult(newItem.Id);
        }

        public Task Update(Guid id, Item item)
        {
            int index = Items.FindIndex(i => i.Id == id);
            Items[index] =  Items[index] with { Name = item.Name, Description = item.Description, Price = item.Price, UpdatedDate = DateTimeOffset.UtcNow };
            return Task.CompletedTask;
        }

        public Task Remove(Guid id)
        {
            int index = Items.FindIndex(i => i.Id == id);
            Items.RemoveAt(index);
            return Task.CompletedTask;
        }
    }
}