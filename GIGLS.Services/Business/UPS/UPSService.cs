using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.UPS;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.UPS;
using GIGLS.Infrastructure;
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

        private InternationalShipmentWaybillDTO FormatShipmentCreationReponse(UPSShipmentResponseFinalPayload upsResponse)
        {
            var finalResult = new InternationalShipmentWaybillDTO
            {
                OutBoundChannel = CompanyMap.UPS,
                ResponseResult = upsResponse.ResponseResult
            };

            if (upsResponse.Fault != null)
            {
                throw new GenericException($"There was an issue processing your request: " +
                    $"{upsResponse.Fault.Detail.Errors.ErrorDetail.PrimaryErrorCode.Description}");
            }
            else
            {
                finalResult.ShipmentIdentificationNumber = upsResponse.ShipmentResponse.ShipmentResults.ShipmentIdentificationNumber;

                if (upsResponse.ShipmentResponse.ShipmentResults.FinalPackageResults.Any())
                {
                    string[] itemIds = upsResponse.ShipmentResponse.ShipmentResults.FinalPackageResults.Select(x => x.TrackingNumber).ToArray();
                    finalResult.PackageResult = string.Join(",", itemIds);
                }
            }

            return finalResult;
        }

        public async Task<UPSShipmentResponseFinalPayload> CreateUPSShipment(InternationalShipmentDTO shipmentDto)
        {
            try
            {
                var result = new UPSShipmentResponseFinalPayload();

                //Get Price from DHL
                var request = GetShipmentRequestPayload(shipmentDto);

                string baseUrl = ConfigurationManager.AppSettings["UPSBaseUrl"];
                string path = ConfigurationManager.AppSettings["UPSShipmentRequest"];
                string url = baseUrl + path;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var json = JsonConvert.SerializeObject(request);
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<UPSShipmentResponseFinalPayload>(responseResult);
                    result.ResponseResult = responseResult;

                    if (result.Fault == null)
                    {
                        var packageData = result.ShipmentResponse.ShipmentResults.PackageResults.ToString();
                        var packageJson = JToken.Parse(packageData);

                        if (packageJson is JArray)
                        {
                            result.ShipmentResponse.ShipmentResults.FinalPackageResults = packageJson.ToObject<List<UPSPackageResults>>();
                        }

                        if (packageJson is JObject)
                        {
                            var packageObj = packageJson.ToObject<UPSPackageResults>();
                            result.ShipmentResponse.ShipmentResults.FinalPackageResults.Add(packageObj);
                        }
                    }
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

        private UPSShipmentRequestDTO GetShipmentRequest(InternationalShipmentDTO shipmentDto)
        {
            //Default value for Request and LabelSpecification
            var payload = new UPSShipmentRequestDTO
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
            return payment;
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
            shipper.Phone.Extension = phoneExtension;
            shipper.Phone.Number = phoneNumber;
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
            reciever.Address.StateProvinceCode = shipmentDto.ReceiverStateOrProvinceCode;
            reciever.Address.PostalCode = shipmentDto.ReceiverPostalCode;
            reciever.Address.CountryCode = shipmentDto.ReceiverCountryCode.Length <= 2 ? shipmentDto.ReceiverCountryCode : shipmentDto.ReceiverCountryCode.Substring(0, 2);
            reciever.Phone.Number = shipmentDto.ReceiverPhoneNumber;
            reciever.Phone.Extension = string.Empty;
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

                package.Dimensions.UnitOfMeasurement.Code = "CM";
                package.Dimensions.UnitOfMeasurement.Description = "Centimeter";
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
