using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.DHL;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.DHL
{
    public class DHLService : IDHLService
    {
        public async Task<InternationalShipmentWaybillDTO> CreateInternationalShipment(InternationalShipmentDTO shipmentDTO)
        {
            var createShipment = await CreateDHLShipment(shipmentDTO);
            var result = FormatShipmentCreationReponse(createShipment);
            return result;
        }

        public async Task<TotalNetResult> GetInternationalShipmentPrice(InternationalShipmentDTO shipmentDTO)
        {
            var getPrice = await GetDHLPrice(shipmentDTO);
            var formattedPrice = FormatPriceReponse(getPrice);
            return formattedPrice;
        }

        private InternationalShipmentWaybillDTO FormatShipmentCreationReponse(ShipmentResPayload payload)
        {
            var output = new InternationalShipmentWaybillDTO();
            if (payload != null)
            {
                output.OutBoundChannel = CompanyMap.DHL;
                output.ShipmentIdentificationNumber = payload.ShipmentTrackingNumber;
                output.PackageResult = payload.Packages[0].TrackingNumber;
                output.PdfFormat = payload.Documents[0].Content;
            }
            else
            {
                throw new GenericException("There was an issue processing your request.");
            }
            return output;
        }

        private TotalNetResult FormatPriceReponse(RateResPayload response)
        {
            var output = new TotalNetResult();
            if (response.Products.Count > 0)
            {
                output.Amount = (decimal)response.Products[0].TotalPrice[0].Price;
                output.Currency = response.Products[0].TotalPrice[0].PriceCurrency;
            }
            else
            {
                throw new GenericException("There was an issue processing your request.");
            }

            return output;
        }

        private async Task<ShipmentResPayload> CreateDHLShipment(InternationalShipmentDTO shipmentDTO)
        {
            try
            {
                var result = new ShipmentResPayload();

                //Get Price from DHL
                var rateRequest = GetShipmentRequestPayload(shipmentDTO);

                string baseUrl = ConfigurationManager.AppSettings["DHLBaseUrl"];
                string path = ConfigurationManager.AppSettings["DHLShipmentRequest"];
                string url = baseUrl + path;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var byteArray = GetAutorizationKey();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var json = JsonConvert.SerializeObject(rateRequest, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ShipmentResPayload>(responseResult);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ShippingPayload GetShipmentRequestPayload(InternationalShipmentDTO shipmentDTO)
        {
            var shipmentPayload = new ShippingPayload();

            //var next2Day = DateTime.UtcNow.AddDays(2);
            shipmentPayload.PlannedShippingDateAndTime =
            shipmentPayload.PlannedShippingDateAndTime = DateTime.Today.AddDays(2).ToString("yyyy-MM-ddTHH:mm:ss'GMT+01:00'");
            shipmentPayload.Pickup.IsRequested = false;
            shipmentPayload.Accounts.Add(GetAccount());
            shipmentPayload.CustomerDetails.ShipperDetails = GetShipperContact(shipmentDTO);
            shipmentPayload.CustomerDetails.ReceiverDetails = GetReceiverContact(shipmentDTO);
            shipmentPayload.Content = GetShippingContent(shipmentDTO);
            shipmentPayload.OutputImageProperties = GetShipperOutputImageProperty();

            return shipmentPayload;
        }

        private ShippingContent GetShippingContent(InternationalShipmentDTO shipmentDTO)
        {
            var signatureTitle = "Mr.";
            if (shipmentDTO.CustomerDetails.Gender == Gender.Female)
            {
                signatureTitle = "Mrs.";
            }
            var content = new ShippingContent
            {
                IsCustomsDeclarable = true,
                DeclaredValue = shipmentDTO.DeclarationOfValueCheck.Value,
                DeclaredValueCurrency = "NGN",
                Incoterm = "DAP",
                UnitOfMeasurement = "metric",
                ExportDeclaration = new ExportDeclaration
                {
                    DestinationPortName = shipmentDTO.ReceiverCity,
                    Invoice = new ShippingInvoice
                    {
                        Number = "12345-ABC",
                        Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                        SignatureName = shipmentDTO.CustomerDetails.CustomerName,
                        SignatureTitle = signatureTitle
                    }
                }
            };

            var removeFiftyPer = 0.5M * shipmentDTO.GrandTotal;
            var chargeValue = Math.Round((7.5M / 100) * removeFiftyPer, 2);
            var charge = new AdditionalCharge { Value = chargeValue, Caption = "freight" };
            content.ExportDeclaration.AdditionalCharges.Add(charge);

            var license = new License { Value = "license", TypeCode = "export" };
            content.ExportDeclaration.Licenses.Add(license);

            int count = 1;
            foreach (var item in shipmentDTO.ShipmentItems)
            {
                content.Description = item.Description;
                var netValue = (float)item.Length * (float)item.Width * (float)item.Height / 5000;
                var package = new ShippingPackage
                {
                    Weight = (float)item.Weight,
                    Dimensions = new Dimensions
                    {
                        Length = (float)item.Length,
                        Width = (float)item.Width,
                        Height = (float)item.Height,
                    }
                };
                content.Packages.Add(package);
                var lineItem = new LineItem
                {
                    Number = count,
                    Description = shipmentDTO.ItemDetails,
                    Price = shipmentDTO.DeclarationOfValueCheck.Value,
                    PriceCurrency = "NGN",
                    ManufacturerCountry = shipmentDTO.ManufacturerCountry.Trim().Length <= 2 ? shipmentDTO.ManufacturerCountry : shipmentDTO.ManufacturerCountry.Trim().Substring(0, 2),
                    ExportReasonType = "permanent",
                    ExportControlClassificationNumber = "",
                    Quantity = new Quantity
                    {
                        Value = item.Quantity,
                        UnitOfMeasurement = "BOX"
                    },
                    Weight = new ShippingWeight
                    {
                        NetValue = (float)Math.Round(netValue, 2),
                        GrossValue = (float)Math.Round(netValue, 2),
                    }
                };
                content.ExportDeclaration.LineItems.Add(lineItem);
                count++;
            }
            return content;
        }

        private ShipmentReceiverDetail GetReceiverContact(InternationalShipmentDTO shipmentDTO)
        {
            var receiver = new ShipmentReceiverDetail
            {
                ContactInformation = new ContactInformation
                {
                    Email = shipmentDTO.ReceiverEmail,
                    Phone = shipmentDTO.ReceiverPhoneNumber,
                    MobilePhone = shipmentDTO.ReceiverPhoneNumber,
                    CompanyName = shipmentDTO.ReceiverCompanyName,
                    FullName = shipmentDTO.ReceiverName
                },
                PostalAddress = new PostalAddress
                {
                    CityName = shipmentDTO.ReceiverCity,
                    PostalCode = shipmentDTO.ReceiverPostalCode,
                    ProvinceCode = shipmentDTO.ReceiverStateOrProvinceCode,
                    CountryCode = shipmentDTO.ReceiverCountryCode.Length <= 2 ? shipmentDTO.ReceiverCountryCode : shipmentDTO.ReceiverCountryCode.Substring(0, 2),
                    countyName = shipmentDTO.ReceiverCountry,
                    AddressLine1 = shipmentDTO.ReceiverAddress,
                    AddressLine2 = shipmentDTO.ReceiverCity,
                    AddressLine3 = shipmentDTO.ReceiverCity
                }
            };
            return receiver;
        }

        private ShipmentShipperDetail GetShipperContact(InternationalShipmentDTO shipmentDTO)
        {
            string email = ConfigurationManager.AppSettings["DHLGIGContactEmail"];
            string phoneNumber = ConfigurationManager.AppSettings["UPSGIGPhoneNumber"];

            var shipper = new ShipmentShipperDetail
            {
                ContactInformation = new ContactInformation
                {
                    Email = email,
                    Phone = phoneNumber,
                    MobilePhone = phoneNumber,
                    CompanyName = "GIG LOGISTICS",
                    FullName = shipmentDTO.CustomerDetails.CustomerName
                },
                PostalAddress = new PostalAddress
                {
                    CityName = "Lagos",
                    PostalCode = "100001",
                    ProvinceCode = "NG",
                    CountryCode = "NG",
                    countyName = "Nigeria",
                    AddressLine1 = "1 Sunday Ogunyade Street, Gbagada Express Way",
                    AddressLine2 = "Beside Mobile Fuel Station",
                    AddressLine3 = "Gbagada 100234, Lagos",
                }
            };
            return shipper;
        }

        private OutputImageProperties GetShipperOutputImageProperty()
        {
            var output = new OutputImageProperties { AllDocumentsInOneImage = true };
            var image1 = new ImageOption { TypeCode = "label", TemplateName = "ECOM26_84_A4_001" };
            output.ImageOptions.Add(image1);
            var image2 = new ImageOption { TypeCode = "waybillDoc", TemplateName = "ARCH_8X4_A4_002", IsRequested = true };
            output.ImageOptions.Add(image2);
            var image3 = new ImageOption { InvoiceType = "commercial", TypeCode = "invoice", IsRequested = true, LanguageCode = "eng" };
            output.ImageOptions.Add(image3);

            return output;
        }

        private async Task<RateResPayload> GetDHLPrice(InternationalShipmentDTO shipmentDTO)
        {
            try
            {
                var result = new RateResPayload();
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
                    var json = JsonConvert.SerializeObject(rateRequest, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                    }
                    string responseResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<RateResPayload>(responseResult);
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

        private RatePayload GetRateRequestPayload(InternationalShipmentDTO shipmentDTO)
        {
            var rateRequest = new RatePayload();

            rateRequest.PlannedShippingDateAndTime = DateTime.Today.AddDays(2).ToString("yyyy-MM-ddTHH:mm:ss'GMT+01:00'");
            rateRequest.Accounts.Add(GetAccount());
            rateRequest.CustomerDetails.ShipperDetails = GetRateShipperAddress();
            rateRequest.CustomerDetails.ReceiverDetails = GetRateReceiverAddress(shipmentDTO);

            foreach (var item in shipmentDTO.ShipmentItems)
            {
                var package = new RatePackage
                {
                    Weight = (float)item.Weight,
                    Dimensions = new Dimensions
                    {
                        Length = (float)item.Length,
                        Width = (float)item.Width,
                        Height = (float)item.Height
                    }
                };
                rateRequest.Packages.Add(package);
            }
            return rateRequest;
        }

        private RateShipperDetail GetRateShipperAddress()
        {
            //Move this detail to web-config for now
            var address = new RateShipperDetail
            {
                CityName = "Lagos",
                PostalCode = "100001",
                CountryCode = "NG",
                CountyName = "Lagos",
                AddressLine1 = "address1",
                AddressLine2 = "address2",
                AddressLine3 = "address3"
            };
            return address;
        }

        private RateReceiverDetail GetRateReceiverAddress(InternationalShipmentDTO shipmentDTO)
        {
            var address = new RateReceiverDetail
            {
                CityName = shipmentDTO.ReceiverCity,
                PostalCode = shipmentDTO.ReceiverPostalCode,
                CountryCode = shipmentDTO.ReceiverCountryCode.Length <= 2 ? shipmentDTO.ReceiverCountryCode : shipmentDTO.ReceiverCountryCode.Substring(0, 2),
                CountyName = shipmentDTO.ReceiverCountry,
                ProvinceCode = shipmentDTO.ReceiverStateOrProvinceCode,
                AddressLine1 = shipmentDTO.ReceiverAddress,
                AddressLine2 = "address2",
                AddressLine3 = "address3"
            };

            return address;
        }

        private Account GetAccount()
        {
            var number = ConfigurationManager.AppSettings["DHLAccount"];
            var account = new Account()
            {
                TypeCode = "shipper",
                Number = number
            };
            return account;
        }

    }
}
