using Microsoft.AspNetCore.Mvc;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class VeterinariosController : Controller
    {
        private readonly IVeterinarioService _service;

        public VeterinariosController(IVeterinarioService service)
        {
            _service = service;
        }

        // 🔹 LISTAR
        public async Task<IActionResult> Index()
        {
            var veterinarios = await _service.GetAllAsync();
            return View(veterinarios);
        }

        // 🔹 DETALLE
        public async Task<IActionResult> Details(int id)
        {
            var veterinario = await _service.GetByIdAsync(id);

            if (veterinario == null)
                return NotFound();

            return View(veterinario);
        }

        // 🔹 CREAR (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 🔹 CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Veterinario veterinario)
        {
            if (!ModelState.IsValid)
                return View(veterinario);

            var result = await _service.CreateAsync(veterinario);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(veterinario);
            }

            TempData["Success"] = "Veterinario creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 EDITAR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var veterinario = await _service.GetByIdAsync(id);

            if (veterinario == null)
                return NotFound();

            return View(veterinario);
        }

        // 🔹 EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Veterinario veterinario)
        {
            if (id != veterinario.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(veterinario);

            var result = await _service.UpdateAsync(veterinario);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(veterinario);
            }

            TempData["Success"] = "Veterinario actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var veterinario = await _service.GetByIdAsync(id);

            if (veterinario == null)
                return NotFound();

            return View(veterinario);
        }

        // 🔹 ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
            {
                TempData["Error"] = "No se pudo eliminar el veterinario";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Veterinario eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}