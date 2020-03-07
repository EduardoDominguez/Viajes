namespace WSViajes.Models.Request
{
    public class InsertaActualizaDireccionRequest
    {
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string Descripcion { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int IdDireccion { get; set; }
        public int IdPersona { get; set; }
        public int IdPersonaCrea { get; set; }
        public int IdPersonaModifica { get; set; }
        public byte Estatus { get; set; }
        public byte Predeterminada { get; set; }
    }
}