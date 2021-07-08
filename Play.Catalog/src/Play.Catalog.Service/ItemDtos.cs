using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate, DateTimeOffset UpdatedDate);

    public record CreateItemDto
    (
        [Required] [StringLength(64)] string Name,
        [Required] [StringLength(128)] string Description,
        [Range(0, 1000)] decimal Price
    );
    
    public record UpdateItemDto
    (
        [Required] [StringLength(64)] string Name,
        [Required] [StringLength(128)] string Description,
        [Range(0, 1000)] decimal Price
    );
}