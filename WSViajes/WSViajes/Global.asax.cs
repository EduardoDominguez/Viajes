using System;
using System.Web.Http;
using WSViajes.AutoMapper;
using Serilog;
using Serilog.Sinks.MSSqlServer;


namespace WSViajes
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.RegisterMappings();
            var cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ViajesEntities"].ConnectionString;
            //cadena = cadena.Substring(cadena.LastIndexOf("data source"), cadena.LastIndexOf("MultipleActiveResultSets=True"));
            var index = cadena.LastIndexOf("data source");
            cadena = cadena.Substring(index);
            cadena = cadena.Substring(0, cadena.LastIndexOf("MultipleActiveResultSets=True"));

            Log.Logger = new LoggerConfiguration()
            .WriteTo
            .MSSqlServer(
                connectionString: cadena,
                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" })
            .CreateLogger();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Exception ex = Server.GetLastError();

            try
            {
                //log.Error("\r\n_________________________" + ex.Message + "\r\n InnerException: " + ex.InnerException.Message + "_________________________", ex);
                Log.Error(ex,  ex.Message + "\r\n InnerException: " + ex.InnerException.Message);
            }
            catch
            {
                //log.Error("\r\n_________________________" + ex.Message + "\r\n _________________________", ex);
                Log.Error(ex, ex.Message);
            }
        }
    }
}
