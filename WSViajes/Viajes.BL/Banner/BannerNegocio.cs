using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Banner;

namespace Viajes.BL.Banner
{
    public class BannerNegocio
    {
        /// <summary>
        /// Método para crear locales
        /// <param name="Entidad">Objeto de tipo E_BANNER con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_BANNER Entidad)
        {
            try
            {
                BannerOperaciones pDatos = new BannerOperaciones();
                return pDatos.Agregar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar local por id
        /// <param name="pIdLocal">Id de del local a consultar</param>
        /// <returns> Objeto tipo E_LOCAL con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_BANNER> ConsultarPorId(Guid pIdBanner)
        {
            try
            {
                BannerOperaciones pDatos = new BannerOperaciones();
                var pResultado = await pDatos.ConsultaBanners(pIdBanner: pIdBanner);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar locales
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_BANNER con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_BANNER>> ConsultarTodo(byte? SoloActivos = null, int? IdGenerico = null)
        {
            try
            {
                BannerOperaciones pDatos = new BannerOperaciones();
                return await pDatos.ConsultaBanners(pSoloActivos: SoloActivos);
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
        public E_MENSAJE CambiaEstatus(E_BANNER pEntidad)
        {
            try
            {
                BannerOperaciones pDatos = new BannerOperaciones();
                return pDatos.CambiaEstatus(pEntidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para editar locales
        /// <param name="Entidad">Objeto de tipo E_LOCAL con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_BANNER Entidad)
        {
            try
            {
                BannerOperaciones pDatos = new BannerOperaciones();
                return pDatos.Editar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
