using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;

namespace POST.Core.IRepositories.Shipments
{
    public interface IShipmentItemRepository : IRepository<ShipmentItem>
    {
    }

    public interface IIntlShipmentRequestItemRepository : IRepository<IntlShipmentRequestItem>
    {
    }
}
