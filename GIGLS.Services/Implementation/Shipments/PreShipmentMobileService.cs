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
            if(!preShipmentDTO.Value.Equals(null))
            {
                newPreShipment.IsdeclaredVal = true;
            }
            foreach (var preShipmentItem in newPreShipment.PreShipmentItems)
            {
                preShipmentItem.SerialNumber = serialNumber;
                serialNumber++;
                var PriceDTO = new PricingDTO
                {
                    DepartureStationId = newPreShipment.SenderLocationId,
                    DestinationStationId = newPreShipment.ReceiverLocationId,
                    Weight = (decimal)preShipmentItem.Weight
                };
                Price =+ await _pricingService.GetMobileRegularPrice(PriceDTO);
            };
            preShipmentDTO.CalculatedTotal = Price;
            //save the display value of Insurance and Vat
            newPreShipment.Vat = preShipmentDTO.vatvalue_display;
            newPreShipment.DiscountValue = preShipmentDTO.InvoiceDiscountValue_display;
            newPreShipment.CalculatedTotal = Price;
            newPreShipment.IsConfirmed = false;


            _uow.PreShipmentMobile.Add(newPreShipment);
            await _uow.CompleteAsync();

            return preShipmentDTO;
        }

       
    }

       
    
}
