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
        Task<PreShipmentMobileDTO> GetPreShipmentMobile(int preShipmentMobileId);
        Task<PreShipmentMobileDTO> GetPreShipmentMobile(string waybill);
        Task<PreShipmentMobileDTO> AddPreShipmentMobile(PreShipmentMobileDTO preShipment);
        Task UpdatePreShipmentMobile(int preShipmentMobileId, PreShipmentMobileDTO preShipment);
        Task UpdatePreShipmentMobile(string waybill, PreShipmentMobileDTO preShipment);
        Task DeletePreShipmentMobile(int shipmentId);
        Task DeletePreShipmentMobile(string waybill);
        Task<bool> CancelPreShipmentMobile(string waybill);
    }
}
