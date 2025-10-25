using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class EspecialidadRepository
    {
        private AcademiaContext CreateContext()
        {
            return new AcademiaContext();
        }

        public async Task<IEnumerable<Especialidad>> GetAllAsync()
        {
            using var context = CreateContext();
            return await context.Especialidades.OrderBy(e => e.Descripcion).ToListAsync();
        }

        public async Task<Especialidad?> GetByIdAsync(int id)
        {
            using var context = CreateContext();
            return await context.Especialidades.FindAsync(id);
        }

        public async Task AddAsync(Especialidad especialidad)
        {
            using var context = CreateContext();
            context.Especialidades.Add(especialidad);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Especialidad especialidad)
        {
            using var context = CreateContext();
            context.Especialidades.Update(especialidad);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = CreateContext();
            var especialidad = await context.Especialidades.FindAsync(id);
            if (especialidad != null)
            {
                context.Especialidades.Remove(especialidad);
                await context.SaveChangesAsync();
            }
        }
    }
}