using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Models;

namespace Infrastructure
{
    public class InfrastructureContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<User>  Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseMySql("server=localhost;user=root;database=lecroise;port=3306;Connect Timeout=5;", new MariaDbServerVersion(new Version(10,4,19)));
    }
}
