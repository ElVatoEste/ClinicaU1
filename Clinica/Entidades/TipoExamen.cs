using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class TipoExamen
    {
        [Key]
        public int TipoExamenID { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreExamen { get; set; }

        public ICollection<Detalle> Detalles { get; set; }
    }
}
