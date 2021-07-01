using GIGLS.Core.DTO;
using GIGLS.Core.IMessageService;
using Newtonsoft.Json;
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
            try
            {
                var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
                var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];
                
                string result = "";
                using (HttpClient client = new HttpClient())
                {
                    if (whatsappToken != null && whatsappToken.Length != 0)
                    {
                        client.BaseAddress = new Uri(whatsappUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);

                        if (message != null)
                        {
                            var response = await client.PostAsJsonAsync("message", message);
                            result = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            result = $"Message cannot be null. Please provide the appropriate values";
                        }
                    }
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> ConfigGetConsentDetailsAsync(WhatsappNumberDTO number)
        {
            try
            {
                var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
                var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];
                whatsappUrl = $"{whatsappUrl}/consent";
                string result = "";
                using (HttpClient client = new HttpClient())
                {
                    if (whatsappToken != null && whatsappToken.Length != 0)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                        var json = JsonConvert.SerializeObject(number);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(whatsappUrl, data);
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> ConfigManageOptInOutAsync(ManageWhatsappConsentDTO consent)
        {
            try
            {
                var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
                var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];
                string result = "";
                using (HttpClient client = new HttpClient())
                {
                    if (whatsappToken != null && whatsappToken.Length != 0)
                    {
                        client.BaseAddress = new Uri(whatsappUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);

                        HttpResponseMessage response = new HttpResponseMessage();

                        if (consent != null)
                        {
                            response = await client.PostAsJsonAsync("consent/manage", consent);
                            result = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            result = $"Message cannot be null. Please provide the appropriate values";
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
