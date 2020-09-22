using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Stores
{
    public interface IServiceCenterPackageRepository : IRepository<ServiceCenterPackage>
    {
        Task<List<ServiceCenterPackageDTO>> GetShipmentPackageForServiceCenter(int[] serviceCenterIds);
    }
}
