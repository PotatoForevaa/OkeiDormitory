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

        [Route("home")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
