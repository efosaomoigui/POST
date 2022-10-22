using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Stores
{
    public interface IServiceCenterPackageRepository : IRepository<ServiceCenterPackage>
    {
        Task<List<ServiceCenterPackageDTO>> GetShipmentPackageForServiceCenter(int[] serviceCenterIds);
    }
}
