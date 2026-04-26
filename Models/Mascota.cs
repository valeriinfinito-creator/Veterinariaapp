using System.ComponentModel.DataAnnotations;

namespace VeterinariaApp.Models
{
    public class Mascota
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Especie { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Raza { get; set; }

        [Range(0, 100)]
        public int Edad { get; set; }

        [Range(0.1, 200)]
        public double Peso { get; set; }

        [Required]
        public int PropietarioId { get; set; }

        public Propietario? Propietario { get; set; }

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}