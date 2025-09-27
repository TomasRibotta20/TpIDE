using Data;
using Domain.Model;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class PlanService
    {
        private readonly PlanRepository _repository;
        public PlanService()
        {
            _repository = new PlanRepository();
        }

        public IEnumerable<PlanDto> GetAll() => _repository.GetAll().Select(MapToDto);
        public PlanDto GetById(int id)
        {
            var plan = _repository.GetById(id);
            return plan == null ? null : MapToDto(plan);
        }
        public void Add(PlanDto planDto)
        {
            var plan = MapToEntityForCreation(planDto);
            _repository.Add(plan);
        }
        public void Update(PlanDto planDto)
        {
            var plan = MapToEntityForUpdate(planDto);
            _repository.Update(plan);
        }
        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private PlanDto MapToDto(Plan plan) => new PlanDto
        {
            Id = plan.Id,
            Descripcion = plan.Descripcion,
            EspecialidadId = plan.EspecialidadId
        };

        private Plan MapToEntityForCreation(PlanDto dto) => new Plan(
            0, // El ID será asignado por la base de datos
            dto.Descripcion,
            dto.EspecialidadId
        );

        private Plan MapToEntityForUpdate(PlanDto dto) => new Plan(
            dto.Id,
            dto.Descripcion,
            dto.EspecialidadId
        );



    }
}
