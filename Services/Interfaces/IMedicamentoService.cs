using VeterinariaApp.Models;

namespace VeterinariaApp.Services.Interfaces
{
    public interface IMedicamentoService
    {
        Task<List<Medicamento>> GetAllAsync();
        Task<Medicamento> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Medicamento medicamento);
        Task<(bool Success, string Message)> UpdateAsync(Medicamento medicamento);
        Task<bool> DeleteAsync(int id);
    }
}