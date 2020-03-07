using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Direccion
{
    public class DireccionOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar direcciones
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE CreaDireccion(E_DIRECCION pDireccion)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_DIRECCION(pDireccion.IdDireccion, pDireccion.Nombre, pDireccion.Calle,
                                        pDireccion.Colonia, pDireccion.Descripcion, pDireccion.NoExt,
                                         pDireccion.NoInt, pDireccion.Latitud, pDireccion.Longitud,
                                         pDireccion.IdPersona, pDireccion.IdPersonaAlta, pDireccion.Estatus, pDireccion.Predeterminada, "I",
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
        /// Método para actualizar direcciones
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaDireccion(E_DIRECCION pDireccion)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_DIRECCION(pDireccion.IdDireccion, pDireccion.Nombre, pDireccion.Calle,
                                         pDireccion.Colonia, pDireccion.Descripcion, pDireccion.NoExt,
                                          pDireccion.NoInt, pDireccion.Latitud, pDireccion.Longitud,
                                          pDireccion.IdPersona, pDireccion.IdPersonaAlta, pDireccion.Estatus, pDireccion.Predeterminada, "U",
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
        /// Método para actualizar estatus direcciones
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaEstatusDireccion(E_DIRECCION pDireccion)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_DIRECCION(pDireccion.IdDireccion, pDireccion.Nombre, pDireccion.Calle,
                                        pDireccion.Colonia, pDireccion.Descripcion, pDireccion.NoExt,
                                         pDireccion.NoInt, pDireccion.Latitud, pDireccion.Longitud,
                                         pDireccion.IdPersona, pDireccion.IdPersonaAlta, pDireccion.Estatus, pDireccion.Predeterminada, "E",
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
        /// Método para actualizar direcci{on predeterminada
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaDireccionPredeterminada(E_DIRECCION pDireccion)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_DIRECCION(pDireccion.IdDireccion, pDireccion.Nombre, pDireccion.Calle,
                                        pDireccion.Colonia, pDireccion.Descripcion, pDireccion.NoExt,
                                         pDireccion.NoInt, pDireccion.Latitud, pDireccion.Longitud,
                                         pDireccion.IdPersona, pDireccion.IdPersonaAlta, pDireccion.Estatus, pDireccion.Predeterminada, "P",
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
        /// Método para consultar direcciones
        /// <param name="pIdDireccion">Id de de la dirección a consultar</param>
        /// <param name="pIdPersona">Id de la persona del la que se quieren consultar la dirección</param>
        /// <param name="pSoloActivos">Indica si se quieren solo activos o todos. 1: Solo activos, 0: Incluye inactivos. </param>
        /// <returns> Objeto tipo List<E_DIRECCION> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_DIRECCION>> ConsultarDirecciones(int? pIdDireccion = null, int? pIdPersona = null, byte? pSoloActivos = null)
        {
            try
            {
                var listaDirecciones = new List<E_DIRECCION>();
                using (context = new ViajesEntities())
                {
                    //var persona = context.CTL_PERSONA.SqlQuery(String.Format("SELECT * FROM dbo.CTL_PERSONA where id_persona", intIdPersona)).ToList();
                    var direcciones = await (from s in context.CTL_DIRECCIONES
                                        where
                                        (pIdDireccion == null || (pIdDireccion != null && s.id_direccion == pIdDireccion))
                                        && (pIdPersona == null || (pIdPersona != null && s.id_persona == pIdPersona))
                                        && (pSoloActivos == null || (pSoloActivos != null && s.estatus == pSoloActivos))
                                       select s).ToListAsync<CTL_DIRECCIONES>();

                    foreach (var direccion in direcciones)
                    {
                        listaDirecciones.Add( new E_DIRECCION {
                                               IdDireccion = direccion.id_direccion,
                                               IdPersona = direccion.id_persona,
                                               Nombre = direccion.nombre,
                                               Calle = direccion.calle,
                                               Colonia = direccion.colonia,
                                               Descripcion = direccion.descripcion,
                                               Latitud = direccion.latitud,
                                               Longitud = direccion.longitud,
                                               NoExt = direccion.no_ext,
                                               NoInt = direccion.no_int,
                                               Predeterminada = direccion.predeterminada
                                            });
                    }

                    return listaDirecciones;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
