using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class MedicamentosViewModel
    {
        public IEnumerable<MedicamentoViewModel> Medicamentos { get; set; }

        public CrearMedicamentoViewModel CrearMedicamento { get; set; } = new CrearMedicamentoViewModel();
    }

    public class MedicamentoViewModel
    {
        public int MedicamentoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
    }

    public class CrearMedicamentoViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio base debe ser mayor que 0.")]
        public decimal PrecioBase { get; set; }
    }

    public class FacturaResultadoViewModel
    {
        public int FacturaID { get; set; }
        public int PacienteID { get; set; }
        public string PacienteNombre { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal Total { get; set; }

        // Detalles de exámenes
        public List<DetalleFacturaViewModel> DetallesFactura { get; set; } = new List<DetalleFacturaViewModel>();

        // Detalles de medicamentos
        public List<MedicamentoFacturaViewModel> MedicamentosFactura { get; set; } = new List<MedicamentoFacturaViewModel>();
    }

    public class DetalleFacturaViewModel
    {
        public int DetalleFacturaID { get; set; }
        public int TipoExamenID { get; set; }
        public string NombreExamen { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class MedicamentoFacturaViewModel
    {
        public int DetalleMedicamentoFacturaID { get; set; }
        public int MedicamentoID { get; set; }
        public string NombreMedicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
