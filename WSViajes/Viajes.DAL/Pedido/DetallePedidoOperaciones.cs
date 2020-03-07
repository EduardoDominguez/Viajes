using System;
using System.Collections.Generic;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using Viajes.DAL.Local;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Pedido
{
    public class DetallePedidoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para consultar detalle de pedidos
        /// <param name="pIdDetallePedido">Folio del pedido</param>
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <returns> Objeto tipo List<E_DETALLE_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_DETALLE_PEDIDO>> Consultar(int? pIdDetallePedido = null, long? pIdPedido = null)
        {
            try
            {
                var listaDetallePedidos = new List<E_DETALLE_PEDIDO>();
                using (context = new ViajesEntities())
                {
                    var detalles = await (from s in context.M_DETALLE_PEDIDO
                                   where
                                   (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                    && (pIdDetallePedido == null || (pIdDetallePedido != null && s.id_detalle_pedido == pIdDetallePedido))
                                   select s).ToListAsync<M_DETALLE_PEDIDO>();

                    foreach (var detalle in detalles)
                    {
                        var local = await new LocalOperaciones().ConsultarLocales(pIdLocal: detalle.id_local);
                        listaDetallePedidos.Add(new E_DETALLE_PEDIDO
                        {
                            IdPedido = detalle.id_pedido,
                            IdDetallePedido = detalle.id_detalle_pedido,
                            Local = local.FirstOrDefault(),
                            IdProducto = detalle.id_producto,
                            Precio = detalle.precio,
                            Cantidad = detalle.cantidad,
                            Observaciones = detalle.observaciones
                        });
                    }

                    return listaDetallePedidos;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


