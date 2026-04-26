using System.ComponentModel.DataAnnotations;

namespace VeterinariaApp.Models
{
    public class Tratamiento
    {
        public int Id { get; set; }

        // 🔹 Datos clínicos
        [Required]
        [StringLength(500)]
        public string Diagnostico { get; set; }

        [StringLength(1000)]
        public string Observaciones { get; set; }

        // 🔹 Relación 1:1 con Cita
        [Required]
        public int CitaId { get; set; }
        public Cita Cita { get; set; }

        // 🔹 Relación con medicamentos
        public ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}