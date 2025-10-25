using System;

namespace Domain.Model
{
    public enum CondicionAlumno
    {
        Libre = 1,
        Regular = 2,
        Promocional = 3
    }

    public class AlumnoCurso
    {
        public int IdInscripcion { get; set; }
        public int IdAlumno { get; set; }
        public int IdCurso { get; set; }
        public CondicionAlumno Condicion { get; set; }
        public int? Nota { get; set; }

        private AlumnoCurso() { }

        public AlumnoCurso(int idAlumno, int idCurso, CondicionAlumno condicion, int? nota = null)
        {
            SetIdAlumno(idAlumno);
            SetIdCurso(idCurso);
            SetCondicion(condicion);
            SetNota(nota);
            IdInscripcion = 0; // El ID será asignado por la base de datos
        }

        public AlumnoCurso(int idInscripcion, int idAlumno, int idCurso, CondicionAlumno condicion, int? nota = null)
        {
            SetIdInscripcion(idInscripcion);
            SetIdAlumno(idAlumno);
            SetIdCurso(idCurso);
            SetCondicion(condicion);
            SetNota(nota);
        }

        public void SetIdInscripcion(int idInscripcion)
        {
            if (idInscripcion <= 0)
                throw new ArgumentException("El ID de la inscripción debe ser mayor que cero.", nameof(idInscripcion));
            IdInscripcion = idInscripcion;
        }

        public void SetIdAlumno(int idAlumno)
        {
            if (idAlumno <= 0)
                throw new ArgumentException("El ID del alumno debe ser mayor que cero.", nameof(idAlumno));
            IdAlumno = idAlumno;
        }

        public void SetIdCurso(int idCurso)
        {
            if (idCurso <= 0)
                throw new ArgumentException("El ID del curso debe ser mayor que cero.", nameof(idCurso));
            IdCurso = idCurso;
        }

        public void SetCondicion(CondicionAlumno condicion)
        {
            if (!Enum.IsDefined(typeof(CondicionAlumno), condicion))
                throw new ArgumentException("La condición del alumno no es válida.", nameof(condicion));
            Condicion = condicion;
        }

        public void SetNota(int? nota)
        {
            if (nota.HasValue && (nota.Value < 1 || nota.Value > 10))
                throw new ArgumentException("La nota debe estar entre 1 y 10.", nameof(nota));
            Nota = nota;
        }
    }
}