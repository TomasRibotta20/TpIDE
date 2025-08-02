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
            var usuario = MapToEntity(usuarioDto);
            _repository.Add(usuario);
        }

        public void Update(UsuarioDto usuarioDto)
        {
            var usuario = MapToEntity(usuarioDto);
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
            Contrasenia = usuario.Contrasenia,
            Email = usuario.Email,
            Habilitado = usuario.Habilitado
        };

        private Usuario MapToEntity(UsuarioDto dto) => new Usuario
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            UsuarioNombre = dto.UsuarioNombre,
            Contrasenia = dto.Contrasenia,
            Email = dto.Email,
            Habilitado = dto.Habilitado
        };
    }
}