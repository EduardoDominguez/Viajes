using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class ActualizaPasswordRequest
    {
        public int IdPersona { get; set; }
        public string TokenPassword { get; set; }
        public int IdTipoUsuario { get; set; }
        public string Password { get; set; }
    }
}