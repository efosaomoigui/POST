using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Zone;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices;
using System.Linq;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Configuration;
using GIGLS.Core.Domain.Utility;
using System.Collections.Generic;

namespace GIGLS.Services.Business.Pricing
{
    public class PricingService : IPricingService
    {
        private readonly IDomesticRouteZoneMapService _routeZone;
        private readonly IDeliveryOptionPriceService _optionPrice;
        private readonly IDomesticZonePriceService _regular;
        private readonly ISpecialDomesticZonePriceService _special;
        private readonly IWeightLimitService _weightLimit;
        private readonly IWeightLimitPriceService _weightLimitPrice;
        private readonly IUserService _userService;
        private readonly IHaulageDistanceMappingService _haulageDistanceMappingService;
        private readonly IHaulageDistanceMappingPriceService _haulageDistanceMappingPriceService;
        private readonly IHaulageService _haulageService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ICountryRouteZoneMapService _countryrouteMapService;
        private readonly IServiceCentreService _centreService;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentDeliveryOptionMappingService _shipmentDeliveryOptionMappingService;

        public PricingService(IDomesticRouteZoneMapService zoneService, IDeliveryOptionPriceService optionPriceService,
            IDomesticZonePriceService regular, ISpecialDomesticZonePriceService special,
            IWeightLimitService weightLimit, IWeightLimitPriceService weightLimitPrice,
            IUserService userService, IHaulageDistanceMappingService haulageDistanceMappingService,
            IHaulageDistanceMappingPriceService haulageDistanceMappingPriceService, IServiceCentreService centreService,
            IHaulageService haulageService, IGlobalPropertyService globalPropertyService,
            ICountryRouteZoneMapService countryrouteMapService, IShipmentService shipmentService,
            IShipmentDeliveryOptionMappingService shipmentDeliveryOptionMappingService,
            IUnitOfWork uow)
        {
            _routeZone = zoneService;
            _optionPrice = optionPriceService;
            _regular = regular;
            _special = special;
            _weightLimit = weightLimit;
            _weightLimitPrice = weightLimitPrice;
            _userService = userService;
            _haulageDistanceMappingService = haulageDistanceMappingService;
            _haulageDistanceMappingPriceService = haulageDistanceMappingPriceService;
            _haulageService = haulageService;
            _globalPropertyService = globalPropertyService;
            _countryrouteMapService = countryrouteMapService;
            _centreService = centreService;
            _shipmentService = shipmentService;
            _shipmentDeliveryOptionMappingService = shipmentDeliveryOptionMappingService;
            _uow = uow;
        }

        public Task<decimal> GetPrice(PricingDTO pricingDto)
        {
            if (pricingDto != null)
            {
                switch (pricingDto.ShipmentType)
                {
                    case ShipmentType.Special:
                        return GetSpecialPrice(pricingDto);

                    case ShipmentType.Regular:
                        return GetRegularPrice(pricingDto);

                    case ShipmentType.Ecommerce:
                        return GetEcommercePrice(pricingDto);
                    default:
                        break;
                }
            }
            return null;
        }

        private async Task<decimal> GetSpecialPrice(PricingDTO pricingDto)
        {
            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ", $"{(int)HttpStatusCode.Forbidden}");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);
            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId, pricingDto.CountryId, pricingDto.Weight);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;
            if (!pricingDto.DeliveryOptionIds.Any())
            {
                throw new GenericException("Delivery Option can not be empty", $"{(int)HttpStatusCode.NotFound}");
            }
            else
            {
                foreach (var deliveryOptionId in pricingDto.DeliveryOptionIds)
                {
                    deliveryOptionPriceTemp += await _optionPrice.GetDeliveryOptionPrice(deliveryOptionId, zone.ZoneId, pricingDto.CountryId);
                }
            }

            decimal deliveryOptionPrice = deliveryOptionPriceTemp;
            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

            //Calculate Rank Price for the customer
            shipmentTotalPrice = await CalculateCustomerRankPrice(pricingDto, shipmentTotalPrice);
            return shipmentTotalPrice;
        }

