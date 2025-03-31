using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using System.Security.Claims;

namespace OkeiDormitory.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DormitoryDbContext _context;
        public AuthenticationController(DormitoryDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string password, string login)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == login);
            if (user == null) { return Unauthorized("Пользователь не найден"); }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var verification = hasher.VerifyHashedPassword(user, user.Password, password);
            if (verification == PasswordVerificationResult.Failed) { return Unauthorized("Неверный пароль"); }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                AllowRefresh = true
            };

            var principal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return Ok("Авторизация прошла успешно");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(string password, string fullName, string login, string role)
        {
            var user = new User()
            {
                FullName = fullName,
                Login = login,
                Role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role)
            };
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, password);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok("Регистрация успешна");
        }
    }
}
