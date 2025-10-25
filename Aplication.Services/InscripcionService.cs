using DTOs;
using Domain.Model;
using Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Aplication.Services
{
    public class InscripcionService
    {
        private readonly AlumnoCursoRepository _inscripcionRepository;
        private readonly PersonaRepository _personaRepository;
        private readonly CursoRepository _cursoRepository;
        private readonly ComisionRepository _comisionRepository;

        public InscripcionService()
        {
            _inscripcionRepository = new AlumnoCursoRepository();
            _personaRepository = new PersonaRepository();
            _cursoRepository = new CursoRepository();
            _comisionRepository = new ComisionRepository();
        }

        // Método auxiliar para generar descripción del curso más informativa
        private async Task<string> GenerarDescripcionCursoAsync(Curso? curso)
        {
            if (curso == null)
                return "Curso no encontrado";

            var descripcion = $"Curso {curso.IdCurso}";
            
            // Intentar obtener la descripción de la comisión
            try
            {
                var comision = await _comisionRepository.GetByIdAsync(curso.IdComision);
                
                if (comision != null && !string.IsNullOrEmpty(comision.DescComision))
                {
                    descripcion += $" - {comision.DescComision}";
                }
                else
                {
                    descripcion += $" - Comisión {curso.IdComision}";
                }
            }
            catch
            {
                descripcion += $" - Comisión {curso.IdComision}";
            }
            
            descripcion += $" ({curso.AnioCalendario})";
            
            return descripcion;
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetAllAsync()
        {
            var inscripciones = await _inscripcionRepository.GetAllAsync();
            var result = new List<AlumnoCursoDto>();

            foreach (var inscripcion in inscripciones)
            {
                var alumno = await _personaRepository.GetByIdAsync(inscripcion.IdAlumno);
                var curso = await _cursoRepository.GetByIdAsync(inscripcion.IdCurso);

                result.Add(new AlumnoCursoDto
                {
                    IdInscripcion = inscripcion.IdInscripcion,
                    IdAlumno = inscripcion.IdAlumno,
                    NombreAlumno = alumno?.Nombre,
                    ApellidoAlumno = alumno?.Apellido,
                    LegajoAlumno = alumno?.Legajo,
                    IdCurso = inscripcion.IdCurso,
                    DescripcionCurso = await GenerarDescripcionCursoAsync(curso),
                    Condicion = (CondicionAlumnoDto)inscripcion.Condicion,
                    Nota = inscripcion.Nota,
                    FechaInscripcion = DateTime.Now // Temporal - en futuro agregar campo a BD
                });
            }

            return result;
        }

        public async Task<AlumnoCursoDto?> GetByIdAsync(int id)
        {
            var inscripcion = await _inscripcionRepository.GetByIdAsync(id);
            if (inscripcion == null) return null;

            var alumno = await _personaRepository.GetByIdAsync(inscripcion.IdAlumno);
            var curso = await _cursoRepository.GetByIdAsync(inscripcion.IdCurso);

            return new AlumnoCursoDto
            {
                IdInscripcion = inscripcion.IdInscripcion,
                IdAlumno = inscripcion.IdAlumno,
                NombreAlumno = alumno?.Nombre,
                ApellidoAlumno = alumno?.Apellido,
                LegajoAlumno = alumno?.Legajo,
                IdCurso = inscripcion.IdCurso,
                DescripcionCurso = await GenerarDescripcionCursoAsync(curso),
                Condicion = (CondicionAlumnoDto)inscripcion.Condicion,
                Nota = inscripcion.Nota,
                FechaInscripcion = DateTime.Now
            };
        }

        public async Task<AlumnoCursoDto> InscribirAlumnoAsync(int idAlumno, int idCurso, CondicionAlumnoDto condicion = CondicionAlumnoDto.Regular)
        {
            // VALIDACIONES DE NEGOCIO
            await ValidarInscripcionAsync(idAlumno, idCurso);

            var inscripcion = new AlumnoCurso(
                idAlumno,
                idCurso,
                (CondicionAlumno)condicion
            );

            var inscripcionCreada = await _inscripcionRepository.CreateAsync(inscripcion);
            return await GetByIdAsync(inscripcionCreada.IdInscripcion) ?? throw new Exception("Error al crear la inscripción");
        }

        public async Task ActualizarCondicionYNotaAsync(int idInscripcion, CondicionAlumnoDto condicion, int? nota = null)
        {
            var inscripcion = await _inscripcionRepository.GetByIdAsync(idInscripcion);
            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            inscripcion.SetCondicion((CondicionAlumno)condicion);
            if (nota.HasValue)
                inscripcion.SetNota(nota.Value);

            await _inscripcionRepository.UpdateAsync(inscripcion);
        }

        public async Task DesinscribirAlumnoAsync(int idInscripcion)
        {
            await _inscripcionRepository.DeleteAsync(idInscripcion);
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetInscripcionesByAlumnoAsync(int idAlumno)
        {
            var inscripciones = await _inscripcionRepository.GetByAlumnoAsync(idAlumno);
            var result = new List<AlumnoCursoDto>();

            foreach (var inscripcion in inscripciones)
            {
                var alumno = await _personaRepository.GetByIdAsync(inscripcion.IdAlumno);
                var curso = await _cursoRepository.GetByIdAsync(inscripcion.IdCurso);

                result.Add(new AlumnoCursoDto
                {
                    IdInscripcion = inscripcion.IdInscripcion,
                    IdAlumno = inscripcion.IdAlumno,
                    NombreAlumno = alumno?.Nombre,
                    ApellidoAlumno = alumno?.Apellido,
                    LegajoAlumno = alumno?.Legajo,
                    IdCurso = inscripcion.IdCurso,
                    DescripcionCurso = await GenerarDescripcionCursoAsync(curso),
                    Condicion = (CondicionAlumnoDto)inscripcion.Condicion,
                    Nota = inscripcion.Nota,
                    FechaInscripcion = DateTime.Now
                });
            }

            return result;
        }

        public async Task<IEnumerable<AlumnoCursoDto>> GetInscripcionesByCursoAsync(int idCurso)
        {
            var inscripciones = await _inscripcionRepository.GetByCursoAsync(idCurso);
            var result = new List<AlumnoCursoDto>();

            foreach (var inscripcion in inscripciones)
            {
                var alumno = await _personaRepository.GetByIdAsync(inscripcion.IdAlumno);
                var curso = await _cursoRepository.GetByIdAsync(inscripcion.IdCurso);

                result.Add(new AlumnoCursoDto
                {
                    IdInscripcion = inscripcion.IdInscripcion,
                    IdAlumno = inscripcion.IdAlumno,
                    NombreAlumno = alumno?.Nombre,
                    ApellidoAlumno = alumno?.Apellido,
                    LegajoAlumno = alumno?.Legajo,
                    IdCurso = inscripcion.IdCurso,
                    DescripcionCurso = await GenerarDescripcionCursoAsync(curso),
                    Condicion = (CondicionAlumnoDto)inscripcion.Condicion,
                    Nota = inscripcion.Nota,
                    FechaInscripcion = DateTime.Now
                });
            }

            return result;
        }

        // VALIDACIONES DE NEGOCIO PRIVADAS
        private async Task ValidarInscripcionAsync(int idAlumno, int idCurso)
        {
            // 1. Verificar que el alumno existe y es válido
            var alumno = await _personaRepository.GetByIdAsync(idAlumno);
            if (alumno == null)
                throw new Exception("El alumno especificado no existe en el sistema");

            if (alumno.TipoPersona != TipoPersona.Alumno)
                throw new Exception("La persona especificada no es un alumno válido");

            // 2. Verificar que el curso existe
            var curso = await _cursoRepository.GetByIdAsync(idCurso);
            if (curso == null)
                throw new Exception("El curso especificado no existe en el sistema");

            // 3. Verificar que no esté ya inscripto (VALIDACIÓN MEJORADA)
            var yaInscripto = await _inscripcionRepository.ExistsInscripcionAsync(idAlumno, idCurso);
            if (yaInscripto)
                throw new Exception($"El alumno {alumno.Nombre} {alumno.Apellido} ya está inscripto en este curso. No se permite inscripción duplicada.");

            // 4. Verificar cupo disponible
            var inscriptosActuales = await _cursoRepository.GetInscriptosCountAsync(idCurso);
            if (inscriptosActuales >= curso.Cupo)
                throw new Exception($"No hay cupo disponible en este curso. Está completo ({inscriptosActuales}/{curso.Cupo} inscriptos). Seleccione otro curso.");

            // 5. Verificar que el curso sea del año actual o futuro
            int anioActual = DateTime.Now.Year;
            if (curso.AnioCalendario < anioActual)
                throw new Exception($"No se puede inscribir a cursos de años anteriores. El curso es del año {curso.AnioCalendario} y el año actual es {anioActual}.");

            // FUTURAS VALIDACIONES (para cuando tengamos Materias):
            // - Verificar correlativas
            // - Verificar que la materia pertenezca al plan del alumno
            // - Verificar límite de materias por cuatrimestre
        }

        public async Task<Dictionary<string, int>> GetEstadisticasGeneralesAsync()
        {
            var todasInscripciones = await _inscripcionRepository.GetAllAsync();
            
            return new Dictionary<string, int>
            {
                ["TotalInscripciones"] = todasInscripciones.Count(),
                ["AlumnosLibres"] = todasInscripciones.Count(i => i.Condicion == CondicionAlumno.Libre),
                ["AlumnosRegulares"] = todasInscripciones.Count(i => i.Condicion == CondicionAlumno.Regular),
                ["AlumnosPromocionales"] = todasInscripciones.Count(i => i.Condicion == CondicionAlumno.Promocional)
            };
        }
    }
}