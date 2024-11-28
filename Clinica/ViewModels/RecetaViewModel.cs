using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class RecetaViewModel
    {
        public int ExpedienteID { get; set; }
        public int RecetaID { get; set; }
        public int DiagnosticoID { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleRecetaViewModel> DetallesReceta { get; set; } = new List<DetalleRecetaViewModel>();
        public List<MedicamentoViewModel> MedicamentosDisponibles { get; set; } = new List<MedicamentoViewModel>();
    }

    public class DetalleRecetaViewModel
    {
        public int RecetaID { get; set; }
        public int MedicamentoID { get; set; }
        public int DiagnosticoID { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioBase { get; set; }
    }

    public class AgregarMedicamentoViewModel
    {
        [Required]
        public int RecetaID { get; set; }

        [Required]
        public int DiagnosticoID { get; set; }

        [Required]
        public int MedicamentoID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
        public int Cantidad { get; set; }
    }

}
