using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.Shared.Models;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
using WpfClientApp.Endpoints;

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
        // Добавить errorModel && midleware???
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof())]
        public async Task<ActionResult> GetAllItems(CancellationToken ct = default)
        {
            var items = await _itemService.GetAllItems(ct);

            return Ok(items);
        }

        [HttpPost()]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateItem([FromForm] ItemRequestModel item, CancellationToken ct = default)
        {
            await _itemService.CreateItem(item, ct);

            return Ok();
        }


        [HttpGet("{itemId:int}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ItemResponseModel))]
        // Добавить errorModel && midleware???
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof())]
        public async Task<ActionResult> GetItem([FromRoute] int itemId, CancellationToken ct = default)
        {
            var item = await _itemService.GetItem(itemId, ct);

            return Ok(item);
        }

        [HttpDelete("{itemId:int}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ItemResponseModel))]
        // Добавить errorModel && midleware???
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof())]
        public async Task<ActionResult> DeleteItem([FromRoute] int itemId, CancellationToken ct = default)
        {
            await _itemService.DeleteItem(itemId, ct);

            return NoContent();
        }

    }
}
