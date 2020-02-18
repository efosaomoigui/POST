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
        public bool IsPaymentSuccessful { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string NetworkProvider { get; set; }
    }

    //For Mobile Money
    public class MobileMoneyDTO 
    {
        public MobileMoneyDTO()
        {
            mobile_money = new Mobile_Money();
        }

        public string reference { get; set; }

        public decimal amount { get; set; }
        public string currency { get; set; }
        public string email { get; set; }

        public Mobile_Money mobile_money { get; set; }
    }

    public class Mobile_Money
    {
        public string phone { get; set; }
        public string provider { get; set; }
    }
}
