using GIGLS.Core.DTO.OnlinePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IKorapayPaymentService : IServiceDependencyMarker
    {
        Task<string> Encrypt(KorapayWebhookDTO payload);
        Task<string> InitializeCharge(KoarapayInitializeCharge payload);
        Task<bool> VerifyAndValidatePaymentForWebhook(KorapayWebhookDTO webhook);
        Task<KorapayQueryChargeResponse> QueryCharge(string reference);
    }
}
