using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.MetodoPago;
using Viajes.EL.Extras;
using WSViajes.Exceptions;
using WSViajes.Models.Response;

namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/MetodoPago")]
    public class MetodoPago : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /*[HttpPost]
        [Route("")]
        public HttpResponseMessage Crea([FromBody] InsertaActualizaPedidoRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaMetodoPago ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Pedido == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdDireccionEntrega.ToString()) || pRequest.Pedido.IdDireccionEntrega == 0)
                    respuesta.Mensaje = "El elemento <<IdDireccionEntrega>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdPersonaPide.ToString()) || pRequest.Pedido.IdPersonaPide == 0)
                    respuesta.Mensaje = "El elemento <<IdPersonaPide>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Pedido.IdMetodoPago.ToString()) || pRequest.Pedido.IdMetodoPago <= 0)
                    respuesta.Mensaje = "El elemento <<IdMetodoPago>> no puede estar vacío ni igual o menor a cero.";
                else if (pRequest.Pedido.Detalle == null || pRequest.Pedido.Detalle.Count <= 0)
                    respuesta.Mensaje = "El elemento <<Detalle>> debe ser un arreglo y tener por lo menos un elemento.";
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


        [HttpPost]
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
        }*/


        //[HttpPut]
        //[Route("")]
        //public HttpResponseMessage ActualizaLocal([FromBody] InsertaActualizaDireccionRequest pRequest)
        //{
        //    //var respuesta = new InsertaDireccionResponse { };
        //    //var strMetodo = " WSViajes - ActualizaDireccion ";
        //    //string sid = Guid.NewGuid().ToString();

        //    //try
        //    //{
        //    //    if (pRequest == null)
        //    //        respuesta.Mensaje = "No se recibió datos de petición.";
        //    //    else if (String.IsNullOrEmpty(pRequest.Nombre))
        //    //        respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
        //    //    else if (String.IsNullOrEmpty(pRequest.Calle))
        //    //        respuesta.Mensaje = "El elemento <<Calle>> no puede estar vacío.";
        //    //    else if (String.IsNullOrEmpty(pRequest.Colonia))
        //    //        respuesta.Mensaje = "El elemento <<Colonia>> no puede estar vacío.";
        //    //    else if (String.IsNullOrEmpty(pRequest.NoExt))
        //    //        respuesta.Mensaje = "El elemento <<NoExt>> no puede estar vacío.";
        //    //    else if (String.IsNullOrEmpty(pRequest.Latitud.ToString()) || pRequest.Latitud == 0)
        //    //        respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
        //    //    else if (String.IsNullOrEmpty(pRequest.Longitud.ToString()) || pRequest.Longitud == 0)
        //    //        respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
        //    //    else if (String.IsNullOrEmpty(pRequest.IdDireccion.ToString()) || pRequest.IdDireccion <= 0)
        //    //        respuesta.Mensaje = "El elemento <<IdDireccion>> no puede estar vacío ni igual o menor a cero.";
        //    //    else if (String.IsNullOrEmpty(pRequest.IdPersonaModifica.ToString()) || pRequest.IdPersonaModifica <= 0)
        //    //        respuesta.Mensaje = "El elemento <<IdPersonaModifica>> no puede estar vacío ni igual o menor a cero.";
        //    //    else
        //    //    {
        //    //        var objDireccion = new E_DIRECCION
        //    //        {
        //    //            NOMBRE = pRequest.Nombre,
        //    //            CALLE = pRequest.Calle,
        //    //            COLONIA = pRequest.Colonia,
        //    //            NO_INT = pRequest.NoInt ?? string.Empty,
        //    //            NO_EXT = pRequest.NoExt,
        //    //            DESCRIPCION = pRequest.Descripcion ?? string.Empty,
        //    //            LATITUD = pRequest.Latitud,
        //    //            LONGITUD = pRequest.Longitud,
        //    //            ID_PERSONA_ALTA = pRequest.IdPersonaModifica,
        //    //            ID_DIRECCION = pRequest.IdDireccion
        //    //        };

        //    //        var respuestaDireccion = new DireccionNegocio().ActualizaDireccion(objDireccion);

        //    //        if (respuestaDireccion.RET_NUMEROERROR == 0)
        //    //        {
        //    //            respuesta.Exito = true;
        //    //            respuesta.Mensaje = respuestaDireccion.RET_VALORDEVUELTO;
        //    //        }
        //    //        else
        //    //        {
        //    //            respuesta.CodigoError = respuestaDireccion.RET_NUMEROERROR;
        //    //            respuesta.Mensaje = respuestaDireccion.RET_MENSAJEERROR;
        //    //        }
        //    //    }
        //    //}
        //    //catch (ServiceException Ex)
        //    //{
        //    //    respuesta.CodigoError = Ex.Codigo;
        //    //    respuesta.Mensaje = Ex.Message;
        //    //}
        //    //catch (Exception Ex)
        //    //{
        //    //    string strErrGUI = Guid.NewGuid().ToString();
        //    //    string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
        //    //    log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

        //    //    respuesta.CodigoError = 10001;
        //    //    respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
        //    //}

        //    //return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        //}


        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> Consulta(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_METODO_PAGO> { };
            var strMetodo = "WSViajes - ConsultaMetodosPago ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new MetodoPagoNegocio().ConsultarTodo(soloActivos);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen metodos de pago con los parámetros solicitados";
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
        [Route("{idMetodoPago}")]
        public async Task<HttpResponseMessage> ConsultaMetodoPagoPorId(int idMetodoPago)
        {
            var respuesta = new ConsultaPorIdResponse<E_METODO_PAGO> { };
            var strMetodo = "WSViajes - ConsultaMetodoPagoPorId ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new MetodoPagoNegocio().ConsultarPorId(idMetodoPago);

                if (respuesta.Data != null)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen metodos de pago con los parámetros solicitados";
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
