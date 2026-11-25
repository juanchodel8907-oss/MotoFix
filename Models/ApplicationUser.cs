
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotoFix.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;
    }
}
