using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Persona;
using Viajes.EL.Extras;
using WSViajes.Exceptions;
using WSViajes.Models;
using WSViajes.Models.Request;
using WSViajes.Models.Response;
using Openpay.Entities;
using WSViajes.Comunes;



namespace WSViajes.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Persona")]
    public class PersonaController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet]
        [Route("Conductor/Coordenadas/Pedido/{IdPedido}")]
        public async Task<HttpResponseMessage> ConsultaCoordenadasByIdPedido(long IdPedido)
        {
            var respuesta = new ConsultarTodoResponse<E_COORDENADAS_CONDUCTOR> { };
            var strMetodo = "WSViajes - ConsultaCoordenadasByIdPedido ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ConductorNegocio().ConsultarCoordenadas(pIdPedido: IdPedido);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen coordenadas con los parámetros solicitados";
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
        [Route("Conductor/Coordenadas")]
        public HttpResponseMessage InsertaCoordenadas(InsertaCoordenadasConductor pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - InsertaCoordenadas ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (pRequest.Coordenadas == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Coordenadas.Latitud.ToString()) || pRequest.Coordenadas.Latitud == 0)
                    respuesta.Mensaje = "El elemento <<Latitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Coordenadas.Longitud.ToString()) || pRequest.Coordenadas.Longitud == 0)
                    respuesta.Mensaje = "El elemento <<Longitud>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Coordenadas.IdPersona.ToString()) || pRequest.Coordenadas.IdPersona <= 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual o menor a cero.";
                else
                {
                    var resultado = new ConductorNegocio().AgregarCoordenadas(pRequest.Coordenadas);

                    if (resultado.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                    }

                    respuesta.Mensaje = resultado.RET_VALORDEVUELTO;
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
        [Route("TokenFirebase")]
        public HttpResponseMessage ActualizaToken([FromBody] ActualizaTokenRequest pRequestToken)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSDeudas - ActualizaToken ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequestToken == null)
                    respuesta.Mensaje = "No se recibió ninguna deuda a registrar.";
                else if (string.IsNullOrEmpty(pRequestToken.TokenFirebase.Trim()))
                    respuesta.Mensaje = "El elemento  <<TokenFirebase>> no puede estar vacío.";
                else if (pRequestToken.IdPersona <= 0)
                    respuesta.Mensaje = "El elemento <<IdUsuario>> debe especificar un usuario.";
                else
                {
                    var resultado = new AccesoNegocio().ActualizaToken(pRequestToken.IdPersona, pRequestToken.TokenFirebase);

                    if (resultado.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                    }

                    respuesta.Mensaje = resultado.RET_VALORDEVUELTO;
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
        [AllowAnonymous]
        [Route("OpenPay/Cliente")]
        public HttpResponseMessage CreaClienteOpenPay(CreaClienteOpenPayRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaClienteOpenPay ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.Nombre.ToString()))
                    respuesta.Mensaje = "El elemento <<Nombre>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.Correo.ToString()))
                    respuesta.Mensaje = "El elemento <<Correo>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";

                else
                {
                    var creaClienteOpen = new OpenPayFunctions().CreateCustomer(pRequest.Nombre, pRequest.Apellidos, pRequest.Correo);
                    var resultado = new PersonaNegocio().AgregarClienteOpenPay(pRequest.IdPersona, creaClienteOpen.Id);

                    if (resultado.RET_NUMEROERROR == 0)
                    {
                        respuesta.Exito = true;
                    }

                    respuesta.Mensaje = resultado.RET_VALORDEVUELTO;
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
        [AllowAnonymous]
        [Route("OpenPay/Cliente/{CustomerId}/Tarjeta/Listar")]
        //public async Task<HttpResponseMessage> ConsultaTarjetasPersona(string CustomerId)
        public HttpResponseMessage ConsultaTarjetasPersona(string CustomerId)
        {
            var respuesta = new ConsultarTodoResponse<Card> { };
            var strMetodo = "WSViajes - ConsultaTarjetasPersona ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data =  new OpenPayFunctions().GetListCardCustomers(CustomerId);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen tarjetas con los parámetros solicitados";
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

        [HttpDelete]
        [AllowAnonymous]
        [Route("OpenPay/Cliente/{CustomerId}/Tarjeta/{CardId}")]
        //public async Task<HttpResponseMessage> ConsultaTarjetasPersona(string CustomerId)
        public HttpResponseMessage EliminarTarjetaCliente(string CustomerId, string CardId)
        {
            var respuesta = new ConsultarTodoResponse<Card> { };
            var strMetodo = "WSViajes - EliminarTarjetaCliente ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = new OpenPayFunctions().GetListCardCustomers(CustomerId);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen tarjetas con los parámetros solicitados";
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
