using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_PRODUCTO_BUSQUEDA
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal Precio { get; set; }
        public string FotografiaProducto { get; set; }
        public int IdLocal { get; set; }
        public string NombreLocal { get; set; }
        public string FotografiaLocal { get; set; }
        public int IdTipoLocal { get; set; }
        public string NombreTipoLocal { get; set; }
        public string FotografiaTipoLocal { get; set; }
    }
}
