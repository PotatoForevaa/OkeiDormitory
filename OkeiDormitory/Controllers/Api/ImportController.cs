using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using OkeiDormitory.Services;
using System.IO;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImportController : ControllerBase
    {
        private readonly DormitoryDbContext _context;
        private readonly AccountService _accountService;
        public ImportController(DormitoryDbContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        [HttpPost("Users")]
        [Authorize]
        public async Task<IActionResult> ImportUsers([FromForm] IFormFile file)
        {
            ExcelPackage.License.SetNonCommercialPersonal("My Name");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            FileInfo fileInfo = new FileInfo(filePath);

            using var package = new ExcelPackage(fileInfo);
            var sheet = package.Workbook.Worksheets[0];

            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;

            for (int row = start.Row + 1; row <= end.Row; row++)
            {
                var value = sheet.Cells[row, 1].Value;
                if (value == null) continue;

                string role;
                switch (sheet.Cells[row, 2].Value.ToString()) 
                {
                    case ("Админ"):
                        role = "Admin";
                        break;

                    case ("Администратор"):
                        role = "Administrator";
                        break;

                    default:
                        role = "Inspector";
                        break;
                }

                var login = sheet.Cells[row, 3].Value.ToString();
                var password = sheet.Cells[row, 4].Value.ToString();
                var fullname = sheet.Cells[row, 1].Value.ToString();

                await _accountService.Register(login, password, fullname, role);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
