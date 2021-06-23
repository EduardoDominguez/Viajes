using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Local;

namespace Viajes.BL.Local
{
    public class LocalNegocio : ICRUD<E_LOCAL>
    {
        /// <summary>
        /// Método para crear locales
        /// <param name="Entidad">Objeto de tipo E_LOCAL con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_LOCAL Entidad)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
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
        public async Task<E_LOCAL> ConsultarPorId(int pIdlocal)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
                var pResultado = await pDatos.ConsultarLocales(pIdlocal);
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
        /// <returns> Objeto tipo E_LOCAL con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_LOCAL>> ConsultarTodo(byte? SoloActivos = null, int? IdGenerico = null)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
                return await pDatos.ConsultarLocales(pSoloActivos: SoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar locales por tipo local
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_LOCAL con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_LOCAL>> ConsultarByTipoLocal(int idTipoLocal)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
                return await pDatos.ConsultarByTipoLocal(idTipoLocal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar locales por id persona responsable
        /// <param name="idPersona">Identificador de la persona</param>
        /// <returns> Objeto tipo E_LOCAL con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_LOCAL> ConsultarByIdPersonaResponsable(int idPersona)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
                var locales = await pDatos.ConsultarByIdPersonaResponsable(idPersona);
                return locales.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <summary>
        /// Método para cambiar el estatus de locales
        /// <param name="pEntidad">Objeto con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con resultado de la operación. </returns>  
        /// </summary>
        public E_MENSAJE CambiaEstatus(E_LOCAL pEntidad)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
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
        public E_MENSAJE Editar(E_LOCAL Entidad)
        {
            try
            {
                LocalOperaciones pDatos = new LocalOperaciones();
                return pDatos.Editar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
