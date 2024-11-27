using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Entidades
{
    public class Medicamento
    {
        [Key]
        public int MedicamentoID { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioBase { get; set; }

        public  ICollection<DetalleReceta> DetalleRecetas { get; set; }
        public ICollection<FacturaMedicamento> FacturaMedicamentos { get; set; }
    }

}