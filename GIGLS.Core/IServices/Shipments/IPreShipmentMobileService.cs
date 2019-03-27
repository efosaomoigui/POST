using GIGLS.Core.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
   public interface IPreShipmentMobileService : IServiceDependencyMarker
    {
        
        Task<PreShipmentMobileDTO> AddPreShipmentMobile(PreShipmentMobileDTO preShipment);
        Task<PreShipmentMobileDTO> GetPrice(PreShipmentMobileDTO preShipment);

    }
}
