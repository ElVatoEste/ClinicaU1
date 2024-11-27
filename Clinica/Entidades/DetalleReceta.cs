using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class DetalleReceta
    {
        [Key, Column(Order = 0)]
        public int RecetaID { get; set; }

        [ForeignKey("RecetaID")]
        public Receta Receta { get; set; }

        [Key, Column(Order = 1)]
        public int MedicamentoID { get; set; }

        [ForeignKey("MedicamentoID")]
        public Medicamento Medicamento { get; set; }

        [Required]
        public int Cantidad { get; set; }
    }
}
