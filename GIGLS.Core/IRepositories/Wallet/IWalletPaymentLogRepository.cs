using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface IWalletPaymentLogRepository : IRepository<WalletPaymentLog>
    {
        Task<List<WalletPaymentLogDTO>> GetWalletPaymentLogs();
    }
}
