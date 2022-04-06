using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentRepository : IRepository<Shipment>
    {
        Task<Tuple<List<ShipmentDTO>, int>> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<ShipmentDTO>, int>> GetDestinationShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<ShipmentDTO>, int>> GetShipmentDetailByWaybills(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds, List<string> waybills);
        Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria queryDto, int[] serviceCentreIds);
        Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds);
        Task<List<ShipmentDTO>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria);
        IQueryable<Shipment> ShipmentsAsQueryable();
        Task<ShipmentDTO> GetBasicShipmentDetail(string waybill);
        Task<List<InvoiceViewDTO>> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Task<List<CODShipmentDTO>> GetCODShipments(BaseFilterCriteria baseFilterCriteria);
        Task<List<CargoMagayaShipmentDTO>> GetCargoMagayaShipments(BaseFilterCriteria baseFilterCriteria);
        Task<List<InvoiceViewDTO>> GetWaybillForServiceCentre(string waybill, int[] serviceCentreIds);
        Task<double> GetSumOfMonthlyOrDailyWeightOfShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria, ShipmentReportType shipmentReportType);
        Task<ShipmentDTO> GetShipment(string waybill);
        Task<List<InvoiceViewDTO>> GetUnPaidWaybillForServiceCentre(int serviceCentreId);
        Task<List<InvoiceViewDTO>> GetCoporateTransactions(DateFilterForDropOff filter);
        Task<CustomerInvoiceDTO> GetCoporateTransactionsByCode(DateFilterForDropOff filter);
        Task<int> GetCountOfVehiclesAndTripsOfMovementManifest(string procedureName, DashboardFilterCriteria dashboardFilterCriteria);
        Task<List<CustomerInvoiceDTO>> GetMonthlyCoporateTransactions();
        Task<List<CustomerInvoiceDTO>> GetCoporateInvoiceList(DateFilterForDropOff filter);
        Task<List<InvoiceViewDTO>> GetIntlPaidWaybillForServiceCentre(NewFilterOptionsDto filterOptionsDto);
        Task<List<InvoiceViewDTO>> GetGoFasterShipments(NewFilterOptionsDto filterOptionsDto);
        Task<List<InvoiceViewDTO>> GetGoFasterShipmentsByServiceCentre(NewFilterOptionsDto filterOptionsDto);
        Task<List<DelayedDeliveryDTO>> GetDelayedDeliveryShipment(int serviceCenterId);
        Task<ShipmentDeliveryReportForHubRepsDTO> GetHubShipmentDeliveryReport(int hubRepServiceCentreId, DateTime from, DateTime to);
    }

    public interface IIntlShipmentRequestRepository : IRepository<IntlShipmentRequest>  
    {
        Task<List<IntlShipmentRequestDTO>> GetShipments(int[] serviceCentreIds);
        Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlTransactionShipmentRequest(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlTransactionShipmentRequest(DateFilterCriteria dateFilterCriteria);
        Task<List<IntlShipmentRequestDTO>> GetIntlShipmentRequestsForUser(ShipmentCollectionFilterCriteria filterCriteria, string currentUserId);
        Task<double> GetSumOfOutboundWeightOfShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria, int queryType);
        Task<int> GetCountOfOutboundShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria,  int queryType);
        Task<List<UnidentifiedItemsForInternationalShippingDTO>> GetUnIdentifiedIntlShipments(NewFilterOptionsDto filter);
        Task<UnidentifiedItemsForInternationalShippingDTO> GetUnIdentifiedIntlShipmentByID(int itemID);
        Task<List<InvoiceViewDTO>> GetProcessedIntlShipment(NewFilterOptionsDto filterOptionsDto);
        Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlReceivedShipmentRequest(DateFilterCriteria dateFilterCriteria);
        Task<Tuple<List<IntlShipmentDTO>, int>> GetIntlShipmentRequestsByUserId(string currentUserId);
        Task<Tuple<List<IntlShipmentDTO>, int>> GetMagayaNotReceivedShipmentRequest(DateFilterCriteria dateFilterCriteria);
        Task<Tuple<List<IntlShipmentDTO>, int>> GetMagayaReceivedShipmentRequest(DateFilterCriteria dateFilterCriteria);
    }
}
