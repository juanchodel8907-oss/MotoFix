using Microsoft.EntityFrameworkCore;
using MotoFix.Models;

namespace MotoFix.Data
{
    public class MotoFixContext : DbContext
    {
        public MotoFixContext(DbContextOptions<MotoFixContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Motocicleta> Motocicletas { get; set; }
    }
}