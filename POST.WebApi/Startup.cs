using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using POST.WebApi.Providers;
using Hangfire;
using System.Timers;
using POST.CORE.IServices.Report;
//using GlobalConfiguration = Hangfire.GlobalConfiguration;
//using Hangfire.SqlServer;
//using Ninject;

[assembly: OwinStartup(typeof(POST.WebApi.Startup))]

namespace POST.WebApi
{
    public partial class Startup
    {
        const double minute = 6000 * 3;
        private readonly IShipmentReportService service;
        public void Configuration(IAppBuilder app)
        {
            //var config = new HttpConfiguration();
            ////app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //ConfigureAuth(app);
            //ConfigureOAuthTokenConsumption(app);
            //app.UseWebApi(config);

            HttpConfiguration config = new HttpConfiguration();

            ConfigureAuth(app);
            ConfigureOAuthTokenConsumption(app);
            //ConfigureWebApi(config);
            app.Use<CustomAuthenticationMiddleware>();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(config);
            //GlobalConfiguration.Configuration.UseSqlServerStorage("GIGLSContextDB", new SqlServerStorageOptions
            //{
            //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //    QueuePollInterval = TimeSpan.Zero,
            //    UseRecommendedIsolationLevel = true,
            //    UsePageLocksOnDequeue = true,
            //    DisableGlobalLocks = true
            //});
            //app.UseHangfireServer();
            //app.UseHangfireDashboard();
            //MethodCaller_Timer();

        }


        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        //protected void MethodCaller_Timer()
        //{
        //    Timer timerforMail = new Timer(120000);
        //    Timer tminute = new Timer(minute);

        //    timerforMail.Elapsed += new ElapsedEventHandler(timerforMail_Elapsed);
        //    timerforMail.Enabled = true;
        //}

        //private void timerforMail_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    using (Controllers.CustomerInvoice.CustomerInvoiceController req = new Controllers.CustomerInvoice.CustomerInvoiceController(service))
        //    {
        //        req.GenerateCustomerInvoice();
        //    }
        //}

    }
}
