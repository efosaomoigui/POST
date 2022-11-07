using GIGL.POST.Core.Domain;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IServices.Partnership
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
        Task<List<AssetTripDTO>> GetFleetTrips(int fleetId);
        Task<FleetPartnerWalletDTO> GetPartnerWalletBalance();
        Task<List<AssetTripDTO>> GetFleetTripsByPartner();

        Task<decimal> GetVariableFleetTripAmount(FleetTripDTO fleetTrip);
        Task<decimal> GetFixFleetTripAmount(string registrationNumber);
        Task<List<FleetPartnerTransactionDTO>> GetPartnerTransactionHistory();
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerTransaction();
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerCreditTransaction();
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerDebitTransaction();
        Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerTransactionByDateRange(FleetFilterCriteria filterCriteria);
    }
}
