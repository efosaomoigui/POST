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

        public PreShipmentMobileService(IUnitOfWork uow,
            IShipmentService shipmentService,
            IDeliveryOptionService deliveryService,
            IServiceCentreService centreService,
            IUserServiceCentreMappingService userServiceCentre,
            INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPricingService pricingService

            )
        {
            _uow = uow;
            _shipmentService = shipmentService;
            _deliveryService = deliveryService;
            _centreService = centreService;
            _userServiceCentre = userServiceCentre;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _pricingService = pricingService;

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
            var waybill = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.WaybillNumber);

            preShipmentDTO.Waybill = waybill;
            var newPreShipment = Mapper.Map<PreShipmentMobile>(preShipmentDTO);

            // add serial numbers to the ShipmentItems
            var serialNumber = 1;
            var Price = 0.0M;
            decimal DeclaredValue = 0.0M;
           
            foreach (var preShipmentItem in newPreShipment.PreShipmentItems)
            {
                preShipmentItem.SerialNumber = serialNumber;
                serialNumber++;
                var PriceDTO = new PricingDTO
                {
                    DepartureStationId = newPreShipment.SenderStationId,
                    DestinationStationId = newPreShipment.ReceiverStationId,
                    Weight = (decimal)preShipmentItem.Weight
                };
                preShipmentItem.CalculatedPrice = await _pricingService.GetMobileRegularPrice(PriceDTO);
                Price += (decimal)preShipmentItem.CalculatedPrice;

                if (!string.IsNullOrEmpty(preShipmentItem.Value))
                {
                    DeclaredValue += Convert.ToDecimal(preShipmentItem.Value);
                    preShipmentDTO.IsdeclaredVal = true;
                }
            };
            int EstimatedDeclaredPrice = Convert.ToInt32(DeclaredValue);
            preShipmentDTO.Total = Price;
            preShipmentDTO.Vat = (decimal)(Convert.ToInt32(preShipmentDTO.Total) * 0.05);
            preShipmentDTO.Insurance = (decimal)(EstimatedDeclaredPrice * 0.01);
            preShipmentDTO.CalculatedTotal = (double)(preShipmentDTO.Total + preShipmentDTO.Vat + preShipmentDTO.Insurance);
            preShipmentDTO.CalculatedTotal=Math.Round((double)preShipmentDTO.CalculatedTotal / 100d, 0) * 100;
            preShipmentDTO.Value = DeclaredValue;

            //save the display value of Insurance and Vat
            newPreShipment.Total = preShipmentDTO.Total;
            newPreShipment.IsConfirmed = false;
            newPreShipment.Insurance = preShipmentDTO.Insurance;
            newPreShipment.Vat = preShipmentDTO.Vat;
            newPreShipment.CalculatedTotal = preShipmentDTO.CalculatedTotal;
            newPreShipment.Value = preShipmentDTO.Value;

            _uow.PreShipmentMobile.Add(newPreShipment);
            await _uow.CompleteAsync();

            return preShipmentDTO;
        }

       
    }

       
    
}
