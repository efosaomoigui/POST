using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Partnership
{
    public interface IPartnerService : IServiceDependencyMarker
    {
        Task<IEnumerable<PartnerDTO>> GetPartners();
        Task<PartnerDTO> GetPartnerById(int partnerId);
        Task<object> AddPartner(PartnerDTO partner);
        Task UpdatePartner(int partnerId, PartnerDTO partner);
        Task RemovePartner(int partnerId);
        Task<List<PartnerDTO>> GetPartnersByDate(BaseFilterCriteria filterCriteria);
        Task<IEnumerable<PartnerDTO>> GetExternalDeliveryPartners();
        Task<IEnumerable<VehicleTypeDTO>> GetVerifiedPartners(string fleetCode);
        Task<List<ExternalPartnerTransactionsPaymentDTO>> GetExternalPartnerTransactionsForPayment(ShipmentCollectionFilterCriteria shipmentCollectionFilterCriteria);
        Task<PartnerDTO> GetPartnerByCode(string partnerCode);
        Task<IEnumerable<VehicleTypeDTO>> GetUnVerifiedPartners(ShipmentCollectionFilterCriteria filterCriteria);
        Task ContactUnverifiedPartner(string email);
        Task<IEnumerable<VehicleTypeDTO>> GetVerifiedByRangePartners(ShipmentCollectionFilterCriteria filterCriteria);
        Task DeactivatePartner(int partnerId);
        Task<List<PartnerTransactionsDTO>> RiderRatings(PaginationDTO pagination);
        Task<List<RiderRateDTO>> GetRidersRatings(PaginationDTO pagination);
    }
}
