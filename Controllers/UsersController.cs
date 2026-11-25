
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoFix.Models;

namespace MotoFix.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Authorize(Policy = "Usuarios.Gestionar")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: Users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Select(user => new UserListViewModel
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Email = user.Email ?? string.Empty
                })
                .ToListAsync();

            return View(users);
        }

        
        // GET: Users/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateUserViewModel();

            await LoadRolesAsync(model);

            return View(model);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadRolesAsync(model);
                return View(model);
            }

            // Comprobar que el correo no esté registrado.
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "Este correo ya está registrado.");

                await LoadRolesAsync(model);
                return View(model);
            }

            // Comprobar que el rol seleccionado exista.
            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError(
                    nameof(model.RoleName),
                    "El rol seleccionado no existe.");

                await LoadRolesAsync(model);
                return View(model);
            }

            // Crear la cuenta.
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await LoadRolesAsync(model);
                return View(model);
            }

            // Asignar el rol seleccionado.
            var roleResult = await _userManager.AddToRoleAsync(
                user, model.RoleName);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await LoadRolesAsync(model);
                return View(model);
            }

            TempData["SuccessMessage"] = "Usuario creado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        
        // GET: Users/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email ?? string.Empty,
                RoleName = userRoles.FirstOrDefault() ?? string.Empty
            };

            model.Roles = await _roleManager.Roles
                .Select(role => role.Name!)
                .ToListAsync();

            return View(model);
        }

        // POST: Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await _roleManager.Roles
                    .Select(role => role.Name!)
                    .ToListAsync();

                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            // Comprobar que el correo no pertenezca a otro usuario.
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "Este correo ya está registrado.");

                model.Roles = await _roleManager.Roles
                    .Select(role => role.Name!)
                    .ToListAsync();

                return View(model);
            }

            // Comprobar que el rol exista.
            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError(
                    nameof(model.RoleName),
                    "El rol seleccionado no existe.");

                model.Roles = await _roleManager.Roles
                    .Select(role => role.Name!)
                    .ToListAsync();

                return View(model);
            }

            user.Nombre = model.Nombre;
            user.Apellido = model.Apellido;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                model.Roles = await _roleManager.Roles
                    .Select(role => role.Name!)
                    .ToListAsync();

                return View(model);
            }

            // Consultar y actualizar los roles del usuario.
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(
                    user, currentRoles);

                if (!removeResult.Succeeded)
                {
                    return BadRequest("No se pudieron actualizar los roles.");
                }
            }

            var addResult = await _userManager.AddToRoleAsync(
                user, model.RoleName);

            if (!addResult.Succeeded)
            {
                return BadRequest("No se pudo asignar el nuevo rol.");
            }

            TempData["SuccessMessage"] = "Usuario actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }


        // Cargar los roles disponibles en el formulario.
        private async Task LoadRolesAsync(CreateUserViewModel model)
        {
            model.Roles = await _roleManager.Roles
                .Select(role => role.Name!)
                .ToListAsync();
        }
    }
}
