﻿using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Login
{
    public class LoginOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para realizar inicio de sesión
        /// <param name="pAcceso">Objeto de tipo E_ACCESO_PERSONA con datos del inicio de sesión</param>
        /// <returns> Objeto tipo E_LOGIN con los datos del movimiento </returns>  
        /// </summary>
        public E_LOGIN Login(E_ACCESO_PERSONA pAcceso)
        {
            try
            {
                var respuesta = new E_LOGIN();
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_ID_PERSONA = new ObjectParameter("RET_ID_PERSONA", typeof(string));
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_LOGIN(pAcceso.Email, pAcceso.Password, pAcceso.TipoUsuario, RET_ID_PERSONA,
                                    RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    var intIdPersona = int.Parse(RET_ID_PERSONA.Value.ToString());

                    if (vMensaje.RET_NUMEROERROR == 0)
                    {
                        //Login correcto, carga datos de la persona
                        respuesta.CORRECTO = true;
                        respuesta.MENSAJE = vMensaje.RET_MENSAJEERROR;

                  
                        //var persona = context.CTL_PERSONA.SqlQuery(String.Format("SELECT * FROM dbo.CTL_PERSONA where id_persona", intIdPersona)).ToList();
                        var persona = (from s in context.CTL_PERSONA
                                        where s.id_persona == intIdPersona
                                       select s).ToList<CTL_PERSONA>().FirstOrDefault();
                        respuesta.PERSONA = new E_PERSONA() {IdPersona = persona.id_persona, Nombre= persona.nombre, Fotografia = persona.fotografia, Telefono = persona.telefono};

                    }
                    else
                    {
                        respuesta.CORRECTO = false;
                        respuesta.MENSAJE = vMensaje.RET_VALORDEVUELTO;
                    }
                    return respuesta;
                }
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
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PERSONA(pPersona.Nombre, pPersona.Telefono, pPersona.Fotografia,
                        pAcceso.Email, pAcceso.Password, pAcceso.TipoUsuario, pAcceso.TokenFirebase, RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
