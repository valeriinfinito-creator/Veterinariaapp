using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;
using VeterinariaApp.ViewModels;

namespace VeterinariaApp.Controllers
{
    public class CitasController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IVeterinarioService _veterinarioService;
        private readonly IMascotaService _mascotaService;

        public CitasController(
            ICitaService citaService,
            IVeterinarioService veterinarioService,
            IMascotaService mascotaService)
        {
            _citaService = citaService;
            _veterinarioService = veterinarioService;
            _mascotaService = mascotaService;
        }

        // 🔹 LISTAR
        public async Task<IActionResult> Index()
        {
            var citas = await _citaService.GetAllAsync();
            return View(citas);
        }

        // 🔹 DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var cita = await _citaService.GetByIdAsync(id);
            if (cita == null) return NotFound();

            return View(cita);
        }

        // 🔹 CREATE GET
        public async Task<IActionResult> Create()
        {
            var model = new CitaViewModel();

            await CargarCombos(model);

            return View(model);
        }

        // 🔹 CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(model);
                return View(model);
            }

            var cita = new Cita
            {
                MascotaId = model.MascotaId,
                VeterinarioId = model.VeterinarioId,
                Fecha = model.Fecha,
                HoraInicio = model.HoraInicio,
                HoraFin = model.HoraFin,
                Estado = "Programada"
            };

            var result = await _citaService.CreateAsync(cita);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarCombos(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var cita = await _citaService.GetByIdAsync(id);
            if (cita == null) return NotFound();

            var model = new CitaViewModel
            {
                Id = cita.Id,
                MascotaId = cita.MascotaId,
                VeterinarioId = cita.VeterinarioId,
                Fecha = cita.Fecha,
                HoraInicio = cita.HoraInicio,
                HoraFin = cita.HoraFin,
                Estado = cita.Estado
            };

            await CargarCombos(model);

            return View(model);
        }

        // 🔹 EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CitaViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarCombos(model);
                return View(model);
            }

            var cita = new Cita
            {
                Id = model.Id,
                MascotaId = model.MascotaId,
                VeterinarioId = model.VeterinarioId,
                Fecha = model.Fecha,
                HoraInicio = model.HoraInicio,
                HoraFin = model.HoraFin,
                Estado = model.Estado
            };

            var result = await _citaService.UpdateAsync(cita);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarCombos(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _citaService.GetByIdAsync(id);
            if (cita == null) return NotFound();

            return View(cita);
        }

        // 🔹 DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _citaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 🔹 CAMBIAR ESTADO
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            await _citaService.CambiarEstadoAsync(id, estado);
            return RedirectToAction(nameof(Index));
        }

        // 🔧 COMBOS
        private async Task CargarCombos(CitaViewModel model)
        {
            var mascotas = await _mascotaService.GetAllAsync();
            var veterinarios = await _veterinarioService.GetAllAsync();

            model.Mascotas = mascotas
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Nombre
                }).ToList();

            model.Veterinarios = veterinarios
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.Nombre
                }).ToList();
        }
    }
}