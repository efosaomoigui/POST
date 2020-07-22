using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class UssdService : IUssdService
    {
        public async Task<USSDResponse> ProcessPaymentForUSSD(USSDDTO ussdDto)
        {
            try
            {
                var responseResult = new USSDResponse();

                //1. Get Token  
                string token = ConfigurationManager.AppSettings["UssdToken"];
                string merchantId = ConfigurationManager.AppSettings["UssdMerchantID"];
                string baseUrl = ConfigurationManager.AppSettings["UssdOgaranyaAPI"];

                //2. Encrypt token and private_key to generate public key 
                string publicKey = GetPublicKey();

                ussdDto.country_code = ussdDto.country_code.Length <= 2 ? ussdDto.country_code : ussdDto.country_code.Substring(0, 2);
                string pay01Url = baseUrl + merchantId + "/pay/" + ussdDto.country_code;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("token", token);
                    client.DefaultRequestHeaders.Add("publickey", publicKey);

                    var ussdDataInJson = JsonConvert.SerializeObject(ussdDto);
                    var data = new StringContent(ussdDataInJson, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(pay01Url, data);
                    string result = await response.Content.ReadAsStringAsync();

                    responseResult = JsonConvert.DeserializeObject<USSDResponse>(result);
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<USSDResponse> VerifyAndValidateUSSDPayment(USSDDTO ussdDto)
        {
            throw new NotImplementedException();
        }

        private string GetPublicKey()
        {
            //1. Get Token  
            string token = ConfigurationManager.AppSettings["UssdToken"];
            string privateKey = ConfigurationManager.AppSettings["UssdPrivateKey"];

            string publicKey = string.Empty;

            var bytes = Encoding.UTF8.GetBytes(token + privateKey);

            using (var hash = new SHA512Managed())
            {
                var hashedData = hash.ComputeHash(bytes);
                publicKey = BitConverter.ToString(hashedData).Replace("-", "").ToLower();
            }

            return publicKey;
        }
    }
}
