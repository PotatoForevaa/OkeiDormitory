using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using OkeiDormitory.Models.ViewModels;

namespace OkeiDormitory.Controllers.Mvc
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly DormitoryDbContext _context;

        public HomeController(DormitoryDbContext context)
        {
            _context = context;
        }

        [Route("/")]
        [Route("[controller]")]
        [Route("[controller]/Feed")]
        public async Task<IActionResult> Feed()
        {
            return View();
        }

        [Route("[controller]/Inspection")]
        [Authorize(Roles = "Inspector,Admin,Administration")]
        public async Task<IActionResult> Inspection(int? roomNumber)
        {
            return View();
        }

        [Route("[controller]/Administration")]
        [Authorize(Roles = "Admin,Administration")]
        public async Task<IActionResult> Administration()
        {
            var users = await _context.Users.ToListAsync();
            var vm = new AdministrationViewModel()
            {
                Users = users
            };
            return View(vm);
        }

        [Route("[controller]/Import")]
        [Authorize(Roles = "Admin,Administration")]
        public async Task<IActionResult> Import()
        {
            return View();
        }
    }
}
