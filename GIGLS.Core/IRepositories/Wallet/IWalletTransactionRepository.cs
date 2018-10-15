using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface IWalletTransactionRepository : IRepository<WalletTransaction>
    {
        Task<List<WalletTransactionDTO>> GetWalletTransactionAsync(int[] serviceCentreIds);
        Task<List<WalletTransactionDTO>> GetWalletTransactionCreditAsync(int[] serviceCentreIds, AccountFilterCriteria accountFilterCriteria);
    }
}
