using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_DETALLE_PEDIDO
    {
        public long IdPedido { get; set; }
        public int IdDetallePedido { get; set; }
        public E_LOCAL Local { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Observaciones { get; set; }
        
    }
}
