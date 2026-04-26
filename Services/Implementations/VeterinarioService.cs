using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Services.Implementations
{
    public class VeterinarioService : IVeterinarioService
    {
        private readonly MySqlDBContext _context;

        public VeterinarioService(MySqlDBContext context)
        {
            _context = context;
        }

        // 🔹 GET ALL
        public async Task<List<Veterinario>> GetAllAsync()
        {
            return await _context.Veterinarios
                .Include(v => v.Citas)
                .ToListAsync();
        }

        // 🔹 GET BY ID
        public async Task<Veterinario> GetByIdAsync(int id)
        {
            return await _context.Veterinarios
                .Include(v => v.Citas)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // 🔹 CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Veterinario veterinario)
        {
            try
            {
                // ❌ Validar duplicado (Nombre + Especialidad)
                if (await ExisteVeterinarioAsync(veterinario.Nombre, veterinario.Especialidad))
                    return (false, "El veterinario ya existe con esa especialidad");

                _context.Veterinarios.Add(veterinario);
                await _context.SaveChangesAsync();

                return (true, "Veterinario creado correctamente");
            }
            catch (Exception ex)
            {
                return (false, "Error: " + ex.Message);
            }
        }

        // 🔹 UPDATE
        public async Task<(bool Success, string Message)> UpdateAsync(Veterinario veterinario)
        {
            try
            {
                var existente = await _context.Veterinarios.FindAsync(veterinario.Id);

                if (existente == null)
                    return (false, "Veterinario no encontrado");

                // ❌ Validar duplicado (excluyendo el mismo)
                if (await _context.Veterinarios.AnyAsync(v =>
                    v.Nombre == veterinario.Nombre &&
                    v.Especialidad == veterinario.Especialidad &&
                    v.Id != veterinario.Id))
                    return (false, "Ya existe un veterinario con ese nombre y especialidad");

                _context.Entry(existente).CurrentValues.SetValues(veterinario);
                await _context.SaveChangesAsync();

                return (true, "Veterinario actualizado correctamente");
            }
            catch (Exception ex)
            {
                return (false, "Error: " + ex.Message);
            }
        }

        // 🔹 DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario == null) return false;

            _context.Veterinarios.Remove(veterinario);
            await _context.SaveChangesAsync();

            return true;
        }

        // 🔹 VALIDAR EXISTENCIA
        public async Task<bool> ExisteVeterinarioAsync(string nombre, string especialidad)
        {
            return await _context.Veterinarios
                .AnyAsync(v => v.Nombre == nombre && v.Especialidad == especialidad);
        }

        // 🔹 DISPONIBILIDAD (CLAVE)
        public async Task<bool> EstaDisponibleAsync(
            int veterinarioId,
            DateTime fecha,
            DateTime horaInicio,
            DateTime horaFin)
        {
            // 🔸 Validar que esté dentro del horario (ej: "08:00-17:00")
            var veterinario = await _context.Veterinarios.FindAsync(veterinarioId);

            if (veterinario == null)
                return false;

            var partes = veterinario.Horario.Split('-');

            var inicioJornada = TimeSpan.Parse(partes[0]);
            var finJornada = TimeSpan.Parse(partes[1]);

            if (horaInicio.TimeOfDay < inicioJornada || horaFin.TimeOfDay > finJornada)
                return false;

            // 🔸 Validar que no tenga citas en ese rango
            return !await _context.Citas.AnyAsync(c =>
                c.VeterinarioId == veterinarioId &&
                c.Fecha.Date == fecha.Date &&
                horaInicio < c.HoraFin &&
                horaFin > c.HoraInicio
            );
        }
    }
}