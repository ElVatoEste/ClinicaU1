using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinica.ViewModels;
using Clinica.Common;
using Microsoft.AspNetCore.Authorization;

namespace Clinica.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MedicamentosController : Controller
    {
        private readonly StoredProcedureHelper _helper;

        public MedicamentosController(StoredProcedureHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new MedicamentosViewModel();

            try
            {
                viewModel.Medicamentos = await _helper.ExecList<MedicamentoViewModel>("Obtener_Medicamentos", null);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener la lista de medicamentos: {ex.Message}";
                viewModel.Medicamentos = new List<MedicamentoViewModel>();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(MedicamentosViewModel model)
        {
            if (model.CrearMedicamento.Nombre == null || model.CrearMedicamento.Descripcion == null || model.CrearMedicamento.PrecioBase <= 0 )
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos.";
                return RedirectToAction("Index");
            }

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Nombre", model.CrearMedicamento.Nombre),
                new SqlParameter("@Descripcion", model.CrearMedicamento.Descripcion),
                new SqlParameter("@PrecioBase", model.CrearMedicamento.PrecioBase)
            };

            try
            {
                await _helper.ExecNonQuery("Crear_Medicamento", parameters);
                TempData["Success"] = "Medicamento creado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear el medicamento: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
