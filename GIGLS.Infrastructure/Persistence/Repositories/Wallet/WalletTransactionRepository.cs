using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.Entity;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletTransactionRepository : Repository<WalletTransaction, GIGLSContext>, IWalletTransactionRepository
    {
        private GIGLSContext _context;

        public WalletTransactionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionAsync(int[] serviceCentreIds)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = _context.WalletTransactions.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var walletTransactions = walletTransactionContext.Include(s => s.ServiceCentre).ToList();
            var walletTransactionDTO = Mapper.Map<IEnumerable<WalletTransactionDTO>>(walletTransactions);
            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionDateAsync (int[] serviceCentreIds, ShipmentCollectionFilterCriteria dateFilter)
        {
            //get startDate and endDate
            var queryDate = dateFilter.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var walletTransactionContext = _context.WalletTransactions.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = walletTransactionContext.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var walletTransactions = walletTransactionContext.Include(s => s.ServiceCentre).ToList();
            var walletTransactionDTO = Mapper.Map<IEnumerable<WalletTransactionDTO>>(walletTransactions);
            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }

        public Task<List<WalletTransactionDTO>> GetWalletTransactionCreditAsync(int[] serviceCentreIds, AccountFilterCriteria accountFilterCriteria)
        {
            //filter by service center
            var walletTransactionContext = _context.WalletTransactions.Where(x => x.CreditDebitType == CreditDebitType.Credit).AsQueryable();
            if (serviceCentreIds.Length > 0)
            {
                walletTransactionContext = _context.WalletTransactions.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var queryDate = accountFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            walletTransactionContext = walletTransactionContext.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate);

            List<WalletTransactionDTO> walletTransactionDTO = (from w in walletTransactionContext
                                                             select new WalletTransactionDTO()
                                                             {
                                                                 WalletTransactionId = w.WalletTransactionId,
                                                                 DateOfEntry = w.DateOfEntry,
                                                                 Amount = w.Amount,
                                                                 CreditDebitType = w.CreditDebitType,
                                                                 Description = w.Description,
                                                                 IsDeferred = w.IsDeferred,
                                                                 PaymentType = w.PaymentType,
                                                                 UserId = w.UserId,
                                                                 ServiceCentreId = w.ServiceCentreId,
                                                                 ServiceCentre = Context.ServiceCentre.Where(s => s.ServiceCentreId == w.ServiceCentreId).Select(x => new ServiceCentreDTO {
                                                                     Code = x.Code,
                                                                     Name = x.Name
                                                                 }).FirstOrDefault(),
                                                                 WalletId = w.WalletId,
                                                                 Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO {
                                                                     Balance = x.Balance,
                                                                     CompanyType = x.CompanyType,
                                                                     CustomerCode = x.CustomerCode,
                                                                     CustomerId = x.CustomerId,
                                                                     CustomerType = x.CustomerType,
                                                                     WalletNumber = x.WalletNumber,
                                                                     //CustomerName = Context.Company.Where(s => s.CustomerCode == x.CustomerCode).FirstOrDefault().Name
                                                                 }).FirstOrDefault()
                                                             }).OrderByDescending(s => s.DateOfEntry).ToList();

            return Task.FromResult(walletTransactionDTO.OrderByDescending(s => s.DateOfEntry).ToList());
        }
    }
}