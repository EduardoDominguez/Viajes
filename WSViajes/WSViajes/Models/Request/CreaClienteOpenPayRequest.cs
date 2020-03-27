using System;
namespace WSViajes.Models.Request
{
    public class CreaClienteOpenPayRequest
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Apellidos { get; set; }
    }
}
