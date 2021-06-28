using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_RPT_GANANCIAS
    {
        public Guid IdPedido { get; set; }
        public string Folio { get; set; }
        public string NombreLocal { get; set; }
        public string NombreRepartidor { get; set; }
        public string TipoRepartidor { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal TotalLocal { get; set; }
        public decimal CostoViaje { get; set; }
        public decimal TotalRepartidor { get; set; }
        public decimal TotalEmpresa { get; set; }
        public DateTime FechaPedido { get; set; }
        public string TipoPedido { get; set; }
    }
}
