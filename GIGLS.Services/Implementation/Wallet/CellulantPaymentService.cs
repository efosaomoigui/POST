using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CellulantPaymentService : ICellulantPaymentService
    {
        private readonly IUnitOfWork _uow;

        public CellulantPaymentService( IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public Task<TransferDetailsDTO> GetAllTransferDetails(string reference)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria baseFilter)
        {
            var transferDetailsDto = _uow.TransferDetails.GetTransferDetails(baseFilter);
            return transferDetailsDto;
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber)
        {
            var transferDetailsDto = _uow.TransferDetails.GetTransferDetailsByAccountNumber(accountNumber);
            return transferDetailsDto;
        }
    }
}
