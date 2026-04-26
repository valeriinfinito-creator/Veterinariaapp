using VeterinariaApp.Models;

namespace VeterinariaApp.Services.Interfaces
{
    public interface IVeterinarioService
    {
        // 🔹 CRUD
        Task<List<Veterinario>> GetAllAsync();
        Task<Veterinario> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Veterinario veterinario);
        Task<(bool Success, string Message)> UpdateAsync(Veterinario veterinario);
        Task<bool> DeleteAsync(int id);

        // 🔹 Validaciones de negocio
        Task<bool> ExisteVeterinarioAsync(string nombre, string especialidad);

        // 🔹 Disponibilidad (se usa en citas)
        Task<bool> EstaDisponibleAsync(int veterinarioId, DateTime fecha, DateTime horaInicio, DateTime horaFin);
    }
}