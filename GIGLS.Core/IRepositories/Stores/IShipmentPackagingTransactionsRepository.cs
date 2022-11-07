using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Report;
using POST.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Stores
{
    public interface IShipmentPackagingTransactionsRepository : IRepository<ShipmentPackagingTransactions>
    {
        Task<List<ShipmentPackagingTransactionsDTO>> GetShipmentPackageTransactions(BankDepositFilterCriteria filterCriteria, int[] serviceCenterIds);
    }
}
