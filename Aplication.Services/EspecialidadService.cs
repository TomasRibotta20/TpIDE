using Data;
using Domain.Model;
using DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class EspecialidadService
    {
        private readonly EspecialidadRepository _repository;

        public EspecialidadService()
        {
            _repository = new EspecialidadRepository();
        }

        public async Task<IEnumerable<EspecialidadDto>> GetAllAsync()
        {
            var especialidades = await _repository.GetAllAsync();
            return especialidades.Select(MapToDto);
        }

        public async Task<EspecialidadDto?> GetByIdAsync(int id)
        {
            var especialidad = await _repository.GetByIdAsync(id);
            return especialidad == null ? null : MapToDto(especialidad);
        }

        public async Task AddAsync(EspecialidadDto especialidadDto)
        {
            var especialidad = MapToEntityForCreation(especialidadDto);
            await _repository.AddAsync(especialidad);
        }

        public async Task UpdateAsync(EspecialidadDto especialidadDto)
        {
            var especialidad = MapToEntityForUpdate(especialidadDto);
            await _repository.UpdateAsync(especialidad);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private EspecialidadDto MapToDto(Especialidad especialidad) => new EspecialidadDto
        {
            Id = especialidad.Id,
            Descripcion = especialidad.Descripcion
        };

        // Para crear nuevas especialidades (sin ID)
        private Especialidad MapToEntityForCreation(EspecialidadDto dto) => new Especialidad(dto.Descripcion);

        // Para actualizar especialidades existentes (con ID)
        private Especialidad MapToEntityForUpdate(EspecialidadDto dto) => new Especialidad(dto.Id, dto.Descripcion);
    }
}