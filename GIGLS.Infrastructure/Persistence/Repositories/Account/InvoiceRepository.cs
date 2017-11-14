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
        public Task<IEnumerable<InvoiceDTO>> GetInvoicesAsync()
        {
            var invoices = Context.Invoice.ToList();
            var invoiceDto = Mapper.Map<IEnumerable<InvoiceDTO>>(invoices);
            return Task.FromResult(invoiceDto);
        }

        public Task<List<InvoiceDTO>> GetInvoicesAsync(AccountFilterCriteria accountFilterCriteria)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            IQueryable<Invoice> invoices = Context.Invoice;

            //If No Date Supply
            if (!accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                var Today = DateTime.Today;
                var nextDay = DateTime.Today.AddDays(1).Date;
                invoices = invoices.Where(x => x.DateCreated >= Today && x.DateCreated < nextDay);
            }

            if (accountFilterCriteria.StartDate.HasValue && accountFilterCriteria.EndDate.HasValue)
            {
                if (accountFilterCriteria.StartDate.Equals(accountFilterCriteria.EndDate))
                {
                    var nextDay = DateTime.Today.AddDays(1).Date;
                    invoices = invoices.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
                }
                else
                {
                    var dayAfterEndDate = EndDate.AddDays(1).Date;
                    invoices = invoices.Where(x => x.DateCreated >= StartDate && x.DateCreated < dayAfterEndDate);
                }
            }

            if (accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                var nextDay = DateTime.Today.AddDays(1).Date;
                invoices = invoices.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
            }

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
