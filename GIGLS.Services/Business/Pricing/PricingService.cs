﻿using GIGLS.Core.IServices.Business;
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
using System;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO.Shipments;

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

        public PricingService(IDomesticRouteZoneMapService zoneService, IDeliveryOptionPriceService optionPriceService,
            IDomesticZonePriceService regular, ISpecialDomesticZonePriceService special,
            IWeightLimitService weightLimit, IWeightLimitPriceService weightLimitPrice,
            IUserService userService, IHaulageDistanceMappingService haulageDistanceMappingService,
            IHaulageDistanceMappingPriceService haulageDistanceMappingPriceService, IServiceCentreService centreService,
            IHaulageService haulageService, IGlobalPropertyService globalPropertyService, ICountryRouteZoneMapService countryrouteMapService, IUnitOfWork uow)
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
            _uow = uow;
        }

        public Task<decimal> GetPrice(PricingDTO pricingDto)
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
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ");
                }
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }

            var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId);

            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

            return shipmentTotalPrice;
        }

        private async Task<decimal> GetRegularPrice(PricingDTO pricingDto)
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
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //This is our limit weight.
            var activeWeightLimit = await _weightLimit.GetActiveWeightLimits();

            decimal PackagePrice;

            if (pricingDto.Weight > activeWeightLimit.Weight)
            {
                PackagePrice = await GetRegularPriceOverflow(pricingDto.Weight, activeWeightLimit.Weight, zone.ZoneId);
            }
            else
            {
                PackagePrice = await GetNormalRegularPrice(pricingDto.Weight, zone.ZoneId);
            }
            return PackagePrice + deliveryOptionPrice;
        }

        private async Task<decimal> GetNormalRegularPrice(decimal weight, int zoneId)
        {
            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, weight, RegularEcommerceType.Regular);
            return PackagePrice;
        }

        private async Task<decimal> GetRegularPriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId)
        {
            //Price for Active Weight Limit
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.Regular);

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.Regular);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }

        public async Task<decimal> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
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

        //Ecommerce Price
        private async Task<decimal> GetEcommercePrice(PricingDTO pricingDto)
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
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //Get Ecommerce limit weight from GlobalProperty
            var ecommerceWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceWeightLimit);

            decimal weightLimit = decimal.Parse(ecommerceWeightLimitObj.Value);

            decimal PackagePrice;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetEcommercePriceOverflow(pricingDto.Weight, weightLimit, zone.ZoneId);
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.Ecommerce);
            }
            
            return PackagePrice + deliveryOptionPrice;
        }
        
        private async Task<decimal> GetEcommercePriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId)
        {
            //Price for Active Weight Limit
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.Regular);

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.Ecommerce);

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
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //Get Ecommerce Return limit weight from GlobalProperty
            var ecommerceWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceReturnWeightLimit);
            decimal weightLimit = decimal.Parse(ecommerceWeightLimitObj.Value);

            decimal PackagePrice;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetEcommercePriceOverflow(pricingDto.Weight, weightLimit, zone.ZoneId);
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.Ecommerce);
            }
            
            return PackagePrice + deliveryOptionPrice;
        }

        public async Task<decimal> GetInternationalPrice(PricingDTO pricingDto)
        {
            // use currentUser login servicecentre
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            if (serviceCenters.Length > 1)
            {
                throw new GenericException("This user is assign to more than one(1) Service Centre  ");
            }

            var serviceCentreDetail = await _centreService.GetServiceCentreById(serviceCenters[0]);

            var zone = await _countryrouteMapService.GetZone(serviceCentreDetail.CountryId, pricingDto.DestinationServiceCentreId);

            //decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //Get International Weight Limit from GlobalProperty
            var internationalWeightLimitObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit);

            decimal weightLimit = decimal.Parse(internationalWeightLimitObj.Value);

            decimal PackagePrice;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetInternationalPriceOverflow(pricingDto.Weight, weightLimit, zone.ZoneId);
            }
            else
            {
                PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.International);
            }

            //Get Percentage Charge For International Shipment Service
            var PercentageForInternationalShipment = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.PercentageForInternationalShipment);
            decimal percentage = decimal.Parse(PercentageForInternationalShipment.Value);

            var percentagePrice = (percentage / 100) * PackagePrice;

            // War Surcharge calculation
            var warSurchargePrice = await WarSurcharge(pricingDto.DestinationServiceCentreId);


            // return PackagePrice + deliveryOptionPrice;
            return (PackagePrice + percentagePrice + warSurchargePrice);
        }


        // War Surcharge calculation
        private async Task<decimal> WarSurcharge(int countryId)
        {
            decimal warSurcharge = 0;

            var libyaObj= await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.LIBYA);
            var syriaObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.SYRIA);
            var yemenObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.YEMEN);

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

        private async Task<decimal> GetInternationalPriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId)
        {
            var InternationalWeightLimit30 = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit30);
            var InternationalWeightLimit70 = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit70);
            var InternationalWeightLimit100 = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.InternationalWeightLimit100);

            decimal weightLimit30 = decimal.Parse(InternationalWeightLimit30.Value);
            decimal weightLimit70 = decimal.Parse(InternationalWeightLimit70.Value);
            decimal weightLimit100 = decimal.Parse(InternationalWeightLimit100.Value);
            
            //Price for Active Weight Limit
            WeightLimitPriceDTO activeZone;

            if (weight <= weightLimit30)
            {
                activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.InternationalWeightLimit30);
            }

            else if (weight <= weightLimit70)
            {
                activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.InternationalWeightLimit70);
            }

            else
            {
                activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId, RegularEcommerceType.InternationalWeightLimit100);
            }
            
            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit, RegularEcommerceType.International);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }
        
        public Task<ShipmentDTO> GetReroutePrice(ReroutePricingDTO pricingDto)
        {
            //var waybill = pricingDto.Waybill;
            //var destinationServiceCentreId = 1;




            throw new NotImplementedException();
        }
    }
}
