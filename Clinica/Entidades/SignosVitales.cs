using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class SignosVitales
    {
        [Key]
        public int SignosVitalesID { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Presion { get; set; }

        public int? Pulso { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Temperatura { get; set; }

        public ICollection<ConsultaMedica> ConsultasMedicas { get; set; }
    }
}
