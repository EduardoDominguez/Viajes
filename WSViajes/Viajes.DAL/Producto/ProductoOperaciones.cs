﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Viajes.DAL.Modelo;
using Viajes.DAL.Local;
using Viajes.EL.Extras;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Viajes.DAL.Producto
{
    public class ProductoOperaciones
    {
        private ViajesEntities context;

        /// <summary>
        /// Método para insertar productos
        /// <param name="pProducto">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE CreaProducto(E_PRODUCTO pProducto)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PRODUCTO(pProducto.IdProducto, pProducto.Nombre, pProducto.Descripcion, pProducto.Precio,
                                         pProducto.Fotografia, pProducto.Local.IdLocal, pProducto.IdPersonaAlta, pProducto.Estatus, "I",
                                         RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

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
        /// Método para actualizar producto
        /// <param name="pProducto">Objeto de tipo E_PRODUCTO con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizaProducto(E_PRODUCTO pProducto)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));


                    context.SP_PRODUCTO(pProducto.IdProducto, pProducto.Nombre, pProducto.Descripcion, pProducto.Precio,
                                         pProducto.Fotografia, pProducto.Local.IdLocal, pProducto.IdPersonaModifica, pProducto.Estatus, "U",
                                         RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

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
        /// Método para cambiar el estatus de productos
        /// <param name="pEntidad">Objeto con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con resultado de la operación. </returns>  
        /// </summary>
        public E_MENSAJE CambiaEstatus(E_PRODUCTO pProducto)
        {
            try
            {
                E_MENSAJE vMensaje;
                using (context = new ViajesEntities())
                {


                    var productoActual = context.CTL_PRODUCTO.Where(p => p.id_producto == pProducto.IdProducto).FirstOrDefault();

                    if (productoActual != null)
                    {
                        //var productoEntity = context.Set<CTL_PRODUCTO>();
                        productoActual.estatus = pProducto.Estatus;
                        productoActual.id_persona_mod = pProducto.IdPersonaModifica;
                        productoActual.fecha_mod = DateTime.Now;

                    }


                    if (context.SaveChanges() > 0)
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Estatus actualizado correctamente", RET_VALORDEVUELTO = "Insertado correctamente" };
                    else
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -1000, RET_MENSAJEERROR = "No se pudo actualizar el estatus", RET_VALORDEVUELTO = "No se pudo actualizar el estatus" };

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar productos
        /// <param name="pIdProducto">Id del producto a consultar</param>
        /// <param name="pIdLocal">Id del local a consultar</param>
        /// <param name="pSoloActivos">Id del local a consultar</param>
        /// <returns> Objeto tipo List<E_PRODUCTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PRODUCTO>> ConsultarProducto(int? pIdProducto = null, int? pIdLocal = null, byte? pSoloActivos = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var productos = await (from s in context.CTL_PRODUCTO
                                     join l in context.CTL_LOCAL on s.id_local equals l.id_local                                     
                                     where
                                      (pIdProducto == null || (pIdProducto != null && s.id_producto == pIdProducto))
                                      && (pIdLocal == null || (pIdLocal != null && s.id_local == pIdLocal))
                                      && (pSoloActivos == null || (pSoloActivos != null && s.estatus == pSoloActivos))
                                      //&& (pSoloActivos == null || (pSoloActivos != null && l.estatus == pSoloActivos))
                                     select s).ToListAsync<CTL_PRODUCTO>();

                    return await procesaPorductos(productos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para insertar productos favoritos
        /// <param name="pIdPersona">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <param name="pIdProducto">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE AgregarProductoFavorito(int pIdPersona, int pIdProducto)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    /*ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));

                    var productos = context.SP_PRODUCTO(pIdProducto, string.Empty, string.Empty, 0,
                                         string.Empty, 0, 0, pIdPersona, 1, "APF",
                                         RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    //var resultado = context.SaveChanges();

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };*/
                    E_MENSAJE vMensaje = new E_MENSAJE();

                    var existeRegistro = (from s in context.R_PERSONA_PRODUCTO_FAVORITO
                                          where
                                          s.id_persona == pIdPersona && s.id_producto == pIdProducto
                                          select s).ToList<R_PERSONA_PRODUCTO_FAVORITO>().FirstOrDefault();

                    if(existeRegistro == null)
                    {
                        context.R_PERSONA_PRODUCTO_FAVORITO.Add(new R_PERSONA_PRODUCTO_FAVORITO() { id_persona = pIdPersona, id_producto = pIdProducto, fecha_alta = DateTime.Now });
                        var resultado = context.SaveChanges();

                        if (resultado <= 0)
                            vMensaje = new E_MENSAJE { RET_NUMEROERROR = -100, RET_MENSAJEERROR = "No se pudo insertar, intente más tarde", RET_VALORDEVUELTO = "No se pudo insertar, intente más tarde" };
                        else
                            vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Insertado", RET_VALORDEVUELTO = "Insertado" };
                    }
                    else
                    {
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -200, RET_MENSAJEERROR = "Ya existe este producto como favorito.", RET_VALORDEVUELTO = "Ya existe este producto como favorito." };
                    }

                    
                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para eliminar productos favoritos
        /// <param name="pIdPersona">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <param name="pIdProducto">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE EliminarProductoFavorito(int pIdPersona, int pIdProducto)
        {
            try
            {
                
                using (context = new ViajesEntities())
                {
                    /*ObjectParameter RET_NUMEROERROR = new ObjectParameter("RET_NUMEROERROR", typeof(string));
                    ObjectParameter RET_MENSAJEERROR = new ObjectParameter("RET_MENSAJEERROR", typeof(string));
                    ObjectParameter RET_VALORDEVUELTO = new ObjectParameter("RET_VALORDEVUELTO", typeof(string));

                    var productos = context.SP_PRODUCTO(pIdProducto, null, null, null,
                                         null, null, null, pIdPersona, null, "EPF",
                                         RET_NUMEROERROR, RET_MENSAJEERROR, RET_VALORDEVUELTO);

                    var resultado = context.SaveChanges();

                    E_MENSAJE vMensaje = new E_MENSAJE { RET_NUMEROERROR = int.Parse(RET_NUMEROERROR.Value.ToString()), RET_MENSAJEERROR = RET_MENSAJEERROR.Value.ToString(), RET_VALORDEVUELTO = RET_VALORDEVUELTO.Value.ToString() };
                    return vMensaje;*/

                    E_MENSAJE vMensaje = new E_MENSAJE();
                    var existeRegistro = (from s in context.R_PERSONA_PRODUCTO_FAVORITO
                                          where
                                          s.id_persona == pIdPersona && s.id_producto == pIdProducto
                                          select s).ToList<R_PERSONA_PRODUCTO_FAVORITO>().FirstOrDefault();

                    if (existeRegistro == null)
                    {
                        vMensaje = new E_MENSAJE { RET_NUMEROERROR = -200, RET_MENSAJEERROR = "No existe este producto como favorito.", RET_VALORDEVUELTO = "Ya existe este producto como favorito." };

                    }
                    else
                    {
                        context.R_PERSONA_PRODUCTO_FAVORITO.Remove
                         (existeRegistro);
                        var resultado = context.SaveChanges();

                        if (resultado <= 0)
                            vMensaje = new E_MENSAJE { RET_NUMEROERROR = -100, RET_MENSAJEERROR = "No se pudo eliminar, intente más tarde", RET_VALORDEVUELTO = "No se pudo eliminar, intente más tarde" };
                        else
                            vMensaje = new E_MENSAJE { RET_NUMEROERROR = 0, RET_MENSAJEERROR = "Eliminado", RET_VALORDEVUELTO = "Eliminado" };


                    }

                    return vMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar productos favoritos
        /// <param name="pIdPersona">Id del la persona</param>
        /// <param name="pIdProducto">Id del producto</param>
        /// <returns> Objeto tipo List<E_PRODUCTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PRODUCTO>> ConsultarFavoritos(int pIdPersona, int? pIdProducto = null)
        {
            try
            {
                using (context = new ViajesEntities())
                {
                    var productos = await (from pf in context.R_PERSONA_PRODUCTO_FAVORITO
                                     join p in context.CTL_PRODUCTO on pf.id_producto equals p.id_producto
                                     join l in context.CTL_LOCAL on p.id_local equals l.id_local
                                     where p.estatus == 1 && pf.id_persona == pIdPersona && l.estatus == 1
                                     && (pIdProducto == null || (pIdProducto != null && pf.id_producto == pIdProducto))
                                     select p).ToListAsync<CTL_PRODUCTO>();

                    return await procesaPorductos(productos);



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<E_PRODUCTO>> procesaPorductos(List<CTL_PRODUCTO> pProductos)
        {
            var listaProductos = new List<E_PRODUCTO>();

            foreach (var producto in pProductos)
            {
                var local = await new LocalOperaciones().ConsultarLocales(pIdLocal: producto.id_local);
                listaProductos.Add(new E_PRODUCTO
                {
                    IdProducto = producto.id_producto,
                    Nombre = producto.nombre,
                    Descripcion = producto.descripcion,
                    Precio = producto.precio,
                    Fotografia = producto.fotografia,
                    Estatus = producto.estatus,
                    Local = local.FirstOrDefault(),
                    IdPersonaAlta = producto.id_persona_alta,
                    IdPersonaModifica = producto.id_persona_mod ?? 0
                });
            }
            return listaProductos;
        }
    }
}
