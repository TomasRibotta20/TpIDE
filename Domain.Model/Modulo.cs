namespace Domain.Model
{
    public class Modulo
    {
        public int Id_Modulo { get; set; }
        public string Desc_Modulo { get; set; } = string.Empty;
        public string Ejecuta { get; set; } = string.Empty;

        // Navegación
        public virtual ICollection<ModulosUsuarios> ModulosUsuarios { get; set; } = new List<ModulosUsuarios>();

        public Modulo() { }
        
        public Modulo(string desc_Modulo, string ejecuta)
        {
            Desc_Modulo = desc_Modulo;
            Ejecuta = ejecuta;
        }
    }
}