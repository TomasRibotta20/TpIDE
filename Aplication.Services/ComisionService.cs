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

        public IEnumerable<DTOs.ComisionDto> GetAll() => _repository.GetAll().Select(MapToDto);

        public DTOs.ComisionDto GetById(int id)
            {
                var comision = _repository.GetById(id);
                return comision == null ? null : MapToDto(comision);
            }
        public void Add(DTOs.ComisionDto comisionDto)
            {
            var comision = MapToEntityForCreation(comisionDto);
            _repository.Add(comision);
        }
        
        public void Update(DTOs.ComisionDto comisionDto)
            {
            var comision = MapToEntityForUpdate(comisionDto);
            _repository.Update(comision);
        }

        public void Delete(int id)
            {
            _repository.Delete(id);
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
