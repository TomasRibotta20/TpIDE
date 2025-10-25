using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Model;

namespace Data
{
    public class CursoRepository
    {
        private readonly AcademiaContext _context;

        public CursoRepository()
        {
            _context = new AcademiaContext();
        }

        public async Task<IEnumerable<Curso>> GetAllAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task<Curso?> GetByIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
        }

        public async Task<Curso> CreateAsync(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task UpdateAsync(Curso curso)
        {
            _context.Entry(curso).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Curso>> GetByComisionAsync(int idComision)
        {
            return await _context.Cursos
                .Where(c => c.IdComision == idComision)
                .ToListAsync();
        }

        public async Task<IEnumerable<Curso>> GetByAnioCalendarioAsync(int anioCalendario)
        {
            return await _context.Cursos
                .Where(c => c.AnioCalendario == anioCalendario)
                .ToListAsync();
        }

        public async Task<int> GetInscriptosCountAsync(int idCurso)
        {
            return await _context.AlumnoCursos
                .Where(ac => ac.IdCurso == idCurso)
                .CountAsync();
        }
    }
}