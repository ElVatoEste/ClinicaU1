using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinica.Entidades
{
    public class Paciente
    {
        [Key]
        public int PacienteID { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Apellido { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaNacimiento { get; set; }

        [StringLength(100)]
        public string Identificacion { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }

        [StringLength(50)]
        public string Contacto { get; set; }

        [StringLength(100)]
        public string Correo { get; set; }

        [StringLength(10)]
        public string Sexo { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(50)]
        public string EstadoCivil { get; set; }

        [StringLength(100)]
        public string NombreResp { get; set; }

        [StringLength(50)]
        public string ParentescoResp { get; set; }

        [StringLength(20)]
        public string ContactoResp { get; set; }

        [Required]
        public int ExpedienteID { get; set; }

        [ForeignKey("ExpedienteID")]
        public Expediente Expediente { get; set; }

        public ICollection<Factura> Facturas { get; set; }
    }

}
