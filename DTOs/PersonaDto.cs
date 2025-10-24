using System;

namespace DTOs
{
    public enum TipoPersonaDto
    {
        Alumno,
        Profesor
    }

    public class PersonaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Direccion { get; set; }  // Permitir nulos
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }  // Permitir nulos
        public DateTime FechaNacimiento { get; set; }
        public int Legajo { get; set; }
        public TipoPersonaDto TipoPersona { get; set; }
        public int? IdPlan { get; set; }
    }
}