using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateQrCodes()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadRoomQr(int roomNumber)
        {
            return Ok();
        }
    }
}
