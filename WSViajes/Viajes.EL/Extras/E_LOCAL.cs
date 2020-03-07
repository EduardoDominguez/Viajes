namespace Viajes.EL.Extras
{
    public class E_LOCAL
    {
        public int IdLocal { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public string Referencias { get; set; }
        public string Fotografia { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public byte Estatus { get; set; }
        public int IdPersonaAlta { get; set; }
        public int IdPersonaModifica { get; set; }
        public E_COSTO Costo { get; set; }
        public E_TIPO_LOCAL TipoLocal { get; set; }

        public E_LOCAL()
        {
            this.Costo = new E_COSTO();
            this.TipoLocal = new E_TIPO_LOCAL();
        }
    }
}
