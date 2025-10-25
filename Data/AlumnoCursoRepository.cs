using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Model;

namespace Data
{
    public class AlumnoCursoRepository
    {
        private readonly AcademiaContext _context;

        public AlumnoCursoRepository()
        {
            _context = new AcademiaContext();
        }

        public async Task<IEnumerable<AlumnoCurso>> GetAllAsync()
        {
            return await _context.AlumnoCursos.ToListAsync();
        }

        public async Task<AlumnoCurso?> GetByIdAsync(int id)
        {
            return await _context.AlumnoCursos.FindAsync(id);
        }

        public async Task<AlumnoCurso> CreateAsync(AlumnoCurso alumnoCurso)
        {
            _context.AlumnoCursos.Add(alumnoCurso);
            await _context.SaveChangesAsync();
            return alumnoCurso;
        }

        public async Task UpdateAsync(AlumnoCurso alumnoCurso)
        {
            _context.Entry(alumnoCurso).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alumnoCurso = await _context.AlumnoCursos.FindAsync(id);
            if (alumnoCurso != null)
            {
                _context.AlumnoCursos.Remove(alumnoCurso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AlumnoCurso>> GetByAlumnoAsync(int idAlumno)
        {
            return await _context.AlumnoCursos
                .Where(ac => ac.IdAlumno == idAlumno)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlumnoCurso>> GetByCursoAsync(int idCurso)
        {
            return await _context.AlumnoCursos
                .Where(ac => ac.IdCurso == idCurso)
                .ToListAsync();
        }

        public async Task<AlumnoCurso?> GetByAlumnoAndCursoAsync(int idAlumno, int idCurso)
        {
            return await _context.AlumnoCursos
                .FirstOrDefaultAsync(ac => ac.IdAlumno == idAlumno && ac.IdCurso == idCurso);
        }

        public async Task<bool> ExistsInscripcionAsync(int idAlumno, int idCurso)
        {
            return await _context.AlumnoCursos
                .AnyAsync(ac => ac.IdAlumno == idAlumno && ac.IdCurso == idCurso);
        }
    }
}