using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Data;
using Play.Catalog.Service.Data.Entities;
using Play.Common.Core;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Guid, Item> _repository;

        public ItemsController(IRepository<Guid, Item> repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get()
        {
            IEnumerable<Item> items = await _repository.GetAll();
            return Ok(items.Select(i => i.AsDto()));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            Item item = await _repository.Get(id);

            return item switch
            {
                null => NotFound(),
                _ => Ok(item.AsDto()),
            };
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create(CreateItemDto dto)
        {
            DateTimeOffset createdDate = DateTimeOffset.UtcNow;
            Item itemToAdd = new()
            {
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = createdDate,
                UpdatedDate = createdDate
            };
            
            Guid createdItemId = await _repository.Add(itemToAdd);
            itemToAdd.Id = createdItemId;
            
            return CreatedAtAction(nameof(GetById), new { id = itemToAdd.Id }, itemToAdd);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateItemDto dto)
        {
            Item existingItem = await _repository.Get(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }

            Item itemToUpdate = new()
            {
                Id = existingItem.Id,
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = existingItem.CreatedDate,
                UpdatedDate = DateTimeOffset.UtcNow
            };
            
            await _repository.Update(id, itemToUpdate);
            
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Item existingItem = await _repository.Get(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }
            
            await _repository.Remove(id);
            
            return NoContent();
        }
    }
}