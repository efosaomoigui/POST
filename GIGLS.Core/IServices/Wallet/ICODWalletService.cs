using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface ICODWalletService : IServiceDependencyMarker
    {
        Task<StellasResponseDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO);
        Task<StellasResponseDTO> GetStellasAccountBal(string customerCode);
        Task<bool> CheckIfUserHasCODWallet(string customerCode);
        Task<StellasResponseDTO> ValidateBVNNumber(ValidateCustomerBVN payload);
    }

}
