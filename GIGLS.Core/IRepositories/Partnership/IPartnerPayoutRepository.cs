using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Partnership
{
    public interface IPartnerPayoutRepository : IRepository<PartnerPayout>
    {
        Task<List<PartnerPayoutDTO>> GetPartnerPayoutByDate(ShipmentCollectionFilterCriteria filterCriteria);
    }
    
}
