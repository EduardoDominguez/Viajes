using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Banner;
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
    [RoutePrefix("api/Banners")]
    public class BannerController : ApiController
    {
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> ConsultaBanners(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_BANNER> { };
            var strMetodo = "WSViajes - ConsultaBanners ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new BannerNegocio().ConsultarTodo(soloActivos);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen banners.";
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
        [Route("{idBanner}")]
        public async Task<HttpResponseMessage> ConsultaBannerById(Guid idBanner)
        {
            var respuesta = new ConsultaPorIdResponse<E_BANNER> { };
            var strMetodo = "WSViajes - ConsultaBannerById ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new BannerNegocio().ConsultarPorId(idBanner);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen locales con los parámetros solicitados";
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
        [Route("")]
        public HttpResponseMessage Creabanner([FromBody] InsertaActualizaBannerRequest pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = "WSViajes - Creabanner ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Fotografia))
                    respuesta.Mensaje = "El elemento <<Fotografia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.IdProducto.ToString()) || pRequest.IdProducto == 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaMovimiento.ToString()) || pRequest.IdPersonaMovimiento <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaMovimiento>> no puede estar vacío ni igual o menor a cero.";
                else
                {


                    var extension = Funciones.getExtensionImagenBasae64(pRequest.Fotografia);
                    var rutaImagen = Funciones.uploadImagen(pRequest.Fotografia, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"),
                                                            System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"),
                                                            string.Empty, extension, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img/Banners"), "Assets/Img/Banners/");

                    if (!string.IsNullOrEmpty(rutaImagen))
                    {

                        pRequest.Fotografia = $"{Url.Content("~/")}{rutaImagen}";


                        var banner = new E_BANNER()
                        {
                            Fotografia = pRequest.Fotografia,
                            IdProducto = pRequest.IdProducto,
                            Nombre = pRequest.Nombre,
                            IdPersonaAlta = pRequest.IdPersonaMovimiento
                        };

                        var respuestaOperacion = new BannerNegocio().Agregar(banner);

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
                    else
                    {
                        respuesta.CodigoError = -3000;
                        respuesta.Mensaje = "No se pudo crear la imagen, intente más tarde";
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
        public async Task<HttpResponseMessage> ActualizaBanner([FromBody] InsertaActualizaBannerRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = " WSViajes - ActualizaBanner ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdBanner.ToString()))
                    respuesta.Mensaje = "El elemento <<IdBanner>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.IdProducto.ToString()) || pRequest.IdProducto == 0)
                    respuesta.Mensaje = "El elemento <<IdProducto>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaMovimiento.ToString()) || pRequest.IdPersonaMovimiento <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaMovimiento>> no puede estar vacío ni igual o menor a cero.";
                else
                {

                    if (!string.IsNullOrEmpty(pRequest.Fotografia))
                    {
                        var extension = Funciones.getExtensionImagenBasae64(pRequest.Fotografia);
                        //Contemplar eliminar foto anterior
                        var rutaImagen = Funciones.uploadImagen(pRequest.Fotografia, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"),
                                                                System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"), string.Empty,
                                                                extension, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img/Banners"), "Assets/Img/Banners/");

                        if (!string.IsNullOrEmpty(rutaImagen))
                        {
                            pRequest.Fotografia = $"{Url.Content("~/")}{rutaImagen}";
                            //Eliminar foto actual
                            var banner = await new BannerNegocio().ConsultarPorId(pRequest.IdBanner);
                            Funciones.deleteExistingFile(banner.Fotografia);
                        }
                    }
                    var respuestaDireccion = new BannerNegocio().Editar(new E_BANNER() { IdBanner = pRequest.IdBanner, Fotografia = pRequest.Fotografia, IdPersonaModifica = pRequest.IdPersonaMovimiento, IdProducto = pRequest.IdProducto});
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


        [HttpPatch]
        [Route("CambiaEstatus")]
        public HttpResponseMessage ActualizaEstatus([FromBody] ActualizaEstatusRegistroRequest<Guid> pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = " WSViajes - ActualizaEstatus ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdRegistro.ToString()))
                    respuesta.Mensaje = "El elemento <<IdRegistro>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaModifica.ToString()) || pRequest.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdEstatus.ToString()) || (pRequest.IdEstatus < 0 || pRequest.IdEstatus > 1))
                    respuesta.Mensaje = "El elemento <<IdEstatus>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var respuestaDireccion = new BannerNegocio().CambiaEstatus(new E_BANNER() { IdBanner = pRequest
                    .IdRegistro,
                    Estatus = pRequest.IdEstatus,
                    IdPersonaModifica = pRequest.IdPersonaModifica});

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
