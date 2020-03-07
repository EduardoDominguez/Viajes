using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_METODO_PAGO
    {
        public int IdMetodoPago { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdPersonaAlta { get; set; }
        public int IdPersonaModifica { get; set; }
        public byte Estatus { get; set; }
    }
}
