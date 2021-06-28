using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data;
//using System.Data.Objects;
//using System.Data.Entity;
//using LinqKit;

namespace Viajes.DAL.Reportes
{
    public class ReportesOperaciones
    {
        private ViajesEntities context;



        /// <summary>
        /// Método para consultar reporte de ganancias
        /// <param name="pFiltros">Filtros para la consulta</param>
        /// <returns> Objeto tipo E_RPT_GANANCIAS<E_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_LISTA_PAGINADA<E_RPT_GANANCIAS>> ConsultaReporteGanancias(E_FILTROS_REPORTE_GANANCIAS pFiltros)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    if (pFiltros.pageIndex < 0)
                        pFiltros.pageIndex = 0;

                    if (pFiltros.pageSize < 0)
                        pFiltros.pageSize = 0;

                    //Inicializa el Resultado.
                    var result = new E_LISTA_PAGINADA<E_RPT_GANANCIAS>()
                    {
                        PageIndex = pFiltros.pageIndex,
                        PageSize = pFiltros.pageSize,
                        TotalRows = 0
                    };

                    IQueryable<V_RPT_GANANCIAS> solicitudRequisicionIQ = from s in context.V_RPT_GANANCIAS select s;

                    /*Genera los Filtros*/                    
                    if (!string.IsNullOrEmpty(pFiltros.palabraClave))
                    {
                        solicitudRequisicionIQ.Where(p => 
                            p.nombre_local.ToUpper().Contains(pFiltros.palabraClave.ToUpper()) ||
                            p.nombre_repartidor.ToUpper().Contains(pFiltros.palabraClave.ToUpper()) ||
                            p.tipo_pedido.ToUpper().Contains(pFiltros.palabraClave.ToUpper())
                            );
                    }


                    if (pFiltros.fechaInicial != DateTime.MinValue && pFiltros.fechaFinal != DateTime.MinValue)
                    {
                        solicitudRequisicionIQ.Where(p => p.fecha_pedido >= pFiltros.fechaInicial && p.fecha_pedido <= pFiltros.fechaFinal);
                    }



                    // //Se deja comentado hasta agregar en la tabla el campo de fecha de creación (hasta agregar los campos de las plazas):
                    // if(!string.IsNullOrEmpty(pParams.FechaCreacionInicio) && !string.IsNullOrEmpty(pParams.FechaCreacionFin)){
                    //     var formatDate = "dd/MM/yyyy";
                    //     CultureInfo provider = new CultureInfo("es-MX");

                    //     predicado = predicado.And(p => p.MSRFechaCreacion >= DateTime.ParseExact(pParams.FechaCreacionInicio, formatDate, provider) && p.MSRFechaCreacion <= DateTime.ParseExact(pParams.FechaCreacionFin, formatDate, provider));
                    //     // hasParams = true;
                    // }


                    /* Aplica los Filtros */
                    //solicitudRequisicionIQ = solicitudRequisicionIQ.Where(predicado);

                    /*Recupera el Total de Registros filtrados*/
                    result.TotalRows = solicitudRequisicionIQ.Count();

                    /* Aplica el Ordenamiento */
                    switch (pFiltros.sortColumn)
                    {
                        case "folio":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.folio);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.folio);
                                    break;
                            }
                            break;
                        case "fechaPedido":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.fecha_pedido);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.fecha_pedido);
                                    break;
                            }
                            break;
                        case "nombreLocal":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.nombre_local);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.nombre_local);
                                    break;
                            }
                            break;

                        case "costoTotal":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.costo_total);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.costo_total);
                                    break;
                            }
                            break;

                        case "totalLocal":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.total_local);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.total_local);
                                    break;
                            }
                            break;

                        case "costoViaje":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.costo_envio);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.costo_envio);
                                    break;
                            }
                            break;

                        case "totalRepartidor":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.total_repartidor);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.total_repartidor);
                                    break;
                            }
                            break;

                        case "totalEmpresa":
                            switch (pFiltros.sortDirection)
                            {
                                case "asc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderBy(s => s.total_empresa);
                                    break;
                                case "desc":
                                    solicitudRequisicionIQ = solicitudRequisicionIQ.OrderByDescending(s => s.total_empresa);
                                    break;
                            }
                            break;
                    }

                    /* Aplica la Paginación */
                    if (pFiltros.pageSize > 0)
                    {
                        solicitudRequisicionIQ = solicitudRequisicionIQ
                            .Skip(pFiltros.pageIndex * pFiltros.pageSize)
                            .Take(pFiltros.pageSize);
                    }

                    /* Recupera los registros filtrados y ordenados */
                    result.Rows = await solicitudRequisicionIQ.AsNoTracking().Select( e => new E_RPT_GANANCIAS {
                        IdPedido = e.id_pedido,
                        Folio = e.folio,
                        CostoTotal = e.costo_total ?? 0,
                        CostoViaje = e.costo_envio,
                        FechaPedido = e.fecha_pedido,
                        NombreLocal = e.nombre_local,
                        NombreRepartidor = e.nombre_repartidor,
                        TotalEmpresa = e.total_empresa,
                        TotalLocal = e.total_local,
                        TotalRepartidor = e.total_repartidor,
                        TipoPedido = e.tipo_pedido
                    }).ToListAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
