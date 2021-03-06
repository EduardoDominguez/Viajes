﻿using System;
using System.Collections.Generic;
using System.Linq;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Persona;
using System.Threading.Tasks;

namespace Viajes.BL.Persona
{
    public class PersonaNegocio
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
        /// Método para consultar personas por token acceso
        /// <param name="pToken">Token de acceso a consultar</param>
        /// <returns> Objeto tipo E_PERSONA con los datos solicitados </returns>  
        /// </summary>       
        public async Task<E_PERSONA> ConsultarPorToken(string pToken)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                var pResultado = await pDatos.Consultar(pToken: pToken);
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
        /// <param name="idPersona">Identificador de la persona a consultar</param>
        /// <param name="idsTipoUsuario">Identificadores de idtipopersona separados por coma</param>
        /// <returns> Objeto tipo E_PERSONA con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PERSONA>> ConsultarTodo(byte? SoloActivos = null, int? idPersona = null, string idsTipoUsuario = null)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return await pDatos.Consultar(pSoloActivos: SoloActivos, pIdPersona: idPersona, pIdsTipoUsuario: idsTipoUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para editar persona
        /// <param name="Entidad">Datosa editar</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE Editar(E_PERSONA Entidad)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return pDatos.Editar(Entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        
        /// <summary>
        /// Método para consultar cliente OpenPay 
        /// <param name="pIdPersona">Id de la persona a consultar</param>
        /// <returns> Objeto tipo string con los datos solicitados </returns>  
        /// </summary>       
        public async Task<string> ConsultarClienteIdOpenPay(int pIdPersona)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return await pDatos.ConsultarClienteIdOpenPay(pIdPersona: pIdPersona);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para editar  el estatus de una persona
        /// <param name="pIdPersona">IdPersona a editar</param>
        /// <param name="pIdEstatus">Estatus a actualizar</param>
        /// <param name="pIdPersonaModifica">Persona que realiza el movimiento</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>       
        public E_MENSAJE ActualizaEstatusRegistro(int pIdPersona, byte pIdEstatus, byte pIdPersonaModifica)
        {
            try
            {
                PersonaOperaciones pDatos = new PersonaOperaciones();
                return pDatos.ActualizaEstatusRegistro(pIdPersona, pIdEstatus, pIdPersonaModifica);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
