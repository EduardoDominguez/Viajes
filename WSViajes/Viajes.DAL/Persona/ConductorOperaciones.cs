using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using Viajes.DAL.Direccion;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Persona
{
    public class ConductorOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar coordenadas de conductores activos
        /// <param name="pDatos">Objeto de tipo E_COORDENADAS_CONDUCTOR con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE AgregarCoordenadas(E_COORDENADAS_CONDUCTOR pDatos)
        {
            try
            {
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {

                    var coordenadas = context.Set<TBL_COORDENADAS_CONDUCTOR>();
                    coordenadas.Add(new TBL_COORDENADAS_CONDUCTOR {id_coordenada = Guid.NewGuid(), id_pedido = pDatos.IdPedido, id_persona = pDatos.IdPersona, longitud = pDatos.Longitud, latitud = pDatos.Latitud, fecha = DateTime.Now});

                    if(context.SaveChanges() > 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Insertado correctamente", RET_VALORDEVUELTO = "Insertado correctamente" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo insertar la coordenada", RET_VALORDEVUELTO = "No se pudo insertar la coordenada" };

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar coordenadas de un conductor
        /// <param name="pIdPersona">Id del conductor</param>
        /// <param name="pIdPedido">Id del pedido en caso de existir</param>
        /// <returns> Objeto tipo List<E_COORDENADAS_CONDUCTOR> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_COORDENADAS_CONDUCTOR>> ConsultarCoordenadas(int? pIdPersona = null, Guid? pIdPedido = null)
        {
            try
            {
                List<E_COORDENADAS_CONDUCTOR> listaCoordenadas = new List<E_COORDENADAS_CONDUCTOR>();
                using (context = new ViajesEntities())
                {
                    var coordenadas = await (from s in context.TBL_COORDENADAS_CONDUCTOR
                                   where
                                   (pIdPersona == null || (pIdPersona != null && s.id_persona == pIdPersona))
                                    && (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                   select s).OrderByDescending(c => c.fecha).ToListAsync<TBL_COORDENADAS_CONDUCTOR>();


                    foreach (var coordenada in coordenadas)
                    {
                        listaCoordenadas.Add(new E_COORDENADAS_CONDUCTOR
                        {
                            IdCoordenada = coordenada.id_coordenada,
                            IdPedido = coordenada.id_pedido,
                            IdPersona = coordenada.id_persona,
                            Fecha = coordenada.fecha,
                            Latitud = coordenada.latitud,
                            Longitud = coordenada.longitud
                            
                        });
                    }

                    return listaCoordenadas;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para insertar conductores
        /// <param name="pPedido">Objeto de tipo E_PEDIDO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_PERSONA pPedido)
        {
            try
            {
                //using (context = new ViajesEntities())
                //{

                //    XElement xmlPedido = new XElement("PEDIDO");

                //    foreach (var detalle in pPedido.Detalle)
                //    {
                //        XElement xDetallePedido = new XElement("DETALLE");
                //        xDetallePedido.Add(
                //            new XAttribute("ID_LOCAL", detalle.Local.IdLocal),
                //            new XAttribute("ID_PRODUCTO", detalle.IdProducto),
                //            new XAttribute("PRECIO", detalle.Precio),
                //            new XAttribute("CANTIDAD", detalle.Cantidad),
                //            new XAttribute("OBSERVACIONES", detalle.Observaciones)
                //        );
                //        xmlPedido.Add(xDetallePedido);
                //    }

                //    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                //    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                //    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                //    context.SP_PEDIDO(pPedido.IdPedido, pPedido.IdPersonaPide, pPedido.DireccionEntrega.IdDireccion,
                //                        pPedido.IdPersonaEntrega, pPedido.Observaciones, pPedido.Folio,
                //                         pPedido.IdMetodoPago, pPedido.Estatus.IdEstatus, xmlPedido.ToString(), "I",
                //                        RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                //    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                //    return vMensaje;
                //}
                return null;
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
        /// Método para consultar conductores
        /// <param name="pIdPersona">Id del pedido a consultar</param>
        /// <returns> Objeto tipo List<E_PERSONA> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PERSONA>> Consultar(int? pIdPersona = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var pedidos = await (from s in context.CTL_PERSONA
                                   where
                                   (pIdPersona == null || (pIdPersona != null && s.id_persona == pIdPersona))
                                   select s).ToListAsync<CTL_PERSONA>();

                    return ProcesaListaConductores(pedidos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<E_PERSONA> ProcesaListaConductores(List<CTL_PERSONA> pPersona)
        {
            var listaConductores = new List<E_PERSONA>();

            foreach (var conductor in pPersona)
            {
                listaConductores.Add(new E_PERSONA
                {
                    IdPersona = conductor.id_persona,
                    Edad = conductor.edad,
                    Nombre = conductor.nombre,
                    Fotografia = conductor.fotografia,
                    Sexo = conductor.sexo,
                    Telefono = conductor.telefono
                });
            }

            return listaConductores;
        }
    }
}

