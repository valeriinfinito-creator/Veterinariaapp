using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Services.Implementations
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly MySqlDBContext _context;

        public MedicamentoService(MySqlDBContext context)
        {
            _context = context;
        }

        public async Task<List<Medicamento>> GetAllAsync()
        {
            return await _context.Medicamentos
                .Include(m => m.Tratamiento)
                .ToListAsync();
        }

        public async Task<Medicamento> GetByIdAsync(int id)
        {
            return await _context.Medicamentos
                .Include(m => m.Tratamiento)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Medicamento m)
        {
            _context.Medicamentos.Add(m);
            await _context.SaveChangesAsync();
            return (true, "Medicamento creado");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Medicamento m)
        {
            _context.Medicamentos.Update(m);
            await _context.SaveChangesAsync();
            return (true, "Medicamento actualizado");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await _context.Medicamentos.FindAsync(id);
            if (obj == null) return false;

            _context.Medicamentos.Remove(obj);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}