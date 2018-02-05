using GIGLS.Core.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentCancelService : IShipmentCancelService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ShipmentCancelService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddShipmentCancel(string waybill)
        {
            if (await _uow.ShipmentCancel.ExistAsync(v => v.Waybill == waybill))
            {
                throw new GenericException($"Shipment with waybill {waybill} already cancelled");
            }

            var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);

            if (shipment == null)
            {
                throw new GenericException($"Shipment with waybill {waybill} does not exist");
            }

            var user = await _userService.GetCurrentUserId();

            var newCancel = new ShipmentCancel
            {
                Waybill = shipment.Waybill,
                CreatedBy = shipment.UserId,
                ShipmentCreatedDate = shipment.DateCreated,
                CancelledBy = user
            };

            _uow.ShipmentCancel.Add(newCancel);
            await _uow.CompleteAsync();
            return new { waybill = newCancel.Waybill };
        }

        public async Task<ShipmentCancelDTO> GetShipmentCancelById(string waybill)
        {
            var shipment = await _uow.ShipmentCancel.GetAsync(x => x.Waybill == waybill);

            if (shipment == null)
            {
                throw new GenericException($"Shipment with waybill {waybill} does not exist");
            }

            return Mapper.Map<ShipmentCancelDTO>(shipment);
        }

        public async Task<List<ShipmentCancelDTO>> GetShipmentCancels()
        {
            return await _uow.ShipmentCancel.GetShipmentCancels();
        }
    }
}
