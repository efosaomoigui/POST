using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Stores
{
    public interface IShipmentPackagingTransactionsRepository : IRepository<ShipmentPackagingTransactions>
    {
        Task<List<ShipmentPackagingTransactionsDTO>> GetShipmentPackageTransactions(BankDepositFilterCriteria filterCriteria, int[] serviceCenterIds);
    }
}
