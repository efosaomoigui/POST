using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Partnership;
using POST.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Partnership
{
    public interface IPartnerApplicationRepository : IRepository<PartnerApplication>
    {
        Task<List<PartnerApplicationDTO>> GetPartnerApplicationsAsync();
    }
}
