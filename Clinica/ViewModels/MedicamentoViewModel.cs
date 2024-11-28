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
}
