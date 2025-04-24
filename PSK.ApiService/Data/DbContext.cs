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
    }
}
