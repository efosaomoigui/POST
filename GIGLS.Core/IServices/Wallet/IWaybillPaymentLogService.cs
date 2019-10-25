using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IWaybillPaymentLogService : IServiceDependencyMarker
    {
        Task<IEnumerable<WaybillPaymentLogDTO>> GetWaybillPaymentLogs();
        Tuple<Task<List<WaybillPaymentLogDTO>>> GetWaybillPaymentLogs(FilterOptionsDto filterOptionsDto);
        Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByReference(string reference);
        Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByWaybill(string waybill);
        Task<List<WaybillPaymentLogDTO>> GetWaybillPaymentLogListByWaybill(string waybill);
        Task<PaymentInitiate> AddWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog);
        Task UpdateWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog);
    }
}
