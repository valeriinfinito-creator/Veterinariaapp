using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Services.Implementations
{
    public class PropietarioService : IPropietarioService
    {
        private readonly MySqlDBContext _context;

        public PropietarioService(MySqlDBContext context)
        {
            _context = context;
        }

        // 🔹 GET ALL
        public async Task<List<Propietario>> GetAllAsync()
        {
            return await _context.Propietarios
                .Include(p => p.Mascotas)
                .ToListAsync();
        }

        // 🔹 GET BY ID
        public async Task<Propietario> GetByIdAsync(int id)
        {
            return await _context.Propietarios
                .Include(p => p.Mascotas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // 🔹 CREATE
        public async Task<(bool Success, string Message)> CreateAsync(Propietario propietario)
        {
            if (await ExisteDocumentoAsync(propietario.Documento))
                return (false, "Ya existe un propietario con ese documento");

            if (await ExisteEmailAsync(propietario.Email))
                return (false, "Ya existe un propietario con ese email");

            _context.Propietarios.Add(propietario);
            await _context.SaveChangesAsync();

            return (true, "Creado correctamente");
        }

        // 🔹 UPDATE
        public async Task<(bool Success, string Message)> UpdateAsync(Propietario propietario)
        {
            var existente = await _context.Propietarios.FindAsync(propietario.Id);

            if (existente == null)
                return (false, "Propietario no encontrado");

            _context.Entry(existente).CurrentValues.SetValues(propietario);
            await _context.SaveChangesAsync();

            return (true, "Actualizado correctamente");
        }

        // 🔹 DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var propietario = await _context.Propietarios.FindAsync(id);

            if (propietario == null)
                return false;

            _context.Propietarios.Remove(propietario);
            await _context.SaveChangesAsync();

            return true;
        }

        // 🔹 VALIDACIONES (ESTO TE FALTABA)

        public async Task<bool> ExisteDocumentoAsync(string documento)
        {
            return await _context.Propietarios
                .AnyAsync(p => p.Documento == documento);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Propietarios
                .AnyAsync(p => p.Email == email);
        }
    }
}