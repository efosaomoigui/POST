using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Infrastructure.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;
using GIGL.GIGLS.Core.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GIGLS.WebApi.App_Start 
{
    public class GiglsSimpleCustomOAuthProvider: OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public GiglsSimpleCustomOAuthProvider(string publicClientId) 
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clientId = string.Empty;
            //string clientSecret = string.Empty;
            //string symmetricKeyAsBase64 = string.Empty;

            //if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            //{
            //    context.TryGetFormCredentials(out clientId, out clientSecret);
            //}

            //if (context.ClientId == null)
            //{
            //    context.SetError("invalid_clientId", "client_Id is not set");
            //    return Task.FromResult<object>(null);
            //}

            //var audience = AudiencesStore.FindAudience(context.ClientId);//user our CRUD here

            //if (audience == null)
            //{
            //    context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
            //    return Task.FromResult<object>(null);
            //}

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            using (var _repo = new AuthRepository<User, GIGLSContext>(new GIGLSContext()))
            {
                User user = await _repo._userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                if (!user.IsActive)
                {
                    context.SetError("invalid_grant", "Your account has not been activeated!");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);

        }
    }
}