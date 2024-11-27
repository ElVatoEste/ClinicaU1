using System.ComponentModel.DataAnnotations;

namespace Clinica.ViewModels
{
    public class PacienteFiltroViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Identificacion { get; set; }

        // Lista para almacenar los resultados de la búsqueda
        public List<PacienteResultado> Pacientes { get; set; } = new List<PacienteResultado>();
    }

}
