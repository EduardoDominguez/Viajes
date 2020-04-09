using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_COORDENADAS_CONDUCTOR
    {
        public Guid IdCoordenada { get; set; }
        public int IdPersona { get; set; }
        public Guid IdPedido { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime Fecha { get; set; }
    }
}
