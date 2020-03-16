using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Banner
{
    public class BannerOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar 
        /// <param name="pBanner">Objeto de tipo E_BANNER con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_BANNER pBanner)
        {
            try
            {
                //using (context = new ViajesEntities())
                //{
                //    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                //    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                //    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                //    context.SP_LOCAL(pLocal.IdLocal, pLocal.Nombre, pLocal.Referencias, pLocal.Latitud, pLocal.Longitud,
                //                         pLocal.Fotografia, pLocal.Calle, pLocal.Colonia, pLocal.NoExt, pLocal.NoInt,
                //                         pLocal.Costo.IdCosto, pLocal.TipoLocal.IdTipoLocal, pLocal.IdPersonaAlta, pLocal.Estatus, "I",
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
        /// Método para actualizar
        /// <param name="pBanner">Objeto de tipo E_BANNER con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_BANNER pBanner)
        {
            try
            {
                //using (context = new ViajesEntities())
                //{
                //    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                //    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                //    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                //    context.SP_LOCAL(pLocal.IdLocal, pLocal.Nombre, pLocal.Referencias, pLocal.Latitud, pLocal.Longitud,
                //                         pLocal.Fotografia, pLocal.Calle, pLocal.Colonia, pLocal.NoExt, pLocal.NoInt,
                //                         pLocal.Costo.IdCosto, pLocal.TipoLocal.IdTipoLocal, pLocal.IdPersonaModifica, pLocal.Estatus, "U",
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
        /// Método para consultar banners
        /// <param name="pIdLocal">Id del local a consultar</param>
        /// <param name="pSoloActivos">Indica consultar solo activos o no</param>
        /// <returns> Objeto tipo List<E_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_BANNER>> ConsultaBanners(string pIdBanner = null, byte? pSoloActivos = null)
        {
            try
            {
                //var  listaBanner = new List<E_BANNER>();
                using (context = new ViajesEntities())
                {
                    var banners = await (from s in context.TBL_BANNERS
                                         //where
                                         //(pIdLocal == null || (pIdLocal != null && s.id_local == pIdLocal))
                                          //&& (pSoloActivos == null || (pSoloActivos != null && s.estatus == pSoloActivos))
                                         select s).ToListAsync<TBL_BANNERS>();

                    return procesaBanners(banners);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para cambiar el estatus
        /// <param name="pBanner">Objeto con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con resultado de la operación. </returns>  
        /// </summary>
        public E_MENSAJE CambiaEstatus(E_BANNER pBanner)
        {
            try
            {
                E_MENSAJE vMensaje = new E_MENSAJE();
                return vMensaje;
                //using (context = new ViajesEntities())
                //{


                    //var localActual = context.CTL_LOCAL.Where(l => l.id_local == pLocal.IdLocal).FirstOrDefault();

                    //if (localActual != null)
                    //{
                    //    //var localEntity = context.Set<CTL_LOCAL>();
                    //    localActual.estatus = pLocal.Estatus;
                    //    localActual.id_persona_mod = pLocal.IdPersonaModifica;
                    //    localActual.fecha_mod = DateTime.Now;

                    //}


                    //if (context.SaveChanges() > 0)
                    //    vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Estatus actualizado correctamente", RET_VALORDEVUELTO = "Insertado correctamente" };
                    //else
                    //    vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo actualizar el estatus", RET_VALORDEVUELTO = "No se pudo actualizar el estatus" };

                //    return vMensaje;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<E_BANNER> procesaBanners(List<TBL_BANNERS> pBanners)
        {
            var listaBanners = new List<E_BANNER>();
            foreach (var banner in pBanners)
            {

                listaBanners.Add(new E_BANNER
                {
                    IdBanner = banner.id_banner,
                    Nombre = banner.nombre,
                    Fotografia = banner.fotografia,
                    IdProducto = banner.id_producto
                });
            }
            return listaBanners;
        }
    }
}
