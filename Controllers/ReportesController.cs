using Microsoft.AspNetCore.Mvc;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public async Task<IActionResult> Index()
        {
            var reportes = await _reporteService.GetReportesAsync();
            return View(reportes);
        }

        public async Task<IActionResult> VeterinarioTop()
        {
            var data = await _reporteService.GetVeterinarioConMasCitasAsync();
            return View(data);
        }

        public async Task<IActionResult> MascotasTop()
        {
            var data = await _reporteService.GetMascotasMasAtendidasAsync();
            return View(data);
        }

        public async Task<IActionResult> MedicamentosTop()
        {
            var data = await _reporteService.GetMedicamentosMasUsadosAsync();
            return View(data);
        }

        public async Task<IActionResult> TasaInasistencia()
        {
            var tasa = await _reporteService.GetTasaInasistenciaAsync();
            return View(tasa);
        }
    }
}