namespace Play.Catalog.Service.Data
{
    public static class ItemDtoExtensions 
    {
        
        public static ItemDto MapToDto(this Item item)
        {
            return new(item.Id, item.Name, item.Description, item.Price, item.CreatedDate, item.UpdatedDate);
        }
        
        public static Item ToItem(this CreateItemDto itemDto)
        {
            return new () { Name = itemDto.Name, Description = itemDto.Description, Price = itemDto.Price };
        }
        
        public static Item ToItem(this UpdateItemDto itemDto)
        {
            return new () { Name = itemDto.Name, Description = itemDto.Description, Price = itemDto.Price };
        }
    }
}