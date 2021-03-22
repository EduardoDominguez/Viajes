using System;
using System.Collections.Generic;
using System.Linq;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Persona;
using System.Threading.Tasks;


namespace Viajes.BL.Persona
{
    public class AccesoNegocio : ICRUD<E_ACCESO_PERSONA>
    {
        /// <summary>
        /// Método para consultar agregar accesos de persona
        /// <param name="Entidad">Datos a agregar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Agregar(E_ACCESO_PERSONA Entidad)
        {
            try
            {
                AccesoOperaciones pDatos = new AccesoOperaciones();
                return pDatos.Agregar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar accesos personas por id
        /// <param name="pIdPersona">Id de la persona a consultar</param>
        /// <returns> Objeto tipo E_ACCESO_PERSONA con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_ACCESO_PERSONA> ConsultarPorId(int pIdPersona)
        {
            try
            {
                AccesoOperaciones pDatos = new AccesoOperaciones();
                var pResultado = await pDatos.Consultar(pIdPersona: pIdPersona);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar personas por id
        /// <param name="pCorreo">Correo de la persona a consultar</param>
        /// <returns> Objeto tipo E_PERSONA con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_ACCESO_PERSONA> ConsultaPorCorreo(string pCorreo)
        {
            try
            {
                AccesoOperaciones pDatos = new AccesoOperaciones();
                var pResultado = await pDatos.Consultar(pCorreo: pCorreo);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar accesos personas
        /// <param name="SoloActivos">Consultar solo activos o no</param>
        /// <returns> Objeto tipo E_ACCESO_PERSONA con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_ACCESO_PERSONA>> ConsultarTodo(byte? SoloActivos = null, int? idPersona = null)
        {
            try
            {
                AccesoOperaciones pDatos = new AccesoOperaciones();
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
        public E_MENSAJE Editar(E_ACCESO_PERSONA Entidad)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Método para actualizar token firebase
        /// <param name="pIdPersona">Persona del token editar</param>
        /// <param name="pToken">Token nuevo</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE ActualizaToken(int pIdPersona, string pToken)
        {
            try
            {
                AccesoOperaciones pDatos = new AccesoOperaciones();
                return pDatos.ActualizaToken(pIdPersona, pToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para actualizar contraseña
        /// <param name="pIdPersona">Persona de la contraseña a editar</param>
        /// <param name="pPassword">Nueva contraseña</param>
        /// <param name="pTokenPassword">En caso de existir token de seguridad</param>
        /// <param name="pTipoUsuario">Tipo de usuario al que se le actualiza la contraseña</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE ActualizaPassword(int pIdPersona, string pPassword,string pTokenPassword, int pTipoUsuario)
        {
            try
            {
                AccesoOperaciones pDatos = new AccesoOperaciones();
                return pDatos.ActualizaPassword(pIdPersona, pPassword, pTokenPassword, pTipoUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        }
}
