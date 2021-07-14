using System;
using Play.Common.Core;

namespace Play.Inventory.Service.Data.Entities
{
    public class InventoryItem : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}