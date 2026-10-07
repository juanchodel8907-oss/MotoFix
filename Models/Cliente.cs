using System.ComponentModel.DataAnnotations;

namespace MotoFix.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Documento { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        public string Correo { get; set; } = string.Empty;

        public ICollection<Motocicleta> Motocicletas { get; set; } = new List<Motocicleta>();
    }
}