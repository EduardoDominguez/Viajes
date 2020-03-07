namespace WSViajes.Models.Request
{
    public class EnviaNotificacionRequest
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public int IdPersona { get; set; }

    }
}