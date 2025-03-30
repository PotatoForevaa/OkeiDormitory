using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using OkeiDormitory.Models.Entities;

namespace OkeiDormitory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DormitoryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddAuthentication("Cookies")
                .AddCookie(options => options.LoginPath = "/login");
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
