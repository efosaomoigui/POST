using POST.Core.Domain.Wallet;
using POST.Core.DTO;
using POST.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IServices.Wallet
{
    public interface ICODWalletService : IServiceDependencyMarker
    {
        Task<StellasResponseDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO0);
        Task<StellasResponseDTO> GetStellasAccountBal(string customerCode);
        Task<StellasResponseDTO> GetStellasBanks();
        Task<StellasResponseDTO> StellasWithdrawal(StellasWithdrawalDTO stellasWithdrawalDTO);
        Task<StellasResponseDTO> StellasValidateBankName(ValidateBankNameDTO validateBankNameDTO);
        Task<StellasResponseDTO> StellasTransfer(StellasTransferDTO transferDTO);
        Task<bool> CheckIfUserHasCODWallet(string customerCode);
        Task<StellasResponseDTO> ValidateBVNNumber(ValidateCustomerBVN payload);
        Task AddCODWalletLoginDetails(string customerCode, string userName, string password);
        Task<LoginDetailsDTO> GetStellasAccountLoginDetails(string customerCode);
    }

}
