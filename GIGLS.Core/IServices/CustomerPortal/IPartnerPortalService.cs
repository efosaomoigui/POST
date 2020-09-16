using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Shipments;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CustomerPortal
{
    public interface IPartnerPortalService : IServiceDependencyMarker
    {
        Task<SignResponseDTO> SignUp(UserDTO user);
        Task<UserDTO> CheckDetails(string user, string userchanneltype);
        Task<bool> CreateCustomer(string CustomerCode);
        Task<PartnerDTO> CreatePartner(string CustomerCode);
        Task<bool> CreateCompany(string CustomerCode);
        Task<UserDTO> ValidateOTP(OTPDTO otp);
        Task<UserDTO> GenerateReferrerCode(UserDTO user);
        Task<SignResponseDTO> ResendOTP(UserDTO user);
        Task<bool> EditProfile(UserDTO user);
        Task<bool> UpdateVehicleProfile(UserDTO user);
        Task<PreShipmentMobileDTO> AddMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        Task<List<string>> GetItemTypes();
        Task<bool> UpdateMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest);
        Task<List<MobilePickUpRequestsDTO>> GetMobilePickupRequest();
        Task<bool> UpdatePreShipmentMobileDetails(List<PreShipmentItemMobileDTO> preshipmentmobile);
        Task<SummaryTransactionsDTO> GetPartnerWalletTransactions();
        Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment);
        Task<object> AddRatings(MobileRatingDTO rating);
        Task<Partnerdto> GetMonthlyPartnerTransactions();
        Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill);
        Task<IdentityResult> ForgotPassword(string email, string password);
        Task SendGenericEmailMessage(MessageType messageType, object obj);
        Task<IdentityResult> ChangePassword(ChangePasswordDTO passwordDTO);
        Task<bool> UpdateDeliveryNumber(MobileShipmentNumberDTO detail);
        Task<object> CancelShipmentWithNoCharge(CancelShipmentDTO shipment);
        Task<IEnumerable<ScanStatusDTO>> GetScanStatus();
        Task<bool> ScanMultipleShipment(List<ScanDTO> scanList);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch();
        Task ReleaseShipmentForCollectionOnScanner(ShipmentCollectionDTO shipmentCollection);
        Task<List<LogVisitReasonDTO>> GetLogVisitReasons();
        Task<object> AddManifestVisitMonitoring(ManifestVisitMonitoringDTO manifestVisitMonitoringDTO);
        Task AddWallet(WalletDTO wallet);
        Task<string> Generate(int length);
        Task<bool> UpdateReceiverDetails(PreShipmentMobileDTO receiver);
        Task<bool> UpdateDeliveryNumberNew(MobileShipmentNumberDTO detail);
        Task<bool> SwitchRider(PartnerReAssignmentDTO request);
    }
}
