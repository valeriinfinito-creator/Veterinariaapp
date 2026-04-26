using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class MascotasController : Controller
    {
        private readonly IMascotaService _mascotaService;
        private readonly IPropietarioService _propietarioService;

        public MascotasController(
            IMascotaService mascotaService,
            IPropietarioService propietarioService)
        {
            _mascotaService = mascotaService;
            _propietarioService = propietarioService;
        }

        // 🔹 LISTAR
        public async Task<IActionResult> Index()
        {
            var mascotas = await _mascotaService.GetAllAsync();
            return View(mascotas);
        }

        // 🔹 DETALLE
        public async Task<IActionResult> Details(int id)
        {
            var mascota = await _mascotaService.GetByIdAsync(id);
            if (mascota == null)
                return NotFound();

            return View(mascota);
        }

        // 🔹 CREAR (GET)
        public async Task<IActionResult> Create()
        {
            await CargarPropietarios();
            return View();
        }

        // 🔹 CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mascota mascota)
        {
            if (!ModelState.IsValid)
            {
                await CargarPropietarios();
                return View(mascota);
            }

            var result = await _mascotaService.CreateAsync(mascota);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarPropietarios();
                return View(mascota);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 EDITAR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var mascota = await _mascotaService.GetByIdAsync(id);
            if (mascota == null)
                return NotFound();

            await CargarPropietarios();
            return View(mascota);
        }

        // 🔹 EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mascota mascota)
        {
            if (id != mascota.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarPropietarios();
                return View(mascota);
            }

            var result = await _mascotaService.UpdateAsync(mascota);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarPropietarios();
                return View(mascota);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var mascota = await _mascotaService.GetByIdAsync(id);
            if (mascota == null)
                return NotFound();

            return View(mascota);
        }

        // 🔹 ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mascotaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 🔧 MÉTODO PRIVADO (CLAVE 🔥)
        private async Task CargarPropietarios()
        {
            var propietarios = await _propietarioService.GetAllAsync();
            ViewBag.Propietarios = new SelectList(propietarios, "Id", "Nombre");
        }
    }
}