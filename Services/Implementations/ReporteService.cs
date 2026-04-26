using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Services.Interfaces;
using VeterinariaApp.ViewModels;

namespace VeterinariaApp.Services.Implementations
{
    public class ReporteService : IReporteService
    {
        private readonly MySqlDBContext _context;

        public ReporteService(MySqlDBContext context)
        {
            _context = context;
        }

        // 🔹 DASHBOARD GENERAL (lo usa HomeController)
        public async Task<object> GetDashboardAsync()
        {
            return new
            {
                VeterinariosTop = await GetVeterinarioConMasCitasAsync(),
                MascotasTop = await GetMascotasMasAtendidasAsync(),
                MedicamentosTop = await GetMedicamentosMasUsadosAsync(),
                TasaInasistencia = await GetTasaInasistenciaAsync()
            };
        }

        // 🔹 REPORTE GENERAL
        public async Task<List<ReporteViewModel>> GetReportesAsync()
        {
            return await _context.Citas
                .Include(c => c.Veterinario)
                .GroupBy(c => c.Veterinario.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();
        }

        // 🔹 VETERINARIOS TOP
        public async Task<List<ReporteViewModel>> GetVeterinarioConMasCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.Veterinario)
                .GroupBy(c => c.Veterinario.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        // 🔹 MASCOTAS TOP
        public async Task<List<ReporteViewModel>> GetMascotasMasAtendidasAsync()
        {
            return await _context.Citas
                .Include(c => c.Mascota)
                .Where(c => c.Estado == "Atendida")
                .GroupBy(c => c.Mascota.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        // 🔹 MEDICAMENTOS TOP
        public async Task<List<ReporteViewModel>> GetMedicamentosMasUsadosAsync()
        {
            return await _context.Medicamentos
                .GroupBy(m => m.Nombre)
                .Select(g => new ReporteViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
        }

        // 🔹 INASISTENCIA
        public async Task<double> GetTasaInasistenciaAsync()
        {
            var total = await _context.Citas.CountAsync();
            if (total == 0) return 0;

            var inasistencias = await _context.Citas
                .CountAsync(c => c.Estado == "No asistió");

            return (double)inasistencias / total * 100;
        }
    }
}