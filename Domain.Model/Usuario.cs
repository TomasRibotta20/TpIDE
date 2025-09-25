using System;
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

        // Agregar propiedades para manejo de contraseñas
        public string Salt { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Constructor para usuarios existentes (con ID)
        public Usuario(int id, string nombre, string apellido, string username, string email, string password, bool habilitado = true)
        {
            SetId(id);
            SetNombre(nombre);
            SetApellido(apellido);
            SetEmail(email);
            SetPassword(password);
            SetHabilitado(habilitado);
            SetUsername(username);
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
            // El ID se establece como 0 y será autogenerado por Entity Framework
            Id = 0;
        }

        // Constructor público sin parámetros para Entity Framework
        // Cambiado a público para que EF pueda usarlo y los objetos tengan funcionalidad completa
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
    }
}
