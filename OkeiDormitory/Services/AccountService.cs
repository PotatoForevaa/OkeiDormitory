using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using System.Security.Claims;
using System.Data;

namespace OkeiDormitory.Services
{
    public class AccountService
    {
        private readonly DormitoryDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(DormitoryDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<bool> Login(string login, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == login);
            if (user == null) return false;

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var verification = hasher.VerifyHashedPassword(user, user.Password, password);
            if (verification == PasswordVerificationResult.Failed) return false;

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

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return true;
        }

        public async Task<bool> Register(string login, string password, string fullName, string role)
        {
            if (_context.Users.Any(u => u.Login == login)) 
                return false;
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
            return true;
        }
    }
}
