using Habitus.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Habitus.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Habito> Habitos { get; set; }
    }
}