using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class FacturaMedicamento
    {
        [Key]
        public int DetalleMedicamentoFacturaID { get; set; }

        [Required]
        public int FacturaID { get; set; }

        [ForeignKey("FacturaID")]
        public Factura Factura { get; set; }

        [Required]
        public int MedicamentoID { get; set; }

        [ForeignKey("MedicamentoID")]
        public Medicamento Medicamento { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioUnitario { get; set; }
    }
}