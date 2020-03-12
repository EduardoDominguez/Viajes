using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Pedido;
using Viajes.EL.Extras;
using WSViajes.Exceptions;
using WSViajes.Models;
using WSViajes.Models.Request;
using WSViajes.Models.Response;
using WSViajes.Comunes;


namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Pedido")]
    public class PedidoController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Crea([FromBody] InsertaActualizaPedidoRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaPedido ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Pedido == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Pedido.DireccionEntrega == null || (String.IsNullOrEmpty(pRequest.Pedido.DireccionEntrega.IdDireccion.ToString()) || pRequest.Pedido.DireccionEntrega.IdDireccion == 0))
                    respuesta.Mensaje = "El elemento <<IdDireccion>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.PersonaPide.IdPersona.ToString()) || pRequest.Pedido.PersonaPide.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaPide>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdMetodoPago.ToString()) || pRequest.Pedido.IdMetodoPago <= 0)
                    respuesta.Mensaje = "El elemento <<IdMetodoPago>> no puede estar vacío ni igual o menor a cero.";
                else if (pRequest.Pedido.Detalle == null || pRequest.Pedido.Detalle.Count <= 0)
                    respuesta.Mensaje = "El elemento <<Detalle>> debe ser un arreglo y tener por lo menos un elemento.";
                /*else if (String.IsNullOrEmpty(pRequest.Pedido.Observaciones.ToString()) || pRequest.IdPersonaCrea <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaCrea>> no puede estar vacío ni igual o menor a cero.";*/
                else
                {

                    var respuestaDireccion = new PedidoNegocio().Agregar(pRequest.Pedido);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }


        [HttpPut]
        [Route("Cancelar")]
        public HttpResponseMessage Cancelar([FromBody] InsertaActualizaPedidoRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CancelarPedido ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Pedido == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPedido.ToString()) || pRequest.Pedido.IdPedido == 0)
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío ni igual a cero.";
                else
                {

                    var respuestaDireccion = new PedidoNegocio().Cancelar(pRequest.Pedido);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }


        [HttpPut]
        [Route("Estatus")]
        public async Task<HttpResponseMessage> ActualizaEstatus([FromBody] InsertaActualizaPedidoRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - ActualizaEstatus ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Pedido == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPedido.ToString()) || pRequest.Pedido.IdPedido == 0)
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.Estatus.IdEstatus.ToString()) || pRequest.Pedido.Estatus.IdEstatus == 0)
                    respuesta.Mensaje = "El elemento <<IdEstatus>> no puede estar vacío ni igual a cero.";
                else
                {
                    var negocio = new PedidoNegocio();
                    var respuestaDireccion = negocio.ActualizaEstatus(pRequest.Pedido);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        var mensaje = Funciones.GetMensajeCambioEstatus(pRequest.Pedido.Estatus.IdEstatus, "");
                        if(!string.IsNullOrEmpty(mensaje))
                        {
                            var enviarMensaje = new NotificacionesController();
                            var pedido = await negocio.ConsultarPorId(pRequest.Pedido.IdPedido);
                            var token = await enviarMensaje.GetTokenUser(pedido.PersonaPide.IdPersona);
                            if (!string.IsNullOrEmpty(token))
                                await enviarMensaje.SendMessage(token, "FastRun", mensaje);
                        }
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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> Consulta()
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaPedidos ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarTodo();

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
        [Route("{idPedido}")]
        public async Task<HttpResponseMessage> ConsultaPedidoPorId(int idPedido)
        {
            var respuesta = new ConsultaPorIdResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultapedidoPorID ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarPorId(idPedido);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
        [Route("Persona/{idPersona}")]
        public async Task<HttpResponseMessage> ConsultaPedidoPorIdPersona(int idPersona)
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaPedidoPorIdPersona ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarTodo(null, idPersona);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
        [Route("Actual/{idPersona}")]
        public async Task<HttpResponseMessage> ConsultaPedidoActual(int idPersona)
        {
            var respuesta = new ConsultaPorIdResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaPedidoActual ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarViajeActual(pIdPersonaPide: idPersona);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
        [Route("Actual/Conductor/{idPersona}")]
        public async Task<HttpResponseMessage> ConsultaPedidoActualRepartidor(int idPersona)
        {
            var respuesta = new ConsultaPorIdResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaPedidoActualRepartidor ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarViajeActual(pIdPersonaEntrega: idPersona);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
        [Route("PorAsignar")]
        public async Task<HttpResponseMessage> ConsultaPorAsignar()
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaPorAsignar ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultaPorAsignar();

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
        [Route("Asignar")]
        public HttpResponseMessage Asignar([FromBody] InsertaActualizaPedidoRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - AsignarPedido ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Pedido == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPedido.ToString()) || pRequest.Pedido.IdPedido == 0)
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.PersonaEntrega.IdPersona.ToString()) || pRequest.Pedido.PersonaEntrega.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<PersonaEntrega.IdPersona>> no puede estar vacío ni igual a cero.";
                else
                {

                    var respuestaDireccion = new PedidoNegocio().Asignar(pRequest.Pedido);

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
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("Historial/Conductor/{idPersona}")]
        public async Task<HttpResponseMessage> ConsultaHistorialConductor(int idPersona)
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaHistorialConductor ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarHistorialConductor(idPersona);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen pedidos con los parámetros solicitados";
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
