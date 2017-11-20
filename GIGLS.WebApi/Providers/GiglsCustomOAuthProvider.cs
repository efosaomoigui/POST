using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Infrastructure.Persistence;
using GIGL.GIGLS.Core.Domain;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;

namespace GIGLS.WebApi.Providers
{
    public class GiglsCustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        //Check to validate a client's credentials
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


        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, User user)
        {
            var _repo = new AuthRepository<User, GIGLSContext>(new GIGLSContext());
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("Userid", user.Id));
            var resultClaimList = _repo._userManager.GetClaimsAsync(user.Id).Result;
            identity.AddClaims(resultClaimList);

            //var userRoles = context.OwinContext.Get<ApplicationUserManager>().GetRoles(user.Id);
            //var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var UserR = _repo._userManager3.GetRolesAsync(user).Result;

            foreach (var role in UserR)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return identity;
        }


        //Check to validate a user's credentials
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Very important for the token service. Remember, same service is also available for webpiconfig
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

                if (user.SystemUserId == null)
                {
                    context.SetError("invalid_grant", "The user has not been assigned a role.");
                    return;
                }

                //if (!user.LockoutEnabled) 
                //{
                //    context.SetError("invalid_grant", "Your account is locked!");
                //    return;
                //}

                if (!user.IsActive)
                {
                    context.SetError("invalid_grant", "Your account has not been activated!");
                    return;
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");

                //preparing AuthenticationProperties Dictionary object
                var authPropDictionary = new Dictionary<string, string>
                {
                    { "UserId", user.Id},
                    { "UserName", user.UserName},
                    { "FirstName", user.FirstName},
                    { "LastName", user.LastName},
                    { "Email", user.Email}
                };

                //get claims for the user
                var claimsStringArray = "";
                foreach(var claim in user.Claims)
                {
                    claimsStringArray += claim.ClaimValue + ",";
                }
                authPropDictionary.Add("Claim", claimsStringArray);

                //get roles for the user
                var roleIds = from ur in user.Roles select ur.RoleId;
                var roles = _repo._roleManager.Roles.Where(s => roleIds.Contains(s.Id));
                var rolesStringArray = "";
                foreach (var role in roles)
                {
                    rolesStringArray += role.Name + ",";
                }
                authPropDictionary.Add("Role", rolesStringArray);

                var props = new AuthenticationProperties(authPropDictionary);
                var ticket = new AuthenticationTicket(oAuthIdentity, props);

                context.Validated(ticket);
            }
        }


        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}