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
using System;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO.User;
using GIGLS.Services.Implementation.Messaging;

namespace GIGLS.WebApi.Providers
{
    public class GiglsCustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
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

            var UserR = _repo._userManager.GetRolesAsync(user.Id).Result;

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

                if (user != null && user.UserChannelType == UserChannelType.Employee && user.SystemUserRole != "Dispatch Rider")
                {
                    //Global Property PasswordExpireDaysCount
                    var expiredDayCount = await _repo._globalProperty.GetAsync(s => s.Key == GlobalPropertyType.PasswordExpireDaysCount.ToString());
                    if (expiredDayCount == null)
                    {
                        context.SetError("password_expired", "Global Property PasswordExpireDaysCount does not exist.");
                        return;
                    }

                    int expiredDays = Convert.ToInt32(expiredDayCount.Value);

                    //check for password expiry
                    var LastUpdatePasswordDate = user.PasswordExpireDate;
                    DateTime TodayDate = DateTime.Now.Date;
                    var DayDifferent = (TodayDate - LastUpdatePasswordDate).Days;

                    if (DayDifferent >= expiredDays)
                    {
                        //Redirect to user reset page
                        context.SetError("password_expired", "The user account has expired.");
                        context.Response.Headers.Add("AuthorizationResponse", new[] { "expireduser" });
                        return;
                    }
                }

                if (user == null)
                {
                    context.SetError("invalid_user", "The user name or password is incorrect.");
                    context.Response.Headers.Add("AuthorizationResponse", new[] { "wronguser" });
                    return;
                }

                if (user.UserChannelType == UserChannelType.Employee)
                {
                    if (user.SystemUserId == null)
                    {
                        context.SetError("invalid_access", Newtonsoft.Json.JsonConvert.SerializeObject(new { result = false, message = "The user has not been assigned a role!" }));
                        return;
                    }
                }

                if (!user.IsActive)
                {
                    context.SetError("deactivated", "Your account has not been activated!");
                    return;
                }

                //differentiate expire toke time
                if(user.UserChannelType == UserChannelType.Partner || user.SystemUserRole == "Dispatch Rider")
                {
                    context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromDays(5);
                }

                if (user.UserChannelType == UserChannelType.Corporate || user.UserChannelType == UserChannelType.IndividualCustomer || user.UserChannelType == UserChannelType.Ecommerce)
                {
                    context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromDays(5);
                }

                if(user.UserChannelType == UserChannelType.Ecommerce || user.UserChannelType == UserChannelType.Corporate)
                {
                    var ecommerce = await _repo._companyProperty.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                    user.Organisation = ecommerce.Name;
                    user.FirstName = ecommerce.Name;
                    user.LastName = ecommerce.Name;
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");

                //preparing AuthenticationProperties Dictionary object
                var authPropDictionary = new Dictionary<string, string>
                {
                    { "UserId", user.Id},
                    { "UserName", user.UserName},
                    { "FirstName", user.FirstName},
                    { "LastName", user.LastName},
                    { "Email", user.Email},
                    { "UserChannelType", user.UserChannelType.ToString()},
                    { "SystemUserRole", user.SystemUserRole},
                    { "PhoneNumber", user.PhoneNumber},
                    { "IsActive", user.IsActive.ToString()},
                    { "Organization", user.Organisation},
                    { "Organisation", user.Organisation},
                    { "UserChannelCode", user.UserChannelCode},
                    { "PictureUrl", user.PictureUrl},
                    { "IsMagaya", user.IsMagaya.ToString()},
                    { "IsInternational", user.IsInternational.ToString()}
                };

                //get claims for the user
                string claimsStringArray = string.Empty;

                if (user.Claims.Any())
                {
                    claimsStringArray = string.Join(",", user.Claims.Select(x => x.ClaimValue));
                }

                authPropDictionary.Add("Claim", claimsStringArray);

                //get roles for the user
                var roleIds = from ur in user.Roles select ur.RoleId;
                var roles = _repo._roleManager.Roles.Where(s => roleIds.Contains(s.Id));

                string rolesStringArray = string.Empty;

                if (roles.Any())
                {
                    rolesStringArray = string.Join(",", roles.Select(x => x.Name));
                }

                authPropDictionary.Add("Role", rolesStringArray);

                var props = new AuthenticationProperties(authPropDictionary);
                var ticket = new AuthenticationTicket(oAuthIdentity, props);

                var isLoginSuccess = context.Validated(ticket);

                //After successful login, send email
                if (isLoginSuccess)
                {
                    var _messageSenderService = (IMessageSenderService)System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IMessageSenderService));
                    await _messageSenderService.SendGenericEmailMessage(MessageType.USER_LOGIN,
                        new UserDTO()
                        {
                            Email = user.Email
                        });
                }
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