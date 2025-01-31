using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    public static readonly List<ItemDto> Items = [
        new ItemDto(Guid.NewGuid(), "Potion", "Uses to cure something", 5, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Antidot", "Uses to relief pains and cure", 6, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Dark Sword", "used to defeat enimies", 10, DateTimeOffset.UtcNow)
    ];

    [HttpGet]
    public IEnumerable<ItemDto> Get() => Items;

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
        var item = Items.SingleOrDefault(x => x.Id == id)!;

        if (item is null) return NotFound();

        return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto itemDto)
    {
        var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
        Items.Add(item);

        return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
    }

    [HttpPut]
    public IActionResult Put(Guid id, UpdateItemDto updateItem)
    {
        var existingItem = Items.SingleOrDefault(x => x.Id == id);

        if (existingItem is null) return NotFound();

        var updatedItem = existingItem! with
        {
            Name = updateItem.Name,
            Description = updateItem.Description,
            Price = updateItem.Price
        };

        var index = Items.FindIndex(x => x.Id == id);
        Items[index] = updatedItem;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = Items.FindIndex(x => x.Id == id);

        if(index < 0) return NotFound();

        Items.RemoveAt(index);

        return NoContent();
    }
}
