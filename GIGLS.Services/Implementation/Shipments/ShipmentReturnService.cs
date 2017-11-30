using GIGLS.CORE.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.CORE.Domain;
using System;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Shipments;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentReturnService : IShipmentReturnService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentCollectionService _collectionService;

        public ShipmentReturnService(IUnitOfWork uow, IShipmentService shipmentService, IShipmentCollectionService collectionService)
        {
            _uow = uow;
            _shipmentService = shipmentService;
            _collectionService = collectionService;
            MapperConfig.Initialize();
        }

        public async Task AddShipmentReturn(ShipmentReturnDTO shipmentReturn)
        {            
            if (await _uow.ShipmentReturn.ExistAsync(v => v.WaybillNew.Equals(shipmentReturn.WaybillNew) && v.WaybillOld.Equals(shipmentReturn.WaybillOld)))
            {
                throw new GenericException($"Waybill {shipmentReturn.WaybillNew} and {shipmentReturn.WaybillOld} already exist");
            }
            var newReturns = Mapper.Map<ShipmentReturn>(shipmentReturn);

            _uow.ShipmentReturn.Add(newReturns);
            await _uow.CompleteAsync();
        }

        public async Task AddShipmentReturn(string waybill)
        {          
            try
            {
                var returnShipment = await _uow.ShipmentReturn.GetAsync(x => x.WaybillOld.Equals(waybill));

                if (returnShipment != null)
                {
                    throw new GenericException($"Shipment with waybill: {waybill} already processed for Returns");
                }

                //var shipmentCollection = await _collectionService.GetShipmentCollectionById(waybill);
                //if (shipmentCollection.ShipmentScanStatus == ShipmentScanStatus.Collected)
                //{
                //    throw new GenericException($"Shipment with waybill: {waybill} had been collected");
                //}                               
                
                await _collectionService.CheckShipmentCollection(waybill);

                var shipment = await _shipmentService.GetShipment(waybill);

                int departure = shipment.DepartureServiceCentreId; 

                shipment.DepartureServiceCentreId = shipment.DestinationServiceCentreId;
                shipment.DestinationServiceCentreId = departure;

                var newShipment = await _shipmentService.AddShipment(shipment);

                var newShipmentReturn = new ShipmentReturn
                {
                    WaybillNew = newShipment.Waybill,
                    WaybillOld = waybill,
                    OriginalPayment = newShipment.GrandTotal,
                    //Discount =                
                };
                
                _uow.ShipmentReturn.Add(newShipmentReturn);
                await _uow.CompleteAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShipmentReturnDTO> GetShipmentReturnById(string waybill)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(waybill));

            if (shipmentReturn == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does Not Exist");
            }
            return Mapper.Map<ShipmentReturnDTO>(shipmentReturn);
        }

        public Task<IEnumerable<ShipmentReturnDTO>> GetShipmentReturns()
        {
            var shipmentReturns = _uow.ShipmentReturn.GetAll();
            var shipmentReturnsDto = Mapper.Map<IEnumerable<ShipmentReturnDTO>>(shipmentReturns);
            return Task.FromResult(shipmentReturnsDto);
        }

        public async Task RemoveShipmentReturn(string waybill)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(waybill));

            if (shipmentReturn == null)
            {
                throw new GenericException("INFORMATION DOES NOT EXIST");
            }
            _uow.ShipmentReturn.Remove(shipmentReturn);
            await _uow.CompleteAsync();
        }

        public async Task UpdateShipmentReturn(ShipmentReturnDTO shipmentReturnDto)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(shipmentReturnDto.WaybillNew));

            if (shipmentReturn == null)
            {
                throw new GenericException("INFORMATION DOES NOT EXIST");
            }

            shipmentReturn.WaybillNew = shipmentReturnDto.WaybillNew;
            shipmentReturn.WaybillOld = shipmentReturnDto.WaybillOld;
            shipmentReturn.Discount = shipmentReturnDto.Discount;
            shipmentReturn.OriginalPayment = shipmentReturnDto.OriginalPayment;
            await _uow.CompleteAsync();
        }
    }
}
