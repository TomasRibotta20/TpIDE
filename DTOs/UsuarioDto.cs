namespace DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Contrasenia { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Habilitado { get; set; }

        // Relación con Persona
        public int? PersonaId { get; set; }
        public PersonaDto? persona { get; set; }

        // Información del módulo principal asignado
        public int? ModuloId { get; set; }
        public string? NombreModulo { get; set; }
        
        // Lista de todos los permisos del usuario en diferentes módulos
        public List<ModulosUsuariosDto> Permisos { get; set; } = new List<ModulosUsuariosDto>();
        
        // Propiedades calculadas útiles para la UI
        public string PermisosResumen => string.Join(", ", 
            Permisos.SelectMany(p => 
            {
                var permisos = new List<string>();
                if (p.Alta) permisos.Add("Alta");
                if (p.Baja) permisos.Add("Baja");
                if (p.Modificacion) permisos.Add("Modificación");
                if (p.Consulta) permisos.Add("Consulta");
                return permisos.Any() ? new[] { $"{p.NombreModulo}: {string.Join(", ", permisos)}" } : Array.Empty<string>();
            }));
    }
}
