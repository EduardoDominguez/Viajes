using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Xml.Linq;
using Viajes.DAL.Modelo;
using Viajes.EL.Extras;
using Viajes.DAL.Direccion;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Persona
{
    public class ConductorOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar coordenadas de conductores activos
        /// <param name="pDatos">Objeto de tipo E_COORDENADAS_CONDUCTOR con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE AgregarCoordenadas(E_COORDENADAS_CONDUCTOR pDatos)
        {
            try
            {
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {

                    var coordenadas = context.Set<TBL_COORDENADAS_CONDUCTOR>();
                    coordenadas.Add(new TBL_COORDENADAS_CONDUCTOR {id_coordenada = Guid.NewGuid(), id_pedido = pDatos.IdPedido, id_persona = pDatos.IdPersona, longitud = pDatos.Longitud, latitud = pDatos.Latitud, fecha = DateTime.Now});

                    if(context.SaveChanges() > 0)
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

        /// <summary>
        /// Método para consultar coordenadas de un conductor
        /// <param name="pIdPersona">Id del conductor</param>
        /// <param name="pIdPedido">Id del pedido en caso de existir</param>
        /// <returns> Objeto tipo List<E_COORDENADAS_CONDUCTOR> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_COORDENADAS_CONDUCTOR>> ConsultarCoordenadas(int? pIdPersona = null, Guid? pIdPedido = null)
        {
            try
            {
                List<E_COORDENADAS_CONDUCTOR> listaCoordenadas = new List<E_COORDENADAS_CONDUCTOR>();
                using (context = new ViajesEntities())
                {
                    var coordenadas = await (from s in context.TBL_COORDENADAS_CONDUCTOR
                                   where
                                   (pIdPersona == null || (pIdPersona != null && s.id_persona == pIdPersona))
                                    && (pIdPedido == null || (pIdPedido != null && s.id_pedido == pIdPedido))
                                   select s).OrderByDescending(c => c.fecha).ToListAsync<TBL_COORDENADAS_CONDUCTOR>();


                    foreach (var coordenada in coordenadas)
                    {
                        listaCoordenadas.Add(new E_COORDENADAS_CONDUCTOR
                        {
                            IdCoordenada = coordenada.id_coordenada,
                            IdPedido = coordenada.id_pedido,
                            IdPersona = coordenada.id_persona,
                            Fecha = coordenada.fecha,
                            Latitud = coordenada.latitud,
                            Longitud = coordenada.longitud
                            
                        });
                    }

                    return listaCoordenadas;
                }
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
        public E_MENSAJE Agregar(E_PERSONA pPersona, E_ACCESO_PERSONA pAcceso, E_CONDUCTOR pDatosExtras)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    
                    ObjectParameter RET_ID_PERSONA = new ObjectParameter("RET_ID_PERSONA", typeof(string));
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PERSONA(pPersona.IdPersona,pPersona.Nombre, pPersona.Telefono, pPersona.Fotografia,
                        pAcceso.Email, pAcceso.Password, pAcceso.TipoUsuario, pAcceso.TokenFirebase, pPersona.Sexo,
                        pDatosExtras.Colonia, pDatosExtras.Calle, pDatosExtras.NoExt, pDatosExtras.NoInt, pDatosExtras.NoLicencia, pDatosExtras.NoPlacas, pDatosExtras.Tipo,
                        pAcceso.ClavePassword, "I", pPersona.IdPersonaMod, RET_ID_PERSONA, RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_ID_PERSONA = int.Parse(RET_ID_PERSONA.Value.ToString()), RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar conductor
        /// <param name="pPersona">Objeto de tipo E_PERSONA con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_PERSONA pPersona)
        {
            try
            {
                using (context = new ViajesEntities())
                {

                    ObjectParameter RET_ID_PERSONA = new ObjectParameter("RET_ID_PERSONA", typeof(string));
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PERSONA(pPersona.IdPersona, pPersona.Nombre, pPersona.Telefono, pPersona.Fotografia,
                        pPersona.Acceso.Email, pPersona.Acceso.Password, pPersona.Acceso.TipoUsuario, pPersona.Acceso.TokenFirebase, pPersona.Sexo,
                        pPersona.Conductor.Colonia, pPersona.Conductor.Calle, pPersona.Conductor.NoExt, pPersona.Conductor.NoInt, pPersona.Conductor.NoLicencia, pPersona.Conductor.NoPlacas, pPersona.Conductor.Tipo,
                        pPersona.Acceso.ClavePassword, "U", pPersona.IdPersonaMod, RET_ID_PERSONA, RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_ID_PERSONA = int.Parse(RET_ID_PERSONA.Value.ToString()), RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar conductores
        /// <param name="SoloActivos">Consultar solo repartidores activos o no</param>
        /// <param name="pIdPersona">Id del conductor  a consultar</param>
        /// <returns> Objeto tipo List<E_PERSONA> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PERSONA>> Consultar(byte? SoloActivos = null, int ? pIdPersona = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var conductores = await (from s in context.CTL_PERSONA 
                                         join ap in context.CTL_ACCESO_PERSONA on s.id_persona equals ap.id_persona
                                         join c in context.CTL_CONDUCTOR on s.id_persona equals c.id_persona
                                   where
                                   (pIdPersona == null || (pIdPersona != null && s.id_persona == pIdPersona))
                                   && (SoloActivos == null || (SoloActivos != null && s.estatus == SoloActivos))
                                   && ap.tipo_usuario == 2
                                             select new E_PERSONA
                                             {
                                                 IdPersona = s.id_persona,
                                                 Edad = s.edad,
                                                 Nombre = s.nombre,
                                                 Fotografia = s.fotografia,
                                                 Sexo = s.sexo,
                                                 Telefono = s.telefono,
                                                 Estatus = s.estatus,
                                                 Acceso = new E_ACCESO_PERSONA { 
                                                                IdPersona = ap.id_persona,
                                                                Email = ap.email,
                                                                ClavePassword = ap.clave_password,
                                                                FechaClavePassword = ap.fecha_clave_password,
                                                                Password = ap.password,
                                                                TipoUsuario = ap.tipo_usuario,
                                                                TokenFirebase = ap.token_firebase

                                                                },
                                                 Conductor = new E_CONDUCTOR { 
                                                                   Calle = c.calle,
                                                                   Colonia = c.colonia,
                                                                   IdPersona = c.id_persona,
                                                                   NoExt = c.no_ext,
                                                                   NoInt = c.no_int,
                                                                   NoLicencia = c.no_licencia, 
                                                                   NoPlacas = c.no_placas,
                                                                   Tipo = c.id_tipo ?? 0
                                                              } 
                                             }
                        ).ToListAsync();

                    return conductores;
                    //return ProcesaListaConductores(conductores);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<E_PERSONA> ProcesaListaConductores(List<CTL_PERSONA> pPersona)
        {
            var listaConductores = new List<E_PERSONA>();

            foreach (var conductor in pPersona)
            {
                //var accesos = await new AccesoOperaciones().Consultar(conductor.id_persona);
                //var accesos = await new AccesoOperaciones().Consultar(conductor.id_persona);


                listaConductores.Add(new E_PERSONA
                {
                    IdPersona = conductor.id_persona,
                    Edad = conductor.edad,
                    Nombre = conductor.nombre,
                    Fotografia = conductor.fotografia,
                    Sexo = conductor.sexo,
                    Telefono = conductor.telefono
                    
                });
            }

            return listaConductores;
        }
    }
}

