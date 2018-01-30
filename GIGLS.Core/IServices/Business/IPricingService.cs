using GIGLS.Core.DTO.PaymentTransactions;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IPricingService : IServiceDependencyMarker
    {
        //Task<decimal> GetRegularPrice(PricingDTO pricingDto);
        //Task<decimal> GetSpecialPrice(PricingDTO pricingDto);
        Task<decimal> GetPrice(PricingDTO pricingDto);
        Task<decimal> GetHaulagePrice(HaulagePricingDTO pricingDto);

        Task<decimal> GetEcommerceReturnPrice(PricingDTO pricingDto);
        
    }
}
