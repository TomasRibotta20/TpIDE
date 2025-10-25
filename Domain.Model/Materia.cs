using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Model
{
    public class Materia
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int HorasSemanales { get; set; }
        public int HorasTotales { get; set; }
        public int IdPlan { get; set; }

        public Materia() { }

        public Materia(int id, string descripcion, int horasSemanales, int horasTotales, int idPlan)
        {
            SetId(id);
            SetDescripcion(descripcion);
            SetHorasSemanales(horasSemanales);
            SetHorasTotales(horasTotales);
            SetIdPlan(idPlan);
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

        public void SetHorasSemanales(int horasSemanales)
        {
            if (horasSemanales <= 0)
                throw new ArgumentException("Las horas semanales deben ser mayores que 0.", nameof(horasSemanales));
            HorasSemanales = horasSemanales;
        }

        public void SetHorasTotales(int horasTotales)
        {
            if (horasTotales <= 0)
                throw new ArgumentException("Las horas totales deben ser mayores que 0.", nameof(horasTotales));
            HorasTotales = horasTotales;
        }

        public void SetIdPlan(int idPlan)
        {
            if (idPlan <= 0)
                throw new ArgumentException("El Id del Plan debe ser mayor que 0.", nameof(idPlan));
            IdPlan = idPlan;
        }
    }
}
