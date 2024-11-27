using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class Factura
    {
        [Key]
        public int FacturaID { get; set; }

        [Required]
        public int PacienteID { get; set; }

        [ForeignKey("PacienteID")]
        public Paciente Paciente { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime FechaFactura { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        public ICollection<DetalleFactura> DetallesFactura { get; set; }
        public ICollection<FacturaMedicamento> FacturaMedicamentos { get; set; }
    }
}
