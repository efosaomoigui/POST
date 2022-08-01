using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface ITransferDetailsRepository : IRepository<TransferDetails>
    {
        Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria filterCriteria, string crAccount);
        Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber, string crAccount);
        Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria filterCriteria);
        Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber);
        Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria filterCriteria, List<string> crAccounts);
        Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber, List<string> crAccounts);
        Task<List<TransferDetailsDTO>> GetAzapayTransferDetails(BaseFilterCriteria filterCriteria);
        Task<List<TransferDetailsDTO>> GetAzapayTransferDetailsByAccountNumber(string accountNumber);
    }
}
