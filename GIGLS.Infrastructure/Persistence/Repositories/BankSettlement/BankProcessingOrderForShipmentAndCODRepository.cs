using GIGL.POST.Core.Domain;
using POST.Core.Domain.BankSettlement;
using POST.Core.DTO.BankSettlement;
using POST.Core.DTO.PaymentTransactions;
using POST.Core.Enums;
using POST.Core.IRepositories.BankSettlement;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.BankSettlement
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

        public Task<Shipment> GetShipmentByWaybill(string waybill)
        {
            var shipment = Context.Shipment.Where(x => x.Waybill == waybill).FirstOrDefault();
            return Task.FromResult(shipment);
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

            //DispatchedBy = Context.Users.Where(d => d.Id == p.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
            var processingorders = from processingorderCode in processingordersvalue
                                   select new BankProcessingOrderForShipmentAndCODDTO
                                   {
                                       ProcessingOrderId = processingorderCode.ProcessingOrderId,
                                       RefCode = processingorderCode.RefCode,
                                       ServiceCenterId = processingorderCode.ServiceCenterId,
                                       Status = processingorderCode.Status,
                                       Waybill = processingorderCode.Waybill,
                                       WaybillCreated = Context.Shipment.Where(c => c.Waybill == processingorderCode.Waybill).Select(x => x.DateCreated).FirstOrDefault(),
                                       GrandTotal = processingorderCode.GrandTotal,
                                       CODAmount = processingorderCode.CODAmount ?? 0,
                                       ServiceCenter = processingorderCode.ServiceCenter,
                                       DemurrageAmount = processingorderCode.DemurrageAmount??0,
                                       VerifiedBy = processingorderCode.VerifiedBy
                                   };
            var result = processingorders.ToList();
            return Task.FromResult(result);
        }

        public Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetAllWaybillsForBankProcessingOrdersV2(DepositType type, string refcode, int [] serviceCenters)
        {
            var processingordersvalue = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable().Where(s => s.DepositType == type && s.RefCode == refcode);

            //if(serviceCenters.Length > 0)
            //{
            //    processingordersvalue = processingordersvalue.Where(s => serviceCenters.Contains(s.ServiceCenterId));
            //}

            var processingorders = from processingorderCode in processingordersvalue
                                   select new BankProcessingOrderForShipmentAndCODDTO
                                   {
                                       ProcessingOrderId = processingorderCode.ProcessingOrderId,
                                       RefCode = processingorderCode.RefCode,
                                       ServiceCenterId = processingorderCode.ServiceCenterId,
                                       Status = processingorderCode.Status,
                                       Waybill = processingorderCode.Waybill,
                                       WaybillCreated = Context.Shipment.Where(c => c.Waybill == processingorderCode.Waybill).Select(x => x.DateCreated).FirstOrDefault(),
                                       GrandTotal = processingorderCode.GrandTotal,
                                       CODAmount = processingorderCode.CODAmount ?? 0,
                                       ServiceCenter = processingorderCode.ServiceCenter,
                                       DemurrageAmount = processingorderCode.DemurrageAmount ?? 0,
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
