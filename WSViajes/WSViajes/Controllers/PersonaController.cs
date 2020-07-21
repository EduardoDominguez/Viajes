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
using System.Configuration;

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
        public async Task<HttpResponseMessage> ConsultaCoordenadasByIdPedido(Guid IdPedido)
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


        [HttpGet]
        [Route("Conductor/Consulta")]
        public async Task<HttpResponseMessage> ConsultaConductores(byte? soloActivos = null)
        {
            var respuesta = new ConsultarTodoResponse<E_PERSONA> { };
            var strMetodo = "WSViajes - ConsultaConductores ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new ConductorNegocio().ConsultarTodo(SoloActivos: soloActivos);

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen conductores con los parámetros solicitados";
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
        [Route("Conductor/Agrega")]
        public HttpResponseMessage AgregarConductor(InsertaActualizaConductorRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - AgregarConductor ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió usuario.";
                else if (String.IsNullOrEmpty(pRequest.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                //else if (String.IsNullOrEmpty(pRequest.ApePaterno))
                //    respuesta.Mensaje = "El elemento  <<ApePaterno>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Telefono))
                    respuesta.Mensaje = "El elemento  <<Telefono>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Fotografia))
                    respuesta.Mensaje = "El elemento  <<Fotografia>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pRequest.Email))
                    respuesta.Mensaje = "El elemento  <<Email>> no puede estar vacío.";
                else
                {
                    var objAcceso = new E_ACCESO_PERSONA { Email = pRequest.Email, Password = "", TipoUsuario = pRequest.TipoUsuario };
                    var objPersona = new E_PERSONA { Sexo = pRequest.Sexo, Nombre = pRequest.Nombre, Telefono = pRequest.Telefono, Fotografia = pRequest.Fotografia };
                    var objDatosConductor = new E_CONDUCTOR { Tipo = pRequest.Tipo, Colonia = pRequest.Colonia, Calle = pRequest.Calle, NoExt = pRequest.NoExt, NoInt = pRequest.NoInt,  NoLicencia = pRequest.NoLicencia, NoPlacas = pRequest.NoPlacas};
                    var respuestaCreaConductor = new ConductorNegocio().Agregar(objPersona, objAcceso, objDatosConductor);
                    if (respuestaCreaConductor.RET_NUMEROERROR >= 0)
                    {
                        //var creaClienteOpen = new OpenPayFunctions().CreateCustomer(pInsertaPersonaRequest.Nombre, "", pInsertaPersonaRequest.Email);
                        //var personaRecienCreada = await new AccesoNegocio().ConsultaPorCorreo(pInsertaPersonaRequest.Email.Trim());
                        //new PersonaNegocio().AgregarClienteOpenPay(personaRecienCreada.IdPersona, creaClienteOpen.Id);
                        new Mailer().Send(pRequest.Email, string.Format("Bienvenido a nuestra plataforma FASTRUN", "Te damos la bienvenida a nuestra plataforma. Ingresa a la siguiente liga para crear tu contraseña y empezar a utilizar tu cuenta: {0}{1} <br/> <b>¡¡Ha empezar a entregar!!</b><br/><br/><p>Saludos del equipo FastRun.</p>", ConfigurationManager.AppSettings["URL_PASSWORD_CONDUCTOR"], respuestaCreaConductor.RET_ID_PERSONA), pRequest.Nombre);
                    }

                    respuesta.Exito = respuestaCreaConductor.RET_NUMEROERROR >= 0;
                    respuesta.Mensaje = respuestaCreaConductor.RET_VALORDEVUELTO;

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
        //[AllowAnonymous]
        [Route("OpenPay/Cliente/Crear")]
        public HttpResponseMessage CreaClienteOpenPay(CreaClienteOpenPayRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CreaClienteOpenPay ";
            string sid = Guid.NewGuid().ToString();
            Customer creaClienteOpen = null;
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

                    if(new PersonaNegocio().ConsultarClienteIdOpenPay(pRequest.IdPersona) != null)
                    {
                        respuesta.Mensaje = "Ya se tiene una relación para esta persona con openpay registrada.";
                    }
                    else
                    {

                        creaClienteOpen = new OpenPayFunctions().CreateCustomer(pRequest.Nombre, pRequest.Apellidos, pRequest.Correo);
                        var resultado = new PersonaNegocio().AgregarClienteOpenPay(pRequest.IdPersona, creaClienteOpen.Id);

                        if (resultado.RET_NUMEROERROR == 0)
                        {
                            respuesta.Exito = true;
                        }

                        respuesta.Mensaje = resultado.RET_VALORDEVUELTO;
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

                if (creaClienteOpen != null)
                    new OpenPayFunctions().DeleteCustomer(creaClienteOpen.Id);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("OpenPay/Tarjeta/Crear")]
        public async Task<HttpResponseMessage> CrearTarjetaOpenPay(CreaTarjetaOpenPayRequest pRequest)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - CrearTarjetaOpenPay ";
            string sid = Guid.NewGuid().ToString();
            try
            {
                if (pRequest == null)
                    respuesta.Mensaje = "No se recibió datos de petición.";
                else if (String.IsNullOrEmpty(pRequest.HolderName.ToString()))
                    respuesta.Mensaje = "El elemento <<HolderName>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.IdPersona.ToString()) || pRequest.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";
                else if (String.IsNullOrEmpty(pRequest.CardNumber) || (pRequest.CardNumber.Length != 16 && pRequest.CardNumber.Length != 19))
                    respuesta.Mensaje = "El elemento <<CardNumber>> no puede estar vacío ni igual a cero y debe tener 16 o 19 caracteres.";
                else if (String.IsNullOrEmpty(pRequest.ExpirationMonth.ToString()) || pRequest.ExpirationMonth.Length > 2)
                    respuesta.Mensaje = "El elemento <<ExpirationMonth>> no puede estar vacío y debe terner 1 o 2 dígitos.";
                else if (String.IsNullOrEmpty(pRequest.ExpirationYear.ToString()))
                    respuesta.Mensaje = "El elemento <<ExpirationYear>> no puede estar vacío.";
                else if (string.IsNullOrEmpty(pRequest.DeviceSessionId))
                    respuesta.Mensaje = "El elemento <<DeviceSessionId>> no puede estar vacío.";
                else
                {
                    string CustomerId = await new PersonaNegocio().ConsultarClienteIdOpenPay(pRequest.IdPersona);

                    if (string.IsNullOrEmpty(CustomerId))
                    {
                        respuesta.CodigoError = 10001;
                        respuesta.Mensaje = $"El usuario indicado no cuenta con una relación a openpay interna.";
                    }
                    else
                    {
                        var resultado = new OpenPayFunctions().CreateCard(pRequest, CustomerId);

                        if (resultado != null)
                        {
                            respuesta.Exito = true;
                            respuesta.Mensaje = "Tarjeta creada con éxito.";
                        }
                        else
                        {
                            respuesta.Mensaje = "No se pudo crear la tarjeta.";
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
        //[AllowAnonymous]
        [Route("{IdPersona}/OpenPay/Tarjeta/Listar")]
        public async Task<HttpResponseMessage> ConsultaTarjetasPersona(int IdPersona)
        //public HttpResponseMessage ConsultaTarjetasPersona(int IdPersona)
        {
            var respuesta = new ConsultarTodoResponse<Card> { };
            var strMetodo = "WSViajes - ConsultaTarjetasPersona ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                string CustomerId = await new PersonaNegocio().ConsultarClienteIdOpenPay(IdPersona);

                if (string.IsNullOrEmpty(CustomerId))
                {
                    respuesta.CodigoError = 10001;
                    respuesta.Mensaje = $"El usuario solicitado no cuenta con una relación a openpay interna.";
                }
                else
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
        //[AllowAnonymous]
        [Route("{IdPersona}/OpenPay/Tarjeta/{CardId}")]
        public async Task<HttpResponseMessage> EliminarTarjetaCliente(int IdPersona, string CardId)
        //public HttpResponseMessage EliminarTarjetaCliente(string CustomerId, string CardId)
        {
            var respuesta = new ConsultarTodoResponse<Card> { };
            var strMetodo = "WSViajes - EliminarTarjetaCliente ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                string CustomerId = await new PersonaNegocio().ConsultarClienteIdOpenPay(IdPersona);

                if (string.IsNullOrEmpty(CustomerId))
                {
                    respuesta.CodigoError = 10001;
                    respuesta.Mensaje = $"El usuario solicitado no cuenta con una relación a openpay interna.";
                }
                else
                {
                    new OpenPayFunctions().DeleteCustomerCard(CustomerId, CardId);
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Trarjeta eliminada con éxito";
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
