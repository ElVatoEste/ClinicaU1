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
    [Authorize(Roles = "Empleado")]
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
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DiagnosticoID", diagnosticoId)
            };

            RecetaViewModel receta;
            try
            {
                receta = await _helper.ExecSingleRow<RecetaViewModel>("Obtener_Receta", parameters);

                if (receta == null)
                {
                    TempData["Info"] = "No se encontró una receta asociada a este diagnóstico. Por favor, cree una nueva receta.";
                    return RedirectToAction("CrearReceta", new { diagnosticoId }); // Redirigir a la acción para crear receta
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener la receta: {ex.Message}";
                return RedirectToAction("CrearReceta", new { diagnosticoId }); // También redirigir en caso de error
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
        public async Task<IActionResult> AgregarMedicamento(AgregarMedicamentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor, complete todos los campos.";
                return RedirectToAction("VerReceta", new { diagnosticoId = model.RecetaID });
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@RecetaID", model.RecetaID),
                new SqlParameter("@MedicamentoID", model.MedicamentoID),
                new SqlParameter("@Cantidad", model.Cantidad)
            };

            try
            {
                await _helper.ExecNonQuery("Agregar_Medicamento", parameters);
                TempData["Success"] = "Medicamento agregado a la receta correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al agregar el medicamento: {ex.Message}";
            }

            return RedirectToAction("VerReceta", new { diagnosticoId = model.RecetaID });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearReceta(CrearRecetaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos.";
                return RedirectToAction("VerReceta", new { diagnosticoId = model.DiagnosticoID });
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DiagnosticoID", model.DiagnosticoID),
                new SqlParameter("@Fecha", model.Fecha)
            };

            try
            {
                await _helper.ExecNonQuery("Crear_Receta", parameters);
                TempData["Success"] = "Receta creada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear la receta: {ex.Message}";
            }

            return RedirectToAction("VerReceta", new { diagnosticoId = model.DiagnosticoID });
        }
    }
}
