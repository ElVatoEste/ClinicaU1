using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class Diagnostico
    {
        [Key]
        public int DiagnosticoID { get; set; }

        [Required]
        public int ConsultaMedicaID { get; set; }

        [ForeignKey("ConsultaMedicaID")]
        public ConsultaMedica ConsultaMedica { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public ICollection<Receta> Recetas { get; set; }
    }
}
