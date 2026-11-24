
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotoFix.Data;
using MotoFix.Models;

namespace MotoFix.Authorization
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly MotoFixContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionAuthorizationHandler(
            MotoFixContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Obtener el identificador del usuario autenticado.
            var userId = _userManager.GetUserId(context.User);

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // Buscar al usuario en la base de datos.
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return;
            }

            // Obtener los roles del usuario.
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any())
            {
                return;
            }

            // Comprobar si alguno de sus roles tiene el permiso requerido.
            var hasPermission = await _context.RolPermisos
                .AnyAsync(rp =>
                    userRoles.Contains(rp.Rol.Name!) &&
                    rp.Permiso.Clave == requirement.Permission);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}