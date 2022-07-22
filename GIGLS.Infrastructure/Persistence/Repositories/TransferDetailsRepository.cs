using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
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

                var transferDetails = GetCellulantTransferDetailsAsQuerable();

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                transferDetails = transferDetails.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.CrAccount == crAccount);

                var transferDetailsDto =  GetListOfTransferDetails(transferDetails);
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
                var transferDetails = GetCellulantTransferDetailsAsQuerable();

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

                var transferDetails = GetCellulantTransferDetailsAsQuerable();

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
                var transferDetails = GetCellulantTransferDetailsAsQuerable();

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

                var transferDetails = GetCellulantTransferDetailsAsQuerable();

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
                var transferDetails = GetCellulantTransferDetailsAsQuerable();

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
                              join s in Context.ServiceCentre on t.CrAccount equals s.CrAccount
                              where t.CrAccount == s.CrAccount
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
                                  TransactionStatus = t.TransactionStatus,
                                  ServiceCenterName = s.Name
                              };
            return Task.FromResult(transferDto.ToList());
        }

        #region Azapay

        public Task<List<TransferDetailsDTO>> GetAzapayTransferDetails(BaseFilterCriteria filterCriteria)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filterCriteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var transferDetails = GetAzapayTransferDetailsAsQuerable();

                if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                }

                transferDetails = transferDetails.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.ProcessingPartner == ProcessingPartnerType.Azapay);

                var transferDetailsDto = GetListOfAzapayTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<TransferDetailsDTO>> GetAzapayTransferDetailsByAccountNumber(string accountNumber)
        {
            try
            {
                var transferDetails = GetAzapayTransferDetailsAsQuerable();

                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    accountNumber = accountNumber.Trim().ToLower();
                    transferDetails = transferDetails.Where(x => x.TimedAccNo.ToLower().Equals(accountNumber));
                }

                var transferDetailsDto = GetListOfAzapayTransferDetails(transferDetails);
                return transferDetailsDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task<List<TransferDetailsDTO>> GetListOfAzapayTransferDetails(IQueryable<TransferDetails> transferDetails)
        {
            var transferDto = from t in transferDetails
                              orderby t.DateCreated descending
                              select new TransferDetailsDTO
                              {
                                  SessionId = t.SessionId,
                                  Amount = t.Amount,
                                  CrAccount = t.CrAccount,
                                  BankCode = t.BankCode,
                                  BankName = t.SenderBank,
                                  CrAccountName = t.CrAccountName,
                                  OriginatorAccountNumber = t.TimedAccNo,
                                  OriginatorName = t.SenderName,
                                  PaymentReference = t.RefId,
                                  CreatedAt = t.CreatedAt,
                                  DateCreated = t.DateCreated,
                                  ResponseCode = t.ResponseCode,
                                  TransactionStatus = t.TransactionStatus,
                                  ServiceCenterName = "GIGL"
                              };
            return Task.FromResult(transferDto.ToList());
        }
        #endregion


        #region Private Methods
        private IQueryable<TransferDetails> GetCellulantTransferDetailsAsQuerable()
        {
            return _context.TransferDetails.AsQueryable().Where(x => x.ProcessingPartner == ProcessingPartnerType.Cellulant);
        }

        private IQueryable<TransferDetails> GetAzapayTransferDetailsAsQuerable()
        {
            return _context.TransferDetails.AsQueryable().Where(x => x.ProcessingPartner == ProcessingPartnerType.Azapay);
        }
        #endregion
    }
}
