using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinica.ViewModels;
using Clinica.Common;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Clinica.Controllers
{
    [Authorize(Roles = "Empleado, Admin")]
    public class RecetasController : Controller
    {
        private readonly StoredProcedureHelper _helper;

        public RecetasController(StoredProcedureHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        public async Task<IActionResult> VerReceta(int diagnosticoId)
        {
            var receta = new RecetaViewModel();

            try
            {
                // Obtener la receta y detalles en una sola consulta
                        var recetaParams = new List<SqlParameter>
                {
                    new SqlParameter("@DiagnosticoID", diagnosticoId)
                };

                var detallesReceta = await _helper.ExecList<DetalleRecetaViewModel>("Obtener_Receta", recetaParams);

                if (!detallesReceta.Any())
                {
                    TempData["Info"] = "No se encontró una receta asociada a este diagnóstico. Por favor, cree una nueva receta.";
                    return RedirectToAction("CrearReceta", new { diagnosticoId });
                }

                // Asignar la información principal de la receta
                var recetaDatos = detallesReceta.First(); // Los datos de la receta serán iguales para todos los registros
                receta.RecetaID = recetaDatos.RecetaID;
                receta.DiagnosticoID = diagnosticoId;
                receta.Fecha = recetaDatos.Fecha;

                // Obtener el ExpedienteID asociado al DiagnosticoID
                var expedienteParams = new List<SqlParameter>
                {
                    new SqlParameter("@DiagnosticoID", diagnosticoId)
                };

                receta.ExpedienteID = await _helper.ExecScalar<int>("Obtener_ExpedientePorDiagnostico", expedienteParams);

                // Asignar los detalles de los medicamentos
                receta.DetallesReceta = detallesReceta.Select(detalle => new DetalleRecetaViewModel
                {
                    MedicamentoID = detalle.MedicamentoID,
                    Nombre = detalle.Nombre,
                    Descripcion = detalle.Descripcion,
                    Cantidad = detalle.Cantidad,
                    PrecioBase = detalle.PrecioBase
                }).ToList();

                // Obtener la lista de medicamentos disponibles para agregar
                receta.MedicamentosDisponibles = await _helper.ExecList<MedicamentoViewModel>("Obtener_Medicamentos", null);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener la receta: {ex.Message}";
                return RedirectToAction("CrearReceta", new { diagnosticoId });
            }
            return View(receta);
        }


        [HttpGet]
        public IActionResult CrearReceta(int diagnosticoId)
        {
            var model = new CrearRecetaViewModel
            {
                DiagnosticoID = diagnosticoId,
                Fecha = DateTime.Now // Fecha actual por defecto
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearReceta(CrearRecetaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos.";
                return RedirectToAction("CrearReceta", new { diagnosticoId = model.DiagnosticoID });
            }

            var recetaParams = new List<SqlParameter>
            {
                new SqlParameter("@DiagnosticoID", model.DiagnosticoID),
                new SqlParameter("@Fecha", model.Fecha)
            };

            try
            {
                await _helper.ExecNonQuery("Crear_Receta", recetaParams);

                TempData["Success"] = "Receta creada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear la receta: {ex.Message}";
            }

            return RedirectToAction("VerReceta", new { diagnosticoId = model.DiagnosticoID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarMedicamento(int recetaId, int medicamentoId)
        {
            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@RecetaID", recetaId),
        new SqlParameter("@MedicamentoID", medicamentoId)
    };

            try
            {
                await _helper.ExecNonQuery("Eliminar_Medicamento_Receta", parameters);
                TempData["Success"] = "Medicamento eliminado correctamente de la receta.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el medicamento: {ex.Message}";
            }

            return RedirectToAction("VerReceta", new { diagnosticoId = recetaId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarMedicamento(AgregarMedicamentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos.";
                return RedirectToAction("VerReceta", new { diagnosticoId = model.DiagnosticoID });
            }

            var medParams = new List<SqlParameter>
            {
                new SqlParameter("@RecetaID", model.RecetaID),
                new SqlParameter("@MedicamentoID", model.MedicamentoID),
                new SqlParameter("@Cantidad", model.Cantidad)
            };

            try
            {
                await _helper.ExecNonQuery("Agregar_Medicamento", medParams);
                TempData["Success"] = "Medicamento agregado exitosamente a la receta.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al agregar el medicamento: {ex.Message}";
            }

            return RedirectToAction("VerReceta", new { diagnosticoId = model.DiagnosticoID });
        }
    }
}
