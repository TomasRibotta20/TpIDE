using Data;
using Domain.Model;
using DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Aplication.Services
{
    public class PersonaService
    {
        private readonly PersonaRepository _repository;

        public PersonaService()
        {
            _repository = new PersonaRepository();
        }

        public IEnumerable<PersonaDto> GetAllAlumnos()
        {
            return _repository.GetAlumnos().Select(MapToDto);
        }

        public IEnumerable<PersonaDto> GetAllProfesores()
        {
            return _repository.GetProfesores().Select(MapToDto);
        }

        public PersonaDto GetById(int id)
        {
            var persona = _repository.GetById(id);
            return persona == null ? null : MapToDto(persona);
        }

        public void Add(PersonaDto personaDto)
        {
            var persona = MapToEntityForCreation(personaDto);
            _repository.Add(persona);
        }

        public void Update(PersonaDto personaDto)
        {
            var persona = MapToEntityForUpdate(personaDto);
            _repository.Update(persona);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        private PersonaDto MapToDto(Persona persona) => new PersonaDto
        {
            Id = persona.Id,
            Nombre = persona.Nombre,
            Apellido = persona.Apellido,
            Direccion = persona.Direccion,
            Email = persona.Email,
            Telefono = persona.Telefono,
            FechaNacimiento = persona.FechaNacimiento,
            Legajo = persona.Legajo,
            TipoPersona = (TipoPersonaDto)persona.TipoPersona,
            IdPlan = persona.IdPlan
        };

        private Persona MapToEntityForCreation(PersonaDto dto) => new Persona(
            dto.Nombre,
            dto.Apellido,
            dto.Direccion,
            dto.Email,
            dto.Telefono,
            dto.FechaNacimiento,
            dto.Legajo,
            (TipoPersona)dto.TipoPersona,
            dto.IdPlan
        );

        private Persona MapToEntityForUpdate(PersonaDto dto) => new Persona(
            dto.Id,
            dto.Nombre,
            dto.Apellido,
            dto.Direccion,
            dto.Email,
            dto.Telefono,
            dto.FechaNacimiento,
            dto.Legajo,
            (TipoPersona)dto.TipoPersona,
            dto.IdPlan
        );
    }
}