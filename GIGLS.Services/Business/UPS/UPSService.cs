using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.UPS;
using GIGLS.Core.IServices.UPS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.UPS
{
    public class UPSService : IUPSService
    {
        public async Task<UPSShipmentResponsePayload> CreateUPSShipment(InternationalShipmentDTO shipmentDto)
        {
            try
            {
                var result = new UPSShipmentResponsePayload();

                //Get Price from DHL
                var request = GetShipmentRequestPayload(shipmentDto);

                string baseUrl = ConfigurationManager.AppSettings["UPSBaseUrl"];
                string path = ConfigurationManager.AppSettings["UPSShipmentRequest"];
                string url = baseUrl + path;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var json = JsonConvert.SerializeObject(request);
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UPSShipmentResponsePayload>(responseResult);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private UPSShipmentRequest GetShipmentRequestPayload(InternationalShipmentDTO shipmentDto)
        {
            var shipment = new UPSShipmentRequest();

            shipment.UPSSecurity = GetUPSSecurity();
            shipment.ShipmentRequest = GetShipmentRequest(shipmentDto);

            return shipment;
        }

        private UPSSecurity GetUPSSecurity()
        {
            string username = ConfigurationManager.AppSettings["UPSUsername"];
            string password = ConfigurationManager.AppSettings["UPSPassword"];
            string accessLicenseNumber = ConfigurationManager.AppSettings["UPSAccessLicenseNumber"];

            var security = new UPSSecurity();
            security.UsernameToken.Username = username;
            security.UsernameToken.Password = password;
            security.ServiceAccessToken.AccessLicenseNumber = accessLicenseNumber;
            return security;
        }

        private ShipmentRequest GetShipmentRequest(InternationalShipmentDTO shipmentDto)
        {
            //Default value for Request and LabelSpecification
            var payload = new ShipmentRequest();
            payload.Shipment = BindShipmentPayload(shipmentDto);
            return payload;
        }

        private UPSShipment BindShipmentPayload(InternationalShipmentDTO shipmentDto)
        {
            //Default value for Service
            var shipment = new UPSShipment();
            shipment.Description = shipmentDto.Description;
            shipment.Shipper = GetShipperInfo();

            return shipment;

        }

        private UPSCustomerInfo GetShipperInfo()
        {
            var shipper = new UPSCustomerInfo
            {
                Name = "GIG Logistics"
            };
            shipper.Address.AddressLine = "GIG LOGISTICS BUILDING, BEHIND MOBI";
            shipper.Address.City = "Lagos";
            shipper.Address.StateProvinceCode = "";
            shipper.Address.PostalCode = "100001";
            shipper.Address.CountryCode = "NG";

            string shipperNumber = ConfigurationManager.AppSettings["UPSShipperNumber"];
            string contactPerson = ConfigurationManager.AppSettings["UPSGIGContactPerson"];
            string contactPhoneNumber = ConfigurationManager.AppSettings["UPSGIGPhoneNumber"];
            string contactPhoneExtension = ConfigurationManager.AppSettings["UPSGIGPhoneExt"];
            
            shipper.ShipperNumber = shipperNumber;
            shipper.AttentionName  = contactPerson;
            shipper.Phone.Extension = contactPhoneExtension;
            shipper.Phone.Number = contactPhoneNumber;
            return shipper;
        }
    }
}
