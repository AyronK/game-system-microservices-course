using Play.Inventory.Service.Data.Entities;
using Play.Inventory.Service.Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Data
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem inventoryItem) 
            => new(inventoryItem.CatalogItemId, inventoryItem.Quantity, inventoryItem.AcquiredDate);
    }
}