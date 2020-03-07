using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Persona;
using WSViajes.Exceptions;
using WSViajes.Models;
using WSViajes.Models.Request;

namespace WSViajes.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Notificacion")]
    public class NotificacionesController : ApiController
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Enviar([FromBody] EnviaNotificacionRequest pEnvivioNotificacion)
        {
            var respuesta = new Respuesta { };
            var strMetodo = "WSViajes - Notificacion/Enviar ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                if (pEnvivioNotificacion == null)
                    respuesta.Mensaje = "No se recibió ningun dato para enviar notificacion.";
                else if (string.IsNullOrEmpty(pEnvivioNotificacion.Titulo.Trim()))
                    respuesta.Mensaje = "El elemento <<Titulo>> no puede estar vacío.";
                else if (string.IsNullOrEmpty(pEnvivioNotificacion.Mensaje.Trim()))
                    respuesta.Mensaje = "El elemento <<Mensaje>>  no puede estar vacío.";
                else if (String.IsNullOrEmpty(pEnvivioNotificacion.IdPersona.ToString()) || pEnvivioNotificacion.IdPersona == 0)
                    respuesta.Mensaje = "El elemento <<IdPersona>> no puede estar vacío ni igual a cero.";
                else
                {
                    var token = await GetTokenUser(pEnvivioNotificacion.IdPersona);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var resultado = await SendMessage(token, pEnvivioNotificacion.Titulo, pEnvivioNotificacion.Mensaje);

                        if (!resultado.Equals("-1"))
                        {
                            respuesta.Exito = true;
                            respuesta.Mensaje = resultado;
                        }
                        else
                            respuesta.Mensaje = "No se pudo enviar la notificación";
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

            return Request.CreateResponse(HttpStatusCode.OK, respuesta);
        }


        public async Task<string> GetTokenUser(int pIdPersona)
        {
            var acceso = await new AccesoNegocio().ConsultarPorId(pIdPersona);

            if (acceso != null && !string.IsNullOrEmpty(acceso.TokenFirebase))
                return acceso.TokenFirebase;
            else
                return "";
            /*{
                respuesta.Mensaje = "No se pudo encontrar el token para el usuario " + pEnvivioNotificacion.IdPersona;
            }*/
        }

        public async Task<string> SendMessage(string pTokenDestinatario, string pTitle, string pBoby)
        {
            string serverKey = ConfigurationManager.AppSettings["SERVER_KEY_FIREBASE"];
            string senderId = ConfigurationManager.AppSettings["SENDER_ID_FIREBASE"];

            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"" + pTokenDestinatario + "\",\"notification\": {\"title\": \"" + pTitle + "\",\"body\": \"" + pBoby + "\"},\"priority\":\"high\"}";
                    //registration_ids, array of strings -  to, single recipient
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = await streamReader.ReadToEndAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}