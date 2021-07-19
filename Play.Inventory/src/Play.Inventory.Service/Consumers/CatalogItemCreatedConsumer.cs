using System;
using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Core;
using Play.Inventory.Service.Data.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<Guid, CatalogItem> _repository;

        public CatalogItemCreatedConsumer(IRepository<Guid, CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            CatalogItemCreated message = context.Message;
            CatalogItem item = await _repository.Get(message.ItemId);

            if (item is not null)
            {
                return;
            }

            item = new CatalogItem
            {
                Id = message.ItemId,
                Description = message.Description,
                Name = message.Name,
            };
            
            await _repository.Add(item);
        }
    }
}