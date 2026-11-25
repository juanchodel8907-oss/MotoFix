
using Microsoft.EntityFrameworkCore;
using MotoFix.Models;

namespace MotoFix.Data
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<MotoFixContext>();

            var permisos = new List<Permiso>
            {
                new Permiso
                {
                    Nombre = "Ver clientes",
                    Clave = "Clientes.Ver",
                    Descripcion = "Consultar los clientes registrados."
                },
                new Permiso
                {
                    Nombre = "Crear clientes",
                    Clave = "Clientes.Crear",
                    Descripcion = "Registrar nuevos clientes."
                },
                new Permiso
                {
                    Nombre = "Editar clientes",
                    Clave = "Clientes.Editar",
                    Descripcion = "Modificar información de clientes."
                },
                new Permiso
                {
                    Nombre = "Eliminar clientes",
                    Clave = "Clientes.Eliminar",
                    Descripcion = "Eliminar clientes registrados."
                },
                new Permiso
                {
                    Nombre = "Ver motocicletas",
                    Clave = "Motocicletas.Ver",
                    Descripcion = "Consultar las motocicletas registradas."
                },
                new Permiso
                {
                    Nombre = "Crear motocicletas",
                    Clave = "Motocicletas.Crear",
                    Descripcion = "Registrar nuevas motocicletas."
                },
                new Permiso
                {
                    Nombre = "Editar motocicletas",
                    Clave = "Motocicletas.Editar",
                    Descripcion = "Modificar información de motocicletas."
                },
                new Permiso
                {
                    Nombre = "Eliminar motocicletas",
                    Clave = "Motocicletas.Eliminar",
                    Descripcion = "Eliminar motocicletas registradas."
                },
                new Permiso
                {
                    Nombre = "Gestionar usuarios",
                    Clave = "Usuarios.Gestionar",
                    Descripcion = "Administrar las cuentas de usuario."
                },
                new Permiso
                {
                    Nombre = "Gestionar roles",
                    Clave = "Roles.Gestionar",
                    Descripcion = "Crear, consultar y editar roles."
                }
            };

            foreach (var permiso in permisos)
            {
                var existe = await context.Permisos
                    .AnyAsync(p => p.Clave == permiso.Clave);

                if (!existe)
                {
                    context.Permisos.Add(permiso);
                }
            }

            await context.SaveChangesAsync();


            // Buscar el rol Administrador.
            var adminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Administrador");

            if (adminRole != null)
            {
                // Obtener los permisos que ya tiene asignados.
                var permisosAsignados = await context.RolPermisos
                    .Where(rp => rp.RolId == adminRole.Id)
                    .Select(rp => rp.PermisoId)
                    .ToListAsync();

                // Buscar los permisos que todavía no tiene.
                var permisosFaltantes = await context.Permisos
                    .Where(p => !permisosAsignados.Contains(p.Id))
                    .ToListAsync();

                // Asignar los permisos faltantes al Administrador.
                foreach (var permiso in permisosFaltantes)
                {
                    context.RolPermisos.Add(new RolPermiso
                    {
                        RolId = adminRole.Id,
                        PermisoId = permiso.Id
                    });
                }

                await context.SaveChangesAsync();
            }

        }
    }
}
