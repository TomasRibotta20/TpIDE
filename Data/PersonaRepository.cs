using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class PersonaRepository
    {
        private AcademiaContext CreateContext() => new AcademiaContext();

        public IEnumerable<Persona> GetAll()
        {
            using var context = CreateContext();
            return context.Personas.ToList();
        }

        public IEnumerable<Persona> GetAlumnos()
        {
            using var context = CreateContext();
            return context.Personas.Where(p => p.TipoPersona == TipoPersona.Alumno).ToList();
        }

        public IEnumerable<Persona> GetProfesores()
        {
            using var context = CreateContext();
            return context.Personas.Where(p => p.TipoPersona == TipoPersona.Profesor).ToList();
        }

        public Persona GetById(int id)
        {
            using var context = CreateContext();
            return context.Personas.Find(id);
        }

        public void Add(Persona persona)
        {
            using var context = CreateContext();
            context.Personas.Add(persona);
            context.SaveChanges();
        }

        public void Update(Persona persona)
        {
            using var context = CreateContext();
            context.Personas.Attach(persona);
            context.Entry(persona).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = CreateContext();
            var persona = context.Personas.Find(id);
            if (persona != null)
            {
                context.Personas.Remove(persona);
                context.SaveChanges();
            }
        }
    }
}