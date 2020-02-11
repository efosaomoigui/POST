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

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentService : IServiceDependencyMarker
    {
        Tuple<Task<List<ShipmentDTO>>, int> GetShipments(FilterOptionsDto filterOptionsDto);
        Task<List<GIGLS.Core.DTO.Account.InvoiceViewDTO>> GetIncomingShipments(FilterOptionsDto filterOptionsDto);
        Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds);
        Task<ShipmentDTO> GetShipment(int shipmentId);
        Task<ShipmentDTO> GetShipment(string waybill);
        Task<ShipmentDTO> GetBasicShipmentDetail(string waybill);
        Task<ShipmentDTO> AddShipment(ShipmentDTO shipment);
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
    }

    public interface IMagayaService : IServiceDependencyMarker
    {
        Task<bool> OpenConnection();
        Task<string> CloseConnection(int access_key);
        Task<string> SetTransactions(int access_key, string type, int flags, string trans_xml);

    }
}
