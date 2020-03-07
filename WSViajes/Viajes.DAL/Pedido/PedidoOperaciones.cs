using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using Viajes.DAL.Direccion;
using Viajes.DAL.Persona;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Pedido
{
    public class PedidoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar pedidos
        /// <param name="pPedido">Objeto de tipo E_PEDIDO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_PEDIDO pPedido)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    XElement xmlPedido = new XElement("PEDIDO");

                    foreach (var detalle in pPedido.Detalle)
                    {
                        if(detalle.Local == null)
                            throw new System.ArgumentException("El elemento Detalle.Local.IdLocal no puede ser nullo.", "parametro");


                        XElement xDetallePedido = new XElement("DETALLE");
                        xDetallePedido.Add(
                            new XAttribute("ID_LOCAL", detalle.Local.IdLocal),
                            new XAttribute("ID_PRODUCTO", detalle.IdProducto),
                            new XAttribute("PRECIO", detalle.Precio),
                            new XAttribute("CANTIDAD", detalle.Cantidad),
                            new XAttribute("OBSERVACIONES", detalle.Observaciones)
                        );
                        xmlPedido.Add(xDetallePedido);
                    }

                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PEDIDO(pPedido.IdPedido, pPedido.PersonaPide.IdPersona, pPedido.DireccionEntrega.IdDireccion,
                                        pPedido.PersonaEntrega.IdPersona, pPedido.Observaciones, pPedido.Folio,
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, xmlPedido.ToString(), "I",
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

        /// <summary>
        /// Método para actualizar pedido
        /// <param name="pDireccion">Objeto de tipo E_PEDIDO con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_DIRECCION pDireccion)
        {
            try
            {
                /*using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_DIRECCION(pDireccion.ID_DIRECCION, pDireccion.NOMBRE, pDireccion.CALLE,
                                        pDireccion.COLONIA, pDireccion.DESCRIPCION, pDireccion.NO_EXT,
                                         pDireccion.NO_INT, pDireccion.LATITUD, pDireccion.LONGITUD,
                                         pDireccion.ID_PERSONA, pDireccion.ID_PERSONA_ALTA, pDireccion.ESTATUS, pDireccion.PREDETERMINADA, "U",
                                        RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }*/

                return new E_MENSAJE();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <param name="pIdPersonaPide">Folio del pedido</param>
        /// <param name="pFolio">Folio del pedido</param>
        /// <returns> Objeto tipo List<E_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> Consultar(long? pIdPedido = null, int? pIdPersonaPide = null, string pFolio = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var pedidos = await (from s in context.M_PEDIDO
                                   where
                                   (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                    && (pIdPersonaPide == null || (pIdPersonaPide != null && s.id_persona_pide == pIdPersonaPide))
                                   select s).ToListAsync<M_PEDIDO>();

                    return await ProcesaListaPedidos(pedidos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar pedido actual
        /// <param name="pIdPersonaPide">Id de la persona que pide</param>
        /// <param name="pIdPersonaEntrega">Id de la persona que entrega</param>
        /// <returns> Objeto tipo List<E_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultarViajeActual(int? pIdPersonaPide = null, int? pIdPersonaEntrega = null)
        {
            try
            {
               
                using (context = new ViajesEntities())
                {
                    var pedidos = await (from s in context.M_PEDIDO
                                   where
                                   //s.id_persona_pide == pIdPersonaPide
                                   (pIdPersonaPide == null || (pIdPersonaPide != null && s.id_persona_pide == pIdPersonaPide))
                                   && (pIdPersonaEntrega == null || (pIdPersonaEntrega != null && s.id_persona_entrega == pIdPersonaEntrega))
                                   && (s.id_estatus == 1 || s.id_estatus == 2 || s.id_estatus == 3 || s.id_estatus == 4)
                                   select s).ToListAsync<M_PEDIDO>();


                    return await ProcesaListaPedidos(pedidos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar los pedidos por asignar del día
        /// <returns> Objeto tipo List<E_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultaPorAsignar()
        {
            try
            {

                using (context = new ViajesEntities())
                {
                    var pedidos = await (from s in context.M_PEDIDO
                                   where
                                   s.id_estatus == 1 
                                   //&& s.fecha_pedido == DateTime.Now
                                   select s).ToListAsync<M_PEDIDO>();


                    return await ProcesaListaPedidos(pedidos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar historal de pedidos por conductor
        /// <param name="pIdPersona">Identificador del conductor a consultar historial</param>
        /// <returns> Objeto tipo List<E_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultarHistorialConductor(int pIdPersona)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    
                    var pedidos = await (from p in context.M_PEDIDO
                                         join per in context.CTL_PERSONA on p.id_persona_entrega equals per.id_persona
                                         join ap in context.CTL_ACCESO_PERSONA on per.id_persona equals ap.id_persona
                                         where
                                         p.id_persona_entrega == pIdPersona && ap.tipo_usuario == 2
                                         orderby p.fecha_entrega
                                         select p).ToListAsync<M_PEDIDO>();

                    return await ProcesaListaPedidos(pedidos);
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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "C",
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

        /// <summary>
        /// Método para asignar pedidos
        /// <param name="pPedido">Objeto de tipo E_PEDIDO con datos a asignar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Asignar(E_PEDIDO pPedido)
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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "A",
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


        /// <summary>
        /// Método para actualizar estatus del pedido
        /// <param name="pPedido">Objeto de tipo E_PEDIDO con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaEstatus(E_PEDIDO pPedido)
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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "AE",
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


        private async Task<List<E_PEDIDO>> ProcesaListaPedidos(List<M_PEDIDO> pPedidos)
        {
            var listaPedidos = new List<E_PEDIDO>();

            foreach (var pedido in pPedidos)
            {
                var direccion = await new DireccionOperaciones().ConsultarDirecciones(pIdDireccion: pedido.id_direccion_entrega);
                var estatus = await  new EstatusOperaciones().Consultar(pIdEstatus: pedido.id_estatus);
                var conductor = await new ConductorOperaciones().Consultar(pIdPersona: pedido.id_persona_entrega);
                var cliente = await new PersonaOperaciones().Consultar(pIdPersona: pedido.id_persona_pide);
                listaPedidos.Add(new E_PEDIDO
                {
                    IdPedido = pedido.id_pedido,
                    //IdPersonaPide = pedido.id_persona_pide,
                    PersonaPide = cliente.FirstOrDefault(),
                    DireccionEntrega = direccion.FirstOrDefault(),
                    PersonaEntrega = conductor.FirstOrDefault(),
                    IdMetodoPago = pedido.id_metodo_pago,
                    IdEncuesta = pedido.id_encuesta,
                    Estatus = estatus.FirstOrDefault(),
                    Calificacion = pedido.calificacion ?? 0,
                    Observaciones = pedido.observaciones,
                    FechaPedido = pedido.fecha_pedido,
                    HoraPedido = pedido.hora_pedido,
                    FechaEntrega = pedido.fecha_entrega ?? DateTime.MinValue,
                    HoraEntrega = pedido.hora_entrega ?? TimeSpan.MinValue,
                    Folio = pedido.folio,
                    Detalle = await new DetallePedidoOperaciones().Consultar(pIdPedido: pedido.id_pedido),
                }); ; ;
            }

            return listaPedidos;
        }
    }
}

