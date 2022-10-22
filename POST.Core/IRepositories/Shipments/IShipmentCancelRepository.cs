using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IShipmentCancelRepository : IRepository<ShipmentCancel>
    {
        Task<List<ShipmentCancelDTO>> GetShipmentCancels();
        Task<List<ShipmentCancelDTO>> GetShipmentCancels(ShipmentCollectionFilterCriteria collectionFilterCriteria);
        Task<ShipmentCancelDTO> GetShipmentCancels(string waybill);
    }
}