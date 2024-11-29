using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Clinica.ViewModels;

namespace Clinica.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                // Si no está autenticado, redirigir al login
                return RedirectToAction("Login", "Account");
            }

            // Lista de tarjetas configuradas para renderización dinámica
            var cards = new List<Card>
            {
                new Card
                {
                    Title = "Gestión de Usuarios",
                    Description = "Administrar usuarios y roles del sistema.",
                    Icon = "fas fa-users",
                    Link = "/Security/Index",
                    Roles = new List<string> { "Admin" }
                },
                new Card
                {
                    Title = "Gestión de Pacientes",
                    Description = "Registrar y buscar pacientes en el sistema.",
                    Icon = "fas fa-procedures",
                    Link = "/Pacientes/Index",
                    Roles = new List<string> { "Empleado", "Admin" }
                },
                new Card
                {
                    Title = "Gestión de Medicamentos",
                    Description = "Registrar y administrar medicamentos.",
                    Icon = "fas fa-capsules",
                    Link = "/Medicamentos/Index",
                    Roles = new List<string> { "Admin" }
                },
                new Card
                {
                    Title = "Gestión de Recetas",
                    Description = "Buscar historial de recetas medicas",
                    Icon = "fas fa-microscope",
                    Link = "/Facturas/ListarFacturas",
                    Roles = new List<string> { "Admin" }
                }
            };

            // Obtener los roles del usuario autenticado
            var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            // Filtrar tarjetas basadas en los roles del usuario
            var filteredCards = cards.Where(card => card.Roles.Any(role => userRoles.Contains(role))).ToList();

            // Renderizar las tarjetas autorizadas
            return View(filteredCards);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
