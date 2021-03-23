using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Direccion;
using Viajes.EL.Extras;
using WSViajes.Exceptions;
using WSViajes.Models.Request;
using WSViajes.Models.Response;
using Serilog;

namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Direccion")]
    public class DireccionesController : ApiController
    {
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreaDireccion([FromBody] InsertaActualizaDireccionRequest pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = "WSViajes - CreaDireccion ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Calle))
                    respuesta.Mensaje = "El elemento <<Calle>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Colonia))
                    respuesta.Mensaje = "El elemento <<Colonia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Descripcion))
                    respuesta.Mensaje = "El elemento <<Descripcion>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.NoExt))
                    respuesta.Mensaje = "El elemento <<NoExt>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Latitud.ToString()) || pRequest.Latitud == 0)
                    respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Longitud.ToString()) || pRequest.Longitud == 0)
                    respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaCrea.ToString()) || pRequest.IdPersonaCrea <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaCrea>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var objDireccion = new E_DIRECCION
                    {
                        Nombre = pRequest.Nombre,
                        Calle = pRequest.Calle,
                        Colonia = pRequest.Colonia,
                        NoInt = pRequest.NoInt ?? string.Empty,
                        NoExt = pRequest.NoExt,
                        Descripcion = pRequest.Descripcion,
                        Latitud = pRequest.Latitud,
                        Longitud = pRequest.Longitud,
                        IdPersona = pRequest.IdPersona,
                        IdPersonaAlta = pRequest.IdPersonaCrea
                    };

                    var respuestaDireccion = new DireccionNegocio().Agregar(objDireccion);

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
        [Route("")]
        public HttpResponseMessage ActualizaDireccion([FromBody] InsertaActualizaDireccionRequest pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = " WSViajes - ActualizaDireccion ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Calle))
                    respuesta.Mensaje = "El elemento <<Calle>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Colonia))
                    respuesta.Mensaje = "El elemento <<Colonia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.NoExt))
                    respuesta.Mensaje = "El elemento <<NoExt>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Latitud.ToString()) || pRequest.Latitud == 0)
                    respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Longitud.ToString()) || pRequest.Longitud == 0)
                    respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdDireccion.ToString()) || pRequest.IdDireccion <= 0)
                    respuesta.Mensaje = "El elemento <<IdDireccion>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaModifica.ToString()) || pRequest.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var objDireccion = new E_DIRECCION
                    {
                        Nombre = pRequest.Nombre,
                        Calle = pRequest.Calle,
                        Colonia = pRequest.Colonia,
                        NoInt = pRequest.NoInt ?? string.Empty,
                        NoExt = pRequest.NoExt,
                        Descripcion = pRequest.Descripcion ?? string.Empty,
                        Latitud = pRequest.Latitud,
                        Longitud = pRequest.Longitud,
                        IdPersonaAlta = pRequest.IdPersonaModifica,
                        IdDireccion = pRequest.IdDireccion
                    };

                    var respuestaDireccion = new DireccionNegocio().Editar(objDireccion);

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
        public async Task<HttpResponseMessage> ConsultaDireccion(int? idPersona = null, byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_DIRECCION> { };
            var strMetodo = "WSViajes - ConsultaDireccion ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new DireccionNegocio().ConsultarTodo(soloActivos, idPersona);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen direcciones con los parámetros solicitados";
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
        [Route("{idDireccion}")]
        public async Task<HttpResponseMessage> ConsultaDireccionById(int idDireccion)
        {
            var respuesta = new ConsultaPorIdResponse<E_DIRECCION> { };
            var strMetodo = "WSViajes - ConsultaDireccionById ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new DireccionNegocio().ConsultarPorId(idDireccion);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen direcciones con los parámetros solicitados";
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
        [Route("Estatus")]
        public HttpResponseMessage ActualizaEstatusDireccion([FromBody] InsertaActualizaDireccionRequest pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = " WSViajes - ActualizaEstatusDireccion ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Estatus.ToString()) || (pRequest.Estatus != 1 && pRequest.Estatus != 0))
                    respuesta.Mensaje = "El elemento <<Activo>> no puede estar vacío y debe ser un valor entre 1 y 0.";
                else if (String.IsNullOrEmpty(pRequest.IdDireccion.ToString()) || pRequest.IdDireccion <= 0)
                    respuesta.Mensaje = "El elemento <<IdDireccion>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaModifica.ToString()) || pRequest.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var objDireccion = new E_DIRECCION
                    {
                        Estatus = pRequest.Estatus,
                        IdPersonaAlta = pRequest.IdPersonaModifica,
                        IdDireccion = pRequest.IdDireccion
                    };

                    var respuestaDireccion = new DireccionNegocio().ActualizaEstatusDireccion(objDireccion);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
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
        [Route("Predeterminada")]
        public HttpResponseMessage ActualizaDireccionPredeterminada([FromBody] InsertaActualizaDireccionRequest pRequest)
        {
            var respuesta = new InsertaDireccionResponse { };
            var strMetodo = " WSViajes - ActualizaDireccionPredeterminada ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                /*else if (String.IsNullOrEmpty(pRequest.Predeterminada.ToString()) || (pRequest.Predeterminada != 1 && pRequest.Predeterminada != 0))
                    respuesta.Mensaje = "El elemento <<Predeterminada>> no puede estar vacío y debe ser un valor entre 1 y 0.";*/
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdDireccion.ToString()) || pRequest.IdDireccion <= 0)
                    respuesta.Mensaje = "El elemento <<IdDireccion>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaModifica.ToString()) || pRequest.IdPersonaModifica <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var objDireccion = new E_DIRECCION
                    {
                        //PREDETERMINADA = pRequest.Predeterminada,
                        IdPersona = pRequest.IdPersona,
                        IdPersonaAlta = pRequest.IdPersonaModifica,
                        IdDireccion = pRequest.IdDireccion
                    };

                    var respuestaDireccion = new DireccionNegocio().ActualizaDireccionPredeterminada(objDireccion);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                    }
                    else
                    {
                        respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                        respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
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
        [Route("Predeterminada/{idPersona}")]
        public async Task<HttpResponseMessage> ConsultaPredeterminada(int idPersona)
        {
            var respuesta = new ConsultaPorIdResponse<E_DIRECCION> { };
            var strMetodo = "WSViajes - ConsultaPredeterminada ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new DireccionNegocio().ConsultarPredeterminada(idPersona);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen direcciones con los parámetros solicitados";
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
