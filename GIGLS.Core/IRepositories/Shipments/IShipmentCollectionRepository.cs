using GIGL.GIGLS.Core.Repositories;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using System.Linq;

namespace GIGLS.CORE.IRepositories.Shipments
{
    public interface IShipmentCollectionRepository : IRepository<ShipmentCollection>
    {
        IQueryable<ShipmentCollectionDTO> ShipmentCollectionsForEcommerceAsQueryable(bool isEcommerce);
    }
}
