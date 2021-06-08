using GIGLS.Core.DTO.DHL;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.DHL;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Configuration;
using System.Linq;
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
            if (payload.Documents.Count > 0 || !string.IsNullOrWhiteSpace(payload.ShipmentTrackingNumber))
            {
                output.OutBoundChannel = CompanyMap.DHL;
                output.ShipmentIdentificationNumber = payload.ShipmentTrackingNumber;
                output.PackageResult = payload.Packages[0].TrackingNumber;
                output.PdfFormat = payload.Documents[0].Content;
            }
            else
            {
                //Log.Error($"FORMAT SHIPMENT CREATION RESPONSE: {payload.ErrorReason}");
                throw new GenericException($"{payload.ErrorReason}");
            }
            return output;
        }

        private TotalNetResult FormatPriceReponse(RateResPayload response)
        {
            var output = new TotalNetResult();
            if (response.Products.Count > 0)
            {
                output.Amount = (decimal)response.Products[0].TotalPrice[0].Price;
                output.InternationalShippingCost = (decimal)response.Products[0].TotalPrice[0].Price;
                output.Currency = response.Products[0].TotalPrice[0].PriceCurrency;
                var insurance = response.Products[0].DetailedPriceBreakdown[0].Breakdown.FirstOrDefault(x => x.Name == "SHIPMENT INSURANCE");
                output.Insurance = insurance != null ? insurance.Price : 0.00M;
            }
            else
            {
                //Log.Error($"FORMAT PRICE RESPONSE: {response.ErrorReason}");
                throw new GenericException($"{response.ErrorReason}");
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
                    //Log.Information($"DHL SHIPMENT PAYLOAD: {json}");
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        var dHLError = JsonConvert.DeserializeObject<DHLErrorFeedBack>(responseResult);
                        result.ErrorReason = dHLError.Detail;
                        //Log.Error($"DHL SHIPMENT ERROR RESPONSE: {dHLError}");

                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<ShipmentResPayload>(responseResult);
                        //Log.Information($"DHL SHIPMENT RESPONSE: {result}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                //Log.Error($"DHL SHIPMENT: {ex.Message} {ex}");
                throw;
            }
        }

        private ShippingPayload GetShipmentRequestPayload(InternationalShipmentDTO shipmentDTO)
        {
            var shipmentPayload = new ShippingPayload();

            shipmentPayload.PlannedShippingDateAndTime = AddWorkdays().ToString("yyyy-MM-ddTHH:mm:ss'GMT+01:00'");
            shipmentPayload.Pickup.IsRequested = false;
            shipmentPayload.Accounts.Add(GetAccount());
            shipmentPayload.CustomerDetails.ShipperDetails = GetShipperContact(shipmentDTO);
            shipmentPayload.CustomerDetails.ReceiverDetails = GetReceiverContact(shipmentDTO);
            shipmentPayload.Content = GetShippingContent(shipmentDTO);
            shipmentPayload.OutputImageProperties = GetShipperOutputImageProperty();
            shipmentPayload.ProductCode = shipmentPayload.Content.TempContentType != null ? "D" : "P";
            shipmentPayload.Content.TempContentType = null;
            if (shipmentDTO.CustomerDetails.Rank == Rank.Class && shipmentDTO.DeclarationOfValueCheck >= 100000.00M)
            {
                // for class customers, whose declared value is above or equal to 100k
                shipmentPayload.ValueAddedServices.Add(GetValueAddedService(shipmentDTO.DeclarationOfValueCheck.Value));
            }
            else if (shipmentDTO.DeclarationOfValueCheck >= 30000.00M)
            {
                // for class customers, whose declared value is above or equal to 30k
                shipmentPayload.ValueAddedServices.Add(GetValueAddedService(shipmentDTO.DeclarationOfValueCheck.Value));
            } 
            return shipmentPayload;
        }

        private ShippingContent GetShippingContent(InternationalShipmentDTO shipmentDTO)
        {
            var content = new ShippingContent();
            var lineItem = new LineItem();
            var weight = new ShippingWeight();

            var signatureTitle = "Mr.";
            if (shipmentDTO.CustomerDetails.Gender == Gender.Female)
            {
                signatureTitle = "Mrs.";
            }
            content = new ShippingContent
            {
                DeclaredValue = shipmentDTO.DeclarationOfValueCheck.Value,
                DeclaredValueCurrency = "NGN",
                Incoterm = "DAP",
                UnitOfMeasurement = "metric",
                ExportDeclaration = new ExportDeclaration
                {
                    DestinationPortName = shipmentDTO.ReceiverCity,
                    Invoice = new ShippingInvoice
                    {
                        Number = "4010097858",
                        Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                        SignatureName = shipmentDTO.CustomerDetails.CustomerName,
                        SignatureTitle = signatureTitle
                    }
                }
            };

            var chargeValue = Math.Round((7.5M / 100) * shipmentDTO.InternationalShippingCost, 2);
            var charge = new AdditionalCharge { Value = chargeValue, Caption = "freight", TypeCode = "freight" };
            content.ExportDeclaration.AdditionalCharges.Add(charge);

            var license = new License { Value = "license", TypeCode = "export" };
            content.ExportDeclaration.Licenses.Add(license);

            int count = 1;
            foreach (var item in shipmentDTO.ShipmentItems)
            {
                var package = new ShippingPackage();
                content.Description = item.Description;
                if (item.InternationalShipmentItemCategory == InternationalShipmentItemCategory.Document)
                {
                    content.TempContentType = "Document";
                    content.IsCustomsDeclarable = false;
                    package = new ShippingPackage
                    {
                        Weight = 1.0F,
                        Dimensions = new Dimensions
                        {
                            Length = 1,
                            Width = 1,
                            Height = 1
                        }
                    };

                    weight = new ShippingWeight
                    {
                        NetValue = 0.1F,
                        GrossValue = 0.1F,
                    };
                }
                else
                {
                    var netValue = (float)item.Length * (float)item.Width * (float)item.Height / 5000;
                    content.IsCustomsDeclarable = true;
                    package = new ShippingPackage
                    {
                        Weight = (float)item.Weight,
                        Dimensions = new Dimensions
                        {
                            Length = (int)Math.Round(item.Length, 0),
                            Width = (int)Math.Round(item.Width, 0),
                            Height = (int)Math.Round(item.Height, 0)
                        }
                    };

                    weight = new ShippingWeight
                    {
                        NetValue = (float)Math.Round(netValue, 2),
                        GrossValue = (float)Math.Round(netValue, 2),
                    };
                }
                content.Packages.Add(package);
                lineItem = new LineItem
                {
                    Number = count,
                    Description = shipmentDTO.ItemDetails,
                    Price = shipmentDTO.DeclarationOfValueCheck.Value,
                    PriceCurrency = "NGN",
                    ManufacturerCountry = shipmentDTO.ManufacturerCountry.Trim().Length <= 2 ? shipmentDTO.ManufacturerCountry.Trim() : shipmentDTO.ManufacturerCountry.Trim().Substring(0, 2),
                    ExportReasonType = "permanent",
                    ExportControlClassificationNumber = "",
                    Quantity = new Quantity
                    {
                        Value = item.Quantity,
                        UnitOfMeasurement = "BOX"
                    }
                };
                lineItem.Weight = weight;
                content.ExportDeclaration.LineItems.Add(lineItem);
                count++;
            }
            return content;
        }

        private ShipmentReceiverDetail GetReceiverContact(InternationalShipmentDTO shipmentDTO)
        {
            var splittedAddress = shipmentDTO.ReceiverAddress.Split(',');
            var postalAddress = new PostalAddress();

            var receiver = new ShipmentReceiverDetail
            {
                ContactInformation = new ContactInformation
                {
                    Email = shipmentDTO.ReceiverEmail,
                    Phone = shipmentDTO.ReceiverPhoneNumber,
                    MobilePhone = shipmentDTO.ReceiverPhoneNumber,
                    CompanyName = shipmentDTO.ReceiverCompanyName,
                    FullName = shipmentDTO.ReceiverName
                }
            };

            postalAddress.CityName = shipmentDTO.ReceiverCity;
            postalAddress.PostalCode = shipmentDTO.ReceiverPostalCode != null ? shipmentDTO.ReceiverPostalCode : string.Empty;
            postalAddress.ProvinceCode = shipmentDTO.ReceiverStateOrProvinceCode;
            postalAddress.CountryCode = shipmentDTO.ReceiverCountryCode.Trim().Length <= 2 ? shipmentDTO.ReceiverCountryCode.Trim() : shipmentDTO.ReceiverCountryCode.Trim().Substring(0, 2);
            postalAddress.countyName = shipmentDTO.ReceiverCountry;
            if (splittedAddress.Length <= 1)
            {
                postalAddress.AddressLine1 = splittedAddress[0].Length <= 5 ? splittedAddress[0] + splittedAddress[1] : splittedAddress[0];
                postalAddress.AddressLine2 = shipmentDTO.ReceiverCountry;
                postalAddress.AddressLine3 = shipmentDTO.ReceiverCountry;
            }
            else
            {
                postalAddress.AddressLine1 = splittedAddress[0].Length <= 5 ? splittedAddress[0] + splittedAddress[1] : splittedAddress[0];
                postalAddress.AddressLine2 = splittedAddress[1] == null ? shipmentDTO.ReceiverCity : splittedAddress[1];
                postalAddress.AddressLine3 = shipmentDTO.ReceiverCountry;
            }

            receiver.PostalAddress = postalAddress;
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
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    //Log.Information($"DHL RATE REQUEST PAYLOAD: {json}");
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, data);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var dHLError = JsonConvert.DeserializeObject<DHLErrorFeedBack>(responseResult);
                        result.ErrorReason = dHLError.Detail;
                        //Log.Error($"DHL RATE ERROR RESPONSE: {dHLError}");
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<RateResPayload>(responseResult);
                        //  Log.Information($"DHL RATE RESPONSE: {result}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                //Log.Error($"DHL RATE: {ex.Message} {ex}");
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

            rateRequest.PlannedShippingDateAndTime = AddWorkdays().ToString("yyyy-MM-ddTHH:mm:ss'GMT+01:00'");
            rateRequest.Accounts.Add(GetAccount());
            rateRequest.CustomerDetails.ShipperDetails = GetRateShipperAddress();
            rateRequest.CustomerDetails.ReceiverDetails = GetRateReceiverAddress(shipmentDTO);
            if (shipmentDTO.CustomerDetails.Rank == Rank.Class && shipmentDTO.DeclarationOfValueCheck >= 100000.00M)
            {
                // for class customers, whose declared value is above or equal to 100k
                rateRequest.ValueAddedServices.Add(GetValueAddedService(shipmentDTO.DeclarationOfValueCheck.Value));
                rateRequest.MonetaryAmount.Add(GetMonetaryAmount(shipmentDTO.DeclarationOfValueCheck.Value));
            }
            else if (shipmentDTO.DeclarationOfValueCheck >= 30000.00M)
            {
                // for class customers, whose declared value is above or equal to 30k
                rateRequest.ValueAddedServices.Add(GetValueAddedService(shipmentDTO.DeclarationOfValueCheck.Value));
                rateRequest.MonetaryAmount.Add(GetMonetaryAmount(shipmentDTO.DeclarationOfValueCheck.Value));
            }
           
            foreach (var item in shipmentDTO.ShipmentItems)
            {
                var package = new RatePackage();
                if (item.InternationalShipmentItemCategory == InternationalShipmentItemCategory.Document)
                {
                    rateRequest.ProductCode = "D";
                    rateRequest.LocalProductCode = "D";
                    rateRequest.IsCustomsDeclarable = false;
                    package = new RatePackage
                    {
                        Weight = 1.0F,
                        Dimensions = new Dimensions
                        {
                            Length = 1,
                            Width = 1,
                            Height = 1
                        }
                    };
                }
                else
                {
                    package = new RatePackage
                    {
                        Weight = (float)item.Weight,
                        Dimensions = new Dimensions
                        {
                            Length = (int)Math.Round(item.Length, 0),
                            Width = (int)Math.Round(item.Width, 0),
                            Height = (int)Math.Round(item.Height, 0)
                        }
                    };
                }
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
                CountyName = "Nigeria",
                AddressLine1 = "1 Sunday Ogunyade Street, Gbagada Express Way",
                AddressLine2 = "Beside Mobile Fuel Station",
                AddressLine3 = "Gbagada 100234, Lagos"
            };
            return address;
        }

        private RateReceiverDetail GetRateReceiverAddress(InternationalShipmentDTO shipmentDTO)
        {
            var splittedAddress = shipmentDTO.ReceiverAddress.Split(',');
            var address = new RateReceiverDetail();

            address.CityName = shipmentDTO.ReceiverCity;
            address.PostalCode = shipmentDTO.ReceiverPostalCode != null ? shipmentDTO.ReceiverPostalCode : string.Empty;
            address.CountryCode = shipmentDTO.ReceiverCountryCode.Trim().Length <= 2 ? shipmentDTO.ReceiverCountryCode.Trim() : shipmentDTO.ReceiverCountryCode.Trim().Substring(0, 2);
            address.CountyName = shipmentDTO.ReceiverCountry;
            address.ProvinceCode = shipmentDTO.ReceiverStateOrProvinceCode;
            if (splittedAddress.Length <= 1)
            {
                address.AddressLine1 = splittedAddress[0].Length <= 5 ? splittedAddress[0] + splittedAddress[1] : splittedAddress[0];
                address.AddressLine2 = shipmentDTO.ReceiverCity;
                address.AddressLine3 = shipmentDTO.ReceiverCity;
            }
            else
            {
                address.AddressLine1 = splittedAddress[0].Length <= 5 ? splittedAddress[0] + splittedAddress[1] : splittedAddress[0];
                address.AddressLine2 = splittedAddress[1] == null ? shipmentDTO.ReceiverCity : splittedAddress[1];
                address.AddressLine3 = shipmentDTO.ReceiverCity;
            }
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

        public async Task<InternationalShipmentTracking> TrackInternationalShipment(string internationalWaybill)
        {
            try
            {
                var result = new InternationalShipmentTracking();
                string baseUrl = ConfigurationManager.AppSettings["DHLBaseUrl"];
                string path = ConfigurationManager.AppSettings["DHLShipmentRequest"];
                string url = $"{baseUrl}{path}/{internationalWaybill}/tracking";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var byteArray = GetAutorizationKey();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    HttpResponseMessage response = await client.GetAsync(url);
                    string responseResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<InternationalShipmentTracking>(responseResult);
                    //Log.Information($"DHL TRACKING RESPONSE: {result}");
                }
                return result;
            }
            catch (Exception ex)
            {
                //Log.Error($"DHL TRACKING: {ex.Message} {ex}");
                throw;
            }
        }

        private DateTime AddWorkdays()
        {
            int nonWorkDays = 2;
            var tmpDate = DateTime.Today;
            while (nonWorkDays > 0)
            {
                tmpDate = tmpDate.AddDays(1);
                if (tmpDate.DayOfWeek < DayOfWeek.Saturday && tmpDate.DayOfWeek > DayOfWeek.Sunday)
                    nonWorkDays--;
            }
            return tmpDate;
        }

        private ValueAddedService GetValueAddedService(decimal declareValue)
        {
            return new ValueAddedService
            {
                ServiceCode = "II",
                Value = declareValue,
                Currency = "NGN",
                Method = "cash"
            };
        }

        private MonetaryAmount GetMonetaryAmount(decimal declareValue)
        {
            return new MonetaryAmount
            {
                TypeCode = "declaredValue",
                Value = declareValue,
                Currency = "NGN"
            };
        }

    }
}
