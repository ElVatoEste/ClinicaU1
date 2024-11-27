using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class PacienteGestionViewModel
    {
        // Propiedades para búsqueda
        public string? NombreBusqueda { get; set; }
        public string? ApellidoBusqueda { get; set; }
        public DateTime? FechaNacimientoBusqueda { get; set; }
        public string? IdentificacionBusqueda { get; set; }
        public List<PacienteResultado> Pacientes { get; set; } = new();

        // Propiedades para registro
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(100)]
        public string Identificacion {  get; set; }

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

    public class PacienteResultado
    {
        public int ExpedienteID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
    }
}
