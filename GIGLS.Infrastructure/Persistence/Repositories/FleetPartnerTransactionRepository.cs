using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class FleetPartnerTransactionRepository : Repository<FleetPartnerTransaction, GIGLSContext>, IFleetPartnerTransactionRepository
    {
        private GIGLSContext _context;
        public FleetPartnerTransactionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerTransaction(string partnercode)
        {
            //To be completed
            var users = _context.Users.Where(x => x.UserChannelCode == partnercode);

            var transactionsDto = from user in users
                                join fleet in _context.Fleet on user.Id equals fleet.EnterprisePartnerId
                                join trans in _context.FleetPartnerTransaction on fleet.RegistrationNumber.ToLower() equals trans.FleetRegistrationNumber.ToLower()
                                select new FleetPartnerTransactionDTO
                                {
                                    CreditDebitType = trans.CreditDebitType,
                                    Amount = trans.Amount,
                                    Description = trans.Description,
                                    DateOfEntry = trans.DateOfEntry
                                };

            return Task.FromResult(transactionsDto.OrderByDescending(x => x.DateOfEntry).ToList());
        }

        public Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerTransactionByDateRange(string partnercode, FleetFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            //To be completed
            var users = _context.Users.Where(x => x.UserChannelCode == partnercode && x.DateCreated >= startDate && x.DateCreated < endDate);

            var transactionsDto = from user in users
                                  join fleet in _context.Fleet on user.Id equals fleet.EnterprisePartnerId
                                  join trans in _context.FleetPartnerTransaction on fleet.RegistrationNumber.ToLower() equals trans.FleetRegistrationNumber.ToLower()
                                  select new FleetPartnerTransactionDTO
                                  {
                                      CreditDebitType = trans.CreditDebitType,
                                      Amount = trans.Amount,
                                      Description = trans.Description,
                                      DateOfEntry = trans.DateOfEntry
                                  };

            return Task.FromResult(transactionsDto.OrderByDescending(x => x.DateOfEntry).ToList());
        }

        public Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerCreditTransaction(string partnercode)
        {
            //To be completed
            var users = _context.Users.Where(x => x.UserChannelCode == partnercode);

            var transactionsDto = from user in users
                                  join fleet in _context.Fleet on user.Id equals fleet.EnterprisePartnerId
                                  join trans in _context.FleetPartnerTransaction on fleet.RegistrationNumber.ToLower() equals trans.FleetRegistrationNumber.ToLower()
                                  where trans.CreditDebitType == CreditDebitType.Credit
                                  select new FleetPartnerTransactionDTO
                                  {
                                      CreditDebitType = trans.CreditDebitType,
                                      Amount = trans.Amount,
                                      Description = trans.Description,
                                      DateOfEntry = trans.DateOfEntry
                                  };

            return Task.FromResult(transactionsDto.OrderByDescending(x => x.DateOfEntry).ToList());
        }

        public Task<List<FleetPartnerTransactionDTO>> GetFleetPartnerDebitTransaction(string partnercode)
        {
            //To be completed
            var users = _context.Users.Where(x => x.UserChannelCode == partnercode);

            var transactionsDto = from user in users
                                  join fleet in _context.Fleet on user.Id equals fleet.EnterprisePartnerId
                                  join trans in _context.FleetPartnerTransaction on fleet.RegistrationNumber.ToLower() equals trans.FleetRegistrationNumber.ToLower()
                                  where trans.CreditDebitType == CreditDebitType.Debit
                                  select new FleetPartnerTransactionDTO
                                  {
                                      CreditDebitType = trans.CreditDebitType,
                                      Amount = trans.Amount,
                                      Description = trans.Description,
                                      DateOfEntry = trans.DateOfEntry
                                  };

            return Task.FromResult(transactionsDto.OrderByDescending(x => x.DateOfEntry).ToList());
        }
    }
}
