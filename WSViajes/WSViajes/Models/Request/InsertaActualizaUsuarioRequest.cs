using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaUsuarioRequest
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Fotografia { get; set; }

        public byte IdPersonaAlta { get; set; }
        public byte IdPersonaMod { get; set; }

        public string Email { get; set; }
        //public string Password { get; set; }
        public byte TipoUsuario { get; set; }

    }
}