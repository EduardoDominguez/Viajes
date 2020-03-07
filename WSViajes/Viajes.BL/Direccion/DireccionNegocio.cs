using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.DAL.Direccion;
using Viajes.EL.Interfaz;

namespace Viajes.BL.Direccion
{
    public class DireccionNegocio : ICRUD<E_DIRECCION>
    {
        /// <summary>
        /// Método para crear direcciones
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_DIRECCION pDireccion)
        {
            //throw new NotImplementedException();
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                return pDatos.CreaDireccion(pDireccion);
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
        public E_MENSAJE Editar(E_DIRECCION pDireccion)
        {
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                return pDatos.ActualizaDireccion(pDireccion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar estatus de la direcciones (dar de baja o activar)
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaEstatusDireccion(E_DIRECCION pDireccion)
        {
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                return pDatos.ActualizaEstatusDireccion(pDireccion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar la dirección predeterminada de una persona.
        /// <param name="pDireccion">Objeto de tipo E_DIRECCION con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaDireccionPredeterminada(E_DIRECCION pDireccion)
        {
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                return pDatos.ActualizaDireccionPredeterminada(pDireccion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar direcciones        
        /// <param name="pSoloActivos">Indica si se quieren solo activos o todos. 1: Solo activos, 0: Incluye inactivos. </param>
        /// <param name="pIdPersona">Id de la persona del la que se quieren consultar la dirección</param>
        /// <returns> Objeto tipo List<E_DIRECCION> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_DIRECCION>> ConsultarTodo(byte? pSoloActivos, int? pIdPersona)
        {
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                return await pDatos.ConsultarDirecciones(null, pIdPersona, pSoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar direcciones por id
        /// <param name="pIdDireccion">Id de de la dirección a consultar</param>
        /// <returns> Objeto tipo E_DIRECCION con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_DIRECCION> ConsultarPorId(int pId)
        {
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                var pResultado = await pDatos.ConsultarDirecciones(pId);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar la dirección predeterminada 
        /// <param name="pIdDireccion">Id de de la dirección a consultar</param>
        /// <returns> Objeto tipo E_DIRECCION con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_DIRECCION> ConsultarPredeterminada(int pIdPersona)
        {
            try
            {
                DireccionOperaciones pDatos = new DireccionOperaciones();
                var pResultado = await pDatos.ConsultarDirecciones(pIdPersona: pIdPersona);
                return pResultado.Where(d=> d.Predeterminada == 1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
