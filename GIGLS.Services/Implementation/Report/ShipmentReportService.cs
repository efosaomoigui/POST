using GIGLS.Core;
using GIGLS.CORE.IServices.Report;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Account;
using System.Web;
using SpreadsheetLight;
using GIGLS.Core.Enums;
using GIGLS.Core.View;
using System.Linq;
using GIGLS.Core.DTO.ShipmentScan;
using System;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.DTO.Report;
using AutoMapper;

namespace GIGLS.Services.Implementation.Report
{
    public class ShipmentReportService : IShipmentReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private IServiceCentreService _serviceCenterService;

        public ShipmentReportService(IUnitOfWork uow, IUserService userService, IServiceCentreService serviceCenterService)
        {
            _uow = uow;
            _userService = userService;
            _serviceCenterService = serviceCenterService;
            MapperConfig.Initialize();
        }

        public async Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria filterCriteria)
        {
            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            var shipmentDto = await _uow.Shipment.GetShipments(filterCriteria, serviceCenters);

            foreach (var item in shipmentDto)
            {
                var user = await _uow.User.GetUserById(item.UserId);
                item.UserId = user.FirstName + " " + user.LastName;
            }
            return shipmentDto;
        }

        public async Task<List<ShipmentDTO>> GetTodayShipments()
        {
            ShipmentFilterCriteria filterCriteria = new ShipmentFilterCriteria
            {
                StartDate = System.DateTime.Today
            };

            var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
            return await _uow.Shipment.GetShipments(filterCriteria, serviceCenters);
        }

        public async Task<List<ShipmentDTO>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria)
        {
            return await _uow.Shipment.GetCustomerShipments(f_Criteria);
        }

        public async Task<object> GetDailySalesByServiceCentreReport(DailySalesDTO dailySalesDTO)
        {
            var rootDir = HttpContext.Current.Server.MapPath("~");
            var filePath = rootDir + @"\ReportTemplate\dailysales.xlsx";
            SLDocument sl = new SLDocument(filePath);

            //heading
            var heading = $"AGILITY SALES REPORT BETWEEN {dailySalesDTO.StartDate.ToShortDateString()} AND {dailySalesDTO.EndDate.ToShortDateString()}";
            sl.SetCellValue("D7", heading);

            var serviceCentreRow = 10;
            foreach (var serviceCentre in dailySalesDTO.DailySalesByServiceCentres)
            {
                // copy from sheet Report2 to Report1
                sl.CopyCellFromWorksheet("Report2", 9, 1, 11, 26, serviceCentreRow - 1, 1);
                var rowHeight = sl.GetRowHeight(serviceCentreRow);

                sl.SetCellValue(serviceCentreRow, 3, serviceCentre.DepartureServiceCentreName + " ");

                //align the service centre name
                SLStyle styleSC = sl.GetCellStyle(serviceCentreRow, 3);
                styleSC.Alignment.Vertical = DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Top;
                sl.SetCellStyle(serviceCentreRow, 3, styleSC);

                var row = serviceCentreRow;

                sl.SetRowHeight(row, rowHeight);

                foreach (var invoice in serviceCentre.Invoices)
                {
                    sl.SetCellValue(row, 4, invoice.Waybill);
                    sl.SetCellValue(row, 6, invoice.DepartureServiceCentreName);
                    sl.SetCellValue(row, 7, invoice.DestinationServiceCentreName);
                    sl.SetCellValue(row, 8, invoice.Vat);
                    sl.SetCellValue(row, 9, invoice.DiscountValue);
                    sl.SetCellValue(row, 10, invoice.Insurance);
                    sl.SetCellValue(row, 11, invoice.ShipmentPackagePrice);
                    sl.SetCellValue(row, 12, invoice.Amount);
                    sl.SetCellValue(row, 14, ((PaymentStatus)invoice.PaymentStatus).ToString());
                    sl.SetCellValue(row, 15, invoice.PaymentMethod);
                    sl.SetCellValue(row, 17, invoice.CustomerDetails.CustomerName);
                    sl.SetCellValue(row, 18, invoice.CustomerDetails.PhoneNumber);
                    sl.SetCellValue(row, 19, invoice.CustomerDetails.CustomerCode.Substring(0, 3));
                    sl.SetCellValue(row, 20, invoice.ReceiverName);
                    sl.SetCellValue(row, 21, invoice.ReceiverPhoneNumber);
                    sl.SetCellValue(row, 22, invoice.UserName);
                    sl.SetCellValue(row, 23, invoice.DateCreated);

                    //insert row and add styles
                    sl.InsertRow(row + 1, 1);
                    sl.SetRowHeight(row + 1, sl.GetRowHeight(row));
                    for (int col = 4; col <= 23; col++)
                    {
                        var style = sl.GetCellStyle(row, col);
                        sl.SetCellStyle(row + 1, col, style);
                    }

                    row++;
                }

                //add subtotal values
                sl.SetCellValue(row + 1, 12, serviceCentre.TotalSales);

                //merge cells
                sl.MergeWorksheetCells(serviceCentreRow, 3, row + 1, 3);

                //update serviceCentreRow
                serviceCentreRow = row + 5;
            }

            //---Total
            // copy from sheet Report2 to Report1
            sl.CopyCellFromWorksheet("Report2", 13, 1, 13, 26, serviceCentreRow, 1);

            //add Total values
            sl.SetCellValue(serviceCentreRow, 12, dailySalesDTO.TotalSales);

            //remove the Report2 sheet
            sl.DeleteWorksheet("Report2");

            //save to file system
            var filename = $"dailysales_{System.Guid.NewGuid()}.xlsx";
            var fileToSave = rootDir + @"\ReportTemplate\download\" + filename;
            sl.SaveAs(fileToSave);

            return await Task.FromResult(filename);
        }

        public async Task<List<ShipmentTrackingView>> GetShipmentTrackingFromView(ScanTrackFilterCriteria f_Criteria)
        {
            var queryable = _uow.ShipmentTracking.GetShipmentTrackingsFromViewAsync(f_Criteria);
            var result = await Task.FromResult(queryable.ToList());
            return result;
        }

        /// <summary>
        /// This method gets the Shipment Tracking Report from based on filter parameters
        /// </summary>
        /// <param name="f_Criteria">Criteria used for filtering</param>
        /// <returns>List of ScanStatusReport objects</returns>
        public async Task<List<ScanStatusReportDTO>> GetShipmentTrackingFromViewReport(ScanTrackFilterCriteria f_Criteria)
        {
            //var queryable = _uow.ShipmentTracking.GetShipmentTrackingsFromViewAsync(f_Criteria);
            var queryable = _uow.ShipmentTracking.GetShipmentTrackingsAsync(f_Criteria);
            var queryableList = queryable.Select(s =>
            new ShipmentTrackingDTO
            {
                Status = s.Status,
                Location = s.Location,
                ShipmentTrackingId = s.ShipmentTrackingId,
                ServiceCentreId = s.ServiceCentreId
            }).ToList();

            //1. Group by Service Centre
            var scanStatusReportList = new List<ScanStatusReportDTO>();
            //var allServiceCentreNames = _uow.ServiceCentre.GetAllAsQueryable().Select(s => s.Name).ToList();
            //var allServiceCentreNames = _uow.ServiceCentre.GetAllAsQueryable().Select(s => new { s.ServiceCentreId, s.Name }).ToList();

            //Get only Nigeria Service centre 
            var allServiceCentreNames = await _uow.ServiceCentre.GetLocalServiceCentres();

            var allScanStatus = _uow.ScanStatus.GetAllAsQueryable().ToList();
            foreach (var scName in allServiceCentreNames)
            {
                var scanStatusReportDTO = new ScanStatusReportDTO
                {
                    StartDate = f_Criteria.StartDate,
                    EndDate = f_Criteria.EndDate,
                    Location = scName.Name,
                    ServiceCentreId = scName.ServiceCentreId
                };

                //1.1 Group by Scan Status
                PopulateScanStatusReport(scanStatusReportDTO, queryableList, scName.ServiceCentreId);

                //1.2 Add to report list
                scanStatusReportList.Add(scanStatusReportDTO);
            }
            
            var result = await Task.FromResult(scanStatusReportList);
            return result;
        }

        /// <summary>
        /// This helper method populates the Scan Status Report
        /// </summary>
        /// <param name="scanStatusReportDTO"></param>
        /// <param name="queryableList"></param>
        /// <param name="serviceCentreName"></param>
        private void PopulateScanStatusReport(ScanStatusReportDTO scanStatusReportDTO,
            List<ShipmentTrackingDTO> queryableList, int serviceCentreId)
        {
            var shipmentScanStatusValues = Enum.GetNames(typeof(ShipmentScanStatus));

            foreach (var shipmentScanStatusName in shipmentScanStatusValues)
            {
                var count_status = queryableList.Where(s => s.ServiceCentreId == serviceCentreId && 
                s.Status == shipmentScanStatusName).Select(x => x.ShipmentTrackingId).Count();
                scanStatusReportDTO.StatusCountMap.Add(shipmentScanStatusName, count_status);
            }
        }


        public async Task<DashboardDTO> GetShipmentProgressSummary(ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO()
            {
                TotalCustomers = 0,
                TotalShipmentAwaitingCollection = 0,
                TotalShipmentDelivered = 0,
                TotalShipmentExpected = 0,
                TotalShipmentOrdered = 0
            };

            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            
            try
            {
                if (baseFilterCriteria.ServiceCentreId > 0)
                {
                    serviceCenterIds = new int[] { baseFilterCriteria.ServiceCentreId };
                }

                if (baseFilterCriteria.StationId > 0)
                {
                    serviceCenterIds = _uow.ServiceCentre.GetAllAsQueryable()
                        .Where(x => x.StationId == baseFilterCriteria.StationId).Select(x => x.ServiceCentreId).ToArray();
                }

                if (baseFilterCriteria.StateId > 0)
                {
                    var stations = _uow.Station.GetAllAsQueryable().Where(x => x.StateId == baseFilterCriteria.StateId).Select(x => x.StationId);
                    serviceCenterIds = _uow.ServiceCentre.GetAllAsQueryable()
                        .Where(w => stations.Contains(w.StationId)).Select(s => s.ServiceCentreId).ToArray();                                        
                }
                
                dashboardDTO = await GetShipmentProgressSummary(serviceCenterIds, baseFilterCriteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dashboardDTO;
        }


        private async Task<DashboardDTO> GetShipmentProgressSummary(int[] serviceCenterId, ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new DashboardDTO();

            //get startDate and endDate
            var queryDate = baseFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            //1. Get Total Shipment Expected filter by date using Date Created
            //1a. Get shipments coming to the service centre 
            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments();

            if (baseFilterCriteria.IsCOD)
            {
                allShipments = allShipments.Where(x => x.CashOnDeliveryAmount > 0);
            }

            var allShipmentsResult = allShipments.Where(s => s.IsShipmentCollected == false && serviceCenterId.Contains(s.DestinationServiceCentreId)
                && s.DateCreated >= startDate && s.DateCreated < endDate).Select(x => x.Waybill).Distinct();

            //1b. For waybill to be collected it must have satisfy the follwoing Shipment Scan Status
            //Collected by customer (OKC & OKT), Return (SSR), Reroute (SRR) : All status satisfy IsShipmentCollected above
            //shipments that have arrived destination service centre or cancelled should not be displayed in expected shipments
            var shipmetCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => !(x.ShipmentScanStatus == ShipmentScanStatus.OKC && x.ShipmentScanStatus == ShipmentScanStatus.OKT
                && x.ShipmentScanStatus == ShipmentScanStatus.SSR && x.ShipmentScanStatus == ShipmentScanStatus.SRR
                && x.ShipmentScanStatus == ShipmentScanStatus.ARF && x.ShipmentScanStatus == ShipmentScanStatus.SSC)).Select(w => w.Waybill);

            //1c. remove all the waybills that at the collection center from the income shipments
            allShipmentsResult = allShipmentsResult.Where(s => !shipmetCollection.Contains(s));
            dashboardDTO.TotalShipmentExpected = allShipmentsResult.Count();


            //2. Get Total Shipment Awaiting Collection
            var shipmentsInWaybills = _uow.Invoice.GetAllFromInvoiceAndShipments();

            if (baseFilterCriteria.IsCOD)
            {
                shipmentsInWaybills = shipmentsInWaybills.Where(x => x.CashOnDeliveryAmount > 0);
            }

            var shipmentsInWaybillsResult = shipmentsInWaybills.Where(s => s.IsShipmentCollected == false
            && serviceCenterId.Contains(s.DestinationServiceCentreId)).Select(x => x.Waybill).Distinct();
            
            var shipmentInCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF
                && x.DateCreated >= startDate && x.DateCreated < endDate).Select(x => x.Waybill);

            shipmentsInWaybillsResult = shipmentsInWaybillsResult.Where(s => shipmentInCollection.Contains(s));
            dashboardDTO.TotalShipmentAwaitingCollection = shipmentsInWaybillsResult.Count();

            //3. Get Total Shipment Created that has not depart service centre
            var allShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

            if (baseFilterCriteria.IsCOD)
            {
                allShipmentsQueryable = allShipmentsQueryable.Where(s => s.CashOnDeliveryAmount > 0);
            }

            allShipmentsQueryable = allShipmentsQueryable.Where(s => serviceCenterId.Contains(s.DepartureServiceCentreId));

            var shipmentTrackingHistory = _uow.ShipmentTracking.GetAllAsQueryable()
                .Where(x => x.Status == ShipmentScanStatus.DSC.ToString() || x.Status == ShipmentScanStatus.DPC.ToString()).Select(x => x.Waybill).Distinct();

            allShipmentsQueryable = allShipmentsQueryable.Where(s => !shipmentTrackingHistory.Contains(s.Waybill));

            dashboardDTO.TotalShipmentOrdered = allShipmentsQueryable.Count();
            
            //4. Get Total Shipment Delivered   
            //4a. Get collected shipment by date filtering : use current date if not date selected
            if(baseFilterCriteria.StartDate == null & baseFilterCriteria.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }

            var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => (x.ShipmentScanStatus == ShipmentScanStatus.OKT)
                && x.DateModified >= startDate && x.DateModified < endDate).Select(x => x.Waybill).Distinct();

            //4b. Get Shipments that its destination is the service centre
            var shipmentsWaybills = _uow.Invoice.GetAllFromInvoiceAndShipments();

            if (baseFilterCriteria.IsCOD)
            {
                shipmentsWaybills = shipmentsWaybills.Where(x => x.CashOnDeliveryAmount > 0);
            }

            var shipmentsWaybillsResult = shipmentsWaybills.Where(s => serviceCenterId.Contains(s.DestinationServiceCentreId) && s.IsShipmentCollected == true).Select(x => x.Waybill).Distinct();

            //4c. Extras the current login staff service centre shipment from the shipment collection
            shipmentsWaybillsResult = shipmentsWaybillsResult.Where(x => shipmentCollection.Contains(x));
            dashboardDTO.TotalShipmentDelivered = shipmentsWaybillsResult.Count();                      
            
            dashboardDTO.MostRecentOrder = new List<ShipmentOrderDTO> { };
            dashboardDTO.GraphData = new List<GraphDataDTO> { };

            return dashboardDTO;
        }

        //Shipment Breakdown
        public async Task<List<InvoiceViewDTO>> GetShipmentProgressSummaryBreakDown(ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new List<InvoiceViewDTO>() { };

            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

            try
            {
                if (baseFilterCriteria.ServiceCentreId > 0)
                {
                    serviceCenterIds = new int[] { baseFilterCriteria.ServiceCentreId };
                }

                if (baseFilterCriteria.StationId > 0)
                {
                    serviceCenterIds = _uow.ServiceCentre.GetAllAsQueryable()
                        .Where(x => x.StationId == baseFilterCriteria.StationId).Select(x => x.ServiceCentreId).ToArray();
                }

                if (baseFilterCriteria.StateId > 0)
                {
                    var stations = _uow.Station.GetAllAsQueryable().Where(x => x.StateId == baseFilterCriteria.StateId).Select(x => x.StationId);
                    serviceCenterIds = _uow.ServiceCentre.GetAllAsQueryable()
                        .Where(w => stations.Contains(w.StationId)).Select(s => s.ServiceCentreId).ToArray();
                }

                switch (baseFilterCriteria.ShipmentProgressSummaryType)
                {
                    case ShipmentProgressSummaryType.ExpectedShipment:
                        return dashboardDTO = await GetShipmentProgressSummaryForExpectedShipment(serviceCenterIds, baseFilterCriteria);
                    case ShipmentProgressSummaryType.AwaitingCollectionShipment:
                        return dashboardDTO = await GetShipmentProgressSummaryForAwaitingCollectionShipment(serviceCenterIds, baseFilterCriteria);
                    case ShipmentProgressSummaryType.DeliveredShipment:
                        return dashboardDTO = await GetShipmentProgressSummaryForDeliveredShipment(serviceCenterIds, baseFilterCriteria);
                    case ShipmentProgressSummaryType.OrderedShipment:
                        return dashboardDTO = await GetShipmentProgressSummaryForOrderedShipment(serviceCenterIds, baseFilterCriteria);
                    default:
                        return dashboardDTO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //break the result into ShipmentProgressSummaryType
        private async Task<List<InvoiceViewDTO>> GetShipmentProgressSummaryForExpectedShipment(int[] serviceCenterId, ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new List<InvoiceViewDTO>() { };

            //get startDate and endDate
            var queryDate = baseFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            
            //3. Get Total Shipment Expected filter by date using Date Created
            //3a. Get shipments coming to the service centre 
            var allShipments = _uow.Invoice.GetAllFromInvoiceAndShipments()
                .Where(s => s.IsShipmentCollected == false && serviceCenterId.Contains(s.DestinationServiceCentreId)
                && s.DateCreated >= startDate && s.DateCreated < endDate);

            if (baseFilterCriteria.IsCOD)
            {
                allShipments = allShipments.Where(x => x.CashOnDeliveryAmount > 0);
            }

            //3b. For waybill to be collected it must have satisfy the follwoing Shipment Scan Status
            //Collected by customer (OKC & OKT), Return (SSR), Reroute (SRR) : All status satisfy IsShipmentCollected above
            //shipments that have arrived destination service centre or cancelled should not be displayed in expected shipments
            var shipmetCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => !(x.ShipmentScanStatus == ShipmentScanStatus.OKC && x.ShipmentScanStatus == ShipmentScanStatus.OKT
                && x.ShipmentScanStatus == ShipmentScanStatus.SSR && x.ShipmentScanStatus == ShipmentScanStatus.SRR
                && x.ShipmentScanStatus == ShipmentScanStatus.ARF && x.ShipmentScanStatus == ShipmentScanStatus.SSC)).Select(w => w.Waybill);
            
            //3c. remove all the waybills that at the collection center from the income shipments
            allShipments = allShipments.Where(s => !shipmetCollection.Any(x => x == s.Waybill));
            dashboardDTO = Mapper.Map<List<InvoiceViewDTO>>(allShipments.OrderByDescending(x => x.DateCreated).ToList());

            //Use to populate service centre 
            var allServiceCentres = await _serviceCenterService.GetServiceCentres();

            //populate the service centres
            foreach (var invoiceViewDTO in dashboardDTO)
            {
                invoiceViewDTO.DepartureServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DepartureServiceCentreId);
                invoiceViewDTO.DestinationServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DestinationServiceCentreId);
            }

            return dashboardDTO;
        }
        private async Task<List<InvoiceViewDTO>> GetShipmentProgressSummaryForOrderedShipment(int[] serviceCenterId, ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new List<InvoiceViewDTO>() { };

            //get startDate and endDate
            var queryDate = baseFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            
            var allShipmentsQueryable = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);
            allShipmentsQueryable = allShipmentsQueryable.Where(s => serviceCenterId.Contains(s.DepartureServiceCentreId));

            if (baseFilterCriteria.IsCOD)
            {
                allShipmentsQueryable = allShipmentsQueryable.Where(x => x.CashOnDeliveryAmount > 0);
            }

            var shipmentTrackingHistory = _uow.ShipmentTracking.GetAllAsQueryable()
                .Where(x => x.Status == ShipmentScanStatus.DSC.ToString() || x.Status == ShipmentScanStatus.DPC.ToString()).Select(x => x.Waybill).Distinct();

            allShipmentsQueryable = allShipmentsQueryable.Where(s => !shipmentTrackingHistory.Contains(s.Waybill));

            dashboardDTO = Mapper.Map<List<InvoiceViewDTO>>(allShipmentsQueryable.OrderByDescending(x => x.DateCreated).ToList());

            //Use to populate service centre 
            var allServiceCentres = await _serviceCenterService.GetServiceCentres();

            //populate the service centres
            foreach (var invoiceViewDTO in dashboardDTO)
            {
                invoiceViewDTO.DepartureServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DepartureServiceCentreId);
                invoiceViewDTO.DestinationServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DestinationServiceCentreId);
            }

            return dashboardDTO;
        }

        private async Task<List<InvoiceViewDTO>> GetShipmentProgressSummaryForDeliveredShipment(int[] serviceCenterId, ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new List<InvoiceViewDTO>() { };

            //get startDate and endDate
            var queryDate = baseFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            //2. Get Total Shipment Delivered   
            if (baseFilterCriteria.StartDate == null & baseFilterCriteria.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }

            //2a. Get collected shipment by date filtering
            var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => (x.ShipmentScanStatus == ShipmentScanStatus.OKT)
                && x.DateModified >= startDate && x.DateModified < endDate).Select(x => x.Waybill).Distinct();

            //2b. Get Shipments that its destination is the service centre
            var shipmentsWaybills = _uow.Invoice.GetAllFromInvoiceAndShipments()
                .Where(s => serviceCenterId.Contains(s.DestinationServiceCentreId) && s.IsShipmentCollected == true);

            if (baseFilterCriteria.IsCOD)
            {
                shipmentsWaybills = shipmentsWaybills.Where(x => x.CashOnDeliveryAmount > 0);
            }

            //2c. Extras the current login staff service centre shipment from the shipment collection
            shipmentsWaybills = shipmentsWaybills.Where(x => shipmentCollection.Any(w => w == x.Waybill));
            dashboardDTO = Mapper.Map<List<InvoiceViewDTO>>(shipmentsWaybills.OrderByDescending(x => x.DateCreated).ToList());

            //Use to populate service centre 
            var allServiceCentres = await _serviceCenterService.GetServiceCentres();

            //populate the service centres
            foreach (var invoiceViewDTO in dashboardDTO)
            {
                invoiceViewDTO.DepartureServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DepartureServiceCentreId);
                invoiceViewDTO.DestinationServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DestinationServiceCentreId);
            }

            return dashboardDTO;
        }

        private async Task<List<InvoiceViewDTO>> GetShipmentProgressSummaryForAwaitingCollectionShipment(int[] serviceCenterId, ShipmentProgressSummaryFilterCriteria baseFilterCriteria)
        {
            var dashboardDTO = new List<InvoiceViewDTO>() { };

            //get startDate and endDate
            var queryDate = baseFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            
            //4. Get Total Shipment Awaiting Collection
            var shipmentsInWaybills = _uow.Invoice.GetAllFromInvoiceAndShipments()
                .Where(s => s.IsShipmentCollected == false && serviceCenterId.Contains(s.DestinationServiceCentreId));

            if (baseFilterCriteria.IsCOD)
            {
                shipmentsInWaybills = shipmentsInWaybills.Where(x => x.CashOnDeliveryAmount > 0);
            }

            var shipmentInCollection = _uow.ShipmentCollection.GetAllAsQueryable()
                .Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF
                && x.DateCreated >= startDate && x.DateCreated < endDate).Select(x => x.Waybill);

            shipmentsInWaybills = shipmentsInWaybills.Where(s => shipmentInCollection.Contains(s.Waybill));
            dashboardDTO = Mapper.Map<List<InvoiceViewDTO>>(shipmentsInWaybills.OrderByDescending(x => x.DateCreated).ToList());

            //Use to populate service centre 
            var allServiceCentres = await _serviceCenterService.GetServiceCentres();

            //populate the service centres
            foreach (var invoiceViewDTO in dashboardDTO)
            {
                invoiceViewDTO.DepartureServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DepartureServiceCentreId);
                invoiceViewDTO.DestinationServiceCentre = allServiceCentres.SingleOrDefault(x => x.ServiceCentreId == invoiceViewDTO.DestinationServiceCentreId);
            }

            return dashboardDTO;
        }
    }
}
