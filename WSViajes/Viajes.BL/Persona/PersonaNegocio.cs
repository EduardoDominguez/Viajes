using System;
using System.Collections.Generic;
using System.Linq;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Persona;
using System.Threading.Tasks;

namespace Viajes.BL.Persona
{
    public class PersonaNegocio : ICRUD<E_PERSONA>
    {
        /// <summary>
        /// Método para consultar agregar persona
        /// <param name="Entidad">Datos a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Agregar(E_PERSONA Entidad)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return pDatos.Agregar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar personas por id
        /// <param name="pIdPersona">Id de la persona a consultar</param>
        /// <returns> Objeto tipo E_PERSONA con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_PERSONA> ConsultarPorId(int pIdPersona)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                var pResultado = await pDatos.Consultar(pIdPersona: pIdPersona);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar personas
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_PERSONA con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PERSONA>> ConsultarTodo(byte? SoloActivos = null, int? idPersona = null)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return await pDatos.Consultar(pSoloActivos: SoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar editar persona
        /// <param name="Entidad">Datosa editar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Editar(E_PERSONA Entidad)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método para agregar una relación de cliente openpay con cliente interno
        /// <param name="pIdPersona">Id de la persona a existente</param>
        /// /// <param name="pIdCustomerOpenPay">Id generado por OpenPay para el cliente.</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE AgregarClienteOpenPay(int pIdPersona, string pIdCustomerOpenPay)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return pDatos.AgregarClienteOpenPay(pIdPersona, pIdCustomerOpenPay);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
