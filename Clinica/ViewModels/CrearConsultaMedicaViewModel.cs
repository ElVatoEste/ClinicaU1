using System;
using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class CrearConsultaMedicaViewModel
    {
        [Required]
        public int ExpedienteID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaSignosVitales { get; set; }

        [Required]
        [StringLength(50)]
        public string Presion { get; set; }

        [Required]
        [StringLength(50)]
        public string Pulso { get; set; }

        [Required]
        [StringLength(50)]
        public string Temperatura { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoConsulta { get; set; }

        [Required]
        [StringLength(500)]
        public string EvaluacionMedica { get; set; }
    }
}
