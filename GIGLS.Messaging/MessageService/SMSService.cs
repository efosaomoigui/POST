using System.Threading.Tasks;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Exceptions;
using GIGLS.Core.IMessage;
using GIGLS.Core.DTO;

namespace GIGLS.Messaging.MessageService
{
    public class SMSService : ISMSService
    {
        public async Task SendAsync(MessageDTO message)
        {
            await ConfigSendGridasync(message);
        }

        // Use NuGet to install Twilio 
        private async Task<bool> ConfigSendGridasync(MessageDTO message)
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
                    to: new PhoneNumber(message.To),
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
