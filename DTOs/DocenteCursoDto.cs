namespace DTOs
{
    public enum TipoCargoDto
    {
        JefeDeCatedra,
        Titular,
        Auxiliar
    }

    public class DocenteCursoDto
    {
        public int IdDictado { get; set; }
        public int IdCurso { get; set; }
        public int IdDocente { get; set; }
        public TipoCargoDto Cargo { get; set; }

        // Propiedades adicionales para mostrar en la interfaz
        public string? NombreDocente { get; set; }
        public string? ApellidoDocente { get; set; }
        public string? NombreCurso { get; set; }
        public string? NombreMateria { get; set; }
        public string? DescComision { get; set; }
        public int? AnioCalendario { get; set; }

        // Propiedades calculadas
        public string NombreCompleto => $"{ApellidoDocente}, {NombreDocente}";
        public string CargoDescripcion => Cargo switch
        {
            TipoCargoDto.JefeDeCatedra => "Jefe de Cátedra",
            TipoCargoDto.Titular => "Titular",
            TipoCargoDto.Auxiliar => "Auxiliar",
            _ => "Desconocido"
        };
        public string DescripcionCurso => $"{NombreMateria} - {DescComision} ({AnioCalendario})";
    }

    public class DocenteCursoCreateDto
    {
        public int IdCurso { get; set; }
        public int IdDocente { get; set; }
        public TipoCargoDto Cargo { get; set; }
    }
}
