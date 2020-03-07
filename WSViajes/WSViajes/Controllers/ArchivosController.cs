using log4net;
using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WSViajes.Exceptions;
using WSViajes.Models.Request;
using WSViajes.Models.Response;


namespace WSViajes.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Archivos")]
    public class ArchivosController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpPost]
        [Route("Img/Upload")]
        public HttpResponseMessage ImgUpload([FromBody] UploadFileRequest pUploadRequest)
        {
            var respuesta = new UploadFileResponse { };
            var strMetodo = "WSViajes - Img/Upload ";
            string sid = Guid.NewGuid().ToString();
            string strRuta = "Assets/Img/";
            string workingDirectory = string.Empty;
            try
            {
                if (pUploadRequest == null)
                    respuesta.Mensaje = "No se recibió archivo.";
                else if (String.IsNullOrEmpty(pUploadRequest.BASE64))
                    respuesta.Mensaje = "El elemento  <<BASE64>> no puede estar vacío.";
                else
                {

                    if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets")))
                    {
                        Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets"));

                        if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img")))
                        {
                            Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath($"~/Assets/Img"));
                        }
                    }

                    if (String.IsNullOrEmpty(pUploadRequest.NOMBRE))
                        strRuta += $"{Guid.NewGuid().ToString()}.png";
                    else
                        strRuta += $"{pUploadRequest.NOMBRE}.png";



                    workingDirectory = System.Web.Hosting.HostingEnvironment.MapPath($"~/{strRuta}");
                    File.WriteAllBytes(workingDirectory, Convert.FromBase64String(pUploadRequest.BASE64.Substring(pUploadRequest.BASE64.IndexOf(",") + 1)));


                    respuesta.Exito = true;
                    respuesta.URL = $"{Url.Content("~/")}{strRuta}";
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
