namespace WSViajes.Models
{
    public class Respuesta
    {
        public string Mensaje { get; set; }
        public bool Exito { get; set; }
        public int CodigoError { get; set; }

        public Respuesta()
        {
            Inicializar();
        }

        public void Inicializar()
        {
            this.Mensaje = string.Empty;
            this.Exito = false;
            this.CodigoError = 0;
        }
    }
}