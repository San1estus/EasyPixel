using CrochetItAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrochetItAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<Patron> Patrones { get; set; }
    }
}
