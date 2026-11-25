
using Microsoft.AspNetCore.Identity;
using MotoFix.Models;

namespace MotoFix.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var configuration = serviceProvider
                .GetRequiredService<IConfiguration>();

            // Obtener las credenciales desde User Secrets.
            var email = configuration["AdminUser:Email"];
            var password = configuration["AdminUser:Password"];
            var nombre = configuration["AdminUser:Nombre"];
            var apellido = configuration["AdminUser:Apellido"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido))
            {
                throw new InvalidOperationException(
                    "Faltan las credenciales del administrador en User Secrets.");
            }

            const string roleName = "Administrador";

            // Crear el rol si todavía no existe.
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "No se pudo crear el rol Administrador: " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            const string employeeRole = "Empleado";

            if (!await roleManager.RoleExistsAsync(employeeRole))
            {
                var employeeResult = await roleManager.CreateAsync(
                    new IdentityRole(employeeRole));

                if (!employeeResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "No se pudo crear el rol Empleado: " +
                        string.Join(", ", employeeResult.Errors.Select(e => e.Description)));
                }
            }

            // Buscar al administrador por su correo.
            var user = await userManager.FindByEmailAsync(email);

            // Crear el usuario si todavía no existe.
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Nombre = nombre,
                    Apellido = apellido,
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(
                    user,
                    password);

                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "No se pudo crear el administrador: " +
                        string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }
            }

            // Asociar el usuario al rol Administrador.
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    roleName);

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "No se pudo asignar el rol al administrador: " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
