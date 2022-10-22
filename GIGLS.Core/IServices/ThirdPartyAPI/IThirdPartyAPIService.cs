using POST.Core.DTO;
using POST.Core.DTO.Account;
using POST.Core.DTO.Customers;
using POST.Core.DTO.OnlinePayment;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.User;
using POST.Core.DTO.Wallet;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.ThirdPartyAPI
{
    public interface IThirdPartyAPIService : IServiceDependencyMarker
    {
        Task<UserDTO> CheckDetailsForLogin(string user);

        //GetPrice
        Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment);

        //CaptureShipment
        Task<object> CreatePreShipment(CreatePreShipmentMobileDTO preShipmentDTO);

        //Track API
        Task<MobileShipmentTrackingHistoryDTO> TrackShipment(string waybillNumber);
        Task<IEnumerable<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber);

        //Localstation
        Task<IEnumerable<StationDTO>> GetLocalStations();
        Task<IEnumerable<StationDTO>> GetInternationalStations();

        //pickuprequests
        Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentCollectionFilterCriteria f_Criteria);

        //Get Active LGAs
        Task<IEnumerable<LGADTO>> GetActiveLGAs();

        Task<IEnumerable<LGADTO>> GetActiveHomeDeliveryLocations();
        Task<PreShipmentMobileDTO> GetPreShipmentMobileByWaybill(string waybillNumber);
        Task<List<ServiceCentreDTO>> GetServiceCentresByStation(int stationId);
        Task<UserDTO> CheckUserPhoneNo(UserValidationFor3rdParty user);

        //Manifests
        Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetManifestsInServiceCenter(DateFilterCriteria dateFilterCriteria);
        Task<List<GroupWaybillAndWaybillDTO>> GetGroupWaybillDataInManifest(string manifestCode);
        Task<bool> ItemShippedFromUKScan(string manifestCode);
        Task<GoogleAddressDTO> GetGoogleAddressDetails(GoogleAddressDTO location);
        Task<string> GetCellulantKey();
        Task<string> Decrypt(string encrytedKey);
        Task<bool> AddCellulantTransferDetails(TransferDetailsDTO transferDetailsDTO);
        Task<object> AddMultiplePreShipmentMobile(PreShipmentMobileMultiMerchantDTO preShipment);
        Task<MultiMerchantMobilePriceDTO> GetPriceMultipleMobileShipment(PreShipmentMobileMultiMerchantDTO preShipment);
        Task<ResponseDTO> ChargeWallet(ChargeWalletDTO chargeWalletDTO);
        Task<ResponseDTO> UpdateUserRankForAlpha(string merchantEmail);
        Task<bool> CODCallBack(CODCallBackDTO cod);
        Task<CellulantPushPaymentStatusResponse> UpdateCODShipmentOnCallBack(PushPaymentStatusRequstPayload payload);
        Task<bool> UpdateCODShipmentOnCallBackStellas(CODCallBackDTO cod);
        Task<string> Decrypt();
        Task<object> CancelShipment(string Waybill);
        Task<CompanyDTO> GetCompanyDetailsByEmail(string email);
        Task<bool> AddAzaPayTransferDetails(AzapayTransferDetailsDTO transferDetailsDTO);
    }
}