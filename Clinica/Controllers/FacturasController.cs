using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Clinica.ViewModels;
using Clinica.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Clinica.Controllers
{
    public class FacturasController : Controller
    {
        private readonly StoredProcedureHelper _helper;

        public FacturasController(StoredProcedureHelper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Empleado")]
        public async Task<IActionResult> GenerarFactura(int pacienteId)
        {
            var model = new FacturaViewModel
            {
                PacienteID = pacienteId,
                FechaFactura = DateTime.Now,
                Detalles = new List<DetalleFacturaConsolidadoViewModel>()
            };

            try
            {
                // Verificar si ya existe una factura para este paciente
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@PacienteID", pacienteId)
                };

                var factura = await _helper.ExecSingleRow<FacturaViewModel>("Buscar_Factura_Por_PacienteID", parameters);

                if (factura != null)
                {
                    // Si ya existe la factura, cargar los detalles
                    model = factura;
                    var detallesParams = new List<SqlParameter>
                    {
                        new SqlParameter("@FacturaID", factura.FacturaID)
                    };
                    model.Detalles = await _helper.ExecList<DetalleFacturaConsolidadoViewModel>("Obtener_Detalles_Factura", detallesParams);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar los datos de la factura: {ex.Message}";
            }

            // Cargar exámenes y medicamentos en ViewBag para los modales
            ViewBag.TiposExamen = await _helper.ExecList<TipoExamenViewModel>("Obtener_TipoExamen");
            ViewBag.Medicamentos = await _helper.ExecList<MedicamentoViewModel>("Obtener_Medicamentos");

            return View(model);
        }

        [Authorize(Roles = "Admin, Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearFactura(FacturaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error en los datos enviados.";
                return RedirectToAction("GenerarFactura", new { pacienteId = model.PacienteID });
            }

            try
            {
                // Crear la factura
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@PacienteID", model.PacienteID),
                    new SqlParameter("@FechaFactura", model.FechaFactura),
                    new SqlParameter
                    {
                        ParameterName = "@FacturaID",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    }
                };

                await _helper.ExecNonQuery("Crear_Factura", parameters);
                var facturaId = (int)parameters.Find(p => p.ParameterName == "@FacturaID").Value;

                TempData["Success"] = $"Factura creada con éxito. ID: {facturaId}";
                return RedirectToAction("GenerarFactura", new { pacienteId = model.PacienteID });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear la factura: {ex.Message}";
                return RedirectToAction("GenerarFactura", new { pacienteId = model.PacienteID });
            }
        }

        [Authorize(Roles = "Admin, Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDetalleExamen(DetalleFacturaExamenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos para agregar examen.";
                return RedirectToAction("GenerarFactura", new { pacienteId = model.FacturaID });
            }

            try
            {
                // Crear el detalle en la tabla Detalles
                var detalleParameters = new List<SqlParameter>
            {
                new SqlParameter("@TipoExamenID", model.DetalleID),  // Suponiendo que DetalleID corresponde al tipo de examen
                new SqlParameter("@Descripcion", model.Descripcion),
                new SqlParameter
                {
                    ParameterName = "@DetalleID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                }
            };

                    await _helper.ExecNonQuery("Crear_Detalle", detalleParameters);
                    var detalleId = (int)detalleParameters.Find(p => p.ParameterName == "@DetalleID").Value;

                    // Ahora insertar el detalle en la factura
                    var facturaParameters = new List<SqlParameter>
            {
                new SqlParameter("@FacturaID", model.FacturaID),
                new SqlParameter("@DetalleID", detalleId),
                new SqlParameter("@Cantidad", model.Cantidad),
                new SqlParameter("@PrecioUnitario", model.PrecioUnitario)
            };

                await _helper.ExecNonQuery("Agregar_DetalleFactura", facturaParameters);

                TempData["Success"] = "Examen agregado con éxito.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al agregar examen: {ex.Message}";
            }

            return RedirectToAction("GenerarFactura", new { pacienteId = model.FacturaID });
        }

        [Authorize(Roles = "Admin, Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDetalleMedicamento(DetalleFacturaMedicamentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos para agregar medicamento.";
                return RedirectToAction("GenerarFactura", new { pacienteId = model.FacturaID });
            }

            try
            {
                // Agregar detalle de medicamento
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@FacturaID", model.FacturaID),
                    new SqlParameter("@MedicamentoID", model.MedicamentoID),
                    new SqlParameter("@Cantidad", model.Cantidad),
                    new SqlParameter("@PrecioUnitario", model.PrecioUnitario)
                };

                await _helper.ExecNonQuery("Agregar_MedicamentoFactura", parameters);
                TempData["Success"] = "Medicamento agregado con éxito.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al agregar medicamento: {ex.Message}";
            }

            return RedirectToAction("GenerarFactura", new { pacienteId = model.FacturaID });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ListarFacturas(int? pacienteId = null)
        {
            try
            {
                var parameters = new List<SqlParameter>
            {
                new SqlParameter("@PacienteID", pacienteId ?? (object)DBNull.Value)
            };

                var facturasDataTable = await _helper.ExecDataTable("Obtener_Facturas_Con_Detalles_Y_Medicamentos", parameters);

                if (facturasDataTable.Rows.Count == 0)
                {
                    TempData["Warning"] = "No se encontraron facturas.";
                    return View(new List<FacturaDataTableViewModel>());
                }

                var facturas = MapDataTableToFacturaViewModel(facturasDataTable);
                return View(facturas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al listar facturas: {ex.Message}";
                return View(new List<FacturaDataTableViewModel>());
            }
        }


        public List<FacturaDataTableViewModel> MapDataTableToFacturaViewModel(DataTable facturasDataTable)
        {
            var facturas = new List<FacturaDataTableViewModel>();

            var facturaGroups = facturasDataTable.AsEnumerable().GroupBy(row => row.Field<int>("FacturaID"));

            foreach (var group in facturaGroups)
            {
                var factura = new FacturaDataTableViewModel
                {
                    FacturaID = group.Key,
                    PacienteID = group.First().Field<int>("PacienteID"),
                    PacienteNombre = group.First().Field<string>("PacienteNombre"),
                    PacienteApellido = group.First().Field<string>("PacienteApellido"),
                    FechaFactura = group.First().Field<DateTime>("FechaFactura"),
                    Total = group.First().Field<decimal>("Total"),
                };

                // Agregar detalles
                factura.Detalles = group.Where(row => row["DetalleFacturaID"] != DBNull.Value).Select(row => new DetalleFacturaDataTableViewModel
                {
                    DetalleFacturaID = row.Field<int>("DetalleFacturaID"),
                    DetalleID = row.Field<int>("DetalleID"),
                    Descripcion = row.Field<string>("DetalleDescripcion"),
                    Cantidad = row.Field<int>("DetalleCantidad"),
                    PrecioUnitario = row.Field<decimal>("DetallePrecioUnitario"),
                }).ToList();

                // Agregar medicamentos
                factura.Medicamentos = group.Where(row => row["DetalleMedicamentoFacturaID"] != DBNull.Value).Select(row => new MedicamentoFacturaDataTableViewModel
                {
                    DetalleMedicamentoFacturaID = row.Field<int>("DetalleMedicamentoFacturaID"),
                    MedicamentoID = row.Field<int>("MedicamentoID"),
                    MedicamentoNombre = row.Field<string>("MedicamentoNombre"),
                    Cantidad = row.Field<int>("MedicamentoCantidad"),
                    PrecioUnitario = row.Field<decimal>("MedicamentoPrecioUnitario"),
                }).ToList();

                facturas.Add(factura);
            }

            return facturas;
        }
    }
}
