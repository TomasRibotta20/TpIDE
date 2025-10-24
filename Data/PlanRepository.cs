using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PlanRepository
    {
        private AcademiaContext CreateContext()
        {
            return new AcademiaContext();
        }

        public IEnumerable<Domain.Model.Plan> GetAll()
        {
            using var context = CreateContext();
            return context.Set<Domain.Model.Plan>().OrderBy(p => p.Descripcion).ToList();
        }

        public Domain.Model.Plan GetById(int id)
        {
            using var context = CreateContext();
            return context.Set<Domain.Model.Plan>().Find(id);
        }

        public void Add(Domain.Model.Plan plan)
        {
            using var context = CreateContext();
            
            var newPlan = new Domain.Model.Plan(
                0, // El ID será asignado por la base de datos, pero necesitamos un valor para el constructor
                plan.Descripcion,
                plan.EspecialidadId
            );
            
            // Ignorar explícitamente la propiedad Id para permitir que la base de datos la asigne
            var entry = context.Set<Domain.Model.Plan>().Add(newPlan);
            entry.Property("Id").IsModified = false;
            
            context.SaveChanges();
        }
        public void Update(Domain.Model.Plan plan)
        {
            using var context = CreateContext();
            context.Set<Domain.Model.Plan>().Update(plan);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = CreateContext();
            var plan = context.Set<Domain.Model.Plan>().Find(id);
            if (plan != null)
            {
                context.Set<Domain.Model.Plan>().Remove(plan);
                context.SaveChanges();
            }
        }

    }
}
