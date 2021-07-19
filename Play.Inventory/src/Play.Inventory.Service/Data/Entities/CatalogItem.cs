using System;
using Play.Common.Core;

namespace Play.Inventory.Service.Data.Entities
{
    public class CatalogItem : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}