using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GIGLS.CORE.DTO.Report;
using System;
using GIGLS.Core.View;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class InvoiceRepository : Repository<Invoice, GIGLSContext>, IInvoiceRepository
    {
        private GIGLSContextForView _GIGLSContextForView;

        public InvoiceRepository(GIGLSContext context) : base(context)
        {
            _GIGLSContextForView = new GIGLSContextForView();
        }

        public Task<IEnumerable<InvoiceDTO>> GetInvoicesAsync(int[] serviceCentreIds)
        {
            //filter by service center using general ledger waybill
            var shipmentContext = Context.Shipment.AsQueryable();
            var serviceCenterWaybills = new List<string>();
            IQueryable<Invoice> invoices = new List<Invoice>().AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                shipmentContext = Context.Shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));

                //filter by cancelled shipments
                shipmentContext = shipmentContext.Where(s => s.IsCancelled == false);

                serviceCenterWaybills = shipmentContext.Select(s => s.Waybill).ToList();
                invoices = Context.Invoice.Where(s => serviceCenterWaybills.Contains(s.Waybill));
            }
            ////
            else
            {
                invoices = Context.Invoice;
            }


            var invoiceDto = Mapper.Map<IEnumerable<InvoiceDTO>>(invoices.ToList());

            return Task.FromResult(invoiceDto);
        }

        public Task<List<InvoiceDTO>> GetInvoicesAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //filter by service center using general ledger waybill
            var shipmentContext = Context.Shipment.AsQueryable();
            var serviceCenterWaybills = new List<string>();
            IQueryable<Invoice> invoices = new List<Invoice>().AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                shipmentContext = Context.Shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));

                //filter by cancelled shipments
                shipmentContext = shipmentContext.Where(s => s.IsCancelled == false);

                serviceCenterWaybills = shipmentContext.Select(s => s.Waybill).ToList();
                invoices = Context.Invoice.Where(s => serviceCenterWaybills.Contains(s.Waybill));
            }
            ////
            else
            {
                invoices = Context.Invoice;
            }

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

            //service center
            if (accountFilterCriteria.ServiceCentreId > 0)
            {
                shipmentContext = Context.Shipment.Where(s => accountFilterCriteria.ServiceCentreId == s.DepartureServiceCentreId);
                serviceCenterWaybills = shipmentContext.Select(s => s.Waybill).ToList();
                invoices = invoices.Where(s => serviceCenterWaybills.Contains(s.Waybill));
            }

            //station
            if (accountFilterCriteria.StationId > 0)
            {
                //get the service centres in that station
                var serviceCentres = Context.ServiceCentre.Where(s => s.StationId == accountFilterCriteria.StationId).
                    Select(a => a.ServiceCentreId).ToList();

                shipmentContext = Context.Shipment.Where(s => serviceCentres.Contains(s.DepartureServiceCentreId));
                serviceCenterWaybills = shipmentContext.Select(s => s.Waybill).ToList();
                invoices = invoices.Where(s => serviceCenterWaybills.Contains(s.Waybill));
            }

            var result = invoices.ToList();
            var invoicesResult = Mapper.Map<IEnumerable<InvoiceDTO>>(result);

            return Task.FromResult(invoicesResult.OrderByDescending(x => x.DateCreated).ToList());
        }

        public Task<List<InvoiceView>> GetInvoicesForReminderAsync(double rangeofdays)  
        {

            //get all invoices from InvoiceView Table
            var invoices = _GIGLSContextForView.InvoiceView.AsQueryable();

            //filter by paymentstatus of non paid
            invoices = invoices.Where(s => s.PaymentStatus == 0 && s.CompanyType == "Corporate" );

            //get todays date
            DateTime dayfromtoday = DateTime.Now.AddDays(rangeofdays); 

            //create a new invoiceview object to collect the filtered view
            var allinvoiceintherange = new List<InvoiceView>();

            //filter some more to get all invoices that meets the range of day to duedate
            //
            foreach (var invoice in invoices) 
            {
                var duedate = invoice.DueDate;

                if (dayfromtoday.Date == duedate.Date)
                {
                    allinvoiceintherange.Add(invoice);
                }
            }

            //a filter list of people to send reminder to
            var result = allinvoiceintherange.ToList();

            return Task.FromResult(result);
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


        public IQueryable<InvoiceView> GetAllFromInvoiceView()
        {
            var invoices = _GIGLSContextForView.InvoiceView.AsQueryable();
            return invoices;
        }

        public IQueryable<CustomerView> GetAllFromCustomerView()
        {
            var customers = _GIGLSContextForView.CustomerView.AsQueryable();
            return customers;
        }

        public IQueryable<InvoiceView> GetAllFromInvoiceAndShipments()
        {
            // filter by cancelled shipments
            var shipments = Context.Shipment.AsQueryable().Where(s => s.IsCancelled == false && s.IsDeleted == false);

            var result = (from s in shipments
                          join i in Context.Invoice on s.Waybill equals i.Waybill
                          select new InvoiceView
                          {
                              PaymentStatus = i.PaymentStatus,
                              IsShipmentCollected = i.IsShipmentCollected,
                              Waybill = s.Waybill,
                              DepartureServiceCentreId = s.DepartureServiceCentreId,
                              DestinationServiceCentreId = s.DestinationServiceCentreId,
                              CompanyType = s.CompanyType,
                              IsInternational = s.IsInternational                            
                          });
            return result;
        }
    }
}
