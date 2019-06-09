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

        public async Task<IEnumerable<InvoiceViewDTO>> GetCODCustomersWhoNeedPayOut() 
        {

            var cType = CompanyType.Ecommerce;

            try
            {
                SqlParameter CompanyType = new SqlParameter("@CompanyType", cType.ToString());
                SqlParameter isCod = new SqlParameter("@IsCashOnDelivery", true);
                SqlParameter isCODPaidOut = new SqlParameter("@IsCODPaidOut", false);

                var result = await _context.Database.SqlQuery<InvoiceViewDTO>("CODCustomerWhoNeedPayOutSP @CompanyType, @IsCashOnDelivery, @IsCODPaidOut",
                     CompanyType, isCod, isCODPaidOut).ToListAsync();

                var newCodForProcessing = result.ToList(); 
                var newCodForProcessingResult = Mapper.Map<IEnumerable<InvoiceViewDTO>>(newCodForProcessing); 

                return await Task.FromResult(newCodForProcessingResult.ToList());

            }
            catch (Exception)
            {
                throw;
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
