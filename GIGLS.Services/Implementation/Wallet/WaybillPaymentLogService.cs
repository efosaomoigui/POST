using GIGLS.Core;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WaybillPaymentLogService : IWaybillPaymentLogService
    {
        private readonly IUnitOfWork _uow;

        public WaybillPaymentLogService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<object> AddWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            throw new NotImplementedException();
        }

        public Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByReference(string reference)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WaybillPaymentLogDTO>> GetWaybillPaymentLogs()
        {
            throw new NotImplementedException();
        }

        public Tuple<Task<List<WaybillPaymentLogDTO>>> GetWaybillPaymentLogs(FilterOptionsDto filterOptionsDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            throw new NotImplementedException();
        }
    }
}
