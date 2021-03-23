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
    public class PersonaOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar personas
        /// <param name="pPersona">Objeto de tipo E_PERSONA con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_PERSONA pPersona)
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
        /// Método para actualizar persona
        /// <param name="pPersona">Objeto de tipo E_PERSONA con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_PERSONA pPersona)
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
        /// Método para consultar personas
        /// <param name="pIdPersona">Id del producto a consultar</param>
        /// <param name="pSoloActivos">Indica si deben de consultarse solo personas activas</param>
        /// <param name="pIdsTipoUsuario">Identificadores de idtipopersona separados por coma</param>
        /// <param name="pToken">Token de acceso buscado</param>
        /// <returns> Objeto tipo List<E_PERSONA> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PERSONA>> Consultar(int? pIdPersona = null, byte? pSoloActivos = null, string pIdsTipoUsuario = null, string pToken = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    byte[] idsTipoUsuario;

                    if (!string.IsNullOrEmpty(pIdsTipoUsuario))
                        idsTipoUsuario  = Array.ConvertAll(pIdsTipoUsuario.Split(','), byte.Parse);

                    var personas = await (from p in context.CTL_PERSONA
                                          join acceso in context.CTL_ACCESO_PERSONA on p.id_persona equals acceso.id_persona

                                          where
                                           (pIdPersona == null || (pIdPersona != null && p.id_persona == pIdPersona))
                                      && (pSoloActivos == null || (pSoloActivos != null && p.estatus == pSoloActivos))
                                      && (pIdsTipoUsuario == null || (pIdsTipoUsuario != null && pIdsTipoUsuario.Contains(acceso.tipo_usuario.ToString())))
                                      && (pToken == null || (pToken != null && acceso.clave_password == pToken))
                                          select p).ToListAsync<CTL_PERSONA>();

                    return await procesaPersonas(personas);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para maperar los datos de la  productos
        /// <param name="pPersonas">LIsta de personas desde la BD</param>
        /// <returns> Objeto tipo List<E_PERSONA> con los datos solicitados </returns>  
        /// </summary>
        private async Task<List<E_PERSONA>> procesaPersonas(List<CTL_PERSONA> pPersonas)
        {
                var listaPersonas = new List<E_PERSONA>();

                foreach (var persona in pPersonas)
                {
                    var accesos = await new AccesoOperaciones().Consultar(persona.id_persona);
                    listaPersonas.Add(new E_PERSONA
                    {
                        IdPersona = persona.id_persona,
                        Nombre = persona.nombre,
                        Edad = persona.edad,
                        Sexo = persona.sexo,
                        Fotografia = persona.fotografia,
                        Telefono = persona.telefono,
                        Estatus = persona.estatus,
                        Acceso = accesos.FirstOrDefault()
                        //IdPersonaAlta = producto.id_persona_alta,
                        //IdPersonaModifica = producto.id_persona_mod ?? 0
                    });
                }
                return listaPersonas;
        }

        /// <summary>
        /// Método para agregar una relación de cliente openpay con cliente interno
        /// <param name="pIdPersona">Id de la persona a existente</param>
        /// /// <param name="pIdCustomerOpenPay">Id generado por OpenPay para el cliente.</param>
        /// <returns> Objeto tipo E_MENSAJE con el resultado de la operación </returns>  
        /// </summary>     
        public E_MENSAJE AgregarClienteOpenPay(int pIdPersona,  string pIdCustomerOpenPay)
        {
            try
            {
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {

                    var coordenadas = context.Set<R_PERSONA_OPENPAY>();
                    coordenadas.Add(new R_PERSONA_OPENPAY { id_persona = pIdPersona, id_customer = pIdCustomerOpenPay });

                    if (context.SaveChanges() > 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Insertado correctamente", RET_VALORDEVUELTO = "Insertado correctamente" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo insertar la coordenada", RET_VALORDEVUELTO = "No se pudo insertar la coordenada" };

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<string> ConsultarClienteIdOpenPay(int pIdPersona)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var rPersona = await(from p in context.R_PERSONA_OPENPAY
                                         where
                                         p.id_persona == pIdPersona
                                         select p).ToListAsync<R_PERSONA_OPENPAY>();

                    return rPersona.FirstOrDefault().id_customer;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
