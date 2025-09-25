using Domain.Model;
namespace Data
{
    public class UsuarioRepository
    {
        private AcademiaContext CreateContext()
        {
            return new AcademiaContext();
        }

        public IEnumerable<Usuario> GetAll()
        {
            using var context = CreateContext();
            return context.Usuarios.OrderBy(u => u.Apellido).ThenBy(u => u.Nombre).ToList();
        }
        public Usuario GetById(int id)
        {
            using var context = CreateContext();
            return context.Usuarios.Find(id);
        }
  
        public Usuario GetByUsername(string username)
        {
            using var context = CreateContext();
            return context.Usuarios.FirstOrDefault(u => u.UsuarioNombre == username);
        }

        public void Add(Usuario usuario)
        {
            using var context = CreateContext();
            context.Usuarios.Add(usuario);
            context.SaveChanges();
        }
        public void Update(Usuario usuario)
        {
            using var context = CreateContext();
            context.Usuarios.Update(usuario);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            using var context = CreateContext();
            var usuario = context.Usuarios.Find(id);
            if (usuario != null)
            {
                context.Usuarios.Remove(usuario);
                context.SaveChanges();
            }
        }

        public bool EmailExists(string email, int? excludeId = null)
        {
            using var context = CreateContext();
            var query = context.Usuarios.Where(c => c.Email.ToLower() == email.ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }
            return query.Any();
        }
    }
}