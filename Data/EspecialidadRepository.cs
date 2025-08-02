using Domain.Model;
namespace Data
{
    public class EspecialidadRepository
    {
        private static List<Especialidad> _especialidades = new List<Especialidad>
        {
            new Especialidad { Id = 1, Descripcion = "Especialidad1" },
            new Especialidad { Id = 2, Descripcion = "Especialidad2" },
            new Especialidad { Id = 3, Descripcion = "Especialidad3" }
        };

        public IEnumerable<Especialidad> GetAll() => _especialidades;

        public Especialidad GetById(int id) => _especialidades.FirstOrDefault(e => e.Id == id);

        public void Add(Especialidad especialidad)
        {
            especialidad.Id = _especialidades.Any() ? _especialidades.Max(e => e.Id) + 1 : 1;
            _especialidades.Add(especialidad);
        }

        public void Update(Especialidad especialidad)
        {
            var existing = GetById(especialidad.Id);
            if (existing != null)
            {
                existing.Descripcion = especialidad.Descripcion;
            }
        }

        public void Delete(int id)
        {
            var especialidad = GetById(id);
            if (especialidad != null) _especialidades.Remove(especialidad);
        }
    }
}