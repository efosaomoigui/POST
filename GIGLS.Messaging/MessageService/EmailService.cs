using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using GIGLS.Core.IMessage;
using GIGLS.Core.DTO;

namespace GIGLS.Messaging.MessageService
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(MessageDTO message)
        {
            await ConfigSendGridasync(message);
        }

        private async Task ConfigSendGridasync(MessageDTO message)
        {
            var myMessage = new SendGridMessage();
            var fromEmail = ConfigurationManager.AppSettings["emailService:FromEmail"];
            var fromName = ConfigurationManager.AppSettings["emailService:FromName"];

            myMessage.AddTo(message.To);
            myMessage.From = new SendGrid.Helpers.Mail.EmailAddress(fromEmail, fromName);
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.Body;
            myMessage.HtmlContent = message.Body;

            var apiKey = ConfigurationManager.AppSettings["emailService:API_KEY"];
            var client = new SendGridClient(apiKey);

            var response = await client.SendEmailAsync(myMessage);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        //private async Task ConfigSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();

        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new SendGrid.Helpers.Mail.EmailAddress("taiseer@gigls.net", "Efe Omoigui");
        //    myMessage.Subject = message.Subject;
        //    myMessage.PlainTextContent = message.Body;
        //    myMessage.HtmlContent = message.Body;

        //    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
        //                                            ConfigurationManager.AppSettings["emailService:Password"]);

        //    // Create a Web transport for sending email.
        //    var transportWeb = new SendGrid.Web(credentials);

        //    //Send the email.
        //    if (transportWeb != null)
        //    {
        //        await transportWeb.DeliverAsync(myMessage);
        //    }
        //    else
        //    {
        //        Trace.TraceError("Failed to create Web transport.");

        //    }

        //    await Task.FromResult(0);
        //}
    }
}
