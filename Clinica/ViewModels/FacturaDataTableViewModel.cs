namespace Clinica.ViewModels
{
    public class FacturaDataTableViewModel
    {
        public int FacturaID { get; set; }
        public int PacienteID { get; set; }
        public string PacienteNombre { get; set; }
        public string PacienteApellido { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal Total { get; set; }
        public List<DetalleFacturaDataTableViewModel> Detalles { get; set; } = new List<DetalleFacturaDataTableViewModel>();
        public List<MedicamentoFacturaDataTableViewModel> Medicamentos { get; set; } = new List<MedicamentoFacturaDataTableViewModel>();
    }

    public class DetalleFacturaDataTableViewModel
    {
        public int DetalleFacturaID { get; set; }
        public int DetalleID { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }

    public class MedicamentoFacturaDataTableViewModel
    {
        public int DetalleMedicamentoFacturaID { get; set; }
        public int MedicamentoID { get; set; }
        public string MedicamentoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
