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
    public class DetallePedidoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para consultar detalle de pedidos
        /// <param name="pIdDetallePedido">Folio del pedido</param>
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <returns> Objeto tipo List<E_DETALLE_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_DETALLE_PEDIDO>> Consultar(Guid? pIdDetallePedido = null, Guid? pIdPedido = null)
        {
            try
            {
                var listaDetallePedidos = new List<E_DETALLE_PEDIDO>();
                using (context = new ViajesEntities())
                {
                    var detalles = await (from s in context.M_DETALLE_PEDIDO
                                          join p in context.CTL_PRODUCTO on s.id_producto equals p.id_producto
                                          where
                                          (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                    && (pIdDetallePedido == null || (pIdDetallePedido != null && s.id_detalle_pedido == pIdDetallePedido))
                                   select new E_DETALLE_PEDIDO
                                   {
                                       IdPedido = s.id_pedido,
                                       IdDetallePedido = s.id_detalle_pedido,
                                       IdLocal = s.id_local,
                                       //Local = await new LocalOperaciones().ConsultarLocales(pIdLocal: s.id_local).FirstOrDefault(),
                                       IdProducto = s.id_producto,
                                       Precio = s.precio,
                                       Cantidad = s.cantidad,
                                       Observaciones = s.observaciones,
                                       NombreProducto = p.nombre
                                   }).ToListAsync();


                    foreach (var detalle in detalles)
                    {
                        var local = await new LocalOperaciones().ConsultarLocales(pIdLocal: detalle.IdLocal);
                        detalle.Local = local.FirstOrDefault();
                        detalle.Extras = await new ProductoOperaciones().ConsultaExtrasPedidoByIdDetalle(detalle.IdDetallePedido);
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


