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
        Task<string> Encrpt(KorapayWebhookDTO payload);
        Task<string> InitializeCharge(KoarapayInitializeCharge payload);
        

    }
}
