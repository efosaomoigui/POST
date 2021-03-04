using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ThirdPartyAPI
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

        //Manifests
        Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetManifestsInServiceCenter(DateFilterCriteria dateFilterCriteria);
        Task<List<GroupWaybillAndWaybillDTO>> GetGroupWaybillDataInManifest(string manifestCode);
    }
}