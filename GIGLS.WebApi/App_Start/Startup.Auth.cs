using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using GIGLS.Infrastructure.Persistence;
using GIGLS.WebApi.Providers;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Configuration;
using GIGLS.WebApi.App_Start;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;

namespace GIGLS.WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; } 

        public static string PublicClientId { get; private set; }
        private string ApiUrl = ConfigurationManager.AppSettings["WebApiUrl"];
        string secret = ConfigurationManager.AppSettings["as:AudienceSecret"]; 

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(GIGLSContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthServerOptions = new OAuthAuthorizationServerOptions 
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new GiglsCustomOAuthProvider(),
                AccessTokenFormat = new GiglsCustomJwtFormat(ApiUrl)

            };
            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }


        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = ApiUrl;
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

    }
}
