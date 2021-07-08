using System;
using Play.Catalog.Service.Core;

namespace Play.Catalog.Service.Data
{
    public record Item : IEntity<Guid>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset UpdatedDate { get; init; }
    }
}