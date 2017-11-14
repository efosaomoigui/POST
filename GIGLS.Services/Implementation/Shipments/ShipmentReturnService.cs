using GIGLS.CORE.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.CORE.Domain;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentReturnService : IShipmentReturnService
    {
        private readonly IUnitOfWork _uow;

        public ShipmentReturnService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task AddShipmentReturn(ShipmentReturnDTO shipmentReturn)
        {
            shipmentReturn.WaybillNew = shipmentReturn.WaybillNew.Trim().ToLower();
            shipmentReturn.WaybillOld = shipmentReturn.WaybillOld.Trim().ToLower();
            
            if (await _uow.ShipmentReturn.ExistAsync(v => v.WaybillNew.ToLower() == shipmentReturn.WaybillNew && v.WaybillOld.ToLower() == shipmentReturn.WaybillOld))
            {
                throw new GenericException($"Waybill {shipmentReturn.WaybillNew} and {shipmentReturn.WaybillOld} already exist");
            }
            var newReturns = Mapper.Map<ShipmentReturn>(shipmentReturn);

            _uow.ShipmentReturn.Add(newReturns);
            await _uow.CompleteAsync();
        }

        public async Task<ShipmentReturnDTO> GetShipmentReturnById(string waybill)
        {
            var shipmentReturn = await _uow.ShipmentReturn.GetAsync(x => x.WaybillNew.Equals(waybill));

            if (shipmentReturn == null)
            {
                throw new GenericException("INFORMATION DOES NOT EXIST");
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
