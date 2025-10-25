using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class DocenteCursoRepository
    {
        private readonly AcademiaContext _context;

        public DocenteCursoRepository(AcademiaContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Obtener todas las asignaciones con información completa
        public async Task<IEnumerable<DocenteCurso>> GetAllAsync()
        {
            return await _context.DocenteCursos
                .Include(dc => dc.Curso)
                .Include(dc => dc.Docente)
                .ToListAsync();
        }

        // Obtener asignación por ID
        public async Task<DocenteCurso?> GetByIdAsync(int id)
        {
            return await _context.DocenteCursos
                .Include(dc => dc.Curso)
                .Include(dc => dc.Docente)
                .FirstOrDefaultAsync(dc => dc.IdDictado == id);
        }

        // Obtener docentes por curso
        public async Task<IEnumerable<DocenteCurso>> GetByCursoIdAsync(int cursoId)
        {
            return await _context.DocenteCursos
                .Include(dc => dc.Docente)
                .Where(dc => dc.IdCurso == cursoId)
                .OrderBy(dc => dc.Cargo)
                .ToListAsync();
        }

        // Obtener cursos por docente
        public async Task<IEnumerable<DocenteCurso>> GetByDocenteIdAsync(int docenteId)
        {
            return await _context.DocenteCursos
                .Include(dc => dc.Curso)
                .Where(dc => dc.IdDocente == docenteId)
                .ToListAsync();
        }

        // Crear nueva asignación
        public async Task<DocenteCurso> CreateAsync(DocenteCurso docenteCurso)
        {
            if (docenteCurso == null)
                throw new ArgumentNullException(nameof(docenteCurso));

            // Validar que el docente existe y es de tipo Profesor
            var docente = await _context.Personas.FindAsync(docenteCurso.IdDocente);
            if (docente == null)
                throw new InvalidOperationException("El docente especificado no existe.");
            if (docente.TipoPersona != TipoPersona.Profesor)
                throw new InvalidOperationException("La persona seleccionada no es un profesor.");

            // Validar que el curso existe
            var curso = await _context.Cursos.FindAsync(docenteCurso.IdCurso);
            if (curso == null)
                throw new InvalidOperationException("El curso especificado no existe.");

            // Validar que no existe ya esta combinación curso-docente-cargo
            var existente = await _context.DocenteCursos
                .FirstOrDefaultAsync(dc => dc.IdCurso == docenteCurso.IdCurso 
                    && dc.IdDocente == docenteCurso.IdDocente 
                    && dc.Cargo == docenteCurso.Cargo);

            if (existente != null)
                throw new InvalidOperationException(
                    $"El profesor ya está asignado a este curso con el cargo de {docenteCurso.Cargo}.");

            _context.DocenteCursos.Add(docenteCurso);
            await _context.SaveChangesAsync();
            return docenteCurso;
        }

        // Actualizar asignación
        public async Task<DocenteCurso> UpdateAsync(DocenteCurso docenteCurso)
        {
            if (docenteCurso == null)
                throw new ArgumentNullException(nameof(docenteCurso));

            var existente = await _context.DocenteCursos.FindAsync(docenteCurso.IdDictado);
            if (existente == null)
                throw new InvalidOperationException("La asignación no existe.");

            // Validar duplicados si se cambió el cargo
            if (existente.Cargo != docenteCurso.Cargo)
            {
                var duplicado = await _context.DocenteCursos
                    .FirstOrDefaultAsync(dc => dc.IdCurso == docenteCurso.IdCurso 
                        && dc.IdDocente == docenteCurso.IdDocente 
                        && dc.Cargo == docenteCurso.Cargo
                        && dc.IdDictado != docenteCurso.IdDictado);

                if (duplicado != null)
                    throw new InvalidOperationException(
                        $"El profesor ya está asignado a este curso con el cargo de {docenteCurso.Cargo}.");
            }

            existente.IdCurso = docenteCurso.IdCurso;
            existente.IdDocente = docenteCurso.IdDocente;
            existente.Cargo = docenteCurso.Cargo;

            await _context.SaveChangesAsync();
            return existente;
        }

        // Eliminar asignación
        public async Task<bool> DeleteAsync(int id)
        {
            var docenteCurso = await _context.DocenteCursos.FindAsync(id);
            if (docenteCurso == null)
                return false;

            _context.DocenteCursos.Remove(docenteCurso);
            await _context.SaveChangesAsync();
            return true;
        }

        // Verificar si un docente está asignado a un curso
        public async Task<bool> ExistsAsync(int cursoId, int docenteId, TipoCargo cargo)
        {
            return await _context.DocenteCursos
                .AnyAsync(dc => dc.IdCurso == cursoId 
                    && dc.IdDocente == docenteId 
                    && dc.Cargo == cargo);
        }

        // Obtener cantidad de docentes por curso
        public async Task<int> GetDocentesCountByCursoAsync(int cursoId)
        {
            return await _context.DocenteCursos
                .Where(dc => dc.IdCurso == cursoId)
                .CountAsync();
        }
    }
}
