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

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId, pricingDto.CountryId, pricingDto.Weight);

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
            var haulageDistanceMapping = await _haulageDistanceMappingService.GetHaulageDistanceMapping(departureServiceCentreId, destinationServiceCentreId);
            var distance = haulageDistanceMapping.Distance;

            //set the default distance to 1
            if (distance == 0)
            {
                distance = 1;
            }

            //Get Haulage Maximum Fixed Distance
            var userActiveCountryId = await GetUserCountryId();
            var maximumFixedDistanceObj = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.HaulageMaximumFixedDistance, userActiveCountryId);
            int maximumFixedDistance = int.Parse(maximumFixedDistanceObj.Value);

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
            decimal deliveryOptionPrice = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);

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


            // return PackagePrice + deliveryOptionPrice;
            return (PackagePrice + percentagePrice + warSurchargePrice);
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
                    CountryId = pricingDto.CountryId
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

            if (pricingDto.DepartureStationId == pricingDto.DestinationStationId)
            {
                pricingDto.DeliveryOptionId = 3;
            }
            else
            {
                pricingDto.DeliveryOptionId = 2;
            }
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
            return PackagePrice + deliveryOptionPrice;
        }

        public async Task<decimal> GetMobileEcommercePrice(PricingDTO pricingDto)
        {


            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;

            if (pricingDto.DepartureStationId == pricingDto.DestinationStationId)
            {
                pricingDto.DeliveryOptionId = 3;
            }
            else
            {
                pricingDto.DeliveryOptionId = 2;
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

        public async Task<decimal> GetMobileSpecialPrice(PricingDTO pricingDto)
        {

            var zone = await _routeZone.GetZoneMobile(pricingDto.DepartureStationId, pricingDto.DestinationStationId);

            //get the deliveryOptionPrice from an array
            decimal deliveryOptionPriceTemp = 0;

            if (pricingDto.DepartureStationId == pricingDto.DestinationStationId)
            {
                pricingDto.DeliveryOptionId = 3;
            }
            else
            {
                pricingDto.DeliveryOptionId = 2;
            }

            decimal PackagePrice = await _special.GetSpecialZonePrice(pricingDto.SpecialPackageId, zone.ZoneId, pricingDto.CountryId, pricingDto.Weight);

            //get the deliveryOptionPrice from an array
            deliveryOptionPriceTemp = await _optionPrice.GetDeliveryOptionPrice(pricingDto.DeliveryOptionId, zone.ZoneId, pricingDto.CountryId);


            decimal deliveryOptionPrice = deliveryOptionPriceTemp;

            decimal shipmentTotalPrice = deliveryOptionPrice + PackagePrice;

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

    }
}
