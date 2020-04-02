using GIGLS.Core.DTO.Partnership;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Partnership
{
    public interface IPartnerPayoutService : IServiceDependencyMarker
    {
        Task<object> AddPartnerPayout(PartnerPayoutDTO partnerPayoutDTO);
    }
}
