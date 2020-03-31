using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Persona
{
    public class AccesoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar acceso de personas
        /// <param name="pPersona">Objeto de tipo E_ACCESO_PERSONA con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_ACCESO_PERSONA pAcceso)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    /*context.SP_PRODUCTO(pProducto.IdProducto, pProducto.Nombre, pProducto.Descripcion, pProducto.Precio,
                                         pProducto.Fotografia, pProducto.IdLocal, pProducto.IdPersonaAlta, pProducto.Estatus, "I",
                                         RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);*/

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar acceesos de persona
        /// <param name="pPersona">Objeto de tipo E_PERSONA con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_ACCESO_PERSONA pAcceso)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    /*context.SP_PRODUCTO(pProducto.IdProducto, pProducto.Nombre, pProducto.Descripcion, pProducto.Precio,
                                         pProducto.Fotografia, pProducto.IdLocal, pProducto.IdPersonaModifica, pProducto.Estatus, "U",
                                         RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);*/

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para consultar accesos persona
        /// <param name="pIdPersona">Id de la persona a consultar</param>
        /// <param name="pSoloActivos">Indica si deben de consultarse solo personas activas</param>
        /// <returns> Objeto tipo List<E_ACCESO_PERSONA> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_ACCESO_PERSONA>> Consultar(int? pIdPersona = null, byte? pSoloActivos = null, string pCorreo = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var accesos = await(from a in context.CTL_ACCESO_PERSONA
                                    join p in context.CTL_PERSONA on a.id_persona equals p.id_persona
                                    where
                                     (pIdPersona == null || (pIdPersona != null && a.id_persona == pIdPersona))
                                     && (pSoloActivos == null || (pSoloActivos != null && p.estatus == pSoloActivos))
                                     && (string.IsNullOrEmpty(pCorreo) || (!string.IsNullOrEmpty(pCorreo) && a.email == pCorreo))
                                    select a).ToListAsync<CTL_ACCESO_PERSONA>();

                    return procesaAccesos(accesos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para maperar los datos de la  productos
        /// <param name="pAccesos">LIsta de accesos de personas desde la BD</param>
        /// <returns> Objeto tipo List<E_ACCESO_PERSONA> con los datos solicitados </returns>  
        /// </summary>
        private List<E_ACCESO_PERSONA> procesaAccesos(List<CTL_ACCESO_PERSONA> pAccesos)
        {
            var listaAccesos = new List<E_ACCESO_PERSONA>();

            foreach (var acceso in pAccesos)
            {
                listaAccesos.Add(new E_ACCESO_PERSONA
                {
                    IdPersona = acceso.id_persona,
                    Email = acceso.email,
                    Password = acceso.password,
                    TipoUsuario = acceso.tipo_usuario,
                    TokenFirebase = acceso.token_firebase
                    //IdPersonaAlta = producto.id_persona_alta,
                    //IdPersonaModifica = producto.id_persona_mod ?? 0
                });
            }
            return listaAccesos;
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
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {
                    var accesos = context.CTL_ACCESO_PERSONA.Where(p => p.id_persona == pIdPersona).FirstOrDefault();

                    if (accesos != null)
                        accesos.token_firebase = pToken;


                    if (context.SaveChanges() > 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Token actualizado correctamente", RET_VALORDEVUELTO = "Token actualizado correctamente" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo actualizar el token", RET_VALORDEVUELTO = "No se pudo actualizar el token" };

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
