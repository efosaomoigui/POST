using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Partnership
{
    public interface IPartnerRepository : IRepository<Partner>
    {
        Task<List<PartnerDTO>> GetPartnersAsync();
        Task<Partner> GetLastValidPartnerCode();
        Task<PartnerDTO> GetPartnerByIdWithCountry(int customerId);
        Task<List<PartnerDTO>> GetExternalPartnersAsync();
        Task<PartnerDTO> GetPartnerBySearchParameters(string parameter);
    }
}
