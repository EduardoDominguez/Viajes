using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Local
{
    public class TipoLocalOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar direcciones
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE CreaLocal(E_DIRECCION pDireccion)
        {
            try
            {
                //using (context = new ViajesEntities())
                //{
                //    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                //    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                //    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                //    context.SP_DIRECCION(pDireccion.ID_DIRECCION, pDireccion.NOMBRE, pDireccion.CALLE,
                //                        pDireccion.COLONIA, pDireccion.DESCRIPCION, pDireccion.NO_EXT,
                //                         pDireccion.NO_INT, pDireccion.LATITUD, pDireccion.LONGITUD,
                //                         pDireccion.ID_PERSONA, pDireccion.ID_PERSONA_ALTA, pDireccion.ESTATUS, pDireccion.PREDETERMINADA, "I",
                //                        RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                //    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                //    return vMensaje;
                //}

                return new E_MENSAJE();
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
        public E_MENSAJE ActualizaLocal(E_DIRECCION pDireccion)
        {
            try
            {
                //using (context = new ViajesEntities())
                //{
                //    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                //    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                //    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                //    context.SP_DIRECCION(pDireccion.ID_DIRECCION, pDireccion.NOMBRE, pDireccion.CALLE,
                //                        pDireccion.COLONIA, pDireccion.DESCRIPCION, pDireccion.NO_EXT,
                //                         pDireccion.NO_INT, pDireccion.LATITUD, pDireccion.LONGITUD,
                //                         pDireccion.ID_PERSONA, pDireccion.ID_PERSONA_ALTA, pDireccion.ESTATUS, pDireccion.PREDETERMINADA, "U",
                //                        RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                //    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                //    return vMensaje;
                //}
                return new E_MENSAJE();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar tipos de locales
        /// <param name="pIdTipoLocal">Id del tipo de local a consultar</param>
        /// <returns> Objeto tipo List<E_TIPO_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_TIPO_LOCAL>> ConsultarTipoLocal(byte? pSoloActivos = null, int? pIdTipoLocal = null)
        {
            try
            {
                var listaTipoLocal = new List<E_TIPO_LOCAL>();
                using (context = new ViajesEntities())
                {
                    var tiposLocal = await (from s in context.CTL_TIPO_LOCAL
                                   where
                                   (pIdTipoLocal == null || (pIdTipoLocal != null && s.id_tipo_local == pIdTipoLocal))
                                   && (pSoloActivos == null || (pSoloActivos != null && s.estatus == pSoloActivos))
                                   select s).ToListAsync<CTL_TIPO_LOCAL>();

                    foreach (var tipoLocal in tiposLocal)
                    {
                        listaTipoLocal.Add(new E_TIPO_LOCAL
                        {
                            IdTipoLocal = tipoLocal.id_tipo_local,
                            Nombre = tipoLocal.nombre,
                            Descripcion = tipoLocal.descripcion,
                            Fotografia = tipoLocal.fotografia
                        });
                    }

                    return listaTipoLocal;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
