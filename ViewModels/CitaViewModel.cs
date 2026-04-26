using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VeterinariaApp.ViewModels
{
    public class CitaViewModel
    {
        public int Id { get; set; }

        [Required]
        public int MascotaId { get; set; }

        [Required]
        public int VeterinarioId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }

        public string Estado { get; set; } = "Programada";

        public List<SelectListItem> Mascotas { get; set; } = new();
        public List<SelectListItem> Veterinarios { get; set; } = new();
    }
}