using DTOs;
using Domain.Model;
using Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Aplication.Services
{
    public class CursoService
    {
        private readonly CursoRepository _cursoRepository;
        private readonly ComisionRepository _comisionRepository;
        private readonly MateriaRepository _materiaRepository;

        public CursoService()
        {
            _cursoRepository = new CursoRepository();
            _comisionRepository = new ComisionRepository();
            var connectionString = "Server=localhost,1433;Database=Universidad;User Id=sa;Password=TuContraseñaFuerte123;TrustServerCertificate=True";
            _materiaRepository = new MateriaRepository(connectionString);
        }

        public async Task<IEnumerable<CursoDto>> GetAllAsync()
        {
            var cursos = await _cursoRepository.GetAllAsync();
            var result = new List<CursoDto>();
            var materiasInvalidas = new List<int>();

            foreach (var curso in cursos)
            {
                var comision = await _comisionRepository.GetByIdAsync(curso.IdComision);
                var inscriptos = await _cursoRepository.GetInscriptosCountAsync(curso.IdCurso);
                
                var materia = _materiaRepository.GetById(curso.IdMateria);
                if (materia == null)
                {
                    materiasInvalidas.Add(curso.IdCurso);
                    continue; // Omitir cursos con materias inválidas
                }

                result.Add(new CursoDto
                {
                    IdCurso = curso.IdCurso,
                    IdMateria = curso.IdMateria,
                    NombreMateria = materia.Descripcion,
                    IdComision = curso.IdComision,
                    DescComision = comision?.DescComision ?? "Comisión no encontrada",
                    AnioCalendario = curso.AnioCalendario,
                    Cupo = curso.Cupo,
                    InscriptosActuales = inscriptos
                });
            }

            if (materiasInvalidas.Any())
            {
                // Log warning about invalid courses
                System.Diagnostics.Debug.WriteLine($"Se encontraron cursos con materias inválidas: {string.Join(", ", materiasInvalidas)}");
            }

            return result.OrderBy(c => c.NombreMateria)
                        .ThenBy(c => c.DescComision)
                        .ThenBy(c => c.AnioCalendario);
        }

        public async Task<CursoDto?> GetByIdAsync(int id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso == null) return null;

            var comision = await _comisionRepository.GetByIdAsync(curso.IdComision);
            var inscriptos = await _cursoRepository.GetInscriptosCountAsync(curso.IdCurso);
            
            var materia = _materiaRepository.GetById(curso.IdMateria);
            if (materia == null)
                throw new Exception($"El curso {id} hace referencia a una materia inválida (ID: {curso.IdMateria})");

            return new CursoDto
            {
                IdCurso = curso.IdCurso,
                IdMateria = curso.IdMateria,
                NombreMateria = materia.Descripcion,
                IdComision = curso.IdComision,
                DescComision = comision?.DescComision ?? "Comisión no encontrada",
                AnioCalendario = curso.AnioCalendario,
                Cupo = curso.Cupo,
                InscriptosActuales = inscriptos
            };
        }

        public async Task<CursoDto> CreateAsync(CursoDto cursoDto)
        {
            // Validar que la materia exista
            var materia = _materiaRepository.GetById(cursoDto.IdMateria);
            if (materia == null)
                throw new Exception($"La materia con ID {cursoDto.IdMateria} no existe");

            await ValidateComisionExistsAsync(cursoDto.IdComision);
            ValidateAnioCalendario(cursoDto.AnioCalendario);
            ValidateCupo(cursoDto.Cupo);

            var curso = new Curso(
                cursoDto.IdMateria,
                cursoDto.IdComision,
                cursoDto.AnioCalendario,
                cursoDto.Cupo
            );

            var createdCurso = await _cursoRepository.CreateAsync(curso);
            return await GetByIdAsync(createdCurso.IdCurso) ?? 
                throw new Exception("Error al crear el curso");
        }

        public async Task UpdateAsync(CursoDto cursoDto)
        {
            var curso = await _cursoRepository.GetByIdAsync(cursoDto.IdCurso);
            if (curso == null)
                throw new Exception($"No se encontró el curso con ID {cursoDto.IdCurso}");

            // Validar que la materia exista
            var materia = _materiaRepository.GetById(cursoDto.IdMateria);
            if (materia == null)
                throw new Exception($"La materia con ID {cursoDto.IdMateria} no existe");

            await ValidateComisionExistsAsync(cursoDto.IdComision);
            ValidateAnioCalendario(cursoDto.AnioCalendario);
            ValidateCupo(cursoDto.Cupo);

            var inscriptosActuales = await _cursoRepository.GetInscriptosCountAsync(cursoDto.IdCurso);
            if (cursoDto.Cupo < inscriptosActuales)
                throw new Exception($"No se puede reducir el cupo a {cursoDto.Cupo} porque ya hay {inscriptosActuales} estudiantes inscriptos");

            curso.SetIdMateria(cursoDto.IdMateria);
            curso.SetIdComision(cursoDto.IdComision);
            curso.SetAnioCalendario(cursoDto.AnioCalendario);
            curso.SetCupo(cursoDto.Cupo);

            await _cursoRepository.UpdateAsync(curso);
        }

        public async Task DeleteAsync(int id)
        {
            // Verificar que no tenga inscripciones antes de eliminar
            var inscriptos = await _cursoRepository.GetInscriptosCountAsync(id);
            if (inscriptos > 0)
                throw new Exception($"No se puede eliminar el curso porque tiene {inscriptos} estudiantes inscriptos");

            await _cursoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CursoDto>> GetByComisionAsync(int idComision)
        {
            var cursos = await _cursoRepository.GetByComisionAsync(idComision);
            var result = new List<CursoDto>();

            foreach (var curso in cursos)
            {
                var comision = await _comisionRepository.GetByIdAsync(curso.IdComision);
                var inscriptos = await _cursoRepository.GetInscriptosCountAsync(curso.IdCurso);

                // Obtener nombre real de la materia (ahora siempre existe)
                var materia = _materiaRepository.GetById(curso.IdMateria);
                string nombreMateria = materia?.Descripcion ?? $"Materia ID {curso.IdMateria}";

                result.Add(new CursoDto
                {
                    IdCurso = curso.IdCurso,
                    IdMateria = curso.IdMateria,
                    NombreMateria = nombreMateria,
                    IdComision = curso.IdComision,
                    DescComision = comision?.DescComision ?? "Comisión no encontrada",
                    AnioCalendario = curso.AnioCalendario,
                    Cupo = curso.Cupo,
                    InscriptosActuales = inscriptos
                });
            }

            return result;
        }

        public async Task<IEnumerable<CursoDto>> GetByAnioCalendarioAsync(int anioCalendario)
        {
            var cursos = await _cursoRepository.GetByAnioCalendarioAsync(anioCalendario);
            var result = new List<CursoDto>();

            foreach (var curso in cursos)
            {
                var comision = await _comisionRepository.GetByIdAsync(curso.IdComision);
                var inscriptos = await _cursoRepository.GetInscriptosCountAsync(curso.IdCurso);

                // Obtener nombre real de la materia (ahora siempre existe)
                var materia = _materiaRepository.GetById(curso.IdMateria);
                string nombreMateria = materia?.Descripcion ?? $"Materia ID {curso.IdMateria}";

                result.Add(new CursoDto
                {
                    IdCurso = curso.IdCurso,
                    IdMateria = curso.IdMateria,
                    NombreMateria = nombreMateria,
                    IdComision = curso.IdComision,
                    DescComision = comision?.DescComision ?? "Comisión no encontrada",
                    AnioCalendario = curso.AnioCalendario,
                    Cupo = curso.Cupo,
                    InscriptosActuales = inscriptos
                });
            }

            return result;
        }

        // Métodos de validación privados
        private async Task ValidateComisionExistsAsync(int idComision)
        {
            var comision = await _comisionRepository.GetByIdAsync(idComision);
            if (comision == null)
                throw new Exception($"La comisión con ID {idComision} no existe");
        }

        private void ValidateAnioCalendario(int anioCalendario)
        {
            int currentYear = DateTime.Now.Year;
            if (anioCalendario < 2000 || anioCalendario > currentYear + 5)
                throw new Exception($"El año calendario debe estar entre 2000 y {currentYear + 5}");
        }

        private void ValidateCupo(int cupo)
        {
            if (cupo <= 0)
                throw new Exception("El cupo debe ser mayor que cero");
            if (cupo > 100)
                throw new Exception("El cupo no puede ser mayor a 100 estudiantes");
        }
    }
}