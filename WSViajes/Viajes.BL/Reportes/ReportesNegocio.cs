using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.DAL.Reportes;

namespace Viajes.BL.Banner
{
    public class ReportesNegocio
    {
          
        public async Task<E_LISTA_PAGINADA<E_RPT_GANANCIAS>> ConsultaReporteGanancias(E_FILTROS_REPORTE_GANANCIAS pFiltros)
        {
            try
            {
                ReportesOperaciones pDatos = new ReportesOperaciones();
                var pResultado = await pDatos.ConsultaReporteGanancias(pFiltros);
                return pResultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
