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

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class InvoiceRepository : Repository<Invoice, GIGLSContext>, IInvoiceRepository
    {
        public InvoiceRepository(GIGLSContext context) : base(context)
        {
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
                serviceCenterWaybills = shipmentContext.Select(s => s.Waybill).ToList();
               invoices = Context.Invoice.Where(s => serviceCenterWaybills.Contains(s.Waybill));
            }
            ////
            else
            {
                invoices = Context.Invoice;
            }

            //If No Date Supply
            if (!accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var nextDay = today.AddDays(1).Date;
                invoices = invoices.Where(x => x.DateCreated >= today && x.DateCreated < nextDay);
            }

            //StartDate has value and EndDate has Value 
            if (accountFilterCriteria.StartDate.HasValue && accountFilterCriteria.EndDate.HasValue)
            {
                if (accountFilterCriteria.StartDate.Equals(accountFilterCriteria.EndDate))
                {
                    var nextDay = ((DateTime) accountFilterCriteria.StartDate).AddDays(1).Date;
                    invoices = invoices.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
                }
                else
                {
                    var dayAfterEndDate = EndDate.AddDays(1).Date;
                    invoices = invoices.Where(x => x.DateCreated >= StartDate && x.DateCreated < dayAfterEndDate);
                }
            }

            //StartDate has value and EndDate has no Value
            if (accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                //var nextDay = ((DateTime)accountFilterCriteria.StartDate).AddDays(1).Date;
                invoices = invoices.Where(x => x.DateCreated >= StartDate && x.DateCreated < today);
            }

            //StartDate has no value and EndDate has Value
            if (accountFilterCriteria.EndDate.HasValue && !accountFilterCriteria.StartDate.HasValue)
            {
                var dayAfterEndDate = EndDate.AddDays(1).Date;
                invoices = invoices.Where(x => x.DateCreated < dayAfterEndDate);
            }

            if(accountFilterCriteria.PaymentStatus.HasValue)
            {
                invoices = invoices.Where(x => x.PaymentStatus.Equals(accountFilterCriteria.PaymentStatus));
            }

            var result = invoices.ToList();
            var invoicesResult = Mapper.Map<IEnumerable<InvoiceDTO>>(result);
            return Task.FromResult(invoicesResult.OrderByDescending(x => x.DateCreated).ToList());
        }        
    }
}
