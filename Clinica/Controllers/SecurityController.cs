using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Clinica.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica.Controllers
{
    [Authorize(Roles = "Empleado")]
    public class SecurityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SecurityController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            var model = new SecurityViewModel
            {
                Users = users.Select(user => new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result.ToList()
                }).ToList(),
                Roles = roles.Select(role => role.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(roles).ToList();

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            TempData["Success"] = "Roles actualizados correctamente.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Error"] = "El nombre del rol no puede estar vacío.";
                return RedirectToAction("Index");
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["Error"] = "El rol ya existe.";
                return RedirectToAction("Index");
            }

            await _roleManager.CreateAsync(new IdentityRole(roleName));
            TempData["Success"] = "Rol creado exitosamente.";
            return RedirectToAction("Index");
        }
    }
}
