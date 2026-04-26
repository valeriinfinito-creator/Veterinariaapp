using VeterinariaApp.Models;

namespace VeterinariaApp.Services.Interfaces
{
    public interface ITratamientoService
    {
        Task<List<Tratamiento>> GetAllAsync();
        Task<Tratamiento> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Tratamiento tratamiento);
        Task<bool> DeleteAsync(int id);
        Task<(bool Success, string Message)> UpdateAsync(Tratamiento tratamiento);
    }
}