using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using System.Security.Claims;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionController : ControllerBase
    {
        private readonly DormitoryDbContext _context;
        public InspectionController(DormitoryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddInspection(
            [FromForm] string roomNumber,
            [FromForm] IFormFile[]? files,
            [FromForm] string? comment,
            [FromForm] string rating)
        {
            var photos = new List<Photo>();
            if (files != null && files.Length > 0)
            {
                int i = 1;
                foreach (var file in files)
                {
                    var fileExtension = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    var fileName = $"{i}.{fileExtension}";

                    var dateString = DateTime.UtcNow.Date.ToString().Split()[0];

                    var directory = Path.Combine("Assessments", $"Room{roomNumber}", dateString);

                    var dbPath = Path.Combine(directory, fileName).Replace(@"\", "/");

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", directory);
                    Directory.CreateDirectory(Path.Combine(path));
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    photos.Add(new Photo()
                    {
                        Path = dbPath
                    });

                    i++;
                }
            }

            var roomNumberInt = Convert.ToInt32(roomNumber);
            var user = await _context.Users.FindAsync(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            Room room;
            if (!await _context.Rooms.AnyAsync(r => r.Number == roomNumberInt))
            {
                room = new Room()
                {
                    Number = roomNumberInt,
                };
                await _context.Rooms.AddAsync(room);
            }
            else
            {
                room = await _context.Rooms.FindAsync(roomNumberInt);
            }

            var assessment = new Assessment()
            {
                Comment = comment,
                Inspector = user,
                Rating = Convert.ToInt32(rating),
                Room = room,
                Photos = photos,
                DateTime = DateTime.Now,
            };
            
            await _context.AddAsync(assessment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
