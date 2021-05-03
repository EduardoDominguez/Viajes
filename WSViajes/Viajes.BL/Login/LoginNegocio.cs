using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viajes.EL.Extras;
using Viajes.DAL.Login;

namespace Viajes.BL.Login
{
    public class LoginNegocio
    {
        /// <summary>
        /// Método para realizar inicio de sesión
        /// <param name="pAcceso">Objeto de tipo E_ACCESO_PERSONA con datos del inicio de sesión</param>
        /// <returns> Objeto tipo E_LOGIN con los datos del movimiento </returns>  
        /// </summary>
        public E_LOGIN Login(E_ACCESO_PERSONA pAccceso)
        {
            try
            {
                LoginOperaciones pDatos = new LoginOperaciones();
                return pDatos.Login(pAccceso);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para realizar crear una persona
        /// <param name="pPersona">Objeto de tipo E_PERSONA con datos de la persona a insertar</param>
        /// /// <param name="pAcceso">Objeto de tipo E_ACCESO_PERSONA con datos para inicio de sesión</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE CreaPersona(E_PERSONA pPersona, E_ACCESO_PERSONA pAcceso)
        {
            try
            {
                LoginOperaciones pDatos = new LoginOperaciones();
                return pDatos.CreaPersona(pPersona, pAcceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para realizar crear una persona
        /// <param name="pPersona">Objeto de tipo E_PERSONA con datos de la persona a insertar</param>
        /// /// <param name="pAcceso">Objeto de tipo E_ACCESO_PERSONA con datos para inicio de sesión</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizarPersona(E_PERSONA pPersona, E_ACCESO_PERSONA pAcceso)
        {
            try
            {
                LoginOperaciones pDatos = new LoginOperaciones();
                return pDatos.CreaPersona(pPersona, pAcceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para actualizar token depassword
        /// <param name="pIdPersona">Identidicador de la persona</param>
        /// /// <param name="pTokenPassword">Clave  para generar password</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizarTokenPassword(int pIdPersona, Guid pTokenPassword)
        {
            try
            {
                LoginOperaciones pDatos = new LoginOperaciones();
                return pDatos.ActializarTokenPassword(pIdPersona, pTokenPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
