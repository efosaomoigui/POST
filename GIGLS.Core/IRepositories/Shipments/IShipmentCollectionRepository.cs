using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Report;
using POST.CORE.Domain;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.CORE.IRepositories.Shipments
{
    public interface IShipmentCollectionRepository : IRepository<ShipmentCollection>
    {
        IQueryable<ShipmentCollectionDTO> ShipmentCollectionsForEcommerceAsQueryable(bool isEcommerce);
        Task<List<ShipmentCollectionForContactDTO>> GetShipmentCollectionForContact(ShipmentContactFilterCriteria baseFilterCriteria);
        Task<List<ShipmentCollectionDTOForArrived>> GetArrivedShipmentCollection(ShipmentContactFilterCriteria baseFilterCriteria);
    }
}
