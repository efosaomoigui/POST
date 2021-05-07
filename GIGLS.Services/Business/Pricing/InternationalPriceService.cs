using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Zone;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Pricing
{
    public class InternationalPriceService : IInternationalPriceService
    {
        private readonly ICountryRouteZoneMapService _countryMapService;
        private readonly IDomesticZonePriceService _domesticZonePriceService;

        public InternationalPriceService(ICountryRouteZoneMapService countryRouteZoneMapService, IDomesticZonePriceService domesticZonePriceService)
        {
            _countryMapService = countryRouteZoneMapService;
            _domesticZonePriceService = domesticZonePriceService;
        }

        public async Task<InternationalShipmentDTO> GetPrice(InternationalShipmentDTO shipmentDTO, CompanyMap companyMap)
        {
            switch (companyMap)
            {
                case CompanyMap.UPS:
                    return await GetUPSPrice(shipmentDTO, companyMap);
                default:
                    break;
            }
            return null;
        }

        private async Task<InternationalShipmentDTO> GetUPSPrice(InternationalShipmentDTO shipmentDTO, CompanyMap companyMap)
        {
            //Get country zone
            var zone = await _countryMapService.GetBasicZone(shipmentDTO.DepartureCountryId, shipmentDTO.DestinationCountryId, companyMap);

            //get price
            foreach (var item in shipmentDTO.ShipmentItems)
            {
                var itemType = item.ItemCategory == InternationalShipmentItemCategory.NonDocument ? RegularEcommerceType.UPSNonDocument : RegularEcommerceType.UPSDocument;
                decimal weight = (decimal)item.Weight;
                var price = await _domesticZonePriceService.GetDomesticZonePrice(zone.ZoneId, weight, itemType, shipmentDTO.DepartureCountryId);
                item.Price = price;
                shipmentDTO.GrandTotal = shipmentDTO.GrandTotal + price;
            }
            return shipmentDTO;
        }
    }
}
