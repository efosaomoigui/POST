using GIGLS.Core.Domain;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Admin;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.DTO.SLA;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Utility;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CustomerPortal
{
    public interface ICustomerPortalService : IServiceDependencyMarker
    {
        Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentCollectionFilterCriteria f_Criteria);
        Task<WalletTransactionSummaryDTO> GetWalletTransactions();
        Task<IEnumerable<InvoiceViewDTO>> GetInvoices();
        Task<InvoiceDTO> GetInvoiceByWaybill(string waybill);
        Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber);
        Task<List<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber);
        Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount();
        Task<IEnumerable<PaymentPartialTransactionDTO>> GetPartialPaymentTransaction(string waybill);
        Task<DashboardDTO> GetDashboard();

        //For Quick Quotes
        Task<IEnumerable<StateDTO>> GetStates(int pageSize = 10, int page = 1);
        int GetStatesTotal();
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres();
        Task<IEnumerable<DeliveryOptionDTO>> GetDeliveryOptions();
        Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages();
        Task<IEnumerable<HaulageDTO>> GetHaulages();
        Task<IEnumerable<InsuranceDTO>> GetInsurances();
        Task<IEnumerable<VATDTO>> GetVATs();
        Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination);
        Task<decimal> GetPrice(PricingDTO pricingDto);
        Task<decimal> GetHaulagePrice(HaulagePricingDTO pricingDto);
        Task<CustomerDTO> GetCustomer(string userId);
        Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword);
        Task<IdentityResult> ChangePassword(ChangePasswordDTO passwordDTO);
        Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO);
        Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDTO);
        Task<object> AddWalletPaymentLogAnonymousUser(WalletPaymentLogDTO walletPaymentLogDTO);
        Task<object> AddWaybillPaymentLogFromApp(WaybillPaymentLogDTO walletPaymentLogDto);
        Task<USSDResponse> InitiatePaymentUsingUSSD(WalletPaymentLogDTO walletPaymentLogDto);
        Task<object> UpdateWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDTO);

        //Task<List<PreShipmentDTO>> GetPreShipments(FilterOptionsDto filterOptionsDto);
        //Task<PreShipmentDTO> GetPreShipment(string waybill);
        Task<UserDTO> Register(UserDTO user);
        Task<SignResponseDTO> SignUp(UserDTO user);
        Task<SignResponseDTO> ResendOTP(UserDTO user);

        Task<List<CodPayOutList>> GetPaidCODByCustomer();

        //SLA
        Task<SLADTO> GetSLA();
        Task<object> SignSLA(int slaId);

        //Payment Log
        Task<Tuple<Task<List<WalletPaymentLogDTO>>, int>> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto);

        Task<List<string>> GetItemTypes();

        Task<List<StationDTO>> GetLocalStations();

        Task<Dictionary<string, List<StationDTO>>> GetAllStations();
        Task<MobilePriceDTO> GetHaulagePrice(HaulagePriceDTO haulagePricingDto);
        Task<IEnumerable<NewCountryDTO>> GetUpdatedCountries();
        Task<bool> ApproveShipment(ApproveShipmentDTO detail);
        Task<PreShipmentSummaryDTO> GetShipmentDetailsFromDeliveryNumber(string DeliveryNumber);

        Task<bool> UpdateReceiverDetails(PreShipmentMobileDTO receiver);
        Task<PartnerDTO> GetPartnerDetails(string Email);
        Task<List<Uri>> DisplayImages();
        Task<string> LoadImage(ImageDTO images);
        Task<bool> VerifyPartnerDetails(PartnerDTO partnerDto);
        Task<string> Generate(int length);
        Task<IdentityResult> ForgotPassword(string email, string password);
        Task SendGenericEmailMessage(MessageType messageType, object obj);
        Task<bool> deleterecord(string detail);
        Task<bool> UpdateDeliveryNumber(MobileShipmentNumberDTO detail);
        Task<Partnerdto> GetMonthlyPartnerTransactions();
        Task<object> AddRatings(MobileRatingDTO rating);
        Task<object> CancelShipment(string Waybill);
        Task<UserDTO> IsOTPValid(int OTP);
        Task<UserDTO> ValidateOTP(OTPDTO otp);
        Task<UserDTO> CheckDetails(string user, string userchanneltype);
        Task<bool> CreateCustomer(string CustomerCode);
        Task<PartnerDTO> CreatePartner(string CustomerCode);
        Task<bool> CreateCompany(string CustomerCode);
        Task<bool> EditProfile(UserDTO user);
        Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment);
        Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser();
        Task<List<TransactionPreShipmentDTO>> GetPreShipmentForUser(UserDTO user, ShipmentCollectionFilterCriteria filterCriteria);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionsForMobile();
        Task<ModifiedWalletTransactionSummaryDTO> GetWalletTransactionsForMobile(ShipmentCollectionFilterCriteria filterCriteria);
        Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment);
        Task<WalletDTO> GetWalletBalance();
        Task<SpecialResultDTO> GetSpecialPackages();
        Task<MobileShipmentTrackingHistoryDTO> trackShipment(string waybillNumber);
        Task<PreShipmentMobileDTO> AddMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        Task<List<MobilePickUpRequestsDTO>> GetMobilePickupRequest();
        Task<bool> UpdateMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        Task<object> ResolveDisputeForMobile(PreShipmentMobileDTO preShipment);
        Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill);
        Task<bool> UpdatePreShipmentMobileDetails(List<PreShipmentItemMobileDTO> preshipmentmobile);
        Task<List<PreShipmentMobileDTO>> GetDisputePreShipment();
        Task<SummaryTransactionsDTO> GetPartnerWalletTransactions();
        Task<bool> UpdateVehicleProfile(UserDTO user);
        Task<IEnumerable<LGADTO>> GetActiveLGAs();
        Task<AdminReportDTO> WebsiteData();
        Task AddWallet(WalletDTO wallet);
        Task<UserDTO> GenerateReferrerCode(UserDTO user);
        Task<string> Decrypt();
        Task<string> EncryptWebsiteKey();
        Task<object> CancelShipmentWithNoCharge(CancelShipmentDTO shipment);

        Task SendPickUpRequestMessage(string userId);
        Task<List<GiglgoStationDTO>> GetGoStations();
        Task<List<DeliveryNumberDTO>> GetDeliveryNumbers(int count);
        Task<UserDTO> CheckDetailsForCustomerPortal(string user);
        Task<UserDTO> CheckDetailsForMobileScanner(string user);
        Task<bool> UpdateGIGGoShipmentStaus(MobilePickUpRequestsDTO mobilePickUpRequestsDTO);
        Task<MultipleShipmentOutput> AddMultiplePreShipmentMobile(NewPreShipmentMobileDTO preShipment);
        Task<MultipleMobilePriceDTO> GetPriceForMultipleShipments(NewPreShipmentMobileDTO preShipment);
        Task<object> ResolveDisputeForMultipleShipment(PreShipmentMobileDTO preShipment);
        Task<MobileGroupCodeWaybillMappingDTO> GetWaybillNumbersInGroup(string groupCode);
        Task<MobileGroupCodeWaybillMappingDTO> GetWaybillDetailsInGroup(string groupCode);

        Task<List<PreShipmentMobileDTO>> AddMobilePickupRequestMultipleShipment(MobilePickUpRequestsDTO pickuprequest);
        Task UpdatePickupManifestStatus(ManifestStatusDTO manifestStatusDTO);
        Task<List<PickupManifestWaybillMappingDTO>> GetWaybillsInPickupManifest(string manifestCode);
        Task<List<PreShipmentDTO>> GetDropOffsForUser(ShipmentCollectionFilterCriteria filterCriteria);
        Task<PreShipmentDTO> GetDropOffDetail(string tempCode);
        Task<bool> CreateOrUpdateDropOff(PreShipmentDTO preShipmentDTO);
        Task<bool> UpdateMobilePickupRequestUsingGroupCode(MobilePickUpRequestsDTO pickuprequest);
        Task<bool> UpdateMobilePickupRequestUsingWaybill(MobilePickUpRequestsDTO pickupRequest);
        Task<List<LocationDTO>> GetPresentDayShipmentLocations();
        Task<ShipmentDetailDanfoDTO> GetShipmentDetailForDanfo(string waybill);
        Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preShipment);
        Task<bool> CreateOrUpdateDropOffForAgent(PreShipmentDTO preShipmentDTO);
        Task<UserDTO> CheckDetailsForAgentApp(string user);
        Task<PreShipmentMobileThirdPartyDTO> AddPreShipmentMobileForThirdParty(CreatePreShipmentMobileDTO preShipment);
        Task<PaymentResponse> VerifyAndValidatePayment(string referenceCode);
        Task<GatewayCodeResponse> GetGatewayCode();
        Task<IEnumerable<ScanStatusDTO>> GetScanStatus();
        Task<bool> ScanMultipleShipment(List<ScanDTO> scanList);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch();
        Task ReleaseShipmentForCollectionOnScanner(ShipmentCollectionDTO shipmentCollection);
        Task<List<LogVisitReasonDTO>> GetLogVisitReasons();
        Task<object> AddManifestVisitMonitoring(ManifestVisitMonitoringDTO manifestVisitMonitoringDTO);
        Task<WalletDTO> GetWalletBalanceWithName();
        Task<List<OutstandingPaymentsDTO>> GetOutstandingPayments();
        Task<bool> PayForShipment(string waybill);
        Task<bool> VerifyDeliveryCode(MobileShipmentNumberDTO detail);
        Task<bool> DeleteDropOff(string waybill);
        Task<object> GetUserCountryCode(UserDTO user);
        Task<IEnumerable<NewCountryDTO>> GetActiveCountries();
        Task<IEnumerable<StationDTO>> GetStationsByCountry(int countryId);
        Task<bool> ProfileInternationalUser(IntertnationalUserProfilerDTO intlUserProfiler);
        Task<List<ServiceCentreDTO>> GetServiceCentresByStation(int stationId);
        Task<bool> ChangeShipmentOwnershipForPartner(PartnerReAssignmentDTO request);
        Task<IEnumerable<LGADTO>> GetActiveHomeDeliveryLocations();
        Task<List<WalletTransactionDTO>> GetWalletTransactionsForMobilePaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO);
        Task<List<PreShipmentMobileDTO>> GetPreShipmentsAndShipmentsPaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO);
        Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId);
        Task<List<MobilePickUpRequestsDTO>> GetAllMobilePickUpRequestsPaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO);
        Task<bool> UpdateDeliveryNumberV2(MobileShipmentNumberDTO detail);

        Task<bool> UpdatePreshipmentMobileStatusToPickedup(string manifestNumber, List<string> waybills);
        Task<List<PreshipmentManifestDTO>> GetAllManifestForPreShipmentMobile();
        Task<bool> UpdatePreShipmentMobile(PreShipmentMobile preshipmentmobile);
        Task<List<StoreDTO>> GetStoresByCountry(int countryId);
        Task<object> CreateNotification(NotificationDTO notificationDTO);
        Task<IEnumerable<NotificationDTO>> GetNotifications(bool? IsRead);
        Task UpdateNotificationAsRead(int notificationId);
        Task<MessageDTO> GetIntlMessageForApp(int countryId);
        Task<ResponseDTO> UnboardUser(NewCompanyDTO company);
        Task<ResponseDTO> ValidateUser(UserValidationNewDTO userDetail);
        Task<ResponseDTO> UpdateUserRank(UserValidationDTO userValidationDTO);
        Task<bool> SendMessage(NewMessageDTO obj);
        Task<UserDTO> GetUserByEmail(string email); 
        Task<ResponseDTO> ChargeWallet(ChargeWalletDTO chargeWalletDTO);
        Task<List<IntlShipmentRequestDTO>> GetIntlShipmentRequestsForUser(ShipmentCollectionFilterCriteria filterCriteria);
        Task<ResponseDTO> VerifyBVNNo(string bvnNo);
        Task<bool> ReleaseMovementManifest(ReleaseMovementManifestDto valMovementManifest);
        Task<IEnumerable<MovementManifestNumberDTO>> GetAllManifestMovementManifestNumberMappings(DateFilterCriteria dateFilterCriteria);
        Task<List<MovementDispatchDTO>> GetManifestsInMovementManifestForMovementDispatch();
        Task<CompanyDTO> UpgradeToEcommerce(UpgradeToEcommerce newCompanyDTO);
        Task<List<ServiceCentreDTO>> GetActiveServiceCentresBySingleCountry(int countryId, int stationId = 0);
        Task<IEnumerable<CountryDTO>> GetIntlShipingCountries();
        Task<List<ServiceCentreDTO>> GetActiveServiceCentres();
        Task<List<AddressDTO>> GetTopFiveUserAddresses();
    }
}
