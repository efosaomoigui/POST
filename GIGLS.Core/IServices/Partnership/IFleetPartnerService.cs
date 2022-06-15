using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Fleets;
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
        Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner(string fleetCode);
        Task<List<FleetPartnerTransactionsDTO>> GetFleetTransaction(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<object>> GetEarningsOfPartnersAttachedToFleet(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<FleetMobilePickUpRequestsDTO>> GetPartnerResponseAttachedToFleet(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<PartnerDTO>> GetExternalPartnersNotAttachedToAnyFleetPartner();
        Task RemovePartnerFromFleetPartner(string partnerCode);
        Task<List<VehicleTypeDTO>> GetVerifiedPartners();
        Task<List<AssetDTO>> GetFleetAttachedToEnterprisePartner();
        Task<AssetDetailsDTO> GetFleetAttachedToEnterprisePartnerById(int fleetId);
        Task<List<FleetTripDTO>> GetFleetTrips(int fleetId);
        Task<FleetPartnerWalletDTO> GetPartnerWalletBalance();
        Task<List<FleetTripDTO>> GetFleetTripsByPartner();

        Task<decimal> GetVariableFleetTripAmount(FleetTripDTO fleetTrip);
        Task<decimal> GetFixFleetTripAmount(string registrationNumber);
        Task<List<FleetPartnerTransaction>> GetPartnerTransactionHistory();
    }
}
