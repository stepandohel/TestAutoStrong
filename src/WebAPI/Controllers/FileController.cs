using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Modeles;
using WebAPI.Services.Interfaces;
using WpfClientApp.Endpoints;

namespace WebAPI.Controllers
{
    [Route(ItemEndpoints.ControllerRoute)]
    public class FileController : ControllerBase
    {
        private readonly IItemService _itemService;

        public FileController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet(ItemEndpoints.GetAllItems)]
        public async Task<ActionResult> GetAllItems(CancellationToken ct)
        {
            var items = await _itemService.GetAllItems();

            return Ok(items);
        }

        [HttpPost(ItemEndpoints.CreateItem)]
        public async Task<ActionResult> CreateItem([FromForm] ItemCM item)
        {
            await _itemService.SaveFile(item);

            return Ok();
        }

    }
}
