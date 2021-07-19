using System;
using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Core;
using Play.Inventory.Service.Data.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<Guid, CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<Guid, CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            CatalogItemUpdated message = context.Message;
            CatalogItem item = await _repository.Get(message.ItemId);

            if (item is null)
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Description = message.Description,
                    Name = message.Name,
                };

                await _repository.Add(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;

                await _repository.Update(item);
            }
        }
    }
}