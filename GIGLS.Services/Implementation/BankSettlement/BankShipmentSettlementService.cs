using GIGLS.Core.IServices.CashOnDeliveryBalance;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Wallet;
using System.Linq;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.BankSettlement;

namespace GIGLS.Services.Implementation.Wallet
{
    public class BankShipmentSettlementService : IBankShipmentSettlementService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly IUserService _userService;

        public BankShipmentSettlementService(IUnitOfWork uow, IWalletService walletService, IUserService userService)
        {
            _uow = uow;
            _walletService = walletService;
            _userService = userService;
            MapperConfig.Initialize();
        }



        public async Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement()
        {
            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var allShipments = _uow.Invoice.GetAllFromInvoiceView();
            allShipments = allShipments.Where(s => s.PaymentMethod == "Cash" && s.PaymentStatus == PaymentStatus.Paid);

            //added for GWA and GWARIMPA service centres
            {
                if (serviceCenters.Length == 1)
                {
                    if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                    {
                        serviceCenters = new int[] { 4, 294 };
                    }
                }
            }

            var cashShipments = new List<GIGLS.Core.DTO.Account.InvoiceViewDTO>();
            if (serviceCenters.Length > 0)
            {
                var shipmentResult = allShipments.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId)).ToList();
                cashShipments = Mapper.Map<List<GIGLS.Core.DTO.Account.InvoiceViewDTO>>(shipmentResult);
            }

            return await Task.FromResult(cashShipments);
        }

    }
}
