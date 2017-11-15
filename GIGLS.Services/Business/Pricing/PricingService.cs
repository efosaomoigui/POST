﻿using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Zone;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;

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
        private readonly IUnitOfWork _uow;

        public PricingService (IDomesticRouteZoneMapService zoneService, IDeliveryOptionPriceService optionPriceService,
            IDomesticZonePriceService regular, ISpecialDomesticZonePriceService special,
            IWeightLimitService weightLimit, IWeightLimitPriceService weightLimitPrice,
            IUnitOfWork uow)
        {
            _routeZone = zoneService;
            _optionPrice = optionPriceService;
            _regular = regular;
            _special = special;
            _weightLimit = weightLimit;
            _weightLimitPrice = weightLimitPrice;
            _uow = uow;
        }

        public async Task<decimal> GetSpecialPrice(PricingDTO pricingDto)
        {
            //for test//
            var userServiceCentreMapping = new UserServiceCentreMapping();
            var userServiceCentreMappingList = await _uow.UserServiceCentreMapping.FindAsync(s => s.IsActive == true);
            foreach(var item in userServiceCentreMappingList)
            {
                userServiceCentreMapping = item;
                break;
            }
            var departureServiceCentreId = userServiceCentreMapping.ServiceCentreId;
            var userId = userServiceCentreMapping.UserId;
            //for test//


            //var zone = await _routeZone.GetZone(pricingDto.DepartureServiceCentreId, pricingDto.DestinationServiceCentreId);
            var zone = await _routeZone.GetZone(departureServiceCentreId, pricingDto.DestinationServiceCentreId);

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId);

            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

            return shipmentTotalPrice;
        }
        
        public Task<decimal> GetPrice(PricingDTO pricingDto)
        {
            switch (pricingDto.ShipmentType)
            {
                case ShipmentType.Special:
                    return GetSpecialPrice(pricingDto);
                    
                case ShipmentType.Regular:
                    return GetRegularPrice(pricingDto);
                default:
                    break;
            }
            return null;
             
            //if(pricingDto.PricingType == PricingType.Special)
            //{
            //   return GetSpecialPrice(pricingDto);
            //}
            
            //if (pricingDto.PricingType == PricingType.Regular)
            //{
            //    return GetRegularPrice(pricingDto);
            //}
            //return null;
        }

        private async Task<decimal> GetRegularPrice(PricingDTO pricingDto)
        {
            //for test//
            var userServiceCentreMapping = new UserServiceCentreMapping();
            var userServiceCentreMappingList = await _uow.UserServiceCentreMapping.FindAsync(s => s.IsActive == true);
            foreach (var item in userServiceCentreMappingList)
            {
                userServiceCentreMapping = item;
                break;
            }
            var departureServiceCentreId = userServiceCentreMapping.ServiceCentreId;
            var userId = userServiceCentreMapping.UserId;
            //for test//



            decimal PackagePrice;

            var zone = await _routeZone.GetZone(departureServiceCentreId, pricingDto.DestinationServiceCentreId);
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //This is our limit weight. This will be deleted once limit Price Setting is approve
            var activeWeight = await _weightLimit.GetActiveWeightLimits();
            decimal weightLimit = activeWeight.Weight;

            if (pricingDto.Weight > weightLimit)
            {
                PackagePrice = await GetRegularPriceOverflow(pricingDto.Weight, zone.ZoneId);
            }
            else
            {
               PackagePrice = await GetNormalRegularPrice(pricingDto.Weight, zone.ZoneId);
            }
            return PackagePrice + deliveryOptionPrice;
        }
        
        private async Task<decimal> GetNormalRegularPrice(decimal weight, int zoneId)
        {
            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, weight);
            return PackagePrice;
        }

        private async Task<decimal> GetRegularPriceOverflow(decimal weight, int zoneId)
        {
            var activeWeight = await _weightLimit.GetActiveWeightLimits();
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId);

            decimal weightLimit = activeWeight.Weight; //This is our limit weight

            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - weightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, weightLimit);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }
                     
    }
}
