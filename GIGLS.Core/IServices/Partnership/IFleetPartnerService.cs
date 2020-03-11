using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Partnership
{
    public interface IFleetPartnerService : IServiceDependencyMarker
    {
        Task<object> AddFleetPartner(FleetPartnerDTO fleetPartnerDTO);
        Task UpdateFleetPartner(int partnerId, FleetPartnerDTO fleetPartnerDTO);
        Task RemoveFleetPartner(int partnerId);
        Task<IEnumerable<FleetPartnerDTO>> GetFleetPartners();
        Task<FleetPartnerDTO> GetFleetPartnerById(int partnerId);
        Task<int> CountOfPartnersUnderFleet();
        Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner();
        Task<List<PartnerTransactionsDTO>> GetFleetTransaction(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<object>> GetEarningsOfPartnersAttachedToFleet(ShipmentCollectionFilterCriteria filterCriteria);
    }
}
