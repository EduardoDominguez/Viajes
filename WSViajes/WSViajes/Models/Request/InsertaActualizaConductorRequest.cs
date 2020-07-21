using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaConductorRequest
    {
        public string Nombre { get; set; }
        /*public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }*/
        public string Sexo { get; set; }
        //public int EDAD { get; set; }
        public string Telefono { get; set; }
        public string Fotografia { get; set; }

        public string IdPersonaAlta { get; set; }
        public string IdPersonaModifica { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public byte TipoUsuario { get; set; }

        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NoExt { get; set; }
        public string NoInt { get; set; }
        public string NoLicencia { get; set; }
        public string NoPlacas { get; set; }
        public byte Tipo { get; set; }

    }
}