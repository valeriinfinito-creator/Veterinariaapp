using VeterinariaApp.Models;

namespace VeterinariaApp.Validators
{
    public static class CitaValidator
    {
        public static (bool IsValid, string Message) Validar(Cita cita)
        {
            // ❌ Fecha pasada
            if (cita.Fecha.Date < DateTime.Now.Date)
                return (false, "No se pueden agendar citas en fechas pasadas");

            // ❌ Hora inválida
            if (cita.HoraFin <= cita.HoraInicio)
                return (false, "La hora fin debe ser mayor que la hora inicio");

            // ❌ Campos obligatorios
            if (cita.MascotaId <= 0)
                return (false, "Debe seleccionar una mascota");

            if (cita.VeterinarioId <= 0)
                return (false, "Debe seleccionar un veterinario");

            return (true, "OK");
        }
    }
}