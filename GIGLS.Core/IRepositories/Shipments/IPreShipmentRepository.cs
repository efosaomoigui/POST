using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IPreShipmentRepository : IRepository<PreShipment>
    {
        IQueryable<PreShipment> PreShipmentsAsQueryable();
        Task<List<PreShipmentDTO>> GetDropOffsForUser(ShipmentCollectionFilterCriteria filterCriteria, string currentUserId);
        Task<List<PreShipmentDTO>> GetDropOffsForUserByUserCodeOrPhoneNo(SearchOption searchOption);
    }
}
