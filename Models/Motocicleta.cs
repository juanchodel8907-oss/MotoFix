using System.ComponentModel.DataAnnotations;

namespace MotoFix.Models
{
    public class Motocicleta
    {
        public int Id { get; set; }

        [Required]
        public string Placa { get; set; } = string.Empty;

        [Required]
        public string Marca { get; set; } = string.Empty;

        [Required]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        public int Cilindraje { get; set; }

        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; } 
    }
}