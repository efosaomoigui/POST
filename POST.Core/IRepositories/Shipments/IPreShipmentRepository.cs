using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IPreShipmentRepository : IRepository<PreShipment>
    {
        IQueryable<PreShipment> PreShipmentsAsQueryable();
        Task<List<PreShipmentDTO>> GetDropOffsForUser(ShipmentCollectionFilterCriteria filterCriteria, string currentUserId);
        Task<List<PreShipmentDTO>> GetDropOffsForUserByUserCodeOrPhoneNo(SearchOption searchOption);
    }
}
