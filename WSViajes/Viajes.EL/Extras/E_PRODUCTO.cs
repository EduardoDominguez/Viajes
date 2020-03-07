using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_PRODUCTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Fotografia { get; set; }
        public E_LOCAL Local { get; set; }
        public int IdPersonaAlta { get; set; }
        public int IdPersonaModifica { get; set; }
        public byte Estatus { get; set; }

        public E_PRODUCTO()
        {
            this.Local = new E_LOCAL();
        }
    }
}
