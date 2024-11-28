namespace Clinica.ViewModels
{
    public class CrearRecetaViewModel
    {
        public int DiagnosticoID { get; set; }
        public DateTime Fecha { get; set; }
        public IEnumerable<MedicamentoViewModel> MedicamentosDisponibles { get; set; } = new List<MedicamentoViewModel>();
        public List<MedicamentoSeleccionadoViewModel> MedicamentosSeleccionados { get; set; } = new List<MedicamentoSeleccionadoViewModel>();
    }


    public class MedicamentoSeleccionadoViewModel
    {
        public int MedicamentoID { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }

}
