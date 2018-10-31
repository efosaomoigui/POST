using GIGL.GIGLS.Core.Repositories;
using GIGLS.CORE.Domain;
using System.Linq;

namespace GIGLS.CORE.IRepositories.Shipments
{
    public interface IOverdueShipmentRepository : IRepository<OverdueShipment>
    {
        IQueryable<OverdueShipment> GetAllFromOverdueShipmentAsQueryable();
    }
}
