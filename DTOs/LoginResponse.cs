using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int? PersonaId { get; set; }
        public string TipoUsuario { get; set; } = string.Empty; // "Administrador", "Profesor", "Alumno"
        public List<string> Permissions { get; set; } = new List<string>();
        public List<string> Modules { get; set; } = new List<string>();
        public Dictionary<string, List<string>> ModulePermissions { get; set; } = new Dictionary<string, List<string>>();
    }
}
