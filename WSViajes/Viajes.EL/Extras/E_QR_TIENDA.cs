using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_QR_TIENDA
    {
        public Guid IdQR { get; set; }
        public string Dispositivo { get; set; }
        public string Ip { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Aplicacion { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
