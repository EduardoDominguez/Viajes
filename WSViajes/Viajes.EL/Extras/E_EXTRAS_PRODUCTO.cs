using System;

namespace Viajes.EL.Extras
{
    public class E_EXTRAS_PRODUCTO
    {
        public Guid IdExtra { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public byte Estatus { get; set; }
        public string NombreEstatus {
            //set { NombreEstatus = value; }  // set method
            get
            {
                switch (this.Estatus)
                {
                    case 0:
                        return "Descativado";
                    case 1:
                        return "Activo";
                    case 2:
                        return "Agotado";
                    default: return "";
                }
            }
        }
    }
}
