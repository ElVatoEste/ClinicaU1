using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinica.ViewModels;
using Clinica.Common;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Clinica.Controllers
{
    [Authorize(Roles = "Empleado")]
    public class ConsultasMedicasController : Controller
    {
        private readonly StoredProcedureHelper _helper;

        public ConsultasMedicasController(StoredProcedureHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        public async Task<IActionResult> ListarConsultas(int expedienteId)
        {
            var viewModel = new ConsultasMedicasListViewModel
            {
                ExpedienteID = expedienteId,
                Consultas = new List<ConsultaMedicaViewModel>()
            };

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ExpedienteID", expedienteId)
            };

            try
            {
                var resultados = await _helper.ExecList<ConsultaMedicaViewModel>("Obtener_ConsultasMedicas", parameters);
                viewModel.Consultas = resultados;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener las consultas médicas: {ex.Message}");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearConsultaMedica(CrearConsultaMedicaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor, complete todos los campos requeridos.";
                return RedirectToAction("ListarConsultas", new { expedienteId = model.ExpedienteID });
            }

            var consultaMedicaId = 0; // Para capturar el ID generado por el procedimiento almacenado

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ExpedienteID", model.ExpedienteID),
                new SqlParameter("@SignosVitalesFecha", model.FechaSignosVitales),
                new SqlParameter("@Presion", model.Presion),
                new SqlParameter("@Pulso", model.Pulso),
                new SqlParameter("@Temperatura", model.Temperatura),
                new SqlParameter("@TipoConsulta", model.TipoConsulta),
                new SqlParameter("@EvaluacionMedica", model.EvaluacionMedica),
                new SqlParameter("@ConsultaMedicaID", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Llamar al procedimiento almacenado
                await _helper.ExecNonQuery("Crear_ConsultaMedica", parameters);

                // Obtener el ID generado
                consultaMedicaId = (int)parameters[^1].Value;

                TempData["Message"] = $"Consulta médica creada con éxito. ID: {consultaMedicaId}";
                return RedirectToAction("ListarConsultas", new { expedienteId = model.ExpedienteID });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear la consulta médica: {ex.Message}";
                return RedirectToAction("ListarConsultas", new { expedienteId = model.ExpedienteID });
            }
        }
    }
}
