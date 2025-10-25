using System;

namespace Domain.Model
{
    public class Curso
    {
        public int IdCurso { get; set; }
        public int IdMateria { get; set; } // Ya no nullable - requerido
        public int IdComision { get; set; }
        public int AnioCalendario { get; set; }
        public int Cupo { get; set; }

        private Curso() { }

        public Curso(int idMateria, int idComision, int anioCalendario, int cupo)
        {
            SetIdMateria(idMateria);
            SetIdComision(idComision);
            SetAnioCalendario(anioCalendario);
            SetCupo(cupo);
            IdCurso = 0; // El ID será asignado por la base de datos
        }

        public Curso(int idCurso, int idMateria, int idComision, int anioCalendario, int cupo)
        {
            SetIdCurso(idCurso);
            SetIdMateria(idMateria);
            SetIdComision(idComision);
            SetAnioCalendario(anioCalendario);
            SetCupo(cupo);
        }

        public void SetIdCurso(int idCurso)
        {
            if (idCurso <= 0)
                throw new ArgumentException("El ID del curso debe ser mayor que cero.", nameof(idCurso));
            IdCurso = idCurso;
        }

        public void SetIdMateria(int idMateria)
        {
            if (idMateria <= 0)
                throw new ArgumentException("El ID de la materia debe ser mayor que cero.", nameof(idMateria));
            IdMateria = idMateria;
        }

        public void SetIdComision(int idComision)
        {
            if (idComision <= 0)
                throw new ArgumentException("El ID de la comisión debe ser mayor que cero.", nameof(idComision));
            IdComision = idComision;
        }

        public void SetAnioCalendario(int anioCalendario)
        {
            int currentYear = DateTime.Now.Year;
            if (anioCalendario < 2000 || anioCalendario > currentYear + 5)
                throw new ArgumentException($"El año calendario debe estar entre 2000 y {currentYear + 5}.", nameof(anioCalendario));
            AnioCalendario = anioCalendario;
        }

        public void SetCupo(int cupo)
        {
            if (cupo <= 0)
                throw new ArgumentException("El cupo debe ser mayor que cero.", nameof(cupo));
            if (cupo > 100)
                throw new ArgumentException("El cupo no puede ser mayor a 100 estudiantes.", nameof(cupo));
            Cupo = cupo;
        }
    }
}