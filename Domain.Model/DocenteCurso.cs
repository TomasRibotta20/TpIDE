using System;

namespace Domain.Model
{
    public enum TipoCargo
    {
        JefeDeCatedra,
        Titular,
        Auxiliar
    }

    public class DocenteCurso
    {
        public int IdDictado { get; set; }
        public int IdCurso { get; set; }
        public int IdDocente { get; set; }
        public TipoCargo Cargo { get; set; }

        // Navigation properties
        public virtual Curso? Curso { get; set; }
        public virtual Persona? Docente { get; set; }

        // Constructor sin parámetros para EF Core
        private DocenteCurso() { }

        // Constructor para crear nueva asignación
        public DocenteCurso(int idCurso, int idDocente, TipoCargo cargo)
        {
            SetIdCurso(idCurso);
            SetIdDocente(idDocente);
            SetCargo(cargo);
            IdDictado = 0; // Será asignado por la base de datos
        }

        // Constructor para actualizar asignación existente
        public DocenteCurso(int idDictado, int idCurso, int idDocente, TipoCargo cargo)
        {
            SetIdDictado(idDictado);
            SetIdCurso(idCurso);
            SetIdDocente(idDocente);
            SetCargo(cargo);
        }

        public void SetIdDictado(int idDictado)
        {
            if (idDictado <= 0)
                throw new ArgumentException("El ID del dictado debe ser mayor que cero.", nameof(idDictado));
            IdDictado = idDictado;
        }

        public void SetIdCurso(int idCurso)
        {
            if (idCurso <= 0)
                throw new ArgumentException("El ID del curso debe ser mayor que cero.", nameof(idCurso));
            IdCurso = idCurso;
        }

        public void SetIdDocente(int idDocente)
        {
            if (idDocente <= 0)
                throw new ArgumentException("El ID del docente debe ser mayor que cero.", nameof(idDocente));
            IdDocente = idDocente;
        }

        public void SetCargo(TipoCargo cargo)
        {
            if (!Enum.IsDefined(typeof(TipoCargo), cargo))
                throw new ArgumentException("El cargo especificado no es válido.", nameof(cargo));
            Cargo = cargo;
        }
    }
}
