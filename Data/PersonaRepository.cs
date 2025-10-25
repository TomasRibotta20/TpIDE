using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class PersonaRepository
    {
        private AcademiaContext CreateContext() => new AcademiaContext();

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            using var context = CreateContext();
            return await context.Personas.ToListAsync();
        }

        public async Task<IEnumerable<Persona>> GetAlumnosAsync()
        {
            using var context = CreateContext();
            return await context.Personas
                .Where(p => p.TipoPersona == TipoPersona.Alumno)
                .ToListAsync();
        }

        public async Task<IEnumerable<Persona>> GetProfesoresAsync()
        {
            using var context = CreateContext();
            return await context.Personas
                .Where(p => p.TipoPersona == TipoPersona.Profesor)
                .ToListAsync();
        }

        public async Task<Persona?> GetByIdAsync(int id)
        {
            using var context = CreateContext();
            return await context.Personas.FindAsync(id);
        }

        public async Task AddAsync(Persona persona)
        {
            using var context = CreateContext();
            context.Personas.Add(persona);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Persona persona)
        {
            using var context = CreateContext();
            context.Personas.Attach(persona);
            context.Entry(persona).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = CreateContext();
            var persona = await context.Personas.FindAsync(id);
            if (persona != null)
            {
                context.Personas.Remove(persona);
                await context.SaveChangesAsync();
            }
        }
    }
}