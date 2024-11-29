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
    [Authorize(Roles = "Empleado, Admin")]
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

        [HttpGet]
        public async Task<IActionResult> DetallesDiagnostico(int consultaId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DiagnosticoID", consultaId)
            };

            DiagnosticoViewModel diagnostico;

            try
            {
                diagnostico = await _helper.ExecSingleRow<DiagnosticoViewModel>("Obtener_Diagnostico", parameters);

                if (diagnostico == null)
                {
                    return NotFound(new { message = "El diagnóstico no existe." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el diagnóstico: {ex.Message}" });
            }

            return Json(diagnostico);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSignosVitales(int id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@SignosVitalesID", id)
            };

            SignosVitalesViewModel signosVitales;

            try
            {
                signosVitales = await _helper.ExecSingleRow<SignosVitalesViewModel>("Obtener_SignosVitales", parameters);

                if (signosVitales == null)
                {
                    return NotFound(new { message = "Los signos vitales no existen." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener los signos vitales: {ex.Message}" });
            }

            return Json(signosVitales);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarDiagnostico(int consultaId, int expedienteId, string descripcion)
        {
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ConsultaMedicaID", consultaId),
                    new SqlParameter("@Descripcion", descripcion)
                };

            try
            {
                await _helper.ExecNonQuery("Registrar_Diagnostico", parameters);
                TempData["Success"] = "Diagnóstico registrado y asociado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar el diagnóstico: {ex.Message}";
            }

            return RedirectToAction("ListarConsultas", new { expedienteId });
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
