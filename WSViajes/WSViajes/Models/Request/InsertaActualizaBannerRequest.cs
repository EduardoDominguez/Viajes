using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaBannerRequest
    {
        public Guid IdBanner { get; set; }
        public string Nombre { get; set; }
        public string Fotografia { get; set; }
        public int IdProducto { get; set; }
        public int IdPersonaMovimiento { get; set; }
        public byte Estatus { get; set; }

    }
}