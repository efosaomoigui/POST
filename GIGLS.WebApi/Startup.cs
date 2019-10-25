using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using GIGLS.WebApi.Providers;
using Hangfire;
using GlobalConfiguration = Hangfire.GlobalConfiguration;
using Hangfire.SqlServer;

[assembly: OwinStartup(typeof(GIGLS.WebApi.Startup))]

namespace GIGLS.WebApi
{
    public partial class Startup
    {
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
            GlobalConfiguration.Configuration.UseSqlServerStorage("GIGLSContextDB");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }


        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
       
    }
}
