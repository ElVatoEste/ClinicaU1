using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class Receta
    {
        [Key]
        public int RecetaID { get; set; }

        [Required]
        public int DiagnosticoID { get; set; }

        [ForeignKey("DiagnosticoID")]
        public Diagnostico Diagnostico { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public ICollection<DetalleReceta> DetallesReceta { get; set; }
    }

}
