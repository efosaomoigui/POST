using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IRiderDeliveryService : IServiceDependencyMarker
    {
        Task<List<RiderDeliveryDTO>> GetRiderDelivery(string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria);
    }
}
