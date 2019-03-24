using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.BankSettlement
{
    public class BankProcessingOrderForShipmentAndCODRepository : Repository<BankProcessingOrderForShipmentAndCOD, GIGLSContext>, IBankProcessingOrderForShipmentAndCODRepository
    {
        public BankProcessingOrderForShipmentAndCODRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCOD(DepositType type)
        {

            var bankProcessingorderforcods = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable();
            bankProcessingorderforcods = bankProcessingorderforcods.Where(s => s.DepositType == type);

            var codorder = from bankProcessingorderforcod in bankProcessingorderforcods
                           select new BankProcessingOrderForShipmentAndCODDTO
                           {
                               ProcessingOrderId = bankProcessingorderforcod.ProcessingOrderId,
                               RefCode = bankProcessingorderforcod.RefCode,
                               Waybill = bankProcessingorderforcod.Waybill,
                               ServiceCenter = bankProcessingorderforcod.ServiceCenter,
                               Status = bankProcessingorderforcod.Status,
                               VerifiedBy = bankProcessingorderforcod.VerifiedBy
                           };
            return Task.FromResult(codorder.ToList());
        }

        public Task<List<BankProcessingOrderCodesDTO>> GetProcessingOrderCode()
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

            return Task.FromResult(processingcodes.ToList());
        }

        public Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCODByRefCode(string refcode)
        {
            var bankProcessingorderforcods = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable();
            bankProcessingorderforcods = bankProcessingorderforcods.Where(s => s.RefCode == refcode);

            var codorder = from bankProcessingorderforcod in bankProcessingorderforcods
                           select new BankProcessingOrderForShipmentAndCODDTO
                           {
                               ProcessingOrderId = bankProcessingorderforcod.ProcessingOrderId,
                               RefCode = bankProcessingorderforcod.RefCode,
                               Waybill = bankProcessingorderforcod.Waybill,
                               ServiceCenter = bankProcessingorderforcod.ServiceCenter,
                               Status = bankProcessingorderforcod.Status,
                           };
            return Task.FromResult(codorder.ToList());
        }

        public Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetAllWaybillsForBankProcessingOrders(DepositType type)
        {
            var processingordersvalue = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable();
            processingordersvalue = processingordersvalue.Where(s => s.DepositType == type);

            var processingorders = from processingorderCode in processingordersvalue
                                   select new BankProcessingOrderForShipmentAndCODDTO
                                   {
                                       ProcessingOrderId = processingorderCode.ProcessingOrderId,
                                       RefCode = processingorderCode.RefCode,
                                       ServiceCenterId = processingorderCode.ServiceCenterId,
                                       Status = processingorderCode.Status,
                                       Waybill = processingorderCode.Waybill,
                                       GrandTotal = processingorderCode.GrandTotal,
                                       CODAmount = processingorderCode.CODAmount ?? 0,
                                       ServiceCenter = processingorderCode.ServiceCenter,
                                       DemurrageAmount = processingorderCode.DemurrageAmount??0,
                                       VerifiedBy = processingorderCode.VerifiedBy
                                   };
            var result = processingorders.ToList();
            return Task.FromResult(result);
        }

        public Task<IQueryable<BankProcessingOrderForShipmentAndCOD>> GetAllWaybillsForBankProcessingOrdersAsQueryable(DepositType type)
        {
            var processingordersvalue = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable();
            processingordersvalue = processingordersvalue.Where(s => s.DepositType == type);

            var result = processingordersvalue;
            return Task.FromResult(result);
        }

        //public Task<List<BankProcessingOrderForShipmentAndCOD>> GetAllWaybillsForBankProcessingOrdersForCOD()
        //{
        //    var processingordersvalue = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable();
        //    var processingorders = from processingorderCode in processingordersvalue
        //                           select new BankProcessingOrderForShipmentAndCOD
        //                           {
        //                               ProcessingOrderId = processingorderCode.ProcessingOrderId,
        //                               RefCode = processingorderCode.RefCode,
        //                               ServiceCenterId = processingorderCode.ServiceCenterId,
        //                               status = processingorderCode.status,
        //                               ServiceCenter = processingorderCode.ServiceCenter,
        //                           };
        //    return Task.FromResult(processingorders.ToList());
        //}


    }
}
