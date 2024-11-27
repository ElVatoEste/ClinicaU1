using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class DetalleFactura
    {
        [Key]
        public int DetalleFacturaID { get; set; }

        [Required]
        public int FacturaID { get; set; }

        [ForeignKey("FacturaID")]
        public Factura Factura { get; set; }

        [Required]
        public int DetalleID { get; set; }

        [ForeignKey("DetalleID")]
        public Detalle Detalle { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioUnitario { get; set; }
    }
}
