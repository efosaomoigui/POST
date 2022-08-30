using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IAzapayPaymentService :  IServiceDependencyMarker
    {
        Task<bool> AddAzaPayTransferDetails(AzapayTransferDetailsDTO transferDetailsDTO);
        Task<ValidateTimedAccountResponseDTO> ValidateTimedAccountRequest(string accountnumber);
        Task<GetTransactionHistoryResponseDTO> GetTransactionHistoryRequest();
        Task<InitiateTimedAccountResponseDTO> InitiateTimedAccountRequest(InitiateTimedAccountRequestDTO payload);
        Task<AzapayTransferResponseDTO> AzapayTransferRequest(AzapayTransferRequestDTO payload);
    }
}
