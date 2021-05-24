using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Producto;
using Viajes.EL.Extras;
using WSViajes.Comunes;
using WSViajes.Exceptions;
using WSViajes.Models;
using WSViajes.Models.Request;
using WSViajes.Models.Response;
using Serilog;

namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Producto")]
    public class ProductoController : ApiController
    {
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreaProducto([FromBody] InsertaActualizaProducto pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaProducto ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Producto == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Producto.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                //else if (String.IsNullOrEmpty(pRequest.Producto.Fotografia))
                //    respuesta.Mensaje = "El elemento <<Fotografia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Producto.IdLocal.ToString()) || pRequest.Producto.IdLocal == 0)
                    respuesta.Mensaje = "El elemento <<IdLocal>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Producto.Precio.ToString()) || pRequest.Producto.Precio == 0)
                    respuesta.Mensaje = "El elemento <<Precio>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Producto.IdPersonaAlta.ToString()) || pRequest.Producto.IdPersonaAlta <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaAlta>> no puede estar vacío ni igual o menor a cero.";
                else
                {

                    if(!String.IsNullOrEmpty(pRequest.Producto.Fotografia))
                    {
                        var extension = Funciones.getExtensionImagenBasae64(pRequest.Producto.Fotografia);
                        var rutaImagen = Funciones.uploadImagen(pRequest.Producto.Fotografia, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"),
                                                                System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"),
                                                                string.Empty, extension, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img/Productos"), "Assets/Img/Productos/");



                        if (!string.IsNullOrEmpty(rutaImagen))
                            pRequest.Producto.Fotografia = $"{Url.Content("~/")}{rutaImagen}";
                        else
                        {
                            respuesta.CodigoError = -3000;
                            respuesta.Mensaje = "No se pudo crear la imagen, intente más tarde";
                        }

                    }


                    var respuestaOperacion = new ProductoNegocio().Agregar(pRequest.Producto);

                    if (respuestaOperacion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaOperacion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaOperacion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaOperacion.RET_MENSAJEERROR;
                    }
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPut]
        [Route("")]
        public async Task<HttpResponseMessage> Actualiza([FromBody] InsertaActualizaProducto pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - ActualizaProducto ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Producto == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Producto.Nombre))
                    respuesta.Mensaje = "El elemento <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Producto.IdLocal.ToString()) || pRequest.Producto.IdLocal == 0)
                    respuesta.Mensaje = "El elemento <<IdLocal>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Producto.Precio.ToString()) || pRequest.Producto.Precio == 0)
                    respuesta.Mensaje = "El elemento <<Precio>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Producto.IdPersonaModifica.ToString()) || pRequest.Producto.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    if (!string.IsNullOrEmpty(pRequest.Producto.Fotografia))
                    {
                        var extension = Funciones.getExtensionImagenBasae64(pRequest.Producto.Fotografia);
                        //Contemplar eliminar foto anterior
                        var rutaImagen = Funciones.uploadImagen(pRequest.Producto.Fotografia, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"),
                                                                System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"), string.Empty,
                                                                extension, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img/Productos"), "Assets/Img/Productos/");

                        if (!string.IsNullOrEmpty(rutaImagen))
                        {
                            pRequest.Producto.Fotografia = $"{Url.Content("~/")}{rutaImagen}";
                            //Eliminar foto actual
                            var producto = await new ProductoNegocio().ConsultarPorId(pRequest.Producto.IdProducto);
                            Funciones.deleteExistingFile(producto.Fotografia);
                        }
                        else
                        {
                            respuesta.CodigoError = -3000;
                            respuesta.Mensaje = "No se pudo obtener la imagen, intente más tarde";
                        }
                    }

                    var respuestaDireccion = new ProductoNegocio().Editar(pRequest.Producto);
                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
                    }
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> ConsultaProductos(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_PRODUCTO> { };
            var strMetodo = "WSViajes - ConsultaProductos ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().ConsultarTodo(pSoloActivos: soloActivos);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen productos con los parámetros solicitados";
                }


            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("{idProducto}")]
        public async Task<HttpResponseMessage> ConsultaProducto(int idProducto)
        {
            var respuesta = new ConsultaPorIdResponse<E_PRODUCTO_DETALLE> { };
            var strMetodo = "WSViajes - ConsultaProductoPorID ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().ConsultarPorId(idProducto);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen productos con los parámetros solicitados";
                }


            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("local/{idLocal}")]
        public async Task<HttpResponseMessage> ConsultaProductoByLocal(int idLocal, byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_PRODUCTO> { };
            var strMetodo = "WSViajes - ConsultaProductoByLocal ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().ConsultarTodo(soloActivos, idLocal);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen productos con los parámetros solicitados";
                }


            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPost]
        [Route("favoritos")]
        public HttpResponseMessage CreaProductoFavorito([FromBody] InsertaActualizaProductoFavorito pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaProductoFavorito ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdProducto.ToString()) || pRequest.IdProducto <= 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual o menor a cero.";
                else
                {

                    var respuestaDireccion = new ProductoNegocio().AgregarProductoFavorito(pRequest.IdPersona, pRequest.IdProducto);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
                    }

                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpDelete]
        [Route("favoritos")]
        public HttpResponseMessage EliminaProductoFavorito([FromBody] InsertaActualizaProductoFavorito pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - EliminaProductoFavorito ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdProducto.ToString()) || pRequest.IdProducto <= 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual o menor a cero.";
                else
                {

                    var respuestaDireccion = new ProductoNegocio().EliminarProductoFavorito(pRequest.IdPersona, pRequest.IdProducto);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
                    }

                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("favoritos/{idPersona}")]
        public async Task<HttpResponseMessage> GetProductosFavorito(int idPersona)
        {
            var respuesta = new ConsultarTodoResponse<E_PRODUCTO> { };
            var strMetodo = "WSViajes - GetProductosFavorito ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().ConsultarFavoritos(idPersona);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen productos con los parámetros solicitados";
                }


            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("favoritos/{idPersona}/producto/{idProducto}")]
        public async Task<HttpResponseMessage> GetProductoFavoritoByIds(int idPersona, int idProducto)
        {
            var respuesta = new ConsultaPorIdResponse<E_PRODUCTO> { };
            var strMetodo = "WSViajes - GetProductoFavoritoByIds ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().ConsultarFavoritosByIds(idPersona, idProducto);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen productos con los parámetros solicitados";
                }


            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPut]
        [Route("CambiaEstatus")]
        public HttpResponseMessage ActualizaEstatus([FromBody] InsertaActualizaProducto pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = " WSViajes - ActualizaEstatus ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Producto == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Producto.IdProducto.ToString()) || pRequest.Producto.IdProducto <= 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.Producto.IdPersonaModifica.ToString()) || pRequest.Producto.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.Producto.Estatus.ToString()) || (pRequest.Producto.Estatus < 0 || pRequest.Producto.Estatus > 1))
                    respuesta.Mensaje = "El elemento <<Estatus>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var respuestaDireccion = new ProductoNegocio().CambiaEstatus(pRequest.Producto);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
                    }
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("{idProducto}/extras")]
        public async Task<HttpResponseMessage> GetExtrasByProducto(int idProducto, byte? soloActivos = 1)
        {
            var respuesta = new ConsultarTodoResponse<E_EXTRAS_PRODUCTO> { };
            var strMetodo = "WSViajes - GetExtrasByProducto ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().ConsultarExtrasByProducto(idProducto, soloActivos);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen extras con los parámetros solicitados";
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }


        [HttpGet]
        [Route("Busqueda")]
        public async Task<HttpResponseMessage> BusquedaProductoByTermino(string termino = "")
        {
            var respuesta = new ConsultarTodoResponse<E_PRODUCTO_BUSQUEDA> { };
            var strMetodo = "WSViajes - BusquedaProductoByTermino ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ProductoNegocio().BusquedaProductoByTermino(termino.Trim());

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen productos con los parámetros solicitados";
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPost]
        [Route("Extras")]
        public HttpResponseMessage AgregaExtraProducto([FromBody] InsertaActualizaExtraRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - AgregaExtraProducto ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdProducto.ToString()) || pRequest.IdProducto == 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Precio.ToString()) || pRequest.Precio == 0)
                    respuesta.Mensaje = "El elemento <<Precio>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";
                else
                {

                    var respuestaDireccion = new ProductoNegocio().AgregarExtrasProducto(pRequest.Nombre, pRequest.IdProducto, pRequest.Precio, pRequest.IdPersona);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
                    }
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPut]
        [Route("Extras")]
        public HttpResponseMessage ActualizaExtraProducto([FromBody] InsertaActualizaExtraRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - ActualizaExtraProducto ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdProducto.ToString()) || pRequest.IdProducto == 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Precio.ToString()) || pRequest.Precio == 0)
                    respuesta.Mensaje = "El elemento <<Precio>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Estatus.ToString()))
                    respuesta.Mensaje = "El elemento <<Estatus>> no puede estar vacío ";
                else if (String.IsNullOrEmpty(pRequest.IdExtra.ToString()))
                    respuesta.Mensaje = "El elemento <<IdExtra>> no puede estar vacío.";
                else
                {

                    var respuestaDireccion = new ProductoNegocio().ActualizarExtrasProducto(pRequest.IdExtra, pRequest.Nombre, pRequest.IdProducto, pRequest.Precio, pRequest.Estatus, pRequest.IdPersona);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
                    }
                }
            }
            catch (ServiceException Ex)
            {
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

    }
}
