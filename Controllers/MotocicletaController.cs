using Microsoft.AspNetCore.Mvc;
using MotoFix.Models;
using MotoFix.Services;
using Microsoft.AspNetCore.Authorization;

namespace MotoFix.Controllers
{
    [Authorize]
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
        [Authorize(Policy = "Motocicletas.Ver")]
        public async Task<IActionResult> Index()
        {
            var motocicletas = await _motocicletaService.ObtenerTodasAsync();
            return View(motocicletas);
        }

        // GET: Motocicleta/Details/5
        [Authorize(Policy = "Motocicletas.Ver")]
        public async Task<IActionResult> Details(int id)
        {
            var motocicleta = await _motocicletaService.ObtenerPorIdAsync(id);

            if (motocicleta == null)
                return NotFound();

            return View(motocicleta);
        }

                // GET: Motocicleta/Create
        [Authorize(Policy = "Motocicletas.Crear")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();

            ViewBag.Clientes = clientes;

            return View();
        }
        // POST: Motocicleta/Create
        [Authorize(Policy = "Motocicletas.Crear")]
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
        [Authorize(Policy = "Motocicletas.Editar")]
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
        [Authorize(Policy = "Motocicletas.Editar")]
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
        [Authorize(Policy = "Motocicletas.Eliminar")]
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
        [Authorize(Policy = "Motocicletas.Eliminar")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _motocicletaService.EliminarAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}