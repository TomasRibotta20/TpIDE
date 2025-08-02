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
    }
}
