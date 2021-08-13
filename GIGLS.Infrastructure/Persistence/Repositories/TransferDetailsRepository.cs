using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class TransferDetailsRepository : Repository<TransferDetails, GIGLSContext>, ITransferDetailsRepository
    {
        private GIGLSContext _context;
        public TransferDetailsRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria filterCriteria, string crAccount)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var transferDetails = _context.TransferDetails.AsQueryable();

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                transferDetails = transferDetails.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.CrAccount == crAccount);

                var transferDetailsDto = GetListOfTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber, string crAccount)
        {
            try
            {
                var transferDetails = _context.TransferDetails.AsQueryable();

                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    accountNumber = accountNumber.Trim().ToLower();
                    transferDetails = transferDetails.Where(x => x.OriginatorAccountNumber.ToLower().Equals(accountNumber) && x.CrAccount == crAccount);
                }

                var transferDetailsDto = GetListOfTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria filterCriteria)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var transferDetails = _context.TransferDetails.AsQueryable();

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                transferDetails = transferDetails.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

                var transferDetailsDto = GetListOfTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber)
        {
            try
            {
                var transferDetails = _context.TransferDetails.AsQueryable();

                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    accountNumber = accountNumber.Trim().ToLower();
                    transferDetails = transferDetails.Where(x => x.OriginatorAccountNumber.ToLower().Equals(accountNumber));
                }

                var transferDetailsDto = GetListOfTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetails(BaseFilterCriteria filterCriteria, List<string> crAccounts)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var transferDetails = _context.TransferDetails.AsQueryable();

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                transferDetails = transferDetails.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && crAccounts.Contains(s.CrAccount));

                var transferDetailsDto = GetListOfTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<TransferDetailsDTO>> GetTransferDetailsByAccountNumber(string accountNumber, List<string> crAccounts)
        {
            try
            {
                var transferDetails = _context.TransferDetails.AsQueryable();

                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    accountNumber = accountNumber.Trim().ToLower();
                    transferDetails = transferDetails.Where(x => x.OriginatorAccountNumber.ToLower().Equals(accountNumber) && crAccounts.Contains(x.CrAccount));
                }

                var transferDetailsDto = GetListOfTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task<List<TransferDetailsDTO>> GetListOfTransferDetails(IQueryable<TransferDetails> transferDetails)
        {
            var transferDto = from t in transferDetails
                              orderby t.DateCreated descending
                              select new TransferDetailsDTO
                              {
                                  SessionId = t.SessionId,
                                  Amount = t.Amount,
                                  CrAccount = t.CrAccount,
                                  BankCode = t.BankCode,
                                  BankName = t.BankName,
                                  CrAccountName = t.CrAccountName,
                                  OriginatorAccountNumber = t.OriginatorAccountNumber,
                                  OriginatorName = t.OriginatorName,
                                  PaymentReference = t.PaymentReference,
                                  CreatedAt = t.CreatedAt,
                                  DateCreated = t.DateCreated,
                                  ResponseCode = t.ResponseCode,
                                  TransactionStatus = t.TransactionStatus
                              };
            return Task.FromResult(transferDto.ToList());
        }
    }
}
