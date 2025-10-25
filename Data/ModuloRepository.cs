using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ModuloRepository
    {
        private readonly AcademiaContext _context;

        public ModuloRepository(AcademiaContext context)
        {
            _context = context;
        }

        public async Task<List<Modulo>> GetAllAsync()
        {
            return await _context.Modulos
                .Include(m => m.ModulosUsuarios)
                .ToListAsync();
        }

        public async Task<Modulo?> GetByIdAsync(int id)
        {
            return await _context.Modulos
                .Include(m => m.ModulosUsuarios)
                .FirstOrDefaultAsync(m => m.Id_Modulo == id);
        }

        public async Task<Modulo> CreateAsync(Modulo modulo)
        {
            _context.Modulos.Add(modulo);
            await _context.SaveChangesAsync();
            return modulo;
        }

        public async Task UpdateAsync(Modulo modulo)
        {
            _context.Entry(modulo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var modulo = await GetByIdAsync(id);
            if (modulo != null)
            {
                _context.Modulos.Remove(modulo);
                await _context.SaveChangesAsync();
            }
        }
    }
}