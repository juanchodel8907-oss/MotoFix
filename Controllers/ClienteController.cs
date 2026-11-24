
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MotoFix.Models;
using MotoFix.Services;
using Microsoft.AspNetCore.Authorization;

namespace MotoFix.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: Cliente
        [Authorize(Policy = "Clientes.Ver")]
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: Cliente/Details/5
        [Authorize(Policy = "Clientes.Ver")]
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // GET: Cliente/Create
        [Authorize(Policy = "Clientes.Crear")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [Authorize(Policy = "Clientes.Crear")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteService.CrearAsync(cliente);

            return RedirectToAction(nameof(Index));
        }

        // GET: Cliente/Edit/5
        [Authorize(Policy = "Clientes.Editar")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [Authorize(Policy = "Clientes.Editar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteService.ActualizarAsync(cliente);

            return RedirectToAction(nameof(Index));
        }

        // GET: Cliente/Delete/5
        [Authorize(Policy = "Clientes.Eliminar")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [Authorize(Policy = "Clientes.Eliminar")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _clienteService.EliminarAsync(id);

                TempData["MensajeExito"] = "Cliente eliminado correctamente.";
            }
            catch (DbUpdateException)
            {
                TempData["MensajeError"] =
                    "No se puede eliminar este cliente porque tiene motocicletas asociadas.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
