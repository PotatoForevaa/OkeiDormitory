using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using OkeiDormitory.Services;
using System.Security.Claims;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DormitoryDbContext _context;
        private readonly AccountService _accountService;
        public AuthenticationController(DormitoryDbContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string password, string login)
        {
            var loginResult = await _accountService.Login(password, login);
            if (!loginResult) return Unauthorized("Неверный логин или пароль!");
            return Ok("Успешная авторизация");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(string password, string fullName, string login, string role)
        {
            var registerResult = await _accountService.Register(password, fullName, login, role);
            if (!registerResult) return BadRequest("Пользователь с таким логином уже существует");
            return Ok("Регистрация успешна");
        }

        [HttpGet("HashAdminPassword")]
        public async Task<IActionResult> HashAdminPassword(string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == "Admin");
            PasswordHasher <User> hasher = new PasswordHasher<User>();
            var newPassword = hasher.HashPassword(user, password);
            user.Password = newPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user.Password);
        }
    }
}
