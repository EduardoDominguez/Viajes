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

                    pPedido.IdPedido = Guid.NewGuid();

                    XElement xmlPedido = new XElement("PEDIDO");

                    foreach (var detalle in pPedido.Detalle)
                    {
                        if(detalle.Local == null)
                            throw new System.ArgumentException("El elemento Detalle.Local.IdLocal no puede ser nullo.", "parametro");

                        Guid IdDetalle = Guid.NewGuid();
                        foreach(var extra in detalle.Extras)
                        {
                            XElement xExtrasProducto = new XElement("EXTRAS");
                            xExtrasProducto.Add(
                                new XAttribute("ID_DETALLE_PEDIDO", IdDetalle.ToString()),
                                new XAttribute("ID_EXTRA", extra.IdExtra),
                                new XAttribute("PRECIO", extra.Precio)
                            );
                            xmlPedido.Add(xExtrasProducto);
                        }

                        XElement xDetallePedido = new XElement("DETALLE");
                        xDetallePedido.Add(
                            new XAttribute("ID_DETALLE_PEDIDO", IdDetalle.ToString()),
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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, xmlPedido.ToString(), "I", pPedido.ReferenciaPago,
                                         pPedido.CostoEnvio, pPedido.TipoPedido, pPedido.Propina, pPedido.IdEstatusFactura, pPedido.Iva,
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
        /// Método para insertar pedidos personalizados
        /// <param name="pPedido">Objeto de tipo E_PEDIDO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE AgregarPersonalizado(E_PEDIDO_PERSONALIZADO pPedido)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    pPedido.IdPedido = Guid.NewGuid();

                    XElement xmlPedido = new XElement("PEDIDO_PERSONALIZADO");

                    //foreach (var detalle in pPedido.Detalle)
                    //{
                       
                        XElement xDetallePedido = new XElement("DETALLE");
                        xDetallePedido.Add(
                            new XAttribute("ID_DETALLE_PEDIDO_PERSONALIZADO", Guid.NewGuid()),
                            new XAttribute("ID_PEDIDO", pPedido.IdPedido),
                            new XAttribute("NOMBRE_LOCAL", pPedido.Detalle.NombreLocal),
                            new XAttribute("DIRECCION", pPedido.Detalle.Direccion),
                            new XAttribute("REFERENCIAS", pPedido.Detalle.Referencias),
                            new XAttribute("PEDIDO", pPedido.Detalle.Pedido),
                            new XAttribute("LATITUD", pPedido.Detalle.Latitud),
                            new XAttribute("LONGITUD", pPedido.Detalle.Longitud),
                            new XAttribute("LIMITE_INFERIOR", pPedido.Detalle.LimiteInferion),
                            new XAttribute("LIMITE_SUPERIOR", pPedido.Detalle.LimiteSuperior)

                        );
                        xmlPedido.Add(xDetallePedido);
                    //}

                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PEDIDO(pPedido.IdPedido, pPedido.PersonaPide.IdPersona, pPedido.DireccionEntrega.IdDireccion,
                                        pPedido.PersonaEntrega.IdPersona, pPedido.Observaciones, pPedido.Folio,
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, xmlPedido.ToString(), "IP", pPedido.ReferenciaPago,
                                         pPedido.CostoEnvio, pPedido.TipoPedido, pPedido.Propina, pPedido.IdEstatusFactura, pPedido.Iva,
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
        public async Task<List<E_PEDIDO>> Consultar(Guid? pIdPedido = null, int? pIdPersonaPide = null, string pFolio = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var pedidos = await (from s in context.M_PEDIDO
                                   where
                                   s.tipo_pedido == 1 //Pedidos normales
                                   && (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                   && (pIdPersonaPide == null || (pIdPersonaPide != null && s.id_persona_pide == pIdPersonaPide)) 
                                    orderby s.fecha_pedido descending, s.hora_pedido descending
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
                                   //s.tipo_pedido == 1 &&  //Pedidos normales
                                   (pIdPersonaPide == null || (pIdPersonaPide != null && s.id_persona_pide == pIdPersonaPide))
                                   && (pIdPersonaEntrega == null || (pIdPersonaEntrega != null && s.id_persona_entrega == pIdPersonaEntrega))
                                   && (s.id_estatus == 1 || s.id_estatus == 2 || s.id_estatus == 3 || s.id_estatus == 4 || s.id_estatus == 8 || s.id_estatus == 9 || s.id_estatus == 10)
                                         //&& (s.id_estatus == 1 || s.id_estatus == 2 || s.id_estatus == 3 || s.id_estatus == 4)
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
                                   //s.id_estatus == 1
                                   s.id_estatus == 8
                                         //&& s.tipo_pedido == 1   //Pedidos normales
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
                                         p.tipo_pedido == 1 &&  //Pedidos normales
                                         p.id_persona_entrega == pIdPersona && ap.tipo_usuario == 2
                                         orderby  p.fecha_pedido descending
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
        /// Método para consultar historal de pedidos por conductor
        /// <param name="pIdLocal">Identificador del local a consultar historial</param>
        /// <param name="pidEstatus">Identificador de estatus</param>
        /// <returns> Objeto tipo List<E_PEDIDO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO>> ConsultarHistorialLocal(int pIdLocal, string[] pidEstatus = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    var listaPeidos = await new DetallePedidoOperaciones().ConsultarPedidosByIdLocal(pIdLocal);

                    var listaIds = listaPeidos.Select(p => p.IdPedido).ToArray();

                    List<M_PEDIDO> pedidos = null;

                    if(pidEstatus != null && pidEstatus.Length > 0)
                    {
                        pedidos = await (from s in context.M_PEDIDO
                                         where
                                         s.tipo_pedido == 1 //Pedidos normales
                                         && listaIds.Contains(s.id_pedido)
                                         && pidEstatus.Contains(s.id_estatus.ToString())
                                         orderby s.fecha_pedido descending, s.hora_pedido descending
                                         select s).ToListAsync<M_PEDIDO>();
                    }
                    else
                    {
                        pedidos = await (from s in context.M_PEDIDO
                                         where
                                         s.tipo_pedido == 1 //Pedidos normales
                                         && listaIds.Contains(s.id_pedido)
                                         orderby s.fecha_pedido descending, s.hora_pedido descending
                                         select s).ToListAsync<M_PEDIDO>();
                    }
                   

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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "C", pPedido.ReferenciaPago,
                                         pPedido.CostoEnvio, pPedido.TipoPedido, pPedido.Propina, pPedido.IdEstatusFactura, pPedido.Iva,
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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "A", pPedido.ReferenciaPago,
                                         pPedido.CostoEnvio, pPedido.TipoPedido, pPedido.Propina, pPedido.IdEstatusFactura, pPedido.Iva,
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
                                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, null, "AE", pPedido.ReferenciaPago,
                                         pPedido.CostoEnvio, pPedido.TipoPedido, pPedido.Propina, pPedido.IdEstatusFactura, pPedido.Iva,
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
        /// Método para consultar las preguntas pendientes por contestar de un pedido
        /// <param name="pIdPedido">Identificador del pedido</param>
        /// <param name="pTipoPregunta">Representa el tipo de preguntas a consultar 1=Cliente, 2=Conductor, 3=Local</param>
        /// <returns> Objeto tipo List<E_PREGUNTA_SERVICIO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PREGUNTA_SERVICIO>> ConsultarPreguntasPendientesByIdPedido(Guid pIdPedido, byte pTipoPregunta)
        {
            try
            {
                using (context = new ViajesEntities())
                {


                    /*Consulta equivalente a subconsulta:
                     * https://stackoverflow.com/questions/183791/how-would-you-do-a-not-in-query-with-linq
                     * 
                     * select *
                        from  CTL_PREGUNTA_SERVICIO cp 
                            where  cp.id_pregunta not in (
				               select rpp.id_pregunta from  M_PEDIDO p 
				                inner join R_PEDIDO_PREGRUNTA rpp on cp.id_pregunta = rpp.id_pregunta
				            WHERE  p.id_pedido = '4791CF3F-BA77-47B0-A431-2402EB52CB57');
                    ¨*/
                    var preguntas = await (from p in context.CTL_PREGUNTA_SERVICIO
                                           where !(
                                                        from pe in context.M_PEDIDO
                                                        join rps in context.R_PEDIDO_PREGRUNTA on pe.id_pedido equals rps.id_pedido
                                                        where pe.id_pedido == pIdPedido
                                                        select rps.id_pregunta
                                                    ).Contains(p.id_pregunta)
                                            && p.tipo_pregunta == pTipoPregunta
                                           orderby p.tipo_pregunta, p.orden
                                         select new E_PREGUNTA_SERVICIO
                                         {
                                            IdPregunta = p.id_pregunta,
                                            Pregunta = p.pregunta,
                                            TipoPregunta = p.tipo_pregunta
                                         }).ToListAsync();

                    return preguntas;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar todas las preguntas por tipo de pregunta
        /// <param name="pTipoPregunta">Representa el tipo de preguntas a consultar 1=Cliente, 2=Conductor, 3=Local</param>
        /// <returns> Objeto tipo List<E_PREGUNTA_SERVICIO> con los datos solicitados </returns>  
        /// </summary>        
        public async Task<List<E_PREGUNTA_SERVICIO>> ConsultarPreguntaByTipo(byte pTipoPregunta)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    var preguntas = await (from p in context.CTL_PREGUNTA_SERVICIO
                                           where p.tipo_pregunta == pTipoPregunta
                                           orderby p.tipo_pregunta, p.orden
                                           select new E_PREGUNTA_SERVICIO
                                           {
                                               IdPregunta = p.id_pregunta,
                                               Pregunta = p.pregunta,
                                               TipoPregunta = p.tipo_pregunta
                                           }).ToListAsync();

                    return preguntas;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserta una respuesta a las preguntas del servicio
        /// <param name="pIdPedido"></param>
        /// <param name="pIdPregunta"></param>
        /// <param name="pRespuesta"></param>
        /// <returns> Objeto tipo E_MENSAJE con los datos DE LA SOLICITUD </returns>  
        /// </summary>        
        public E_MENSAJE AgregarRespuestaPreguntaServicio(Guid pIdPedido, Guid pIdPregunta, byte pRespuesta)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    E_MENSAJE vMensaje = new E_MENSAJE();
                    
                    context.R_PEDIDO_PREGRUNTA.Add(new R_PEDIDO_PREGRUNTA() { id_pregunta = pIdPregunta, id_pedido = pIdPedido, respuesta = pRespuesta }); // fecha_alta = DateTime.Now 
                    var resultado = context.SaveChanges();

                    if (resultado <= 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -100, RET_MENSAJEERROR = "No se pudo insertar, intente más tarde", RET_VALORDEVUELTO = "No se pudo insertar, intente más tarde" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Insertado", RET_VALORDEVUELTO = "Insertado" };
                    

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar agregar pedido
        /// <param name="pIdPersona">Identificador de la persona que rechaza</param>
        /// <param name="pIdPedido">Identificador del pedido</param>
        /// <param name="pMotivo">Motivo de rechazo</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE AgregaRechazoPedido(int pIdPersona, Guid pIdPedido, string pMotivo)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    E_MENSAJE vMensaje = new E_MENSAJE();

                    context.TBL_RECHAZO_PEDIDO.Add(new TBL_RECHAZO_PEDIDO() { id_persona = pIdPersona, id_pedido = pIdPedido, motivo = pMotivo, fecha =  DateTime.Now }); // fecha_alta = DateTime.Now 
                    var resultado = context.SaveChanges();

                    if (resultado <= 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -100, RET_MENSAJEERROR = "No se pudo insertar, intente más tarde", RET_VALORDEVUELTO = "No se pudo insertar, intente más tarde" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Insertado", RET_VALORDEVUELTO = "Insertado" };


                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar agregar pedido
        /// <param name="pTiempoEspera">Tiempo de espera en minutos del pedido</param>
        /// <param name="pIdPedido">Identificador del pedido</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE AgregaTiempoEspera(int pTiempoEspera, Guid pIdPedido)
        {
            try
            {
                E_MENSAJE vMensaje = new E_MENSAJE();
                using (context = new ViajesEntities())
                {

                    var pedido = (from s in context.M_PEDIDO
                                  where
                                  s.id_pedido.Equals(pIdPedido)
                                  select s).ToList<M_PEDIDO>().FirstOrDefault();


                    if (pedido != null)
                    {
                        
                        pedido.tiempo_espera = pTiempoEspera;
                        var resultado = context.SaveChanges();

                        if (resultado <= 0)
                            vMensaje = new E_MENSAJE { RET_NUMEROERROR = -100, RET_MENSAJEERROR = "No se pudo actualizar, intente más tarde", RET_VALORDEVUELTO = "No se pudo actualizar, intente más tarde" };
                        else
                            vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Actrualizado", RET_VALORDEVUELTO = "Actrualizado" };
                    }
                    else
                    {
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -200, RET_MENSAJEERROR = "No se pudo encontrar el pedido.", RET_VALORDEVUELTO = "No se pudo encontrar el pedido." };
                    }
                   
                    return vMensaje;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar pedidos personalizados
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <param name="pIdPersonaPide">Folio del pedido</param>
        /// <param name="pFolio">Folio del pedido</param>
        /// <returns> Objeto tipo List<E_PEDIDO_PERSONALIZADO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PEDIDO_PERSONALIZADO>> ConsultarPersonalizadosTodo(Guid? pIdPedido = null, int? pIdPersonaPide = null, string pFolio = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var pedidos = await (from s in context.M_PEDIDO
                                         where
                                         s.tipo_pedido == 2 //Pedidos normales
                                         && (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                         && (pIdPersonaPide == null || (pIdPersonaPide != null && s.id_persona_pide == pIdPersonaPide))
                                         orderby s.fecha_pedido descending, s.hora_pedido descending
                                         select s).ToListAsync<M_PEDIDO>();

                    return await ProcesaListaPedidosPersonalizados(pedidos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar en que rango entra el costo en base a un pedido
        /// <paramref name="pMetros"/>
        /// <returns> Objeto tipo E_TAFIRA_ENVIO con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_TAFIRA_ENVIO>> ConsultaCotoEnvioByDistancia(int pMetros)
        {
            try
            {

                using (context = new ViajesEntities())
                {
                    var tarifas = await (from s in context.TBL_TARIFA_ENVIO
                                         where  (pMetros >= s.distancia_menor  && pMetros <= s.distancia_mayor)
                                         select new E_TAFIRA_ENVIO {
                                             CostoEnvio = s.costo_envio,
                                             DistanciaMayor = s.distancia_mayor,
                                             DistanciaMenor = s.distancia_menor,
                                             IdTarifa = s.id_tarifa
                                         }).ToListAsync();

                    return tarifas;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<E_PEDIDO_PERSONALIZADO>> ProcesaListaPedidosPersonalizados(List<M_PEDIDO> pPedidos)
        {
            var listaPedidos = new List<E_PEDIDO_PERSONALIZADO>();

            foreach (var pedido in pPedidos)
            {
                var direccion = await new DireccionOperaciones().ConsultarDirecciones(pIdDireccion: pedido.id_direccion_entrega);
                var estatus = await new EstatusOperaciones().Consultar(pIdEstatus: pedido.id_estatus);
                var cliente = await new PersonaOperaciones().Consultar(pIdPersona: pedido.id_persona_pide);
                var conductor = await new ConductorOperaciones().Consultar(pIdPersona: (pedido.id_persona_entrega == null) ? 0 : pedido.id_persona_entrega);
                listaPedidos.Add(new E_PEDIDO_PERSONALIZADO
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
                    ReferenciaPago = pedido.referencia_pago,
                    CostoEnvio = pedido.costo_envio,
                    TipoPedido = pedido.tipo_pedido,
                    Propina = pedido.propina,
                    IdEstatusFactura = pedido.id_estatus_factura,
                    Iva = pedido.iva,
                    Detalle = await new DetallePedidoPersonalizadoOperaciones().Consultar(pIdPedido: pedido.id_pedido)
                }); 
            }

            return listaPedidos;
        }

        private async Task<List<E_PEDIDO>> ProcesaListaPedidos(List<M_PEDIDO> pPedidos)
        {   
            var listaPedidos = new List<E_PEDIDO>();

            foreach (var pedido in pPedidos)
            {
                var direccion = await new DireccionOperaciones().ConsultarDirecciones(pIdDireccion: pedido.id_direccion_entrega);
                var estatus = await  new EstatusOperaciones().Consultar(pIdEstatus: pedido.id_estatus);
                var cliente = await new PersonaOperaciones().Consultar(pIdPersona: pedido.id_persona_pide);
                var conductor = await new ConductorOperaciones().Consultar(pIdPersona: (pedido.id_persona_entrega == null)? 0: pedido.id_persona_entrega);
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
                    ReferenciaPago = pedido.referencia_pago,
                    CostoEnvio = pedido.costo_envio,
                    TipoPedido = pedido.tipo_pedido,
                    Propina = pedido.propina,
                    IdEstatusFactura = pedido.id_estatus_factura,
                    Iva = pedido.iva,
                    TiempoEspera = pedido.tiempo_espera,
                    Detalle = await new DetallePedidoOperaciones().Consultar(pIdPedido: pedido.id_pedido),
                }); ; ;
            }

            return listaPedidos;
        }
    }
}

