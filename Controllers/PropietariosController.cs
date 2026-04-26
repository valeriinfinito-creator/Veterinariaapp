using Microsoft.AspNetCore.Mvc;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IPropietarioService _service;

        public PropietariosController(IPropietarioService service)
        {
            _service = service;
        }

        // 🔹 LISTAR
        public async Task<IActionResult> Index()
        {
            var propietarios = await _service.GetAllAsync();
            return View(propietarios);
        }

        // 🔹 DETALLE
        public async Task<IActionResult> Details(int id)
        {
            var propietario = await _service.GetByIdAsync(id);

            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // 🔹 CREAR (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 🔹 CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Propietario propietario)
        {
            if (!ModelState.IsValid)
                return View(propietario);

            var result = await _service.CreateAsync(propietario);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(propietario);
            }

            TempData["Success"] = "Propietario creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 EDITAR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var propietario = await _service.GetByIdAsync(id);

            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // 🔹 EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Propietario propietario)
        {
            if (id != propietario.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(propietario);

            var result = await _service.UpdateAsync(propietario);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(propietario);
            }

            TempData["Success"] = "Propietario actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // 🔹 ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var propietario = await _service.GetByIdAsync(id);

            if (propietario == null)
                return NotFound();

            return View(propietario);
        }

        // 🔹 ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
            {
                TempData["Error"] = "No se pudo eliminar el propietario";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Propietario eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}