using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Persona;


namespace Viajes.BL.Persona
{
    public class ConductorNegocio : ICRUD<E_PERSONA>
    {
        /// <summary>
        /// Método para consultar agregar conductor
        /// <param name="Entidad">Datos del pedido a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Agregar(E_PERSONA Entidad)
        {
            try
            {
                ConductorOperaciones pDatos = new ConductorOperaciones();
                //return pDatos.Agregar(Entidad);
                return null;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para agregar coordenadas de los conductores activos
        /// <param name="Entidad">Datos de la entidad a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE AgregarCoordenadas(E_COORDENADAS_CONDUCTOR Entidad)
        {
            try
            {
                ConductorOperaciones pDatos = new ConductorOperaciones();
                return pDatos.AgregarCoordenadas(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar agregar conductores
        /// <param name="Entidad">Datos generales del conductor a agregar</param>
        /// <param name="pDatosAcceso">Datos de acceso de conductor</param>
        /// <param name="pDatosExtras">Datos adicionales de conductor</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Agregar(E_PERSONA Entidad, E_ACCESO_PERSONA pDatosAcceso, E_CONDUCTOR pDatosExtras)
        {
            try
            {
                ConductorOperaciones pDatos = new ConductorOperaciones();
                return pDatos.Agregar(Entidad, pDatosAcceso, pDatosExtras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar coordenadas
        /// <param name="pIdPersona">Id del conductor</param>
        /// <param name="pIdPedido">Id del pedido</param>
        /// <returns> Objeto tipo List<E_COORDENADAS_CONDUCTOR> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_COORDENADAS_CONDUCTOR>> ConsultarCoordenadas(int? pIdPersona = null, Guid? pIdPedido = null)
        {
            try
            {
                ConductorOperaciones pDatos = new ConductorOperaciones();
                return await pDatos.ConsultarCoordenadas(pIdPersona, pIdPedido);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Método para consultar conductores por id
        /// <param name="pIdPedido">Id del pedido a consultar</param>
        /// <returns> Objeto tipo E_PEDIDO con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_PERSONA> ConsultarPorId(int pIdPersona)
        {
            try
            {
                ConductorOperaciones pDatos = new ConductorOperaciones();
                var pResultado = await pDatos.Consultar(pIdPersona: pIdPersona);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar conductores
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_PERSONA con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PERSONA>> ConsultarTodo(byte? SoloActivos = null, int? idPersona = null)
        {
            try
            {
                ConductorOperaciones pDatos = new ConductorOperaciones();
                return await pDatos.Consultar(SoloActivos, idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public E_MENSAJE Editar(E_PERSONA Entidad)
        {
            throw new NotImplementedException();
        }
    }
}
