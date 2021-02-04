using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
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
        Task<ShipmentDTO> AddShipment(ShipmentDTO shipment);
        Task<InvoiceDTO> GetInvoiceByWaybill(string waybill);
        Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto);
        Task<bool> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO);
        Task<ShipmentDTO> GetShipment(string waybill);
        Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill);
        Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection);
        Task<IEnumerable<CountryDTO>> GetActiveCountries();
        Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId);
        Task<NewPricingDTO> GetGrandPriceForShipment(NewShipmentDTO newShipmentDTO);
        Task<IEnumerable<LGADTO>> GetLGAs();
        Task<IEnumerable<SpecialDomesticPackageDTO>> GetActiveSpecialDomesticPackages();
        Task<ShipmentDTO> GetDropOffShipmentForProcessing(string code);
        Task<DailySalesDTO> GetWaybillForServiceCentre(string waybill);
        Task<DailySalesDTO> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria);
        Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preShipment);
    }
}
