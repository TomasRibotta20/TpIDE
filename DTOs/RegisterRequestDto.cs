using System;

namespace DTOs
{
    public class RegisterRequestDto
    {
        // Datos del Usuario
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Datos de la Persona
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Legajo { get; set; }
        public TipoPersonaDto TipoPersona { get; set; }  // Profesor o Alumno
        public int? IdPlan { get; set; }
    }

    public class RegisterResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public int? PersonaId { get; set; }
        public UsuarioDto? Usuario { get; set; }
    }
}
