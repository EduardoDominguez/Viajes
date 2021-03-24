using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaUsuarioRequest
    {
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Fotografia { get; set; }

        public string IdPersonaAlta { get; set; }
        public string IdPersonaModifica { get; set; }

        public string Email { get; set; }
        //public string Password { get; set; }
        public byte TipoUsuario { get; set; }

    }
}