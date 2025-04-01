using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Services;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        private readonly DormitoryDbContext _context;
        private readonly QrService _qrService;
        public QrController(DormitoryDbContext context, QrService qrService)
        {
            _context = context;
            _qrService = qrService;
        }
        [HttpPost("CreateQrCodes")]
        public async Task<IActionResult> CreateQrCodes()
        {
            await _qrService.CreateAllQrCodesAsync();
            return Ok();
        }

        [HttpGet("DownloadQr")]
        public async Task<IActionResult> DownloadQr(int roomNumber)
        {
            return Ok();
        }
    }
}
