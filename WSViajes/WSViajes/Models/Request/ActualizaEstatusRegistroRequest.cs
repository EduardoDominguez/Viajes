using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class ActualizaEstatusRegistroRequest<T>
    {
        public T IdRegistro { get; set; }
        public byte IdEstatus { get; set; }
        public byte IdPersonaModifica { get; set; }
    }
}