using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class Expediente
    {
        [Key]
        public int ExpedienteID { get; set; }

        [Required]
        public int PacienteID { get; set; }

        [ForeignKey("PacienteID")]
        public Paciente Paciente { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<ConsultaMedica> ConsultasMedicas { get; set; }
    }

}
