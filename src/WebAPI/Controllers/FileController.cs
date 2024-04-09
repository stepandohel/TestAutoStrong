using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Modeles;
using WpfClientApp.Endpoints;

namespace WebAPI.Controllers
{
    [Route(ItemEndpoints.ControllerRoute)]
    public class FileController : ControllerBase
    {
        IWebHostEnvironment _appEnvironment;

        public FileController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserCatalogs(CancellationToken ct)
        {
            return Ok();
        }

        [HttpPost(ItemEndpoints.CreateItem)]
        public async Task<ActionResult> CreateItem([FromForm] ItemCM item)
        {
            //Вот это надо в сервис вынести
            string path = "/Files/" + item.File.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await item.File.CopyToAsync(fileStream);
            }


            return Ok();
        }

    }
}
