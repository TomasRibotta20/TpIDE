namespace DTOs
{
    public class CursoDto
    {
        public int IdCurso { get; set; }
        public int? IdMateria { get; set; } // Nullable temporalmente
        public string? NombreMateria { get; set; } // Para mostrar en la UI
        public int IdComision { get; set; }
        public string? DescComision { get; set; } // Para mostrar en la UI
        public int AnioCalendario { get; set; }
        public int Cupo { get; set; }
        public int? InscriptosActuales { get; set; } // Para mostrar cupo disponible
        
        // Propiedades calculadas para compatibilidad con formularios
        public string Nombre => NombreMateria ?? "Sin nombre";
        public string Comision => DescComision ?? "Sin comision";
        public int InscriptosCount => InscriptosActuales ?? 0;
    }
}