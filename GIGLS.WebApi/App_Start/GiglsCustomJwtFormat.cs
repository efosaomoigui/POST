using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace GIGLS.WebApi.App_Start
{
    //API to issue JWT tokens instead of default access tokens
    public class GiglsCustomJwtFormat: ISecureDataFormat<AuthenticationTicket>
    {
    
        private readonly string _issuer = string.Empty;
 
        //There is no direct support for JWT so hence this implementation
        public GiglsCustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }
 
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
 
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];
 
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var signingKey = new HmacSigningCredentials(keyByteArray);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
 
            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
 
            return jwt;
        }
 
        public AuthenticationTicket Unprotect(string protectedText)
        {
            return null;
        }
    
    }
}