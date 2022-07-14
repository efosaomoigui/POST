using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
   public  class AzapayPaymentService : IAzapayPaymentService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;
        private IServiceCentreService _serviceCenterService;
        private readonly IWalletService _walletService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly INodeService _nodeService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IStellasService _stellasService;
        public AzapayPaymentService(IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService,
            IWalletService walletService, IPaymentTransactionService paymentTransactionService, INodeService nodeService, IMessageSenderService messageSenderService, IStellasService stellasService)
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _nodeService = nodeService;
            _messageSenderService = messageSenderService;
            _stellasService = stellasService;
            MapperConfig.Initialize();
        }
        public async Task<bool> AddAzaPayTransferDetails(AzapayTransferDetailsDTO transferDetailsDTO)
        {
            try
            {
                if (transferDetailsDTO is null)
                {
                    throw new GenericException("invalid payload", $"{(int)HttpStatusCode.BadRequest}");
                }

                var entity = await _uow.TransferDetails.ExistAsync(x => x.RefId == transferDetailsDTO.RefId);
                if (entity)
                {
                    throw new GenericException($"This transfer details with RefId {transferDetailsDTO.RefId} already exist.", $"{(int)HttpStatusCode.Forbidden}");
                }

                if (transferDetailsDTO.Status == "CONFIRMED")
                {
                    transferDetailsDTO.TransactionStatus = "success";
                }
                else
                {
                    transferDetailsDTO.TransactionStatus = "pending";
                }

                transferDetailsDTO.ProcessingPartner = ProcessingPartnerType.Azapay;
                var transferDetails = Mapper.Map<TransferDetails>(transferDetailsDTO);
                _uow.TransferDetails.Add(transferDetails);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
