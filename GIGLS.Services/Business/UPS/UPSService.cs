using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.UPS;
using GIGLS.Core.IServices.UPS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.UPS
{
    public class UPSService : IUPSService
    {
        public async Task<InternationalShipmentWaybillDTO> CreateInternationalShipment(InternationalShipmentDTO shipmentDTO)
        {
            var createShipment = await CreateUPSShipment(shipmentDTO);
            var result = FormatShipmentCreationReponse(createShipment);
            return result;
        }

        //yet to be implemented
        private InternationalShipmentWaybillDTO FormatShipmentCreationReponse(UPSShipmentResponsePayload createShipment)
        {
            InternationalShipmentWaybillDTO output = new InternationalShipmentWaybillDTO();

            return output;
        }

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
            var shipment = new UPSShipmentRequest
            {
                UPSSecurity = GetUPSSecurity(),
                ShipmentRequest = GetShipmentRequest(shipmentDto)
            };

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
            var payload = new ShipmentRequest
            {
                Shipment = BindShipmentPayload(shipmentDto)
            };
            return payload;
        }

        private UPSShipment BindShipmentPayload(InternationalShipmentDTO shipmentDto)
        {
            //Default value for Service
            var shipment = new UPSShipment
            {
                Description = shipmentDto.Description,
                Shipper = GetShipperInfo(),
                ShipTo = GetReceiverInfo(shipmentDto),
                PaymentInformation = GetPaymentInformation(),
                Package = GetPackageList(shipmentDto)
            };

            return shipment;
        }

        private UPSPaymentInformation GetPaymentInformation()
        {
            string accountNumber = ConfigurationManager.AppSettings["UPSAccountNumber"];
            var payment = new UPSPaymentInformation();
            payment.ShipmentCharge.BillShipper.AccountNumber = accountNumber;

            throw new NotImplementedException();
        }

        private UPSCustomerInfo GetShipperInfo()
        {
            string shipperNumber = ConfigurationManager.AppSettings["UPSShipperNumber"];
            string staff = ConfigurationManager.AppSettings["UPSGIGContactPerson"];
            string phoneNumber = ConfigurationManager.AppSettings["UPSGIGPhoneNumber"];
            string phoneExtension = ConfigurationManager.AppSettings["UPSGIGPhoneExt"];

            var shipper = new UPSCustomerInfo
            {
                Name = "GIG Logistics"
            };
            shipper.Address.AddressLine = "GIG LOGISTICS BUILDING, BEHIND MOBI";
            shipper.Address.City = "Lagos";
            shipper.Address.StateProvinceCode = "";
            shipper.Address.PostalCode = "100001";
            shipper.Address.CountryCode = "NG";            
            shipper.ShipperNumber = shipperNumber;
            shipper.AttentionName  = staff;
            shipper.Phone.Extension = phoneNumber;
            shipper.Phone.Number = phoneExtension;
            return shipper;
        }

        private UPSCustomerInfo GetReceiverInfo(InternationalShipmentDTO shipmentDto)
        {
            var reciever = new UPSCustomerInfo
            {
                Name = shipmentDto.ReceiverCompanyName,
                AttentionName = shipmentDto.ReceiverName
            };
            reciever.Address.AddressLine = shipmentDto.ReceiverAddress;
            reciever.Address.City = shipmentDto.ReceiverCity;
            reciever.Address.StateProvinceCode = shipmentDto.ReceiverCountryCode.Length <= 2 ? shipmentDto.ReceiverCountryCode : shipmentDto.ReceiverCountryCode.Substring(0, 2);
            reciever.Address.PostalCode = shipmentDto.ReceiverPostalCode;
            reciever.Address.CountryCode = shipmentDto.ReceiverCountryCode;
            reciever.Phone.Number = shipmentDto.ReceiverPhoneNumber; 
            return reciever;
        }

        private List<UPSPackage> GetPackageList(InternationalShipmentDTO shipmentDto)
        {
            var packageList = new List<UPSPackage>();

            int count = 1;
            foreach (var item in shipmentDto.ShipmentItems)
            {
                var package = new UPSPackage
                {
                    Description = item.Description                    
                };
                //package.Packaging.Code = "02";
                //package.Packaging.Description = "Customer Supplied Package";

                package.Dimensions.Length = item.Length.ToString();
                package.Dimensions.Width = item.Width.ToString();
                package.Dimensions.Height = item.Height.ToString();

                package.PackageWeight.UnitOfMeasurement.Code = "KGS";
                package.PackageWeight.UnitOfMeasurement.Description = "KILOGRAMS";
                package.PackageWeight.Weight = item.Weight.ToString();

                //Packages
                packageList.Add(package);
                count++;
            }

            return packageList;
        }
    }
}
