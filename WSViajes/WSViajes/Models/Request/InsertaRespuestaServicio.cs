using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaRespuestaServicio
    {
        public Guid IdPregunta { get; set; }
        public Guid IdPedido { get; set; }
        public byte Respuesta { get; set; }
    }
}