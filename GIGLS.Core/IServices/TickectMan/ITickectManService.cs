using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Zone;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.TickectMan
{
    public interface ITickectManService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices();
        Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination);
        Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices();
        Task<Object> GetCustomerBySearchParam(string customerType, SearchOption option);
        Task<decimal> GetPrice(PricingDTO pricingDto);
        Task<ShipmentDTO> AddShipment(NewShipmentDTO shipment);
        Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto);
        Task<bool> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO);
        Task<ShipmentDTO> GetShipment(string waybill);
        Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill);
        Task ReleaseShipmentForCollection(ShipmentCollectionDTOForFastTrack shipmentCollectionforDto);
        Task<IEnumerable<CountryDTO>> GetActiveCountries();
        Task<List<ServiceCentreDTO>> GetActiveServiceCentresBySingleCountry(int countryId);
        Task<NewPricingDTO> GetGrandPriceForShipment(NewShipmentDTO newShipmentDTO);
        Task<IEnumerable<LGADTO>> GetLGAs();
        Task<IEnumerable<SpecialDomesticPackageDTO>> GetActiveSpecialDomesticPackages();
        Task<ShipmentDTO> GetDropOffShipmentForProcessing(string code);
        Task<DailySalesDTO> GetWaybillForServiceCentre(string waybill);
        Task<DailySalesDTO> GetSalesForServiceCentre(DateFilterForDropOff dateFilterCriteria);
        //Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preShipment);
        Task<NewPricingDTO> GetPriceForDropOff(NewShipmentDTO newShipmentDTO);
        Task<ServiceCentreDTO> GetServiceCentreById(int centreid);
        Task<UserDTO> CheckDetailsForMobileScanner(string user);
        Task<int[]> GetPriviledgeServiceCenters(string userId);
        Task<PreShipmentSummaryDTO> GetShipmentDetailsFromDeliveryNumber(string DeliveryNumber);
        Task<bool> ApproveShipment(ApproveShipmentDTO detail);
        Task<IEnumerable<ServiceCentreDTO>> GetServiceCentreByStation(int stationId);
        Task<ShipmentDTO> AddAgilityShipmentToGIGGo(PreShipmentMobileFromAgilityDTO shipment);
        Task<MobilePriceDTO> GetGIGGOPrice(PreShipmentMobileDTO preShipment);
    }
}
                                                                                  