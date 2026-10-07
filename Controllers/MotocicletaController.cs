using Microsoft.AspNetCore.Mvc;
using MotoFix.Models;
using MotoFix.Services;

namespace MotoFix.Controllers
{
    public class MotocicletaController : Controller
    {
        private readonly IMotocicletaService _motocicletaService;
        private readonly IClienteService _clienteService;

        public MotocicletaController(
            IMotocicletaService motocicletaService,
            IClienteService clienteService)
        {
            _motocicletaService = motocicletaService;
            _clienteService = clienteService;
        }
        // GET: Motocicleta
        public async Task<IActionResult> Index()
        {
            var motocicletas = await _motocicletaService.ObtenerTodasAsync();
            return View(motocicletas);
        }

        // GET: Motocicleta/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var motocicleta = await _motocicletaService.ObtenerPorIdAsync(id);

            if (motocicleta == null)
            {
                return NotFound();
            }

            return View(motocicleta);
        }

                // GET: Motocicleta/Create
            // GET: Motocicleta/Create
        public async Task<IActionResult> Create()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();

            ViewBag.Clientes = clientes;

            return View();
        }

        // POST: Motocicleta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Motocicleta motocicleta)
        {
            if (!ModelState.IsValid)
            {
                var clientes = await _clienteService.ObtenerTodosAsync();
                ViewBag.Clientes = clientes;

                return View(motocicleta);
            }

            await _motocicletaService.CrearAsync(motocicleta);

            return RedirectToAction(nameof(Index));
        }

        // GET: Motocicleta/Edit/5
// GET: Motocicleta/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var motocicleta = await _motocicletaService.ObtenerPorIdAsync(id);

            if (motocicleta == null)
            {
                return NotFound();
            }

            var clientes = await _clienteService.ObtenerTodosAsync();
            ViewBag.Clientes = clientes;

            return View(motocicleta);
        }

        // POST: Motocicleta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Motocicleta motocicleta)
        {
            if (id != motocicleta.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var clientes = await _clienteService.ObtenerTodosAsync();
                ViewBag.Clientes = clientes;

                return View(motocicleta);
            }

            await _motocicletaService.ActualizarAsync(motocicleta);

            return RedirectToAction(nameof(Index));
        }
        // GET: Motocicleta/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var motocicleta = await _motocicletaService.ObtenerPorIdAsync(id);

            if (motocicleta == null)
            {
                return NotFound();
            }

            return View(motocicleta);
        }

        // POST: Motocicleta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _motocicletaService.EliminarAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}