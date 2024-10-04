using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionControl.Models
{
    class AppDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<WorkPosition> WorkPositions { get; set; }

        public DbSet<UserWorkPosition> UserWorkPositions { get; set; }

        string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWorkPosition>()
                .HasKey(uwp => uwp.Id); 

            modelBuilder.Entity<UserWorkPosition>()
                .HasOne(uwp => uwp.User)
                .WithMany(u => u.UserWorkPositions) 
                .HasForeignKey(uwp => uwp.UserId);

            modelBuilder.Entity<UserWorkPosition>()
                .HasOne(uwp => uwp.WorkPosition)
                .WithMany() 
                .HasForeignKey(uwp => uwp.PositionId);

            base.OnModelCreating(modelBuilder);
        }







    }
}
