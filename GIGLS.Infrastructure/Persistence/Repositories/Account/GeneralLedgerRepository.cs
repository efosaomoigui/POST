using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
using System;
using System.Data.Entity;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class GeneralLedgerRepository : Repository<GeneralLedger, GIGLSContext>, IGeneralLedgerRepository
    {
        public GeneralLedgerRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(int[] serviceCentreIds)
        {
            //filter by service center
            var generalLedgerContext = Context.GeneralLedger.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                generalLedgerContext = Context.GeneralLedger.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }
            ////

            var generalLedgers = generalLedgerContext.Include(s => s.ServiceCentre).ToList();
            var generalLedgerDto = Mapper.Map<IEnumerable<GeneralLedgerDTO>>(generalLedgers);
            return Task.FromResult(generalLedgerDto.OrderByDescending(s => s.DateOfEntry).ToList());
        }
        
        public Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(CreditDebitType creditDebitType, int[] serviceCentreIds)
        {
            //filter by service center
            var generalLedgerContext = Context.GeneralLedger.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                generalLedgerContext = Context.GeneralLedger.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }
            ////

            var generalLedgers = generalLedgerContext.Include(s => s.ServiceCentre).Where(s => s.CreditDebitType == creditDebitType).ToList();
            var generalLedgerDto = Mapper.Map<IEnumerable<GeneralLedgerDTO>>(generalLedgers);
            return Task.FromResult(generalLedgerDto.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<GeneralLedgerDTO>> GetGeneralLedgersAsync(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds)
        {
            DateTime StartDate = accountFilterCriteria.StartDate.GetValueOrDefault().Date;
            DateTime EndDate = accountFilterCriteria.EndDate?.Date ?? StartDate;

            //filter by service center
            var generalLedgerContext = Context.GeneralLedger.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                generalLedgerContext = Context.GeneralLedger.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }
            ////

            IQueryable<GeneralLedger> generalLedgers = generalLedgerContext.Include("ServiceCentre");

            //get startDate and endDate
            var queryDate = accountFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            generalLedgers = generalLedgers.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            //filter by country Id
            if (accountFilterCriteria.CountryId > 0)
            {
                generalLedgers = generalLedgers.Where(s => s.CountryId == accountFilterCriteria.CountryId);
            }

            if (accountFilterCriteria.creditDebitType.HasValue)
            {
                generalLedgers = generalLedgers.Where(x => x.CreditDebitType == accountFilterCriteria.creditDebitType);
            }
            
            if (accountFilterCriteria.PaymentType.HasValue)
            {
                generalLedgers = generalLedgers.Where(x => x.PaymentType == accountFilterCriteria.PaymentType);
            }

            if (accountFilterCriteria.ServiceCentreId > 0)
            {
                generalLedgers = generalLedgers.Where(x => x.ServiceCentreId == accountFilterCriteria.ServiceCentreId);
            }

            if (accountFilterCriteria.StationId > 0)
            {
                var serviceCentre = Context.ServiceCentre.Where(y => y.StationId == accountFilterCriteria.StationId);
                generalLedgers = generalLedgers.Where(y => serviceCentre.Any(x => y.ServiceCentreId == x.ServiceCentreId));
            }

            if (accountFilterCriteria.StateId > 0)
            {
                var station = Context.Station.Where(s => s.StateId == accountFilterCriteria.StateId);
                var serviceCentre = Context.ServiceCentre.Where(w => station.Any(x => w.StationId == x.StationId));
                generalLedgers = generalLedgers.Where(y => serviceCentre.Any(x => y.ServiceCentreId == x.ServiceCentreId));
            }

            if (accountFilterCriteria.IsDeferred.HasValue)
            {
                generalLedgers = generalLedgers.Where(x => x.IsDeferred == accountFilterCriteria.IsDeferred);
            }

            if (accountFilterCriteria.PaymentServiceType.HasValue)
            {
                generalLedgers = generalLedgers.Where(x => x.PaymentServiceType == accountFilterCriteria.PaymentServiceType);
                //generalLedgers = generalLedgers.Where(x => x.Description.Contains("demurrage"));
            }

            var result = generalLedgers.ToList();
            var generalLedgersResult = Mapper.Map<IEnumerable<GeneralLedgerDTO>>(result);    
            return Task.FromResult(generalLedgersResult.OrderByDescending(x => x.DateOfEntry).ToList());
        }
    }
}
