using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Local;
using Viajes.EL.Extras;
using WSViajes.Comunes;
using WSViajes.Exceptions;
using WSViajes.Models;
using WSViajes.Models.Request;
using WSViajes.Models.Response;


namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Local")]
    public class LocalController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("")]
        public HttpResponseMessage CrearLocal([FromBody] InsertaActualizaLocalRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CrearLocal ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Local == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Local.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Calle))
                    respuesta.Mensaje = "El elemento <<Calle>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Colonia))
                    respuesta.Mensaje = "El elemento <<Colonia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.NoExt))
                    respuesta.Mensaje = "El elemento <<NoExt>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Referencias))
                    respuesta.Mensaje = "El elemento <<Referencias>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Fotografia))
                    respuesta.Mensaje = "El elemento <<Fotografia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Costo.IdCosto.ToString()) || pRequest.Local.Costo.IdCosto == 0)
                    respuesta.Mensaje = "El elemento <<IdCosto>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.TipoLocal.IdTipoLocal.ToString()) || pRequest.Local.TipoLocal.IdTipoLocal == 0)
                    respuesta.Mensaje = "El elemento <<IdTipoLocal>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.Latitud.ToString()) || pRequest.Local.Latitud == 0)
                    respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.Longitud.ToString()) || pRequest.Local.Longitud == 0)
                    respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.IdPersonaAlta.ToString()) || pRequest.Local.IdPersonaAlta <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaAlta>> no puede estar vacío ni igual o menor a cero.";
                else
                {

                    var extension = Funciones.getExtensionImagenBasae64(pRequest.Local.Fotografia);
                    var rutaImagen = Funciones.uploadImagen(pRequest.Local.Fotografia, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"),
                                                            System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"),
                                                            string.Empty, extension, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img/Locales"), "Assets/Img/Locales/");

                    if (!string.IsNullOrEmpty(rutaImagen))
                    {

                        pRequest.Local.Fotografia = $"{Url.Content("~/")}{rutaImagen}";

                        var respuestaOperacion = new LocalNegocio().Agregar(pRequest.Local);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPut]
        [Route("")]
        public async Task<HttpResponseMessage> ActualizaLocal([FromBody] InsertaActualizaLocalRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = " WSViajes - ActualizaLocal ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Local == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Local.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Calle))
                    respuesta.Mensaje = "El elemento <<Calle>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Colonia))
                    respuesta.Mensaje = "El elemento <<Colonia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.NoExt))
                    respuesta.Mensaje = "El elemento <<NoExt>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Referencias))
                    respuesta.Mensaje = "El elemento <<Referencias>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Local.Costo.IdCosto.ToString()) || pRequest.Local.Costo.IdCosto == 0)
                    respuesta.Mensaje = "El elemento <<IdCosto>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.TipoLocal.IdTipoLocal.ToString()) || pRequest.Local.TipoLocal.IdTipoLocal == 0)
                    respuesta.Mensaje = "El elemento <<IdTipoLocal>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.Latitud.ToString()) || pRequest.Local.Latitud == 0)
                    respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.Longitud.ToString()) || pRequest.Local.Longitud == 0)
                    respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.IdPersonaModifica.ToString()) || pRequest.Local.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else
                {

                    if (!string.IsNullOrEmpty(pRequest.Local.Fotografia))
                    {
                        var extension = Funciones.getExtensionImagenBasae64(pRequest.Local.Fotografia);
                        //Contemplar eliminar foto anterior
                        var rutaImagen = Funciones.uploadImagen(pRequest.Local.Fotografia, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"),
                                                                System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"), string.Empty,
                                                                extension, System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img/Locales"), "Assets/Img/Locales/");

                        if (!string.IsNullOrEmpty(rutaImagen))
                        {
                            pRequest.Local.Fotografia = $"{Url.Content("~/")}{rutaImagen}";
                            //Eliminar foto actual
                            var local = await new LocalNegocio().ConsultarPorId(pRequest.Local.IdLocal);
                            Funciones.deleteExistingFile(local.Fotografia);
                        }
                    }
                    var respuestaDireccion = new LocalNegocio().Editar(pRequest.Local);
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> ConsultaLocales(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_LOCAL> { };
            var strMetodo = "WSViajes - ConsultaLocales ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new LocalNegocio().ConsultarTodo(soloActivos);

                if (respuesta.Data.Count > 0)
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("{idLocal}")]
        public async Task<HttpResponseMessage> ConsultaLocal(int idLocal)
        {
            var respuesta = new ConsultaPorIdResponse<E_LOCAL> { };
            var strMetodo = "WSViajes - ConsultaLocalPorID ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new LocalNegocio().ConsultarPorId(idLocal);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPut]
        [Route("CambiaEstatus")]
        public HttpResponseMessage ActualizaEstatus([FromBody] InsertaActualizaLocalRequest pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = " WSViajes - ActualizaEstatus ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Local == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Local.IdLocal.ToString()) || pRequest.Local.IdLocal <= 0)
                    respuesta.Mensaje = "El elemento <<IdLocal>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.IdPersonaModifica.ToString()) || pRequest.Local.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.Local.Estatus.ToString()) || (pRequest.Local.Estatus < 0 || pRequest.Local.Estatus > 1))
                    respuesta.Mensaje = "El elemento <<Estatus>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var respuestaDireccion = new LocalNegocio().CambiaEstatus(pRequest.Local);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("Costo")]
        public async Task<HttpResponseMessage> ConsultaCostoLocal(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_COSTO> { };
            var strMetodo = "WSViajes - ConsultaCostoLocal ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new CostoLocalNegocio().ConsultarTodo(soloActivos);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen costos de locales con los parámetros solicitados";
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("Costo/{idCosto}")]
        public async Task<HttpResponseMessage> ConsultaCostoLocalById(int idCosto)
        {
            var respuesta = new ConsultaPorIdResponse<E_COSTO> { };
            var strMetodo = "WSViajes - ConsultaCostoLocalById ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new CostoLocalNegocio().ConsultarPorId(idCosto);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen costos de locales con los parámetros solicitados";
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("TipoLocal")]
        public async Task<HttpResponseMessage> ConsultaTipoLocal(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_TIPO_LOCAL> { };
            var strMetodo = "WSViajes - ConsultaTipoLocal ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new TipoLocalNegocio().ConsultarTodo(soloActivos);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen costos de locales con los parámetros solicitados";
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("TipoLocal/{idTipoLocal}")]
        public async Task<HttpResponseMessage> ConsultaTipoLocalById(int idTipoLocal)
        {
            var respuesta = new ConsultaPorIdResponse<E_TIPO_LOCAL> { };
            var strMetodo = "WSViajes - ConsultaTipoLocalById ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new TipoLocalNegocio().ConsultarPorId(idTipoLocal);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen tipos de local con los parámetros solicitados";
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("TipoLocal/{idTipoLocal}/Locales")]
        public async Task<HttpResponseMessage> ConsultaLocalByTipoLocal(int idTipoLocal)
        {
            var respuesta = new ConsultarTodoResponse<E_LOCAL> { };
            var strMetodo = "WSViajes - ConsultaLocalByTipoLocal ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new LocalNegocio().ConsultarByTipoLocal(idTipoLocal);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }
    }
}
