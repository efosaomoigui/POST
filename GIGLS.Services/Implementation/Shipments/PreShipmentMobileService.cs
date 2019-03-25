using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Domain;
using AutoMapper;


using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Services.Implementation.Shipments
{
    public class PreShipmentMobileService : IPreShipmentMobileService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
        private readonly IDeliveryOptionService _deliveryService;
        private readonly IServiceCentreService _centreService;
        private readonly IUserServiceCentreMappingService _userServiceCentre;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPricingService _pricingService;
        private readonly IWalletService _walletService;

        public PreShipmentMobileService(IUnitOfWork uow,
            IShipmentService shipmentService,
            IDeliveryOptionService deliveryService,
            IServiceCentreService centreService,
            IUserServiceCentreMappingService userServiceCentre,
            INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPricingService pricingService,
            IWalletService walletService

            )
        {
            _uow = uow;
            _shipmentService = shipmentService;
            _deliveryService = deliveryService;
            _centreService = centreService;
            _userServiceCentre = userServiceCentre;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _pricingService = pricingService;
            _walletService = walletService;

            MapperConfig.Initialize();
        }
        public Task<PreShipmentMobileDTO> AddPreShipmentMobile(PreShipmentMobileDTO preShipment)
        {
            try
            {
                var newPreShipment = CreatePreShipment(preShipment);
                _uow.CompleteAsync();

                //scan the shipment for tracking
                //await ScanPreShipment(new ScanDTO
                //{
                //    WaybillNumber = newPreShipment.Waybill,
                //    ShipmentScanStatus = ShipmentScanStatus.PRECRT
                //});

                //send message
                //_messageSenderService.SendMessage(MessageType.PreShipmentCreation, EmailSmsType.All, preShipmentDTO);

                return newPreShipment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<PreShipmentMobileDTO> CreatePreShipment(PreShipmentMobileDTO preShipmentDTO)
        {
            // get the current user info
           
            var wallet = await _walletService.GetWalletByCustomerCode(preShipmentDTO.CustomerCode);
            if (wallet.Balance > Convert.ToDecimal(preShipmentDTO.CalculatedTotal))
            {
                var price = (wallet.Balance - Convert.ToDecimal(preShipmentDTO.CalculatedTotal));
                var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber);
                preShipmentDTO.Waybill = waybill;
                var newPreShipment = Mapper.Map<PreShipmentMobile>(preShipmentDTO);
                newPreShipment.DeliveryPrice = preShipmentDTO.DeliveryPrice;
                newPreShipment.IsConfirmed = false;
                newPreShipment.InsuranceValue = preShipmentDTO.InsuranceValue;
                newPreShipment.Vat = preShipmentDTO.Vat;
                newPreShipment.CalculatedTotal = preShipmentDTO.CalculatedTotal;
                _uow.PreShipmentMobile.Add(newPreShipment);
                var Updatedwallet = await _uow.Wallet.GetAsync(wallet.WalletId);
                Updatedwallet.Balance = price;
                await _uow.CompleteAsync();
                return preShipmentDTO;
            }
           else if(wallet.Balance < preShipmentDTO.Total)
            {
                throw new GenericException("Insufficient Wallet Balance");
            }
            return new PreShipmentMobileDTO();
        }

        public async Task<PreShipmentMobileDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
           
            var Price = 0.0M;
            decimal DeclaredValue = 0.0M;
            foreach (var preShipmentItem in preShipment.PreShipmentItems)
            {
              var PriceDTO = new PricingDTO
                {
                    DepartureStationId = preShipment.SenderStationId,
                    DestinationStationId = preShipment.ReceiverStationId,
                    Weight = (decimal)preShipmentItem.Weight
                };
                preShipmentItem.CalculatedPrice = await _pricingService.GetMobileRegularPrice(PriceDTO);
                Price += (decimal)preShipmentItem.CalculatedPrice;

                if (!string.IsNullOrEmpty(preShipmentItem.Value))
                {
                    DeclaredValue += Convert.ToDecimal(preShipmentItem.Value);
                    preShipment.IsdeclaredVal = true;
                }
            };
            int EstimatedDeclaredPrice = Convert.ToInt32(DeclaredValue);
            preShipment.DeliveryPrice = Price;
            preShipment.Vat = (decimal)(Convert.ToInt32(preShipment.DeliveryPrice) * 0.05);
            preShipment.InsuranceValue = (decimal)(EstimatedDeclaredPrice * 0.01);
            preShipment.CalculatedTotal = (double)(preShipment.DeliveryPrice + preShipment.Vat + preShipment.InsuranceValue);
            preShipment.CalculatedTotal = Math.Round((double)preShipment.CalculatedTotal / 100d, 0) * 100;
            preShipment.Value = DeclaredValue;
            return preShipment;
            
        }
    }

       
    
}
