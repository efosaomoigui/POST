using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class WaybillPaymentLogDTO : BaseDomainDTO
    {
        public WaybillPaymentLogDTO()
        {
            FlutterWaveData = new FlutterWaveDTO();
        }

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

        public FlutterWaveDTO FlutterWaveData { get; set; }
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

    public class FlutterWaveDTO
    {
        public string PBFPubKey { get; set; }
        public string accountbank { get; set; }
        public string accountnumber { get; set; }
        public string currency { get; set; } = "NGN";
        public string payment_type { get; set; } = "account";
        public string country { get; set; } = "NG";
        public decimal amount { get; set; }
        public string email { get; set; }
        public string passcode { get; set; }
        public string bvn { get; set; }
        public string phonenumber { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string IP { get; set; }
        public string txRef { get; set; }
        public string device_fingerprint { get; set; }
    }

    public class FlutterWaveObject
    {
        public string PBFPubKey { get; set; }
        public string client { get; set; }
        public string alg { get; set; } 
    }

    public class FlutterWaveOTPObject
    {
        public string PBFPubKey { get; set; }
        public string transaction_reference { get; set; }
        public string otp { get; set; }
    }
}
