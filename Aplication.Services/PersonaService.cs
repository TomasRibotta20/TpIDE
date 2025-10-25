using Data;
using Domain.Model;
using DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class PersonaService
    {
        private readonly PersonaRepository _repository;

        public PersonaService()
        {
            _repository = new PersonaRepository();
        }

        /// <summary>
        /// Obtiene todas las personas (alumnos y profesores)
        /// </summary>
        public async Task<IEnumerable<PersonaDto>> GetAllAsync()
        {
            var personas = await _repository.GetAllAsync();
            return personas.Select(MapToDto);
        }

        public async Task<IEnumerable<PersonaDto>> GetAllAlumnosAsync()
        {
            var alumnos = await _repository.GetAlumnosAsync();
            return alumnos.Select(MapToDto);
        }

        public async Task<IEnumerable<PersonaDto>> GetAllProfesoresAsync()
        {
            var profesores = await _repository.GetProfesoresAsync();
            return profesores.Select(MapToDto);
        }

        public async Task<PersonaDto?> GetByIdAsync(int id)
        {
            var persona = await _repository.GetByIdAsync(id);
            return persona == null ? null : MapToDto(persona);
        }

        public async Task AddAsync(PersonaDto personaDto)
        {
            var persona = MapToEntityForCreation(personaDto);
            await _repository.AddAsync(persona);
        }

        public async Task UpdateAsync(PersonaDto personaDto)
        {
            var persona = MapToEntityForUpdate(personaDto);
            await _repository.UpdateAsync(persona);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
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