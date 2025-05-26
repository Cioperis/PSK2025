using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.Latency;
using PSK.ServiceDefaults.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Transactions;

namespace PSK.ApiService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PositiveMessage> AutoMessages { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PositiveMessage> PositiveMessage { get; set; }
        public DbSet<UserMessage> UserMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discussion>()
                .Property(e => e.Version)
                .IsConcurrencyToken();
        }
    }
}
