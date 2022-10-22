using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Partnership;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Partnership
{
    public interface IPartnerPayoutRepository : IRepository<PartnerPayout>
    {
        Task<List<PartnerPayoutDTO>> GetPartnerPayoutByDate(ShipmentCollectionFilterCriteria filterCriteria);
    }
    
}
