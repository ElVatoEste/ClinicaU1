using Clinica.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Clinica.Common
{
    public class Initializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public Initializer(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task InitializeAsync()
        {
            Console.WriteLine($"[{DateTime.Now}] === INICIO DE LA CONFIGURACIÓN INICIAL ===");

            await InitializeRolesAsync();
            await InitializeUsuariosAsync();

            Console.WriteLine($"[{DateTime.Now}] === CONFIGURACIÓN INICIAL COMPLETADA ===");
        }

        private async Task InitializeRolesAsync()
        {
            Console.WriteLine($"[{DateTime.Now}] Inicializando roles...");

            await EnsureRoleExistsAsync("Admin");
            await EnsureRoleExistsAsync("Empleado");
            await EnsureRoleExistsAsync("Usuario");

            Console.WriteLine($"[{DateTime.Now}] Roles inicializados.\n");
        }

        private async Task InitializeUsuariosAsync()
        {
            Console.WriteLine($"[{DateTime.Now}] Inicializando usuarios...");

            await EnsureUsuarioExistsAsync("admin", "Admin123*", "Admin");
            await EnsureUsuarioExistsAsync("empleado", "Empleado123*", "Empleado");
            await EnsureUsuarioExistsAsync("usuario", "Usuario123*", "Usuario");

            Console.WriteLine($"[{DateTime.Now}] Usuarios inicializados.\n");
        }

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    Console.WriteLine($"[{DateTime.Now}] Rol '{roleName}' creado con éxito.");
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now}] Error al crear el rol '{roleName}': {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now}] El rol '{roleName}' ya existe.");
            }
        }

        private async Task EnsureUsuarioExistsAsync(string username, string password, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = $"{username}@example.com",
                    EmailConfirmed = true 
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    Console.WriteLine($"[{DateTime.Now}] Usuario '{username}' creado con éxito.");
                    await _userManager.AddToRoleAsync(user, roleName);
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now}] Error al crear el usuario '{username}': {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now}] El usuario '{username}' ya existe.");
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                    Console.WriteLine($"[{DateTime.Now}] Rol '{roleName}' asignado al usuario '{username}'.");
                }
            }
        }
    }
}
