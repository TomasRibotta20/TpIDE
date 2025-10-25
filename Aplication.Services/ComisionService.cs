using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class ComisionService
    {
        private readonly ComisionRepository _repository;

        public ComisionService()
        {
            _repository = new ComisionRepository();
        }

        public async Task<IEnumerable<DTOs.ComisionDto>> GetAllAsync()
        {
            var comisiones = await _repository.GetAllAsync();
            return comisiones.Select(MapToDto);
        }

        public async Task<DTOs.ComisionDto?> GetByIdAsync(int id)
        {
            var comision = await _repository.GetByIdAsync(id);
            return comision == null ? null : MapToDto(comision);
        }

        public async Task AddAsync(DTOs.ComisionDto comisionDto)
        {
            var comision = MapToEntityForCreation(comisionDto);
            await _repository.AddAsync(comision);
        }
        
        public async Task UpdateAsync(DTOs.ComisionDto comisionDto)
        {
            var comision = MapToEntityForUpdate(comisionDto);
            await _repository.UpdateAsync(comision);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private DTOs.ComisionDto MapToDto(Domain.Model.Comision comision) => new DTOs.ComisionDto
        {
            IdComision = comision.IdComision,
            DescComision = comision.DescComision,
            AnioEspecialidad = comision.AnioEspecialidad,
            IdPlan = comision.IdPlan
        };

        // Para crear nuevas comisiones (sin ID)
        private Domain.Model.Comision MapToEntityForCreation(DTOs.ComisionDto dto) => new Domain.Model.Comision(
            dto.DescComision,
            dto.AnioEspecialidad,
            dto.IdPlan
        );

        // Para actualizar comisiones existentes (con ID)
        private Domain.Model.Comision MapToEntityForUpdate(DTOs.ComisionDto dto) => new Domain.Model.Comision(
            dto.IdComision,
            dto.DescComision,
            dto.AnioEspecialidad,
            dto.IdPlan
        );
    }
}
