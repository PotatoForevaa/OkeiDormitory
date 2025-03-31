using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;
using System.Net;

namespace OkeiDormitory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                    options.SlidingExpiration = true;
                });
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DormitoryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseSeeding((context, _) =>
                {
                    if (!context.Set<Role>().Any(r => r.Name == "Admin"))
                        context.Set<Role>().Add(new Role() { Name = "Admin" });
                    if (!context.Set<Role>().Any(r => r.Name == "Administrator"))
                        context.Set<Role>().Add(new Role() { Name = "Administrator" });
                    if (!context.Set<Role>().Any(r => r.Name == "Inspector"))
                        context.Set<Role>().Add(new Role() { Name = "Inspector" });
                    context.SaveChanges();
                })
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    if (!await context.Set<Role>().AnyAsync(r => r.Name == "Admin", cancellationToken))
                        await context.Set<Role>().AddAsync(new Role() { Name = "Admin" });
                    if (!await context.Set<Role>().AnyAsync(r => r.Name == "Administrator", cancellationToken))
                        await context.Set<Role>().AddAsync(new Role() { Name = "Administrator" });
                    if (!await context.Set<Role>().AnyAsync(r => r.Name == "Inspector", cancellationToken))
                        await context.Set<Role>().AddAsync(new Role() { Name = "Inspector" });
                    await context.SaveChangesAsync(cancellationToken);
                }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
