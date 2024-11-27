using System;
using System.Collections.Generic;

namespace Clinica.ViewModels
{
    public class ConsultaMedicaViewModel
    {
        public int ConsultaMedicaID { get; set; }
        public int ExpedienteID { get; set; }
        public int? SignosVitalesID { get; set; }
        public int? DiagnosticoID { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoConsulta { get; set; }
        public string EvaluacionMedica { get; set; }
    }

    public class ConsultasMedicasListViewModel
    {
        public int ExpedienteID { get; set; }
        public string PacienteNombre { get; set; } // Opcional: para mostrar el nombre del paciente
        public List<ConsultaMedicaViewModel> Consultas { get; set; } = new();
    }
}
