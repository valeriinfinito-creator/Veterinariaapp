using System.ComponentModel.DataAnnotations;

namespace VeterinariaApp.Models
{
    public class Cita
    {
        public int Id { get; set; }

        // 🔹 Relaciones
        [Required]
        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }

        [Required]
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }

        // 🔹 Fecha y hora
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }

        // 🔹 Estado
        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Programada";

        // 🔹 Relación 1:1 con tratamiento
        public Tratamiento Tratamiento { get; set; }
    }
}