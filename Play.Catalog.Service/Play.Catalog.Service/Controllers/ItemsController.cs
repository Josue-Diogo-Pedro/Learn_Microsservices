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
    public ItemDto GetById(Guid id) => Items.SingleOrDefault(x => x.Id == id)!;

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

        var updatedItem = existingItem with
        {
            Name = updateItem.Name,
            Description = updateItem.Description,
            Price = updateItem.Price
        };

        var index = Items.FindIndex(x => x.Id == id);
        Items[index] = updatedItem;

        return NoContent();
    }
}
