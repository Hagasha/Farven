using Farven.Models;
using Microsoft.EntityFrameworkCore;

namespace Farven.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
