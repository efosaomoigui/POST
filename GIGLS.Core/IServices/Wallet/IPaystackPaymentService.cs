using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayStack.Net;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IPaystackPaymentService : IServiceDependencyMarker 
    {
        Task<bool> MakePayment(string LiveSecret, WalletPaymentLogDTO wpd);
        Task<bool> VerifyPayment(string reference, string livesecret);  
    }
}
