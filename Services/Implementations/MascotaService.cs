using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Services.Implementations
{
    public class MascotaService : IMascotaService
    {
        private readonly MySqlDBContext _context;

        public MascotaService(MySqlDBContext context)
        {
            _context = context;
        }

        // 🔹 GET ALL
        public async Task<List<Mascota>> GetAllAsync()
        {
            return await _context.Mascotas
                .Include(m => m.Propietario)
                .ToListAsync();
        }

        // 🔹 GET BY ID
        public async Task<Mascota> GetByIdAsync(int id)
        {
            return await _context.Mascotas
                .Include(m => m.Propietario)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // 🔹 CREATE (CORREGIDO)
        public async Task<(bool Success, string Message)> CreateAsync(Mascota mascota)
        {
            try
            {
                // 🔥 Validar que el propietario exista
                var propietarioExiste = await _context.Propietarios
                    .AnyAsync(p => p.Id == mascota.PropietarioId);

                if (!propietarioExiste)
                    return (false, "El propietario seleccionado no existe");

                _context.Mascotas.Add(mascota);
                await _context.SaveChangesAsync();

                return (true, "Mascota creada correctamente");
            }
            catch (Exception ex)
            {
                return (false, "Error al crear mascota: " + ex.Message);
            }
        }

        // 🔹 UPDATE (CORREGIDO)
        public async Task<(bool Success, string Message)> UpdateAsync(Mascota mascota)
        {
            try
            {
                var existente = await _context.Mascotas.FindAsync(mascota.Id);

                if (existente == null)
                    return (false, "Mascota no encontrada");

                _context.Entry(existente).CurrentValues.SetValues(mascota);
                await _context.SaveChangesAsync();

                return (true, "Mascota actualizada correctamente");
            }
            catch (Exception ex)
            {
                return (false, "Error al actualizar mascota: " + ex.Message);
            }
        }

        // 🔹 DELETE (CORREGIDO)
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var mascota = await _context.Mascotas.FindAsync(id);

                if (mascota == null)
                    return false;

                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}