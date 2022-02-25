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
        Task<CreateStellaAccounResponsetDTO> CreateStellasAccount(CreateStellaAccountDTO createStellaAccountDTO);
        Task<GetCustomerBalanceDTO> GetCustomerStellasAccount(string accountNo);


    }

}
