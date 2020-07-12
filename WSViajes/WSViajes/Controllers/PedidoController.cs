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
using Viajes.BL.Direccion;
using Viajes.BL.Local;
using Viajes.BL.Persona;
using System.Collections.Generic;
using Viajes.EL.Enum;
using System.Configuration;
using Newtonsoft.Json;
using System.Linq;

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
        public async Task<HttpResponseMessage> Crea([FromBody] InsertaActualizaPedidoRequest pRequest)
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
                else if (String.IsNullOrEmpty(pRequest.SessionId) && pRequest.Pedido.IdMetodoPago == 2)
                    respuesta.Mensaje = "El elemento <<SessionId>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.TokenTarjeta) && pRequest.Pedido.IdMetodoPago == 2)
                    respuesta.Mensaje = "El elemento <<TokenTarjeta>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.CostoEnvio.ToString()))
                    respuesta.Mensaje = "El elemento <<CostoEnvio>> no puede estar vacío.";
                /*else if (String.IsNullOrEmpty(pRequest.Pedido.Observaciones.ToString()) || pRequest.IdPersonaCrea <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaCrea>> no puede estar vacío ni igual o menor a cero.";*/
                else
                {
                    pRequest.Pedido.TipoPedido = 1;
                    /**
                    * IdMetodoPago 1 = efectivo, 2 = tarjeta, 3 = paypal
                    * */
                    if (pRequest.Pedido.IdMetodoPago == 2)
                    {
                        string CustomerId = await new PersonaNegocio().ConsultarClienteIdOpenPay(pRequest.Pedido.PersonaPide.IdPersona);

                        if (string.IsNullOrEmpty(CustomerId))
                        {
                            respuesta.CodigoError = 10001;
                            respuesta.Mensaje = $"El usuario indicado no cuenta con una relación a openpay interna.";
                        }
                        else
                        {
                            var pago = new OpenPayFunctions().CreateCharge(CustomerId, pRequest.TokenTarjeta, "", getTotalPedido(pRequest.Pedido.Detalle), pRequest.SessionId);

                            if(pago.ErrorMessage == null)
                            {
                                pRequest.Pedido.ReferenciaPago = pago.Id;
                                var respuestaDireccion = new PedidoNegocio().Agregar(pRequest.Pedido);

                                if (respuestaDireccion.RET_NUMEROERROR == 0)
                                {
                                    respuesta.Exito = true;
                                    respuesta.Mensaje = "Pedido registrado con éxito";//respuestaDireccion.RET_VALORDEVUELTO;
                                }
                                else
                                {
                                    respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                                    respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                                }
                            }
                            else
                            {
                                respuesta.CodigoError = -2000;
                                respuesta.Mensaje = pago.ErrorMessage;
                            }
                            
                        }
                    }
                    else
                    {
                        var respuestaDireccion = new PedidoNegocio().Agregar(pRequest.Pedido);

                        if (respuestaDireccion.RET_NUMEROERROR == 0)
                        {
                            respuesta.Exito = true;
                            respuesta.Mensaje = "Pedido registrado con éxito";//respuestaDireccion.RET_VALORDEVUELTO;
                        }
                        else
                        {
                            respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                            respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                        }
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
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPedido.ToString()))
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío.";
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
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPedido.ToString()) || pRequest.Pedido.IdPedido.ToString().Length !=36)
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío ni igual a cero y debe tener 36 caracteres.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.Estatus.IdEstatus.ToString()) || pRequest.Pedido.Estatus.IdEstatus == 0)
                    respuesta.Mensaje = "El elemento <<IdEstatus>> no puede estar vacío ni igual a cero.";
                else
                {
                    var pedidoNegocio = new PedidoNegocio();
                    var respuestaDireccion = pedidoNegocio.ActualizaEstatus(pRequest.Pedido);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        var mensaje = Funciones.GetMensajeCambioEstatus(pRequest.Pedido.Estatus.IdEstatus, "");
                        if(!string.IsNullOrEmpty(mensaje))
                        {
                            var enviarMensaje = new NotificacionesController();
                            var mailer = new Mailer();
                            var pedido = await pedidoNegocio.ConsultarPorId(pRequest.Pedido.IdPedido);
                            var token = await enviarMensaje.GetTokenUser(pedido.PersonaPide.IdPersona);
                            var persona = await new PersonaNegocio().ConsultarPorId(pedido.PersonaPide.IdPersona);
                            if (!string.IsNullOrEmpty(token))
                                await enviarMensaje.SendMessage(token, "FastRun", mensaje, (byte)NotificacionFirebase.Cliente);

                            mailer.Send(persona.Acceso.Email, "Novedades en tu pedido", mensaje, persona.Nombre);

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
        public async Task<HttpResponseMessage> Consulta(string pListaIdEstatus = null)
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaPedidos ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarTodo();

                if (respuesta.Data.Count > 0)
                {
                    if(!String.IsNullOrEmpty(pListaIdEstatus))
                    {
                        var estatus = pListaIdEstatus.Split(',');

                        respuesta.Data = respuesta.Data.Where(p => estatus.Contains(p.Estatus.IdEstatus.ToString())).ToList();
                    }
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
        public async Task<HttpResponseMessage> ConsultaPedidoPorId(Guid idPedido)
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
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPedido.ToString()) || pRequest.Pedido.IdPedido.ToString().Length != 36)
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío y debe tener 36 caracteres.";
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

        [HttpGet]
        [Route("Historial/Local/{pIdLocal}")]
        public async Task<HttpResponseMessage> ConsultaHistorialLocal(int pIdLocal, string pIdEstatus = null)
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO> { };
            var strMetodo = "WSViajes - ConsultaHistorialLocal ";
            string sid = Guid.NewGuid().ToString();
            string[] listaEstatus = null;
            try
            {
                if (!String.IsNullOrEmpty(pIdEstatus) && pIdEstatus.Contains(","))
                    listaEstatus = pIdEstatus.Split(',');
                else if(!String.IsNullOrEmpty(pIdEstatus))
                    listaEstatus = new string[]{ pIdEstatus };

                respuesta.Data = await new PedidoNegocio().ConsultarHistorialLocal(pIdLocal, listaEstatus);

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
        [Route("{idPedido}/Preguntas/{tipoPreguntas}")]
        public async Task<HttpResponseMessage> ConsultaPreguntasPendientesPedido(Guid idPedido, byte tipoPreguntas)
        {
            var respuesta = new ConsultarTodoResponse<E_PREGUNTA_SERVICIO> { };
            var strMetodo = "WSViajes - ConsultaPreguntasPendientesPedido ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarPreguntasPendientesByIdPedido(idPedido, tipoPreguntas);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen preguntas con los parámetros solicitados";
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
        [Route("Preguntas/{tipoPreguntas}")]
        public async Task<HttpResponseMessage> ConsultaPreguntas(byte tipoPreguntas)
        {
            var respuesta = new ConsultarTodoResponse<E_PREGUNTA_SERVICIO> { };
            var strMetodo = "WSViajes - ConsultaPreguntas ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarPreguntaByTipo(tipoPreguntas);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen preguntas con los parámetros solicitados";
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

        [HttpPost]
        [Route("RespuestaServicio")]
        public HttpResponseMessage CreaRespuestaPreguntaServicio([FromBody] InsertaRespuestaServicio pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaRespuestaPreguntaServicio ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (string.IsNullOrEmpty(pRequest.IdPregunta.ToString()) || pRequest.IdPregunta.ToString().Length != 36)
                    respuesta.Mensaje = "El elemento <<IdPregunta>> no puede estar vacío y debe tener 36 caracteres.";
                else if (string.IsNullOrEmpty(pRequest.IdPedido.ToString()) || pRequest.IdPedido.ToString().Length != 36)
                    respuesta.Mensaje = "El elemento <<IdPedido>> no puede estar vacío y debe tener 36 caracteres.";
                else if (string.IsNullOrEmpty(pRequest.Respuesta.ToString()))
                    respuesta.Mensaje = "El elemento <<Respuesta>> no puede estar vacío.";
                else
                {
                    
                    var respuestaDireccion = new PedidoNegocio().AgregarRespuestaPreguntaServicio(pRequest.IdPedido, pRequest.IdPregunta, pRequest.Respuesta);

                    if (respuestaDireccion.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = "Respuesta registrada con éxito";
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

        [HttpPost]
        [Route("CostoEntrega")]
        public async Task<HttpResponseMessage> CalculaCostoEntrega([FromBody] CalculaCostoEntrega pRequest)
        {
            var respuesta = new ConsultaPorIdResponse<decimal> { };
            var strMetodo = "WSViajes - CalculaCostoEntrega ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (string.IsNullOrEmpty(pRequest.IdDireccionEntrega.ToString()) || pRequest.IdDireccionEntrega == 0)
                    respuesta.Mensaje = "El elemento <<IdDireccionEntrega>> no puede estar vacío y debe ser mayor a cero";
                else if (pRequest.DireccionesRecoge == null || pRequest.DireccionesRecoge.Count <= 0)
                    respuesta.Mensaje = "El elemento <<DireccionesRecoge>> debe ser una lista y tener por lo menos un elemento.";
                else
                {

                    var DireccionEntrega = await new DireccionNegocio().ConsultarPorId(pRequest.IdDireccionEntrega);
                    var DireccionRecoge = await new LocalNegocio().ConsultarPorId(pRequest.DireccionesRecoge[0]);
                    var uriApiMaps = ConfigurationManager.AppSettings["URI_API_MAPS_DISTANCE"];
                    int DistanciaMetros = 0;
                    E_TAFIRA_ENVIO costoEnvio = null;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(uriApiMaps);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.GetAsync(string.Format("?origin={0}&destination={1}&key={2}", $"{DireccionEntrega.Latitud.ToString().Replace(",", ".")},{DireccionEntrega.Longitud.ToString().Replace(",", ".")}", $"{DireccionRecoge.Latitud.ToString().Replace(",", ".")},{DireccionRecoge.Longitud.ToString().Replace(",", ".")}", ConfigurationManager.AppSettings["SERVER_KEY_MAPS"]));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonDeserializado = JsonConvert.DeserializeObject<E_RESPOSE_DISTANCE>(response.Content.ReadAsStringAsync().Result);
                            if (jsonDeserializado.status.ToLower().Equals("ok"))
                            {
                        
                                string metrosApi = jsonDeserializado.routes[0]["legs"][0]["distance"]["value"];
                                DistanciaMetros = Int32.Parse(metrosApi);
                                costoEnvio = await new PedidoNegocio().ConsultaCotoEnvioByDistancia(DistanciaMetros);
                            }
                        }
                    }

                    respuesta.Exito = true;
                    respuesta.Mensaje = "Datos cargados con éxito.";
                    respuesta.Data = costoEnvio.CostoEnvio;

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

        [HttpPost]
        [Route("Personalizado")]
        public async Task<HttpResponseMessage> CreaPersonalizado([FromBody] InsertaActualizaPedidoPersonalizadoRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaPedidoPersonalizado ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.IdDireccionEntrega.ToString()) || pRequest.IdDireccionEntrega == 0)
                    respuesta.Mensaje = "El elemento <<IdDireccionEntrega>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersonaPide.ToString()) || pRequest.IdPersonaPide == 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaPide>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdMetodoPago.ToString()) || pRequest.IdMetodoPago <= 0)
                    respuesta.Mensaje = "El elemento <<IdMetodoPago>> no puede estar vacío ni igual o menor a cero.";
                else if (String.IsNullOrEmpty(pRequest.SessionId) && pRequest.IdMetodoPago == 2)
                    respuesta.Mensaje = "El elemento <<SessionId>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.TokenTarjeta) && pRequest.IdMetodoPago == 2)
                    respuesta.Mensaje = "El elemento <<TokenTarjeta>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.CostoEnvio.ToString()))
                    respuesta.Mensaje = "El elemento <<CostoEnvio>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.ToString()))
                    respuesta.Mensaje = "El elemento <<Pedido>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Nombrelocal.ToString()))
                    respuesta.Mensaje = "El elemento <<Nombrelocal>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Direccion.ToString()))
                    respuesta.Mensaje = "El elemento <<Direccion>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Referencias.ToString()))
                    respuesta.Mensaje = "El elemento <<Referencias>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Latitud.ToString()) || pRequest.Latitud == 0)
                    respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Longitud.ToString()) || pRequest.Longitud == 0)
                    respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.LimiteInferion.ToString()) || pRequest.LimiteInferion == 0)
                    respuesta.Mensaje = "El elemento <<LimiteInferion>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.LimiteSuperior.ToString()) || pRequest.LimiteSuperior == 0)
                    respuesta.Mensaje = "El elemento <<LimiteSuperior>> no puede estar vacío ni igual a cero.";
                else
                {
                    /**
                    * IdMetodoPago 1 = efectivo, 2 = tarjeta, 3 = paypal
                    * */
                    if (pRequest.IdMetodoPago == 2)
                    {
                        string CustomerId = await new PersonaNegocio().ConsultarClienteIdOpenPay(pRequest.IdPersonaPide);

                        if (string.IsNullOrEmpty(CustomerId))
                        {
                            respuesta.CodigoError = 10001;
                            respuesta.Mensaje = $"El usuario indicado no cuenta con una relación a openpay interna.";
                        }
                        else
                        {
                            var pago = new OpenPayFunctions().CreateCharge(CustomerId, pRequest.TokenTarjeta, "", pRequest.CostoEnvio, pRequest.SessionId);

                            if (pago.ErrorMessage == null)
                            {
                                //pRequest.ReferenciaPago = pago.Id;
                                E_PEDIDO_PERSONALIZADO pedidoPersonalizado = new E_PEDIDO_PERSONALIZADO
                                {
                                    ReferenciaPago = pago.Id,
                                    TipoPedido = 2,
                                    Observaciones = pRequest.Observaciones,
                                    CostoEnvio = pRequest.CostoEnvio, 
                                    IdMetodoPago = pRequest.IdMetodoPago
                                };

                                pedidoPersonalizado.PersonaPide.IdPersona = pRequest.IdPersonaPide;
                                pedidoPersonalizado.DireccionEntrega.IdDireccion = pRequest.IdDireccionEntrega;

                                pedidoPersonalizado.Detalle = new E_PEDIDO_PERSONALIZADO_DETALLE
                                {
                                    Pedido = pRequest.Pedido,
                                    Direccion = pRequest.Direccion,
                                    Latitud = pRequest.Latitud,
                                    Longitud = pRequest.Longitud,
                                    LimiteInferion = pRequest.LimiteInferion,
                                    LimiteSuperior = pRequest.LimiteSuperior,
                                    NombreLocal = pRequest.Nombrelocal, 
                                    Referencias = pRequest.Referencias
                                };

                                var respuestaDireccion = new PedidoNegocio().AgregarPersonalizado(pedidoPersonalizado);

                                if (respuestaDireccion.RET_NUMEROERROR == 0)
                                {
                                    respuesta.Exito = true;
                                    respuesta.Mensaje = "Pedido registrado con éxito";//respuestaDireccion.RET_VALORDEVUELTO;
                                }
                                else
                                {
                                    respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                                    respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                                }
                            }
                            else
                            {
                                respuesta.CodigoError = -2000;
                                respuesta.Mensaje = pago.ErrorMessage;
                            }

                        }
                    }
                    else
                    {
                        E_PEDIDO_PERSONALIZADO pedidoPersonalizado = new E_PEDIDO_PERSONALIZADO
                        {
                            TipoPedido = 2,
                            Observaciones = pRequest.Observaciones,
                            CostoEnvio = pRequest.CostoEnvio,
                            IdMetodoPago = pRequest.IdMetodoPago
                        };

                        pedidoPersonalizado.PersonaPide.IdPersona = pRequest.IdPersonaPide;
                        pedidoPersonalizado.DireccionEntrega.IdDireccion = pRequest.IdDireccionEntrega;

                        pedidoPersonalizado.Detalle = new E_PEDIDO_PERSONALIZADO_DETALLE
                        {
                            Pedido = pRequest.Pedido,
                            Direccion = pRequest.Direccion,
                            Latitud = pRequest.Latitud,
                            Longitud = pRequest.Longitud,
                            LimiteInferion = pRequest.LimiteInferion,
                            LimiteSuperior = pRequest.LimiteSuperior,
                            NombreLocal = pRequest.Nombrelocal,
                            Referencias = pRequest.Referencias
                        };

                        var respuestaDireccion = new PedidoNegocio().AgregarPersonalizado(pedidoPersonalizado);

                        if (respuestaDireccion.RET_NUMEROERROR == 0)
                        {
                            respuesta.Exito = true;
                            respuesta.Mensaje = "Pedido registrado con éxito";//respuestaDireccion.RET_VALORDEVUELTO;
                        }
                        else
                        {
                            respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
                            respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
                        }
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
        [Route("Personalizado")]
        public async Task<HttpResponseMessage> ConsultaPedidoPersonalizado()
        {
            var respuesta = new ConsultarTodoResponse<E_PEDIDO_PERSONALIZADO> { };
            var strMetodo = "WSViajes - ConsultaPedidoPersonalizado ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarPersonalizadosTodo();

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
        [Route("Personalizado/{idPedido}")]
        public async Task<HttpResponseMessage> ConsultaPedidoPersonalizadoPorId(Guid idPedido)
        {
            var respuesta = new ConsultaPorIdResponse<E_PEDIDO_PERSONALIZADO> { };
            var strMetodo = "WSViajes - ConsultaPedidoPersonalizadoPorId ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new PedidoNegocio().ConsultarPersonalizadosPorId(idPedido);

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


        private decimal getTotalPedido(List<E_DETALLE_PEDIDO> productos)
        {
            decimal total = 0;
              
            foreach(var producto in productos)
            {
                total += (producto.Cantidad * producto.Precio);
                foreach(var extra in producto.Extras)
                {
                    total += extra.Precio;
                }
            }

            return total;
        }
    }
}
