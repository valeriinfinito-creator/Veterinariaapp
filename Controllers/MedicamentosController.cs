using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinariaApp.Models;
using VeterinariaApp.Services.Interfaces;

namespace VeterinariaApp.Controllers
{
    public class MedicamentosController : Controller
    {
        private readonly IMedicamentoService _medicamentoService;
        private readonly ITratamientoService _tratamientoService;

        public MedicamentosController(
            IMedicamentoService medicamentoService,
            ITratamientoService tratamientoService)
        {
            _medicamentoService = medicamentoService;
            _tratamientoService = tratamientoService;
        }

        // 🔹 LISTAR
        public async Task<IActionResult> Index()
        {
            var medicamentos = await _medicamentoService.GetAllAsync();
            return View(medicamentos);
        }

        // 🔹 DETALLE
        public async Task<IActionResult> Details(int id)
        {
            var medicamento = await _medicamentoService.GetByIdAsync(id);
            if (medicamento == null)
                return NotFound();

            return View(medicamento);
        }

        // 🔹 CREAR (GET)
        public async Task<IActionResult> Create()
        {
            await CargarTratamientos();
            return View();
        }

        // 🔹 CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicamento medicamento)
        {
            if (!ModelState.IsValid)
            {
                await CargarTratamientos(medicamento.TratamientoId);
                return View(medicamento);
            }

            var result = await _medicamentoService.CreateAsync(medicamento);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarTratamientos(medicamento.TratamientoId);
                return View(medicamento);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 EDITAR (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var medicamento = await _medicamentoService.GetByIdAsync(id);
            if (medicamento == null)
                return NotFound();

            await CargarTratamientos(medicamento.TratamientoId);
            return View(medicamento);
        }

        // 🔹 EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medicamento medicamento)
        {
            if (id != medicamento.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarTratamientos(medicamento.TratamientoId);
                return View(medicamento);
            }

            var result = await _medicamentoService.UpdateAsync(medicamento);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                await CargarTratamientos(medicamento.TratamientoId);
                return View(medicamento);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var medicamento = await _medicamentoService.GetByIdAsync(id);
            if (medicamento == null)
                return NotFound();

            return View(medicamento);
        }

        // 🔹 ELIMINAR (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicamentoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 🔧 MÉTODO PRIVADO (CLAVE 🔥)
        private async Task CargarTratamientos(int? tratamientoSeleccionado = null)
        {
            var tratamientos = await _tratamientoService.GetAllAsync();

            ViewBag.Tratamientos = new SelectList(
                tratamientos,
                "Id",
                "Diagnostico",
                tratamientoSeleccionado
            );
        }
    }
}