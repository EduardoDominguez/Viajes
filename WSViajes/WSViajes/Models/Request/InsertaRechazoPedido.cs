using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaRechazoPedido
    {
        public int IdPersona { get; set; }
        public Guid IdPedido { get; set; }
        public string Motivo { get; set; }
    }
}