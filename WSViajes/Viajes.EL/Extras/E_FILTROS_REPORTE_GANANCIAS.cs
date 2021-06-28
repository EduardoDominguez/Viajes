using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_FILTROS_REPORTE_GANANCIAS : E_FILTROS_REPORTE
    {
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
    }
}
