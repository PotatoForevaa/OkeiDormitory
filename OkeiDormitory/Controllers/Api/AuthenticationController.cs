using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using OkeiDormitory.Data;
using OkeiDormitory.Models.DTOs;
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
        public async Task<IActionResult> Login([FromBody] LoginData loginData)
        {
            var loginResult = await _accountService.Login(loginData.Password, loginData.Login);
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

        [HttpGet("CreateAdminUser")]
        public async Task<IActionResult> CreateAdminUser(string password)
        {
            var user = new User()
            {
                FullName = "Админов Админ Админович",
                Login = "Admin",
                Password = password,
                Role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin")
            };
            PasswordHasher <User> hasher = new PasswordHasher<User>();
            var newPassword = hasher.HashPassword(user, password);
            user.Password = newPassword;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user.Password);
        }
    }
}
