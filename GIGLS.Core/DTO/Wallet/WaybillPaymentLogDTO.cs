using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class WaybillPaymentLogDTO : BaseDomainDTO
    {
        public int WaybillPaymentLogId { get; set; }

        public string Waybill { get; set; }

        public string Reference { get; set; }

        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionResponse { get; set; }
        public string Currency { get; set; }

        public OnlinePaymentType OnlinePaymentType { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }

        public bool IsWaybillSettled { get; set; }
    }
}
