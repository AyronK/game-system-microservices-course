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
        private readonly IRepository<Guid, InventoryItem> _inventoryItemsRepository;
        private readonly IRepository<Guid, CatalogItem> _catalogItemsRepository;

        public ItemsController(IRepository<Guid, InventoryItem> inventoryItemsRepository, IRepository<Guid, CatalogItem> catalogItemsRepository)
        {
            _inventoryItemsRepository = inventoryItemsRepository;
            _catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<InventoryItemDto>> Get(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            InventoryItem[] inventoryItems = (await _inventoryItemsRepository.GetAll(inventoryItem => inventoryItem.UserId == userId)).ToArray();
            IEnumerable<Guid> ids = inventoryItems.Select(inventoryItem => inventoryItem.CatalogItemId);
            IEnumerable<CatalogItem> catalogItems = await _catalogItemsRepository.GetAll(catalogItem => ids.Contains(catalogItem.Id));
            
            IEnumerable<InventoryItemDto> inventoryItemsDtos = inventoryItems.Select(inventoryItem =>
            {
                CatalogItem matchingCatalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.Id);
                return inventoryItem.AsDto(matchingCatalogItem.Name, matchingCatalogItem.Description);
            });

            return Ok(inventoryItemsDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(GrantItemsDto dto)
        {
            InventoryItem inventoryItem = await _inventoryItemsRepository
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
                
                await _inventoryItemsRepository.Add(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += dto.Quantity;
                await _inventoryItemsRepository.Update(inventoryItem);
            }

            return Ok();
        }
    }
}