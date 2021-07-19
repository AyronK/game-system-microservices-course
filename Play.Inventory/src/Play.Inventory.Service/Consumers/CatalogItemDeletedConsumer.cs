using System;
using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Core;
using Play.Inventory.Service.Data.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<Guid, CatalogItem> _repository;

        public CatalogItemDeletedConsumer(IRepository<Guid, CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            CatalogItemDeleted message = context.Message;
            CatalogItem item = await _repository.Get(message.ItemId);

            if (item is null)
            {
                return;
            }
            
            await _repository.Remove(message.ItemId);
            
        }
    }
}