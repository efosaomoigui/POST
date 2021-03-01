using AutoMapper;
using GIGLS.Core.Domain.Archived;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.Archived;
using GIGLS.Core.View;
using GIGLS.Core.View.Archived;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Archived
{
    public class InvoiceArchiveRepository : Repository<Invoice_Archive, GIGLSContext>, IInvoiceArchiveRepository
    {
        private GIGLSContextForView _GIGLSContextForView;

        public InvoiceArchiveRepository(GIGLSContext context) : base(context)
        {
            _GIGLSContextForView = new GIGLSContextForView();
        }

        public Task<List<InvoiceDTO>> GetInvoicesAsync(int[] serviceCentreIds)
        {
            var shipments = Context.Shipment_Archive.AsQueryable()
                .Where(s => s.IsCancelled == false && s.IsDeleted == false);
            if (serviceCentreIds.Length > 0)
            {
                shipments = shipments.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var invoiceDto = (from s in shipments
                          join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                          select new InvoiceDTO
                          {
                              InvoiceId = i.InvoiceId,
                              InvoiceNo = i.InvoiceNo,
                              Amount = i.Amount,
                              PaymentStatus = i.PaymentStatus,
                              PaymentMethod = i.PaymentMethod,
                              PaymentDate = i.PaymentDate,
                              Waybill = i.Waybill,
                              DueDate = i.DueDate,
                              IsInternational = i.IsInternational,
                              ServiceCentreId = i.ServiceCentreId,
                              IsShipmentCollected = i.IsShipmentCollected,
                              CountryId = i.CountryId,
                              DateCreated = i.DateCreated,
                              DateModified = i.DateModified
                          }).ToList();

            return Task.FromResult(invoiceDto);
        }

        public Task<List<InvoiceDTO>> GetInvoicesAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {              
            //get startDate and endDate
            var queryDate = accountFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var shipments = Context.Shipment_Archive.AsQueryable()
                .Where(s => s.IsCancelled == false && s.IsDeleted == false);

            shipments = shipments.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            if (serviceCentreIds.Length > 0)
            {
                shipments = shipments.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            if (accountFilterCriteria.ServiceCentreId > 0)
            {
                shipments = shipments.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            //station
            if (accountFilterCriteria.StationId > 0)
            {
                //get the service centres in that station
                var serviceCentres = Context.ServiceCentre.Where(s => s.StationId == accountFilterCriteria.StationId).
                    Select(a => a.ServiceCentreId).ToList();
                shipments = shipments.Where(s => serviceCentres.Contains(s.DepartureServiceCentreId));
            }

            var invoiceDto = (from s in shipments
                              join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                              select new InvoiceDTO
                              {
                                  InvoiceId = i.InvoiceId,
                                  InvoiceNo = i.InvoiceNo,
                                  Amount = i.Amount,
                                  PaymentStatus = i.PaymentStatus,
                                  PaymentMethod = i.PaymentMethod,
                                  PaymentDate = i.PaymentDate,
                                  Waybill = i.Waybill,
                                  DueDate = i.DueDate,
                                  IsInternational = i.IsInternational,
                                  ServiceCentreId = i.ServiceCentreId,
                                  IsShipmentCollected = i.IsShipmentCollected,
                                  CountryId = i.CountryId,
                                  DateCreated = i.DateCreated,
                                  DateModified = i.DateModified                                  
                              });

            //payment status
            if (accountFilterCriteria.PaymentStatus.HasValue)
            {
                invoiceDto = invoiceDto.Where(x => x.PaymentStatus.Equals(accountFilterCriteria.PaymentStatus));
            }

            var invoicesResult = invoiceDto.ToList();
            return Task.FromResult(invoicesResult.OrderByDescending(x => x.DateCreated).ToList());
        }

        public IQueryable<InvoiceArchiveView> GetInvoicesForReminderAsync()
        {
            //get all invoices from InvoiceView Table
            var invoices = _GIGLSContextForView.InvoiceArchiveView.AsQueryable();

            //filter by paymentstatus of non paid
            invoices = invoices.Where(s => s.PaymentStatus == 0 && s.CompanyType == "Corporate");
            return invoices;
        }

        public IQueryable<InvoiceArchiveView> GetWalletForReminderAsync()
        {
            //get all invoices from InvoiceView Table
            var invoices = _GIGLSContextForView.InvoiceArchiveView.AsQueryable();

            //filter by paymentstatus of non paid
            invoices = invoices.Where(s => s.CompanyType == "Ecommerce");
            return invoices;
        }

        //Stored Procedure version
        //var salesPeople = await context.Database.SqlQuery<SalesPerson>("AllSalesPeople").ToListAsync();
        public async Task<List<InvoiceViewDTO>> GetInvoicesFromViewAsyncFromSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            var queryDate = accountFilterCriteria.getStartDateAndEndDate();

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", queryDate.Item1);
            SqlParameter endDate = new SqlParameter("@EndDate", queryDate.Item2);

            SqlParameter paymentStatus = new SqlParameter("@PaymentStatus", Convert.ToInt32(accountFilterCriteria.PaymentStatus));//accountFilterCriteria.PaymentStatus

            var sc = (serviceCentreIds.Length > 0) ? serviceCentreIds[0] : 0;
            //SqlParameter paymentMethod = new SqlParameter("@paymentMethod", accountFilterCriteria.p);
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc); //serviceCentreIds[0]

            SqlParameter stationId = new SqlParameter("@StationId", accountFilterCriteria.StationId);
            SqlParameter CompanyType = new SqlParameter("@CompanyType", (object)accountFilterCriteria.CompanyType ?? DBNull.Value);
            SqlParameter isCod = new SqlParameter("@IsCashOnDelivery", (object)accountFilterCriteria.IsCashOnDelivery ?? DBNull.Value);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            var invoices =
           await _GIGLSContextForView.Database.SqlQuery<InvoiceViewDTO>("NewInvoiceViewSP @IsCancelled, @StartDate, @EndDate, @PaymentStatus, @DepartureServiceCentreId, @StationId, @CompanyType, @IsCashOnDelivery, @CountryId",
                iscancelled, startDate, endDate, paymentStatus, departureServiceCentreId, stationId, CompanyType, isCod, CountryId).ToListAsync();

            var result = invoices.ToList();
            var invoicesResult = Mapper.Map<IEnumerable<InvoiceViewDTO>>(result);

            return await Task.FromResult(invoicesResult.ToList()); // Task.FromResult(invoicesResult.OrderByDescending(x => x.DateCreated).ToList());
        }

        //Shipent Monitors
        //Stored Procedure version
        public async Task<List<InvoiceMonitorDTO>> GetShipmentMonitorSetSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter endDate = new SqlParameter("@EndDate", EndDate);


            SqlParameter paymentStatus = new SqlParameter("@PaymentStatus", DBNull.Value);
            //var sc = (serviceCentreIds.Length == 0) ? serviceCentreIds[0] : 0;
            var sc = (serviceCentreIds.Length == 1) ? serviceCentreIds[0] : 0;
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc);
            SqlParameter stationId = new SqlParameter("@StationId", (int)accountFilterCriteria.StationId);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            SqlParameter[] param = new SqlParameter[]
            {
                iscancelled,
                startDate,
                endDate,
                paymentStatus,
                departureServiceCentreId,
                stationId,
                CountryId
            };

            var listCreated = new List<InvoiceMonitorDTO>();

            try
            {
                listCreated = await _GIGLSContextForView.Database.SqlQuery<InvoiceMonitorDTO>("NewSp2 " +
                  "@IsCancelled, @StartDate, @EndDate, @PaymentStatus, @DepartureServiceCentreId, @StationId, @CountryId",
                  param)
                  .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult(listCreated);
        }


        //Shipent Monitors
        //Stored Procedure version
        public async Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentMonitorSetSP_NotGrouped(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter endDate = new SqlParameter("@EndDate", EndDate);


            SqlParameter paymentStatus = new SqlParameter("@PaymentStatus", DBNull.Value);
            var sc = (serviceCentreIds.Length == 1) ? serviceCentreIds[0] : 0;
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc);
            SqlParameter stationId = new SqlParameter("@StationId", (int)accountFilterCriteria.StationId);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            SqlParameter[] param = new SqlParameter[]
            {
                iscancelled,
                startDate,
                endDate,
                paymentStatus,
                departureServiceCentreId,
                stationId,
                CountryId
            };

            var listCreated = new List<InvoiceViewDTOUNGROUPED>();

            try
            {
                listCreated = await _GIGLSContextForView.Database.SqlQuery<InvoiceViewDTOUNGROUPED>("NewSp2 " +
                  "@IsCancelled, @StartDate, @EndDate, @PaymentStatus, @DepartureServiceCentreId, @StationId, @CountryId",
                  param)
                  .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult(listCreated);
        }

        //Shipent Monitors
        //Stored Procedure version
        public async Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentMonitorSetSP_NotGroupedx(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter endDate = new SqlParameter("@EndDate", EndDate);

            SqlParameter paymentStatus = new SqlParameter("@PaymentStatus", DBNull.Value);//accountFilterCriteria.PaymentStatus
            var sc = (serviceCentreIds.Length == 1) ? serviceCentreIds[0] : 0;
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc); //serviceCentreIds[0]
            SqlParameter stationId = new SqlParameter("@StationId", (int)accountFilterCriteria.StationId);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            SqlParameter[] param = new SqlParameter[]
            {
                iscancelled,
                startDate,
                endDate,
                paymentStatus,
                departureServiceCentreId,
                stationId,
                CountryId
            };

            var listCreated = new List<InvoiceViewDTOUNGROUPED>();

            try
            {
                listCreated = await _GIGLSContextForView.Database.SqlQuery<InvoiceViewDTOUNGROUPED>("NewSp_Expected2 " +
                  "@IsCancelled, @StartDate, @EndDate, @PaymentStatus, @DepartureServiceCentreId, @StationId, @CountryId",
                  param)
                  .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(listCreated);
        }


        //Shipent Monitors Expected
        //Stored Procedure version
        //var salesPeople = await context.Database.SqlQuery<SalesPerson>("AllSalesPeople").ToListAsync();
        public async Task<List<InvoiceMonitorDTO>> GetShipmentMonitorSetSPExpected(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter endDate = new SqlParameter("@EndDate", EndDate);


            SqlParameter paymentStatus = new SqlParameter("@PaymentStatus", DBNull.Value);

            var sc = (serviceCentreIds.Length == 1) ? serviceCentreIds[0] : 0;
            //var sc = (serviceCentreIds.Length == 0) ? 0 : serviceCentreIds;  //we need contain function to handle list of service centre fixing -- bug
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc);
            SqlParameter stationId = new SqlParameter("@StationId", (int)accountFilterCriteria.StationId);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            SqlParameter[] param = new SqlParameter[]
            {
                iscancelled,
                startDate,
                endDate,
                paymentStatus,
                departureServiceCentreId,
                stationId,
                CountryId
            };

            var listCreated = new List<InvoiceMonitorDTO>();

            try
            {
                listCreated = await _GIGLSContextForView.Database.SqlQuery<InvoiceMonitorDTO>("NewSp_Expected2 " +
                  "@IsCancelled, @StartDate, @EndDate, @PaymentStatus, @DepartureServiceCentreId, @StationId, @CountryId",
                  param)
                  .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult(listCreated);
        }

        public async Task<List<InvoiceMonitorDTO>> GetShipmentWaitingForCollection(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter endDate = new SqlParameter("@EndDate", EndDate);

            var sc = (serviceCentreIds.Length == 1) ? serviceCentreIds[0] : 0;
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc);
            SqlParameter stationId = new SqlParameter("@StationId", (int)accountFilterCriteria.StationId);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            SqlParameter[] param = new SqlParameter[]
            {
                iscancelled,
                startDate,
                endDate,
                departureServiceCentreId,
                stationId,
                CountryId
            };

            var listCreated = new List<InvoiceMonitorDTO>();

            try
            {
                listCreated = await _GIGLSContextForView.Database.SqlQuery<InvoiceMonitorDTO>("ShipmentWaitingForCollection " +
                  "@IsCancelled, @StartDate, @EndDate, @DepartureServiceCentreId, @StationId, @CountryId",
                  param)
                  .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult(listCreated);
        }

        public async Task<List<InvoiceViewDTOUNGROUPED>> GetShipmentWaitingForCollection_NotGrouped(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter endDate = new SqlParameter("@EndDate", EndDate);

            var sc = (serviceCentreIds.Length == 1) ? serviceCentreIds[0] : 0;
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc); //serviceCentreIds[0]
            SqlParameter stationId = new SqlParameter("@StationId", (int)accountFilterCriteria.StationId);
            SqlParameter CountryId = new SqlParameter("@CountryId", (int)accountFilterCriteria.CountryId);

            SqlParameter[] param = new SqlParameter[]
            {
                iscancelled,
                startDate,
                endDate,
                departureServiceCentreId,
                stationId,
                CountryId
            };

            var listCreated = new List<InvoiceViewDTOUNGROUPED>();

            try
            {
                listCreated = await _GIGLSContextForView.Database.SqlQuery<InvoiceViewDTOUNGROUPED>("ShipmentWaitingForCollection " +
                  "@IsCancelled, @StartDate, @EndDate,@DepartureServiceCentreId, @StationId, @CountryId",
                  param)
                  .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.FromResult(listCreated);
        }

        public async Task<List<InvoiceViewDTO>> GetInvoicesFromViewWithDeliveryTimeAsyncFromSP(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            var queryDate = accountFilterCriteria.getStartDateAndEndDate();

            //declare parameters for the stored procedure
            SqlParameter iscancelled = new SqlParameter("@IsCancelled", (object)accountFilterCriteria.IsCancelled ?? DBNull.Value);
            SqlParameter startDate = new SqlParameter("@StartDate", queryDate.Item1);
            SqlParameter endDate = new SqlParameter("@EndDate", queryDate.Item2);

            SqlParameter paymentStatus = new SqlParameter("@PaymentStatus", Convert.ToInt32(accountFilterCriteria.PaymentStatus));//accountFilterCriteria.PaymentStatus

            var sc = (serviceCentreIds.Length > 0) ? serviceCentreIds[0] : 0;
            //SqlParameter paymentMethod = new SqlParameter("@paymentMethod", accountFilterCriteria.p);
            SqlParameter departureServiceCentreId = new SqlParameter("@DepartureServiceCentreId", sc); //serviceCentreIds[0]

            SqlParameter stationId = new SqlParameter("@StationId", accountFilterCriteria.StationId);
            SqlParameter CompanyType = new SqlParameter("@CompanyType", (object)accountFilterCriteria.CompanyType ?? DBNull.Value);
            SqlParameter isCod = new SqlParameter("@IsCashOnDelivery", (object)accountFilterCriteria.IsCashOnDelivery ?? DBNull.Value);

            var invoices =
           await _GIGLSContextForView.Database.SqlQuery<InvoiceViewDTO>("NewInvoiceViewSPWithDeliveryTime @IsCancelled, @StartDate, @EndDate, @PaymentStatus, @DepartureServiceCentreId, @StationId, @CompanyType, @IsCashOnDelivery",
                iscancelled, startDate, endDate, paymentStatus, departureServiceCentreId, stationId, CompanyType, isCod).ToListAsync();

            var result = invoices.ToList();
            var invoicesResult = Mapper.Map<IEnumerable<InvoiceViewDTO>>(result);

            return await Task.FromResult(invoicesResult.ToList()); // Task.FromResult(invoicesResult.OrderByDescending(x => x.DateCreated).ToList());
        }

        public Task<List<InvoiceViewDTO>> GetInvoicesFromViewAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //filter by service center of the login user
            var invoices = _GIGLSContextForView.InvoiceView.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                invoices = invoices.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            //filter by cancelled shipments
            invoices = invoices.Where(s => s.IsCancelled == false);

            //get startDate and endDate
            var queryDate = accountFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            invoices = invoices.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            //payment status
            if (accountFilterCriteria.PaymentStatus.HasValue)
            {
                invoices = invoices.Where(x => x.PaymentStatus.Equals(accountFilterCriteria.PaymentStatus));
            }

            //Company Type
            if (!string.IsNullOrEmpty(accountFilterCriteria.CompanyType))
            {
                invoices = invoices.Where(x => x.CompanyType.Equals(accountFilterCriteria.CompanyType));
            }

            //service center
            if (accountFilterCriteria.ServiceCentreId > 0)
            {
                invoices = invoices.Where(s => accountFilterCriteria.ServiceCentreId == s.DepartureServiceCentreId);
            }

            //station
            if (accountFilterCriteria.StationId > 0)
            {
                //get the service centres in that station
                invoices = invoices.Where(s => s.DepartureStationId == accountFilterCriteria.StationId);
            }

            //IsCashOnDelivery
            if (accountFilterCriteria.IsCashOnDelivery == true)
            {
                invoices = invoices.Where(s => s.CashOnDeliveryAmount > 0);
            }

            var result = invoices.ToList();
            var invoicesResult = Mapper.Map<IEnumerable<InvoiceViewDTO>>(result);

            return Task.FromResult(invoicesResult.OrderByDescending(x => x.DateCreated).ToList());
        }

        public IQueryable<InvoiceArchiveView> GetAllFromInvoiceView()
        {
            var invoices = _GIGLSContextForView.InvoiceArchiveView.AsQueryable();
            return invoices;
        }

        public IQueryable<InvoiceArchiveView> GetAllFromInvoiceViewFromDateRange(string startdate, string enddate)
        {
            var invoices = _GIGLSContextForView.InvoiceArchiveView.AsQueryable();
            return invoices;
        }

        public IQueryable<InvoiceView> GetAllFromInvoiceAndShipments()
        {
            // filter by cancelled shipments
            var shipments = Context.Shipment_Archive.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false);

            var result = (from s in shipments
                          join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                          select new InvoiceView
                          {
                              PaymentStatus = i.PaymentStatus,
                              IsShipmentCollected = i.IsShipmentCollected,
                              GrandTotal = i.Amount,
                              Waybill = s.Waybill,
                              DepartureServiceCentreId = s.DepartureServiceCentreId,
                              DestinationServiceCentreId = s.DestinationServiceCentreId,
                              CompanyType = s.CompanyType,
                              IsInternational = s.IsInternational,
                              DateCreated = s.DateCreated,
                              DeliveryOptionId = s.DeliveryOptionId,
                              DepositStatus = s.DepositStatus,
                              PaymentMethod = i.PaymentMethod,
                              CashOnDeliveryAmount = s.CashOnDeliveryAmount,
                              ReprintCounterStatus = s.ReprintCounterStatus,
                              ShipmentScanStatus = s.ShipmentScanStatus,
                              IsGrouped = s.IsGrouped,
                              DepartureCountryId = s.DepartureCountryId,
                              DestinationCountryId = s.DestinationCountryId,
                              IsFromMobile = s.IsFromMobile,
                              PickupOptions = s.PickupOptions,
                              ReceiverAddress = s.ReceiverAddress
                          });
            return result;
        }

        public IQueryable<InvoiceView> GetAllFromInvoiceAndShipments(ShipmentCollectionFilterCriteria filterCriteria)
        {
            // get startDate and endDate
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            int[] testingServiceCentre = { 314, 315, 332, 338, 339, 340 };

            // filter by cancelled shipments, country and exclude all our testing service money from the list
            var shipments = Context.Shipment_Archive.AsQueryable()
                .Where(s => s.IsCancelled == false && s.IsDeleted == false && s.DateCreated >= startDate && s.DateCreated < endDate && s.DepartureCountryId == filterCriteria.CountryId
                && !testingServiceCentre.Contains(s.DepartureServiceCentreId) && !testingServiceCentre.Contains(s.DestinationServiceCentreId));


            var result = (from s in shipments
                          join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                          select new InvoiceView
                          {
                              PaymentStatus = i.PaymentStatus,
                              IsShipmentCollected = i.IsShipmentCollected,
                              GrandTotal = i.Amount,
                              Waybill = s.Waybill,
                              DepartureServiceCentreId = s.DepartureServiceCentreId,
                              DestinationServiceCentreId = s.DestinationServiceCentreId,
                              CompanyType = s.CompanyType,
                              IsInternational = s.IsInternational,
                              DateCreated = s.DateCreated,
                              DeliveryOptionId = s.DeliveryOptionId,
                              DepositStatus = s.DepositStatus,
                              PaymentMethod = i.PaymentMethod,
                              CashOnDeliveryAmount = s.CashOnDeliveryAmount,
                              ReprintCounterStatus = s.ReprintCounterStatus,
                              ShipmentScanStatus = s.ShipmentScanStatus,
                              IsGrouped = s.IsGrouped,
                              DepartureCountryId = s.DepartureCountryId,
                              DestinationCountryId = s.DestinationCountryId,
                              PickupOptions = s.PickupOptions,
                              ApproximateItemsWeight = s.ApproximateItemsWeight,
                              CustomerCode = s.CustomerCode,
                              IsCashOnDelivery = s.IsCashOnDelivery,
                              CODAmount = s.CashOnDeliveryAmount
                          });
            return result;
        }

        public IQueryable<InvoiceView> GetAllInvoiceShipments()
        {
            var shipments = Context.Shipment_Archive.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false);

            var result = (from s in shipments
                          join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                          join dept in Context.ServiceCentre on s.DepartureServiceCentreId equals dept.ServiceCentreId
                          join dest in Context.ServiceCentre on s.DestinationServiceCentreId equals dest.ServiceCentreId
                          join option in Context.DeliveryOption on s.DeliveryOptionId equals option.DeliveryOptionId
                          select new InvoiceView
                          {
                              ShipmentId = s.ShipmentId,
                              Waybill = s.Waybill,
                              CustomerId = s.CustomerId,
                              CustomerType = s.CustomerType,
                              CustomerCode = s.CustomerCode,
                              DateCreated = s.DateCreated,
                              DateModified = s.DateModified,
                              DeliveryOptionId = s.DeliveryOptionId,
                              DeliveryOptionCode = option.Code,
                              Description = option.Description,
                              DepartureServiceCentreId = s.DepartureServiceCentreId,
                              DepartureServiceCentreCode = dept.Code,
                              DepartureServiceCentreName = dept.Name,
                              DestinationServiceCentreId = s.DestinationServiceCentreId,
                              DestinationServiceCentreCode = dest.Code,
                              DestinationServiceCentreName = dest.Name,
                              PaymentStatus = i.PaymentStatus,
                              ReceiverAddress = s.ReceiverAddress,
                              ReceiverCity = s.ReceiverCity,
                              ReceiverCountry = s.ReceiverCountry,
                              ReceiverEmail = s.ReceiverEmail,
                              ReceiverName = s.ReceiverName,
                              ReceiverPhoneNumber = s.ReceiverPhoneNumber,
                              ReceiverState = s.ReceiverState,
                              SealNumber = s.SealNumber,
                              UserId = s.UserId,
                              Value = s.Value,
                              GrandTotal = i.Amount,
                              DiscountValue = s.DiscountValue,
                              CompanyType = s.CompanyType,
                              IsShipmentCollected = i.IsShipmentCollected,
                              IsInternational = s.IsInternational,
                              DepositStatus = s.DepositStatus,
                              PaymentMethod = i.PaymentMethod,
                              PickupOptions = s.PickupOptions,
                              IsCancelled = s.IsCancelled,
                              DepartureCountryId = s.DepartureCountryId,
                              DestinationCountryId = s.DestinationCountryId,
                              isInternalShipment = s.isInternalShipment
                          });
            return result;
        }

        public IQueryable<InvoiceView> GetCustomerTransactions()
        {
            var shipments = Context.Shipment_Archive.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false);

            var result = (from s in shipments
                          join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                          join dept in Context.ServiceCentre on s.DepartureServiceCentreId equals dept.ServiceCentreId
                          join dest in Context.ServiceCentre on s.DestinationServiceCentreId equals dest.ServiceCentreId
                          select new InvoiceView
                          {
                              Waybill = s.Waybill,
                              CustomerCode = s.CustomerCode,
                              DateCreated = s.DateCreated,
                              ReceiverEmail = s.ReceiverEmail,
                              ReceiverName = s.ReceiverName,
                              PickupOptions = s.PickupOptions,
                              Description = s.Description,
                              GrandTotal = i.Amount,
                              DepartureServiceCentreName = dept.Name,
                              DestinationServiceCentreName = dest.Name,
                              DepartureCountryId = s.DepartureCountryId,
                              DestinationCountryId = s.DestinationCountryId
                          });
            return result;
        }

        public IQueryable<InvoiceView> GetCustomerInvoices()
        {
            var shipments = Context.Shipment_Archive.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false);

            var result = (from s in shipments
                          join i in Context.Invoice_Archive on s.Waybill equals i.Waybill
                          select new InvoiceView
                          {
                              Waybill = s.Waybill,
                              CustomerCode = s.CustomerCode,
                              DateCreated = s.DateCreated,
                              Amount = i.Amount,
                              InvoiceNo = i.InvoiceNo,
                              PaymentMethod = i.PaymentMethod,
                              PaymentStatus = i.PaymentStatus,
                              DepartureCountryId = s.DepartureCountryId,
                              DestinationCountryId = s.DestinationCountryId
                          });
            return result;
        }

        public IQueryable<Report_AllTimeSalesByCountry> GetAllTimeSalesByCountry()
        {
            var result = _GIGLSContextForView.Report_AllTimeSalesByCountry.AsQueryable();
            return result;
        }

        public IQueryable<Report_BusiestRoute> GetBusiestRoute()
        {
            var result = _GIGLSContextForView.Report_BusiestRoute.AsQueryable();
            return result;
        }

        public IQueryable<Report_CustomerRevenue> GetCustomerRevenue()
        {
            var result = _GIGLSContextForView.Report_CustomerRevenue.AsQueryable();
            return result;
        }

        public IQueryable<Report_MostShippedItemByWeight> GetMostShippedItemByWeight()
        {
            var result = _GIGLSContextForView.Report_MostShippedItemByWeight.AsQueryable();
            return result;
        }

        public IQueryable<Report_RevenuePerServiceCentre> GetRevenuePerServiceCentre()
        {
            var result = _GIGLSContextForView.Report_RevenuePerServiceCentre.AsQueryable();
            return result;
        }

        public IQueryable<Report_TotalServiceCentreByState> GetTotalServiceCentreByState()
        {
            var result = _GIGLSContextForView.Report_TotalServiceCentreByState.AsQueryable();
            return result;
        }

        public IQueryable<Report_TotalOrdersDelivered> GetTotalOrdersDelivered()
        {
            var result = _GIGLSContextForView.Report_TotalOrdersDelivered.AsQueryable();
            return result;
        }

        public async Task<List<object>> SalesPerServiceCenter(List<InvoiceArchiveView> invoice)
        {
            IEnumerable<object> result = (from s in invoice
                                          group s by s.DepartureServiceCentreId into sales
                                          select new
                                          {
                                              ServiceCenterId = sales.Key,
                                              Name = Context.ServiceCentre.Where(c => c.ServiceCentreId == sales.Key).Select(x => new ServiceCentreDTO
                                              {
                                                  Code = x.Code,
                                                  Name = x.Name
                                              }).FirstOrDefault(),

                                              Revenue = sales.Sum(p => p.GrandTotal)
                                          }).OrderByDescending(i => i.Revenue).ToList();

            var results = result.ToList();
            return await Task.FromResult(results);
        }

        public async Task<List<object>> MostShippedItemsByWeight(List<InvoiceView> invoice)
        {
            IEnumerable<object> result = (from s in invoice
                                          group s by s.ApproximateItemsWeight into weight
                                          select new
                                          {
                                              Weight = weight.Key,
                                              WeightCount = weight.Count()
                                          }).OrderByDescending(i => i.WeightCount).Take(5).ToList();

            var results = result.ToList();
            return await Task.FromResult(results);
        }

        public async Task<List<object>> CountOfCustomers(List<InvoiceView> invoice)
        {
            IEnumerable<object> result = (from s in invoice
                                          group s by s.CustomerCode into customers
                                          select new
                                          {
                                              CustomerCode = customers.Key,
                                              Count = customers.Count()
                                          }).OrderByDescending(i => i.Count).ToList();

            var results = result.ToList();
            return await Task.FromResult(results);
        }

        public async Task<int> GetCountOfMonthlyOrDailyShipmentCreated(DashboardFilterCriteria dashboardFilterCriteria, ShipmentReportType shipmentReportType)
        {
            try
            {
                int result = 0;

                DateTime dt = DateTime.Today;
                var beginningDate = dt;
                var endingDate = DateTime.Now;

                if (shipmentReportType == ShipmentReportType.Monthly)
                {
                    beginningDate = new DateTime(dt.Year, dt.Month, 1);
                }
                else if (shipmentReportType == ShipmentReportType.Daily)
                {
                    beginningDate = dt;
                }

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", beginningDate);
                SqlParameter endDate = new SqlParameter("@EndDate", endingDate);
                SqlParameter countryId = new SqlParameter("@CountryId", dashboardFilterCriteria.ActiveCountryId);

                SqlParameter[] param = new SqlParameter[]
                {
                    startDate,
                    endDate,
                    countryId
                };

                var summary = await Context.Database.SqlQuery<int>("MonthlyShipmentCreated " +
                   "@StartDate, @EndDate, @CountryId",
                   param).FirstOrDefaultAsync();

                if (summary != null)
                {
                    result = summary;
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
