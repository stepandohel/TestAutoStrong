using Microsoft.AspNetCore.Mvc;
using Server.Shared.Endpoints;
using Server.Shared.Models;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route(ItemEndpoints.ControllerRoute)]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet()]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemResponseModel>))]
        public async Task<ActionResult> GetAllItems(CancellationToken ct = default)
        {
            var items = await _itemService.GetAllItems(ct);

            return Ok(items);
        }

        [HttpPost()]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateItem([FromForm] ItemRequestModel item, CancellationToken ct = default)
        {
            var createdItem = await _itemService.CreateItem(item, ct);

            return CreatedAtAction(nameof(GetItem), new { itemId = createdItem.Id }, createdItem);
        }


        [HttpGet("{itemId:int}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ItemResponseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetItem([FromRoute] int itemId, CancellationToken ct = default)
        {
            var item = await _itemService.GetItem(itemId, ct);

            if (item is null)
            {
                return BadRequest();
            }

            return Ok(item);
        }

        [HttpPut("{itemId:int}")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ItemResponseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateItem([FromRoute] int itemId, [FromForm] ItemRequestModel item, CancellationToken ct = default)
        {
            var updatedItem = await _itemService.UpdateItem(itemId, item, ct);

            if (updatedItem is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetItem), new { itemId = updatedItem.Id }, updatedItem);
        }

        [HttpDelete("{itemId:int}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteItem([FromRoute] int itemId, CancellationToken ct = default)
        {
           var isDeleted =  await _itemService.DeleteItem(itemId, ct);

            if(!isDeleted)
            {
                return BadRequest();
            }

            return NoContent();
        }

    }
}
