namespace DTOs
{
    public class ModulosUsuariosDto
    {
        public int Id_ModuloUsuario { get; set; }
        public int UsuarioId { get; set; }
        public int ModuloId { get; set; }
        public bool Alta { get; set; }
        public bool Baja { get; set; }
        public bool Modificacion { get; set; }
        public bool Consulta { get; set; }
        
        // Información adicional del módulo (opcional, para facilitar visualización)
        public string? NombreModulo { get; set; }
        public string? DescripcionModulo { get; set; }
    }
}