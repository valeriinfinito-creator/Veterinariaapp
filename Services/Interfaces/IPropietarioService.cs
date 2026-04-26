using VeterinariaApp.Models;

namespace VeterinariaApp.Services.Interfaces
{
    public interface IPropietarioService
    {
        Task<List<Propietario>> GetAllAsync();
        Task<Propietario> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Propietario propietario);
        Task<(bool Success, string Message)> UpdateAsync(Propietario propietario);
        Task<bool> DeleteAsync(int id);

        Task<bool> ExisteDocumentoAsync(string documento);
        Task<bool> ExisteEmailAsync(string email);
    }
}