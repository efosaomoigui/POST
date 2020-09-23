using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentItemRepository : IRepository<ShipmentItem>
    {
    }

    public interface IIntlShipmentRequestItemRepository : IRepository<IntlShipmentRequestItem>
    {
    }
}
