
namespace Domain.Model
{
    public class ModulosUsuarios
    {
        public int Id_ModuloUsuario { get; set; }
        public int UsuarioId { get; set; }
        public int ModuloId { get; set; }
        public bool alta { get; set; }
        public bool baja { get; set; }
        public bool modificacion { get; set; }
        public bool consulta { get; set; }

        // Navegación
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual Modulo Modulo { get; set; } = null!;

        public ModulosUsuarios() { }

        public ModulosUsuarios(int usuarioId, int moduloId, bool alta, bool baja, bool modificacion, bool consulta)
        {
            SetID(usuarioId);
            SetModuloID(moduloId);
            SetAlta(alta);
            SetBaja(baja);
            SetModificacion(modificacion);
            SetConsulta(consulta);

        }

        public void SetID(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El Id del Usuario debe ser mayor que 0.", nameof(id));
            UsuarioId = id;

        }
        public void SetModuloID(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El Id del Modulo debe ser mayor que 0.", nameof(id));
            ModuloId = id;
        }

        public void SetAlta(bool alta)
        {
            this.alta = alta;
        }

        public void SetBaja(bool baja)
        {
            this.baja = baja;
        }

        public void SetModificacion(bool modificacion)
        {
            this.modificacion = modificacion;
        }

        public void SetConsulta(bool consulta)
        {
            this.consulta = consulta;
        }

        public bool TienePermiso(string permiso)
        {
            
            return permiso.ToLower() switch
            {
                "alta" => alta,
                "baja" => baja,
                "modificacion" => modificacion,
                "consulta" => consulta,
                _ => false,
            };
        }

        internal IEnumerable<string> ObtenerNombresPermisos()
        {           
            var permisos = new List<string>();
            if (alta) permisos.Add("alta");
            if (baja) permisos.Add("baja");
            if (modificacion) permisos.Add("modificacion");
            if (consulta) permisos.Add("consulta");
            return permisos;
        }
    }
}