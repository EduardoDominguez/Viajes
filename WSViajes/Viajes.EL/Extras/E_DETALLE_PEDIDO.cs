using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_DETALLE_PEDIDO
    {
        public Guid IdPedido { get; set; }
        public Guid IdDetallePedido { get; set; }
        public E_LOCAL Local { get; set; }
        public int IdLocal { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Observaciones { get; set; }
        public List<E_EXTRAS_PRODUCTO> Extras { get; set; }

        public E_DETALLE_PEDIDO()
        {
            this.Extras = new List<E_EXTRAS_PRODUCTO>();
        }


    }
}
