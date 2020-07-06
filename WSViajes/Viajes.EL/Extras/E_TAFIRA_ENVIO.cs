using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_TAFIRA_ENVIO
    {
        public Guid IdTarifa { get; set; }
        public int DistanciaMenor { get; set; }
        public int DistanciaMayor { get; set; }
        public decimal CostoEnvio { get; set; }
    }
}
