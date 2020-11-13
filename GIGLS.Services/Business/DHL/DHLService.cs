using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.DHL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.DHL
{
    public class DHLService : IDHLService
    {
        public Task<InternationalShipmentDTO> AddInternationalShipment(InternationalShipmentDTO shipmentDTO)
        {
            throw new NotImplementedException();
        }

        public Task<InternationalShipmentDTO> GetInternationalShipmentPrice(InternationalShipmentDTO shipmentDTO)
        {
            throw new NotImplementedException();
        }

        private async Task<InternationalShipmentDTO> GetDHLPrice(InternationalShipmentDTO shipmentDTO)
        {
            try
            {
                //Get Price from DHL
                var rateRequest = GetRateRequestPayload(shipmentDTO);

                string baseUrl = ConfigurationManager.AppSettings["DHLBaseUrl"];
                string path = ConfigurationManager.AppSettings["DHLPriceRequest"];
                string url = baseUrl + path;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var byteArray = GetAutorizationKey();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    var json = JsonConvert.SerializeObject(rateRequest);
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RateRequestResponse>(responseResult);
                    return shipmentDTO;

                }

                return shipmentDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] GetAutorizationKey()
        {
            string username = ConfigurationManager.AppSettings["DHLUsername"];
            string secretKey = ConfigurationManager.AppSettings["DHLKey"];
            var key = Encoding.ASCII.GetBytes("username:secretKey");
            return key;
        }

        private RateRequestPayload GetRateRequestPayload(InternationalShipmentDTO shipmentDTO)
        {
            var rateRequest = new RateRequest();
            rateRequest.RequestedShipment.DropOffType = DropOffType.REGULAR_PICKUP.ToString();
            rateRequest.RequestedShipment.ShipTimestamp = DateTime.Now.AddDays(2);
            rateRequest.RequestedShipment.UnitOfMeasurement = UnitOfMeasurement.SI.ToString();
            rateRequest.RequestedShipment.Content = shipmentDTO.Content;

            //Shipper address -- GIGL default address
            var address = GetShipperAddress();
            rateRequest.RequestedShipment.Ship.Shipper.City = address.City;
            rateRequest.RequestedShipment.Ship.Shipper.PostalCode = address.PostalCode;
            rateRequest.RequestedShipment.Ship.Shipper.CountryCode = address.CountryCode;

            string dhlAccount = ConfigurationManager.AppSettings["DHLAccount"];
            rateRequest.RequestedShipment.Account = dhlAccount;

            //Receiver Address
            rateRequest.RequestedShipment.Ship.Recipient.City = shipmentDTO.ReceiverCity;
            rateRequest.RequestedShipment.Ship.Recipient.PostalCode = shipmentDTO.RecieverPostalCode;
            rateRequest.RequestedShipment.Ship.Recipient.CountryCode = shipmentDTO.ReceiverCode;

            int count = 1;
            foreach (var item in shipmentDTO.ShipmentItems)
            {
                var package = new RequestedPackages();
                package.number = count;
                package.Weight.Value = (decimal)item.Weight;
                package.Dimensions.Length = (decimal)item.Length;
                package.Dimensions.Width = (decimal)item.Width;
                package.Dimensions.Height = (decimal)item.Height;

                //Packages
                rateRequest.RequestedShipment.Packages.RequestedPackages.Add(package);
                count++;
            }

            RateRequestPayload ratePayload = new RateRequestPayload
            {
                RateRequest = rateRequest
            };
            return ratePayload;
        }

        private AddressPayload GetShipperAddress()
        {
            AddressPayload address = new AddressPayload
            {
                City = "Lagos",
                CountryCode = "100001",
                PostalCode = "NG"
            };

            return address;
        }
    }
}
