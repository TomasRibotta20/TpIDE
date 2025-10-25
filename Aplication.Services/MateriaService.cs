
using Data;
using Domain.Model;
using DTOs;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Aplication.Services
{
    public class MateriaService
    {
        private readonly MateriaRepository _repository;
        private readonly PlanService _planService; // Asume que PlanService está disponible

        // Constructor para permitir la instanciación manual o por DI
        public MateriaService(MateriaRepository repository, PlanService planService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _planService = planService ?? throw new ArgumentNullException(nameof(planService));
        }

        // Métodos de servicio (Solo estructura - Debes implementar la lógica)
        public IEnumerable<MateriaDto> GetAll()
        {
            // 1. Obtener materias del repositorio
            var materias = _repository.GetAll();
            // 2. Obtener planes para mapear descripción (optimizar si son muchos)
            var planes = _planService.GetAll();
            // 3. Mapear a DTO y añadir descripción del plan
            return materias.Select(m => {
                var dto = MapToDto(m);
                dto.DescripcionPlan = planes.FirstOrDefault(p => p.Id == m.IdPlan)?.Descripcion ?? "N/A";
                return dto;
            });
        }

        public MateriaDto? GetById(int id)
        {
            var materia = _repository.GetById(id);
            if (materia == null) return null;
            var dto = MapToDto(materia);
            var plan = _planService.GetById(materia.IdPlan);
            dto.DescripcionPlan = plan?.Descripcion ?? "N/A";
            return dto;
        }

        public void Add(MateriaDto materiaDto)
        {
            // Validar que el Plan exista
            if (_planService.GetById(materiaDto.IdPlan) == null)
            {
                throw new ArgumentException($"El Plan con ID {materiaDto.IdPlan} no existe.");
            }
            var materia = MapToEntityForCreation(materiaDto);
            // La validación de dominio ocurre en el constructor/setters de Materia
            _repository.Add(materia);
        }

        public void Update(MateriaDto materiaDto)
        {
            // Validar que la materia exista
            if (_repository.GetById(materiaDto.Id) == null)
            {
                throw new KeyNotFoundException($"Materia con ID {materiaDto.Id} no encontrada.");
            }
            // Validar que el Plan exista
            if (_planService.GetById(materiaDto.IdPlan) == null)
            {
                throw new ArgumentException($"El Plan con ID {materiaDto.IdPlan} no existe.");
            }
            var materia = MapToEntityForUpdate(materiaDto);
            _repository.Update(materia);
        }

        public void Delete(int id)
        {
            // Validar que la materia exista antes de eliminar
            if (_repository.GetById(id) == null)
            {
                throw new KeyNotFoundException($"Materia con ID {id} no encontrada.");
            }
            _repository.Delete(id);
        }

        // --- Métodos de Mapeo ---
        private MateriaDto MapToDto(Materia materia) => new MateriaDto
        {
            Id = materia.Id,
            Descripcion = materia.Descripcion,
            HorasSemanales = materia.HorasSemanales,
            HorasTotales = materia.HorasTotales,
            IdPlan = materia.IdPlan
            // DescripcionPlan se asigna en GetAll/GetById
        };

        private Materia MapToEntityForCreation(MateriaDto dto) => new Materia(
            0, // ID 0 para que la base de datos lo asigne
            dto.Descripcion,
            dto.HorasSemanales,
            dto.HorasTotales,
            dto.IdPlan
        );

        private Materia MapToEntityForUpdate(MateriaDto dto) => new Materia(
            dto.Id,
            dto.Descripcion,
            dto.HorasSemanales,
            dto.HorasTotales,
            dto.IdPlan
        );
    }
}