using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTOs
{
    public class MateriaDto
    {
        public int Id { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;
        public int HorasSemanales { get; set; }
        public int HorasTotales { get; set; }

        public int IdPlan { get; set; }

        [DisplayName("Plan de Estudios")]
        public string DescripcionPlan { get; set; } = string.Empty;
    }
}
