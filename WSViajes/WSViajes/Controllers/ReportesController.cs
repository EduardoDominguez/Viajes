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
using Serilog;
using AutoMapper;


namespace WSViajes.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Reportes")]
    public class ReportesController : ApiController
    {
        [HttpGet]
        [Route("RptGanancias")]
        public async Task<HttpResponseMessage> RptGanancias([FromUri] FiltrosReporteGanancias pFiltros)
        {
            var respuesta = new ConsultaPorIdResponse<E_LISTA_PAGINADA<E_RPT_GANANCIAS>> { };
            var strMetodo = "WSViajes - RptGanancias ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<FiltrosReporteGanancias, E_FILTROS_REPORTE_GANANCIAS>()
                    );

                //Using automapper
                var mapper = new Mapper(config);
                var filtrosMap = mapper.Map<E_FILTROS_REPORTE_GANANCIAS>(pFiltros);

                respuesta.Data = await new ReportesNegocio().ConsultaReporteGanancias(filtrosMap);

                if (respuesta.Data.TotalRows > 0)
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
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }

        [HttpGet]
        [Route("RptGeneral")]
        public async Task<HttpResponseMessage> RptGeneral([FromUri] FiltrosReporteGeneral pFiltros)
        {
            var respuesta = new ConsultaPorIdResponse<E_LISTA_PAGINADA<E_RPT_GENERAL>> { };
            var strMetodo = "WSViajes - RptGeneral ";
            string sid = Guid.NewGuid().ToString();

            try
            {
                //Initialize the mapper
                var config = new MapperConfiguration(cfg =>
                        cfg.CreateMap<FiltrosReporteGeneral, E_FILTROS_REPORTE_GENERAL>()
                    );

                //Using automapper
                var mapper = new Mapper(config);
                var filtrosMap = mapper.Map<E_FILTROS_REPORTE_GENERAL>(pFiltros);

                respuesta.Data = await new ReportesNegocio().ConsultaReporteGeneral(filtrosMap);

                if (respuesta.Data.TotalRows > 0)
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
                Log.Error(Ex, "[" + strMetodo + "]" + "[SID:" + sid + "]" + strMensaje);

                respuesta.CodigoError = 10001;
                respuesta.Mensaje = "ERROR INTERNO DEL SERVICIO [" + strErrGUI + "]";
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, respuesta);
        }
    }
}
