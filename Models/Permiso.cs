
using System.ComponentModel.DataAnnotations;

namespace MotoFix.Models
{
    public class Permiso
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Clave { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public ICollection<RolPermiso> RolPermisos { get; set; }
            = new List<RolPermiso>();
    }
}
