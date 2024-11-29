using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class FacturaViewModel
    {
        public int FacturaID { get; set; }
        public int PacienteID { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal Total { get; set; }

        // Lista consolidada de detalles
        public List<DetalleFacturaConsolidadoViewModel> Detalles { get; set; } = new List<DetalleFacturaConsolidadoViewModel>();
    }

    public class DetalleFacturaConsolidadoViewModel
    {
        public int FacturaID { get; set; }
        public string DetalleTipo { get; set; } // Examen o Medicamento
        public int DetalleID { get; set; } // Puede ser DetalleID o MedicamentoID
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class DetalleFacturaExamenViewModel
    {
        public int PacienteID { get; set; }
        public int FacturaID { get; set; }
        public int DetalleID { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }

    public class DetalleFacturaMedicamentoViewModel
    {
        public int PacienteID { get; set; }
        public int FacturaID { get; set; }
        public int MedicamentoID { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
    public class FacturaBusquedaViewModel
    {
        public int? FacturaID { get; set; }
        public int? PacienteID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
