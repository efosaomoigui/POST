using GIGLS.Core.DTO;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface ICellulantPaymentService : IServiceDependencyMarker
    {
        Task<string> GetCellulantKey();
        Task<string> DecryptKey(string encrytedKey);
        Task<bool> AddCellulantTransferDetails(TransferDetailsDTO transferDetailsDTO);
        Task<TransferDetailsDTO> GetAllTransferDetails(string reference);
        Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria baseFilter);
        Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber);
        Task<CellulantResponseDTO> CheckoutEncryption(CellulantPayloadDTO payload);
    }
}
