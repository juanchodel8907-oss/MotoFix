
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotoFix.Models;

namespace MotoFix.Data
{
    public class MotoFixContext : IdentityDbContext<ApplicationUser>
    {
        public MotoFixContext(DbContextOptions<MotoFixContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Motocicleta> Motocicletas { get; set; }

        public DbSet<Permiso> Permisos { get; set; }

        public DbSet<RolPermiso> RolPermisos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave primaria compuesta de RolPermiso.
            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });

            // Relación entre RolPermiso e IdentityRole.
            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany()
                .HasForeignKey(rp => rp.RolId);

            // Relación entre RolPermiso y Permiso.
            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.PermisoId);

            // Evitar eliminar clientes que tengan motocicletas asociadas.
            modelBuilder.Entity<Motocicleta>()
                .HasOne(m => m.Cliente)
                .WithMany(c => c.Motocicletas)
                .HasForeignKey(m => m.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
