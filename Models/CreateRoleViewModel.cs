
using System.ComponentModel.DataAnnotations;

namespace MotoFix.Models
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [Display(Name = "Nombre del rol")]
        public string Nombre { get; set; } = string.Empty;

        public List<int> PermisosSeleccionados { get; set; } = new();

        public List<Permiso> PermisosDisponibles { get; set; } = new();
    }
}
