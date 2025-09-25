namespace Domain.Model
{
    public class Especialidad
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    
        // Constructor para especialidades existentes (con ID)
        public Especialidad(int id, string descripcion) {
            SetID(id);
            SetDescripcion(descripcion);
        }
        
        // Constructor para nuevas especialidades (sin ID)
        public Especialidad(string descripcion) {
            SetDescripcion(descripcion);
            // El ID será asignado por la base de datos
            Id = 0;
        }
        
        // Constructor sin parámetros requerido por EF Core
        private Especialidad() { }

        public void SetID(int id) {
            if (id <= 0) {
                throw new ArgumentException("El ID debe ser mayor que cero.", nameof(id));
            }
            Id = id;
        }
        
        public void SetDescripcion(string descripcion) {
            if (string.IsNullOrWhiteSpace(descripcion)) {
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(descripcion));
            }
            if (descripcion.Length > 50) {
                throw new ArgumentException("La descripción no puede exceder los 50 caracteres.", nameof(descripcion));
            }
            Descripcion = descripcion;
        }
    }
}