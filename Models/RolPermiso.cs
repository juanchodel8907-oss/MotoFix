
using Microsoft.AspNetCore.Identity;

namespace MotoFix.Models
{
    public class RolPermiso
    {
        public string RolId { get; set; } = string.Empty;

        public IdentityRole Rol { get; set; } = null!;

        public int PermisoId { get; set; }

        public Permiso Permiso { get; set; } = null!;
    }
}
