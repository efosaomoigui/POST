using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
 
namespace GIGLS.Core.IRepositories.Magaya
{
    public interface IMagayaShipmentRepository : IRepository<MagayaShipmentAgility> 
    {
        //Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);

    }
}
