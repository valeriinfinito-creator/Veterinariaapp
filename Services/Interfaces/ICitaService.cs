using VeterinariaApp.Models;

namespace VeterinariaApp.Services.Interfaces
{
    public interface ICitaService
    {
        Task<(bool Success, string Message)> CreateAsync(Cita cita);
        Task<List<Cita>> GetAllAsync();
        Task<Cita> GetByIdAsync(int id);
        Task<(bool Success, string Message)> UpdateAsync(Cita cita);
        Task<bool> DeleteAsync(int id);
        Task<bool> CambiarEstadoAsync(int citaId, string estado);
        Task<List<Mascota>> GetMascotasAsync();
    }
}