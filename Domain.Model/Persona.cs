using System;

namespace Domain.Model
{
    public enum TipoPersona
    {
        Alumno,
        Profesor
    }

    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Direccion { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Legajo { get; set; }
        public TipoPersona TipoPersona { get; set; }
        public int? IdPlan { get; set; }

        // Constructor para EF Core (sin parámetros)
        public Persona() { }

        // Constructor para crear una nueva persona (sin ID)
        public Persona(string nombre, string apellido, string? direccion, string email, string? telefono, DateTime fechaNacimiento, int legajo, TipoPersona tipoPersona, int? idPlan)
        {
            Nombre = nombre ?? string.Empty;
            Apellido = apellido ?? string.Empty;
            Direccion = direccion;
            Email = email ?? string.Empty;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Legajo = legajo;
            TipoPersona = tipoPersona;
            IdPlan = idPlan;
        }

        // Constructor para actualizar una persona existente (con ID)
        public Persona(int id, string nombre, string apellido, string? direccion, string email, string? telefono, DateTime fechaNacimiento, int legajo, TipoPersona tipoPersona, int? idPlan)
        {
            Id = id;
            Nombre = nombre ?? string.Empty;
            Apellido = apellido ?? string.Empty;
            Direccion = direccion;
            Email = email ?? string.Empty;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Legajo = legajo;
            TipoPersona = tipoPersona;
            IdPlan = idPlan;
        }
    }
}