using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class CreaTarjetaOpenPayRequest
    {
        public int IdPersona {get; set;}
        public string HolderName { get; set; }
        public string CardNumber { get; set; }
        public string Cvv2 { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string DeviceSessionId { get; set; }

    }
}