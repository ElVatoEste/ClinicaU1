using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class Detalle
    {
        [Key]
        public int DetalleID { get; set; }

        [Required]
        public int TipoExamenID { get; set; }

        [ForeignKey("TipoExamenID")]
        public TipoExamen TipoExamen { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; }

        public ICollection<DetalleFactura> DetallesFactura { get; set; }
    }
}
