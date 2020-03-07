using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Viajes.DAL.Local
{
    public class CostoLocalOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar costos de locales
        /// <param name="pCosto">Objeto de tipo E_COSTO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_COSTO pCosto)
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
        /// Método para actualizar costos de locales
        /// <param name="pCosto">Objeto de tipo E_COSTO con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_COSTO pCosto)
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
        /// Método para consultar locales
        /// <param name="pIdCosto">Id del costo a consultar</param>
        /// <param name="pSoloActivos">Indica consultar solo activos o no</param>
        /// <returns> Objeto tipo List<E_COSTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_COSTO>> Consultar(int? pIdCosto = null, byte? pSoloActivos = null)
        {
            try
            {
                var listaCostos = new List<E_COSTO>();
                using (context = new ViajesEntities())
                {
                    var costos = await (from s in context.CTL_COSTO
                                   where
                                   (pIdCosto == null || (pIdCosto != null && s.id_costo == pIdCosto))
                                    && (pSoloActivos == null || (pSoloActivos != null && s.estatus == pSoloActivos))

                                   select s).ToListAsync<CTL_COSTO>();

                    foreach (var costo in costos)
                    {
                        listaCostos.Add(new E_COSTO
                        {
                            IdCosto = costo.id_costo,
                            Nombre = costo.nombre,
                            Descripcion = costo.descripcion,
                            Estatus = costo.estatus
                        });
                    }
                    return listaCostos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