        private async Task<decimal> GetRegularPrice(PricingDTO pricingDto)
        {
            decimal price;

            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ", $"{(int)HttpStatusCode.Forbidden}");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            //get country by service centre
            var departureCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DepartureServiceCentreId);
            var destinationCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DestinationServiceCentreId);

            ///--1. Price within a country
            if (departureCountry.CountryId == destinationCountry.CountryId)
            {
                price = await GetRegularPriceLocal(pricingDto);
            }
            else
            {
                price = await GetRegularPriceInternational(pricingDto);
            }

            price = await CalculateCustomerRankPrice(pricingDto, price);
            return price;
        }

        private async Task<decimal> GetRegularPriceLocal(PricingDTO pricingDto)
        {

            var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;

            if (!pricingDto.DeliveryOptionIds.Any())
            {
                throw new GenericException("Delivery Option can not be empty", $"{(int)HttpStatusCode.NotFound}");
            }
            else
            {
                foreach (var deliveryOptionId in pricingDto.DeliveryOptionIds)
                {
                    deliveryOptionPriceTemp += await _optionPrice.GetDeliveryOptionPrice(deliveryOptionId, zone.ZoneId, pricingDto.CountryId);
                }
            }

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

            ///--1. Price within a country
            if (zone.ZoneId == 9)
            {
                PackagePrice = await GetNormalRegularPrice(pricingDto.Weight, zone.ZoneId, pricingDto.CountryId);
            }
            else
            {
                if (pricingDto.Weight > activeWeightLimit.Weight)
                {
                    PackagePrice = await GetRegularPriceOverflow(pricingDto.Weight, activeWeightLimit.Weight, zone.ZoneId, pricingDto.CountryId);
                }
                else
                {
                    PackagePrice = await GetNormalRegularPrice(pricingDto.Weight, zone.ZoneId, pricingDto.CountryId);
                }
            }

            return PackagePrice + deliveryOptionPrice;
        }

        private async Task<decimal> GetRegularPriceInternational(PricingDTO pricingDto)
        {
            //get country by service centre
            var departureCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DepartureServiceCentreId);
            var destinationCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DestinationServiceCentreId);

            var zone = await _uow.CountryRouteZoneMap.GetAsync(
                    s => s.DepartureId == departureCountry.CountryId && s.DestinationId == destinationCountry.CountryId);

            if (zone == null)
            {
                throw new Exception("Country Route Zone Mapping has not been set.");
            }

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;

            if (pricingDto.DeliveryOptionIds.Count() == 0)
            {
                throw new GenericException("Delivery Option can not be empty");
            }
            else
            {
                foreach (var deliveryOptionId in pricingDto.DeliveryOptionIds)
                {
                    deliveryOptionPriceTemp += await _optionPrice.GetDeliveryOptionPrice(deliveryOptionId, zone.ZoneId, pricingDto.CountryId);
                }
            }

            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            //check for volumetric weight
            if (pricingDto.IsVolumetric)
            {
                decimal volume = (pricingDto.Length * pricingDto.Height * pricingDto.Width) / 5000;
                pricingDto.Weight = pricingDto.Weight > volume ? pricingDto.Weight : volume;
            }

            decimal PackagePrice = 0;

            //update to accomodate for over 100 KG item
            //This is our limit weight.
            if (pricingDto.Weight > 100)
            {
                decimal activeWeightLimit = 100.0M;
                PackagePrice = await GetRegularPriceOverflow(pricingDto.Weight, activeWeightLimit, zone.ZoneId, departureCountry.CountryId);
                //throw new GenericException("WEIGHT EXIST INTERNATIONAL WEIGHT LIMIT");
            }
            else
            {
                PackagePrice = await GetNormalRegularPrice(pricingDto.Weight, zone.ZoneId, departureCountry.CountryId);
            }

            return PackagePrice + deliveryOptionPrice;
        }

        private async Task<decimal> GetNormalRegularPrice(decimal weight, int zoneId, int countryId)
        {
            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, weight, RegularEcommerceType.Regular, countryId);
            return PackagePrice;
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

        public async Task<decimal> GetHaulagePriceOld(HaulagePricingDTO haulagePricingDto)
        {
            decimal price = 0;

            var departureServiceCentreId = haulagePricingDto.DepartureServiceCentreId;
            var destinationServiceCentreId = haulagePricingDto.DestinationServiceCentreId;
            var haulageid = haulagePricingDto.Haulageid;

            //check haulage exists
            var haulage = await _haulageService.GetHaulageById(haulageid);
            if (haulage == null)
            {
                throw new GenericException("The Tonne specified has not been mapped");
            }

            //ensure departureServiceCentreId is set
            if (departureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ");
                }
                departureServiceCentreId = serviceCenters[0];
            }

            //get the distance based on the stations
            var haulageDistanceMapping = await _haulageDistanceMappingService.
                GetHaulageDistanceMapping(departureServiceCentreId, destinationServiceCentreId);
            var distance = haulageDistanceMapping.Distance;

            //get the price
            var haulageDistanceMappingPriceList = await _uow.HaulageDistanceMappingPrice.
                FindAsync(z => z.HaulageId == haulageid &&
                distance >= z.StartRange && distance <= z.EndRange);
            var haulageDistanceMappingPrice = haulageDistanceMappingPriceList.FirstOrDefault();

            if (haulageDistanceMappingPrice != null)
            {
                if (distance == 0)
                {
                    distance = 1;
                }
                price = haulageDistanceMappingPrice.Price * distance;
                return price;
            }

            return price;
        }

        public async Task<decimal> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
        {
            var departureServiceCentreId = haulagePricingDto.DepartureServiceCentreId;
            var destinationServiceCentreId = haulagePricingDto.DestinationServiceCentreId;
            var haulageid = haulagePricingDto.Haulageid;

            //check haulage exists
            var haulage = await _haulageService.GetHaulageById(haulageid);
            if (haulage == null)
            {
                throw new GenericException("The Tonne specified has not been mapped", $"{(int)HttpStatusCode.NotFound}");
            }

            //ensure departureServiceCentreId is set
            if (departureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ", $"{(int)HttpStatusCode.Forbidden}");
                }
                departureServiceCentreId = serviceCenters[0];
            }

            //get the distance based on the stations
            var haulageDistanceMapping = await _haulageDistanceMappingService.GetHaulageDistanceMapping(departureServiceCentreId, destinationServiceCentreId);
            var distance = haulageDistanceMapping.Distance;

            //set the default distance to 1
            if (distance == 0)
            {
                distance = 1;
            }

            //Get Haulage Maximum Fixed Distance            
            if(haulagePricingDto.CountryId == 0)
            {
                var userActiveCountryId = await GetUserCountryId();
                haulagePricingDto.CountryId = userActiveCountryId;
            }
            var maximumFixedDistanceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.HaulageMaximumFixedDistance, haulagePricingDto.CountryId);
            int maximumFixedDistance = int.Parse(maximumFixedDistanceObj.Value);

            decimal price = 0;
            //calculate price for the haulage
            if (distance <= maximumFixedDistance)
            {
                price = haulage.FixedRate;
            }
            else
            {
                //1. get the fixed rate and substract the maximum fixed distance from distance
                decimal fixedRate = haulage.FixedRate;
                distance = distance - maximumFixedDistance;

                //2. multiply the remaining distance with the additional pate
                price = fixedRate + distance * haulage.AdditionalRate;
            }

            var priceDTO = new PricingDTO
            {
                CountryId = haulagePricingDto.CountryId,
                CustomerCode = haulagePricingDto.CustomerCode
            };
            price = await CalculateCustomerRankPrice(priceDTO, price);
            return price;
        }

        //Ecommerce Price
        private async Task<decimal> GetEcommercePrice(PricingDTO pricingDto)
        {
            decimal price;

            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ", $"{(int)HttpStatusCode.Forbidden}");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            //get country by service centre
            var departureCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DepartureServiceCentreId);
            var destinationCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DestinationServiceCentreId);

            ///--1. Price within a country
            if (departureCountry.CountryId == destinationCountry.CountryId)
            {
                price = await GetEcommercePriceLocal(pricingDto);
            }
            else
            {
                price = await GetEcommercePriceInternational(pricingDto);
            }

            price = await CalculateCustomerRankPrice(pricingDto, price);
            return price;
        }

        private async Task<decimal> GetEcommercePriceLocal(PricingDTO pricingDto)
        {
            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ", $"{(int)HttpStatusCode.Forbidden}");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;

            if (!pricingDto.DeliveryOptionIds.Any())
            {
                throw new GenericException("Delivery Option can not be empty", $"{(int)HttpStatusCode.NotFound}");
            }
            else
            {
                foreach (var deliveryOptionId in pricingDto.DeliveryOptionIds)
                {
                    deliveryOptionPriceTemp += await _optionPrice.GetDeliveryOptionPrice(deliveryOptionId, zone.ZoneId, pricingDto.CountryId);
                }
            }

            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            //check for volumetric weight
            if (pricingDto.IsVolumetric)
            {
                decimal volume = (pricingDto.Length * pricingDto.Height * pricingDto.Width) / 5000;
                pricingDto.Weight = pricingDto.Weight > volume ? pricingDto.Weight : volume;
            }

            //Get Ecommerce limit weight from GlobalProperty
            //var ecommerceWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceWeightLimit, pricingDto.CountryId);
            //decimal weightLimit = decimal.Parse(ecommerceWeightLimitObj.Value);
            var activeWeightLimit = await _weightLimit.GetActiveWeightLimits();
            
            decimal PackagePrice;

            if (pricingDto.Weight > activeWeightLimit.Weight)
            {
                PackagePrice = await GetEcommercePriceOverflow(pricingDto.Weight, activeWeightLimit.Weight, zone.ZoneId, pricingDto.CountryId);
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.Regular, pricingDto.CountryId);
            }

            return PackagePrice + deliveryOptionPrice;
        }

        private async Task<decimal> GetEcommercePriceInternational(PricingDTO pricingDto)
        {
            //get country by service centre
            var departureCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DepartureServiceCentreId);
            var destinationCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DestinationServiceCentreId);

            var zone = await _uow.CountryRouteZoneMap.GetAsync(
                    s => s.DepartureId == departureCountry.CountryId && s.DestinationId == destinationCountry.CountryId);

            if (zone == null)
            {
                throw new Exception("Country Route Zone Mapping has not been set.");
            }

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;

            if (!pricingDto.DeliveryOptionIds.Any())
            {
                throw new GenericException("Delivery Option can not be empty");
            }
            else
            {
                foreach (var deliveryOptionId in pricingDto.DeliveryOptionIds)
                {
                    deliveryOptionPriceTemp += await _optionPrice.GetDeliveryOptionPrice(deliveryOptionId, zone.ZoneId, pricingDto.CountryId);
                }
            }

            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            //check for volumetric weight
            if (pricingDto.IsVolumetric)
            {
                decimal volume = (pricingDto.Length * pricingDto.Height * pricingDto.Width) / 5000;
                pricingDto.Weight = pricingDto.Weight > volume ? pricingDto.Weight : volume;
            }

            decimal PackagePrice;

            //This is our limit weight.
            if (pricingDto.Weight > 100)
            {
                decimal activeWeightLimit = 100.0M;
                PackagePrice = await GetEcommercePriceOverflow(pricingDto.Weight, activeWeightLimit, zone.ZoneId, departureCountry.CountryId);
                //throw new GenericException("WEIGHT EXIST INTERNATIONAL WEIGHT LIMIT");
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.Regular, pricingDto.CountryId);
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

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.Regular, countryId);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }

        private async Task<decimal> GetEcommerceReturnPriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId, int countryId)
        {
            //Price for Active Weight Limit Pending the time we have Ecommerce weight limit
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.Regular, countryId);

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.ReturnForEcommerce, countryId);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }

        public async Task<decimal> GetEcommerceReturnPrice(PricingDTO pricingDto)
        {
            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);
            decimal deliveryOptionPrice = 0.0M; //await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);

            //Get Ecommerce Return limit weight from GlobalProperty
            var ecommerceWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceReturnWeightLimit, pricingDto.CountryId);
            decimal weightLimit = decimal.Parse(ecommerceWeightLimitObj.Value);

            //check for volumetric weight
            if (pricingDto.IsVolumetric)
            {
                decimal volume = (pricingDto.Length * pricingDto.Height * pricingDto.Width) / 5000;
                pricingDto.Weight = pricingDto.Weight > volume ? pricingDto.Weight : volume;
            }

            decimal PackagePrice;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetEcommerceReturnPriceOverflow(pricingDto.Weight, weightLimit, zone.ZoneId, pricingDto.CountryId);
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.ReturnForEcommerce, pricingDto.CountryId);
            }

            return PackagePrice + deliveryOptionPrice;
        }

        public async Task<decimal> GetInternationalPrice(PricingDTO pricingDto)
        {
            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                if (serviceCenters.Length > 1)
                {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            var serviceCentreDetail = await _centreService.GetServiceCentreByIdForInternational(pricingDto.DepartureServiceCentreId);

            var zone = await _countryrouteMapService.GetZone(serviceCentreDetail.CountryId, pricingDto.DestinationServiceCentreId);

            //decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //Get International Weight Limit from GlobalProperty
            var internationalWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit, pricingDto.CountryId);

            decimal weightLimit = decimal.Parse(internationalWeightLimitObj.Value);

            decimal PackagePrice;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetInternationalPriceOverflow(pricingDto.Weight, weightLimit, zone.ZoneId, pricingDto.CountryId);
            }
            else
            {
                //Check if the weight less than 2.0 Kg and it is Document Shipment (Add this to global Property later)
                if (pricingDto.Weight <= 2 && pricingDto.IsInternationalDocument)
                {
                    PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.InternationalDocument, pricingDto.CountryId);
                }
                else
                {
                    PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.International, pricingDto.CountryId);
                }
            }

            //Get Percentage Charge For International Shipment Service
            var PercentageForInternationalShipment = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PercentageForInternationalShipment, pricingDto.CountryId);
            decimal percentage = decimal.Parse(PercentageForInternationalShipment.Value);

            var percentagePrice = (percentage / 100) * PackagePrice;

            // War Surcharge calculation
            var warSurchargePrice = await WarSurcharge(pricingDto.DestinationServiceCentreId);

            PackagePrice = PackagePrice + percentagePrice + warSurchargePrice;

            PackagePrice = await CalculateCustomerRankPrice(pricingDto, PackagePrice);
            return PackagePrice;
        }

        // War Surcharge calculation
        private async Task<decimal> WarSurcharge(int countryId)
        {
            decimal warSurcharge = 0;

            var userActiveCountryId = await GetUserCountryId();

            var libyaObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.LIBYA, userActiveCountryId);
            var syriaObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.SYRIA, userActiveCountryId);
            var yemenObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.YEMEN, userActiveCountryId);

            var country = await _uow.Country.GetAsync(countryId);
            if (country != null)
            {
                if (country.CountryName == libyaObj.Key)
                {
                    warSurcharge = decimal.Parse(libyaObj.Value);
                }

                if (country.CountryName == syriaObj.Key)
                {
                    warSurcharge = decimal.Parse(syriaObj.Value);
                }

                if (country.CountryName == yemenObj.Key)
                {
                    warSurcharge = decimal.Parse(yemenObj.Value);
                }
            }

            return warSurcharge;
        }

        private async Task<decimal> GetInternationalPriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId, int countryId)
        {
            var userActiveCountryId = await GetUserCountryId();

            var InternationalWeightLimit30 = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit30, userActiveCountryId);
            var InternationalWeightLimit70 = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit70, userActiveCountryId);
            var InternationalWeightLimit100 = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit100, userActiveCountryId);

            decimal weightLimit30 = decimal.Parse(InternationalWeightLimit30.Value);
            decimal weightLimit70 = decimal.Parse(InternationalWeightLimit70.Value);
            decimal weightLimit100 = decimal.Parse(InternationalWeightLimit100.Value);

            //Price for Active Weight Limit
            WeightLimitPriceDTO activeZone;

            if (weight <= weightLimit30)
            {
                activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.InternationalWeightLimit30, countryId);
            }

            else if (weight <= weightLimit70)
            {
                activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.InternationalWeightLimit70, countryId);
            }

            else
            {
                activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.InternationalWeightLimit100, countryId);
            }

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.International, countryId);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }

        public async Task<ShipmentDTO> GetReroutePrice(ReroutePricingDTO pricingDto)
        {
            var waybill = pricingDto.Waybill;
            var departureServiceCentreId = pricingDto.DepartureServiceCentreId;
            var destinationServiceCentreId = pricingDto.DestinationServiceCentreId;

            //1. get the shipment
            var shipment = await _shipmentService.GetShipment(waybill);

            //2. get the shipment item and calculate the new prices
            decimal totalPrice = 0;
            if (!shipment.Waybill.Contains("AWR") || !shipment.Waybill.Contains("WR"))
            {
                foreach (var item in shipment.ShipmentItems)
                {
                    // Get SpecialPackageId if shipment is a special type
                    var specialPackageId = 0;
                    if (item.ShipmentType == ShipmentType.Special)
                    {
                        var specialPackage = await _uow.SpecialDomesticPackage.GetAsync(s => s.Name == item.Description);
                        if (specialPackage != null)
                        {
                            specialPackageId = specialPackage.SpecialDomesticPackageId;
                        }
                    }

                    //get ShipmentDeliveryOptionMapping
                    var shipmentDeliveryOptionMapping =
                        await _shipmentDeliveryOptionMappingService.GetDeliveryOptionInWaybill(waybill);
                    var deliveryOptionIds = shipmentDeliveryOptionMapping.Select(s => s.DeliveryOptionId).ToList();

                    if (shipment.CompanyType == CompanyType.Ecommerce.ToString())
                    {
                        item.ShipmentType = ShipmentType.Ecommerce;
                    }

                    //unit price per item
                    var itemPrice = await GetPrice(new PricingDTO()
                    {
                        DepartureServiceCentreId = departureServiceCentreId,
                        DestinationServiceCentreId = destinationServiceCentreId,
                        DeliveryOptionId = shipment.DeliveryOptionId,
                        DeliveryOptionIds = deliveryOptionIds,
                        ShipmentType = item.ShipmentType,
                        SpecialPackageId = specialPackageId,
                        Weight = decimal.Parse(item.Weight.ToString()),
                        Width = decimal.Parse(item.Width.ToString()),
                        Length = decimal.Parse(item.Length.ToString()),
                        Height = decimal.Parse(item.Height.ToString()),
                        IsVolumetric = item.IsVolumetric,
                        CountryId = pricingDto.CountryId,
                        CustomerCode = shipment.CustomerCode
                    });

                    //unit price based on quantity
                    var totalItemPrice = itemPrice * item.Quantity;

                    //set the new price
                    item.Price = totalItemPrice;

                    //accumulate the total price
                    totalPrice += totalItemPrice;
                }

                //set totalPrice and GrandTotal for reroute
                shipment.Total = totalPrice;
                shipment.GrandTotal = totalPrice;
            }

            //update departure and destination
            shipment.DepartureServiceCentreId = departureServiceCentreId;
            shipment.DepartureServiceCentre = await _centreService.GetServiceCentreByIdForInternational(departureServiceCentreId);
            shipment.DestinationServiceCentreId = destinationServiceCentreId;
            shipment.DestinationServiceCentre = await _centreService.GetServiceCentreByIdForInternational(destinationServiceCentreId);

            ///////Added for Country Ratio Price //////
            //shipment = await UpdateShipmentPriceBasedOnCountryCurrencyRatio(shipment);
            return shipment;
        }

        public async Task<decimal> GetMobileRegularPrice(PricingDTO pricingDto)
        {
            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);
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
            decimal deliveryOptionPriceTemp = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);
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

            PackagePrice = PackagePrice + deliveryOptionPrice;
            PackagePrice = await CalculateCustomerRankPrice(pricingDto, PackagePrice);
            return PackagePrice;
        }

        public async Task<decimal> GetDropOffRegularPriceForIndividual(PricingDTO pricingDto)
        {
            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);

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

            PackagePrice = PackagePrice + deliveryOptionPrice;
            PackagePrice = await CalculateCustomerRankPrice(pricingDto, PackagePrice);
            return PackagePrice;
        }

        public async Task<decimal> GetMobileEcommercePrice(PricingDTO pricingDto)
        {
            decimal deliveryOptionPriceTemp = 0;
            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);
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

            deliveryOptionPriceTemp = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);
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

        public async Task<decimal> GetEcommerceDropOffPrice(PricingDTO pricingDto)
        {
            decimal deliveryOptionPriceTemp = 0;
            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);

            deliveryOptionPriceTemp = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);
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

        public async Task<decimal> GetMobileSpecialPrice(PricingDTO pricingDto)
        {
            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);
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

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId, pricingDto.CountryId, pricingDto.Weight);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);
            decimal deliveryOptionPrice = deliveryOptionPriceTemp;
            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

            shipmentTotalPrice = await CalculateCustomerRankPrice(pricingDto, shipmentTotalPrice);
            return shipmentTotalPrice;
        }

        public async Task<decimal> GetDropOffSpecialPrice(PricingDTO pricingDto)
        {
            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId, pricingDto.CountryId, pricingDto.Weight);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);
            //decimal deliveryOptionPrice = deliveryOptionPriceTemp;
            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;
            shipmentTotalPrice = await CalculateCustomerRankPrice(pricingDto, shipmentTotalPrice);
            return shipmentTotalPrice;
        }

        public async Task<decimal> GetCountryCurrencyRatio()
        {
            decimal countryCurrencyRatio = 0;

            //1. Get the user active country
            var countries = await _userService.GetPriviledgeCountrys();
            if (countries.Count == 1)
            {
                var userActiveCountry = countries[0];
                var currencyRatio = userActiveCountry.CurrencyRatio;

                if (currencyRatio > 0)
                {
                    countryCurrencyRatio = currencyRatio;
                }
                else
                {
                    //throw exception
                    throw new GenericException($"The CurrencyRatio for this country ({userActiveCountry.CountryName}) has not been assigned. Kindly inform the administrator.");
                }
            }
            else
            {
                //Use default country
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var countryId = currentUser.UserActiveCountryId;

                if (countryId > 0)
                {
                    var country = await _uow.Country.GetAsync(countryId);
                    if (country.CurrencyRatio > 0)
                    {
                        countryCurrencyRatio = country.CurrencyRatio;
                    }
                    else
                    {
                        //throw exception
                        throw new GenericException($"The CurrencyRatio for this country ({country.CountryName}) has not been assigned. Kindly inform the administrator.");
                    }
                }
                else
                {
                    //throw exception
                    throw new GenericException($"You have not been assigned an Active Country. Kindly inform the administrator.");
                }
            }

            return countryCurrencyRatio;
        }

        public async Task<int> GetUserCountryId()
        {
            int UserCountryId = 0;

            //1. Get the user active country
            var countries = await _userService.GetPriviledgeCountrys();
            if (countries.Count == 1)
            {
                var userActiveCountry = countries[0];
                UserCountryId = userActiveCountry.CountryId;
            }
            else
            {
                //Use default country
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                var countryId = currentUser.UserActiveCountryId;

                if (countryId > 0)
                {
                    UserCountryId = countryId;
                }
                else
                {
                    //throw exception
                    throw new GenericException($"You have not been assigned an Active Country. Kindly inform the administrator.");
                }
            }

            return UserCountryId;
        }

        private async Task<ShipmentDTO> UpdateShipmentPriceBasedOnCountryCurrencyRatio(ShipmentDTO shipment)
        {
            var countryCurrencyRatio = await GetCountryCurrencyRatio();

            //update each shipment item price
            foreach (var item in shipment.ShipmentItems)
            {
                item.Price = item.Price * countryCurrencyRatio;
            }

            //update the Total and GrandTotal for the complete shipment
            shipment.Total = shipment.Total * countryCurrencyRatio;
            shipment.GrandTotal = shipment.GrandTotal * countryCurrencyRatio;

            return shipment;
        }

        public async Task<NewPricingDTO> GetGrandPriceForShipment(NewShipmentDTO newShipmentDTO)
        {
            NewPricingDTO newPricingDTO = new NewPricingDTO();
            if (newShipmentDTO == null)
            {
                throw new GenericException("Invalid payload", $"{(int)HttpStatusCode.BadRequest}");
            }
            if (!newShipmentDTO.ShipmentItems.Any())
            {
                throw new GenericException("No shipment item", $"{(int)HttpStatusCode.BadRequest}");
            }

            decimal totalPrice = 0;
            decimal grandTotal = 0;

             //get total price for shipment items
            foreach (var item in newShipmentDTO.ShipmentItems)
            {
                var priceDTO = JObject.FromObject(newShipmentDTO).ToObject<PricingDTO>();
                priceDTO.Weight = Convert.ToDecimal(item.Weight);
                priceDTO.Length = Convert.ToDecimal(item.Length);
                priceDTO.IsVolumetric = item.IsVolumetric;
                priceDTO.Height = Convert.ToDecimal(item.Height);
                priceDTO.ShipmentType = item.ShipmentType;
                priceDTO.SpecialPackageId = item.SpecialPackageId.Value;
                priceDTO.Width = Convert.ToDecimal(item.Width);
                priceDTO.CountryId = newShipmentDTO.DepartureCountryId;
                priceDTO.CustomerCode = newShipmentDTO.CustomerCode;

                if (priceDTO.ShipmentType.ToString().ToLower() == ShipmentType.Regular.ToString().ToLower())
                {
                    var itemPrice = await GetRegularPrice(priceDTO);
                    totalPrice += itemPrice;
                }
                else if (priceDTO.ShipmentType.ToString().ToLower() == ShipmentType.Special.ToString().ToLower())
                {
                    var itemPrice = await GetSpecialPrice(priceDTO);
                    totalPrice += itemPrice;
                }
                else if (priceDTO.ShipmentType.ToString().ToLower() == ShipmentType.Ecommerce.ToString().ToLower())
                {
                    var itemPrice = await GetEcommercePrice(priceDTO);
                    totalPrice += itemPrice;
                }
            }
                                                          
            //calculate the vat
            var vatDTO = await _uow.VAT.GetAsync(x => x.CountryId == newShipmentDTO.DepartureCountryId);
            decimal vat = (vatDTO != null) ? (vatDTO.Value / 100) : (7.5M / 100);
            var vatForItems = totalPrice * vat;
            var vatValue = totalPrice + vatForItems;
            grandTotal += vatValue;

            //calculate insurance
            decimal insurance = 0.0m;
            if (newShipmentDTO.DeclarationOfValueCheck > 0)
            {
                insurance = await CalculateInsurance(newShipmentDTO);
                grandTotal = grandTotal + insurance;
            }

            if (newShipmentDTO.DiscountValue > 0)
            {
                var discount = newShipmentDTO.DiscountValue / 100m;
                decimal distValue = grandTotal * discount.Value;
                newPricingDTO.DiscountedValue = distValue;
                grandTotal = grandTotal - distValue;
            }

            var factor = Convert.ToDecimal(Math.Pow(10, -2));
            newPricingDTO.Vat = vatForItems;
            newPricingDTO.Insurance = insurance;
            newPricingDTO.Total = totalPrice;
            newPricingDTO.GrandTotal = Math.Round(grandTotal * factor) / factor;
            return newPricingDTO;
        }

        public async Task<decimal> CalculateCustomerRankPrice(PricingDTO pricingDto, decimal price)
        {
            if (!string.IsNullOrWhiteSpace(pricingDto.CustomerCode))
            {
                var customer = await _uow.Company.GetAsync(x => x.CustomerCode == pricingDto.CustomerCode);

                if(customer != null)
                {
                    if(customer.CompanyType == CompanyType.Ecommerce)
                    {
                        if (customer.Rank == Rank.Class)
                        {
                            price = await CalculateClassRankPrice(pricingDto.CountryId, price);
                        }
                        else
                        {
                            price = await CalculateBasicRankPrice(pricingDto.CountryId, price);
                        }
                    }
                }
            }
            return price;
        }

        private async Task<decimal> CalculateClassRankPrice(int countryId, decimal price)
        {
            var classrank = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.ClassRankPercentage, countryId);
            decimal classRankPercentage = decimal.Parse(classrank.Value);
            decimal classRankValue = ((100M - classRankPercentage) / 100M);
            price = price * classRankValue;
            return price;
        }

        private async Task<decimal> CalculateBasicRankPrice(int countryId, decimal price)
        {
            var classrank = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.BasicRankPercentage, countryId);
            decimal classRankPercentage = decimal.Parse(classrank.Value);
            decimal classRankValue = ((100M - classRankPercentage) / 100M);
            price = price * classRankValue;
            return price;
        }

        private async Task<decimal> CalculateInsurance(NewShipmentDTO newShipmentDTO)
        {
            decimal insurance = 0.0m;
            decimal minimumDeclareValueCheck = 0.0m;
            decimal declarationValue = Convert.ToDecimal(newShipmentDTO.DeclarationOfValueCheck);
            if (newShipmentDTO.DepartureCountryId == 1)
            {
                var customerInfo = await _uow.Company.GetAsync(x => x.CustomerCode == newShipmentDTO.CustomerCode);
                if (customerInfo != null)
                {
                    if (customerInfo.Rank == Rank.Class)
                    {
                        var classValue = ConfigurationManager.AppSettings["MinimumDeclareValueCheckClass"];
                        minimumDeclareValueCheck = Convert.ToDecimal(classValue);
                    }
                    else
                    {
                        //all customer in Nigeria
                        var basicValue = ConfigurationManager.AppSettings["MinimumDeclareValueCheckBasic"];
                        minimumDeclareValueCheck = Convert.ToDecimal(basicValue);
                    }
                }
                else
                {
                    //all customer in Nigeria
                    var basicValue = ConfigurationManager.AppSettings["MinimumDeclareValueCheckBasic"];
                    minimumDeclareValueCheck = Convert.ToDecimal(basicValue);
                }
            }
            else
            {
                //Ghana
                var ghanaValue = ConfigurationManager.AppSettings["MinimumDeclareValueCheckGhana"];
                minimumDeclareValueCheck = Convert.ToDecimal(ghanaValue); ;
            }

            if(declarationValue > minimumDeclareValueCheck)
            {
                var insuranceDTO = await _uow.Insurance.GetAsync(x => x.CountryId == newShipmentDTO.DepartureCountryId);
                decimal insuranceValue = (insuranceDTO != null) ? (insuranceDTO.Value / 100) : (1M / 100);
                insurance = declarationValue * insuranceValue;
            }
            return insurance;
        }
        public async Task<decimal> GetPriceForUK(PricingDTO pricingDto)
        {
            //get country by service centre
            try
            {
                var price = 0.0m;
                if (pricingDto.DepartureServiceCentreId <= 0)
                {
                    // use currentUser login servicecentre
                    var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                    if (serviceCenters.Length > 1)
                    {
                        throw new GenericException("This user is assign to more than one(1) Service Centre  ", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    pricingDto.DepartureServiceCentreId = serviceCenters[0];
                }
                var departureCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DepartureServiceCentreId);
                var destinationCountry = await _uow.Country.GetCountryByServiceCentreId(pricingDto.DestinationServiceCentreId);
                var globalProperties = _uow.GlobalProperty.GetAllAsQueryable().Where(x => x.CountryId == departureCountry.CountryId && x.IsActive == true).ToList();
                if (pricingDto != null)
                {
                    switch (pricingDto.Description)
                    {
                        case GlobalPropertyType.NewGadgets:
                            return await GetPriceForNewGadgetsUK(pricingDto, globalProperties);

                        case GlobalPropertyType.ChildrenGadgets:
                            return await GetPriceForNewGadgetsUK(pricingDto, globalProperties);

                        case GlobalPropertyType.UsedGadgets:
                            return await GetPriceForUsedGadgetsUK(pricingDto, globalProperties);

                        case GlobalPropertyType.UsedGadgetsLessThan6Kg:
                            return await GetPriceForUsedGadgetsUK(pricingDto, globalProperties);

                        case GlobalPropertyType.Perfumes:
                            return await GetPriceForPerfumesUK(pricingDto, globalProperties);

                        case GlobalPropertyType.PerfumesLessThan6Kg:
                            return await GetPriceForPerfumesUK(pricingDto, globalProperties);

                        case GlobalPropertyType.OthersUK:
                            return await GetPriceForOthersUK(pricingDto, globalProperties);
                        default:
                            break;
                    }
                }
                return price;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<decimal> GetPriceForOthersUK(PricingDTO pricingDto,List<GlobalProperty> globalProperties)
        {
            //get country by service centre
            var price = 0.0m;
            // check if categories is others, and weight is less than 20kg use flat rate of 20kg else use 5kg 
            //check if the weight less than 20kg
            if (pricingDto.Weight < 20)
            {
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.OthersLessThan20KgUK.ToString()).FirstOrDefault();
                //get itemCategory where is others and less than 20kg
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            else
            {
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.OthersUK.ToString()).FirstOrDefault();
                //get itemCategory where is others and greater or equal to 20kg
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            return price;
        }

        private async Task<decimal> GetPriceForNewGadgetsUK(PricingDTO pricingDto, List<GlobalProperty> globalProperties)
        {
            //get country by service centre
            var price = 0.0m;
            // check if categories is new gadgets adult then use flat rate of 50pounds
             if (pricingDto.Description == GlobalPropertyType.NewGadgets)
            {
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.NewGadgets.ToString()).FirstOrDefault();
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    //get itemCategory where is new gadgets
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            else if (pricingDto.Description == GlobalPropertyType.ChildrenGadgets)
            {
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.ChildrenGadgets.ToString()).FirstOrDefault();
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    //get itemCategory where is children gadgets
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            return price;
        }

        private async Task<decimal> GetPriceForUsedGadgetsUK(PricingDTO pricingDto, List<GlobalProperty> globalProperties)
        {
            //get country by service centre
            var price = 0.0m;
            // check if categories is new gadgets adult then use flat rate of 50pounds
            if (pricingDto.Weight < 6)
            {
                //get itemCategory where is used gadgets less than 6kg
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.UsedGadgetsLessThan6Kg.ToString()).FirstOrDefault();
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            else
            {
                //get itemCategory where is used gadgets greater than 6kg
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.UsedGadgets.ToString()).FirstOrDefault();
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            return price;
        }

        private async Task<decimal> GetPriceForPerfumesUK(PricingDTO pricingDto, List<GlobalProperty> globalProperties)
        {
            //get country by service centre
            var price = 0.0m;
            // check if categories is new gadgets adult then use flat rate of 50pounds
            if (pricingDto.Weight < 6)
            {
                //get itemCategory where is perfumes less than 6kg
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.PerfumesLessThan6Kg.ToString()).FirstOrDefault();
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            else
            {
                //get itemCategory where is perfumes greater than 6kg
                var itemCategory = globalProperties.Where(x => x.Key == GlobalPropertyType.PerfumesLessThan6Kg.ToString()).FirstOrDefault();
                for (int i = 1; i <= pricingDto.Quantity; i++)
                {
                    price = price + Convert.ToDecimal(itemCategory.Value);
                }
            }
            return price;
        }
    }
}
