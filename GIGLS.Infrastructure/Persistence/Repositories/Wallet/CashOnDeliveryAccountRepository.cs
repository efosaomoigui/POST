using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using AutoMapper;
using System.Linq;
using GIGLS.Core.Enums;
using System.Data.SqlClient;
using GIGLS.Core.DTO.Account;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Configuration;
using GIGLS.Core.Domain.BankSettlement;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
{
    public class CashOnDeliveryAccountRepository : Repository<CashOnDeliveryAccount, GIGLSContext>, ICashOnDeliveryAccountRepository
    {
        private GIGLSContext _context;

        public CashOnDeliveryAccountRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync()
        {
            try
            {
                var codAccounts = _context.CashOnDeliveryAccount.Include("Wallet").ToList();
                codAccounts = codAccounts.OrderByDescending(x => x.DateCreated).ToList();
                var codAccountsDto = Mapper.Map<IEnumerable<CashOnDeliveryAccountDTO>>(codAccounts);
                return Task.FromResult(codAccountsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync(CODStatus codStatus)
        {
            try
            {
                var codAccounts = _context.CashOnDeliveryAccount.Include("Wallet").Where(s => s.CODStatus == codStatus).ToList();
                var codAccountsDto = Mapper.Map<IEnumerable<CashOnDeliveryAccountDTO>>(codAccounts);
                return Task.FromResult(codAccountsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Shipment> GetShipmentByWaybill(string waybill)
        {
            var shipment = _context.Shipment.Where(x => x.Waybill == waybill).FirstOrDefault();
            return Task.FromResult(shipment); 
        }

        public Task<IQueryable<CodPayOutList>> GetPaidOutCODListsAsQueryable() 
        {
            var shipment = _context.CodPayOutList.AsQueryable();
            return Task.FromResult(shipment);
        }

        public async Task<IEnumerable<NewInvoiceViewDTO>> GetCODCustomersWhoNeedPayOut() 
        {

            string CODpayoutDateStart = ConfigurationManager.AppSettings["CODpayoutDateStart"];
            var CODpayoutDateStartValue = DateTime.MinValue;

            bool success = DateTime.TryParse(CODpayoutDateStart, out CODpayoutDateStartValue);

            var cType = nameof(CompanyType.Ecommerce);

            try
            {

                SqlParameter CompanyType = new SqlParameter("@CompanyType", cType);
                SqlParameter isCod = new SqlParameter("@IsCashOnDelivery", (object)true ?? DBNull.Value);
                SqlParameter isCODPaidOut = new SqlParameter("@IsCODPaidOut", (object)false ?? DBNull.Value);
                SqlParameter codDateStart = new SqlParameter("@CodDateStart", CODpayoutDateStartValue);

                var result = await _context.Database.SqlQuery<NewInvoiceViewDTO>("CODCustomerWhoNeedPayOutSP @CompanyType, @IsCashOnDelivery, @IsCODPaidOut, @CodDateStart", CompanyType, isCod, isCODPaidOut, codDateStart).ToListAsync();

                var newCodForProcessing = result.ToList(); 
                //var newCodForProcessingResult = Mapper.Map<IEnumerable<InvoiceView2DTO>>(newCodForProcessing); 

                return await Task.FromResult(newCodForProcessing);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<CashOnDeliveryAccount> GetCODCollectedAsQueryable()  
        {
                var codAccounts = _context.CashOnDeliveryAccount.AsQueryable();
                return codAccounts;
        }

    }

    public class CashOnDeliveryRegisterAccountRepository : Repository<CashOnDeliveryRegisterAccount, GIGLSContext>, ICashOnDeliveryRegisterAccountRepository
    {
        private GIGLSContext _context;

        public CashOnDeliveryRegisterAccountRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<CashOnDeliveryRegisterAccount> GetCODAsQueryable()
        {
            var codAccounts = _context.CashOnDeliveryRegisterAccount.AsQueryable();
            return codAccounts;
        }

    }


    public class DemurrageRegisterAccountRepository : Repository<DemurrageRegisterAccount, GIGLSContext>, IDemurrageRegisterAccountRepository
    {
        private GIGLSContext _context;

        public DemurrageRegisterAccountRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DemurrageRegisterAccount> GetDemurrageAsQueryable()
        {
            var DemurrageAccounts = _context.DemurrageRegisterAccount.AsQueryable();
            return DemurrageAccounts;
        } 

    }
}
