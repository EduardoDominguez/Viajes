using log4net;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Login;
using Viajes.BL.Persona;
using Viajes.BL.Local;
using Viajes.EL.Extras;
using WSViajes.Comunes;
using WSViajes.Exceptions;
using WSViajes.Models.Request;
using WSViajes.Models.Response;

namespace WSViajes.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> Login([FromBody] LoginRequest pLoginRequest)
        {
            var respuesta = new LoginResponse { };
            var strMetodo = "WSViajes - Login ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pLoginRequest == null)
                    respuesta.Mensaje = "No se recibió usuario.";
                else if (String.IsNullOrEmpty(pLoginRequest.Email))
                    respuesta.Mensaje = "El elemento  <<Email>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pLoginRequest.Password))
                    respuesta.Mensaje = "Debe especificar el <<Password>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pLoginRequest.TipoUsuario.ToString()))
                    respuesta.Mensaje = "Debe especificar el <<TipoUsuario>> no puede estar vacío.";
                else
                {
                    var objAcceso = new E_ACCESO_PERSONA { Email = pLoginRequest.Email, Password = pLoginRequest.Password, TipoUsuario = pLoginRequest.TipoUsuario };
                    var pNegocio = new LoginNegocio();
                    var respuestaLogin = pNegocio.Login(objAcceso);

                    if (respuestaLogin.CORRECTO)
                    {
                        respuesta.Exito = true;
                        respuesta.Persona = respuestaLogin.PERSONA;
                        respuesta.Token = CreateToken(respuesta.Persona);
                        if (pLoginRequest.TipoUsuario == 4)
                        {
                            var local = await new LocalNegocio().ConsultarByIdPersonaResponsable(respuestaLogin.PERSONA.IdPersona);
                            respuesta.IdLocal = local.IdLocal;
                        }
                            
                    }
                    else
                    {
                        respuesta.Mensaje = respuestaLogin.MENSAJE;
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
        [Route("Registrar")]
        public async Task<HttpResponseMessage> Registrar([FromBody] InsertaPersonaRequest pInsertaPersonaRequest)
        {
            var respuesta = new InsertaPersonaResponse { };
            var strMetodo = "WSViajes - Registrar ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pInsertaPersonaRequest == null)
                    respuesta.Mensaje = "No se recibió usuario.";
                else if (String.IsNullOrEmpty(pInsertaPersonaRequest.Nombre))
                    respuesta.Mensaje = "El elemento  <<Nombre>> no puede estar vacío.";
                /*else if (String.IsNullOrEmpty(pInsertaPersonaRequest.ApePaterno))
                    respuesta.Mensaje = "El elemento  <<ApePaterno>> no puede estar vacío.";*/
                else if (String.IsNullOrEmpty(pInsertaPersonaRequest.Telefono))
                    respuesta.Mensaje = "El elemento  <<Telefono>> no puede estar vacío.";
                /*else if (String.IsNullOrEmpty(pInsertaPersonaRequest.Fotografia))
                    respuesta.Mensaje = "El elemento  <<Fotografia>> no puede estar vacío.";*/
                else if (String.IsNullOrEmpty(pInsertaPersonaRequest.Email))
                    respuesta.Mensaje = "El elemento  <<Email>> no puede estar vacío.";
                else if (String.IsNullOrEmpty(pInsertaPersonaRequest.Password))
                    respuesta.Mensaje = "Debe especificar el <<Password>> no puede estar vacío.";
                else if (pInsertaPersonaRequest.TipoUsuario <= 0)
                    respuesta.Mensaje = "Debe especificar el <<TipoUsuario>> no puede estar vacío y debe ser mayor a 0.";
                else
                {
                    var objAcceso = new E_ACCESO_PERSONA { Email = pInsertaPersonaRequest.Email, Password = pInsertaPersonaRequest.Password, TipoUsuario = pInsertaPersonaRequest.TipoUsuario, TokenFirebase = pInsertaPersonaRequest.TokenFirebase };
                    var objPersona = new E_PERSONA { Nombre = pInsertaPersonaRequest.Nombre, Telefono = pInsertaPersonaRequest.Telefono, Fotografia = pInsertaPersonaRequest.Fotografia };
                    var pNegocio = new LoginNegocio();
                    var respuestaCrearPersona = pNegocio.CreaPersona(objPersona, objAcceso);

                    if (respuestaCrearPersona.RET_NUMEROERROR >= 0)
                    {

                        var creaClienteOpen = new OpenPayFunctions().CreateCustomer(pInsertaPersonaRequest.Nombre, "", pInsertaPersonaRequest.Email);
                        var personaRecienCreada = await new AccesoNegocio().ConsultaPorCorreo(pInsertaPersonaRequest.Email.Trim());
                        new PersonaNegocio().AgregarClienteOpenPay(personaRecienCreada.IdPersona, creaClienteOpen.Id);
                        new Mailer().Send(pInsertaPersonaRequest.Email, "Bienvenido a nuestra plataforma FASTRUN", "Te damos la bienvenida a nuestra plataforma de pedidos y compras a través de tu aplicación. <br/> <b>¡¡Ha empezar a ordenar!!</b><br/><br/><p>Saludos del equipo FastRun.</p>", pInsertaPersonaRequest.Nombre);
                    }

                    respuesta.Exito = respuestaCrearPersona.RET_NUMEROERROR >= 0;
                    respuesta.Mensaje = respuestaCrearPersona.RET_VALORDEVUELTO;

                }
            }
            catch (ServiceException Ex)
            {
                respuesta.Exito = false;
                respuesta.CodigoError = Ex.Codigo;
                respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                respuesta.Exito = false;
                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        private string CreateToken(E_PERSONA pPersona)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["JWT_EXPIRE_DAYS"]));
            //DateTime expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["JWT_EXPIRE_DAYS"]));

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, pPersona.Nombre)
            });

            string sec = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"], audience: ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"],
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
