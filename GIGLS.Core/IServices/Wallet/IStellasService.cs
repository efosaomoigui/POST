using POST.Core.Domain.Wallet;
using POST.Core.DTO;
using POST.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IServices.Wallet
{
    public interface IStellasService : IServiceDependencyMarker
    {
        Task<StellasResponseDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO);
        Task<StellasResponseDTO> GetCustomerStellasAccount(string accountNo);
        Task<StellasResponseDTO> GetBanks();
        Task<StellasResponseDTO> StellasWithdrawal(StellasWithdrawalDTO createStellaAccountDTO);
        Task<StellasResponseDTO> StellasValidateBankName(ValidateBankNameDTO validateBankNameDTO);
        Task<StellasResponseDTO> StellasTransfer(StellasTransferDTO createStellaAccountDTO);
        Task<StellasResponseDTO> ValidateBVNNumber(ValidateCustomerBVN payload);
        Task<StellasResponseDTO> CreateAccountOnCoreBanking(CreateAccountCoreBankingDTO payload);
    }
}
