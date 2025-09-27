using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Comision
    {
        public int IdComision { get; set; }
        public string DescComision { get; set; }
        public int AnioEspecialidad { get; set; }
        public int IdPlan { get; set; }

        private Comision() { }

        public Comision(string descComision, int anioEspecialidad, int idPlan)
        {
            SetDescComision(descComision);
            SetAnioEspecialidad(anioEspecialidad);
            SetIdPlan(idPlan);
            // El ID de la comisión será asignado por la base de datos
            IdComision = 0;
        }

        public Comision(int idComision, string descComision, int anioEspecialidad, int idPlan)
        {
            SetIdComision(idComision);
            SetDescComision(descComision);
            SetAnioEspecialidad(anioEspecialidad);
            SetIdPlan(idPlan);
        }

        public void SetIdComision(int idComision)
        {
            if (idComision <= 0)
                throw new ArgumentException("El ID de la comisión debe ser mayor que cero.", nameof(idComision));
            IdComision = idComision;
        }

        public void SetDescComision(string descComision)
        {
            if (string.IsNullOrWhiteSpace(descComision))
            {
                throw new ArgumentException("La descripción de la comisión no puede estar vacía.", nameof(descComision));
            }
            if (descComision.Length > 50)
            {
                throw new ArgumentException("La descripción de la comisión no puede exceder los 50 caracteres.", nameof(descComision));
            }
            DescComision = descComision;
        }

        public void SetAnioEspecialidad(int anioEspecialidad)
        {
            if (anioEspecialidad <= 0)
            {
                throw new ArgumentException("El año de especialidad debe ser mayor que cero.", nameof(anioEspecialidad));
            }
            AnioEspecialidad = anioEspecialidad;
        }

        public void SetIdPlan(int idPlan)
        {
            if (idPlan <= 0)
            {
                throw new ArgumentException("El ID del plan debe ser mayor que cero.", nameof(idPlan));
            }
            IdPlan = idPlan;
        }
    }
}
