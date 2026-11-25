
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoFix.Data;
using MotoFix.Models;

namespace MotoFix.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Authorize(Policy = "Roles.Gestionar")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MotoFixContext _context;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            MotoFixContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Roles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles
                .OrderBy(role => role.Name)
                .ToListAsync();

            return View(roles);
        }

        // GET: Roles/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateRoleViewModel();

            await LoadPermissionsAsync(model);

            return View(model);
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadPermissionsAsync(model);
                return View(model);
            }

            if (await _roleManager.RoleExistsAsync(model.Nombre))
            {
                ModelState.AddModelError(
                    nameof(model.Nombre),
                    "Este rol ya existe.");

                await LoadPermissionsAsync(model);
                return View(model);
            }

            var selectedIds = model.PermisosSeleccionados
                .Distinct()
                .ToList();

            var validCount = await _context.Permisos
                .CountAsync(p => selectedIds.Contains(p.Id));

            if (validCount != selectedIds.Count)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Uno o más permisos seleccionados no son válidos.");

                await LoadPermissionsAsync(model);
                return View(model);
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var role = new IdentityRole(model.Nombre);

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty, error.Description);
                }

                await LoadPermissionsAsync(model);
                return View(model);
            }

            foreach (var permisoId in selectedIds)
            {
                _context.RolPermisos.Add(new RolPermiso
                {
                    RolId = role.Id,
                    PermisoId = permisoId
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "Rol creado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Roles/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == (await _roleManager.FindByNameAsync("Administrador"))?.Id)
            {
                TempData["Error"] = "No se puede editar el rol Administrador.";
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Nombre = role.Name ?? string.Empty,

                PermisosSeleccionados = await _context.RolPermisos
                    .Where(rp => rp.RolId == role.Id)
                    .Select(rp => rp.PermisoId)
                    .ToListAsync()
            };

            await LoadPermissionsAsync(model);

            return View(model);
        }

        // POST: Roles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var administrador = await _roleManager.FindByNameAsync("Administrador");

            if (administrador != null && model.Id == administrador.Id)
            {
                TempData["Error"] = "No se puede modificar el rol Administrador.";
                return RedirectToAction(nameof(Index));
            }
            
            if (!ModelState.IsValid)
            {
                await LoadPermissionsAsync(model);
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                return NotFound();
            }

            var existingRole = await _roleManager.FindByNameAsync(
                model.Nombre);

            if (existingRole != null && existingRole.Id != role.Id)
            {
                ModelState.AddModelError(
                    nameof(model.Nombre),
                    "Ya existe otro rol con ese nombre.");

                await LoadPermissionsAsync(model);
                return View(model);
            }

            var selectedIds = model.PermisosSeleccionados
                .Distinct()
                .ToList();

            var validCount = await _context.Permisos
                .CountAsync(p => selectedIds.Contains(p.Id));

            if (validCount != selectedIds.Count)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Uno o más permisos seleccionados no son válidos.");

                await LoadPermissionsAsync(model);
                return View(model);
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            await _roleManager.SetRoleNameAsync(role, model.Nombre);

            var updateResult = await _roleManager.UpdateAsync(role);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty, error.Description);
                }

                await LoadPermissionsAsync(model);
                return View(model);
            }

            var currentPermissions = await _context.RolPermisos
                .Where(rp => rp.RolId == role.Id)
                .ToListAsync();

            _context.RolPermisos.RemoveRange(currentPermissions);

            foreach (var permisoId in selectedIds)
            {
                _context.RolPermisos.Add(new RolPermiso
                {
                    RolId = role.Id,
                    PermisoId = permisoId
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "Rol actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // Cargar permisos disponibles para crear un rol.
        private async Task LoadPermissionsAsync(CreateRoleViewModel model)
        {
            model.PermisosDisponibles = await _context.Permisos
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        // Cargar permisos disponibles para editar un rol.
        private async Task LoadPermissionsAsync(EditRoleViewModel model)
        {
            model.PermisosDisponibles = await _context.Permisos
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
    }
}
