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

        public PricingService(IDomesticRouteZoneMapService zoneService, IDeliveryOptionPriceService optionPriceService,
            IDomesticZonePriceService regular, ISpecialDomesticZonePriceService special,
            IWeightLimitService weightLimit, IWeightLimitPriceService weightLimitPrice,
            IUserService userService, IHaulageDistanceMappingService haulageDistanceMappingService,
            IHaulageDistanceMappingPriceService haulageDistanceMappingPriceService,
            IHaulageService haulageService, IUnitOfWork uow)
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
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId);

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
            var weightInTonne = haulagePricingDto.WeightInTonne;

            //check haulage exists
            var haulages = await _haulageService.GetHaulages();
            var haulage = haulages.FirstOrDefault(s => s.Tonne == weightInTonne);
            if(haulage == null)
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
            var haulageDistanceMappingPrices = await _haulageDistanceMappingPriceService.
                GetHaulageDistanceMappingPrices();

            foreach (var item in haulageDistanceMappingPrices)
            {
                if (item.StartRange >= distance && distance <= item.EndRange
                    && item.Haulage.Tonne == weightInTonne)
                {
                    price = item.Price * distance;
                    return price;
                }
            }

            return price;
        }
        
        private async Task<decimal> GetEcommercePrice(PricingDTO pricingDto)
        {
            if (pricingDto.DepartureServiceCentreId <= 0)
            {
                // use currentUser login servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                if (serviceCenters.Length > 1) {
                    throw new GenericException("This user is assign to more than one(1) Service Centre  ");
                }                    
                pricingDto.DepartureServiceCentreId = serviceCenters[0];
            }
            
            var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zone.ZoneId, pricingDto.Weight, RegularEcommerceType.Ecommerce);
            return PackagePrice + deliveryOptionPrice;
        }        
    }
}
