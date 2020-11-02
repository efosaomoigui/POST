using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Utility;
using GIGLS.Core.DTO.Zone;
using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Customers;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IPreShipmentMobileService : IServiceDependencyMarker
    {
        Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment);
        Task<object> AddPreShipmentMobileThirdParty(CreatePreShipmentMobileDTO preShipment);
        Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment);
        Task<List<PreShipmentMobileDTO>> GetShipments(BaseFilterCriteria filterOptionsDto);
        Task<PreShipmentMobileDTO> GetShipmentByWaybill(string waybill);
        Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill);
        Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser();
        Task<List<TransactionPreShipmentDTO>> GetPreShipmentForUser(UserDTO user, ShipmentCollectionFilterCriteria filterCriteria);
        Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages();
        Task<MobileShipmentTrackingHistoryDTO> TrackShipment(string waybillNumber);
        Task<PreShipmentMobileDTO> AddMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        //Task<List<PreShipmentMobileDTO>> AddMobilePickupRequestMultipleShipment(MobilePickUpRequestsDTO pickuprequest);
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
        Task<PartnerDTO> CreatePartner(string CustomerCode);
        Task<bool> deleterecord(string detail);
        Task<bool> VerifyPartnerDetails(PartnerDTO partnerDto);

        Task<PartnerDTO> GetPartnerDetails(string EmailId);

        Task<bool> UpdateReceiverDetails(PreShipmentMobileDTO receiver);

        Task<int> GetCountryId();
        
        Task<string> LoadImage(ImageDTO images);

        Task<List<Uri>> DisplayImages();
        Task<PreShipmentSummaryDTO> GetShipmentDetailsFromDeliveryNumber(string DeliveryNumber);
        Task<bool> ApproveShipment(ApproveShipmentDTO detail);

        Task<bool> CreateCompany(string CustomerCode);
        Task<MobilePriceDTO> GetHaulagePrice(HaulagePriceDTO haulagePricingDto);
        Task<bool> EditProfile(UserDTO user);
        Task<bool> UpdateVehicleProfile(UserDTO user);
        Task<GIGGoDashboardDTO> GetDashboardInfo(BaseFilterCriteria filterCriteria);
        Task<object> CancelShipmentWithNoCharge(string Waybill, string Userchanneltype);
        Task<List<GiglgoStationDTO>> GetGoStations();
        Task<decimal> GetPickUpPriceForMultipleShipment(string customerType, string vehicleType, int CountryId);
        Task<MultipleShipmentOutput> CreateMobileShipment(NewPreShipmentMobileDTO newPreShipment);
        Task<MultipleMobilePriceDTO> GetPriceForMultipleShipments(NewPreShipmentMobileDTO preShipmentItemMobileDTO);
        Task<object> ResolveDisputeForMultipleShipments(PreShipmentMobileDTO preShipment);
        Task ScanMobileShipment(ScanDTO scanDTO);
        Task<bool> UpdateMobilePickupRequestUsingGroupCode(MobilePickUpRequestsDTO pickuprequest);
        Task<bool> UpdateMobilePickupRequestUsingWaybill(MobilePickUpRequestsDTO pickuprequest);
        Task<List<PreShipmentMobileDTO>> AddMobilePickupRequestMultipleShipment(MobilePickUpRequestsDTO pickuprequest);
        Task<List<LocationDTO>> GetPresentDayShipmentLocations();
        Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preShipment);
        Task<MobilePriceDTO> GetPriceForBike(PreShipmentMobileDTO preShipment);
        Task<bool> VerifyDeliveryCode(MobileShipmentNumberDTO detail);
        Task<bool> ChangeShipmentOwnershipForPartner(PartnerReAssignmentDTO request);

        Task<List<PreShipmentMobileDTO>> GetPreShipmentsAndShipmentsPaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO);
        Task<bool> UpdateDeliveryNumberV2(MobileShipmentNumberDTO detail);
        Task<string> GenerateDeliveryCode();
        Task<List<PreShipmentMobileDTO>> GetBatchPreShipmentMobile(string searchParam);
        Task<bool> SendReceiverDeliveryCodeBySMS(PreShipmentMobile preShipmentMobile, string number);
        Task<List<CompanyDTO>> GetBatchPreShipmentMobileOwners();

    }
}