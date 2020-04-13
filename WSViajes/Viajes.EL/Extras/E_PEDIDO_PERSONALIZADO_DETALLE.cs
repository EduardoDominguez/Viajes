using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_PEDIDO_PERSONALIZADO_DETALLE
    {
        public Guid IdPedido { get; set; }
        public Guid IdDetallePedidoPersonalizado { get; set; }
        public string NombreLocal { get; set; }
        public string Direccion { get; set; }        
        public string Pedido { get; set; }
        public string Referencias { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public decimal LimiteInferion { get; set; }
        public decimal LimiteSuperior { get; set; }
    }
}
