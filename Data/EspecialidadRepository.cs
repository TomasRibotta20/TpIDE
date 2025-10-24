using Domain.Model;
using System;
namespace Data
{
    public class EspecialidadRepository
    {
        private AcademiaContext CreateContext()
        {
            return new AcademiaContext();
        }

        public IEnumerable<Especialidad> GetAll()
        {
            using var context = CreateContext();
            return context.Especialidades.OrderBy(e => e.Descripcion).ToList();
        }

        public Especialidad GetById(int id)
        {
            using var context = CreateContext();
            return context.Especialidades.Find(id);
        }
        public void Add(Especialidad especialidad)
        {
            using var context = CreateContext();
            context.Especialidades.Add(especialidad);
            context.SaveChanges();
        }

        public void Update(Especialidad especialidad)
        {
            using var context = CreateContext();
            context.Especialidades.Update(especialidad);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            using var context = CreateContext();
            var especialidad = context.Especialidades.Find(id);
            if (especialidad != null)
            {
                context.Especialidades.Remove(especialidad);
                context.SaveChanges();
            }
        }
    }
}