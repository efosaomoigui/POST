using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
 
namespace POST.Core.IRepositories.Magaya
{
    public interface IMagayaShipmentRepository : IRepository<MagayaShipmentAgility> 
    {
        //Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);

    }
}
