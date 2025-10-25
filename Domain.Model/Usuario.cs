using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Domain.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Habilitado { get; set; }
        public string Salt { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        
        // Relación con Persona usando una FK separada (nullable)
        public int? PersonaId { get; set; }
        public virtual Persona? Persona { get; set; }
        
        public virtual ICollection<ModulosUsuarios> ModulosUsuarios { get; set; } = new List<ModulosUsuarios>();

        // Constructor para usuarios existentes (con ID)
        public Usuario(int id, string nombre, string apellido, string username, string email, string password, Persona? persona, bool habilitado = true)
        {
            SetId(id);
            SetNombre(nombre);
            SetApellido(apellido);
            SetEmail(email);
            SetPassword(password);
            SetHabilitado(habilitado);
            SetUsername(username);
            if (persona != null)
            {
                SetPersona(persona);
            }
        }

        // Constructor para nuevos usuarios (sin ID, será autogenerado por EF)
        public Usuario(string nombre, string apellido, string username, string email, string password, bool habilitado = true)
        {
            SetNombre(nombre);
            SetApellido(apellido);
            SetEmail(email);
            SetPassword(password);
            SetHabilitado(habilitado);
            SetUsername(username);
            Id = 0;
        }

        // Constructor público sin parámetros para Entity Framework
        public Usuario()
        {
            // Inicialización básica
        }

        public void SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.", nameof(username));

            if (username.Length < 3)
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.", nameof(username));

            UsuarioNombre = username;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(nombre));
            Nombre = nombre;
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede ser nulo o vacío.", nameof(apellido));
            Apellido = apellido;
        }

        public void SetEmail(string email)
        {
            if (!EsEmailValido(email))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(email));
            Email = email;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(password));

            if (password.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.", nameof(password));

            Salt = GenerateSalt();
            PasswordHash = HashPassword(password, Salt);
        }

        public void SetHabilitado(bool habilitado)
        {
            Habilitado = habilitado;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (string.IsNullOrWhiteSpace(Salt) || string.IsNullOrWhiteSpace(PasswordHash))
                return false;

            string hashedInput = HashPassword(password, Salt);
            return PasswordHash == hashedInput;
        }

        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return emailRegex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[32];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            byte[] saltedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }

        // Método actualizado: Agregar un módulo al usuario
        public void AgregarModuloUsuario(ModulosUsuarios moduloUsuario)
        {
            if (moduloUsuario == null)
                throw new ArgumentException("El módulo de usuario no puede ser nulo.", nameof(moduloUsuario));

            ModulosUsuarios.Add(moduloUsuario);
        }

        // Verifica si el usuario tiene un permiso específico en algún módulo
        public bool TienePermiso(string permiso)
        {
            if (!Habilitado)
                throw new InvalidOperationException("El usuario está deshabilitado");

            if (ModulosUsuarios == null || !ModulosUsuarios.Any())
                return false;

            return ModulosUsuarios.Any(mu => mu.TienePermiso(permiso));
        }

        // Verifica si el usuario tiene un permiso específico en un módulo específico
        public bool TienePermisoEnModulo(string permiso, int moduloId)
        {
            if (!Habilitado)
                throw new InvalidOperationException("El usuario está deshabilitado");

            var moduloUsuario = ModulosUsuarios?.FirstOrDefault(mu => mu.ModuloId == moduloId);
            return moduloUsuario?.TienePermiso(permiso) ?? false;
        }

        // Obtiene todos los permisos del usuario en todos los módulos
        public IEnumerable<string> ObtenerTodosLosPermisos()
        {
            if (ModulosUsuarios == null || !ModulosUsuarios.Any())
                return new List<string>();

            var permisos = new HashSet<string>();
            foreach (var moduloUsuario in ModulosUsuarios)
            {
                foreach (var permiso in moduloUsuario.ObtenerNombresPermisos())
                {
                    permisos.Add(permiso);
                }
            }
            return permisos;
        }

        // Obtiene los nombres de todos los módulos asignados al usuario
        public IEnumerable<string> ObtenerNombresModulos()
        {
            if (ModulosUsuarios == null || !ModulosUsuarios.Any())
                return new List<string>();

            return ModulosUsuarios
                .Where(mu => mu.Modulo != null)
                .Select(mu => mu.Modulo.Desc_Modulo);
        }

        public void SetPersona(Persona? persona)
        {
            // Ahora la persona puede ser nula ya que no se mapea a la BD
            Persona = persona;
        }
    }
}
