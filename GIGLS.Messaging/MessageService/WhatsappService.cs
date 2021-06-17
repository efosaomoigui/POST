using GIGLS.Core.DTO;
using GIGLS.Core.IMessageService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Messaging.MessageService
{
    public class WhatsappService : IWhatsappService
    {
        public async Task<string> SendWhatsappMessageAsync(WhatsAppMessageDTO message)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(message.RecipientWhatsapp))
            {
                WhatsAppMessagesDTO messageDTO = new WhatsAppMessagesDTO();
                messageDTO.Message.Add(message);
                result = await ConfigSendWhatsappMessageAsync(messageDTO);
            }
            return result;
        }
        public async Task<string> GetConsentDetailsAsync(WhatsappNumberDTO number)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(number.PhoneNumber))
            {
                result = await ConfigGetConsentDetailsAsync(number);
            }
            return result;
        }
        public async Task<string> ManageOptInOutAsync(ManageWhatsappConsentDTO consent)
        {
            string result = "";
            if (consent != null)
            {
                result = await ConfigManageOptInOutAsync(consent);
            }
            return result;
        }
        private async Task<string> ConfigSendWhatsappMessageAsync(WhatsAppMessagesDTO message)
        {
            var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
            var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];

            using (HttpClient client = new HttpClient())
            {
                if (whatsappToken != null && whatsappToken.Length != 0)
                {
                    client.BaseAddress = new Uri(whatsappUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                }
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    if (message != null)
                    {
                        response = await client.PostAsJsonAsync("message", message);
                        if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            return response.StatusCode.ToString();
                        }
                        else
                        {
                            return response.StatusCode.ToString();
                        }
                    }
                    else
                    {
                        return "Whatsapp message cannot be null or empty";
                    }
                }
                catch (Exception ex)
                {

                    return ex.Message.ToString();
                }

            }
        }
        private async Task<string> ConfigGetConsentDetailsAsync(WhatsappNumberDTO number)
        {
            var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
            var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];

            using (HttpClient client = new HttpClient())
            {
                if (whatsappToken != null && whatsappToken.Length != 0)
                {
                    client.BaseAddress = new Uri(whatsappUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                }
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    if (number != null)
                    {
                        response = await client.PostAsJsonAsync("consent", number);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return response.StatusCode.ToString();
                        }
                        else
                        {
                            return response.StatusCode.ToString();
                        }
                    }
                    else
                    {
                        return "Whatsapp message cannot be null or empty";
                    }
                }
                catch (Exception ex)
                {

                    return ex.Message.ToString();
                }

            }
        }
        private async Task<string> ConfigManageOptInOutAsync(ManageWhatsappConsentDTO consent)
        {
            var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
            var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];

            using (HttpClient client = new HttpClient())
            {
                if (whatsappToken != null && whatsappToken.Length != 0)
                {
                    client.BaseAddress = new Uri(whatsappUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                }
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    if (consent != null)
                    {
                        response = await client.PostAsJsonAsync("consent/manage", consent);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return response.StatusCode.ToString();
                        }
                        else
                        {
                            return response.StatusCode.ToString();
                        }
                    }
                    else
                    {
                        return "Whatsapp message cannot be null or empty";
                    }
                }
                catch (Exception ex)
                {

                    return ex.Message.ToString();
                }

            }
        }
    }
}
