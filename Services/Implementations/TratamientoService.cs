using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Services.Implementations
{
    public class TratamientoService : ITratamientoService
    {
        private readonly MySqlDBContext _context;

        public TratamientoService(MySqlDBContext context)
        {
            _context = context;
        }

        public async Task<List<Tratamiento>> GetAllAsync()
        {
            return await _context.Tratamientos
                .Include(t => t.Cita)
                .ToListAsync();
        }

        public async Task<Tratamiento> GetByIdAsync(int id)
        {
            return await _context.Tratamientos
                .Include(t => t.Cita)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Tratamiento t)
        {
            _context.Tratamientos.Add(t);
            await _context.SaveChangesAsync();
            return (true, "Tratamiento creado");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Tratamiento t)
        {
            _context.Tratamientos.Update(t);
            await _context.SaveChangesAsync();
            return (true, "Tratamiento actualizado");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await _context.Tratamientos.FindAsync(id);
            if (obj == null) return false;

            _context.Tratamientos.Remove(obj);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}