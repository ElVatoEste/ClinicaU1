using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Clinica.ViewModels;

namespace Clinica.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Lista de tarjetas configuradas para renderización dinámica
            var cards = new List<Card>
            {
                new Card
                {
                    Title = "Gestión de Usuarios",
                    Description = "Administrar usuarios y roles del sistema.",
                    Icon = "fas fa-users",
                    Link = "/Usuarios/Index",
                    Roles = new List<string> { "Empleado" }
                },
                new Card
                {
                    Title = "Citas Médicas",
                    Description = "Gestión y seguimiento de citas.",
                    Icon = "fas fa-calendar-check",
                    Link = "/Citas/Index",
                    Roles = new List<string> { "Usuario", "Empleado" }
                },
                new Card
                {
                    Title = "Reportes",
                    Description = "Ver reportes e indicadores del sistema.",
                    Icon = "fas fa-chart-line",
                    Link = "/Reportes/Index",
                    Roles = new List<string> { "Empleado" }
                },
                new Card
                {
                    Title = "Gestión de Pacientes",
                    Description = "Registrar y buscar pacientes en el sistema.",
                    Icon = "fas fa-procedures",
                    Link = "/Pacientes/Index",
                    Roles = new List<string> { "Empleado" }
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
