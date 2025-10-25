using Domain.Model;
using Data;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DocenteCursoService
    {
        private readonly DocenteCursoRepository _docenteCursoRepository;
        private readonly CursoRepository _cursoRepository;
        private readonly PersonaRepository _personaRepository;
        private readonly MateriaRepository _materiaRepository;
        private readonly ComisionRepository _comisionRepository;

        public DocenteCursoService(
            DocenteCursoRepository docenteCursoRepository,
            CursoRepository cursoRepository,
            PersonaRepository personaRepository,
            MateriaRepository materiaRepository,
            ComisionRepository comisionRepository)
        {
            _docenteCursoRepository = docenteCursoRepository ?? throw new ArgumentNullException(nameof(docenteCursoRepository));
            _cursoRepository = cursoRepository ?? throw new ArgumentNullException(nameof(cursoRepository));
            _personaRepository = personaRepository ?? throw new ArgumentNullException(nameof(personaRepository));
            _materiaRepository = materiaRepository ?? throw new ArgumentNullException(nameof(materiaRepository));
            _comisionRepository = comisionRepository ?? throw new ArgumentNullException(nameof(comisionRepository));
        }

        // Constructor sin parámetros para facilitar instanciación
        public DocenteCursoService()
        {
            var context = new AcademiaContext();
            _docenteCursoRepository = new DocenteCursoRepository(context);
            _cursoRepository = new CursoRepository();
            _personaRepository = new PersonaRepository();
            _materiaRepository = new MateriaRepository("Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True");
            _comisionRepository = new ComisionRepository();
        }

        // Obtener todas las asignaciones con información completa
        public async Task<IEnumerable<DocenteCursoDto>> GetAllAsync()
        {
            var asignaciones = await _docenteCursoRepository.GetAllAsync();
            var dtos = new List<DocenteCursoDto>();

            foreach (var asignacion in asignaciones)
            {
                dtos.Add(await MapToDto(asignacion));
            }

            return dtos;
        }

        // Obtener asignación por ID
        public async Task<DocenteCursoDto?> GetByIdAsync(int id)
        {
            var asignacion = await _docenteCursoRepository.GetByIdAsync(id);
            return asignacion != null ? await MapToDto(asignacion) : null;
        }

        // Obtener docentes asignados a un curso
        public async Task<IEnumerable<DocenteCursoDto>> GetByCursoIdAsync(int cursoId)
        {
            var asignaciones = await _docenteCursoRepository.GetByCursoIdAsync(cursoId);
            var dtos = new List<DocenteCursoDto>();

            foreach (var asignacion in asignaciones)
            {
                dtos.Add(await MapToDto(asignacion));
            }

            return dtos;
        }

        // Obtener cursos asignados a un docente
        public async Task<IEnumerable<DocenteCursoDto>> GetByDocenteIdAsync(int docenteId)
        {
            var asignaciones = await _docenteCursoRepository.GetByDocenteIdAsync(docenteId);
            var dtos = new List<DocenteCursoDto>();

            foreach (var asignacion in asignaciones)
            {
                dtos.Add(await MapToDto(asignacion));
            }

            return dtos;
        }

        // Crear nueva asignación
        public async Task<DocenteCursoDto> CreateAsync(DocenteCursoCreateDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            var cargo = MapCargo(createDto.Cargo);
            var docenteCurso = new DocenteCurso(
                createDto.IdCurso,
                createDto.IdDocente,
                cargo
            );

            var creado = await _docenteCursoRepository.CreateAsync(docenteCurso);
            return await MapToDto(creado);
        }

        // Actualizar asignación
        public async Task<DocenteCursoDto> UpdateAsync(int id, DocenteCursoCreateDto updateDto)
        {
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            var existente = await _docenteCursoRepository.GetByIdAsync(id);
            if (existente == null)
                throw new InvalidOperationException("La asignación no existe.");

            var cargo = MapCargo(updateDto.Cargo);
            var actualizado = new DocenteCurso(
                id,
                updateDto.IdCurso,
                updateDto.IdDocente,
                cargo
            );

            var resultado = await _docenteCursoRepository.UpdateAsync(actualizado);
            return await MapToDto(resultado);
        }

        // Eliminar asignación
        public async Task<bool> DeleteAsync(int id)
        {
            return await _docenteCursoRepository.DeleteAsync(id);
        }

        // Verificar si un docente puede ser asignado a un curso
        public async Task<bool> CanAssignAsync(int cursoId, int docenteId, TipoCargoDto cargo)
        {
            var cargoModel = MapCargo(cargo);
            return !await _docenteCursoRepository.ExistsAsync(cursoId, docenteId, cargoModel);
        }

        // Métodos auxiliares de mapeo
        private async Task<DocenteCursoDto> MapToDto(DocenteCurso docenteCurso)
        {
            var docente = docenteCurso.Docente ?? await _personaRepository.GetByIdAsync(docenteCurso.IdDocente);
            var curso = docenteCurso.Curso ?? await _cursoRepository.GetByIdAsync(docenteCurso.IdCurso);

            string? nombreMateria = null;
            string? descComision = null;

            if (curso != null)
            {
                if (curso.IdMateria.HasValue)
                {
                    var materia = await _materiaRepository.GetByIdAsync(curso.IdMateria.Value);
                    nombreMateria = materia?.Descripcion;
                }

                var comision = await _comisionRepository.GetByIdAsync(curso.IdComision);
                descComision = comision?.DescComision;
            }

            return new DocenteCursoDto
            {
                IdDictado = docenteCurso.IdDictado,
                IdCurso = docenteCurso.IdCurso,
                IdDocente = docenteCurso.IdDocente,
                Cargo = MapCargoDto(docenteCurso.Cargo),
                NombreDocente = docente?.Nombre,
                ApellidoDocente = docente?.Apellido,
                NombreCurso = $"Curso {docenteCurso.IdCurso}",
                NombreMateria = nombreMateria,
                DescComision = descComision,
                AnioCalendario = curso?.AnioCalendario
            };
        }

        private TipoCargo MapCargo(TipoCargoDto cargoDto)
        {
            return cargoDto switch
            {
                TipoCargoDto.JefeDeCatedra => TipoCargo.JefeDeCatedra,
                TipoCargoDto.Titular => TipoCargo.Titular,
                TipoCargoDto.Auxiliar => TipoCargo.Auxiliar,
                _ => throw new ArgumentException("Cargo inválido", nameof(cargoDto))
            };
        }

        private TipoCargoDto MapCargoDto(TipoCargo cargo)
        {
            return cargo switch
            {
                TipoCargo.JefeDeCatedra => TipoCargoDto.JefeDeCatedra,
                TipoCargo.Titular => TipoCargoDto.Titular,
                TipoCargo.Auxiliar => TipoCargoDto.Auxiliar,
                _ => throw new ArgumentException("Cargo inválido", nameof(cargo))
            };
        }
    }
}
