namespace WSViajes.Models.Request
{
    public class ActualizaTokenRequest
    {
        public int IdPersona { get; set; }
        public string TokenFirebase { get; set; }
    }
}