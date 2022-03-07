using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IStellasService : IServiceDependencyMarker
    {
        Task<StellasResponseDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO);
        Task<StellasResponseDTO> GetCustomerStellasAccount(string accountNo);
        Task<StellasResponseDTO> GetBanks();
        Task<StellasResponseDTO> StellasWithdrawal(StellasWithdrawalDTO createStellaAccountDTO);
        Task<StellasResponseDTO> StellasValidateBankName(ValidateBankNameDTO validateBankNameDTO);
        Task<StellasResponseDTO> StellasTransfer(StellasTransferDTO createStellaAccountDTO);


    }

}
