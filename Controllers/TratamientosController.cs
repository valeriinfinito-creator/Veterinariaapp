using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class TratamientosController : Controller
    {
        private readonly ITratamientoService _tratamientoService;
        private readonly ICitaService _citaService;

        public TratamientosController(
            ITratamientoService tratamientoService,
            ICitaService citaService)
        {
            _tratamientoService = tratamientoService;
            _citaService = citaService;
        }

        // 🔹 LISTAR
        public async Task<IActionResult> Index()
        {
            var tratamientos = await _tratamientoService.GetAllAsync();
            return View(tratamientos);
        }

        // 🔹 DETALLE
        public async Task<IActionResult> Details(int id)
        {
            var tratamiento = await _tratamientoService.GetByIdAsync(id);
            if (tratamiento == null)
                return NotFound();

            return View(tratamiento);
        }

        // 🔹 CREAR (GET)
        public async Task<IActionResult> Create()
        {
            await CargarCitas();
            return View();
        }

        // 🔹 CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tratamiento tratamiento)
        {
            if (!ModelState.IsValid)
            {
                await CargarCitas(tratamiento.CitaId);
                return View(tratamiento);
            }

            var result = await _tratamientoService.CreateAsync(tratamiento);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarCitas(tratamiento.CitaId);
                return View(tratamiento);
            }

            TempData["Success"] = "Tratamiento registrado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 EDITAR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var tratamiento = await _tratamientoService.GetByIdAsync(id);
            if (tratamiento == null)
                return NotFound();

            await CargarCitas(tratamiento.CitaId);
            return View(tratamiento);
        }

        // 🔹 EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tratamiento tratamiento)
        {
            if (id != tratamiento.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarCitas(tratamiento.CitaId);
                return View(tratamiento);
            }

            var result = await _tratamientoService.UpdateAsync(tratamiento);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarCitas(tratamiento.CitaId);
                return View(tratamiento);
            }

            TempData["Success"] = "Tratamiento actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var tratamiento = await _tratamientoService.GetByIdAsync(id);
            if (tratamiento == null)
                return NotFound();

            return View(tratamiento);
        }

        // 🔹 ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eliminado = await _tratamientoService.DeleteAsync(id);

            if (!eliminado)
            {
                TempData["Error"] = "No se pudo eliminar el tratamiento";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Tratamiento eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔧 MÉTODO PRIVADO (CLAVE 🔥)
        private async Task CargarCitas(int? citaSeleccionada = null)
        {
            var citas = await _citaService.GetAllAsync();

            ViewBag.Citas = new SelectList(
                citas,
                "Id",
                "Id", // puedes mejorar esto luego (ej: Mascota + Fecha)
                citaSeleccionada
            );
        }
    }
}