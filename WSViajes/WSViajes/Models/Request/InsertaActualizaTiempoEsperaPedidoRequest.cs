using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaTiempoEsperaPedidoRequest
    {
        public Guid IdPedido { get; set; }
        public int TiempoEspera { get; set; }
    }
}