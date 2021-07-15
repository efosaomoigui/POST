using GIGLS.Core.DTO;
using GIGLS.Core.IMessageService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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
                whatsappUrl = $"{whatsappUrl}message/"; 

                string result = "";
                using (var client = new HttpClient())
                {
                    if (whatsappToken != null && whatsappToken.Length != 0)
                    {
                        if (message != null)
                        {
                            System.Net.ServicePointManager.SecurityProtocol |=
                                                                            SecurityProtocolType.Tls12 |
                                                                            SecurityProtocolType.Tls11 |
                                                                            SecurityProtocolType.Tls;

                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                            var json = JsonConvert.SerializeObject(message);
                            var data = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(whatsappUrl, data);
                            result = await response.Content.ReadAsStringAsync();
                            var status = JObject.Parse(result)["status"].ToString();
                            if (status.Contains("success"))
                            {
                                var id = JObject.Parse(result)["data"]["id"].ToString();
                                result = $"Status : {status}, Id : {id}";
                            }
                            else
                            {
                                result = $"Status : {status}";
                            }
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

        private async Task<string> ConfigGetConsentDetailsAsync(WhatsappNumberDTO number)
        {
            try
            {
                var whatsappToken = ConfigurationManager.AppSettings["WhatsAppToken"];
                var whatsappUrl = ConfigurationManager.AppSettings["WhatsAppUrl"];
                whatsappUrl = $"{whatsappUrl}consent/";
                string result = "";
                using (var client = new HttpClient())
                {
                    if (whatsappToken != null && whatsappToken.Length != 0)
                    {
                        System.Net.ServicePointManager.SecurityProtocol |=
                                                                            SecurityProtocolType.Tls12 |
                                                                            SecurityProtocolType.Tls11 |
                                                                            SecurityProtocolType.Tls;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                        var json = JsonConvert.SerializeObject(number);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(whatsappUrl, data);
                        result = await response.Content.ReadAsStringAsync();
                        result = JObject.Parse(result)["status"].ToString();
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
                whatsappUrl = $"{whatsappUrl}consent/manage/";
                string result = "";
                using (var client = new HttpClient())
                {
                    if (whatsappToken != null && whatsappToken.Length != 0)
                    {
                        System.Net.ServicePointManager.SecurityProtocol |=
                                                                            SecurityProtocolType.Tls12 |
                                                                            SecurityProtocolType.Tls11 |
                                                                            SecurityProtocolType.Tls;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whatsappToken);
                        var json = JsonConvert.SerializeObject(consent);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(whatsappUrl, data);
                        result = await response.Content.ReadAsStringAsync();
                        result = JObject.Parse(result)["status"].ToString();
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
