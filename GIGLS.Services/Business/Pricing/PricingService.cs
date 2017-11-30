using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Zone;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;

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
        private readonly IUnitOfWork _uow;

        public PricingService (IDomesticRouteZoneMapService zoneService, IDeliveryOptionPriceService optionPriceService,
            IDomesticZonePriceService regular, ISpecialDomesticZonePriceService special,
            IWeightLimitService weightLimit, IWeightLimitPriceService weightLimitPrice, IUserService userService,
            IUnitOfWork uow)
        {
            _routeZone = zoneService;
            _optionPrice = optionPriceService;
            _regular = regular;
            _special = special;
            _weightLimit = weightLimit;
            _weightLimitPrice = weightLimitPrice;
            _userService = userService;
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
                default:
                    break;
            }
            return null;
        }

        private async Task<decimal> GetSpecialPrice(PricingDTO pricingDto)
        {
            //Get Login User and Service Centre User belong 
            var currentUser = await _userService.GetCurrentUserId();
            
            var departureServiceCentreId = await _userService.GetPriviledgeServiceCenters();

            if(departureServiceCentreId.Length > 1)
                throw new GenericException("This user is assign to more than one(1) Service Centre  ");
            
            var zone = await _routeZone.GetZone(departureServiceCentreId[0], pricingDto.DestinationServiceCentreId);

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId);

            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

            return shipmentTotalPrice;
        }
        
        

        private async Task<decimal> GetRegularPrice(PricingDTO pricingDto)
        {
            //Get Login User and Service Centre User belong 
            var currentUser = await _userService.GetCurrentUserId();

            var departureServiceCentreId = await _userService.GetPriviledgeServiceCenters();

            if (departureServiceCentreId.Length > 1)
                throw new GenericException("This user is assign to more than one(1) Service Centre  ");

            decimal PackagePrice;

            var zone = await _routeZone.GetZone(departureServiceCentreId[0], pricingDto.DestinationServiceCentreId);
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId);

            //This is our limit weight. This will be deleted once limit Price Setting is approve
            var activeWeightLimit = await _weightLimit.GetActiveWeightLimits();
           
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
            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, weight);
            return PackagePrice;
        }

        private async Task<decimal> GetRegularPriceOverflow(decimal weight, decimal activeWeightLimit, int zoneId)
        {
            var activeZone = await _weightLimitPrice.GetWeightLimitPriceByZoneId(zoneId);
                       
            decimal weightlimitZonePrice = activeZone.Price; //Limit Price addition base on zone
            decimal additionalWeight = activeZone.Weight; //Addtional Weight Divisor

            decimal weightDifferent = weight - activeWeightLimit;

            decimal weightLimitPrice = (weightDifferent / additionalWeight) * weightlimitZonePrice;

            decimal PackagePrice = await _regular.GetDomesticZonePrice(zoneId, activeWeightLimit);

            decimal shipmentTotalPrice = PackagePrice + weightLimitPrice;

            return shipmentTotalPrice;
        }
                     
    }
}
