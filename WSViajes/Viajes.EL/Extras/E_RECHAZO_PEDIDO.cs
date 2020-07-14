using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_RECHAZO_PEDIDO
    {
        public int IdPersona { get; set; }
        public Guid IdPedido { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
