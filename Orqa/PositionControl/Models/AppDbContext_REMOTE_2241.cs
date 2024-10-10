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
        






    }
}
