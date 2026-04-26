using VeterinariaApp.ViewModels;

namespace VeterinariaApp.Services.Interfaces
{
    public interface IReporteService
    {
        Task<object> GetDashboardAsync();

        Task<List<ReporteViewModel>> GetVeterinarioConMasCitasAsync();
        Task<List<ReporteViewModel>> GetMascotasMasAtendidasAsync();
        Task<List<ReporteViewModel>> GetMedicamentosMasUsadosAsync();
        Task<double> GetTasaInasistenciaAsync();
        Task<List<ReporteViewModel>> GetReportesAsync();
    }
}