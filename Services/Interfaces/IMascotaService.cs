using VeterinariaApp.Models;

namespace VeterinariaApp.Services.Interfaces
{
    public interface IMascotaService
    {
        Task<List<Mascota>> GetAllAsync();
        Task<Mascota> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Mascota mascota);
        Task<(bool Success, string Message)> UpdateAsync(Mascota mascota);
        Task<bool> DeleteAsync(int id);
    }
}