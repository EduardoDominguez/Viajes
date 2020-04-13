using System;
using System.Collections.Generic;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using Viajes.DAL.Local;
using System.Data.Entity;
using System.Threading.Tasks;
using Viajes.DAL.Producto;

namespace Viajes.DAL.Pedido
{
    public class DetallePedidoPersonalizadoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para consultar detalle de pedidos personalizado
        /// <param name="pIdDetallePedido">Folio del pedido</param>
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <returns> Objeto tipo E_PEDIDO_PERSONALIZADO_DETALLE con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_PEDIDO_PERSONALIZADO_DETALLE> Consultar(Guid? pIdDetallePedido = null, Guid? pIdPedido = null)
        {
            try
            {
                //var listaDetallePedidos = new List<E_DETALLE_PEDIDO>();
                using (context = new ViajesEntities())
                {
                    var detalles = await (from s in context.M_DETALLE_PEDIDO_PERSONALIZADO
                                          where
                                          (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                          && (pIdDetallePedido == null || (pIdDetallePedido != null && s.id_detalle_pedido_personalizado == pIdDetallePedido))
                                          select new E_PEDIDO_PERSONALIZADO_DETALLE
                                          {
                                              IdPedido = s.id_pedido,
                                              IdDetallePedidoPersonalizado = s.id_detalle_pedido_personalizado,
                                              Direccion = s.direccion,
                                              Latitud = s.latitud,
                                              LimiteInferion = s.limite_inferior,
                                              LimiteSuperior = s.limite_superior,
                                              Longitud = s.longitud,
                                              NombreLocal = s.nombre_local,
                                              Pedido = s.pedido, 
                                              Referencias = s.referencias
                                          }).ToListAsync();

                    return detalles.FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


