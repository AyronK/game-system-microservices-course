using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Core;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Data;
using Play.Inventory.Service.Data.Entities;
using Play.Inventory.Service.Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Guid, InventoryItem> _itemsRepository;
        private readonly CatalogClient _catalogClient;
        
        public ItemsController(IRepository<Guid, InventoryItem> itemsRepository, CatalogClient catalogClient)
        {
            _itemsRepository = itemsRepository;
            _catalogClient = catalogClient;
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<InventoryItemDto>> Get(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            IReadOnlyCollection<CatalogItemDto> catalogItems = await _catalogClient.GetCatalogItems();
            IEnumerable<InventoryItem> inventoryItems = await _itemsRepository.GetAll(i => i.UserId == userId);
            
            IEnumerable<InventoryItemDto> inventoryItemsDtos = inventoryItems.Select(selector: inventoryItem =>
            {
                (_, string name, string description) = catalogItems
                    .Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                
                return inventoryItem.AsDto(name, description);
            });

            return Ok(inventoryItemsDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(GrantItemsDto dto)
        {
            InventoryItem inventoryItem = await _itemsRepository
                .Get(i => i.UserId == dto.UserId && i.CatalogItemId == dto.CatalogItemId);

            if (inventoryItem is null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = dto.CatalogItemId,
                    UserId = dto.UserId,
                    Quantity = dto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow,
                };
                
                await _itemsRepository.Add(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += dto.Quantity;
                await _itemsRepository.Update(inventoryItem.Id, inventoryItem);
            }

            return Ok();
        }
    }
}