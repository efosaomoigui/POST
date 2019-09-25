
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.BankSettlement
{
    public class BankProcessingOrderCodesRepository : Repository<BankProcessingOrderCodes, GIGLSContext>, IBankProcessingOrderCodesRepository
    {
        public BankProcessingOrderCodesRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCode(DepositType type)
        {
            var processingorderCodes = Context.BankProcessingOrderCodes.AsQueryable();
            processingorderCodes = processingorderCodes.Where(s => s.DepositType == type);
            var processingcodes = from processingorderCode in processingorderCodes
                                  select new BankProcessingOrderCodesDTO
                                  {
                                      CodeId = processingorderCode.CodeId,
                                      Code = processingorderCode.Code,
                                      DateAndTimeOfDeposit = processingorderCode.DateAndTimeOfDeposit,
                                      DepositType = processingorderCode.DepositType,
                                      TotalAmount = processingorderCode.TotalAmount,
                                      UserId = processingorderCode.UserId,
                                      Status = processingorderCode.Status,
                                      ServiceCenter = processingorderCode.ServiceCenter,
                                      ScName = processingorderCode.ScName,
                                      FullName = processingorderCode.FullName,
                                      VerifiedBy = processingorderCode.VerifiedBy
                                  };

            return Task.FromResult(processingcodes.OrderByDescending(s => s.DateAndTimeOfDeposit).ToList());
        }
        //gets the deposits for the month by default once the page loads
        public Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeForDefaultDate(DepositType type)
        {
           
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var currentDate = new DateTime(date.Year, date.Month, DateTime.Now.Day);
                        
            var processingorderCodes = Context.BankProcessingOrderCodes.Where(s => s.DateCreated >= firstDayOfMonth && s.DateCreated < currentDate).AsQueryable();                                    

            processingorderCodes = processingorderCodes.Where(s => s.DepositType == type);
            var processingcodes = from processingorderCode in processingorderCodes
                                  select new BankProcessingOrderCodesDTO
                                  {
                                      CodeId = processingorderCode.CodeId,
                                      Code = processingorderCode.Code,
                                      DateAndTimeOfDeposit = processingorderCode.DateAndTimeOfDeposit,
                                      DepositType = processingorderCode.DepositType,
                                      TotalAmount = processingorderCode.TotalAmount,
                                      UserId = processingorderCode.UserId,
                                      Status = processingorderCode.Status,
                                      ServiceCenter = processingorderCode.ServiceCenter,
                                      ScName = processingorderCode.ScName,
                                      FullName = processingorderCode.FullName,
                                      VerifiedBy = processingorderCode.VerifiedBy
                                  };

            return Task.FromResult(processingcodes.OrderByDescending(s => s.DateAndTimeOfDeposit).ToList());
        } 

        //gets the deposits for the date range
        public Task<List<BankProcessingOrderCodesDTO>> GetBankOrderProcessingCodeByDate(DepositType type, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;                                  

            var processingorderCodes = Context.BankProcessingOrderCodes.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate && s.DepositType == type).AsQueryable();                       

            //processingorderCodes = processingorderCodes.Where(s => s.DepositType == type);
            var processingcodes = from processingorderCode in processingorderCodes
                                  select new BankProcessingOrderCodesDTO
                                  {
                                      CodeId = processingorderCode.CodeId,
                                      Code = processingorderCode.Code,
                                      DateAndTimeOfDeposit = processingorderCode.DateAndTimeOfDeposit,
                                      DepositType = processingorderCode.DepositType,
                                      TotalAmount = processingorderCode.TotalAmount,
                                      UserId = processingorderCode.UserId,
                                      Status = processingorderCode.Status,
                                      ServiceCenter = processingorderCode.ServiceCenter,
                                      ScName = processingorderCode.ScName,
                                      FullName = processingorderCode.FullName,
                                      VerifiedBy = processingorderCode.VerifiedBy
                                  };

            return Task.FromResult(processingcodes.OrderByDescending(s => s.DateAndTimeOfDeposit).ToList());
        }

        public IQueryable<BankProcessingOrderCodesDTO> GetBankOrderProcessingCodeAsQueryable()
        {
            var processingorderCodes = Context.BankProcessingOrderCodes.AsQueryable();
            var processingcodes = from processingorderCode in processingorderCodes
                                  select new BankProcessingOrderCodesDTO
                                  {
                                      CodeId = processingorderCode.CodeId,
                                      Code = processingorderCode.Code,
                                      DateAndTimeOfDeposit = processingorderCode.DateAndTimeOfDeposit,
                                      DepositType = processingorderCode.DepositType,
                                      TotalAmount = processingorderCode.TotalAmount,
                                      UserId = processingorderCode.UserId,
                                      Status = processingorderCode.Status,
                                      ServiceCenter = processingorderCode.ServiceCenter,
                                  };
            return processingcodes.OrderByDescending(s => s.DateAndTimeOfDeposit);
        }

        public Task<List<BankProcessingOrderCodesDTO>> GetProcessingOrderCodebyRefCode(string refcode)
        {
            var processingorderCodes = Context.BankProcessingOrderCodes.AsQueryable();
            processingorderCodes = processingorderCodes.Where(s => s.Code == refcode);

            var codorder = from processingorderCode in processingorderCodes
                           select new BankProcessingOrderCodesDTO
                           {
                               CodeId = processingorderCode.CodeId,
                               Code = processingorderCode.Code,
                               DateAndTimeOfDeposit = processingorderCode.DateAndTimeOfDeposit,
                               DepositType = processingorderCode.DepositType,
                               TotalAmount = processingorderCode.TotalAmount,
                               UserId = processingorderCode.UserId,
                               Status = processingorderCode.Status,
                               ServiceCenter = processingorderCode.ServiceCenter,
                           };
            return Task.FromResult(codorder.ToList());
        }

        public Task<Shipment> GetShipmentByWaybill(string waybill)
        {
            var shipment = Context.Shipment.Where(x => x.Waybill == waybill).FirstOrDefault();
            return Task.FromResult(shipment);
        }
    }
}
