using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
using WpfClientApp.Endpoints;

namespace WebAPI.Controllers
{
    [Route(ItemEndpoints.ControllerRoute)]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IItemService _itemService;

        public FileController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet()]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemRequestModel>))]
        // Добавить errorModel && midleware???
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof())]
        public async Task<ActionResult> GetAllItems(CancellationToken ct)
        {
            var items = await _itemService.GetAllItems();

            return Ok(items);
        }

        [HttpPost()]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateItem([FromForm] ItemRequestModel item)
        {
            await _itemService.CreateItem(item);

            return Ok();
        }

    }
}
