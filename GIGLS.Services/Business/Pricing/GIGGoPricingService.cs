using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Zone;
using GIGLS.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Pricing
{
    public class GIGGoPricingService : IGIGGoPricingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IDomesticRouteZoneMapService _domesticroutezonemapservice;
        private readonly IUserService _userService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ISpecialDomesticZonePriceService _specialDomesticZonePriceService;
        private readonly IDeliveryOptionPriceService _deliveryOptionPriceService;
        private readonly IDomesticZonePriceService _regular;
        private readonly IWeightLimitPriceService _weightLimitPrice;
        private readonly IWeightLimitService _weightLimit;

        public GIGGoPricingService(IUnitOfWork uow, IDomesticRouteZoneMapService domesticroutezonemapservice, IUserService userService,
            IGlobalPropertyService globalPropertyService,
            ISpecialDomesticZonePriceService specialDomesticZonePriceService, IDeliveryOptionPriceService deliveryOptionPriceService,
            IDomesticZonePriceService regular, IWeightLimitPriceService weightLimitPrice, IWeightLimitService weightLimit)
        {
            _uow = uow;
            _domesticroutezonemapservice = domesticroutezonemapservice;
            _userService = userService;
            _globalPropertyService = globalPropertyService;
            _specialDomesticZonePriceService = specialDomesticZonePriceService;
            _deliveryOptionPriceService = deliveryOptionPriceService;
            _regular = regular;
            _weightLimitPrice = weightLimitPrice;
            _weightLimit = weightLimit;
            MapperConfig.Initialize();
        }

        public async Task<MobilePriceDTO> GetGIGGOPrice(PreShipmentMobileDTO preShipment)
        {
            if (!preShipment.PreShipmentItems.Any())
            {
                throw new GenericException($"Shipment Items cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
            }

            var zoneid = await _domesticroutezonemapservice.GetZoneMobile(preShipment.SenderStationId, preShipment.ReceiverStationId);
            preShipment.ZoneMapping = zoneid.ZoneId;

            if (string.IsNullOrEmpty(preShipment.VehicleType))
            {
                return await GetPrice(preShipment);
            }

            if (preShipment.VehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower() && preShipment.ZoneMapping == 1
                && preShipment.SenderLocation.Latitude != null && preShipment.SenderLocation.Longitude != null
                && preShipment.ReceiverLocation.Latitude != null && preShipment.ReceiverLocation.Longitude != null)
            {
                return await GetPriceForBike(preShipment);
            }
            else
            {
                return await GetPrice(preShipment);
            }
        }

        public async Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
            try
            {
                if (preShipment == null)
                {
                    throw new GenericException("Invalid Data");
                }

                if (!preShipment.PreShipmentItems.Any())
                {
                    throw new GenericException("Preshipment Item Not Found");
                }
                if (preShipment.IsFromAgility)
                {
                    var user = await _uow.User.GetUserUsingCustomerForCustomerPortal(preShipment.CustomerCode);
                    if(user != null)
                    {
                        preShipment.UserId = user.Id;
                    }
                }
                else
                {
                    var userId = await _userService.GetCurrentUserId();
                    preShipment.UserId = userId;
                }

                var zoneid = await _domesticroutezonemapservice.GetZoneMobile(preShipment.SenderStationId, preShipment.ReceiverStationId);

                var country = await _uow.Country.GetCountryByStationId(preShipment.SenderStationId);
                if (country == null)
                {
                    throw new GenericException("Sender Station Country Not Found", $"{(int)HttpStatusCode.NotFound}");
                }
                preShipment.CountryId = country.CountryId;

                //change the quantity of the preshipmentItem if it fall into promo category
                preShipment = await ChangePreshipmentItemQuantity(preShipment, zoneid.ZoneId);

                var price = 0.0M;
                var amount = 0.0M;
                var individualPrice = 0.0M;
                decimal declaredValue = 0.0M;

                //undo comment when App is updated
                if (zoneid.ZoneId == 1 && preShipment.ReceiverLocation != null && preShipment.SenderLocation != null)
                {
                    if (preShipment.ReceiverLocation.Latitude != null && preShipment.SenderLocation.Latitude != null)
                    {
                        int ShipmentCount = preShipment.PreShipmentItems.Count;

                        amount = await CalculateGeoDetailsBasedonLocation(preShipment);
                        individualPrice = (amount / ShipmentCount);
                    }
                }

                //Get the customer Type
                if (preShipment.IsFromAgility)
                {
                    var userChannel = await _uow.Company.GetAsync(x => x.CustomerCode == preShipment.CustomerCode);
                    if (userChannel != null)
                    {
                        preShipment.Shipmentype = ShipmentType.Ecommerce;
                    }
                }
                else
                {
                    var userChannelCode = await _userService.GetUserChannelCode();
                    var userChannel = await _uow.Company.GetAsync(x => x.CustomerCode == userChannelCode);
                    if (userChannel != null)
                    {
                        preShipment.Shipmentype = ShipmentType.Ecommerce;
                    }
                }

                //Get the vat value from VAT
                var vatDTO = await _uow.VAT.GetAsync(x => x.CountryId == preShipment.CountryId);
                decimal vat = (vatDTO != null) ? (vatDTO.Value / 100) : (7.5M / 100);

                foreach (var preShipmentItem in preShipment.PreShipmentItems)
                {
                    if (preShipmentItem.Quantity == 0)
                    {
                        throw new GenericException("Item Quantity cannot be zero");
                    }

                    if (preShipmentItem.SpecialPackageId == null)
                    {
                        preShipmentItem.SpecialPackageId = 0;
                    }

                    var priceDTO = new PricingDTO
                    {
                        DepartureStationId = preShipment.SenderStationId,
                        DestinationStationId = preShipment.ReceiverStationId,
                        Weight = preShipmentItem.Weight,
                        SpecialPackageId = (int)preShipmentItem.SpecialPackageId,
                        ShipmentType = preShipmentItem.ShipmentType,
                        CountryId = preShipment.CountryId  //Nigeria
                    };

                    if (preShipmentItem.ShipmentType == ShipmentType.Special)
                    {
                        if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                        {
                            priceDTO.DeliveryOptionId = 4;
                        }

                        preShipmentItem.CalculatedPrice = await GetMobileSpecialPrice(priceDTO);
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                        preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + individualPrice;
                    }
                    else if (preShipmentItem.ShipmentType == ShipmentType.Regular)
                    {
                        if (preShipmentItem.Weight == 0)
                        {
                            throw new GenericException("Item weight cannot be zero");
                        }

                        if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                        {
                            preShipmentItem.CalculatedPrice = await GetMobileEcommercePrice(priceDTO);
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + individualPrice;
                        }
                        else
                        {
                            preShipmentItem.CalculatedPrice = await GetMobileRegularPrice(priceDTO);
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice * preShipmentItem.Quantity;
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + individualPrice;
                        }
                    }

                    var vatForPreshipment = (preShipmentItem.CalculatedPrice * vat);

                    if (!string.IsNullOrWhiteSpace(preShipmentItem.Value))
                    {
                        declaredValue += Convert.ToDecimal(preShipmentItem.Value);
                        var DeclaredValueForPreShipment = Convert.ToDecimal(preShipmentItem.Value);
                        if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                        {
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + (DeclaredValueForPreShipment * 0.01M);
                        }
                        else
                        {
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + (DeclaredValueForPreShipment * 0.01M) + vatForPreshipment;
                        }
                        preShipment.IsdeclaredVal = true;
                    }
                    else
                    {
                        if (preShipment.Shipmentype != ShipmentType.Ecommerce)
                        {
                            preShipmentItem.CalculatedPrice = preShipmentItem.CalculatedPrice + vatForPreshipment;
                        }
                    }

                    preShipmentItem.CalculatedPrice = (decimal)Math.Round((double)preShipmentItem.CalculatedPrice);
                    price += (decimal)preShipmentItem.CalculatedPrice;
                };

                var DiscountPercent = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DiscountPercentage, preShipment.CountryId);
                var Percentage = Convert.ToDecimal(DiscountPercent.Value);
                var PercentageTobeUsed = ((100M - Percentage) / 100M);
                decimal EstimatedDeclaredPrice = preShipment.IsFromAgility ? Convert.ToDecimal(preShipment.Value) : Convert.ToDecimal(declaredValue);
                preShipment.DeliveryPrice = price * PercentageTobeUsed;
                preShipment.InsuranceValue = (EstimatedDeclaredPrice * 0.01M);
                preShipment.CalculatedTotal = (double)(price);
                preShipment.CalculatedTotal = Math.Round((double)preShipment.CalculatedTotal);
                preShipment.Value = declaredValue;
                var discount = Math.Round(price - (decimal)preShipment.DeliveryPrice);
                preShipment.DiscountValue = discount;

                var Pickuprice = await GetPickUpPrice(preShipment.VehicleType, preShipment.CountryId, preShipment.UserId);
                var PickupValue = Convert.ToDecimal(Pickuprice);

                var IsWithinProcessingTime = await WithinProcessingTime(preShipment.CountryId);

                decimal grandTotal = (decimal)preShipment.DeliveryPrice + PickupValue;

                //GIG Go Promo Price
                var gigGoPromo = await CalculatePromoPrice(preShipment, zoneid.ZoneId, PickupValue);
                if (gigGoPromo.GrandTotal > 0)
                {
                    grandTotal = (decimal)gigGoPromo.GrandTotal;
                    discount = (decimal)gigGoPromo.Discount;
                    preShipment.DiscountValue = discount;
                    preShipment.GrandTotal = grandTotal;
                }

                var returnprice = new MobilePriceDTO()
                {
                    MainCharge = (decimal)preShipment.CalculatedTotal,
                    DeliveryPrice = preShipment.DeliveryPrice,
                    PickUpCharge = PickupValue,
                    InsuranceValue = preShipment.InsuranceValue,
                    GrandTotal = grandTotal,
                    PreshipmentMobile = preShipment,
                    CurrencySymbol = country.CurrencySymbol,
                    CurrencyCode = country.CurrencyCode,
                    IsWithinProcessingTime = IsWithinProcessingTime,
                    Discount = discount
                };
                return returnprice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Price for Bike Shipments within a city
        public async Task<MobilePriceDTO> GetPriceForBike(PreShipmentMobileDTO preShipment)
        {
            try
            {
                if (preShipment == null)
                {
                    throw new GenericException("Invalid Data");
                }

                if (!preShipment.PreShipmentItems.Any())
                {
                    throw new GenericException($"Shipment Items cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
                }

                var zoneid = preShipment.ZoneMapping != null ? (int)preShipment.ZoneMapping : 0;

                var country = await _uow.Country.GetCountryByStationId(preShipment.SenderStationId);
                if (country == null)
                {
                    throw new GenericException("Sender Station Country Not Found", $"{(int)HttpStatusCode.NotFound}");
                }
                preShipment.CountryId = country.CountryId;

                var basePriceBike = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikeBasePrice, preShipment.CountryId);
                var basePriceBikeValue = Convert.ToDecimal(basePriceBike.Value);

                //change the quantity of the preshipmentItem if it fall into promo category
                preShipment = await ChangePreshipmentItemQuantity(preShipment, zoneid);

                decimal discount = 0.0M;
                var amount = await CalculateBikePriceBasedonLocation(preShipment);

                decimal pickuprice = 0.0M;  //await GetPickUpPrice(preShipment.VehicleType, preShipment.CountryId, preShipment.UserId);
                decimal pickupValue = 0.0M; // Convert.ToDecimal(pickuprice);

                decimal mainCharge = basePriceBikeValue + amount;

                decimal percentage = 0.0M;

                //Get the customer Types
                if (preShipment.IsFromAgility)
                {
                    var userChannel = await _uow.Company.GetAsync(x => x.CustomerCode == preShipment.CustomerCode);

                    if (userChannel != null)
                    {
                        if (userChannel.CompanyType == CompanyType.Ecommerce)
                        {
                            preShipment.Shipmentype = ShipmentType.Ecommerce;
                        }
                    }
                }
                else
                {
                    preShipment.Shipmentype = await GetEcommerceCustomerShipmentType(preShipment.Shipmentype);
                }

                if (preShipment.Shipmentype == ShipmentType.Ecommerce)
                {
                    var discountPercent = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceGIGGOIntraStateBikeDiscount, preShipment.CountryId);
                    percentage = Convert.ToDecimal(discountPercent.Value);
                }
                else
                {
                    var discountPercent = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.DiscountBikePercentage, preShipment.CountryId);
                    percentage = Convert.ToDecimal(discountPercent.Value);
                }

                var percentageTobeUsed = ((100M - percentage) / 100M);

                var calculatedTotal = (double)(mainCharge * percentageTobeUsed);
                calculatedTotal = Math.Round(calculatedTotal);
                preShipment.DeliveryPrice = (decimal)calculatedTotal;

                discount = Math.Round(mainCharge - (decimal)calculatedTotal);
                decimal grandTotal = (decimal)calculatedTotal + pickupValue;

                //GIG Go Promo Price
                var gigGoPromo = await CalculatePromoPrice(preShipment, zoneid, pickupValue);
                if (gigGoPromo.GrandTotal > 0)
                {
                    grandTotal = (decimal)gigGoPromo.GrandTotal;
                    discount = (decimal)gigGoPromo.Discount;
                    preShipment.DiscountValue = discount;
                    preShipment.GrandTotal = grandTotal;
                }

                var countOfItems = preShipment.PreShipmentItems.Count();
                var individualPrice = grandTotal / countOfItems;

                foreach (var preShipmentItem in preShipment.PreShipmentItems)
                {
                    preShipmentItem.CalculatedPrice = individualPrice;
                };

                var IsWithinProcessingTime = await WithinProcessingTime(preShipment.CountryId);
                var returnprice = new MobilePriceDTO()
                {
                    MainCharge = mainCharge,
                    DeliveryPrice = preShipment.DeliveryPrice,
                    Vat = 0.0M,
                    PickUpCharge = pickuprice,
                    InsuranceValue = 0.0M,
                    GrandTotal = grandTotal,
                    PreshipmentMobile = preShipment,
                    CurrencySymbol = country.CurrencySymbol,
                    CurrencyCode = country.CurrencyCode,
                    IsWithinProcessingTime = IsWithinProcessingTime,
                    Discount = discount
                };
                return returnprice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<PreShipmentMobileDTO> ChangePreshipmentItemQuantity(PreShipmentMobileDTO preShipmentDTO, int zoneId)
        {
            if (!string.IsNullOrWhiteSpace(preShipmentDTO.VehicleType))
            {
                //1.if the shipment Vehicle Type is Bike and the zone is lagos, Calculate the Promo price
                if (zoneId == 1 && preShipmentDTO.VehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower())
                {
                    //GIG Go Promo Price
                    bool totalWeight = await GetTotalWeight(preShipmentDTO);

                    if (totalWeight)
                    {
                        //1. Get the Promo price 900
                        var gigGoPromo = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GIGGOPromo, preShipmentDTO.CountryId);
                        decimal promoPrice = decimal.Parse(gigGoPromo.Value);

                        //if there is promo then change the quantity to 1
                        if (promoPrice > 0)
                        {
                            foreach (var preShipmentItem in preShipmentDTO.PreShipmentItems)
                            {
                                preShipmentItem.Quantity = 1;
                            }
                        }
                    }
                }
            }

            return preShipmentDTO;
        }

        private async Task<bool> GetTotalWeight(PreShipmentMobileDTO preShipmentDTO)
        {
            foreach (var preShipmentItem in preShipmentDTO.PreShipmentItems)
            {
                if (preShipmentItem.ShipmentType == ShipmentType.Special)
                {
                    var getPackageWeight = await _uow.SpecialDomesticPackage.GetAsync(x => x.SpecialDomesticPackageId == preShipmentItem.SpecialPackageId);
                    if (getPackageWeight != null && getPackageWeight.Weight > 3)
                    {
                        return false;
                    }
                }
                else if (preShipmentItem.ShipmentType == ShipmentType.Regular)
                {
                    if (preShipmentItem.Weight > 3)
                    {
                        return false;
                    }
                }
            };
            return true;
        }

        private async Task<decimal> CalculateGeoDetailsBasedonLocation(PreShipmentMobileDTO item)
        {
            var FixedDistance = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GiglgoMaximumFixedDistance, item.CountryId);
            var FixedPriceForTime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GiglgoFixedPriceForTime, item.CountryId);
            var FixedPriceForDistance = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GiglgoFixedPriceForDistance, item.CountryId);

            var FixedDistanceValue = int.Parse(FixedDistance.Value);
            var FixedPriceForTimeValue = Convert.ToDecimal(FixedPriceForTime.Value);
            var FixedPriceForDistanceValue = Convert.ToDecimal(FixedPriceForDistance.Value);

            var Location = new LocationDTO
            {
                DestinationLatitude = (double)item.ReceiverLocation.Latitude,
                DestinationLongitude = (double)item.ReceiverLocation.Longitude,
                OriginLatitude = (double)item.SenderLocation.Latitude,
                OriginLongitude = (double)item.SenderLocation.Longitude
            };

            RootObject details = await GetGeoDetails(Location);
            var time = (details.rows[0].elements[0].duration.value / 60);
            var distance = (details.rows[0].elements[0].distance.value / 1000);

            decimal amount = time * FixedPriceForTimeValue;

            if (distance > FixedDistanceValue)
            {
                var distancedifference = (distance - FixedDistanceValue);
                amount += distancedifference * FixedPriceForDistanceValue;
            }

            return amount;
        }

        public async Task<decimal> GetPickUpPrice(string vehicleType, int CountryId, string UserId)
        {
            try
            {
                var PickUpPrice = 0.0M;
                if (!string.IsNullOrWhiteSpace(vehicleType))
                {
                    if (string.IsNullOrWhiteSpace(UserId))
                    {
                        PickUpPrice = await GetPickUpPriceForIndividual(vehicleType, CountryId);
                    }
                    else
                    {

                        var user = await _userService.GetUserById(UserId);
                        var exists = await _uow.Company.ExistAsync(s => s.CustomerCode == user.UserChannelCode);
                        if (user.UserChannelType == UserChannelType.Ecommerce || exists)
                        {
                            PickUpPrice = await GetPickUpPriceForEcommerce(vehicleType, CountryId);
                        }
                        else
                        {
                            PickUpPrice = await GetPickUpPriceForIndividual(vehicleType, CountryId);
                        }
                    }

                }
                return PickUpPrice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<decimal> GetPickUpPriceForIndividual(string vehicleType, int CountryId)
        {
            decimal PickUpPrice = 0.0M;

            if (string.IsNullOrWhiteSpace(vehicleType))
            {
                return PickUpPrice;
            }

            vehicleType = vehicleType.ToLower();

            if (vehicleType == Vehicletype.Car.ToString().ToLower())
            {
                var carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.CarPickupPrice, CountryId);
                PickUpPrice = Convert.ToDecimal(carPickUprice.Value);
            }
            else if (vehicleType == Vehicletype.Bike.ToString().ToLower())
            {
                var bikePickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikePickUpPrice, CountryId);
                PickUpPrice = Convert.ToDecimal(bikePickUprice.Value);
            }
            else if (vehicleType == Vehicletype.Van.ToString().ToLower())
            {
                var vanPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.VanPickupPrice, CountryId);
                PickUpPrice = Convert.ToDecimal(vanPickUprice.Value);
            }
            return PickUpPrice;
        }

        private async Task<decimal> GetPickUpPriceForEcommerce(string vehicleType, int CountryId)
        {
            var PickUpPrice = 0.0M;

            if (string.IsNullOrWhiteSpace(vehicleType))
            {
                return PickUpPrice;
            }

            vehicleType = vehicleType.ToLower();

            if (vehicleType.ToLower() == Vehicletype.Car.ToString().ToLower())
            {
                var carPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceCarPickupPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(carPickUprice.Value));
            }
            else if (vehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower())
            {
                var bikePickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceBikePickUpPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(bikePickUprice.Value));
            }
            else if (vehicleType.ToLower() == Vehicletype.Van.ToString().ToLower())
            {
                var vanPickUprice = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceVanPickupPrice, CountryId);
                PickUpPrice = (Convert.ToDecimal(vanPickUprice.Value));
            }
            return PickUpPrice;
        }

        private async Task<bool> WithinProcessingTime(int CountryId)
        {
            bool IsWithinTime = false;
            var Startime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PickUpStartTime, CountryId);
            var Endtime = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PickUpEndTime, CountryId);
            TimeSpan start = new TimeSpan(Convert.ToInt32(Startime.Value), 0, 0);
            TimeSpan end = new TimeSpan(Convert.ToInt32(Endtime.Value), 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now > start && now < end)
            {
                IsWithinTime = true;
            }
            return IsWithinTime;
        }

        //promote for GIG Go Feb 2020
        private async Task<MobilePriceDTO> CalculatePromoPrice(PreShipmentMobileDTO preShipmentDTO, int zoneId, decimal pickupValue)
        {
            var result = new MobilePriceDTO
            {
                GrandTotal = 0,
                Discount = 0
            };

            if (!string.IsNullOrWhiteSpace(preShipmentDTO.VehicleType))
            {
                //1.if the shipment Vehicle Type is Bike and the zone is lagos, Calculate the Promo price
                if (preShipmentDTO.VehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower() && zoneId == 1)
                {
                    //GIG Go Promo Price
                    bool totalWeight = await GetTotalWeight(preShipmentDTO);

                    if (totalWeight)
                    {
                        //1. Get the Promo price 999
                        var gigGoPromo = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GIGGOPromo, preShipmentDTO.CountryId);
                        decimal promoPrice = decimal.Parse(gigGoPromo.Value);

                        if (promoPrice > 0)
                        {
                            //3. Make the Promo Price to be GrandTotal 
                            result.GrandTotal = promoPrice;

                            //2. Substract the Promo price from GrandTotal and bind the different to Discount
                            result.Discount = (decimal)preShipmentDTO.CalculatedTotal + pickupValue - promoPrice;
                        }
                    }
                }
            }

            return result;
        }

        private async Task<decimal> CalculateBikePriceBasedonLocation(PreShipmentMobileDTO item)
        {
            var Location = new LocationDTO
            {
                DestinationLatitude = (double)item.ReceiverLocation.Latitude,
                DestinationLongitude = (double)item.ReceiverLocation.Longitude,
                OriginLatitude = (double)item.SenderLocation.Latitude,
                OriginLongitude = (double)item.SenderLocation.Longitude
            };

            RootObject details = await GetGeoDetails(Location);
            var distance = (details.rows[0].elements[0].distance.value / 1000);
            decimal price = 0.0M;

            if (distance <= 25)
            {
                var priceFor25AndBelowkm = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikePrice25KM, item.CountryId);
                var priceFor25AndBelowkmValue = Convert.ToDecimal(priceFor25AndBelowkm.Value);
                price = distance * priceFor25AndBelowkmValue;
            }
            else if (distance > 25 && distance <= 35)
            {
                var priceFor25To35km = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikePrice26TO35KM, item.CountryId);
                var priceFor25To35kmValue = Convert.ToDecimal(priceFor25To35km.Value);
                price = distance * priceFor25To35kmValue;
            }
            else if (distance > 35)
            {
                var priceFor36Above = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BikePrice36KM, item.CountryId);
                var priceFor36AboveValue = Convert.ToDecimal(priceFor36Above.Value);
                price = distance * priceFor36AboveValue;
            }

            return price;
        }

        private async Task<ShipmentType> GetEcommerceCustomerShipmentType(ShipmentType shipmentType)
        {
            //Get the customer Type
            var userChannelCode = await _userService.GetUserChannelCode();
            var userChannel = await _uow.Company.GetAsync(x => x.CustomerCode == userChannelCode);

            if (userChannel != null)
            {
                if (userChannel.CompanyType == CompanyType.Ecommerce)
                {
                    shipmentType = ShipmentType.Ecommerce;
                }
            }

            return shipmentType;
        }

        public async Task<decimal> GetMobileSpecialPrice(PricingDTO pricingDto)
        {
            decimal deliveryOptionPriceTemp = 0;

            var zone = await _domesticroutezonemapservice.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);
            if (zone != null)
            {
                if (zone.ZoneId == 1)
                {
                    if (pricingDto.DeliveryOptionId == 0)
                    {
                        pricingDto.DeliveryOptionId = 7;
                    }
                }
                else
                {
                    pricingDto.DeliveryOptionId = 2;
                }
            }

            decimal PackagePrice = await _specialDomesticZonePriceService.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId, pricingDto.CountryId, pricingDto.Weight);

            //get the deliveryOptionPrice from an array
            deliveryOptionPriceTemp = await _deliveryOptionPriceService.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);


            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

            return shipmentTotalPrice;
        }

        public async Task<decimal> GetMobileEcommercePrice(PricingDTO pricingDto)
        {
            decimal deliveryOptionPriceTemp = 0;
            var zone = await _domesticroutezonemapservice.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);
            if (zone != null)
            {
                if (zone.ZoneId == 1)
                {
                    pricingDto.DeliveryOptionId = 4;
                }
                else
                {
                    pricingDto.DeliveryOptionId = 2;
                }
            }

            deliveryOptionPriceTemp = await _deliveryOptionPriceService.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);
            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            //check for volumetric weight
            if (pricingDto.IsVolumetric)
            {
                decimal volume = (pricingDto.Length * pricingDto.Height * pricingDto.Width) / 5000;
                pricingDto.Weight = pricingDto.Weight > volume ? pricingDto.Weight : volume;
            }

            //Get Ecommerce limit weight from GlobalProperty
            var ecommerceWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceWeightLimit, pricingDto.CountryId);
            decimal weightLimit = decimal.Parse(ecommerceWeightLimitObj.Value);

            decimal PackagePrice;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetEcommercePriceOverflow(pricingDto.Weight, weightLimit, zone.ZoneId, pricingDto.CountryId);
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.Ecommerce, pricingDto.CountryId);
            }

            return PackagePrice + deliveryOptionPrice;
        }

        private async Task<decimal> GetEcommercePriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId, int countryId)
        {
            //Price for Active Weight Limit
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.Regular, countryId);

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.Ecommerce, countryId);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }

        public async Task<decimal> GetMobileRegularPrice(PricingDTO pricingDto)
        {

            var zone = await _domesticroutezonemapservice.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);
            if (zone != null)
            {
                if (zone.ZoneId == 1)
                {
                    pricingDto.DeliveryOptionId = 7;
                }
                else
                {
                    pricingDto.DeliveryOptionId = 2;
                }
            }

            //get the deliveryOptionPrice from an array

            decimal deliveryOptionPriceTemp = await _deliveryOptionPriceService.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);

            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            //check for volumetric weight
            if (pricingDto.IsVolumetric)
            {
                decimal volume = (pricingDto.Length * pricingDto.Height * pricingDto.Width) / 5000;
                pricingDto.Weight = pricingDto.Weight > volume ? pricingDto.Weight : volume;
            }

            //This is our limit weight.
            var activeWeightLimit = await _weightLimit.GetActiveWeightLimits();

            decimal PackagePrice;

            if (pricingDto.Weight > activeWeightLimit.Weight)
            {
                PackagePrice = await GetRegularPriceOverflow(pricingDto.Weight, activeWeightLimit.Weight, zone.ZoneId, pricingDto.CountryId);
            }
            else
            {
                PackagePrice = await GetNormalRegularPrice(pricingDto.Weight, zone.ZoneId, pricingDto.CountryId);
            }
            return PackagePrice + deliveryOptionPrice;
        }

        private async Task<decimal> GetRegularPriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId, int countryId)
        {
            //Price for Active Weight Limit
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.Regular, countryId);

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.Regular, countryId);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }

        private async Task<decimal> GetNormalRegularPrice(decimal weight, int zoneId, int countryId)
        {
            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, weight, RegularEcommerceType.Regular, countryId);
            return PackagePrice;
        }

        public async Task<RootObject> GetGeoDetails(LocationDTO location)
        {
            var Response = new RootObject();
            try
            {
                var GoogleURL = ConfigurationManager.AppSettings["DistanceURL"];
                var GoogleApiKey = ConfigurationManager.AppSettings["DistanceApiKey"];
                GoogleApiKey = await Decrypt(GoogleApiKey);
                var finalURL = $"{GoogleURL}{GoogleApiKey}&units=metric&origins={location.OriginLatitude},{location.OriginLongitude}&destinations={location.DestinationLatitude},{location.DestinationLongitude}";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalURL);
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream result = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(result);
                    string responseFromServer = reader.ReadToEnd();
                    Response = JsonConvert.DeserializeObject<RootObject>(responseFromServer);

                }
            }
            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(Response);
        }

        public async Task<string> Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


    }
}
