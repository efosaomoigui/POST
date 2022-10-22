using GIGL.POST.Core.Repositories;
using POST.CORE.Domain;
using System.Linq;

namespace POST.CORE.IRepositories.Shipments
{
    public interface IOverdueShipmentRepository : IRepository<OverdueShipment>
    {
        IQueryable<OverdueShipment> GetAllFromOverdueShipmentAsQueryable();
    }
}
