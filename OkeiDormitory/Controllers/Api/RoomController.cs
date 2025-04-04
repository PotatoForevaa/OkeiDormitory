using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly DormitoryDbContext _context;
        public RoomController(DormitoryDbContext context)
        {
            _context = context;
        }

        [HttpPost("{roomNumber}/reward")]
        [Authorize(Roles ="Admin,Administrator")]
        public async Task<IActionResult> Reward([FromRoute] int roomNumber, [FromForm] string name, [FromForm] string description)
        {
            var reward = new Reward()
            {
                AwardDate = DateTime.Now,
                Description = description,
                Name = name,
                Room = await _context.Rooms.FindAsync(roomNumber)
            };

            await _context.AddAsync(reward);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
