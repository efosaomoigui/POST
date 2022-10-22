using POST.Core.DTO.Shipments;
using POST.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IServices.Business
{
    public interface IShipmentTrackService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber);
        Task<IEnumerable<ShipmentTrackingDTO>> TrackShipmentForMobile(string waybillNumber);
    }
}
