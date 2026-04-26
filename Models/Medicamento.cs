using System.ComponentModel.DataAnnotations;

namespace VeterinariaApp.Models
{
    public class Medicamento
    {
        public int Id { get; set; }

        // 🔹 Datos del medicamento
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Dosis { get; set; } // Ej: "500mg"

        [Required]
        [StringLength(100)]
        public string Frecuencia { get; set; } // Ej: "Cada 8 horas"

        // 🔹 Relación con tratamiento
        [Required]
        public int TratamientoId { get; set; }
        public Tratamiento Tratamiento { get; set; }
    }
}