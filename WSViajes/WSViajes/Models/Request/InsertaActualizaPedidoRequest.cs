﻿using Viajes.EL.Extras;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaPedidoRequest
    {
        public E_PEDIDO Pedido { get; set; }

        public string TokenTarjeta { get; set; }
        public string SessionId { get; set; }

    }
}