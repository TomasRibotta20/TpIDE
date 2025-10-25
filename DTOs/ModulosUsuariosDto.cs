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
        
        // Informaci�n adicional del m�dulo (opcional, para facilitar visualizaci�n)
        public string? NombreModulo { get; set; }
        public string? DescripcionModulo { get; set; }
    }
}