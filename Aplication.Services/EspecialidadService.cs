using Data;
using Domain.Model;
using DTOs;
namespace Aplication.Services
{
    public class EspecialidadService
    {
        private readonly EspecialidadRepository _repository;

        public EspecialidadService()
        {
            _repository = new EspecialidadRepository();
        }

        public IEnumerable<EspecialidadDto> GetAll() => _repository.GetAll().Select(MapToDto);

        public EspecialidadDto GetById(int id)
        {
            var especialidad = _repository.GetById(id);
            return especialidad == null ? null : MapToDto(especialidad);
        }

        public void Add(EspecialidadDto especialidadDto)
        {
            var especialidad = MapToEntity(especialidadDto);
            _repository.Add(especialidad);
        }

        public void Update(EspecialidadDto especialidadDto)
        {
            var especialidad = MapToEntity(especialidadDto);
            _repository.Update(especialidad);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private EspecialidadDto MapToDto(Especialidad especialidad) => new EspecialidadDto
        {
            Id = especialidad.Id,
            Descripcion = especialidad.Descripcion
        };

        private Especialidad MapToEntity(EspecialidadDto dto) => new Especialidad
        {
            Id = dto.Id,
            Descripcion = dto.Descripcion
        };
    }
}