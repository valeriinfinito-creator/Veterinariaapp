using System.ComponentModel.DataAnnotations;

namespace VeterinariaApp.Models
{
    public class Veterinario
    {
        public int Id { get; set; }

        // 🔹 Datos del veterinario
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Especialidad { get; set; }

        [Required]
        [StringLength(100)]
        public string Horario { get; set; } 
        // Ej: "08:00-17:00"

        // 🔹 Relación con citas
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}