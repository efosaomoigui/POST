using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IShipmentTrackService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber);
    }
}
