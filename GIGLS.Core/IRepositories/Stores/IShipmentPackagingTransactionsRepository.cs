using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Stores;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Stores
{
    public interface IShipmentPackagingTransactionsRepository : IRepository<ShipmentPackagingTransactions>
    {
        Task<List<ShipmentPackagingTransactionsDTO>> GetShipmentPackageTransactions(BaseFilterCriteria filterCriteria, int[] serviceCenterIds);
    }
}
