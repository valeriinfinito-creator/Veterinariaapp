using System.ComponentModel.DataAnnotations;

namespace VeterinariaApp.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        // 🔹 Datos personales
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Documento { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // 🔹 Relación con mascotas
        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}