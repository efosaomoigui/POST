using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Exceptions;

namespace GIGLS.Messaging.MessageService
{
    public class SMSService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await ConfigSendGridasync(message);
        }

        // Use NuGet to install Twilio 
        private async Task<bool> ConfigSendGridasync(IdentityMessage message)
        {
            bool result = false;

            // Use your account SID and authentication token instead
            // of the placeholders shown here.
            string accountSID = ConfigurationManager.AppSettings["smsService:accountSID"];
            string authToken = ConfigurationManager.AppSettings["smsService:authToken"];

            var fromNumber = ConfigurationManager.AppSettings["smsService:FromNumber"];

            // Initialize the TwilioClient.
            TwilioClient.Init(accountSID, authToken);

            try
            {
                // Send an SMS message.
                var msg = MessageResource.Create(
                    to: new PhoneNumber(message.Destination),
                    from: new PhoneNumber(fromNumber),
                    body: message.Body);

                result = true;
            }
            catch (TwilioException ex)
            {
                // An exception occurred making the REST call
                throw ex;
            }

            return await Task.FromResult(result);
        }
    }
}
