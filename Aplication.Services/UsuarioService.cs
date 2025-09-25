using Data;
using Domain.Model;
using DTOs;

namespace Aplication.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _repository;
        public UsuarioService()
        {
            _repository = new UsuarioRepository();
        }

        public IEnumerable<UsuarioDto> GetAll() => _repository.GetAll().Select(MapToDto);

        public UsuarioDto GetById(int id)
        {
            var usuario = _repository.GetById(id);
            return usuario == null ? null : MapToDto(usuario);
        }

        public void Add(UsuarioDto usuarioDto)
        {
            var usuario = MapToEntityForCreation(usuarioDto);
            _repository.Add(usuario);
        }

        public void Update(UsuarioDto usuarioDto)
        {
            var usuario = MapToEntityForUpdate(usuarioDto);
            _repository.Update(usuario);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private UsuarioDto MapToDto(Usuario usuario) => new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            UsuarioNombre = usuario.UsuarioNombre,
            Contrasenia = usuario.PasswordHash,
            Email = usuario.Email,
            Habilitado = usuario.Habilitado
        };

        // Para crear nuevos usuarios (sin ID)
        private Usuario MapToEntityForCreation(UsuarioDto dto) => new Usuario(
            dto.Nombre, 
            dto.Apellido, 
            dto.UsuarioNombre, 
            dto.Email, 
            dto.Contrasenia, 
            dto.Habilitado
        );

        // Para actualizar usuarios existentes (con ID)
        private Usuario MapToEntityForUpdate(UsuarioDto dto) => new Usuario(
            dto.Id, 
            dto.Nombre, 
            dto.Apellido, 
            dto.UsuarioNombre, 
            dto.Email, 
            dto.Contrasenia, 
            dto.Habilitado
        );
    }
}