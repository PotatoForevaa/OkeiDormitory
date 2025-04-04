﻿using System;
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
            var assessments = await _context.Assessments
                .Include(a => a.Photos)
                .Include(a => a.Inspector)
                .Include(a => a.Room).OrderByDescending(a => a.DateTime)
                .ToListAsync();
            var vm = new FeedViewModel()
            {
                Assessments = assessments,
            };

            return View(vm);
        }

        [Route("[controller]/Inspection")]
        [Authorize(Roles = "Inspector,Admin,Administration")]
        public async Task<IActionResult> Inspection(int? roomNumber)
        {
            ViewData["RoomNumber"] = roomNumber;
            return View();
        }

        [Route("[controller]/Rooms")]
        public async Task<IActionResult> Rooms()
        {
            var vm = new RoomsViewModel()
            {
                Rooms = await _context.Rooms.Include(r => r.Rewards).Include(r => r.Assessments).ToListAsync()
            };
            return View(vm);
        }

        [Route("[controller]/Rooms/{roomNumber}")]
        public async Task<IActionResult> Room(int roomNumber)
        {
            var room = await _context.Rooms.Include(r => r.Assessments).Include(r => r.Rewards).FirstOrDefaultAsync(r => r.Number == roomNumber);
            return View(room);
        }

        [Route("[controller]/Administration")]
        [Authorize(Roles = "Admin,Administration")]
        public async Task<IActionResult> Administration()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
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
