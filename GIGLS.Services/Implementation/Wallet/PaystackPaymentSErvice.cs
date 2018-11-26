using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.Enums;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayStack.Net;
using System.Configuration;

namespace GIGLS.Services.Implementation.Wallet
{
    public class PaystackPaymentService : IPaystackPaymentService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public PaystackPaymentService(IUserService userService, IUnitOfWork uow)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<bool> MakePayment(string LiveSecret, WalletPaymentLogDTO wpd)
        {
            var api = new PayStackApi(LiveSecret);

            // Initializing a transaction
            var response = api.Transactions.Initialize(wpd.Email, wpd.PaystackAmount);
           
            //return response.Status;
            return response.Status;

        }

        public async Task<bool> VerifyPayment(string reference, string livesecret)
        {
            var api = new PayStackApi(livesecret);

            // Verifying a transaction
            var verifyResponse = api.Transactions.Verify(reference); 

            return verifyResponse.Status;
        }

    }

}
