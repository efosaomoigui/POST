using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class OverdueShipmentService : IOverdueShipmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public OverdueShipmentService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task AddOverdueShipment(OverdueShipmentDTO overdueShipment)
        {
            overdueShipment.Waybill = overdueShipment.Waybill.Trim().ToLower();

            if (await _uow.OverdueShipment.ExistAsync(v => v.Waybill.ToLower() == overdueShipment.Waybill))
            {
                throw new GenericException($"Waybill {overdueShipment.Waybill} already exist");
            }

            var currentUserId = await _userService.GetCurrentUserId();

            var data = Mapper.Map<OverdueShipment>(overdueShipment);
            data.UserId = currentUserId;

            _uow.OverdueShipment.Add(data);
            await _uow.CompleteAsync();
        }

        public async Task<OverdueShipmentDTO> GetOverdueShipmentById(string waybill)
        {
            var overdueShipment = await _uow.OverdueShipment.GetAsync(x => x.Waybill.Equals(waybill));

            if (overdueShipment == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does not exist");
            }
            return Mapper.Map<OverdueShipmentDTO>(overdueShipment);

        }

        public async Task<IEnumerable<OverdueShipmentDTO>> GetOverdueShipments()
        {
            //get OverdueShipment
            var overdueShipment = _uow.OverdueShipment.GetAll().ToList();

            var overdueShipmentDto = Mapper.Map<IEnumerable<OverdueShipmentDTO>>(overdueShipment);

            return await Task.FromResult(overdueShipmentDto.OrderByDescending(x => x.DateModified));
        }

        public async Task RemoveOverdueShipment(string waybill)
        {
            var overdueShipment = await _uow.OverdueShipment.GetAsync(x => x.Waybill.Equals(waybill));

            if (overdueShipment == null)
            {
                throw new GenericException($"Shipment with waybill: {waybill} does not exist");
            }
            _uow.OverdueShipment.Remove(overdueShipment);
            await _uow.CompleteAsync();
        }

        public async Task UpdateOverdueShipment(OverdueShipmentDTO overdueShipmentDto)
        {
            var overdueShipment = await _uow.OverdueShipment.GetAsync(x => x.Waybill.Equals(overdueShipmentDto.Waybill));

            if (overdueShipment == null)
            {
                throw new GenericException("Shipment does not exist");
            }

            if (overdueShipmentDto.UserId == null)
            {
                overdueShipmentDto.UserId = await _userService.GetCurrentUserId();
            }

            overdueShipment.UserId = overdueShipmentDto.UserId;

            await _uow.CompleteAsync();
        }
    }
}
