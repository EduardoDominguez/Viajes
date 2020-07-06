using System;
using System.Collections.Generic;
using Viajes.EL.Extras;
using Viajes.EL.Interfaz;
using Viajes.DAL.Producto;
using System.Linq;
using System.Threading.Tasks;

namespace Viajes.BL.Producto
{
    public class ProductoNegocio
    {
        /// <summary>
        /// Método para crear productos
        /// <param name="pProducto">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Agregar(E_PRODUCTO pProducto)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.CreaProducto(pProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar productos por id
        /// <param name="pIdProducto">Id del producto a consultar</param>
        /// <returns> Objeto tipo E_PRODUCTO con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_PRODUCTO_DETALLE> ConsultarPorId(int pIdProducto)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                var pResultado = await pDatos.ConsultarProductoId(pIdProducto);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar todos productos
        /// <param name="pSoloActivos">Indica si debe consultar solo activos o no, por default consulta todo</param>
        /// <param name="pIdLocal">Id del local a consultar</param>
        /// <returns> Objeto tipo List<E_PRODUCTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PRODUCTO>> ConsultarTodo(byte? pSoloActivos = null, int? pIdLocal = null)
        {

            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return await pDatos.ConsultarProducto(null, pIdLocal, pSoloActivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar productos
        /// <param name="pProducto">Objeto de tipo E_PRODUCTO con datos a insertar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE Editar(E_PRODUCTO pProducto)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.ActualizaProducto(pProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para agregar productos favoritos
        /// <param name="pIdPersona">Id de la persona a agregar</param>
        /// <param name="pIdProducto">Id del producto a relacionar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE AgregarProductoFavorito(int pIdPersona, int pIdProducto)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.AgregarProductoFavorito(pIdPersona, pIdProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para eliminar productos favoritos
        /// <param name="pIdPersona">Id de la persona a agregar</param>
        /// <param name="pIdProducto">Id del producto a relacionar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE EliminarProductoFavorito(int pIdPersona, int pIdProducto)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.EliminarProductoFavorito(pIdPersona, pIdProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para agregar extras a productos
        /// <param name="pNombre">Nombre del extra</param>
        /// <param name="pIdProducto">Id del producto a relacionar</param>
        /// <param name="pPrecio">Precio del extra</param>
        /// <param name="pIdPersonaAlta">Persona que registra el extra</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE AgregarExtrasProducto(string pNombre, int pIdProducto, decimal pPrecio, int pIdPersonaAlta)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.AgregarExtraProducto(pNombre, pIdProducto, pPrecio, pIdPersonaAlta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar extras a productos
        /// <param name="pIdExtra">Identificador del extra</param>
        /// <param name="pNombre">Nombre del extra</param>
        /// <param name="pIdProducto">Id del producto a relacionar</param>
        /// <param name="pPrecio">Precio del extra</param>
        /// <param name="pEstatus">Estatus del extra</param>
        /// <param name="pIdPersonaMod">Persona que registra el extra</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE ActualizarExtrasProducto(Guid pIdExtra, string pNombre, int pIdProducto, decimal pPrecio, byte pEstatus, int pIdPersonaMod)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.ActualizarExtrasProducto(pIdExtra, pNombre, pIdProducto, pPrecio, pEstatus, pIdPersonaMod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar todos productos favoritos de una persona
        /// <param name="pIdPersona">Id persona a consultar productos</param>        
        /// <returns> Objeto tipo List<E_PRODUCTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_PRODUCTO>> ConsultarFavoritos(int pIdPersona)
        {

            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return await pDatos.ConsultarFavoritos(pIdPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar un favorito por Ids
        /// <param name="pIdPersona">Id persona a consultar productos</param>        
        /// <param name="pIdProducto">Id producto a consultar</param>        
        /// <returns> Objeto tipo List<E_PRODUCTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<E_PRODUCTO> ConsultarFavoritosByIds(int pIdPersona, int pIdProducto)
        {

            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                var pResultado = await pDatos.ConsultarFavoritos(pIdPersona, pIdProducto);
                return pResultado.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para consultar  extras de productos        
        /// <param name="pIdProducto">Id producto a consultar</param>        
        /// <returns> Objeto tipo List<E_EXTRAS_PRODUCTO> con los datos solicitados </returns>  
        /// </summary>
        public async Task<List<E_EXTRAS_PRODUCTO>> ConsultarExtrasByProducto(int pIdProducto)
        {

            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                var pResultado = await pDatos.ConsultaExtrasByProducto(pIdProducto);
                return pResultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        /// <summary>
        /// Método para actualizar estatus de productos
        /// <param name="pProducto">Objeto de tipo E_PRODUCTO con datos a actualizar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public E_MENSAJE CambiaEstatus(E_PRODUCTO pProducto)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return pDatos.CambiaEstatus(pProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método para busqueda de productos
        /// <param name="pTermino">Palabra clave al buscar</param>
        /// <returns> Objeto tipo E_MENSAJE con los datos del movimiento </returns>  
        /// </summary>
        public async Task<List<E_PRODUCTO_BUSQUEDA>> BusquedaProductoByTermino(string pTermino)
        {
            try
            {
                ProductoOperaciones pDatos = new ProductoOperaciones();
                return await pDatos.BusquedaProductoByTermino(pTermino);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

