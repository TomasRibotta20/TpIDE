using Data;
using Domain.Model;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Aplication.Services
{
    public class UsuarioService
    {
        private readonly AcademiaContext _context;
        private readonly UsuarioRepository _repository;

        public UsuarioService()
        {
            _context = new AcademiaContext();
            _repository = new UsuarioRepository(_context);
        }

        /// <summary>
        /// Inicializa los módulos base del sistema si no existen
        /// </summary>
        public async Task InicializarModulosAsync()
        {
            if (await _context.Modulos.AnyAsync())
            {
                System.Diagnostics.Debug.WriteLine("[UsuarioService] Módulos ya inicializados");
                return;
            }

            var modulos = new List<Modulo>
            {
                new Modulo("Usuarios", "Gestión de usuarios del sistema"),
                new Modulo("Alumnos", "Gestión de alumnos"),
                new Modulo("Profesores", "Gestión de profesores"),
                new Modulo("Cursos", "Gestión de cursos"),
                new Modulo("Inscripciones", "Gestión de inscripciones a cursos"),
                new Modulo("Planes", "Gestión de planes de estudio"),
                new Modulo("Especialidades", "Gestión de especialidades"),
                new Modulo("Comisiones", "Gestión de comisiones"),
                new Modulo("Reportes", "Visualización de reportes")
            };

            _context.Modulos.AddRange(modulos);
            await _context.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine($"[UsuarioService] {modulos.Count} módulos inicializados correctamente");
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return usuarios.Select(MapToDto);
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            return usuario == null ? null : MapToDto(usuario);
        }

        public async Task AddAsync(UsuarioDto usuarioDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Asegurar que existan los módulos
                await InicializarModulosAsync();

                // Validar persona según tipo de usuario
                await ValidarPersonaSegunTipoAsync(usuarioDto.PersonaId);

                // Crear el usuario
                var usuario = new Usuario(
                    usuarioDto.Nombre,
                    usuarioDto.Apellido,
                    usuarioDto.UsuarioNombre,
                    usuarioDto.Email,
                    usuarioDto.Contrasenia,
                    usuarioDto.Habilitado
                );

                // Asignar PersonaId si se especificó
                if (usuarioDto.PersonaId.HasValue)
                {
                    usuario.PersonaId = usuarioDto.PersonaId.Value;
                }

                // Guardar el usuario para obtener su ID
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Asignar permisos según tipo de usuario
                await AsignarPermisosPorTipoUsuarioAsync(usuario.Id, usuarioDto.PersonaId);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear usuario: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(UsuarioDto usuarioDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var usuarioExistente = await _repository.GetByIdAsync(usuarioDto.Id);
                
                if (usuarioExistente == null)
                    throw new InvalidOperationException($"Usuario con Id {usuarioDto.Id} no encontrado");

                // Validar persona según tipo de usuario
                await ValidarPersonaSegunTipoAsync(usuarioDto.PersonaId);

                // Actualizar los campos básicos
                usuarioExistente.SetNombre(usuarioDto.Nombre);
                usuarioExistente.SetApellido(usuarioDto.Apellido);
                usuarioExistente.SetUsername(usuarioDto.UsuarioNombre);
                usuarioExistente.SetEmail(usuarioDto.Email);
                usuarioExistente.SetHabilitado(usuarioDto.Habilitado);
                
                // Actualizar PersonaId
                var personaIdAnterior = usuarioExistente.PersonaId;
                usuarioExistente.PersonaId = usuarioDto.PersonaId;
                
                // Solo actualizar la contraseña si se proporciona una nueva y no es el hash existente
                if (!string.IsNullOrWhiteSpace(usuarioDto.Contrasenia) && 
                    usuarioDto.Contrasenia != usuarioExistente.PasswordHash)
                {
                    usuarioExistente.SetPassword(usuarioDto.Contrasenia);
                }

                _context.Usuarios.Update(usuarioExistente);
                await _context.SaveChangesAsync();
                
                // Si cambió la persona asociada, reconfigurar permisos
                if (personaIdAnterior != usuarioDto.PersonaId)
                {
                    await AsignarPermisosPorTipoUsuarioAsync(usuarioDto.Id, usuarioDto.PersonaId);
                }
                // Si se especificaron permisos manualmente, usarlos
                else if (usuarioDto.Permisos != null && usuarioDto.Permisos.Any())
                {
                    // Eliminar permisos existentes
                    var permisosExistentes = await _context.ModulosUsuarios
                        .Where(mu => mu.UsuarioId == usuarioDto.Id)
                        .ToListAsync();
                    
                    _context.ModulosUsuarios.RemoveRange(permisosExistentes);
                    await _context.SaveChangesAsync();
                    
                    // Agregar nuevos permisos
                    foreach (var permisoDto in usuarioDto.Permisos)
                    {
                        var moduloUsuario = new ModulosUsuarios
                        {
                            UsuarioId = usuarioDto.Id,
                            ModuloId = permisoDto.ModuloId,
                            alta = permisoDto.Alta,
                            baja = permisoDto.Baja,
                            modificacion = permisoDto.Modificacion,
                            consulta = permisoDto.Consulta
                        };

                        _context.ModulosUsuarios.Add(moduloUsuario);
                    }
                    
                    await _context.SaveChangesAsync();
                }
                
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al actualizar usuario: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Obtiene todos los módulos disponibles del sistema
        /// </summary>
        public async Task<IEnumerable<ModuloDto>> GetModulosAsync()
        {
            await InicializarModulosAsync();
            var modulos = await _context.Modulos.ToListAsync();
            return modulos.Select(m => new ModuloDto
            {
                Id_Modulo = m.Id_Modulo,
                Desc_Modulo = m.Desc_Modulo,
                Ejecuta = m.Ejecuta
            });
        }

        /// <summary>
        /// Obtiene tipos de usuario predefinidos
        /// </summary>
        public List<string> GetTiposUsuario()
        {
            return new List<string> { "Administrador", "Profesor", "Alumno" };
        }

        /// <summary>
        /// Determina el tipo de usuario basándose en la persona asociada
        /// </summary>
        private async Task<string> DeterminarTipoUsuarioAsync(int? personaId)
        {
            if (personaId == null)
                return "Administrador";

            var persona = await _context.Personas.FindAsync(personaId.Value);
            if (persona == null)
                return "Administrador";

            return persona.TipoPersona switch
            {
                TipoPersona.Profesor => "Profesor",
                TipoPersona.Alumno => "Alumno",
                _ => "Administrador"
            };
        }

        /// <summary>
        /// Valida que la persona sea válida según el tipo de usuario
        /// </summary>
        private async Task ValidarPersonaSegunTipoAsync(int? personaId)
        {
            // Si es null, es administrador (válido)
            if (personaId == null)
                return;

            var persona = await _context.Personas.FindAsync(personaId.Value);
            if (persona == null)
                throw new InvalidOperationException("La persona especificada no existe");

            // Validar que sea Profesor o Alumno (no hay otro tipo válido)
            if (persona.TipoPersona != TipoPersona.Profesor && persona.TipoPersona != TipoPersona.Alumno)
                throw new InvalidOperationException("El tipo de persona debe ser Profesor o Alumno");
        }

        /// <summary>
        /// Asigna permisos según el tipo de usuario (determinado por la persona asociada)
        /// </summary>
        private async Task AsignarPermisosPorTipoUsuarioAsync(int usuarioId, int? personaId)
        {
            var tipoUsuario = await DeterminarTipoUsuarioAsync(personaId);
            var modulos = await _context.Modulos.ToListAsync();

            // Eliminar permisos existentes
            var permisosExistentes = await _context.ModulosUsuarios
                .Where(mu => mu.UsuarioId == usuarioId)
                .ToListAsync();
            _context.ModulosUsuarios.RemoveRange(permisosExistentes);

            // Asignar permisos según tipo
            List<ModulosUsuarios> nuevosPermisos = new();

            foreach (var modulo in modulos)
            {
                var permiso = CrearPermisoSegunTipo(usuarioId, modulo.Id_Modulo, modulo.Desc_Modulo, tipoUsuario);
                if (permiso != null)
                    nuevosPermisos.Add(permiso);
            }

            _context.ModulosUsuarios.AddRange(nuevosPermisos);
        }

        /// <summary>
        /// Crea un permiso según el tipo de usuario y módulo
        /// </summary>
        private ModulosUsuarios? CrearPermisoSegunTipo(int usuarioId, int moduloId, string nombreModulo, string tipoUsuario)
        {
            bool alta = false, baja = false, modificacion = false, consulta = false;

            switch (tipoUsuario)
            {
                case "Administrador":
                    // Administrador: todos los permisos en todos los módulos
                    alta = baja = modificacion = consulta = true;
                    break;

                case "Profesor":
                    // Profesor: permisos según módulo
                    switch (nombreModulo)
                    {
                        case "Inscripciones":
                            // Puede modificar condiciones y notas
                            modificacion = consulta = true;
                            break;
                        case "Cursos":
                        case "Alumnos":
                        case "Reportes":
                            // Solo consulta
                            consulta = true;
                            break;
                        case "Profesores":
                            // Puede ver y modificar sus propios datos
                            modificacion = consulta = true;
                            break;
                        default:
                            // Sin permisos en otros módulos
                            return null;
                    }
                    break;

                case "Alumno":
                    // Alumno: permisos limitados
                    switch (nombreModulo)
                    {
                        case "Inscripciones":
                            // Puede inscribirse y ver sus inscripciones
                            alta = consulta = true;
                            break;
                        case "Cursos":
                            // Solo consulta de cursos disponibles
                            consulta = true;
                            break;
                        case "Alumnos":
                            // Puede ver sus propios datos
                            consulta = true;
                            break;
                        default:
                            // Sin permisos en otros módulos
                            return null;
                    }
                    break;

                default:
                    return null;
            }

            return new ModulosUsuarios
            {
                UsuarioId = usuarioId,
                ModuloId = moduloId,
                alta = alta,
                baja = baja,
                modificacion = modificacion,
                consulta = consulta
            };
        }

        private UsuarioDto MapToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                UsuarioNombre = usuario.UsuarioNombre,
                Contrasenia = string.Empty, // No devolver el hash de contraseña
                Email = usuario.Email,
                Habilitado = usuario.Habilitado,
                PersonaId = usuario.PersonaId,
                persona = usuario.Persona != null ? new PersonaDto
                {
                    Id = usuario.Persona.Id,
                    Nombre = usuario.Persona.Nombre,
                    Apellido = usuario.Persona.Apellido,
                    Email = usuario.Persona.Email,
                    Direccion = usuario.Persona.Direccion,
                    Telefono = usuario.Persona.Telefono,
                    FechaNacimiento = usuario.Persona.FechaNacimiento,
                    Legajo = usuario.Persona.Legajo,
                    TipoPersona = usuario.Persona.TipoPersona == Domain.Model.TipoPersona.Alumno 
                        ? TipoPersonaDto.Alumno 
                        : TipoPersonaDto.Profesor,
                    IdPlan = usuario.Persona.IdPlan
                } : null,
                Permisos = usuario.ModulosUsuarios?.Select(mu => new ModulosUsuariosDto
                {
                    Id_ModuloUsuario = mu.Id_ModuloUsuario,
                    UsuarioId = mu.UsuarioId,
                    ModuloId = mu.ModuloId,
                    Alta = mu.alta,
                    Baja = mu.baja,
                    Modificacion = mu.modificacion,
                    Consulta = mu.consulta,
                    NombreModulo = mu.Modulo?.Desc_Modulo,
                    DescripcionModulo = mu.Modulo?.Ejecuta
                }).ToList() ?? new List<ModulosUsuariosDto>()
            };
        }

        // Métodos síncronos para compatibilidad con UsuarioEndpoints
        public IEnumerable<UsuarioDto> GetAll()
        {
            return GetAllAsync().GetAwaiter().GetResult();
        }

        public UsuarioDto? GetById(int id)
        {
            return GetByIdAsync(id).GetAwaiter().GetResult();
        }

        public void Add(UsuarioDto usuarioDto)
        {
            AddAsync(usuarioDto).GetAwaiter().GetResult();
        }

        public void Update(UsuarioDto usuarioDto)
        {
            UpdateAsync(usuarioDto).GetAwaiter().GetResult();
        }

        public void Delete(int id)
        {
            DeleteAsync(id).GetAwaiter().GetResult();
        }
    }
}