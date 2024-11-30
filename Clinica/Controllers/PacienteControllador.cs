using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Clinica.Common;
using System.Threading.Tasks;
using Clinica.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Clinica.Controllers
{
    [Authorize]
    public class PacientesController : Controller
    {
        private readonly StoredProcedureHelper _helper;

        public PacientesController(StoredProcedureHelper storedProcedureHelper)
        {
            _helper = storedProcedureHelper;
        }

        [Authorize(Roles = "Empleado, Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new PacienteGestionViewModel();

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Nombre", DBNull.Value ?? (object)DBNull.Value),
                new SqlParameter("@Apellido", DBNull.Value ?? (object)DBNull.Value),
                new SqlParameter("@FechaNacimiento", DBNull.Value ?? (object)DBNull.Value),
                new SqlParameter("@Identificacion", DBNull.Value ?? (object)DBNull.Value)
            };

            try
            {
                var resultados = await _helper.ExecList<PacienteResultado>("Buscar_Expedientes", parameters);
                model.Pacientes = resultados;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al cargar pacientes: {ex.Message}");
            }

            return View(model);
        }

        [Authorize(Roles = "Empleado, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buscar(PacienteGestionViewModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Nombre", (object)model.NombreBusqueda ?? DBNull.Value),
                new SqlParameter("@Apellido", (object)model.ApellidoBusqueda ?? DBNull.Value),
                new SqlParameter("@FechaNacimiento", (object)model.FechaNacimientoBusqueda ?? DBNull.Value),
                new SqlParameter("@Identificacion", (object)model.IdentificacionBusqueda ?? DBNull.Value)
            };

            try
            {
                var resultados = await _helper.ExecList<PacienteResultado>("Buscar_Expedientes", parameters);
                model.Pacientes = resultados;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al buscar pacientes: {ex.Message}");
            }

            return View("Index", model);
        }

        [HttpPost]
        [Authorize(Roles = "Empleado, Admin")]
        public async Task<IActionResult> Registrar(PacienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            // Validar que la fecha esté dentro del rango permitido
            if (model.FechaNacimiento < new DateTime(1753, 1, 1) || model.FechaNacimiento > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento debe estar entre 1/1/1753 y 12/31/9999.");
                return View("Index", model);
            }

            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Nombre", model.Nombre),
                    new SqlParameter("@Apellido", model.Apellido),
                    new SqlParameter("@Identificacion", model.Identificacion),
                    new SqlParameter("@FechaNacimiento", model.FechaNacimiento),
                    new SqlParameter("@Direccion", model.Direccion),
                    new SqlParameter("@Contacto", model.Contacto),
                    new SqlParameter("@Correo", model.Correo),
                    new SqlParameter("@Sexo", model.Sexo),
                    new SqlParameter("@Telefono", model.Telefono),
                    new SqlParameter("@EstadoCivil", model.EstadoCivil),
                    new SqlParameter("@NombreResp", model.NombreResp),
                    new SqlParameter("@ParentescoResp", model.ParentescoResp),
                    new SqlParameter("@ContactoResp", model.ContactoResp)
                };

            try
            {
                var result = await _helper.ExecSingleRow<PacienteResultado>("Registrar_Paciente", parameters);
                TempData["Message"] = $"Paciente registrado con éxito. ID Expediente: {result.ExpedienteID}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al registrar paciente: {ex.Message}");
                return View("Index", model);
            }
        }
    }
}
