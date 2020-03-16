using log4net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Viajes.BL.Banner;
using Viajes.EL.Extras;
using WSViajes.Comunes;
using WSViajes.Exceptions;
using WSViajes.Models;
using WSViajes.Models.Request;
using WSViajes.Models.Response;


namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Banners")]
    public class BannerController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> ConsultaBanners()
        {
            var respuesta = new ConsultarTodoResponse<E_BANNER> { };
            var strMetodo = "WSViajes - ConsultaBanners ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                respuesta.Data = await new BannerNegocio().ConsultarTodo();

                if (respuesta.Data.Count > 0)
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = $"Registros cargados con éxito";
                }
                else
                {
                    respuesta.CodigoError = 10000;
                    respuesta.Mensaje = $"No existen banners.";
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
