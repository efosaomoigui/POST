using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdParty.WebServices.Magaya.Business.New;
using ThirdParty.WebServices.Magaya.DTO;
using ThirdParty.WebServices.Magaya.Services;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentService : IServiceDependencyMarker
    {
        Task<Tuple<List<ShipmentDTO>, int>> GetShipments(FilterOptionsDto filterOptionsDto);
        Task<List<InvoiceViewDTO>> GetIncomingShipments(FilterOptionsDto filterOptionsDto);
        Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds);
        Task<ShipmentDTO> GetShipment(int shipmentId);
        Task<ShipmentDTO> GetShipment(string waybill);
        Task<ShipmentDTO> GetBasicShipmentDetail(string waybill);
        Task<ShipmentDTO> AddShipment(ShipmentDTO shipment);
        Task<ShipmentDTO> AddShipmentForPaymentWaiver(ShipmentDTO shipmentDTO);
        Task UpdateShipment(int shipmentId, ShipmentDTO shipment);
        Task UpdateShipment(string waybill, ShipmentDTO shipment);
        Task DeleteShipment(int shipmentId);
        Task DeleteShipment(string waybill);
        Task<Shipment> GetShipmentForScan(string waybill);
        Task<List<ServiceCentreDTO>> GetUnGroupMappingServiceCentres();
        Task<List<InvoiceViewDTO>> GetUnGroupedWaybillsForServiceCentre(FilterOptionsDto filterOptionsDto, bool filterByDestinationSC = false);
        Task<List<InvoiceView>> GetUnGroupedWaybillsForServiceCentreDropDown(FilterOptionsDto filterOptionsDto, bool filterByDestinationSC = false);
        Task<List<ServiceCentreDTO>> GetUnmappedManifestServiceCentres();
        //Task<List<GroupWaybillNumberMappingDTO>> GetUnmappedGroupedWaybillsForServiceCentre(FilterOptionsDto filterOptionsDto);
        Task<List<GroupWaybillNumberDTO>> GetUnmappedGroupedWaybillsForServiceCentre(FilterOptionsDto filterOptionsDto);
        Task<DomesticRouteZoneMapDTO> GetZone(int destinationServiceCentre);
        Task<CountryRouteZoneMapDTO> GetCountryZone(int destinationCountry);
        Task<DailySalesDTO> GetDailySales(AccountFilterCriteria accountFilterCriteria);
        Task<DailySalesDTO> GetDailySalesByServiceCentre(AccountFilterCriteria accountFilterCriteria); 

        Task<Object[]> GetShipmentCreatedByDateMonitor(AccountFilterCriteria accountFilterCriteria, LimitDates Limitdates);
        Task<Object[]> GetShipmentCreatedByDateMonitorx(AccountFilterCriteria accountFilterCriteria, LimitDates Limitdates);
        Task<List<InvoiceViewDTOUNGROUPED2>> GetShipmentWaybillsByDateMonitor(AccountFilterCriteria accountFilterCriteria, LimitDates Limitdates);
        Task<List<InvoiceViewDTOUNGROUPED2>> GetShipmentWaybillsByDateMonitorx(AccountFilterCriteria accountFilterCriteria, LimitDates Limitdates);
        Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType);
        Task<bool> CancelShipment(string waybill);

        Task<List<ServiceCentreDTO>> GetAllWarehouseServiceCenters();
        Task<DailySalesDTO> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria);

        Task<ColoredInvoiceMonitorDTO> GetShipmentMonitor(AccountFilterCriteria accountFilterCriteria);
        Task<ColoredInvoiceMonitorDTO> GetShipmentMonitorx(AccountFilterCriteria accountFilterCriteria);
        Task<ColoredInvoiceMonitorDTO> GetShipmentMonitorEXpected(AccountFilterCriteria accountFilterCriteria);

        Task<bool> RePrintCountUpdater();
        Task<bool> AddShipmentFromMobile(ShipmentDTO shipment);
        Task<bool> ScanShipment(ScanDTO scan);
        Task RemoveWaybillNumberFromGroupForCancelledShipment(string groupWaybillNumber, string waybillNumber);
        Task<PreShipmentDTO> GetTempShipment(string code);
        Task<ShipmentDTO> GetDropOffShipmentForProcessing(string code);
        Task<List<ManifestDTO>> GetUnmappedManifestListForServiceCentre();
        Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlTransactionShipments(FilterOptionsDto filterOptionsDto);
        Task<List<ServiceCentreDTO>> GetUnmappedManifestServiceCentresForSuperManifest();
        Task<List<ManifestDTO>> GetUnmappedManifestForServiceCentre(FilterOptionsDto filterOptionsDto);
    }

    public interface IMagayaService : IServiceDependencyMarker
    {
        bool OpenConnection(out int access_key); 
        string CloseConnection(int access_key);
        Task<api_session_error> SetTransactions(int access_key, TheWarehouseReceiptCombo magayaShipmentDTO);
        string GetTransactions(int access_key, WarehouseReceipt magayaShipmentDTO);
        string SetEntity(int access_key, EntityDto entitydto);
        EntityList GetEntities(int access_key, string startwithstring);
        //List<Entity> GetEmployees(int access_key, string startwithstring);
        //EntityList GetVendors(int access_key, string startwithstring);
        List<ModeOfTransportation> GetModesOfTransportation();
        PortList GetPorts();
        PackageList GetPackageList();
        LocationList GetLocations();
        ChargeDefinitionList GetChargeDefinitionList(int access_key);
        List<string> GetItemStatus();
        Description CommodityDescription(string description);
        GUIDItemList QueryLog(int access_key, QuerylogDt0 qdto);
        TransactionTypes TransactionTypes();
        WarehouseReceiptList GetWarehouseReceiptRangeByDate(int access_key, QuerylogDt0 querydto);
        Tuple<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList> GetFirstTransbyDate(int access_key, QuerylogDt0 querydto, out string customcookie, out int more_result);
        bool GetNextTransByDate(int access_key, ref string cookie, out string trans_list_xml, out int more_results);
        bool GetFirstTransByDate(int access_key, QuerylogDt0 querydto, out string cookie, out int more_results);
        //ShipmentList GetShipmentRangeByDate(int access_key, QuerylogDt0 querydto);
        Tuple<WarehouseReceiptList, ShipmentList, InvoiceList, PaymentList> GetNextTransByDate2(int access_key, out int more_results,  ref string cookie,  string type);
        TransactionResults LargeQueryLog(int access_key, QuerylogDt0 querydto);
        Task<string> GetMagayaWayBillNumber();
        EntityList GetEntityObect();
        Task<List<ServiceCentreDTO>> GetDestinationServiceCenters();
        Task<string> SetEntityIntl(CustomerDTO custDTo);
        Task<IntlShipmentRequestDTO> CreateIntlShipmentRequest(IntlShipmentRequestDTO shipmentDTO);
        Task<Tuple<List<IntlShipmentDTO>, int>> getIntlShipmentRequests(FilterOptionsDto filterOptionsDto);

        Task<IntlShipmentRequestDTO> GetShipmentRequest(string requestNumber);
        Task<IntlShipmentRequestDTO> GetShipmentRequest(int shipmentRequestId);
    }


}
