using DocumentFormat.OpenXml.Vml.Spreadsheet;
using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.DHL.Enum;
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

        private async Task<RateRequestResponse> CreateDHLShipment(InternationalShipmentDTO shipmentDTO)
        {
            try
            {
                var result = new RateRequestResponse();

                //Get Price from DHL
                var rateRequest = GetShipmentRequestPayload(shipmentDTO);

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
                    result = JsonConvert.DeserializeObject<RateRequestResponse>(responseResult);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ShipmentRequestedPayload GetShipmentRequestPayload(InternationalShipmentDTO shipmentDTO)
        {
            var shipmentPayload = new ShipmentRequestedPayload();
            var shipmentInfo = GetShipmentInfo(shipmentDTO);
            var internationalDetail = GetInternationalDetail(shipmentDTO);
            var packages = GetPackages(shipmentDTO);
            string timeStamp = GetShipTimeStamp();
            var shipperContact = GetShipperContact();
            var receiverContact = GetReceiverContact(shipmentDTO);

            var ship = new ShipDTO
            {
                Shipper = shipperContact,
                Recipient =receiverContact
            };

            shipmentPayload.ShipmentRequest.RequestedShipment.ShipmentInfo = shipmentInfo;
            shipmentPayload.ShipmentRequest.RequestedShipment.InternationalDetail = internationalDetail;
            shipmentPayload.ShipmentRequest.RequestedShipment.Ship = ship;
            shipmentPayload.ShipmentRequest.RequestedShipment.Packages = packages;
            shipmentPayload.ShipmentRequest.RequestedShipment.ShipTimestamp = timeStamp;
            return shipmentPayload;
        }

        private string GetShipTimeStamp()
        {
            var next2Day = DateTime.Now.AddDays(2);
            string timeStamp = next2Day.ToString("yyyy-MM-ddTHH:mm:ss'GMT+01:00'");
            return timeStamp;
        }

        private PackagesDTO GetPackages(InternationalShipmentDTO shipmentDTO)
        {
            PackagesDTO packages = new PackagesDTO
            {
                RequestedPackages = GetItemPackages(shipmentDTO)
            };
            return packages;
        }
            
        private List<RequestedPackagesDTO> GetItemPackages(InternationalShipmentDTO shipmentDTO)
        {
            var items = new List<RequestedPackagesDTO>();

            int count = 1;
           foreach(var item in shipmentDTO.ShipmentItems)
            {
                RequestedPackagesDTO requestedPackagesDTO = new RequestedPackagesDTO();
                requestedPackagesDTO.CustomerReferences = item.Description_s;
                requestedPackagesDTO.number = count;
                requestedPackagesDTO.Weight = (decimal)item.Weight;

                Dimensions dimensions = new Dimensions
                {
                    Height = (decimal)item.Height,
                    Width = (decimal)item.Width,
                    Length = (decimal)item.Length
                };

                requestedPackagesDTO.Dimensions = dimensions;
                items.Add(requestedPackagesDTO);      
                count++;
            }
            return items;
        }

        private InternationalDetailDTO GetInternationalDetail(InternationalShipmentDTO shipmentDTO)
        {
            var commodities = GetCommodities(shipmentDTO);
            var internationalDetail = new InternationalDetailDTO
            {
                Commodities = commodities,
                Content = shipmentDTO.Content
            };
            return internationalDetail;
        }

        private ShipmentInfoDTO GetShipmentInfo(InternationalShipmentDTO shipmentDTO)
        {
            ShipmentInfoDTO shipmentInfo = new ShipmentInfoDTO
            {
                DropOffType = DropOffType.REGULAR_PICKUP.ToString(),
                UnitOfMeasurement = UnitOfMeasurement.SI.ToString()
            };
            return shipmentInfo;
        }

        private Commodity GetCommodities(InternationalShipmentDTO shipmentDTO)
        {
            var commodity = new Commodity
            {
                Description = shipmentDTO.Description,
                NumberOfPieces = shipmentDTO.ShipmentItems.Count,
                Quantity = (int)shipmentDTO.ApproximateItemsWeight,
                CustomsValue = (int)shipmentDTO.DeclarationOfValueCheck
            };
            return commodity;
        }

        private Details GetReceiverContact(InternationalShipmentDTO shipmentDTO)
        {
            throw new NotImplementedException();
        }

        private Details GetShipperContact()
        {
            Details shipper = new Details
            {
                Contact = new Contact
                {
                    PersonName = "KME HEERBAL",
                    CompanyName = "GIG LOGISTICS",
                    PhoneNumber = "2349057138606",
                    EmailAddress = "azeez.oladejo@giglogistics.com"
                },
                Address = new AddressPayload
                {
                    StreetLines = "GBAGADA PHASE 2",
                    City = "ILUPEJU ",
                    PostalCode = "100001",
                    CountryCode = "NG"
                }
            };
            return shipper;
        }

        private async Task<RateRequestResponse> GetDHLPrice(InternationalShipmentDTO shipmentDTO)
        {
            try
            {
                var result = new RateRequestResponse();

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
                    result = JsonConvert.DeserializeObject<RateRequestResponse>(responseResult);
                }

                return result;
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
            string accessKey = username + ":" + secretKey;
            var key = Encoding.ASCII.GetBytes(accessKey);
            return key;
        }

        private RateRequestPayload GetRateRequestPayload(InternationalShipmentDTO shipmentDTO)
        {
            var rateRequest = new RateRequest();
            string dhlAccount = ConfigurationManager.AppSettings["DHLAccount"];
            rateRequest.RequestedShipment.Account = dhlAccount;

            rateRequest.RequestedShipment.DropOffType = DropOffType.REGULAR_PICKUP.ToString();
            rateRequest.RequestedShipment.ShipTimestamp = DateTime.Now.AddDays(2);
            rateRequest.RequestedShipment.UnitOfMeasurement = UnitOfMeasurement.SI.ToString();
            rateRequest.RequestedShipment.Content = shipmentDTO.Content;

            //Shipper address -- GIGL default address
            var address = GetShipperAddress();
            rateRequest.RequestedShipment.Ship.Shipper.City = address.City;
            rateRequest.RequestedShipment.Ship.Shipper.PostalCode = address.PostalCode;
            rateRequest.RequestedShipment.Ship.Shipper.CountryCode = address.CountryCode;

            //Receiver Address
            rateRequest.RequestedShipment.Ship.Recipient.City = shipmentDTO.ReceiverCity;
            rateRequest.RequestedShipment.Ship.Recipient.PostalCode = shipmentDTO.RecieverPostalCode;
            rateRequest.RequestedShipment.Ship.Recipient.CountryCode = shipmentDTO.ReceiverCode;

            int count = 1;
            foreach (var item in shipmentDTO.ShipmentItems)
            {
                var package = new RequestedPackages
                {
                    number = count
                };
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
            //Move this detail to web-config for now
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
