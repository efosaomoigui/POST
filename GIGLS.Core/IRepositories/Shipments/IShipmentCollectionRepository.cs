using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Report;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.CORE.IRepositories.Shipments
{
    public interface IShipmentCollectionRepository : IRepository<ShipmentCollection>
    {
        IQueryable<ShipmentCollectionDTO> ShipmentCollectionsForEcommerceAsQueryable(bool isEcommerce);
        Task<List<ShipmentCollectionForContactDTO>> GetShipmentCollectionForContact(ShipmentContactFilterCriteria baseFilterCriteria);
        Task<List<ShipmentCollectionDTOForArrived>> GetArrivedShipmentCollection(ShipmentContactFilterCriteria baseFilterCriteria);
    }
}
