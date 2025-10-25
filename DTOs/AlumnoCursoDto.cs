using System;

namespace DTOs
{
    public enum CondicionAlumnoDto
    {
        Libre = 1,
        Regular = 2,
        Promocional = 3
    }

    public class AlumnoCursoDto
    {
        public int IdInscripcion { get; set; }
        public int IdAlumno { get; set; }
        public string? NombreAlumno { get; set; } // Para mostrar en la UI
        public string? ApellidoAlumno { get; set; } // Para mostrar en la UI
        public int? LegajoAlumno { get; set; } // Para mostrar en la UI
        public int IdCurso { get; set; }
        public string? DescripcionCurso { get; set; } // Para mostrar en la UI
        public CondicionAlumnoDto Condicion { get; set; }
        public int? Nota { get; set; }
        public DateTime? FechaInscripcion { get; set; } // Para auditoría
    }
}