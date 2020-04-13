using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.MetodoPago
{
    public class MetodoPagoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para consultar metodos de pago
        /// <param name="pId">Id del pedido a consultar</param>
        /// <param name="SoloActivos">Indica consultar solo activos o no</param>
        /// <returns> Objeto tipo List<E_METODO_PAGO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_METODO_PAGO>> Consultar(int? pId = null, int? SoloActivos = null)
        {
            try
            {
                var listaMetodosPago = new List<E_METODO_PAGO>();
                using (context = new ViajesEntities())
                {
                    var metodosPago = await (from s in context.CTL_METODO_PAGO
                                   where
                                   (pId == null || (pId != null && s.id_metodo_pago == pId))
                                    && (SoloActivos == null || (SoloActivos != null && s.estatus == SoloActivos))
                                   select s).ToListAsync<CTL_METODO_PAGO>();

                    foreach (var metodoPago in metodosPago)
                    {
                        listaMetodosPago.Add(new E_METODO_PAGO
                        {
                            IdMetodoPago = metodoPago.id_metodo_pago,
                            Nombre = metodoPago.nombre,
                            Descripcion = metodoPago.descripcion,
                            Estatus = metodoPago.estatus,
                            IdPersonaAlta = metodoPago.id_persona_alta,
                        });
                    }

                    return listaMetodosPago;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Método para cancelar pedidos
        /// <param name="pPedido">Objeto de tipo E_PEDIDO con datos a cancelar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Cancelar(E_PEDIDO pPedido)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PEDIDO(pPedido.IdPedido, pPedido.PersonaPide.IdPersona, pPedido.DireccionEntrega.IdDireccion,
                                        pPedido.PersonaEntrega.IdPersona, pPedido.Observaciones, pPedido.Folio,
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "C", pPedido.ReferenciaPago,
                                         pPedido.CostoEnvio, pPedido.TipoPedido,
                                        RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

