using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using GIGLS.Core.Domain.Wallet;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletPaymentLogRepository : Repository<WalletPaymentLog, GIGLSContext>, IWalletPaymentLogRepository
    {
        private GIGLSContextForView _GIGLSContextForView;

        public WalletPaymentLogRepository(GIGLSContext context) : base(context)
        {
            _GIGLSContextForView = new GIGLSContextForView();
        }

        public Task<List<WalletPaymentLogDTO>> GetWalletPaymentLogs()
        {
            try
            {
                var walletPaymentLogs = Context.WalletPaymentLog;

                var walletPaymentLogsDTO = from w in walletPaymentLogs
                                           select new WalletPaymentLogDTO
                                           {
                                               WalletPaymentLogId = w.WalletPaymentLogId,
                                               WalletId = w.WalletId,
                                               Wallet = Context.Wallets.Where(s => s.WalletId == w.WalletId).Select(x => new WalletDTO
                                               {
                                                   WalletId = x.WalletId,
                                                   Balance = x.Balance,
                                                   CustomerCode = x.CustomerCode,
                                                   WalletNumber = x.WalletNumber,
                                                   CustomerType = x.CustomerType,
                                               }).FirstOrDefault(),
                                               Amount = w.Amount,
                                               TransactionStatus = w.TransactionStatus,
                                               UserId = w.UserId,
                                               IsWalletCredited = w.IsWalletCredited,
                                               DateCreated = w.DateCreated,
                                               DateModified = w.DateModified
                                           };
                return Task.FromResult(walletPaymentLogsDTO.OrderBy(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<Task<List<WalletPaymentLogView>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto, string walletNumber)
        {
            try
            {
                var walletPaymentLogDto = new List<WalletPaymentLogDTO>();
                var pageNumber = filterOptionsDto?.page ?? FilterOptionsDto.DefaultPageNumber;
                var pageSize = filterOptionsDto?.count ?? FilterOptionsDto.DefaultCount;

                //build query
                var queryable = GetAllFromWalletPaymentLogView();

                if(walletNumber != null)
                {
                    queryable = queryable.Where(x => x.WalletNumber == walletNumber);
                }

                var filter = filterOptionsDto?.filter ?? null;
                var filterValue = filterOptionsDto?.filterValue ?? null;
                if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(filterValue))
                {
                    var caseObject = new WalletPaymentLogView();
                    switch (filter)
                    {
                        case nameof(caseObject.WalletNumber):
                            queryable = queryable.Where(s => s.WalletNumber.Contains(filterValue));
                            break;
                        case nameof(caseObject.Reference):
                            queryable = queryable.Where(s => s.Reference.Contains(filterValue));
                            break;
                        case nameof(caseObject.Name):
                            queryable = queryable.Where(s => s.Name.Contains(filterValue) 
                            || s.FirstName.Contains(filterValue));
                            break;
                        case nameof(caseObject.FirstName):
                            queryable = queryable.Where(s => s.Name.Contains(filterValue)
                            || s.FirstName.Contains(filterValue));
                            break;
                    }
                }
                
                //populate the count variable
                var totalCount = queryable.Count();

                //page the query
                queryable = queryable.OrderByDescending(x => x.DateCreated);
                var result = queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
                return new Tuple<Task<List<WalletPaymentLogView>>, int>(Task.FromResult(result.ToList()), totalCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<Task<List<WalletPaymentLogDTO>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto, int walletId)
        {
            try
            {
                var pageNumber = filterOptionsDto?.page ?? FilterOptionsDto.DefaultPageNumber;
                var pageSize = filterOptionsDto?.count ?? FilterOptionsDto.DefaultCount;

                //build query
                var queryable = Context.WalletPaymentLog.AsQueryable();

                if (walletId > 0)
                {
                    queryable = queryable.Where(x => x.WalletId == walletId);
                }

                var filter = filterOptionsDto?.filter ?? null;
                var filterValue = filterOptionsDto?.filterValue ?? null;
                if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(filterValue))
                {
                    var caseObject = new WalletPaymentLog();
                    switch (filter)
                    {
                        case nameof(caseObject.Reference):
                            queryable = queryable.Where(s => s.Reference.Contains(filterValue));
                            break;
                    }
                }

                //populate the count variable
                var totalCount = queryable.Count();

                //page the query
                queryable = queryable.OrderByDescending(x => x.DateCreated);
                var result = queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

                var walletPaymentLogDto = Mapper.Map<List<WalletPaymentLogDTO>>(result);
                
                return new Tuple<Task<List<WalletPaymentLogDTO>>, int>(Task.FromResult(walletPaymentLogDto), totalCount);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IQueryable<WalletPaymentLogView> GetAllFromWalletPaymentLogView()
        {
            var walletPaymentLogViews = _GIGLSContextForView.WalletPaymentLogView.AsQueryable();
            return walletPaymentLogViews;
        }

        public Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogView(DateFilterCriteria filterCriteria)
        {
            //get startDate and endDate
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var walletPaymentLogViews = _GIGLSContextForView.WalletPaymentLogView.AsQueryable().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate
                                    && s.UserActiveCountryId == filterCriteria.CountryId);

            return Task.FromResult(walletPaymentLogViews.OrderByDescending(x => x.DateCreated).ToList());
            
        }

        public Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogViewBySearchParameter(string searchItem)
        {
            var walletPaymentLogViews = _GIGLSContextForView.WalletPaymentLogView.AsQueryable();

            if (searchItem != null)
            {
                searchItem = searchItem.ToLower();
                walletPaymentLogViews = walletPaymentLogViews.Where(x => x.PhoneNumber == searchItem || x.Reference == searchItem
                                        || x.CustomerCode == searchItem || x.Email == searchItem);

            }
            else
            {
                throw new GenericException("Kindly enter a search parameter");
            }


            return Task.FromResult(walletPaymentLogViews.OrderByDescending(x => x.DateCreated).ToList());

        }
    }

}
