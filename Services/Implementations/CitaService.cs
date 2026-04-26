using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Helpers;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;
using VeterinariaApp.Validators;

namespace VeterinariaApp.Services.Implementations
{
    public class CitaService : ICitaService
    {
        private readonly MySqlDBContext _context;
        private readonly EmailHelper _email;

        public CitaService(MySqlDBContext context, EmailHelper email)
        {
            _context = context;
            _email = email;
        }

        // 🔹 CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Cita cita)
        {
            try
            {
                var validacion = CitaValidator.Validar(cita);
                if (!validacion.IsValid)
                    return (false, validacion.Message);

                if (await TieneCitasActivasAsync(cita.MascotaId))
                    return (false, "La mascota ya tiene 2 citas activas");

                if (await EstaBloqueadaAsync(cita.MascotaId))
                    return (false, "Mascota bloqueada por inasistencias");

                if (await HayConflictoHorarioAsync(
                    cita.VeterinarioId,
                    cita.Fecha,
                    cita.HoraInicio,
                    cita.HoraFin))
                    return (false, "El veterinario ya tiene una cita en ese horario");

                cita.Estado = "Programada";

                _context.Citas.Add(cita);
                await _context.SaveChangesAsync();

                // 🔥 IMPORTANTE: recargar datos con Include
                var citaCompleta = await _context.Citas
                    .Include(c => c.Mascota)
                    .Include(c => c.Veterinario)
                    .FirstOrDefaultAsync(c => c.Id == cita.Id);

                if (citaCompleta != null)
                {
                    await _email.EnviarCitaAsignada(
                        "juandaortega0712@gmail.com",
                        citaCompleta.Mascota.Nombre,
                        citaCompleta.Fecha.ToString("dd/MM/yyyy"),
                        citaCompleta.Veterinario.Nombre
                    );
                }

                return (true, "Cita creada correctamente");
            }
            catch (Exception ex)
            {
                return (false, "Error: " + ex.Message);
            }
        }

        // 🔹 GET ALL
        public async Task<List<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .ToListAsync();
        }

        // 🔹 GET BY ID
        public async Task<Cita> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // 🔹 UPDATE
        public async Task<(bool Success, string Message)> UpdateAsync(Cita cita)
        {
            var existente = await _context.Citas.FindAsync(cita.Id);
            if (existente == null)
                return (false, "Cita no encontrada");

            _context.Entry(existente).CurrentValues.SetValues(cita);
            await _context.SaveChangesAsync();

            return (true, "Cita actualizada");
        }

        // 🔹 DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 CAMBIAR ESTADO
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return false;

            cita.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 MASCOTAS
        public async Task<List<Mascota>> GetMascotasAsync()
        {
            return await _context.Mascotas.ToListAsync();
        }

        // 🔹 BLOQUEO
        public async Task<bool> EstaBloqueadaAsync(int mascotaId)
        {
            var inasistencias = await ContarInasistenciasAsync(mascotaId);
            return inasistencias >= 3;
        }

        public async Task<int> ContarInasistenciasAsync(int mascotaId)
        {
            return await _context.Citas
                .CountAsync(c => c.MascotaId == mascotaId &&
                                 c.Estado == "No asistió");
        }

        public async Task<bool> HayConflictoHorarioAsync(
            int veterinarioId,
            DateTime fecha,
            DateTime inicio,
            DateTime fin,
            int? citaId = null)
        {
            return await _context.Citas.AnyAsync(c =>
                c.VeterinarioId == veterinarioId &&
                c.Fecha.Date == fecha.Date &&
                c.Id != citaId &&
                inicio < c.HoraFin &&
                fin > c.HoraInicio
            );
        }

        public async Task<bool> TieneCitasActivasAsync(int mascotaId)
        {
            var count = await _context.Citas
                .CountAsync(c => c.MascotaId == mascotaId &&
                                 c.Estado == "Programada");

            return count >= 2;
        }
    }
}