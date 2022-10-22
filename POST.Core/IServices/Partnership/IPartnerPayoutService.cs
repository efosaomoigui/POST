using POST.Core.DTO.Partnership;
using System.Threading.Tasks;

namespace POST.Core.IServices.Partnership
{
    public interface IPartnerPayoutService : IServiceDependencyMarker
    {
        Task<object> AddPartnerPayout(PartnerPayoutDTO partnerPayoutDTO);
    }
}
