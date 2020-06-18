using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IPricingService : IServiceDependencyMarker
    {
        Task<decimal> GetPrice(PricingDTO pricingDto);
        Task<decimal> GetHaulagePrice(HaulagePricingDTO pricingDto);
        Task<decimal> GetEcommerceReturnPrice(PricingDTO pricingDto);
        Task<decimal> GetInternationalPrice(PricingDTO pricingDto);
        Task<ShipmentDTO> GetReroutePrice(ReroutePricingDTO pricingDto);
        Task<decimal> GetMobileRegularPrice(PricingDTO pricingDto);
        Task<decimal> GetMobileEcommercePrice(PricingDTO pricingDto);
        Task<decimal> GetMobileSpecialPrice(PricingDTO pricingDto);
        Task<decimal> GetCountryCurrencyRatio();
        Task<int> GetUserCountryId();
        Task<decimal> GetDropOffRegularPriceForIndividual(PricingDTO pricingDto);
        Task<decimal> GetDropOffSpecialPrice(PricingDTO pricingDto);
    }
}
