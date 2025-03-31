using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OkeiDormitory.Models.Entities;

namespace OkeiDormitory.Data
{
    public class DormitoryDbContext : DbContext
    {
        public DormitoryDbContext(DbContextOptions<DormitoryDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>()
                .Property(r => r.Number)
                .ValueGeneratedNever();
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
