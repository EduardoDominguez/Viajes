using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Enum
{
    public enum Errores
    {
        SinDatos = 10000,
        ErrorServicio = 10001,
        GastosAComprobar = 2,
        PagoNoPresupuestal = 3,
        FacturaDirecta = 4,
        SolicitudPagoMixta = 5
    }
}
