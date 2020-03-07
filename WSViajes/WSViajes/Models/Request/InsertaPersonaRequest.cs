namespace WSViajes.Models.Request
{
    public class InsertaPersonaRequest
    {
        public string Nombre { get; set; }
        /*public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }*/
        //public string SEXO { get; set; }
        //public int EDAD { get; set; }
        public string Telefono { get; set; }
        public string Fotografia { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public byte TipoUsuario { get; set; }
        public string TokenFirebase { get; set; }
    }
}