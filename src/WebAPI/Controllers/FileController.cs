using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class FileController : ControllerBase
    {
        [HttpGet()]
        public async Task<ActionResult> GetUserCatalogs(CancellationToken ct)
        {
            return Ok();
        }

    }
}
