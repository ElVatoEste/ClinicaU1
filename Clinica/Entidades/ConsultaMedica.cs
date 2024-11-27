using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class ConsultaMedica
    {
        [Key]
        public int ConsultaMedicaID { get; set; }

        [Required]
        public int ExpedienteID { get; set; }

        [ForeignKey("ExpedienteID")]
        public Expediente Expediente { get; set; }

        [Required]
        public int SignosVitalesID { get; set; }

        [ForeignKey("SignosVitalesID")]
        public SignosVitales SignosVitales { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string TipoConsulta { get; set; }

        public string EvaluacionMedica { get; set; }

        public int? DiagnosticoID { get; set; }

        public ICollection<Diagnostico> Diagnosticos { get; set; }
    }
}
