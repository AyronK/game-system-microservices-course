using Play.Catalog.Service.Data.Entities;

namespace Play.Catalog.Service.Data
{
    public static class ItemDtoExtensions 
    {
        
        public static ItemDto AsDto(this Item item)
        {
            return new(item.Id, item.Name, item.Description, item.Price, item.CreatedDate, item.UpdatedDate);
        }
    }
}