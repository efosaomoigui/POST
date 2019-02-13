using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.BankSettlement;
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

        public Task<List<BankProcessingOrderForShipmentAndCODDTO>> GetProcessingOrderForShipmentAndCOD()
        {

            var bankProcessingorderforcods = Context.BankProcessingOrderForShipmentAndCOD.AsQueryable();

            var codorder = from bankProcessingorderforcod in bankProcessingorderforcods
                           select new BankProcessingOrderForShipmentAndCODDTO
                           {
                               ProcessingOrderId = bankProcessingorderforcod.ProcessingOrderId,
                               RefCode = bankProcessingorderforcod.RefCode,
                               Waybill = bankProcessingorderforcod.Waybill,
                               ServiceCenter = bankProcessingorderforcod.ServiceCenter,
                               status = bankProcessingorderforcod.status,
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
                                      status = processingorderCode.status,
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
                               status = bankProcessingorderforcod.status,
                           };
            return Task.FromResult(codorder.ToList());
        }


    }
}
