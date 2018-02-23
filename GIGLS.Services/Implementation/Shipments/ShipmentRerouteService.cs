using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentRerouteService : IShipmentRerouteService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;

        public ShipmentRerouteService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }
        
        public Task<IEnumerable<ShipmentRerouteDTO>> GetRerouteShipments()
        {
            //get all shipments by servicecentre
            //var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            //var shipments = await _uow.Shipment.FindAsync(s => serviceCenters.Contains(s.DepartureServiceCentreId));
            //var shipmentsWaybills = shipments.ToList().Select(a => a.Waybill).AsEnumerable();

            ////get collected shipment
            //var shipmentReturns = await _uow.ShipmentReturn.FindAsync(x => shipmentsWaybills.Contains(x.WaybillNew));
            //var shipmentReturnsDto = Mapper.Map<IEnumerable<ShipmentReturnDTO>>(shipmentReturns);
            //return await Task.FromResult(shipmentReturnsDto.OrderByDescending(x => x.DateCreated));

            //comment this below and uncomment the one above once the UI is implemented
            var shipmentReturns = _uow.ShipmentReroute.GetAll().ToList().OrderByDescending(x => x.DateCreated);
            var shipmentReturnsDto = Mapper.Map<IEnumerable<ShipmentRerouteDTO>>(shipmentReturns);
            return Task.FromResult(shipmentReturnsDto);
        }

        public async Task<ShipmentDTO> AddRerouteShipment(ShipmentDTO shipment)
        {
            if (await _uow.ShipmentReroute.ExistAsync(v => v.WaybillOld.Equals(shipment.Waybill)))
            {
                throw new GenericException($"Waybill {shipment.Waybill} already exist");
            }

            var user = await _userService.GetCurrentUserId();
            ShipmentRerouteInitiator initiator = shipment.GrandTotal > 0 ? ShipmentRerouteInitiator.Customer : ShipmentRerouteInitiator.Staff;

            //create new shipment return
            var newShipmentReroute = new ShipmentReroute
            {
                WaybillNew = shipment.Waybill,
                WaybillOld = shipment.Waybill,
                RerouteBy = user,
                ShipmentRerouteInitiator = initiator
            };
            _uow.ShipmentReroute.Add(newShipmentReroute);

            await _uow.CompleteAsync();

            return shipment;
        }

        public async Task DeleteRerouteShipment(string waybill)
        {
            var shipmentReroute = await _uow.ShipmentReroute.GetAsync(x => x.WaybillNew.Equals(waybill) || x.WaybillOld.Equals(waybill));

            if (shipmentReroute == null)
            {
                throw new GenericException($"Waybill: {waybill} does not exist");
            }
            _uow.ShipmentReroute.Remove(shipmentReroute);
            await _uow.CompleteAsync();
        }

        public async Task<ShipmentRerouteDTO> GetRerouteShipment(string waybill)
        {
            var shipmentReroute = await _uow.ShipmentReroute.GetAsync(x => x.WaybillNew.Equals(waybill) || x.WaybillOld.Equals(waybill));

            if (shipmentReroute == null)
            {
                throw new GenericException($"Waybill: {waybill} does not exist");
            }

            var shipmentRerouteDto = Mapper.Map<ShipmentRerouteDTO>(shipmentReroute);

            var user = await _userService.GetUserById(shipmentRerouteDto.RerouteBy);

            shipmentRerouteDto.RerouteBy = user.FirstName + " " + user.LastName;

            return shipmentRerouteDto;
        }
        
        public Task UpdateRerouteShipment(ShipmentRerouteDTO rerouteDto)
        {
            throw new NotImplementedException();
        }
    }
}
