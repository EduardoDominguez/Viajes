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
    public class LocalOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar locales
        /// <param name="pDireccion">Objeto de tipo E_LOCAL con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_LOCAL pLocal)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_LOCAL(pLocal.IdLocal, pLocal.Nombre, pLocal.Referencias, pLocal.Latitud, pLocal.Longitud, 
                                         pLocal.Fotografia, pLocal.Calle, pLocal.Colonia, pLocal.NoExt, pLocal.NoInt, 
                                         pLocal.Costo.IdCosto, pLocal.TipoLocal.IdTipoLocal, pLocal.IdPersonaAlta, pLocal.Estatus, "I",
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
        /// Método para actualizar locales
        /// <param name="pDireccion">Objeto de tipo E_LOCAL con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_LOCAL pLocal)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_LOCAL(pLocal.IdLocal, pLocal.Nombre, pLocal.Referencias, pLocal.Latitud, pLocal.Longitud,
                                         pLocal.Fotografia, pLocal.Calle, pLocal.Colonia, pLocal.NoExt, pLocal.NoInt,
                                         pLocal.Costo.IdCosto, pLocal.TipoLocal.IdTipoLocal, pLocal.IdPersonaModifica, pLocal.Estatus, "U",
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
        /// Método para consultar locales por tipo local
        /// <param name="pIdTipoLocal">Id del tipo local a consultar</param>
        /// <returns> Objeto tipo List<E_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_LOCAL>> ConsultarByTipoLocal(int pIdTipoLocal)
        {
            try
            {
                var listaLocales = new List<E_LOCAL>();
                using (context = new ViajesEntities())
                {
                    var locales = await (from s in context.CTL_LOCAL
                                         where
                                         s.id_tipo_local == pIdTipoLocal
                                         select s).ToListAsync<CTL_LOCAL>();

                    return await procesaLocales(locales);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar locales
        /// <param name="pIdLocal">Id del local a consultar</param>
        /// <param name="pSoloActivos">Indica consultar solo activos o no</param>
        /// <returns> Objeto tipo List<E_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_LOCAL>> ConsultarLocales(int? pIdLocal = null, byte? pSoloActivos = null)
        {
            try
            {
                var listaLocales = new List<E_LOCAL>();
                using (context = new ViajesEntities())
                {
                    var locales = await (from s in context.CTL_LOCAL
                                         where
                                         (pIdLocal == null || (pIdLocal != null && s.id_local == pIdLocal))
                                          && (pSoloActivos == null || (pSoloActivos != null && s.estatus == pSoloActivos))
                                         select s).ToListAsync<CTL_LOCAL>();

                    return await procesaLocales(locales);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para cambiar el estatus de locales
        /// <param name="pEntidad">Objeto con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con resultado de la operación. </returns>  
        /// </summary>
        public E_MENSAJE CambiaEstatus(E_LOCAL pLocal)
        {
            try
            {
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {


                    var localActual = context.CTL_LOCAL.Where(l => l.id_local == pLocal.IdLocal).FirstOrDefault();

                    if(localActual != null)
                    {
                        //var localEntity = context.Set<CTL_LOCAL>();
                        localActual.estatus = pLocal.Estatus;
                        localActual.id_persona_mod = pLocal.IdPersonaModifica;
                        localActual.fecha_mod = DateTime.Now;

                    }
                    

                    if (context.SaveChanges() > 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Estatus actualizado correctamente", RET_VALORDEVUELTO = "Insertado correctamente" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo actualizar el estatus", RET_VALORDEVUELTO = "No se pudo actualizar el estatus" };

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<E_LOCAL>> procesaLocales(List<CTL_LOCAL> pLocales)
        {
            var listaLocales = new List<E_LOCAL>();
            foreach (var local in pLocales)
            {
                var costo = await new CostoLocalOperaciones().Consultar(local.id_costo);
                var tipoLocal = await new TipoLocalOperaciones().ConsultarTipoLocal(pIdTipoLocal: local.id_tipo_local);
                listaLocales.Add(new E_LOCAL
                {
                    IdLocal = local.id_local,
                    Nombre = local.nombre,
                    Calle = local.calle,
                    Colonia = local.colonia,
                    Referencias = local.referencias,
                    Fotografia = local.fotografia,
                    Latitud = local.latitud,
                    Longitud = local.longitud,
                    NoExt = local.no_ext,
                    NoInt = local.no_int,
                    Estatus = local.estatus,
                    Costo = costo.FirstOrDefault(),
                    TipoLocal = tipoLocal.FirstOrDefault()
                });
            }
            return listaLocales;
        }
    }
}
