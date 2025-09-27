using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Plan
    {

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int EspecialidadId { get; set; }

        public Plan( int id, string descripcion, int especialidadId)
        {
            SetId(id);
            SetDescripcion(descripcion);
            SetEspecialidadId(especialidadId);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor o igual que 0.", nameof(id));
            Id = id;
        }

        public void SetDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(descripcion));
            Descripcion = descripcion;
        }

        public void SetEspecialidadId(int especialidadId)
        {
            if (especialidadId <= 0)
                throw new ArgumentException("El Id de la especialidad debe ser mayor que 0.", nameof(especialidadId));
            EspecialidadId = especialidadId;
        }

    }
}
