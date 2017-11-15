using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Tracking
{
    public class ShipmentTrackService : IShipmentTrackService
    {
        private readonly IShipmentTrackingService _shipmentTrackingService;

        public ShipmentTrackService(IShipmentTrackingService shipmentTrackingService)
        {
            _shipmentTrackingService = shipmentTrackingService;
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            return await _shipmentTrackingService.GetShipmentTrackings(waybillNumber);
        }

    }
}
