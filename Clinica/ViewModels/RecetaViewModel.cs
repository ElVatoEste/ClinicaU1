namespace Clinica.ViewModels
{
    public class RecetaViewModel
    {
        public int RecetaID { get; set; }
        public int DiagnosticoID { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleRecetaViewModel> DetallesReceta { get; set; } = new();
        public List<MedicamentoViewModel> MedicamentosDisponibles { get; set; } = new(); // Lista de medicamentos existentes
        public AgregarMedicamentoViewModel NuevoMedicamento { get; set; } = new();
    }

    public class AgregarMedicamentoViewModel
    {
        public int RecetaID { get; set; }
        public int MedicamentoID { get; set; }
        public int Cantidad { get; set; }
    }


    public class DetalleRecetaViewModel
    {
        public int MedicamentoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioBase { get; set; }
    }
    public class CrearRecetaViewModel
    {
        public int DiagnosticoID { get; set; }
        public DateTime Fecha { get; set; }
    }

}
