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
using System.IO;
using GIGLS.Core.Enums;
using GIGLS.Core.View;
using System.Linq;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using System;
using GIGL.GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Report
{
    public class ShipmentReportService : IShipmentReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ShipmentReportService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
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

    }
}
