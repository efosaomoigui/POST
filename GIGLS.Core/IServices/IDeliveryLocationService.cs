using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IDeliveryLocationService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeliveryLocationDTO>> GetLocations();
    }
}
