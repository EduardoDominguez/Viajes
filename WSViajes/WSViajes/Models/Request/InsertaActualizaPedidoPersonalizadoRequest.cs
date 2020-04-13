using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaPedidoPersonalizadoRequest
    {
        public int IdPersonaPide { get; set; }
        public int IdDireccionEntrega { get; set; }
        public int IdMetodoPago { get; set; }
        public string Observaciones { get; set; }        
        public decimal CostoEnvio { get; set; }
        public string TokenTarjeta { get; set; }
        public string SessionId { get; set; }   
        
        //Detalle del pedido personalizado
        public string Nombrelocal { get; set; }
        public string Pedido { get; set; }
        public string Direccion { get; set; }
        public string Referencias { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public decimal LimiteInferion { get; set; }
        public decimal LimiteSuperior { get; set; }
    }
}
