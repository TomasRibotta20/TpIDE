using DTOs;
using Data;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSecret = _configuration["Jwt:Secret"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? "AcademiaAPI";
            _jwtAudience = _configuration["Jwt:Audience"] ?? "AcademiaClient";
            _jwtExpirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            using var context = new AcademiaContext();
            var usuarioRepo = new UsuarioRepository(context);
            
            // Buscar usuario por nombre de usuario
            var usuario = await usuarioRepo.GetByUsernameAsync(request.Username);
            
            if (usuario == null || !usuario.ValidatePassword(request.Password))
            {
                return null;
            }

            if (!usuario.Habilitado)
            {
                throw new InvalidOperationException("El usuario está deshabilitado");
            }

            // Determinar tipo de usuario
            string tipoUsuario;
            if (usuario.PersonaId == null)
            {
                tipoUsuario = "Administrador";
            }
            else if (usuario.Persona?.TipoPersona == TipoPersona.Profesor)
            {
                tipoUsuario = "Profesor";
            }
            else if (usuario.Persona?.TipoPersona == TipoPersona.Alumno)
            {
                tipoUsuario = "Alumno";
            }
            else
            {
                tipoUsuario = "Usuario";
            }

            // Generar token JWT
            var token = GenerateJwtToken(usuario);
            
            // Construir diccionario de permisos por módulo
            var modulePermissions = new Dictionary<string, List<string>>();
            foreach (var moduloUsuario in usuario.ModulosUsuarios)
            {
                if (moduloUsuario.Modulo != null)
                {
                    var moduleName = moduloUsuario.Modulo.Desc_Modulo;
                    var permissions = new List<string>();
                    
                    if (moduloUsuario.alta) permissions.Add("alta");
                    if (moduloUsuario.baja) permissions.Add("baja");
                    if (moduloUsuario.modificacion) permissions.Add("modificacion");
                    if (moduloUsuario.consulta) permissions.Add("consulta");
                    
                    modulePermissions[moduleName] = permissions;
                }
            }
            
            return new LoginResponse
            {
                UserId = usuario.Id,
                Token = token,
                Username = usuario.UsuarioNombre,
                Email = usuario.Email,
                PersonaId = usuario.PersonaId,
                TipoUsuario = tipoUsuario,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                Permissions = usuario.ObtenerTodosLosPermisos().ToList(),
                Modules = usuario.ObtenerNombresModulos().ToList(),
                ModulePermissions = modulePermissions
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                var usuarioService = new Aplication.Services.UsuarioService();
                var personaRepo = new PersonaRepository();

                // 1. Validar que el nombre de usuario no exista
                var usuarios = await usuarioService.GetAllAsync();
                if (usuarios.Any(u => u.UsuarioNombre.Equals(request.UsuarioNombre, StringComparison.OrdinalIgnoreCase)))
                {
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = "El nombre de usuario ya está en uso"
                    };
                }

                // 2. Validar que el email no exista
                var personas = await personaRepo.GetAllAsync();
                if (personas.Any(p => p.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = "El email ya está registrado"
                    };
                }

                // 3. Validar que el legajo no exista
                if (personas.Any(p => p.Legajo == request.Legajo))
                {
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = "El legajo ya está registrado"
                    };
                }

                // 4. Validar el tipo de persona
                if (request.TipoPersona != TipoPersonaDto.Profesor && request.TipoPersona != TipoPersonaDto.Alumno)
                {
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = "Solo se pueden registrar usuarios de tipo Profesor o Alumno"
                    };
                }

                // 5. Crear la Persona
                var persona = new Persona(
                    nombre: request.Nombre,
                    apellido: request.Apellido,
                    direccion: request.Direccion,
                    email: request.Email,
                    telefono: request.Telefono,
                    fechaNacimiento: request.FechaNacimiento,
                    legajo: request.Legajo,
                    tipoPersona: request.TipoPersona == TipoPersonaDto.Profesor ? TipoPersona.Profesor : TipoPersona.Alumno,
                    idPlan: request.IdPlan
                );

                await personaRepo.AddAsync(persona);

                // 6. Crear el Usuario (usando el servicio de usuarios para obtener permisos automáticos)
                var usuarioDto = new UsuarioDto
                {
                    UsuarioNombre = request.UsuarioNombre,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    Contrasenia = request.Password,
                    Habilitado = true,
                    PersonaId = persona.Id
                };

                await usuarioService.AddAsync(usuarioDto);
                
                // Obtener el usuario creado
                var usuariosActualizados = await usuarioService.GetAllAsync();
                var usuarioCreado = usuariosActualizados.FirstOrDefault(u => u.UsuarioNombre == request.UsuarioNombre);

                return new RegisterResponseDto
                {
                    Success = true,
                    Message = $"Usuario {request.TipoPersona} registrado exitosamente",
                    UsuarioId = usuarioCreado?.Id,
                    PersonaId = persona.Id,
                    Usuario = usuarioCreado
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = $"Error al registrar usuario: {ex.Message}"
                };
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UsuarioNombre),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Agregar módulos y permisos específicos de cada módulo como claims
            foreach (var moduloUsuario in usuario.ModulosUsuarios)
            {
                if (moduloUsuario.Modulo != null)
                {
                    var nombreModulo = moduloUsuario.Modulo.Desc_Modulo;
                    
                    // Agregar módulo
                    claims.Add(new Claim("Modulo", nombreModulo));
                    
                    // Agregar permisos específicos del módulo en formato "ModuleName:permission"
                    if (moduloUsuario.alta)
                        claims.Add(new Claim("Permission", $"{nombreModulo}:alta"));
                    if (moduloUsuario.baja)
                        claims.Add(new Claim("Permission", $"{nombreModulo}:baja"));
                    if (moduloUsuario.modificacion)
                        claims.Add(new Claim("Permission", $"{nombreModulo}:modificacion"));
                    if (moduloUsuario.consulta)
                        claims.Add(new Claim("Permission", $"{nombreModulo}:consulta"));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}