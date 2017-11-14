using GIGLS.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Partnership
{
    public interface IPartnerApplicationService : IServiceDependencyMarker
    {
        Task<IEnumerable<PartnerApplicationDTO>> GetPartnerApplications();
        Task<PartnerApplicationDTO> GetPartnerApplicationById(int partnerApplicationId);
        Task<object> AddPartnerApplication(PartnerApplicationDTO partnerApplication);
        Task UpdatePartnerApplication(int partnerApplicationId, PartnerApplicationDTO partnerApplication);
        Task ApprovePartnerApplication(int partnerApplicationId, PartnerApplicationDTO partnerApplication);
        Task RemovePartnerApplication(int partnerApplicationId);
    }
}
