using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Partnership
{
    public interface IPartnerApplicationRepository : IRepository<PartnerApplication>
    {
        Task<List<PartnerApplicationDTO>> GetPartnerApplicationsAsync();
    }
}
