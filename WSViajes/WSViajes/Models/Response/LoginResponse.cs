using Viajes.EL.Extras;

namespace WSViajes.Models.Response
{
    public class LoginResponse : Respuesta
    {
        public E_PERSONA Persona { get; set; }
        public string Token { get; set; }
        public int IdLocal { get; set; }
    }
}