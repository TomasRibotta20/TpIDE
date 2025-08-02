using Domain.Model;
namespace Data
{
    public class UsuarioRepository
    {
        private static List<Usuario> _usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, Nombre = "Juan", Apellido = "Perez", Email = "juan@mail.com", UsuarioNombre = "jperez", Contrasenia = "password", Habilitado = true },
            new Usuario { Id = 2, Nombre = "Maria", Apellido = "Gomez", Email = "maria@mail.com", UsuarioNombre = "mgomez", Contrasenia = "password", Habilitado = true }
        };

        public IEnumerable<Usuario> GetAll() => _usuarios;
        public Usuario GetById(int id) => _usuarios.FirstOrDefault(u => u.Id == id);
        public void Add(Usuario usuario)
        {
            usuario.Id = _usuarios.Any() ? _usuarios.Max(u => u.Id) + 1 : 1;
            _usuarios.Add(usuario);
        }
        public void Update(Usuario usuario)
        {
            var existing = GetById(usuario.Id);
            if (existing != null)
            {
                existing.Nombre = usuario.Nombre;
                existing.Apellido = usuario.Apellido;
                existing.UsuarioNombre = usuario.UsuarioNombre;
                existing.Contrasenia = usuario.Contrasenia;
                existing.Email = usuario.Email;
                existing.Habilitado = usuario.Habilitado;
            }
        }
        public void Delete(int id)
        {
            var usuario = GetById(id);
            if (usuario != null) _usuarios.Remove(usuario);
        }
    }
}