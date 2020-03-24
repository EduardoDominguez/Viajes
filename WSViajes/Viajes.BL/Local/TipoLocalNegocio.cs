using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.DAL.Local;
using Viajes.EL.Interfaz;

namespace Viajes.BL.Local
{
    public class TipoLocalNegocio : ICRUD<E_TIPO_LOCAL>
    {
        public E_MENSAJE Agregar(E_TIPO_LOCAL pEntidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método para consultar tipos de locales por id
        /// <param name="pIdTipoLocal">Id del tipo de local a consultar</param>
        /// <returns> Objeto tipo List<E_TIPO_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_TIPO_LOCAL> ConsultarPorId(int pIdTipoLocal)
        {
            try
            {
                TipoLocalOperaciones pDatos = new TipoLocalOperaciones();
                var pResultado = await pDatos.ConsultarTipoLocal(pIdTipoLocal: pIdTipoLocal);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// Método para crear direcciones
        ///// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a insertar</param>
        ///// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        ///// </summary>
        //public E_MENSAJE CreaLocal(E_DIRECCION pDireccion)
        //{
        //    try
        //    {
        //        DireccionOperaciones pDatos = new DireccionOperaciones();
        //        return pDatos.CreaDireccion(pDireccion);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Método para actualizar direcciones
        ///// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a actualizar</param>
        ///// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        ///// </summary>
        //public E_MENSAJE ActualizaLocal(E_DIRECCION pDireccion)
        //{
        //    try
        //    {
        //        DireccionOperaciones pDatos = new DireccionOperaciones();
        //        return pDatos.ActualizaDireccion(pDireccion);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Método para consultar tipos de locales
        /// <param name="pSoloActivos">Indica si consultar solo activos o no</param>
        /// <param name="pIdTipoLocal">Id del tipo de local a consultar</param>
        /// <returns> Objeto tipo List<E_TIPO_LOCAL> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_TIPO_LOCAL>> ConsultarTodo(byte? pSoloActivos = null, int? pIdTipoLocal = null)
        {
            try
            {
                TipoLocalOperaciones pDatos = new TipoLocalOperaciones();
                return await pDatos.ConsultarTipoLocal(pSoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public E_MENSAJE Editar(E_TIPO_LOCAL pEntidad)
        {
            throw new NotImplementedException();
        }
    }
}

