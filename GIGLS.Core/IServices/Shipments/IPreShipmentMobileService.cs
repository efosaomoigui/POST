using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Zone;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IPreShipmentMobileService : IServiceDependencyMarker
    {
        Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment);
        Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment);
        Task<List<PreShipmentMobileDTO>> GetShipments(BaseFilterCriteria filterOptionsDto);
        Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill);
        Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser();
        Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages();
        Task<MobileShipmentTrackingHistoryDTO> TrackShipment(string waybillNumber);
        Task<PreShipmentMobileDTO> AddMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        Task<List<MobilePickUpRequestsDTO>> GetMobilePickupRequest();
        Task<bool> UpdateMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        Task <bool> UpdatePreShipmentMobileDetails(List<PreShipmentItemMobileDTO> Preshipmentmobile);
        Task<SpecialResultDTO> GetSpecialPackages();
        Task<List<PreShipmentMobileDTO>> GetDisputePreShipment();
        Task<SummaryTransactionsDTO> GetPartnerWalletTransactions();
        Task<object> ResolveDisputeForMobile(PreShipmentMobileDTO preShipment);
        Task<object> CancelShipment(string Waybill);
        Task<object> AddRatings(MobileRatingDTO rating);
        Task<Partnerdto> GetMonthlyPartnerTransactions();
        Task<bool> CreateCustomer(string CustomerCode);

        Task<bool> UpdateDeliveryNumber(MobileShipmentNumberDTO detail);
        Task<string> CreatePartner(string CustomerCode);
        Task<bool> deleterecord(string detail);
        Task<bool> VerifyPartnerDetails(PartnerDTO partner);

        Task<PartnerDTO> GetPartnerDetails(string EmailId);

        Task<bool> UpdateReceiverDetails(PreShipmentMobileDTO receiver);

        Task<int> GetCountryId();

        
        Task<string> LoadImage(ImageDTO images);

        Task<List<Uri>> DisplayImages();
        Task<PreShipmentSummaryDTO> GetShipmentDetailsFromDeliveryNumber(string DeliveryNumber);
        Task<bool> ApproveShipment(string waybillNumber);

        Task<bool> CreateCompany(string CustomerCode);
        Task<MobilePriceDTO> GetHaulagePrice(HaulagePriceDTO haulagePricingDto);
        Task<bool> EditProfile(UserDTO user);
        Task<bool> UpdateVehicleProfile(UserDTO user);

    }
}