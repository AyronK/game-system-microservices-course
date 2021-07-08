using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Core;
using Play.Catalog.Service.Data;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Guid, Item> _repository;

        public ItemsController()
        {
            _repository = new StaticItemsRepository();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get()
        {
            IEnumerable<Item> items = await _repository.GetAll();
            return Ok(items.Select(i => i.MapToDto()));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetById(Guid id)
        {
            Item item = await _repository.Find(id);

            return item switch
            {
                null => NotFound(),
                _ => Ok(item.MapToDto()),
            };
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create(CreateItemDto dto)
        {
            Item createdItem = await _repository.Add(dto.ToItem());
            return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateItemDto dto)
        {
            if (!await _repository.Contains(id))
            {
                return NotFound();
            }
            
            await _repository.Update(id, dto.ToItem());
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _repository.Contains(id))
            {
                return NotFound();
            }
            
            await _repository.Remove(id);
            return NoContent();
        }
    }
}