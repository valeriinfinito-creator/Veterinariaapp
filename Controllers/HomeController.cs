using Microsoft.AspNetCore.Mvc;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReporteService _reporteService;

        public HomeController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        // 🔹 DASHBOARD PRINCIPAL
        public async Task<IActionResult> Index()
        {
            var dashboard = await _reporteService.GetDashboardAsync();
            return View(dashboard);
        }

        // 🔹 PRIVACY (opcional)
        public IActionResult Privacy()
        {
            return View();
        }

        // 🔹 ERROR
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}