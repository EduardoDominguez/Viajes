using System;

namespace WSViajes.Models.Request
{
    public class InsertaActualizaExtraRequest
    {
        public Guid IdExtra { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public byte Estatus { get; set; }
        public int IdPersona { get; set; }
    }
}