using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using GIGLS.Infrastructure;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Collections.Generic;
using EfeAuthen.Models;

namespace GIGLS.WebApi.Controllers.User
{
    [Authorize]
    public class LoginController : BaseWebApiController
    {
        public LoginController(string controllerName) : base(nameof(LoginController))
        {
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            //trim
            if(userLoginModel.username != null)
            {
                userLoginModel.username = userLoginModel.username.Trim();
            }

            if (userLoginModel.Password != null)
            {
                userLoginModel.Password = userLoginModel.Password.Trim();
            }

            //const string apiBaseUri = "http://localhost/GIGLS.WebApi/";
            const string apiBaseUri = "http://giglsresourceapi.azurewebsites.net/api/";
            string getTokenResponse;

            return await HandleApiOperationAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    //setup client
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //setup login data
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("grant_type", "password"),
                         new KeyValuePair<string, string>("Username", userLoginModel.username),
                         new KeyValuePair<string, string>("Password", userLoginModel.Password),
                     });

                    //setup client
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //setup login data

                    HttpResponseMessage responseMessage = client.PostAsync("Token", formContent).Result;
                    //get access token from response body
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);

                    getTokenResponse = jObject.GetValue("access_token").ToString();

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Operation could not complete login successfully:");
                    }

                    return new ServiceResponse<JObject>
                    {
                        Object = jObject
                    };
                }
            });
        }

    }
}
