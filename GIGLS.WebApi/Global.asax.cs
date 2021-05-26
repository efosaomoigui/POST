//using Serilog;
//using SerilogWeb.Classic.Enrichers;
using System;
using System.Web.Http;

namespace GIGLS.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
           // Log.Logger = new LoggerConfiguration()
              //  .Enrich.With<HttpRequestIdEnricher>()
              //.WriteTo.Seq("http://localhost:5341/")
               // .WriteTo.RollingFile(pathFormat: (@"C:\Agilitylog\Log-{Date}.txt"))
                //.CreateLogger();


            // Important to call at exit so that batched events are flushed.
           // Log.CloseAndFlush();
        }
    }
}
