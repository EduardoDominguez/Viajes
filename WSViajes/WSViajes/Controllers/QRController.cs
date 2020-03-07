using log4net;
using System;
using System.Web;
//using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.QR;
using Viajes.EL.Extras;
using WSViajes.Exceptions;
using WSViajes.Models.Request;
using WSViajes.Models.Response;
using System.Configuration;


namespace WSViajes.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("QR")]
    public class QRController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [HttpGet]
        [Route("visit-app")]
        public async Task<HttpResponseMessage> ConsultaDireccion()
        {
            //var respuesta = new ConsultarTodoResponse<E_QR_TIENDA> { };
            var strMetodo = "visita-app-qr";
            string sid = Guid.NewGuid().ToString();
            var appAndroidPath = ConfigurationManager.AppSettings["URI_APP_ANDROID"];
            try
            {
                HttpBrowserCapabilities Navegador = HttpContext.Current.Request.Browser;
                string SistemaOperativo = Navegador.Platform;
                E_QR_TIENDA datos = new E_QR_TIENDA();
                datos.Ip = HttpContext.Current.Request.UserHostAddress;
                datos.Dispositivo = SistemaOperativo;
                datos.Aplicacion = "ANDROID";

                await new QRNegocio().Agregar(datos);                
            }
            catch (ServiceException Ex)
            {
                //respuesta.CodigoError = Ex.Codigo;
                //respuesta.Mensaje = Ex.Message;
            }
            catch (Exception Ex)
            {
                string strErrGUI = Guid.NewGuid().ToString();
                string strMensaje = "Error Interno del Servicio [GUID: " + strErrGUI + "].";
                log.Error("[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje, Ex);

                //respuesta.CodigoError = 10001;
                //respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(appAndroidPath);
            return response;

        }
    }
}
