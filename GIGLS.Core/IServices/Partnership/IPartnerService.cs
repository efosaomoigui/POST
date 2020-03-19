using GIGLS.Core.DTO.Partnership;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Partnership
{
    public interface IPartnerService : IServiceDependencyMarker
    {
        Task<IEnumerable<PartnerDTO>> GetPartners();
        Task<PartnerDTO> GetPartnerById(int partnerId);
        Task<object> AddPartner(PartnerDTO partner);
        Task UpdatePartner(int partnerId, PartnerDTO partner);
        Task RemovePartner(int partnerId);
        Task<List<PartnerDTO>> GetPartnersByDate(BaseFilterCriteria filterCriteria);
        Task<IEnumerable<PartnerDTO>> GetExternalDeliveryPartners();
        Task<IEnumerable<VehicleTypeDTO>> GetVerifiedPartners();
    }
}
