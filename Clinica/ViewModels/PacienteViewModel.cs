using System;
using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class PacienteViewModel
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [Required, StringLength(100)]
        public string Identificacion { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(255)]
        public string Direccion { get; set; }

        [Required, StringLength(50)]
        public string Contacto { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, StringLength(10)]
        public string Sexo { get; set; }

        [Required, StringLength(20)]
        public string Telefono { get; set; }

        [Required, StringLength(50)]
        public string EstadoCivil { get; set; }

        [Required, StringLength(100)]
        public string NombreResp { get; set; }

        [Required, StringLength(50)]
        public string ParentescoResp { get; set; }

        [Required, StringLength(50)]
        public string ContactoResp { get; set; }
    }
}
