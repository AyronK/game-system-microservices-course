using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Guid, Item> repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
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
            
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = createdDate,
                UpdatedDate = createdDate
            };
            
            await _repository.Add(item);
            await _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description, item.Price));
            
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateItemDto dto)
        {
            Item existingItem = await _repository.Get(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }

            Item item = new()
            {
                Id = existingItem.Id,
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                CreatedDate = existingItem.CreatedDate,
                UpdatedDate = DateTimeOffset.UtcNow
            };
            
            await _repository.Update(item);
            await _publishEndpoint.Publish(new CatalogItemUpdated(item.Id, item.Name, item.Description, item.Price));
            
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
            await _publishEndpoint.Publish(new CatalogItemDeleted(id));
            
            return NoContent();
        }
    }
}